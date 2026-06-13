import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { DentalEndpoint } from '../../../services/dental-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AuthService } from '../../../services/auth.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { DentalImaging } from '../../../models/dental.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';

export const DD_MMM_YYYY_FORMATS = {
  parse: { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput: 'dd-MMM-yyyy',
    monthYearLabel: 'MMM yyyy',
    dateA11yLabel: 'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

class DdMmmYyyyDateAdapter extends NativeDateAdapter {
  override parse(value: string): Date | null {
    if (!value) return null;
    const parts = value.split('-');
    if (parts.length === 3) {
      const day = parseInt(parts[0], 10);
      const month = new Date(`${parts[1]} 1 2000`).getMonth();
      const year = parseInt(parts[2], 10);
      if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
        return new Date(year, month, day);
      }
    }
    return super.parse(value);
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'dd-MMM-yyyy') {
      const day = date.getDate().toString().padStart(2, '0');
      const month = date.toLocaleString('en', { month: 'short' });
      return `${day}-${month}-${date.getFullYear()}`;
    }
    return super.format(date, displayFormat);
  }
}

@Component({
  selector: 'app-dental-imaging-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MatPaginatorModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS }
  ],
  templateUrl: './dental-imaging-report.component.html',
  styleUrl: './dental-imaging-report.component.scss'
})
export class DentalImagingReportComponent implements OnInit {
  private readonly dentalEndpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly authService = inject(AuthService);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;

  readonly imagingRecords = signal<DentalImaging[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);

  searchText = '';
  selectedPatient = '';
  dateFrom!: Date;
  dateTo!: Date;

  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo = signal<Date>(new Date());
  private readonly appliedPatient = signal<string>('');
  readonly appliedSearch = signal<string>('');

  readonly pageSize = 10;
  readonly currentPage = signal<number>(0);
  readonly displayedColumns = ['date', 'patient', 'consultId', 'imagingType', 'findings', 'file'];

  readonly dateFiltered = computed(() => {
    let data = this.imagingRecords();

    const pno = this.appliedPatient();
    if (pno) data = data.filter(record => (record.pno ?? '') === pno);

    const from = new Date(this.appliedDateFrom());
    from.setHours(0, 0, 0, 0);
    const to = new Date(this.appliedDateTo());
    to.setHours(23, 59, 59, 999);

    data = data.filter(record => {
      if (!record.imagingDate) return false;
      const time = new Date(record.imagingDate).getTime();
      return time >= from.getTime() && time <= to.getTime();
    });

    return data.sort((a, b) =>
      new Date(b.imagingDate ?? 0).getTime() - new Date(a.imagingDate ?? 0).getTime()
    );
  });

  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();

    return this.dateFiltered().filter(record =>
      this.resolvePatientName(record).toLowerCase().includes(term) ||
      (record.pno ?? '').toLowerCase().includes(term) ||
      (record.consultId ?? '').toLowerCase().includes(term) ||
      (record.imagingType ?? '').toLowerCase().includes(term) ||
      (record.toothRegion ?? '').toLowerCase().includes(term) ||
      this.resolveFindings(record).toLowerCase().includes(term) ||
      (record.fileName ?? '').toLowerCase().includes(term)
    );
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  readonly totalImages = computed(() => this.filtered().length);
  readonly uniquePatients = computed(() =>
    new Set(this.filtered().map(record => (record.pno ?? '').trim()).filter(Boolean)).size
  );
  readonly recordsWithFiles = computed(() =>
    this.filtered().filter(record => !!(record.filePath?.trim() || record.fileName?.trim())).length
  );
  readonly imagingTypes = computed(() =>
    new Set(this.filtered().map(record => (record.imagingType ?? '').trim()).filter(Boolean)).size
  );

  readonly patientOptions = computed(() => {
    const seen = new Set<string>();
    const options: { pNo: string; label: string }[] = [];

    for (const visit of this.todayVisits()) {
      const pNo = (visit.pNo ?? '').trim();
      if (!pNo || seen.has(pNo)) continue;
      seen.add(pNo);
      options.push({ pNo, label: visit.fullname?.trim() || this.resolvePatientNameByPno(pNo) });
    }

    return options.sort((a, b) => a.label.localeCompare(b.label));
  });

  get userRoles(): string[] {
    return this.authService.currentUser?.roles ?? [];
  }

  get isManagement(): boolean {
    return this.userRoles.map(role => role.toLowerCase()).includes('management');
  }

  get printSidebarLinks(): { label: string; path: string }[] {
    const all = [
      { label: 'Daily Report', path: 'frontdesk-daily-report', dept: 'frontdesk' },
      { label: 'Appointments Report', path: 'frontdesk-appointments-report', dept: 'frontdesk' },
      { label: 'Registration Report', path: 'frontdesk-registration-report', dept: 'frontdesk' },
      { label: 'Laser Sessions', path: 'laser-sessions-report', dept: 'laser' },
      { label: 'Laser Safety', path: 'laser-safety-report', dept: 'laser' },
      { label: 'Laser Utilization', path: 'laser-utilization-report', dept: 'laser' },
      { label: 'Spa Services', path: 'spa-services-report', dept: 'spa' },
      { label: 'Spa Therapists', path: 'spa-therapists-report', dept: 'spa' },
      { label: 'Spa Packages', path: 'spa-packages-report', dept: 'spa' },
      { label: 'Dental Treatments', path: 'dental-treatments-report', dept: 'dental' },
      { label: 'Dental Imaging', path: 'dental-imaging-report', dept: 'dental' },
      { label: 'Dental Cases', path: 'dental-cases-report', dept: 'dental' },
      { label: 'Consultations', path: 'aesthetics-consultations-report', dept: 'aesthetics' },
      { label: 'Procedures', path: 'aesthetics-procedures-report', dept: 'aesthetics' }
    ];

    if (this.isManagement) return all;
    const roles = new Set(this.userRoles.map(role => role.toLowerCase()));
    return all.filter(link => roles.has(link.dept));
  }

  ngOnInit(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.appliedDateFrom.set(new Date(today));
    this.appliedDateTo.set(new Date(today));
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading dental imaging report...');

    const patientsPromise = this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
      .catch(() => [] as HPatient[]);
    const visitsPromise = this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise()
      .catch(() => [] as QryhvisitsForToday[]);

    Promise.all([
      this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().toPromise(),
      patientsPromise,
      visitsPromise
    ]).then(([records, patients, visits]) => {
      this.imagingRecords.set(records ?? []);
      this.patients.set(patients ?? []);
      this.todayVisits.set(visits ?? []);
      this.currentPage.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load dental imaging report.\r\nError: "${error?.message ?? error}"`,
        MessageSeverity.error,
        error
      );
    });
  }

  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.appliedPatient.set(this.selectedPatient);
    this.appliedSearch.set(this.searchText);
    this.currentPage.set(0);
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedPatient = '';
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.runReport();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  printReport(): void {
    window.print();
  }

  exportExcel(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const worksheet = XLSX.utils.json_to_sheet(rows.map(row => ({
      Date: row.date,
      Patient: row.patient,
      'Patient No': row.pno,
      'Consult ID': row.consultId,
      'Imaging Type': row.imagingType,
      'Tooth Region': row.toothRegion,
      Findings: row.findings,
      Impression: row.impression,
      File: row.fileName
    })));
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Dental Imaging');
    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    this.downloadBlob(blob, this.buildFileName('dental-imaging-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Date', 'Patient', 'Patient No', 'Consult ID', 'Imaging Type', 'Tooth Region', 'Findings', 'Impression', 'File'];
    const csvLines = [
      headers.join(','),
      ...rows.map(row => [
        row.date,
        row.patient,
        row.pno,
        row.consultId,
        row.imagingType,
        row.toothRegion,
        row.findings,
        row.impression,
        row.fileName
      ].map(value => this.escapeCsv(value)).join(','))
    ];

    const csvContent = '\uFEFF' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('dental-imaging-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Dental Imaging Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Patient | Consult ID | Imaging Type | Findings'
    ];

    for (const row of rows) {
      const line = `${row.date} | ${row.patient} | ${row.consultId} | ${row.imagingType} | ${row.findings}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const pdfContent = this.buildSimplePdf(lines);
    const blob = new Blob([pdfContent], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('dental-imaging-report', 'pdf'));
  }

  resolvePatientName(record: DentalImaging): string {
    if (record.patientName?.trim()) return record.patientName.trim();
    return this.resolvePatientNameByPno(record.pno);
  }

  resolvePatientInitial(record: DentalImaging): string {
    return this.resolvePatientName(record)[0]?.toUpperCase() ?? '?';
  }

  resolveFindings(record: DentalImaging): string {
    return [record.findings, record.impression, record.recommendations]
      .map(value => value?.trim())
      .filter(Boolean)
      .join('; ') || '-';
  }

  resolveFileName(record: DentalImaging): string {
    return record.fileName?.trim() || record.filePath?.split(/[\\/]/).pop() || '-';
  }

  private getExportRows(): Array<{
    date: string;
    patient: string;
    pno: string;
    consultId: string;
    imagingType: string;
    toothRegion: string;
    findings: string;
    impression: string;
    fileName: string;
  }> {
    return this.filtered().map(record => ({
      date: this.formatDate(record.imagingDate),
      patient: this.resolvePatientName(record),
      pno: record.pno ?? '',
      consultId: record.consultId ?? '',
      imagingType: record.imagingType ?? '-',
      toothRegion: record.toothRegion ?? '-',
      findings: record.findings ?? '-',
      impression: record.impression ?? '-',
      fileName: this.resolveFileName(record)
    }));
  }

  private resolvePatientNameByPno(pNo: string | undefined): string {
    const key = (pNo ?? '').trim();
    if (!key) return '-';
    const patient = this.patients().find(item => item.pno === key);
    if (!patient) return key;
    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() || key;
  }

  private formatDate(value: string | Date | undefined): string {
    if (!value) return '-';
    const date = value instanceof Date ? value : new Date(value);
    if (isNaN(date.getTime())) return '-';
    const day = date.getDate().toString().padStart(2, '0');
    const month = date.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${date.getFullYear()}`;
  }

  private buildFileName(prefix: string, extension: string): string {
    const now = new Date();
    const date = `${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}`;
    const time = `${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}${now.getSeconds().toString().padStart(2, '0')}`;
    return `${prefix}-${date}-${time}.${extension}`;
  }

  private downloadBlob(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    anchor.style.display = 'none';
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
    URL.revokeObjectURL(url);
  }

  private escapeCsv(value: string): string {
    return `"${(value ?? '').replace(/"/g, '""')}"`;
  }

  private escapePdfText(value: string): string {
    return (value ?? '')
      .replaceAll('\\', '\\\\')
      .replaceAll('(', '\\(')
      .replaceAll(')', '\\)');
  }

  private buildSimplePdf(lines: string[]): string {
    const safeLines = lines.slice(0, 220);
    const lineHeight = 14;
    const startY = 800;

    const content = [
      'BT',
      '/F1 10 Tf',
      `40 ${startY} Td`,
      ...safeLines.flatMap((line, index) => {
        const escaped = this.escapePdfText(line);
        return index === 0 ? [`(${escaped}) Tj`] : [`0 -${lineHeight} Td`, `(${escaped}) Tj`];
      }),
      'ET'
    ].join('\n');

    const objects = [
      '1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n',
      '2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n',
      '3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n',
      '4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n',
      `5 0 obj\n<< /Length ${content.length} >>\nstream\n${content}\nendstream\nendobj\n`
    ];

    let pdf = '%PDF-1.4\n';
    const offsets = [0];

    for (const obj of objects) {
      offsets.push(pdf.length);
      pdf += obj;
    }

    const xrefOffset = pdf.length;
    pdf += `xref\n0 ${objects.length + 1}\n`;
    pdf += '0000000000 65535 f \n';

    for (let i = 1; i <= objects.length; i++) {
      pdf += `${offsets[i].toString().padStart(10, '0')} 00000 n \n`;
    }

    pdf += `trailer\n<< /Size ${objects.length + 1} /Root 1 0 R >>\nstartxref\n${xrefOffset}\n%%EOF`;
    return pdf;
  }
}
