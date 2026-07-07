import { AfterViewInit, Component, OnInit, ViewChild, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AccountService } from '../../../services/account.service';
import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';
import { AppConfigService } from '../../../services/app-config.service';
import { Permissions } from '../../../models/permission.model';
import { ExpenseDialogComponent } from './expense-dialog.component';
import {
  ExpenseDialogData,
  ExpenseEntry,
  ExpenseListItem,
  ExpenseTranIdResponse,
  PagedExpenseResult,
} from '../../../models/accounting/expense.model';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    TranslateModule,
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
            <input matInput [ngModel]="searchText()" (ngModelChange)="onSearchTextChanged($event)" />
            <mat-icon matSuffix>search</mat-icon>
          </mat-form-field>
        </div>

        <div class="filter-actions">
          <button mat-button (click)="onClearFilters()" [disabled]="loadingIndicator || !searchText().trim()">
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

        <table mat-table [dataSource]="rows" [trackBy]="trackBySNo" class="expenses-table" matSort matSortActive="sn" matSortDirection="asc" matSortDisableClear>
          <ng-container matColumnDef="sNo">
            <th mat-header-cell *matHeaderCellDef class="hidden-col"></th>
            <td mat-cell *matCellDef="let row" class="hidden-col">{{ row.sNo }}</td>
          </ng-container>

          <ng-container matColumnDef="sn">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="sn">{{ 'expenses.SNo' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="mono">{{ row.sn }}</td>
          </ng-container>

          <ng-container matColumnDef="tranNo">
            <th mat-header-cell *matHeaderCellDef class="hidden-col"></th>
            <td mat-cell *matCellDef="let row" class="hidden-col">{{ row.tranNo }}</td>
          </ng-container>

          <ng-container matColumnDef="tranDate">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="tranDate">{{ 'expenses.TranDate' | translate }}</th>
            <td mat-cell *matCellDef="let row">{{ row.tranDate | date:'dd-MMM-yyyy' }}</td>
          </ng-container>

          <ng-container matColumnDef="account">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountName">{{ 'expenses.AccountName' | translate }}</th>
            <td mat-cell *matCellDef="let row">
              <div class="account-cell">
                <span class="account-name">{{ row.accountName }}</span>
                <span class="account-no mono">{{ row.accountNo }}</span>
              </div>
            </td>
          </ng-container>

          <ng-container matColumnDef="debit">
            <th mat-header-cell *matHeaderCellDef class="num" mat-sort-header="debit">{{ 'expenses.Debit' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="num">{{ row.debit | number:'1.2-2' }}</td>
          </ng-container>

          <ng-container matColumnDef="credit">
            <th mat-header-cell *matHeaderCellDef class="num" mat-sort-header="credit">{{ 'expenses.Credit' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="num">{{ row.credit | number:'1.2-2' }}</td>
          </ng-container>

          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="description">{{ 'expenses.Description' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="ellipsis">{{ row.description }}</td>
          </ng-container>

          <ng-container matColumnDef="period">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="period">{{ 'expenses.Period' | translate }}</th>
            <td mat-cell *matCellDef="let row">{{ row.period }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef class="action-col">{{ 'expenses.Actions' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="action-col">
              <button mat-icon-button color="primary" (click)="openEditDialog(row)" aria-label="Edit expense" [disabled]="loadingIndicator || !canManageExpenses">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deleteExpense(row)" aria-label="Delete expense" [disabled]="loadingIndicator || !canManageExpenses">
                <mat-icon>delete</mat-icon>
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
    .filter-grid { display: grid; grid-template-columns: minmax(240px, 420px); gap: 12px; }
    .filter-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 8px; }
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
    .action-col { width: 126px; text-align: right; }
    .hidden-col { display: none; width: 0; padding: 0 !important; border: 0 !important; }
    .empty-cell { text-align: center; padding: 32px 16px; color: rgba(0,0,0,.5); }
    .empty-cell mat-icon { font-size: 36px; height: 36px; width: 36px; opacity: .6; }
    .empty-cell p { margin: 8px 0 0; }
    @media (max-width: 992px) { .page-shell { padding: 16px; } .filter-grid { grid-template-columns: minmax(200px, 1fr); } }
    @media (max-width: 575.98px) { .page-shell { padding: 12px; } .filter-grid { grid-template-columns: 1fr; } .page-title { font-size: 1.2rem; } .ellipsis { max-width: 140px; } }
  `],
  animations: [fadeInOut]
})
export class ExpensesComponent implements OnInit, AfterViewInit {
  private expenseEndpoint = inject(ExpenseEndpoint);
  private alertService = inject(AlertService);
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);
  private appConfig = inject(AppConfigService);

  private skipNextSearchEffect = true;

  readonly displayedColumns = ['sNo', 'sn', 'tranNo', 'tranDate', 'account', 'debit', 'credit', 'description', 'period', 'actions'];
  readonly pageSize = 10;

  rows = new MatTableDataSource<ExpenseListItem>([]);
  rowsCache: ExpenseListItem[] = [];

  loadingIndicator = false;
  currentPage = 1;
  totalCount = 0;

  searchText = signal('');

  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    effect((onCleanup) => {
      this.searchText();

      if (this.skipNextSearchEffect) {
        this.skipNextSearchEffect = false;
        return;
      }

      const timer = setTimeout(() => {
        this.currentPage = 1;
        this.loadData();
      }, this.appConfig.searchDebounceMs);

      onCleanup(() => clearTimeout(timer));
    });
  }

  get canManageExpenses(): boolean {
    return this.accountService.userHasPermission(Permissions.manageAccounting);
  }

  ngOnInit(): void {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.rows.sort = this.sort;
    this.rows.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'tranDate': return new Date(item.tranDate).getTime();
        case 'accountName': return item.accountName ?? '';
        case 'description': return item.description ?? '';
        case 'period': return item.period ?? '';
        default: return (item as never as Record<string, unknown>)[property] as string | number;
      }
    };
  }

  onSearchTextChanged(value: string): void {
    this.searchText.set(value ?? '');
  }

  onClearFilters(): void {
    this.skipNextSearchEffect = true;
    this.searchText.set('');
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

    this.loadingIndicator = true;
    this.expenseEndpoint.getNextTranIdEndpoint<ExpenseTranIdResponse>().subscribe({
      next: result => {
        this.openDialog({ entry: null, tranId: result?.tranId ?? '', isEdit: false });
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingIndicator = false;
      }
    });
  }

  openEditDialog(row: ExpenseListItem): void {
    const tranId = row.tranNo?.trim();
    if (!tranId) {
      this.alertService.showMessage('Validation', 'Transaction id is required for editing.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator = true;
    this.expenseEndpoint.getExpenseEntriesByTranIdEndpoint<ExpenseEntry[]>(tranId).subscribe({
      next: entries => {
        const loadedEntries = entries ?? [];
        this.openDialog({
          entry: loadedEntries[0] ?? null,
          entries: loadedEntries,
          tranId,
          isEdit: true,
        });
      },
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
    const tranId = row.tranNo?.trim();
    if (!tranId) {
      this.alertService.showMessage('Validation', 'Transaction id is required for delete.', MessageSeverity.warn);
      return;
    }

    this.alertService.showDialog(
      `Delete expense transaction ${tranId}?\n\nThis action cannot be undone.`,
      DialogType.confirm,
      () => this.confirmDelete(tranId)
    );
  }

  trackBySNo(_index: number, item: ExpenseListItem): number {
    return item.sNo;
  }

  private confirmDelete(tranId: string): void {
    this.loadingIndicator = true;
    this.expenseEndpoint.getDeleteExpenseByTranIdEndpoint<void>(tranId).subscribe({
      next: () => {
        this.alertService.showMessage('Deleted', 'Expense transaction has been removed.', MessageSeverity.success);
        this.loadData();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingIndicator = false;
      }
    });
  }

  private openDialog(data: ExpenseDialogData): void {
    const ref = this.dialog.open(ExpenseDialogComponent, {
      data,
      width: '1100px',
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

  private loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    const normalizedSearch = this.searchText().trim();
    const today = this.startOfDay(new Date());
    const useCurrentDateDefault = !normalizedSearch;
    const todayParam = this.toDateParam(today);

    this.expenseEndpoint.getExpensesEndpoint<PagedExpenseResult>({
      search: normalizedSearch || undefined,
      fromDate: useCurrentDateDefault ? todayParam : undefined,
      toDate: useCurrentDateDefault ? todayParam : undefined,
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
        if (this.sort) {
          this.rows.sort = this.sort;
        }
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

  private toDateParam(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
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
