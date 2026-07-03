import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MatNativeDateModule,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AccountService } from '../../../services/account.service';
import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';
import { Permissions } from '../../../models/permission.model';
import { ExpenseDialogComponent } from './expense-dialog.component';
import {
  ExpenseEntry,
  ExpenseListItem,
  ExpenseViewMode,
  PagedExpenseResult,
} from '../../../models/accounting/expense.model';

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
  selector: 'app-expenses',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    TranslateModule,
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter }
  ],
  template: `
    <div class="page-shell" @fadeInOut>
      <div class="page-header">
        <div class="page-title-block">
          <mat-icon class="page-icon">payments</mat-icon>
          <div>
            <h2 class="page-title">{{ 'expenses.PageTitle' | translate }}</h2>
            <p class="page-subtitle">{{ 'expenses.Subtitle' | translate }}</p>
          </div>
        </div>
        <div class="page-actions">
          <button mat-flat-button color="primary" (click)="openNewDialog()" [disabled]="loadingIndicator || !canManageExpenses">
            <mat-icon>add</mat-icon>
            {{ 'expenses.AddExpense' | translate }}
          </button>
        </div>
      </div>

      <section class="filter-card">
        <div class="filter-grid">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'expenses.Search' | translate }}</mat-label>
            <input matInput [(ngModel)]="searchText" (keyup.enter)="onApplyFilters()" />
            <mat-icon matSuffix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>{{ 'expenses.FromDate' | translate }}</mat-label>
            <input matInput [matDatepicker]="fromPicker" [(ngModel)]="fromDate" />
            <mat-datepicker-toggle matIconSuffix [for]="fromPicker"></mat-datepicker-toggle>
            <mat-datepicker #fromPicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>{{ 'expenses.ToDate' | translate }}</mat-label>
            <input matInput [matDatepicker]="toPicker" [(ngModel)]="toDate" />
            <mat-datepicker-toggle matIconSuffix [for]="toPicker"></mat-datepicker-toggle>
            <mat-datepicker #toPicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>{{ 'expenses.ViewMode' | translate }}</mat-label>
            <select matNativeControl [(ngModel)]="viewMode">
              <option value="all">{{ 'expenses.ViewAll' | translate }}</option>
              <option value="unposted">{{ 'expenses.ViewUnposted' | translate }}</option>
              <option value="posted">{{ 'expenses.ViewPosted' | translate }}</option>
            </select>
          </mat-form-field>
        </div>

        <div class="filter-actions">
          <button mat-stroked-button color="primary" (click)="onApplyFilters()" [disabled]="loadingIndicator">
            <mat-icon>filter_list</mat-icon>
            {{ 'expenses.Apply' | translate }}
          </button>
          <button mat-button (click)="onClearFilters()" [disabled]="loadingIndicator">
            <mat-icon>clear</mat-icon>
            {{ 'expenses.Clear' | translate }}
          </button>
        </div>
      </section>

      <section class="table-card">
        @if (loadingIndicator) {
          <div class="table-spinner">
            <mat-progress-spinner diameter="40" mode="indeterminate"></mat-progress-spinner>
          </div>
        }

        <table mat-table [dataSource]="rows" [trackBy]="trackBySNo" class="expenses-table">
          <ng-container matColumnDef="sNo">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.SNo' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="mono">{{ row.sNo }}</td>
          </ng-container>

          <ng-container matColumnDef="tranDate">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.TranDate' | translate }}</th>
            <td mat-cell *matCellDef="let row">{{ row.tranDate | date:'dd-MMM-yyyy' }}</td>
          </ng-container>

          <ng-container matColumnDef="debitAccountName">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.ExpenseAccount' | translate }}</th>
            <td mat-cell *matCellDef="let row">
              <div class="account-cell">
                <span class="account-name">{{ row.debitAccountName }}</span>
                <span class="account-no mono">{{ row.accountDebit }}</span>
              </div>
            </td>
          </ng-container>

          <ng-container matColumnDef="creditAccountName">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.PayingAccount' | translate }}</th>
            <td mat-cell *matCellDef="let row">
              <div class="account-cell">
                <span class="account-name">{{ row.creditAccountName }}</span>
                <span class="account-no mono">{{ row.accountCredit }}</span>
              </div>
            </td>
          </ng-container>

          <ng-container matColumnDef="amount">
            <th mat-header-cell *matHeaderCellDef class="num">{{ 'expenses.Amount' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="num">{{ row.amount | number:'1.2-2' }}</td>
          </ng-container>

          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.Description' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="ellipsis">{{ row.description || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>{{ 'expenses.Status' | translate }}</th>
            <td mat-cell *matCellDef="let row">
              <span class="status-pill" [class.posted]="row.isPost" [class.unposted]="!row.isPost">
                {{ row.isPost ? ('expenses.Posted' | translate) : ('expenses.Unposted' | translate) }}
              </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef class="action-col">{{ 'expenses.Actions' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="action-col">
              <button mat-icon-button color="primary" (click)="openEditDialog(row)" [disabled]="!canEdit(row)" aria-label="Edit expense">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deleteExpense(row)" [disabled]="!canDelete(row)" aria-label="Delete expense">
                <mat-icon>delete_outline</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>

          <tr class="mat-row no-data" *matNoDataRow>
            <td class="mat-cell empty-cell" [attr.colspan]="displayedColumns.length">
              @if (!loadingIndicator) {
                <mat-icon>inbox</mat-icon>
                <p>{{ 'expenses.NoData' | translate }}</p>
              }
            </td>
          </tr>
        </table>

        <mat-paginator
          [length]="totalCount"
          [pageSize]="pageSize"
          [pageIndex]="currentPage - 1"
          [pageSizeOptions]="[10, 25, 50, 100]"
          (page)="onPageChange($event)"
          [disabled]="loadingIndicator">
        </mat-paginator>
      </section>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; display: flex; flex-direction: column; gap: 16px; }
    .page-header { display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 12px; }
    .page-title-block { display: flex; align-items: center; gap: 12px; }
    .page-icon { color: #3f51b5; font-size: 32px; height: 32px; width: 32px; }
    .page-title { margin: 0; font-size: 1.5rem; font-weight: 600; }
    .page-subtitle { margin: 0; color: rgba(0,0,0,.6); font-size: .85rem; }
    .filter-card, .table-card { background: #fff; border-radius: 8px; border: 1px solid rgba(0,0,0,.06); padding: 16px; }
    .filter-grid { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: 12px; }
    .filter-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 12px; }
    .table-card { position: relative; }
    .table-spinner { position: absolute; inset: 0; display: flex; align-items: center; justify-content: center; background: rgba(255,255,255,.6); z-index: 2; }
    .expenses-table { width: 100%; }
    .expenses-table th.mat-header-cell { background: #f5f5f7; color: rgba(0,0,0,.7); font-weight: 600; font-size: .78rem; text-transform: uppercase; letter-spacing: .04em; }
    .expenses-table td.mat-cell, .expenses-table th.mat-header-cell { padding: 8px 12px; }
    .mono { font-family: 'Consolas', 'Menlo', monospace; font-size: .85rem; }
    .num { text-align: right; font-variant-numeric: tabular-nums; }
    .account-cell { display: flex; flex-direction: column; line-height: 1.2; }
    .account-name { font-weight: 500; }
    .account-no { color: rgba(0,0,0,.55); font-size: .75rem; }
    .ellipsis { max-width: 220px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .status-pill { display: inline-flex; align-items: center; padding: 4px 10px; border-radius: 999px; font-size: .75rem; font-weight: 600; }
    .status-pill.posted { background: rgba(46,125,50,.12); color: #2e7d32; }
    .status-pill.unposted { background: rgba(239,108,0,.12); color: #ef6c00; }
    .action-col { width: 110px; text-align: right; }
    .empty-cell { text-align: center; padding: 32px 16px; color: rgba(0,0,0,.5); }
    .empty-cell mat-icon { font-size: 36px; height: 36px; width: 36px; opacity: .6; }
    .empty-cell p { margin: 8px 0 0; }
    @media (max-width: 992px) { .page-shell { padding: 16px; } .filter-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
    @media (max-width: 575.98px) { .page-shell { padding: 12px; } .filter-grid { grid-template-columns: 1fr; } .page-title { font-size: 1.2rem; } .ellipsis { max-width: 140px; } }
  `],
  animations: [fadeInOut]
})
export class ExpensesComponent implements OnInit {
  private expenseEndpoint = inject(ExpenseEndpoint);
  private alertService = inject(AlertService);
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);

  readonly displayedColumns = ['sNo', 'tranDate', 'debitAccountName', 'creditAccountName', 'amount', 'description', 'status', 'actions'];
  readonly pageSize = 10;

  rows = new MatTableDataSource<ExpenseListItem>([]);
  rowsCache: ExpenseListItem[] = [];

  loadingIndicator = false;
  currentPage = 1;
  totalCount = 0;

  searchText = '';
  fromDate: Date = this.startOfDay(new Date());
  toDate: Date = this.startOfDay(new Date());
  viewMode: ExpenseViewMode = 'all';

  get canManageExpenses(): boolean {
    return this.accountService.userHasPermission(Permissions.manageAccounting);
  }

  ngOnInit(): void {
    this.loadData();
  }

  onApplyFilters(): void {
    this.currentPage = 1;
    this.loadData();
  }

  onClearFilters(): void {
    this.searchText = '';
    this.fromDate = this.startOfDay(new Date());
    this.toDate = this.startOfDay(new Date());
    this.viewMode = 'all';
    this.currentPage = 1;
    this.loadData();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.loadData();
  }

  openNewDialog(): void {
    if (!this.canManageExpenses) {
      this.alertService.showMessage('Access Denied', 'You do not have permission to manage expenses.', MessageSeverity.warn);
      return;
    }

    this.openDialog(null);
  }

  openEditDialog(row: ExpenseListItem): void {
    if (!this.canEdit(row)) {
      return;
    }

    this.loadingIndicator = true;
    this.expenseEndpoint.getExpenseByIdEndpoint<ExpenseEntry>(row.sNo).subscribe({
      next: entry => this.openDialog(entry),
      error: error => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingIndicator = false;
      }
    });
  }

  deleteExpense(row: ExpenseListItem): void {
    if (!this.canDelete(row)) {
      return;
    }

    this.alertService.showDialog(
      `Delete expense entry ${row.sNo}?`,
      1,
      () => this.deleteExpenseConfirmed(row),
    );
  }

  canEdit(row: ExpenseListItem): boolean {
    return this.canManageExpenses && !row.isPost && !row.isClose;
  }

  canDelete(row: ExpenseListItem): boolean {
    return this.canManageExpenses && !row.isPost && !row.isClose;
  }

  trackBySNo(_index: number, item: ExpenseListItem): number {
    return item.sNo;
  }

  private openDialog(entry: ExpenseEntry | null): void {
    const ref = this.dialog.open(ExpenseDialogComponent, {
      data: { entry },
      width: '900px',
      maxWidth: '95vw',
      disableClose: true,
      autoFocus: 'first-tabbable',
      restoreFocus: true,
    });

    ref.afterClosed().subscribe(result => {
      if (result?.saved) {
        this.loadData();
      }
    });
  }

  private deleteExpenseConfirmed(row: ExpenseListItem): void {
    this.loadingIndicator = true;
    this.expenseEndpoint.getDeleteExpenseEndpoint<void>(row.sNo).subscribe({
      next: () => {
        this.alertService.showMessage('Success', 'Expense deleted successfully.', MessageSeverity.success);
        this.loadData();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.expenseEndpoint.getExpensesEndpoint<PagedExpenseResult>({
      search: this.searchText.trim() || undefined,
      fromDate: this.fromDate?.toISOString(),
      toDate: this.toDate?.toISOString(),
      viewMode: this.viewMode,
      page: this.currentPage,
      pageSize: this.pageSize,
    }).subscribe({
      next: result => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        const items = result?.items ?? [];
        this.totalCount = result?.totalCount ?? 0;
        this.rowsCache = [...items];
        this.rows.data = items;
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.rows.data = [];
        this.rowsCache = [];
        this.totalCount = 0;
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private getErrorMessage(error: unknown): string {
    const err = (error ?? {}) as { error?: { errors?: Record<string, string[]>; title?: string }; message?: string };
    const errors = err.error?.errors ? Object.values(err.error.errors).flat() : [];
    return errors[0] ?? err.error?.title ?? err.message ?? 'Unknown error';
  }

  private startOfDay(date: Date): Date {
    const copy = new Date(date);
    copy.setHours(0, 0, 0, 0);
    return copy;
  }
}
