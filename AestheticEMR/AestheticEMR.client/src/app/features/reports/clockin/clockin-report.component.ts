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
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AuthService } from '../../../services/auth.service';
import { RosterEndpoint, RosterGridItem } from '../../../services/roster-endpoint.service';

export const DD_MMM_YYYY_FORMATS = {
  parse: { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput:          'dd-MMM-yyyy',
    monthYearLabel:     'MMM yyyy',
    dateA11yLabel:      'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

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
  selector: 'app-clockin-report',
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
    MatChipsModule,
    MatPaginatorModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE,   useValue: 'en-GB' },
    { provide: DateAdapter,        useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS,   useValue: DD_MMM_YYYY_FORMATS }
  ],
  templateUrl: './clockin-report.component.html',
  styleUrl: './clockin-report.component.scss'
})
export class ClockinReportComponent implements OnInit {
  private readonly endpoint    = inject(RosterEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly authService  = inject(AuthService);

  loadingIndicator = false;
  Number = Number;  // Expose Number function to template

  readonly rows = signal<RosterGridItem[]>([]);

  // UI-bound filter inputs
  searchText = '';
  selectedDept = '';
  dateFrom!: Date;
  dateTo!: Date;

  // Applied signals — only update when Run Report is clicked
  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo   = signal<Date>(new Date());
  private readonly appliedDept     = signal<string>('');
  readonly appliedSearch           = signal<string>('');

  readonly pageSize    = 10;
  readonly currentPage = signal<number>(0);

  readonly displayedColumns = ['date', 'staff', 'shift', 'clockIn', 'clockOut', 'hoursWorked', 'status', 'fine'];

  // ---- Distinct department names (for dropdown) ----
  readonly deptOptions = computed(() => {
    const set = new Set<string>();
    for (const r of this.rows()) {
      if (r.deptName?.trim()) set.add(r.deptName.trim());
    }
    return [...set].sort();
  });

  // ---- Date + dept filtered rows ----
  readonly dateFiltered = computed(() => {
    const from = new Date(this.appliedDateFrom()); from.setHours(0, 0, 0, 0);
    const to   = new Date(this.appliedDateTo());   to.setHours(23, 59, 59, 999);

    const dept = this.appliedDept();

    return this.rows()
      .filter(r => !!r.date)
      .filter(r => {
        const t = new Date(r.date!).getTime();
        return t >= from.getTime() && t <= to.getTime();
      })
      .filter(r => !dept || (r.deptName ?? '') === dept)
      .sort((a, b) => new Date(b.date ?? 0).getTime() - new Date(a.date ?? 0).getTime());
  });

  // ---- Live search applied on top ----
  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();

    return this.dateFiltered().filter(r =>
      (r.staffName  ?? '').toLowerCase().includes(term) ||
      (r.deptName   ?? '').toLowerCase().includes(term) ||
      (r.shiftName  ?? '').toLowerCase().includes(term) ||
      (r.shiftAbbrv ?? '').toLowerCase().includes(term) ||
      (r.status     ?? '').toLowerCase().includes(term)
    );
  });

  // ---- Current page slice ----
  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  // ---- KPI cards ----
  readonly totalClockIns   = computed(() => this.filtered().filter(r => !!r.clockIn).length);
  readonly lateCount       = computed(() => this.filtered().filter(r => (r.status ?? '').toLowerCase() === 'late').length);
  readonly onTimeCount     = computed(() => this.filtered().filter(r => (r.status ?? '').toLowerCase() === 'present').length);
  readonly absentCount     = computed(() => this.filtered().filter(r => (r.status ?? '').toLowerCase() === 'absent').length);
  readonly totalHoursWorked = computed(() => {
    let totalMinutes = 0;
    for (const r of this.filtered()) {
      totalMinutes += this.minutesBetween(r.clockIn, r.clockOut);
    }
    return (totalMinutes / 60).toFixed(1);
  });
  readonly totalFines      = computed(() => this.filtered().reduce((sum, r) => sum + (Number(r.fine) || 0), 0));

  get userRoles(): string[] {
    return this.authService.currentUser?.roles ?? [];
  }
  get isManagement(): boolean {
    return this.userRoles.map(r => r.toLowerCase()).includes('management');
  }
  get printSidebarLinks(): { label: string; path: string }[] {
    const all = [
      { label: 'Frontdesk Daily',          path: 'frontdesk-daily-report',          dept: 'frontdesk' },
      { label: 'Frontdesk Appointments',   path: 'frontdesk-appointments-report',   dept: 'frontdesk' },
      { label: 'Frontdesk Registration',   path: 'frontdesk-registration-report',   dept: 'frontdesk' },
      { label: 'Laser Sessions',           path: 'laser-sessions-report',           dept: 'laser' },
      { label: 'Laser Safety',             path: 'laser-safety-report',             dept: 'laser' },
      { label: 'Laser Utilization',        path: 'laser-utilization-report',        dept: 'laser' },
      { label: 'Spa Services',             path: 'spa-services-report',             dept: 'spa' },
      { label: 'Spa Therapists',           path: 'spa-therapists-report',           dept: 'spa' },
      { label: 'Spa Packages',             path: 'spa-packages-report',             dept: 'spa' },
      { label: 'Dental Treatments',        path: 'dental-treatments-report',        dept: 'dental' },
      { label: 'Dental Imaging',           path: 'dental-imaging-report',           dept: 'dental' },
      { label: 'Dental Cases',             path: 'dental-cases-report',             dept: 'dental' },
      { label: 'Consultations',            path: 'aesthetics-consultations-report', dept: 'aesthetics' },
      { label: 'Procedures',               path: 'aesthetics-procedures-report',    dept: 'aesthetics' },
      { label: 'Staff Roster',             path: 'staff-roster-report',             dept: 'staff-roster' },
      { label: 'Clock-In',                 path: 'clockin-report',                  dept: 'staff-roster' }
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
    this.alertService.startLoadingMessage('Loading clock-in data...');

    this.endpoint.getGridEndpoint<RosterGridItem[]>({ deptId: 'all', latestOnly: false })
      .subscribe({
        next: items => {
          this.rows.set(items ?? []);
          this.currentPage.set(0);
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();
        },
        error: error => {
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to load clock-in data.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  /** Push UI filter values into signals → triggers computed re-evaluation */
  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.appliedDept.set(this.selectedDept);
    this.appliedSearch.set(this.searchText);
    this.currentPage.set(0);
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedDept = '';
    const today = new Date(); today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo   = new Date(today);
    this.runReport();
  }

  printReport(): void { window.print(); }

  // ------------------------------------------------------------------ Exports
  exportExcel(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const data = rows.map(r => ({
      Date:         r.date,
      Staff:        r.staff,
      Department:   r.department,
      Shift:        r.shift,
      'Clock In':   r.clockIn,
      'Clock Out':  r.clockOut,
      'Hours Worked': r.hoursWorked,
      Status:       r.status,
      Fine:         r.fine
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook  = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Clock-In Report');

    const arrayBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([arrayBuffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    this.downloadBlob(blob, this.buildFileName('clockin-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Date', 'Staff', 'Department', 'Shift', 'Clock In', 'Clock Out', 'Hours Worked', 'Status', 'Fine'];
    const csvLines = [
      headers.join(','),
      ...rows.map(r => [r.date, r.staff, r.department, r.shift, r.clockIn, r.clockOut, r.hoursWorked, r.status, r.fine]
        .map(v => this.escapeCsv(v)).join(','))
    ];

    const csvContent = '﻿' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('clockin-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Clock-In Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Staff | Shift | In | Out | Hours | Status | Fine'
    ];

    for (const r of rows) {
      const line = `${r.date} | ${r.staff} | ${r.shift} | ${r.clockIn} | ${r.clockOut} | ${r.hoursWorked} | ${r.status} | ${r.fine}`;
      lines.push(line.length > 150 ? `${line.slice(0, 147)}...` : line);
    }

    const pdfContent = this.buildSimplePdf(lines);
    const blob = new Blob([pdfContent], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('clockin-report', 'pdf'));
  }

  private getExportRows(): {
    date: string; staff: string; department: string; shift: string;
    clockIn: string; clockOut: string; hoursWorked: string; status: string; fine: string;
  }[] {
    return this.filtered().map(r => ({
      date:        this.formatDate(r.date),
      staff:       r.staffName ?? '—',
      department:  r.deptName  ?? '—',
      shift:       r.shiftName ?? r.shiftAbbrv ?? '—',
      clockIn:     r.clockIn   ?? '—',
      clockOut:    r.clockOut  ?? '—',
      hoursWorked: this.formatHours(this.minutesBetween(r.clockIn, r.clockOut)),
      status:      r.status    ?? '—',
      fine:        r.fine != null ? String(r.fine) : '—'
    }));
  }

  // ------------------------------------------------------------------ Helpers
  formatDate(value: string | Date | undefined): string {
    if (!value) return '—';
    const d = value instanceof Date ? value : new Date(value);
    if (isNaN(d.getTime())) return '—';
    const day   = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${d.getFullYear()}`;
  }

  staffInitials(name: string | undefined): string {
    if (!name?.trim()) return '?';
    const parts = name.trim().split(/\s+/);
    return ((parts[0]?.[0] ?? '') + (parts[1]?.[0] ?? '')).toUpperCase() || '?';
  }

  getStatusColor(status: string | undefined): string {
    const s = (status ?? '').toLowerCase();
    if (s === 'present') return 'chip-present';
    if (s === 'absent')  return 'chip-absent';
    if (s === 'late')    return 'chip-late';
    if (s === 'leave')   return 'chip-leave';
    if (s === 'off')     return 'chip-off';
    return 'chip-default';
  }

  /** Hours worked between two HH:mm strings (returns "—" if not parseable). */
  hoursWorkedFor(row: RosterGridItem): string {
    return this.formatHours(this.minutesBetween(row.clockIn, row.clockOut));
  }

  private minutesBetween(start: string | null | undefined, end: string | null | undefined): number {
    if (!start || !end) return 0;
    const s = this.parseTime(start);
    const e = this.parseTime(end);
    if (s == null || e == null) return 0;
    let diff = e - s;
    if (diff < 0) diff += 24 * 60; // overnight
    return diff;
  }

  private parseTime(value: string): number | null {
    if (!value) return null;
    const match = value.trim().match(/^(\d{1,2}):(\d{2})(?::\d{2})?$/);
    if (!match) return null;
    const h = parseInt(match[1], 10);
    const m = parseInt(match[2], 10);
    if (isNaN(h) || isNaN(m) || h < 0 || h > 23 || m < 0 || m > 59) return null;
    return h * 60 + m;
  }

  private formatHours(minutes: number): string {
    if (!minutes || minutes <= 0) return '—';
    const h = Math.floor(minutes / 60);
    const m = minutes % 60;
    return `${h}h ${m.toString().padStart(2, '0')}m`;
  }

  private buildFileName(prefix: string, extension: string): string {
    const now = new Date();
    const stamp = `${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}-${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}${now.getSeconds().toString().padStart(2, '0')}`;
    return `${prefix}-${stamp}.${extension}`;
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
    const safeLines  = lines.slice(0, 220);
    const lineHeight = 14;
    const startY     = 800;

    const content = [
      'BT',
      '/F1 10 Tf',
      `40 ${startY} Td`,
      ...safeLines.flatMap((line, index) => {
        const escaped = this.escapePdfText(line);
        if (index === 0) return [`(${escaped}) Tj`];
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

  private getApiError(err: unknown): string {
    const e = err as { error?: unknown; message?: string; status?: number; statusText?: string };
    if (e?.error) {
      const body = e.error;
      if (typeof body === 'string') return body;
      if (typeof body === 'object') {
        const b = body as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (b.detail) return `${b.title ?? 'Error'}: ${b.detail}`;
        if (b.message) return b.message;
        if (b.errors) {
          const flat = Object.entries(b.errors).map(([k, v]) => `${k}: ${(v ?? []).join(', ')}`).join('\n');
          if (flat) return flat;
        }
        try { return JSON.stringify(body); } catch { /* fall through */ }
      }
    }
    if (e?.status) return `${e.status} ${e.statusText ?? ''} - ${e.message ?? 'Request failed'}`.trim();
    return (err as { message?: string })?.message ?? 'An error occurred.';
  }

  getErrorMessage(error: unknown): string {
    return this.getApiError(error);
  }
}
