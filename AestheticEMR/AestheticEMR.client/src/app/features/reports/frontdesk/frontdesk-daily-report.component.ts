import { Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
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
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../../services/h-retainership-endpoint.service';
import { AuthService } from '../../../services/auth.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HRetainership } from '../../../models/legacy/h-retainership.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';

export const DD_MMM_YYYY_FORMATS = {
  parse:   { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput:          'dd-MMM-yyyy',
    monthYearLabel:     'MMM yyyy',
    dateA11yLabel:      'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

/** Adapter that parses/formats "dd-MMM-yyyy" strings (e.g. 08-May-2026) */
class DdMmmYyyyDateAdapter extends NativeDateAdapter {
  override parse(value: string): Date | null {
    if (!value) return null;
    const parts = value.split('-');
    if (parts.length === 3) {
      const day   = parseInt(parts[0], 10);
      const month = new Date(`${parts[1]} 1 2000`).getMonth();
      const year  = parseInt(parts[2], 10);
      if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
        return new Date(year, month, day);
      }
    }
    return super.parse(value);
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'dd-MMM-yyyy') {
      const d   = date.getDate().toString().padStart(2, '0');
      const m   = date.toLocaleString('en', { month: 'short' });
      const y   = date.getFullYear();
      return `${d}-${m}-${y}`;
    }
    return super.format(date, displayFormat);
  }
}

@Component({
  selector: 'app-frontdesk-daily-report',
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
    { provide: DateAdapter,      useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS }
  ],
  templateUrl: './frontdesk-daily-report.component.html',
  styleUrl: './frontdesk-daily-report.component.scss'
})
export class FrontdeskDailyReportComponent implements OnInit {
  private readonly attendanceEndpoint   = inject(AttendanceEndpoint);
  private readonly patientEndpoint      = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);
  private readonly authService          = inject(AuthService);
  private readonly alertService         = inject(AlertService);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  loadingIndicator = false;

  readonly attendances  = signal<Attendance[]>([]);
  readonly patients     = signal<HPatient[]>([]);
  readonly retainerships = signal<HRetainership[]>([]);
  readonly todayVisits  = signal<QryhvisitsForToday[]>([]);

  // Two-way bound to the filter inputs (UI state)
  searchText      = '';
  selectedPatient = '';   // pNo from attendance dropdown
  dateFrom!: Date;
  dateTo!: Date;

  // Applied signals — only updated when Run Report is clicked
  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo   = signal<Date>(new Date());
  private readonly appliedPatient  = signal<string>('');
  readonly appliedSearch           = signal<string>('');

  readonly pageSize    = 10;
  readonly currentPage = signal<number>(0);

  readonly displayedColumns = ['date', 'patient', 'clinicType', 'company', 'category', 'status'];

  // ---- Computed: attendances matching APPLIED date/patient filters ----
  readonly dateFiltered = computed(() => {
    let data = this.attendances();

    const pno = this.appliedPatient();
    if (pno) data = data.filter(a => (a.pNo ?? '') === pno);

    const from = new Date(this.appliedDateFrom()); from.setHours(0, 0, 0, 0);
    const to   = new Date(this.appliedDateTo());   to.setHours(23, 59, 59, 999);

    data = data.filter(a => {
      if (!a.recDate) return false;
      const t = new Date(a.recDate).getTime();
      return t >= from.getTime() && t <= to.getTime();
    });

    return data.sort((a, b) =>
      new Date(b.recDate ?? 0).getTime() - new Date(a.recDate ?? 0).getTime()
    );
  });

  // ---- Computed: live search applied ON TOP of date-filtered results ----
  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();
    return this.dateFiltered().filter(a =>
      this.resolvePatientName(a.pNo).toLowerCase().includes(term) ||
      (a.pNo        ?? '').toLowerCase().includes(term) ||
      (a.consultId  ?? '').toLowerCase().includes(term) ||
      this.resolveCompany(a.coyname).toLowerCase().includes(term) ||
      (a.clinicType ?? '').toLowerCase().includes(term) ||
      (a.attndStatus ?? '').toLowerCase().includes(term)
    );
  });

  // ---- Current page slice ----
  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  // ---- KPI cards (reflect filtered set) ----
  readonly totalVisits    = computed(() => this.filtered().length);
  readonly todayVisitsKpi = computed(() => this.filtered().filter(a => this.isToday(a.recDate)).length);
  readonly uniquePatients = computed(() =>
    new Set(this.filtered().map(a => (a.pNo ?? '').trim()).filter(Boolean)).size
  );
  readonly thisMonthCount = computed(() => {
    const now = new Date();
    return this.attendances().filter(a => {
      if (!a.recDate) return false;
      const d = new Date(a.recDate);
      return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
    }).length;
  });

  // ---- Attendance "select patient" dropdown (today's visits, all clinics) ----
  readonly patientOptions = computed(() => {
    const seen = new Set<string>();
    const opts: { pNo: string; label: string }[] = [];
    for (const v of this.todayVisits()) {
      if (!v.pNo || seen.has(v.pNo)) continue;
      seen.add(v.pNo);
      opts.push({ pNo: v.pNo, label: v.fullname?.trim() || v.pNo });
    }
    return opts.sort((a, b) => a.label.localeCompare(b.label));
  });

  // ---- Role helpers for print sidebar ----
  get userRoles(): string[] {
    return this.authService.currentUser?.roles ?? [];
  }
  get isManagement(): boolean {
    return this.userRoles.map(r => r.toLowerCase()).includes('management');
  }
  get printSidebarLinks(): { label: string; path: string }[] {
    const all = [
      { label: 'Daily Report',          path: 'frontdesk-daily-report',          dept: 'frontdesk' },
      { label: 'Appointments Report',   path: 'frontdesk-appointments-report',   dept: 'frontdesk' },
      { label: 'Registration Report',   path: 'frontdesk-registration-report',   dept: 'frontdesk' },
      { label: 'Laser Sessions',        path: 'laser-sessions-report',           dept: 'laser' },
      { label: 'Laser Safety',          path: 'laser-safety-report',             dept: 'laser' },
      { label: 'Laser Utilization',     path: 'laser-utilization-report',        dept: 'laser' },
      { label: 'Spa Services',          path: 'spa-services-report',             dept: 'spa' },
      { label: 'Spa Therapists',        path: 'spa-therapists-report',           dept: 'spa' },
      { label: 'Spa Packages',          path: 'spa-packages-report',             dept: 'spa' },
      { label: 'Dental Treatments',     path: 'dental-treatments-report',        dept: 'dental' },
      { label: 'Dental Imaging',        path: 'dental-imaging-report',           dept: 'dental' },
      { label: 'Dental Cases',          path: 'dental-cases-report',             dept: 'dental' },
      { label: 'Consultations',         path: 'aesthetics-consultations-report', dept: 'aesthetics' },
      { label: 'Procedures',            path: 'aesthetics-procedures-report',    dept: 'aesthetics' },
    ];

    if (this.isManagement) return all;
    const roles = new Set(this.userRoles.map(r => r.toLowerCase()));
    return all.filter(l => roles.has(l.dept));
  }

  ngOnInit(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo   = new Date(today);
    this.appliedDateFrom.set(new Date(today));
    this.appliedDateTo.set(new Date(today));
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading daily report...');

    const patientsPromise = this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
      .catch(() => [] as HPatient[]);
    const retainershipsPromise = this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>().toPromise()
      .catch(() => [] as HRetainership[]);
    const visitsPromise = this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise()
      .catch(() => [] as QryhvisitsForToday[]);

    Promise.all([
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      patientsPromise,
      retainershipsPromise,
      visitsPromise
    ]).then(([attendances, patients, retainerships, visits]) => {
      this.attendances.set(attendances ?? []);
      this.patients.set(patients ?? []);
      this.retainerships.set(retainerships ?? []);
      this.todayVisits.set(visits ?? []);
      this.currentPage.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load daily report.\r\nError: "${error?.message ?? error}"`,
        MessageSeverity.error,
        error
      );
    });
  }

  /** Push UI filter values into signals → triggers computed re-evaluation */
  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.appliedPatient.set(this.selectedPatient);
    this.appliedSearch.set(this.searchText);
    this.currentPage.set(0);
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  clearFilters: () => void = (): void => {
    this.searchText      = '';
    this.selectedPatient = '';
    const today = new Date(); today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo   = new Date(today);
    this.runReport();
  };

  printReport(): void { window.print(); }

  // ---- Export ----
  exportExcel(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const data = rows.map(r => ({
      Date: r.date,
      'Consult ID': r.consultId,
      'Patient No': r.pno,
      Patient: r.patient,
      Clinic: r.clinic,
      Company: r.company,
      Category: r.category,
      Status: r.status
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Daily Report');

    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    this.downloadBlob(blob, this.buildFileName('frontdesk-daily-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Date', 'Consult ID', 'Patient No', 'Patient', 'Clinic', 'Company', 'Category', 'Status'];
    const csvLines = [
      headers.join(','),
      ...rows.map(r => [r.date, r.consultId, r.pno, r.patient, r.clinic, r.company, r.category, r.status]
        .map(v => this.escapeCsv(v)).join(','))
    ];
    const csvContent = '﻿' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('frontdesk-daily-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Frontdesk Daily Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Patient | Clinic | Company | Status'
    ];
    for (const r of rows) {
      const line = `${r.date} | ${r.patient} | ${r.clinic} | ${r.company} | ${r.status}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const pdfContent = this.buildSimplePdf(lines);
    const blob = new Blob([pdfContent], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('frontdesk-daily-report', 'pdf'));
  }

  private getExportRows(): Array<{
    date: string; consultId: string; pno: string; patient: string;
    clinic: string; company: string; category: string; status: string;
  }> {
    return this.filtered().map(a => ({
      date: this.formatDate(a.recDate),
      consultId: a.consultId ?? '',
      pno: a.pNo ?? '',
      patient: this.resolvePatientName(a.pNo),
      clinic: a.clinicType ?? '—',
      company: this.resolveCompany(a.coyname),
      category: a.clientCat ?? '—',
      status: a.attndStatus ?? '—'
    }));
  }

  // ---- Resolvers ----
  resolvePatientName(pNo: string | undefined): string {
    const key = (pNo ?? '').trim();
    if (!key) return '—';
    const p = this.patients().find(x => x.pno === key);
    if (!p) return key;
    return `${p.pSurName ?? ''} ${p.pFirstname ?? ''}`.trim() || key;
  }

  resolvePatientInitial(pNo: string | undefined): string {
    return this.resolvePatientName(pNo)[0]?.toUpperCase() ?? '?';
  }

  resolveCompany(coyname: string | undefined): string {
    const key = (coyname ?? '').trim();
    if (!key) return '—';
    const r = this.retainerships().find(x => x.retainId === key);
    return r ? (r.retainName || key) : key;
  }

  // ---- Helpers ----
  private isToday(value?: string | null): boolean {
    if (!value) return false;
    const d = new Date(value);
    if (isNaN(d.getTime())) return false;
    const today = new Date();
    return d.getFullYear() === today.getFullYear()
      && d.getMonth() === today.getMonth()
      && d.getDate() === today.getDate();
  }

  private formatDate(value: string | Date | undefined): string {
    if (!value) return '—';
    const d = value instanceof Date ? value : new Date(value);
    if (isNaN(d.getTime())) return '—';
    const day = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${d.getFullYear()}`;
  }

  private buildFileName(prefix: string, extension: string): string {
    const now = new Date();
    const part = `${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}-${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}${now.getSeconds().toString().padStart(2, '0')}`;
    return `${prefix}-${part}.${extension}`;
  }

  private downloadBlob(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.style.display = 'none';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  }

  private escapeCsv(value: string): string {
    const escaped = (value ?? '').replace(/"/g, '""');
    return `"${escaped}"`;
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
        if (index === 0) {
          return [`(${escaped}) Tj`];
        }
        return [`0 -${lineHeight} Td`, `(${escaped}) Tj`];
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
