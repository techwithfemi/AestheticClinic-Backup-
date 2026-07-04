import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AccountService } from '../../../services/account.service';
import { ChartOfAccountEndpoint } from '../../../services/chart-of-account-endpoint.service';
import { Permissions } from '../../../models/permission.model';
import {
  ChartOfAccountDefaults,
  ChartOfAccountEntry,
  ChartOfAccountGroupLookup,
  ChartOfAccountListItem,
  PagedChartOfAccountResult,
} from '../../../models/accounting/chart-of-account.model';
import { ChartOfAccountDialogComponent } from './chart-of-account-dialog.component';

@Component({
  selector: 'app-chart-of-accounts',
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
          <mat-icon class="page-icon">account_tree</mat-icon>
          <div>
            <h2 class="page-title">{{ 'chartOfAccounts.PageTitle' | translate }}</h2>
            <p class="page-subtitle">{{ 'chartOfAccounts.Subtitle' | translate }}</p>
          </div>
        </div>
        <div class="page-actions">
          <button mat-flat-button color="primary" (click)="openNewDialog()" [disabled]="loadingIndicator || !canManage">
            <mat-icon>add</mat-icon>
            {{ 'chartOfAccounts.AddAccount' | translate }}
          </button>
        </div>
      </div>

      <section class="filter-card">
        <div class="filter-grid one-col">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'chartOfAccounts.Search' | translate }}</mat-label>
            <input matInput [ngModel]="searchText" (ngModelChange)="onSearchTextChanged($event)" />
            <mat-icon matSuffix>search</mat-icon>
          </mat-form-field>
        </div>

        <div class="filter-actions">
          <button mat-button (click)="onClearFilters()" [disabled]="loadingIndicator">
            <mat-icon>clear</mat-icon>
            {{ 'chartOfAccounts.Clear' | translate }}
          </button>
          <button mat-button (click)="onRefresh()" [disabled]="loadingIndicator">
            <mat-icon>refresh</mat-icon>
            {{ 'chartOfAccounts.Refresh' | translate }}
          </button>
        </div>
      </section>

      <section class="table-card">
        @if (loadingIndicator) {
          <div class="table-spinner">
            <mat-progress-spinner diameter="40" mode="indeterminate"></mat-progress-spinner>
          </div>
        }

        <table mat-table [dataSource]="rows" [trackBy]="trackBySNo" matSort [matSortActive]="sortBy" [matSortDirection]="sortDirection" (matSortChange)="onSortChange($event)" class="coa-table">
          <ng-container matColumnDef="accountNo">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountNo">{{ 'chartOfAccounts.AccountNo' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="mono">{{ row.accountNo }}</td>
          </ng-container>

          <ng-container matColumnDef="accountName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountName">{{ 'chartOfAccounts.AccountName' | translate }}</th>
            <td mat-cell *matCellDef="let row">{{ row.accountName }}</td>
          </ng-container>

          <ng-container matColumnDef="groupName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="groupName">{{ 'chartOfAccounts.AccountGroup' | translate }}</th>
            <td mat-cell *matCellDef="let row">{{ row.groupName }}</td>
          </ng-container>

          <ng-container matColumnDef="accountDesc">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountDesc">{{ 'chartOfAccounts.Description' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="ellipsis">{{ row.accountDesc || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="accountOpAmt">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountOpAmt" class="num">{{ 'chartOfAccounts.OpeningBal' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="num">{{ row.accountOpAmt | number:'1.2-2' }}</td>
          </ng-container>

          <ng-container matColumnDef="accountClAmt">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="accountClAmt" class="num">{{ 'chartOfAccounts.ClosingBal' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="num">{{ row.accountClAmt | number:'1.2-2' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef class="action-col">{{ 'chartOfAccounts.Actions' | translate }}</th>
            <td mat-cell *matCellDef="let row" class="action-col">
              <button mat-icon-button color="primary" (click)="openEditDialog(row)" [disabled]="!canManage" aria-label="Edit account">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deleteAccount(row)" [disabled]="!canManage" aria-label="Delete account">
                <mat-icon>delete_outline</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns" (dblclick)="openEditDialog(row)"></tr>

          <tr class="mat-row no-data" *matNoDataRow>
            <td class="mat-cell empty-cell" [attr.colspan]="displayedColumns.length">
              @if (!loadingIndicator) {
                <mat-icon>inbox</mat-icon>
                <p>{{ 'chartOfAccounts.NoData' | translate }}</p>
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
    .filter-grid { display: grid; gap: 12px; }
    .filter-grid.one-col { grid-template-columns: 1fr; }
    .filter-actions { display: flex; justify-content: flex-end; flex-wrap: wrap; gap: 8px; margin-top: 12px; }
    .table-card { position: relative; }
    .table-spinner { position: absolute; inset: 0; display: flex; align-items: center; justify-content: center; background: rgba(255,255,255,.6); z-index: 2; }
    .coa-table { width: 100%; }
    .coa-table th.mat-header-cell { background: #f5f5f7; color: rgba(0,0,0,.7); font-weight: 600; font-size: .78rem; text-transform: uppercase; letter-spacing: .04em; }
    .coa-table td.mat-cell, .coa-table th.mat-header-cell { padding: 8px 12px; }
    .mono { font-family: 'Consolas', 'Menlo', monospace; font-size: .85rem; }
    .num { text-align: right; font-variant-numeric: tabular-nums; }
    .ellipsis { max-width: 220px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .action-col { width: 110px; text-align: right; }
    .empty-cell { text-align: center; padding: 32px 16px; color: rgba(0,0,0,.5); }
    .empty-cell mat-icon { font-size: 36px; height: 36px; width: 36px; opacity: .6; }
    .empty-cell p { margin: 8px 0 0; }
    @media (max-width: 992px) { .page-shell { padding: 16px; } }
    @media (max-width: 575.98px) { .page-shell { padding: 12px; } .page-title { font-size: 1.2rem; } .ellipsis { max-width: 140px; } }
  `],
  animations: [fadeInOut],
})
export class ChartOfAccountsComponent implements OnInit, OnDestroy {
  private endpoint = inject(ChartOfAccountEndpoint);
  private alertService = inject(AlertService);
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);
  private readonly destroy$ = new Subject<void>();
  private readonly searchChanged$ = new Subject<string>();
  private readonly gridStateKey = 'chartOfAccounts.gridState';

  readonly displayedColumns = ['accountNo', 'accountName', 'groupName', 'accountDesc', 'accountOpAmt', 'accountClAmt', 'actions'];
  readonly pageSize = 10;

  rows = new MatTableDataSource<ChartOfAccountListItem>([]);
  rowsCache: ChartOfAccountListItem[] = [];

  loadingIndicator = false;
  currentPage = 1;
  totalCount = 0;
  searchText = '';

  sortBy = 'accountNo';
  sortDirection: 'asc' | 'desc' = 'asc';

  defaults: ChartOfAccountDefaults = {
    autoAccountNo: 'YES',
    receiveExtData: 'NO',
    receiveArData: 'NO',
    receiveApData: 'NO',
    receiveExpenseData: 'NO',
    receivePayrollData: 'NO',
  };
  groups: ChartOfAccountGroupLookup[] = [];

  get canManage(): boolean {
    return this.accountService.userHasPermission(Permissions.manageAccounting);
  }

  ngOnInit(): void {
    this.restoreGridState();

    this.searchChanged$
      .pipe(
        debounceTime(350),
        distinctUntilChanged(),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.currentPage = 1;
        this.persistGridState();
        this.loadData();
      });

    this.loadLookupsAndData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchTextChanged(value: string): void {
    this.searchText = value ?? '';
    this.searchChanged$.next(this.searchText.trim());
  }

  onClearFilters(): void {
    this.searchText = '';
    this.currentPage = 1;
    this.persistGridState();
    this.loadData();
  }

  onRefresh(): void {
    this.persistGridState();
    this.loadLookupsAndData();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.persistGridState();
    this.loadData();
  }

  onSortChange(sort: Sort): void {
    this.sortBy = sort.active || 'accountNo';
    this.sortDirection = (sort.direction || 'asc') as 'asc' | 'desc';
    this.currentPage = 1;
    this.persistGridState();
    this.loadData();
  }

  openNewDialog(): void {
    if (!this.canManage) {
      this.alertService.showMessage('Access Denied', 'You do not have permission to manage chart of accounts.', MessageSeverity.warn);
      return;
    }

    this.openDialog(null);
  }

  openEditDialog(row: ChartOfAccountListItem): void {
    if (!this.canManage) {
      return;
    }

    this.loadingIndicator = true;
    this.endpoint.getChartOfAccountByIdEndpoint<ChartOfAccountEntry>(row.sNo).subscribe({
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

  deleteAccount(row: ChartOfAccountListItem): void {
    if (!this.canManage) {
      return;
    }

    this.alertService.showDialog(
      `Are you sure to Delete this Account (${row.accountNo})`,
      1,
      () => this.deleteAccountConfirmed(row),
    );
  }

  trackBySNo(_index: number, item: ChartOfAccountListItem): number {
    return item.sNo;
  }

  private openDialog(entry: ChartOfAccountEntry | null): void {
    const ref = this.dialog.open(ChartOfAccountDialogComponent, {
      data: {
        entry,
        groups: this.groups,
        defaults: this.defaults,
      },
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

  private deleteAccountConfirmed(row: ChartOfAccountListItem): void {
    this.loadingIndicator = true;
    this.endpoint.getDeleteChartOfAccountEndpoint<void>(row.sNo).subscribe({
      next: () => {
        this.alertService.showMessage('Success', 'Record succesfully Deleted', MessageSeverity.success);
        this.loadData();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private loadLookupsAndData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.endpoint.getChartOfAccountDefaultsEndpoint<ChartOfAccountDefaults>().subscribe({
      next: defaults => {
        this.defaults = defaults ?? this.defaults;

        this.endpoint.getChartOfAccountGroupsEndpoint<ChartOfAccountGroupLookup[]>().subscribe({
          next: groups => {
            this.groups = groups ?? [];
            this.loadData();
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
          }
        });
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.endpoint.getChartOfAccountsEndpoint<PagedChartOfAccountResult>({
      search: this.searchText.trim() || undefined,
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortDirection: this.sortDirection,
    }).subscribe({
      next: result => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        const items = result?.items ?? [];
        this.totalCount = result?.totalCount ?? 0;
        this.rowsCache = [...items];
        this.rows.data = items;
        this.persistGridState();
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

  private restoreGridState(): void {
    const raw = sessionStorage.getItem(this.gridStateKey);
    if (!raw) {
      return;
    }

    try {
      const state = JSON.parse(raw) as {
        searchText?: string;
        currentPage?: number;
        sortBy?: string;
        sortDirection?: 'asc' | 'desc';
      };

      this.searchText = state.searchText ?? '';
      this.currentPage = state.currentPage && state.currentPage > 0 ? state.currentPage : 1;
      this.sortBy = state.sortBy || 'accountNo';
      this.sortDirection = state.sortDirection === 'desc' ? 'desc' : 'asc';
    } catch {
      sessionStorage.removeItem(this.gridStateKey);
    }
  }

  private persistGridState(): void {
    const state = {
      searchText: this.searchText,
      currentPage: this.currentPage,
      sortBy: this.sortBy,
      sortDirection: this.sortDirection,
    };

    sessionStorage.setItem(this.gridStateKey, JSON.stringify(state));
  }

  private getErrorMessage(error: unknown): string {
    const err = (error ?? {}) as { error?: { errors?: Record<string, string[]>; title?: string }; message?: string };
    const modelErrors = err.error?.errors;

    if (modelErrors) {
      for (const key of Object.keys(modelErrors)) {
        const messages = modelErrors[key];
        if (messages?.length) {
          return messages[0];
        }
      }
    }

    return err.error?.title ?? err.message ?? 'Unknown error';
  }
}
