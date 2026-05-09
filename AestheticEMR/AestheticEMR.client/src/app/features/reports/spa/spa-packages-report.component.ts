import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AuthService } from '../../../services/auth.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { User } from '../../../models/user.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

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
      const d = date.getDate().toString().padStart(2, '0');
      const m = date.toLocaleString('en', { month: 'short' });
      const y = date.getFullYear();
      return `${d}-${m}-${y}`;
    }
    return super.format(date, displayFormat);
  }
}

@Component({
  selector: 'app-spa-packages-report',
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
  templateUrl: './spa-packages-report.component.html',
  styleUrl: './spa-packages-report.component.scss'
})
export class SpaPackagesReportComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);
  private readonly authService = inject(AuthService);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;

  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly patients = signal<AestheticPatient[]>([]);
  readonly users = signal<User[]>([]);

  searchText = '';
  dateFrom!: Date;
  dateTo!: Date;

  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo = signal<Date>(new Date());
  readonly appliedSearch = signal<string>('');

  readonly pageSize = 10;
  readonly currentPage = signal<number>(0);

  readonly displayedColumns = ['date', 'patient', 'therapist', 'package', 'service', 'notes'];

  readonly dateFiltered = computed(() => {
    const from = new Date(this.appliedDateFrom());
    from.setHours(0, 0, 0, 0);
    const to = new Date(this.appliedDateTo());
    to.setHours(23, 59, 59, 999);

    return this.consultations()
      .filter(c => c.consultationDate && c.services?.trim())
      .filter(c => {
        const t = new Date(c.consultationDate!).getTime();
        return t >= from.getTime() && t <= to.getTime();
      })
      .sort((a, b) =>
        new Date(b.consultationDate ?? 0).getTime() - new Date(a.consultationDate ?? 0).getTime());
  });

  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();

    return this.dateFiltered().filter(c =>
      this.resolvePatientName(c).toLowerCase().includes(term) ||
      this.resolveProviderName(c.provider).toLowerCase().includes(term) ||
      (c.services ?? '').toLowerCase().includes(term) ||
      (c.indication ?? '').toLowerCase().includes(term) ||
      (c.pNo ?? '').toLowerCase().includes(term) ||
      (c.consultId ?? '').toLowerCase().includes(term)
    );
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  readonly totalBookings = computed(() => this.filtered().length);
  readonly totalPackages = computed(() => {
    return new Set(this.filtered().map(c => c.services?.trim()).filter(Boolean)).size;
  });
  readonly mostPopular = computed(() => {
    const map = new Map<string, number>();
    for (const row of this.filtered()) {
      const pkg = row.services?.trim();
      if (!pkg) continue;
      map.set(pkg, (map.get(pkg) ?? 0) + 1);
    }
    if (!map.size) return '—';
    return [...map.entries()].sort((a, b) => b[1] - a[1])[0][0];
  });
  readonly thisMonthBookings = computed(() => {
    const now = new Date();
    return this.consultations().filter(c => {
      if (!c.consultationDate || !c.services?.trim()) return false;
      const d = new Date(c.consultationDate);
      return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
    }).length;
  });

  get userRoles(): string[] {
    return this.authService.currentUser?.roles ?? [];
  }

  get isManagement(): boolean {
    return this.userRoles.map(r => r.toLowerCase()).includes('management');
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
    const roles = new Set(this.userRoles.map(r => r.toLowerCase()));
    return all.filter(x => roles.has(x.dept));
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
    this.alertService.startLoadingMessage('Loading packages data...');

    const usersPromise = this.accountEndpoint.getUsersEndpoint<User[]>().toPromise()
      .catch((error: { status?: number }) => {
        if (error?.status === 401 || error?.status === 403) {
          return [] as User[];
        }

        throw error;
      });

    Promise.all([
      this.endpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      usersPromise
    ]).then(([consultations, patients, users]) => {
      this.consultations.set(consultations ?? []);
      this.patients.set(patients ?? []);
      this.users.set(users ?? []);
      this.currentPage.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load packages report.\r\nError: "${error?.message ?? error}"`,
        MessageSeverity.error,
        error
      );
    });
  }

  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.currentPage.set(0);
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  clearFilters(): void {
    this.searchText = '';
    this.appliedSearch.set('');
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.runReport();
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

    const data = rows.map(r => ({
      Date: r.date,
      Patient: r.patient,
      'PNO/Consult ID': r.pnoOrConsultId,
      Therapist: r.therapist,
      Package: r.packageName,
      Service: r.service,
      Notes: r.notes
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Spa Packages Report');

    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    this.downloadBlob(blob, this.buildFileName('spa-packages-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Date', 'Patient', 'PNO/Consult ID', 'Therapist', 'Package', 'Service', 'Notes'];
    const csvLines = [
      headers.join(','),
      ...rows.map(r => [r.date, r.patient, r.pnoOrConsultId, r.therapist, r.packageName, r.service, r.notes]
        .map(v => this.escapeCsv(v)).join(','))
    ];

    const csvContent = '\uFEFF' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('spa-packages-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Spa Packages Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Patient | Therapist | Package | Service | Notes'
    ];

    for (const r of rows) {
      const line = `${r.date} | ${r.patient} | ${r.therapist} | ${r.packageName} | ${r.service} | ${r.notes}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const pdfContent = this.buildSimplePdf(lines);
    const blob = new Blob([pdfContent], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('spa-packages-report', 'pdf'));
  }

  private getExportRows(): Array<{
    date: string;
    patient: string;
    pnoOrConsultId: string;
    therapist: string;
    packageName: string;
    service: string;
    notes: string;
  }> {
    return this.filtered().map(row => ({
      date: this.formatDate(row.consultationDate),
      patient: this.resolvePatientName(row),
      pnoOrConsultId: row.pNo ?? row.consultId ?? '',
      therapist: this.resolveProviderName(row.provider),
      packageName: row.services ?? '—',
      service: row.indication ?? '—',
      notes: row.procedureDescription ?? '—'
    }));
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

  resolvePatientName(row: AestheticConsultation): string {
    if (row.patientName?.trim()) return row.patientName.trim();
    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName}`.trim() : `Patient #${row.patientId}`;
  }

  resolvePatientInitial(row: AestheticConsultation): string {
    return this.resolvePatientName(row)[0]?.toUpperCase() ?? '?';
  }

  resolveProviderName(provider: string | undefined): string {
    if (!provider?.trim()) return '—';
    const user = this.users().find(x => x.id === provider.trim());
    if (user) return user.fullName || user.userName;
    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return guidPattern.test(provider.trim()) ? 'Unknown Therapist' : provider.trim();
  }
}





