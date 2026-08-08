import { Component, OnDestroy, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  AdminAuditReportModuleLookup,
  AdminAuditReportRow,
  AdminAuditReportUserLookup,
  AestheticEndpoint
} from '../../../services/aesthetic-endpoint.service';
import { fadeInOut } from '../../../services/animations';
import { UtcDisplayPipe } from '../../../pipes/utc-display.pipe';

interface JsonDisplay {
  readonly isJson: boolean;
  readonly pairs: readonly { key: string; value: string }[];
  readonly hiddenCount: number;
  readonly pretty: string;
  readonly raw: string;
}

interface AdminAuditReportGridRow extends AdminAuditReportRow {
  readonly userActionDisplay: JsonDisplay;
  readonly originalActionDisplay: JsonDisplay;
  readonly srcDisplay: JsonDisplay;
}

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
  selector: 'app-admin-audit-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatTableModule,
    MatPaginatorModule,
    NgSelectModule,
    TranslateModule,
    UtcDisplayPipe
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS }
  ],
  animations: [fadeInOut],
  templateUrl: './admin-audit-report.component.html',
  styleUrl: './admin-audit-report.component.scss'
})
export class AdminAuditReportComponent implements OnInit, OnDestroy {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private autoSearchTimer: ReturnType<typeof setTimeout> | null = null;
  private readonly autoSearchDelayMs = 450;

  loadingIndicator = false;

  readonly userOptions = signal<AdminAuditReportUserLookup[]>([]);
  readonly moduleOptions = signal<AdminAuditReportModuleLookup[]>([]);
  readonly rows = signal<AdminAuditReportGridRow[]>([]);

  filterType: 'ALL' | 'MODULE' | 'USER' = 'ALL';
  selectedUserName = '';
  selectedUserDisplayText = '';
  selectedModuleName = '';
  searchTranCode = '';
  dateFrom!: Date;
  dateTo!: Date;

  readonly pageSize = 10;
  readonly currentPage = signal(0);
  readonly displayedColumns = ['date', 'time', 'userAction', 'originalAction', 'remarks', 'src', 'employee', 'tranCode', 'module'];
  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.rows().slice(start, start + this.pageSize);
  });

  ngOnInit(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.scheduleAutoSearch();
  }

  ngOnDestroy(): void {
    if (this.autoSearchTimer) {
      clearTimeout(this.autoSearchTimer);
      this.autoSearchTimer = null;
    }
  }

  onFilterTypeChanged(): void {
    this.selectedUserName = '';
    this.selectedUserDisplayText = '';
    this.selectedModuleName = '';

    if (this.filterType === 'USER' && this.userOptions().length === 0) {
      this.loadUsers();
    }

    if (this.filterType === 'MODULE' && this.moduleOptions().length === 0) {
      this.loadModules();
    }

    this.scheduleAutoSearch();
  }

  onSearchChanged(): void {
    this.scheduleAutoSearch();
  }

  onDateChanged(): void {
    this.scheduleAutoSearch();
  }

  onModuleChanged(): void {
    this.scheduleAutoSearch();
  }

  runReport(showValidation = true): void {
    if (!this.canRunReport()) {
      if (showValidation) {
        this.showValidationMessage();
      }

      if (!showValidation) {
        this.rows.set([]);
        this.currentPage.set(0);
      }

      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading audit report...');

    this.endpoint.getAuditReportRowsEndpoint({
      fromDate: this.formatDateForApi(this.dateFrom),
      toDate: this.formatDateForApi(this.dateTo),
      filterType: this.filterType,
      filterValue: this.getFilterValue(),
      searchTerm: this.searchTranCode.trim() || undefined
    }).subscribe({
      next: rows => {
        this.rows.set(this.mapRows(rows ?? []));
        this.currentPage.set(0);
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', `Unable to load admin audit report.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      },
      complete: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
      }
    });
  }

  onPageChanged(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  private loadUsers(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading users...');

    this.endpoint.getAuditReportUsersEndpoint().subscribe({
      next: users => {
        this.userOptions.set(users ?? []);
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', `Unable to load users.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      },
      complete: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
      }
    });
  }

  private loadModules(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading modules...');

    this.endpoint.getAuditReportModulesEndpoint().subscribe({
      next: modules => {
        this.moduleOptions.set(modules ?? []);
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', `Unable to load modules.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      },
      complete: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
      }
    });
  }

  onUserChanged(): void {
    const selected = this.userOptions().find(x => x.userName === this.selectedUserName);
    this.selectedUserDisplayText = selected?.displayText ?? '';
    this.scheduleAutoSearch();
  }

  private getFilterValue(): string | undefined {
    if (this.filterType === 'USER') {
      return this.selectedUserName || undefined;
    }

    if (this.filterType === 'MODULE') {
      return this.selectedModuleName || undefined;
    }

    return undefined;
  }

  private canRunReport(): boolean {
    if (!this.dateFrom || !this.dateTo || this.dateFrom > this.dateTo) {
      return false;
    }

    if (this.filterType === 'USER' && !this.selectedUserName) {
      return false;
    }

    if (this.filterType === 'MODULE' && !this.selectedModuleName) {
      return false;
    }

    return true;
  }

  private showValidationMessage(): void {
    if (!this.dateFrom || !this.dateTo) {
      this.alertService.showMessage('Validation', 'Please specify report period.', MessageSeverity.warn);
      return;
    }

    if (this.dateFrom > this.dateTo) {
      this.alertService.showMessage('Validation', 'Start date cannot be later than end date.', MessageSeverity.warn);
      return;
    }

    if (this.filterType === 'USER' && !this.selectedUserName) {
      this.alertService.showMessage('Validation', 'Please select a user.', MessageSeverity.warn);
      return;
    }

    if (this.filterType === 'MODULE' && !this.selectedModuleName) {
      this.alertService.showMessage('Validation', 'Please select a module.', MessageSeverity.warn);
    }
  }

  private scheduleAutoSearch(): void {
    if (this.autoSearchTimer) {
      clearTimeout(this.autoSearchTimer);
    }

    this.autoSearchTimer = setTimeout(() => {
      this.runReport(false);
    }, this.autoSearchDelayMs);
  }

  private formatDateForApi(value: Date): string {
    const year = value.getFullYear();
    const month = `${value.getMonth() + 1}`.padStart(2, '0');
    const day = `${value.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (error && typeof error === 'object' && 'message' in error) return String((error as { message?: unknown }).message ?? error);
    return String(error);
  }

  private mapRows(rows: AdminAuditReportRow[]): AdminAuditReportGridRow[] {
    return rows.map(row => ({
      ...row,
      userActionDisplay: this.buildJsonDisplay(row.userAction),
      originalActionDisplay: this.buildJsonDisplay(row.originalAction),
      srcDisplay: this.buildJsonDisplay(row.src)
    }));
  }

  private buildJsonDisplay(raw: string | null | undefined, maxPairs = 4, maxValueLength = 80): JsonDisplay {
    const text = (raw ?? '').trim();
    if (!text) {
      return {
        isJson: false,
        pairs: [],
        hiddenCount: 0,
        pretty: '',
        raw: ''
      };
    }

    try {
      const parsed = JSON.parse(text) as unknown;
      if (parsed && typeof parsed === 'object' && !Array.isArray(parsed)) {
        const entries = Object.entries(parsed as Record<string, unknown>);
        const pairs = entries.slice(0, maxPairs).map(([key, value]) => ({
          key,
          value: this.stringifyJsonValue(value, maxValueLength)
        }));

        return {
          isJson: true,
          pairs,
          hiddenCount: Math.max(entries.length - pairs.length, 0),
          pretty: JSON.stringify(parsed, null, 2),
          raw: text
        };
      }

      return {
        isJson: true,
        pairs: [{ key: 'value', value: this.stringifyJsonValue(parsed, maxValueLength) }],
        hiddenCount: 0,
        pretty: JSON.stringify(parsed, null, 2),
        raw: text
      };
    } catch {
      return {
        isJson: false,
        pairs: [],
        hiddenCount: 0,
        pretty: text,
        raw: text
      };
    }
  }

  private stringifyJsonValue(value: unknown, maxLength: number): string {
    if (value === null || value === undefined) {
      return 'null';
    }

    const text = typeof value === 'string'
      ? value
      : typeof value === 'object'
        ? JSON.stringify(value)
        : String(value);

    return text.length > maxLength ? `${text.slice(0, maxLength)}…` : text;
  }
}



