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

import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { AuthService } from '../../../services/auth.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';
import { User } from '../../../models/user.model';

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
  selector: 'app-frontdesk-registration-report',
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
  templateUrl: './frontdesk-registration-report.component.html',
  styleUrl: './frontdesk-registration-report.component.scss'
})
export class FrontdeskRegistrationReportComponent implements OnInit {
  private readonly endpoint        = inject(HPatientEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly authService     = inject(AuthService);
  private readonly alertService    = inject(AlertService);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  loadingIndicator = false;

  readonly registrations = signal<HPatient[]>([]);
  readonly users         = signal<User[]>([]);
  readonly todayVisits   = signal<QryhvisitsForToday[]>([]);

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

  readonly displayedColumns = ['date', 'patient', 'sex', 'phone', 'company', 'registeredBy'];

  // ---- Computed: registrations matching APPLIED date/patient filters ----
  readonly dateFiltered = computed(() => {
    let data = this.registrations();

    const pno = this.appliedPatient();
    if (pno) data = data.filter(r => (r.pno ?? '') === pno);

    const from = new Date(this.appliedDateFrom()); from.setHours(0, 0, 0, 0);
    const to   = new Date(this.appliedDateTo());   to.setHours(23, 59, 59, 999);

    data = data.filter(r => {
      if (!r.regDate) return false;
      const t = new Date(r.regDate).getTime();
      return t >= from.getTime() && t <= to.getTime();
    });

    return data.sort((a, b) =>
      new Date(b.regDate ?? 0).getTime() - new Date(a.regDate ?? 0).getTime()
    );
  });

  // ---- Computed: live search applied ON TOP of date-filtered results ----
  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();
    return this.dateFiltered().filter(r =>
      `${r.pSurName ?? ''} ${r.pFirstname ?? ''}`.toLowerCase().includes(term) ||
      (r.pno      ?? '').toLowerCase().includes(term) ||
      (r.pPhoneNo ?? '').toLowerCase().includes(term) ||
      (r.email    ?? '').toLowerCase().includes(term) ||
      (r.coyName  ?? '').toLowerCase().includes(term)
    );
  });

  // ---- Current page slice ----
  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  // ---- KPI cards (reflect filtered set) ----
  readonly totalRegistrations = computed(() => this.filtered().length);
  readonly registeredToday    = computed(() => this.filtered().filter(r => this.isToday(r.regDate)).length);
  readonly uniqueCompanies    = computed(() =>
    new Set(this.filtered().map(r => (r.coyName ?? '').trim()).filter(Boolean)).size
  );
  readonly thisMonthCount = computed(() => {
    const now = new Date();
    return this.registrations().filter(r => {
      if (!r.regDate) return false;
      const d = new Date(r.regDate);
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
    this.alertService.startLoadingMessage('Loading registration report...');

    const usersPromise = this.accountEndpoint.getUsersEndpoint<User[]>().toPromise()
      .catch((error: { status?: number }) => {
        if (error?.status === 401 || error?.status === 403) {
          return [] as User[];
        }
        throw error;
      });

    const visitsPromise = this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise()
      .catch(() => [] as QryhvisitsForToday[]);

    Promise.all([
      this.endpoint.getHPatientsEndpoint<HPatient[]>().toPromise(),
      usersPromise,
      visitsPromise
    ]).then(([registrations, users, visits]) => {
      this.registrations.set(registrations ?? []);
      this.users.set(users ?? []);
      this.todayVisits.set(visits ?? []);
      this.currentPage.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load registration report.\r\nError: "${error?.message ?? error}"`,
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
      'Patient No': r.pno,
      Patient: r.patient,
      Sex: r.sex,
      Phone: r.phone,
      Company: r.company,
      Category: r.category,
      'Registered By': r.registeredBy
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Registration Report');

    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    this.downloadBlob(blob, this.buildFileName('registration-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Date', 'Patient No', 'Patient', 'Sex', 'Phone', 'Company', 'Category', 'Registered By'];
    const csvLines = [
      headers.join(','),
      ...rows.map(r => [r.date, r.pno, r.patient, r.sex, r.phone, r.company, r.category, r.registeredBy]
        .map(v => this.escapeCsv(v)).join(','))
    ];
    const csvContent = '﻿' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('registration-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Registration Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Patient | Sex | Phone | Company | Registered By'
    ];
    for (const r of rows) {
      const line = `${r.date} | ${r.patient} | ${r.sex} | ${r.phone} | ${r.company} | ${r.registeredBy}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const pdfContent = this.buildSimplePdf(lines);
    const blob = new Blob([pdfContent], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('registration-report', 'pdf'));
  }

  private getExportRows(): Array<{
    date: string; pno: string; patient: string; sex: string;
    phone: string; company: string; category: string; registeredBy: string;
  }> {
    return this.filtered().map(r => ({
      date: this.formatDate(r.regDate),
      pno: r.pno ?? '',
      patient: this.resolvePatientName(r),
      sex: r.sex ?? '—',
      phone: r.pPhoneNo ?? '—',
      company: r.coyName ?? '—',
      category: r.clientCatId ?? '—',
      registeredBy: this.resolveUserName(r.userName)
    }));
  }

  // ---- Resolvers ----
  resolvePatientName(row: HPatient): string {
    const name = `${row.pSurName ?? ''} ${row.pFirstname ?? ''}`.trim();
    return name || `Patient ${row.pno ?? ''}`.trim();
  }

  resolvePatientInitial(row: HPatient): string {
    return this.resolvePatientName(row)[0]?.toUpperCase() ?? '?';
  }

  resolveUserName(userName: string | undefined): string {
    if (!userName?.trim()) return '—';
    const key = userName.trim();
    const u = this.users().find(x => x.id === key || x.userName === key);
    return u ? (u.fullName || u.userName) : key;
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
