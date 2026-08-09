import { AfterViewInit, Component, Input, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { NgSelectModule } from '@ng-select/ng-select';

import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { JournalAccountLookup, JournalListLine, PagedJournalLinesResult } from '../../../models/accounting/journal-entry.model';

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
  selector: 'app-accounting-journal-report-base',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatSortModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS }
  ],
  animations: [fadeInOut],
  template: `
    <mat-card class="report-card" [@fadeInOut]>
      <mat-card-header>
        <mat-card-title>{{ title }}</mat-card-title>
        <mat-card-subtitle>{{ subtitle }}</mat-card-subtitle>
      </mat-card-header>

      <mat-card-content>
        <div class="form-grid">
          <mat-form-field appearance="outline">
            <mat-label>Start Date</mat-label>
            <input matInput [matDatepicker]="startPicker" [(ngModel)]="dateFrom">
            <mat-datepicker-toggle matSuffix [for]="startPicker"></mat-datepicker-toggle>
            <mat-datepicker #startPicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>End Date</mat-label>
            <input matInput [matDatepicker]="endPicker" [(ngModel)]="dateTo">
            <mat-datepicker-toggle matSuffix [for]="endPicker"></mat-datepicker-toggle>
            <mat-datepicker #endPicker></mat-datepicker>
          </mat-form-field>

          <div class="ng-select-field">
            <ng-select
              [items]="accountOptions()"
              bindLabel="accountName"
              bindValue="accountNo"
              [searchable]="true"
              [clearable]="false"
              [virtualScroll]="true"
              [(ngModel)]="selectedAccountNo"
              (ngModelChange)="onAccountChanged()"
              placeholder="--Select Account--">
              <ng-option [value]="selectAccountOption">--Select Account--</ng-option>
              <ng-option [value]="'(ALL)'">(ALL)</ng-option>
            </ng-select>
          </div>

          <mat-form-field appearance="outline">
            <mat-label>Search</mat-label>
            <input matInput [(ngModel)]="searchText" placeholder="Tran No / Account / Description">
          </mat-form-field>
        </div>

        <div class="button-row">
          <button mat-raised-button color="primary" (click)="runReport()" [disabled]="loadingIndicator()">
            <mat-icon>analytics</mat-icon>
            Run Report
          </button>
          <button mat-stroked-button color="primary" (click)="clearFilters()" [disabled]="loadingIndicator()">
            <mat-icon>filter_alt_off</mat-icon>
            Clear
          </button>
        </div>

        <div class="summary-row">
          <div class="summary-item"><span>Rows:</span> {{ filteredRows().length }}</div>
          <div class="summary-item"><span>Total Debit:</span> {{ totalDebit() | number:'1.2-2' }}</div>
          <div class="summary-item"><span>Total Credit:</span> {{ totalCredit() | number:'1.2-2' }}</div>
        </div>

        @if (loadingIndicator()) {
          <div class="loading-wrap">
            <mat-spinner diameter="36"></mat-spinner>
          </div>
        } @else {
          <div class="table-wrap">
            <table mat-table [dataSource]="pagedRows()" matSort [matSortActive]="sortState.active" [matSortDirection]="sortState.direction" (matSortChange)="onSortChange($event)" class="mat-elevation-z1 full-width-table">
              <ng-container matColumnDef="sn">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="sn">SN</th>
                <td mat-cell *matCellDef="let row">{{ row.sn }}</td>
              </ng-container>

              <ng-container matColumnDef="tranDate">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="tranDate">Tran Date</th>
                <td mat-cell *matCellDef="let row">{{ formatDate(row.tranDate) }}</td>
              </ng-container>

              <ng-container matColumnDef="accountName">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="accountName">Account Name</th>
                <td mat-cell *matCellDef="let row">{{ row.accountName }}</td>
              </ng-container>

              <ng-container matColumnDef="accountNo">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="accountNo">Account No</th>
                <td mat-cell *matCellDef="let row">{{ row.accountNo }}</td>
              </ng-container>

              <ng-container matColumnDef="description">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="description">Description</th>
                <td mat-cell *matCellDef="let row">{{ row.description || 'NIL' }}</td>
              </ng-container>

              <ng-container matColumnDef="debit">
                <th mat-header-cell *matHeaderCellDef class="amount-col" mat-sort-header="debit">Debit</th>
                <td mat-cell *matCellDef="let row" class="amount-col">{{ row.debit | number:'1.2-2' }}</td>
              </ng-container>

              <ng-container matColumnDef="credit">
                <th mat-header-cell *matHeaderCellDef class="amount-col" mat-sort-header="credit">Credit</th>
                <td mat-cell *matCellDef="let row" class="amount-col">{{ row.credit | number:'1.2-2' }}</td>
              </ng-container>

              <ng-container matColumnDef="tranNo">
                <th mat-header-cell *matHeaderCellDef mat-sort-header="tranNo">Tran No</th>
                <td mat-cell *matCellDef="let row">{{ row.tranNo }}</td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
            </table>
          </div>

          <mat-paginator
            [length]="filteredRows().length"
            [pageSize]="pageSize"
            [pageIndex]="currentPage"
            [pageSizeOptions]="[10]"
            (page)="onPageChange($event)"
            [disabled]="loadingIndicator()">
          </mat-paginator>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .report-card { margin: 16px; }
    .form-grid { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: 12px; margin: 8px 0 12px; }

    .ng-select-field { width: 100%; }

    :host ::ng-deep .ng-select {
      width: 100%;
      font-size: 14px;
    }

    :host ::ng-deep .ng-select .ng-select-container {
      min-height: 56px;
      border-radius: 4px;
      border: 1px solid rgba(0, 0, 0, 0.23);
      background: #fff;
      color: rgba(0, 0, 0, 0.87);
      box-shadow: none;
      transition: border-color .15s ease, box-shadow .15s ease;
    }

    :host ::ng-deep .ng-select .ng-select-container .ng-value-container {
      padding-left: 12px;
      padding-right: 8px;
      padding-top: 6px;
      padding-bottom: 6px;
    }

    :host ::ng-deep .ng-select .ng-placeholder,
    :host ::ng-deep .ng-select .ng-value,
    :host ::ng-deep .ng-select .ng-input > input {
      color: rgba(0, 0, 0, 0.87);
    }

    :host ::ng-deep .ng-select .ng-arrow-wrapper .ng-arrow {
      border-color: rgba(0, 0, 0, 0.54) transparent transparent;
    }

    :host ::ng-deep .ng-select.ng-select-focused .ng-select-container,
    :host ::ng-deep .ng-select.ng-select-opened .ng-select-container {
      border-color: #3f51b5;
      box-shadow: 0 0 0 1px rgba(63, 81, 181, 0.35);
    }

    :host ::ng-deep .ng-dropdown-panel {
      border: 1px solid #d1d5db;
      border-radius: 4px;
      background: #fff;
      box-shadow: 0 8px 20px rgba(15, 23, 42, 0.12);
      margin-top: 2px;
    }

    :host ::ng-deep .ng-dropdown-panel .ng-dropdown-panel-items .ng-option {
      color: rgba(0, 0, 0, 0.87);
      background: #fff;
      padding: 9px 12px;
    }

    :host ::ng-deep .ng-dropdown-panel .ng-dropdown-panel-items .ng-option.ng-option-marked {
      background: #f5f7fb;
      color: rgba(0, 0, 0, 0.87);
    }

    :host ::ng-deep .ng-dropdown-panel .ng-dropdown-panel-items .ng-option.ng-option-selected {
      background: #eef2ff;
      color: #1f2937;
      font-weight: 500;
    }

    .button-row { display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 12px; }
    .summary-row { display: flex; gap: 16px; flex-wrap: wrap; margin: 8px 0 12px; }
    .summary-item { padding: 6px 10px; background: #f6f8fb; border-radius: 6px; }
    .summary-item span { font-weight: 600; margin-right: 6px; }
    .loading-wrap { display: flex; justify-content: center; padding: 24px 0; }
    .table-wrap { overflow: auto; }
    .full-width-table { width: 100%; min-width: 900px; }
    .amount-col { text-align: right; }
    @media (max-width: 1200px) { .form-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
    @media (max-width: 768px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class AccountingJournalReportBaseComponent implements OnInit, AfterViewInit {
  private readonly journalEndpoint = inject(JournalEndpoint);
  private readonly alertService = inject(AlertService);

  @Input() reportType: 'all' | 'income' | 'expense' = 'all';
  @Input() title = 'Accounting Journal Entries Report';
  @Input() subtitle = 'Day Book style listing';

  @ViewChild(MatSort) sort?: MatSort;
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  loadingIndicator = signal(false);
  accounts = signal<JournalAccountLookup[]>([]);
  rowsCache = signal<JournalListLine[]>([]);

  readonly selectAccountOption = '--Select Account--';
  readonly pageSize = 10;

  dateFrom = this.startOfDay(new Date());
  dateTo = this.startOfDay(new Date());
  selectedAccountNo = this.selectAccountOption;
  searchText = '';
  currentPage = 0;

  sortState: { active: string; direction: 'asc' | 'desc' } = {
    active: 'sn',
    direction: 'asc'
  };

  readonly displayedColumns = ['sn', 'tranDate', 'accountName', 'accountNo', 'description', 'debit', 'credit', 'tranNo'];

  accountOptions = computed(() => this.accounts().filter(x => this.matchesGroupByAccountNo(x.accountNo ?? '')));

  filteredRows = computed(() => {
    const accountNo = (this.selectedAccountNo ?? '').trim();
    const rows = this.rowsCache().filter(x => this.matchesGroupByAccountNo(x.accountNo ?? ''));

    if (!accountNo || accountNo === '(ALL)' || accountNo === this.selectAccountOption) {
      return rows;
    }

    return rows.filter(x => (x.accountNo ?? '').trim() === accountNo);
  });

  sortedRows = computed(() => {
    const rows = [...this.filteredRows()];
    const { active, direction } = this.sortState;
    const factor = direction === 'desc' ? -1 : 1;

    return rows.sort((a, b) => {
      let left: string | number = '';
      let right: string | number = '';

      switch (active) {
        case 'sn':
          left = Number(a.sn) || 0;
          right = Number(b.sn) || 0;
          break;
        case 'tranDate':
          left = new Date(a.tranDate).getTime() || 0;
          right = new Date(b.tranDate).getTime() || 0;
          break;
        case 'accountName':
          left = (a.accountName ?? '').toLowerCase();
          right = (b.accountName ?? '').toLowerCase();
          break;
        case 'accountNo':
          left = (a.accountNo ?? '').toLowerCase();
          right = (b.accountNo ?? '').toLowerCase();
          break;
        case 'description':
          left = (a.description ?? '').toLowerCase();
          right = (b.description ?? '').toLowerCase();
          break;
        case 'debit':
          left = Number(a.debit) || 0;
          right = Number(b.debit) || 0;
          break;
        case 'credit':
          left = Number(a.credit) || 0;
          right = Number(b.credit) || 0;
          break;
        case 'tranNo':
          left = (a.tranNo ?? '').toLowerCase();
          right = (b.tranNo ?? '').toLowerCase();
          break;
      }

      if (left < right) return -1 * factor;
      if (left > right) return 1 * factor;
      return 0;
    });
  });

  pagedRows = computed(() => {
    const start = this.currentPage * this.pageSize;
    return this.sortedRows().slice(start, start + this.pageSize);
  });

  totalDebit = computed(() => this.filteredRows().reduce((sum, row) => sum + (Number(row.debit) || 0), 0));
  totalCredit = computed(() => this.filteredRows().reduce((sum, row) => sum + (Number(row.credit) || 0), 0));

  ngOnInit(): void {
    this.loadAccounts();
    this.runReport();
  }

  ngAfterViewInit(): void {
    this.ensureValidPageIndex();
  }

  onSortChange(sort: Sort): void {
    this.sortState = {
      active: sort.active || 'sn',
      direction: (sort.direction as 'asc' | 'desc') || 'asc'
    };
    this.currentPage = 0;
    this.ensureValidPageIndex();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex;
  }

  onAccountChanged(): void {
    this.currentPage = 0;
    this.ensureValidPageIndex();
  }

  async runReport(): Promise<void> {
    if (!this.dateFrom || !this.dateTo) {
      this.alertService.showMessage('Validation', 'Start Date and End Date are required.', MessageSeverity.warn);
      return;
    }

    if (this.startOfDay(this.dateFrom).getTime() > this.startOfDay(this.dateTo).getTime()) {
      this.alertService.showMessage('Validation', 'Start Date cannot be after End Date.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator.set(true);
    this.alertService.startLoadingMessage('Loading journal entries report...');

    try {
      const rows = await this.loadAllJournalLines();
      this.rowsCache.set(rows);
      this.currentPage = 0;
      this.ensureValidPageIndex();
      this.alertService.stopLoadingMessage();
    } catch (error) {
      this.rowsCache.set([]);
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load journal entries report.\r\nError: "${this.getErrorMessage(error)}"`,
        MessageSeverity.error,
        error
      );
    } finally {
      this.loadingIndicator.set(false);
    }
  }

  clearFilters(): void {
    const today = this.startOfDay(new Date());
    this.dateFrom = today;
    this.dateTo = today;
    this.selectedAccountNo = this.selectAccountOption;
    this.searchText = '';
    this.currentPage = 0;
    void this.runReport();
  }

  formatDate(value: string | Date | undefined): string {
    if (!value) return '';
    const d = value instanceof Date ? value : new Date(value);
    if (isNaN(d.getTime())) return '';
    const day = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${d.getFullYear()}`;
  }

  private matchesGroupByAccountNo(accountNo: string): boolean {
    const normalized = accountNo.trim();
    if (!normalized) return this.reportType === 'all';

    if (this.reportType === 'income') {
      return normalized.startsWith('4');
    }

    if (this.reportType === 'expense') {
      return normalized.startsWith('5');
    }

    return true;
  }

  private async loadAccounts(): Promise<void> {
    try {
      const accounts = await firstValueFrom(this.journalEndpoint.getJournalAccountsEndpoint<JournalAccountLookup[]>());
      this.accounts.set((accounts ?? []).filter(x => !!x.accountNo && !!x.accountName));
    } catch (error) {
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load accounts.\r\nError: "${this.getErrorMessage(error)}"`,
        MessageSeverity.warn,
        error
      );
    }
  }

  private async loadAllJournalLines(): Promise<JournalListLine[]> {
    const pageSize = 200;
    const search = this.searchText?.trim() || undefined;
    const fromDate = this.toIsoDate(this.dateFrom);
    const toDate = this.toIsoDate(this.dateTo);

    let page = 1;
    let totalCount = 0;
    const allRows: JournalListLine[] = [];

    do {
      const result = await firstValueFrom(
        this.journalEndpoint.getJournalEntryLinesEndpoint<PagedJournalLinesResult>({
          search,
          fromDate,
          toDate,
          page,
          pageSize
        })
      );

      totalCount = result?.totalCount ?? 0;
      const items = result?.items ?? [];
      allRows.push(...items);

      if (items.length === 0) {
        break;
      }

      page++;
    } while (allRows.length < totalCount);

    return allRows;
  }

  private toIsoDate(value: Date): string {
    return `${value.getFullYear()}-${(value.getMonth() + 1).toString().padStart(2, '0')}-${value.getDate().toString().padStart(2, '0')}`;
  }

  private startOfDay(date: Date): Date {
    const d = new Date(date);
    d.setHours(0, 0, 0, 0);
    return d;
  }

  private ensureValidPageIndex(): void {
    const total = this.filteredRows().length;
    const maxPage = Math.max(Math.ceil(total / this.pageSize) - 1, 0);
    if (this.currentPage > maxPage) {
      this.currentPage = maxPage;
      if (this.paginator) {
        this.paginator.pageIndex = this.currentPage;
      }
    }
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (error && typeof error === 'object' && 'message' in error) {
      return String((error as { message?: unknown }).message ?? error);
    }
    return String(error);
  }
}
