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
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { fadeInOut } from '../../../services/animations';

import { firstValueFrom } from 'rxjs';

import { AestheticConsultation, AestheticFollowUp } from '../../../models/aesthetic.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { User } from '../../../models/user.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AuthService } from '../../../services/auth.service';

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
      return `${d}-${m}-${date.getFullYear()}`;
    }
    return super.format(date, displayFormat);
  }
}

interface AttendanceOption {
  key: string;
  consultId: string;
  pNo: string;
  patientName: string;
  recDate?: string;
}

@Component({
  selector: 'app-aesthetics-outcomes-report',
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
  animations: [fadeInOut],
  templateUrl: './aesthetics-outcomes-report.component.html',
  styleUrl: './aesthetics-outcomes-report.component.scss'
})
export class AestheticsOutcomesReportComponent implements OnInit {
  private readonly alertService = inject(AlertService);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly aestheticEndpoint = inject(AestheticEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);
  private readonly authService = inject(AuthService);

  loadingIndicator = false;

  readonly followUps = signal<AestheticFollowUp[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly users = signal<User[]>([]);

  searchText = '';
  dateFrom!: Date;
  dateTo!: Date;
  selectedAttendanceKey = '';

  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo = signal<Date>(new Date());
  readonly appliedSearch = signal<string>('');

  readonly currentPage = signal<number>(0);
  readonly pageSize = 10;

  readonly displayedColumns = ['date', 'patient', 'consultId', 'procedure', 'score', 'outcome', 'user', 'status'];
  readonly selectedOutcome = signal<AestheticFollowUp | null>(null);

  readonly attendanceOptions = computed(() => {
    return this.todayVisits()
      .filter(v => !!v.consultId && !!v.pNo)
      .map(v => ({
        key: `${(v.consultId ?? '').trim()}|${(v.pNo ?? '').trim()}`,
        consultId: (v.consultId ?? '').trim(),
        pNo: (v.pNo ?? '').trim(),
        patientName: (v.fullname ?? '').trim() || this.resolvePatientNameByPNo(v.pNo),
        recDate: v.recDate
      } as AttendanceOption))
      .sort((a, b) => (b.recDate ?? '').localeCompare(a.recDate ?? ''));
  });

  readonly dateFiltered = computed(() => {
    const from = new Date(this.appliedDateFrom());
    from.setHours(0, 0, 0, 0);
    const to = new Date(this.appliedDateTo());
    to.setHours(23, 59, 59, 999);

    return this.followUps()
      .filter(f => this.getFollowUpDateValue(f) >= from.getTime() && this.getFollowUpDateValue(f) <= to.getTime())
      .sort((a, b) => this.getFollowUpDateValue(b) - this.getFollowUpDateValue(a));
  });

  readonly filteredRows = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();

    return this.dateFiltered().filter(f =>
      this.resolveFollowUpPatientName(f).toLowerCase().includes(term)
      || (this.resolveFollowUpConsultId(f)).toLowerCase().includes(term)
      || (this.resolveProcedureType(f)).toLowerCase().includes(term)
      || (f.outcome ?? '').toLowerCase().includes(term)
      || this.resolveProviderName(f).toLowerCase().includes(term)
    );
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filteredRows().slice(start, start + this.pageSize);
  });

  readonly totalRows = computed(() => this.filteredRows().length);
  readonly averageScore = computed(() => {
    const scores = this.filteredRows()
      .map(x => x.patientSatisfactionScore)
      .filter((x): x is number => x !== undefined && x !== null);
    if (!scores.length) return '—';
    const avg = scores.reduce((sum, v) => sum + v, 0) / scores.length;
    return avg.toFixed(1);
  });
  readonly positiveOutcomes = computed(() => this.filteredRows().filter(x => (x.patientSatisfactionScore ?? 0) >= 8).length);
  readonly completedFollowUps = computed(() => this.filteredRows().filter(x => x.isCompleted).length);

  readonly selectedSummary = computed(() => {
    const selected = this.selectedOutcome();
    if (!selected) {
      return {
        patientName: '—',
        consultId: '—',
        pNo: '—',
        procedure: '—',
        user: '—',
        date: '—',
        score: '—',
        status: '—',
        outcome: 'No outcome selected.'
      };
    }

    return {
      patientName: this.resolveFollowUpPatientName(selected),
      consultId: this.resolveFollowUpConsultId(selected) || '—',
      pNo: this.resolveFollowUpPNo(selected) || '—',
      procedure: this.resolveProcedureType(selected),
      user: this.resolveProviderName(selected),
      date: this.formatDate(this.resolveFollowUpDate(selected)),
      score: selected.patientSatisfactionScore?.toString() ?? '—',
      status: this.resolveStatus(selected),
      outcome: selected.outcome || selected.notes || 'No comments provided.'
    };
  });

  get userRoles(): string[] {
    return this.authService.currentUser?.roles ?? [];
  }

  get isManagement(): boolean {
    return this.userRoles.map(r => r.toLowerCase()).includes('management');
  }

  get printSidebarLinks(): { label: string; path: string }[] {
    const all = [
      { label: 'Frontdesk Daily', path: 'frontdesk-daily-report', dept: 'frontdesk' },
      { label: 'Frontdesk Appointments', path: 'frontdesk-appointments-report', dept: 'frontdesk' },
      { label: 'Frontdesk Registration', path: 'frontdesk-registration-report', dept: 'frontdesk' },
      { label: 'Laser Sessions', path: 'laser-sessions-report', dept: 'laser' },
      { label: 'Laser Safety', path: 'laser-safety-report', dept: 'laser' },
      { label: 'Laser Utilization', path: 'laser-utilization-report', dept: 'laser' },
      { label: 'Spa Services', path: 'spa-services-report', dept: 'spa' },
      { label: 'Spa Therapists', path: 'spa-therapists-report', dept: 'spa' },
      { label: 'Spa Packages', path: 'spa-packages-report', dept: 'spa' },
      { label: 'Dental Treatments', path: 'dental-treatments-report', dept: 'dental' },
      { label: 'Dental Imaging', path: 'dental-imaging-report', dept: 'dental' },
      { label: 'Dental Cases', path: 'dental-cases-report', dept: 'dental' },
      { label: 'Aesthetics Consultations', path: 'aesthetics-consultations-report', dept: 'aesthetics' },
      { label: 'Aesthetics Procedures', path: 'aesthetics-procedures-report', dept: 'aesthetics' },
      { label: 'Aesthetics Outcomes', path: 'aesthetics-outcomes-report', dept: 'aesthetics' },
      { label: 'Aesthetics Skin Assessment', path: 'aesthetics-skin-assessment-report', dept: 'aesthetics' },
      { label: 'Aesthetics Consent', path: 'aesthetics-consent-report', dept: 'aesthetics' }
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

  async load(): Promise<void> {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading outcomes report...');

    try {
      const usersPromise = firstValueFrom(this.accountEndpoint.getUsersEndpoint<User[]>())
        .catch((error: { status?: number }) => {
          if (error?.status === 401 || error?.status === 403) return [] as User[];
          throw error;
        });

      const [todayVisits, patients, followUps, botox, laser, spa, users] = await Promise.all([
        firstValueFrom(this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>()),
        firstValueFrom(this.patientEndpoint.getHPatientsEndpoint<HPatient[]>()),
        firstValueFrom(this.aestheticEndpoint.getFollowUpsEndpoint<AestheticFollowUp[]>()),
        firstValueFrom(this.aestheticEndpoint.getBotoxConsultationsEndpoint<AestheticConsultation[]>()),
        firstValueFrom(this.aestheticEndpoint.getLaserConsultationsEndpoint<AestheticConsultation[]>()),
        firstValueFrom(this.aestheticEndpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>()),
        usersPromise
      ]);

      const byId = new Map<number, AestheticConsultation>();
      for (const c of [...(botox ?? []), ...(laser ?? []), ...(spa ?? [])]) {
        byId.set(c.id, c);
      }

      this.todayVisits.set(todayVisits ?? []);
      this.legacyPatients.set(patients ?? []);
      this.followUps.set(followUps ?? []);
      this.consultations.set([...byId.values()]);
      this.users.set(users ?? []);
      this.currentPage.set(0);

      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    } catch (error: unknown) {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      const message = error instanceof Error ? error.message : String(error);
      this.alertService.showStickyMessage('Load Error', `Unable to load outcomes report.\r\nError: "${message}"`, MessageSeverity.error, error);
    }
  }

  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.currentPage.set(0);

    if (this.selectedAttendanceKey && !this.attendanceOptions().some(x => x.key === this.selectedAttendanceKey)) {
      this.selectedAttendanceKey = '';
      this.selectedOutcome.set(null);
    }
  }

  clearFilters(): void {
    this.searchText = '';
    this.appliedSearch.set('');
    this.selectedAttendanceKey = '';
    this.selectedOutcome.set(null);

    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);

    this.runReport();
  }

  displayReport(): void {
    const selected = this.attendanceOptions().find(x => x.key === this.selectedAttendanceKey);
    if (!selected) {
      this.alertService.showStickyMessage('Selection Required', 'Select patient first.', MessageSeverity.warn);
      return;
    }

    const match = this.filteredRows()
      .filter(f =>
        (f.patientSatisfactionConsultId ?? '').trim().toLowerCase() === selected.consultId.toLowerCase()
        && (f.patientSatisfactionPNo ?? '').trim().toLowerCase() === selected.pNo.toLowerCase())
      .sort((a, b) => this.getFollowUpDateValue(b) - this.getFollowUpDateValue(a))[0]
      || this.filteredRows()
        .filter(f => (this.resolveFollowUpPNo(f)).trim().toLowerCase() === selected.pNo.toLowerCase())
        .sort((a, b) => this.getFollowUpDateValue(b) - this.getFollowUpDateValue(a))[0];

    if (!match) {
      this.alertService.showStickyMessage('No Data', `No outcome record found for ${selected.patientName} [${selected.consultId}].`, MessageSeverity.warn);
      this.selectedOutcome.set(null);
      return;
    }

    this.selectedOutcome.set(match);
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

    const worksheet = XLSX.utils.json_to_sheet(rows);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Outcomes Report');
    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    this.downloadBlob(blob, this.buildFileName('aesthetics-outcomes-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = Object.keys(rows[0]);
    const csvLines = [
      headers.join(','),
      ...rows.map(r => headers.map(h => this.escapeCsv(String(r[h] ?? ''))).join(','))
    ];

    const blob = new Blob(['\uFEFF' + csvLines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('aesthetics-outcomes-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Aesthetics Outcomes Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Date | Patient | Consult ID | Procedure | Score | User | Status'
    ];

    for (const row of rows) {
      const line = `${row['Date']} | ${row['Patient']} | ${row['Consult ID']} | ${row['Procedure']} | ${row['Satisfaction Score']} | ${row['User']} | ${row['Status']}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const blob = new Blob([this.buildSimplePdf(lines)], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('aesthetics-outcomes-report', 'pdf'));
  }

  resolveConsultId(row: AestheticFollowUp): string {
    return this.resolveFollowUpConsultId(row) || '—';
  }

  resolveFollowUpPatientName(row: AestheticFollowUp): string {
    if (row.patientName?.trim()) return row.patientName.trim();

    const pNo = this.resolveFollowUpPNo(row);
    if (pNo) return this.resolvePatientNameByPNo(pNo);

    return row.patientId ? `Patient #${row.patientId}` : 'Unknown Patient';
  }

  resolveProviderName(row: AestheticFollowUp): string {
    const consultation = this.resolveConsultation(row);
    const provider = consultation?.provider;
    if (!provider?.trim()) return '—';

    const value = provider.trim();
    const user = this.users().find(u => u.id === value);
    if (user) return user.fullName || user.userName;

    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return guidPattern.test(value) ? 'Unknown User' : value;
  }

  resolveProcedureType(row: AestheticFollowUp): string {
    const consultation = this.resolveConsultation(row);
    return consultation?.procedureType || consultation?.indication || 'Aesthetic Follow-up';
  }

  resolveStatus(row: AestheticFollowUp): string {
    if (row.isCompleted) return 'Completed';
    if ((row.patientSatisfactionScore ?? 0) > 0) return 'Scored';
    return 'Pending';
  }

  private resolveFollowUpConsultId(row: AestheticFollowUp): string {
    return row.patientSatisfactionConsultId?.trim()
      || this.resolveConsultation(row)?.consultId?.trim()
      || '';
  }

  private resolveFollowUpPNo(row: AestheticFollowUp): string {
    return row.patientSatisfactionPNo?.trim()
      || this.resolveConsultation(row)?.pNo?.trim()
      || '';
  }

  private resolveConsultation(row: AestheticFollowUp): AestheticConsultation | undefined {
    const consult = this.consultations().find(c => c.id === row.consultationId);
    if (consult) return consult;

    const byConsultAndPNo = this.consultations().find(c =>
      (c.consultId ?? '').trim().toLowerCase() === (row.patientSatisfactionConsultId ?? '').trim().toLowerCase()
      && (c.pNo ?? '').trim().toLowerCase() === (row.patientSatisfactionPNo ?? '').trim().toLowerCase());

    return byConsultAndPNo;
  }

  private resolvePatientNameByPNo(pNo: string): string {
    const fromTodayVisits = this.todayVisits().find(v => (v.pNo ?? '').trim().toLowerCase() === (pNo ?? '').trim().toLowerCase());
    if (fromTodayVisits?.fullname?.trim()) return fromTodayVisits.fullname.trim();

    const p = this.legacyPatients().find(x => (x.pno ?? '').trim().toLowerCase() === (pNo ?? '').trim().toLowerCase());
    if (!p) return pNo || 'Unknown Patient';
    const full = [p.pSurName, p.pFirstname].filter(Boolean).join(' ').trim();
    return full || pNo || 'Unknown Patient';
  }

  private getFollowUpDateValue(row: AestheticFollowUp): number {
    return new Date(this.resolveFollowUpDate(row)).getTime() || 0;
  }

  private resolveFollowUpDate(row: AestheticFollowUp): string {
    return row.patientSatisfactionSubmittedOn
      || row.completedDate
      || row.scheduledDate
      || '';
  }

  private getExportRows(): Record<string, string>[] {
    return this.filteredRows().map(row => ({
      Date: this.formatDate(this.resolveFollowUpDate(row)),
      Patient: this.resolveFollowUpPatientName(row),
      'Consult ID': this.resolveFollowUpConsultId(row) || '—',
      Procedure: this.resolveProcedureType(row),
      'Satisfaction Score': row.patientSatisfactionScore?.toString() || '—',
      Outcome: row.outcome || row.notes || '—',
      User: this.resolveProviderName(row),
      Status: this.resolveStatus(row)
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
}


