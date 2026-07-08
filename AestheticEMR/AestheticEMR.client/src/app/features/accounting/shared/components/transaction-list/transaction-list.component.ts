import { AfterViewInit, Component, Input, OnInit, ViewChild, effect, inject, signal } from '@angular/core';
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

import { fadeInOut } from '../../../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../../../services/alert.service';
import { AccountService } from '../../../../../services/account.service';
import { AppConfigService } from '../../../../../services/app-config.service';
import { Permissions } from '../../../../../models/permission.model';
import {
  TransactionConfig,
  TransactionListItem,
  TransactionDialogData,
} from '../../models/transaction-config.interface';
import { TransactionDialogComponent } from '../transaction-dialog/transaction-dialog.component';

@Component({
  selector: 'app-transaction-list',
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
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.scss',
  animations: [fadeInOut]
})
export class TransactionListComponent implements OnInit, AfterViewInit {
  @Input() config!: TransactionConfig;

  private accountService = inject(AccountService);
  private alertService = inject(AlertService);
  private dialog = inject(MatDialog);
  private appConfig = inject(AppConfigService);

  private skipNextSearchEffect = true;

  readonly displayedColumns = ['sNo', 'sn', 'tranNo', 'tranDate', 'account', 'debit', 'credit', 'description', 'period', 'actions'];
  readonly pageSize = 10;

  rows = new MatTableDataSource<TransactionListItem>([]);
  rowsCache: TransactionListItem[] = [];

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

  get canManageTransactions(): boolean {
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
    if (!this.canManageTransactions) {
      this.alertService.showMessage('Access Denied', 'You do not have permission to manage transactions.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator = true;
    this.config.nextTranIdEndpoint().subscribe({
      next: result => {
        this.openDialog({
          entry: null,
          tranId: result?.tranId ?? '',
          isEdit: false,
          config: this.config
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

  openEditDialog(row: TransactionListItem): void {
    const tranId = row.tranNo?.trim();
    if (!tranId) {
      this.alertService.showMessage('Validation', 'Transaction id is required for editing.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator = true;
    this.config.entriesByTranIdEndpoint(tranId).subscribe({
      next: entries => {
        const loadedEntries = entries ?? [];
        this.openDialog({
          entry: loadedEntries[0] ?? null,
          entries: loadedEntries,
          tranId,
          isEdit: true,
          config: this.config
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

  deleteTransaction(row: TransactionListItem): void {
    const tranId = row.tranNo?.trim();
    const period = row.period?.trim();
    const coyID = row.coyID?.trim();

    if (!tranId) {
      this.alertService.showMessage('Validation', 'Transaction id is required for delete.', MessageSeverity.warn);
      return;
    }

    if (!period || !coyID) {
      this.alertService.showMessage('Validation', 'Period and CoyID are required for delete.', MessageSeverity.warn);
      return;
    }

    this.alertService.showDialog(
      `Delete transaction ${tranId}?\n\nThis action cannot be undone.`,
      DialogType.confirm,
      () => this.confirmDelete(tranId, period, coyID)
    );
  }

  trackBySNo(_index: number, item: TransactionListItem): number {
    return item.sNo;
  }

  private confirmDelete(tranId: string, period: string, coyID: string): void {
    this.loadingIndicator = true;
    this.config.deleteTranIdEndpoint(tranId, period, coyID).subscribe({
      next: () => {
        this.alertService.showMessage('Deleted', 'Transaction has been removed.', MessageSeverity.success);
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

  private openDialog(data: TransactionDialogData): void {
    const ref = this.dialog.open(TransactionDialogComponent, {
      data,
      width: '1100px',
      maxWidth: '95vw',
      disableClose: true,
      autoFocus: 'first-tabbable',
      restoreFocus: true,
    });

    ref.afterClosed().subscribe(result => {
      if (result?.saved) {
        this.currentPage = 1;

        const savedTranId = result?.tranId?.trim();
        if (savedTranId) {
          this.skipNextSearchEffect = true;
          this.searchText.set(savedTranId);
        }

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

    this.config.listEndpoint({
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
