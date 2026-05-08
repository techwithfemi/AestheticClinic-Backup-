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
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';

import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AuthService } from '../../../services/auth.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { User } from '../../../models/user.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

export const DD_MMM_YYYY_FORMATS = {
  parse:   { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput:          'dd-MMM-yyyy',
    monthYearLabel:     'MMM yyyy',
    dateA11yLabel:      'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

/** Adapter that parses "dd-MMM-yyyy" strings (e.g. 08-May-2026) */
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
  selector: 'app-spa-services-report',
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
    MatChipsModule,
    MatTooltipModule,
    MatDividerModule,
    MatPaginatorModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE,   useValue: 'en-GB' },
    { provide: DateAdapter,        useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS,   useValue: DD_MMM_YYYY_FORMATS }
  ],
  templateUrl: './spa-services-report.component.html',
  styleUrl: './spa-services-report.component.scss'
})
export class SpaServicesReportComponent implements OnInit {
  private readonly endpoint      = inject(AestheticEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);
  private readonly authService   = inject(AuthService);
  private readonly alertService  = inject(AlertService);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  loadingIndicator = false;

  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly patients      = signal<AestheticPatient[]>([]);
  readonly users         = signal<User[]>([]);

  // Two-way bound to the filter inputs (UI state)
  searchText      = '';
  selectedService = '';
  dateFrom!: Date;
  dateTo!: Date;

  // Applied signals — only updated when Run Report is clicked
  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo   = signal<Date>(new Date());
  private readonly appliedService  = signal<string>('');
  readonly appliedSearch           = signal<string>('');

  readonly pageSize    = 10;
  readonly currentPage = signal<number>(0);

  readonly displayedColumns = ['date', 'patient', 'service', 'area', 'provider', 'notes'];

  // ---- Computed: consultations matching APPLIED date/service filters ----
  readonly dateFiltered = computed(() => {
    let data = this.consultations();

    const svc = this.appliedService();
    if (svc) data = data.filter(c => c.indication === svc);

    const from = new Date(this.appliedDateFrom()); from.setHours(0, 0, 0, 0);
    const to   = new Date(this.appliedDateTo());   to.setHours(23, 59, 59, 999);

    data = data.filter(c => {
      if (!c.consultationDate) return false;
      const t = new Date(c.consultationDate).getTime();
      return t >= from.getTime() && t <= to.getTime();
    });

    return data.sort((a, b) =>
      new Date(b.consultationDate ?? 0).getTime() - new Date(a.consultationDate ?? 0).getTime()
    );
  });

  // ---- Computed: live search applied ON TOP of date-filtered results ----
  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();
    return this.dateFiltered().filter(c =>
      this.resolvePatientName(c).toLowerCase().includes(term) ||
      (c.consultId  ?? '').toLowerCase().includes(term) ||
      (c.pNo        ?? '').toLowerCase().includes(term) ||
      (c.indication ?? '').toLowerCase().includes(term)
    );
  });

  // ---- Current page slice ----
  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  // ---- KPI (always reflect filtered) ----
  readonly totalSessions   = computed(() => this.filtered().length);
  readonly uniquePatients  = computed(() => new Set(this.filtered().map(c => c.pNo ?? c.patientId)).size);
  readonly topService      = computed(() => {
    const counts = new Map<string, number>();
    for (const c of this.filtered()) {
      const svc = c.indication ?? 'Unknown';
      counts.set(svc, (counts.get(svc) ?? 0) + 1);
    }
    if (!counts.size) return '—';
    return [...counts.entries()].sort((a, b) => b[1] - a[1])[0][0];
  });
  readonly thisMonthSessions = computed(() => {
    const now = new Date();
    return this.consultations().filter(c => {
      if (!c.consultationDate) return false;
      const d = new Date(c.consultationDate);
      return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
    }).length;
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
      // Frontdesk
      { label: 'Daily Report',          path: 'frontdesk-daily-report',          dept: 'frontdesk' },
      { label: 'Appointments Report',   path: 'frontdesk-appointments-report',   dept: 'frontdesk' },
      { label: 'Registration Report',   path: 'frontdesk-registration-report',   dept: 'frontdesk' },
      // Laser
      { label: 'Laser Sessions',        path: 'laser-sessions-report',           dept: 'laser' },
      { label: 'Laser Safety',          path: 'laser-safety-report',             dept: 'laser' },
      { label: 'Laser Utilization',     path: 'laser-utilization-report',        dept: 'laser' },
      // Spa
      { label: 'Spa Services',          path: 'spa-services-report',             dept: 'spa' },
      { label: 'Spa Therapists',        path: 'spa-therapists-report',           dept: 'spa' },
      { label: 'Spa Packages',          path: 'spa-packages-report',             dept: 'spa' },
      // Dental
      { label: 'Dental Treatments',     path: 'dental-treatments-report',        dept: 'dental' },
      { label: 'Dental Imaging',        path: 'dental-imaging-report',           dept: 'dental' },
      { label: 'Dental Cases',          path: 'dental-cases-report',             dept: 'dental' },
      // Aesthetics
      { label: 'Consultations',         path: 'aesthetics-consultations-report', dept: 'aesthetics' },
      { label: 'Procedures',            path: 'aesthetics-procedures-report',    dept: 'aesthetics' },
    ];

    if (this.isManagement) return all;
    const roles = new Set(this.userRoles.map(r => r.toLowerCase()));
    return all.filter(l => roles.has(l.dept));
  }

  // ---- Service types for dropdown (from all loaded consultations) ----
  readonly serviceTypes = computed(() => {
    const types = this.consultations().map(c => c.indication ?? '').filter(s => s.trim());
    return [...new Set(types)].sort();
  });

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
    this.alertService.startLoadingMessage('Loading spa services report...');

    Promise.all([
      this.endpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.accountEndpoint.getUsersEndpoint<User[]>().toPromise()
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
        `Unable to load spa services report.\r\nError: "${error?.message ?? error}"`,
        MessageSeverity.error,
        error
      );
    });
  }

  /** Push UI filter values into signals → triggers computed re-evaluation */
  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.appliedService.set(this.selectedService);
    this.appliedSearch.set(this.searchText);
    this.currentPage.set(0);
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  clearFilters(): void {
    this.searchText      = '';
    this.selectedService = '';
    const today = new Date(); today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo   = new Date(today);
    this.runReport();
  }

  printReport(): void { window.print(); }

  // ---- Resolvers ----
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
    const u = this.users().find(x => x.id === provider.trim());
    if (u) return u.fullName || u.userName;
    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return guidPattern.test(provider.trim()) ? 'Unknown Therapist' : provider.trim();
  }

  getServiceColor(service: string | undefined): string {
    const colors: Record<string, string> = {
      'Massage':    'chip-massage',
      'Facial':     'chip-facial',
      'Body Scrub': 'chip-scrub',
      'Sauna':      'chip-sauna',
      'Manicure':   'chip-manicure',
      'Pedicure':   'chip-pedicure',
      'Waxing':     'chip-waxing',
    };
    return colors[service ?? ''] ?? 'chip-default';
  }
}



