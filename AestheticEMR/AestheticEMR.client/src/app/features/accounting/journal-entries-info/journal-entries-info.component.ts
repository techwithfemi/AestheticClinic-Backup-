import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { AccountService } from '../../../services/account.service';
import {
  JournalEntry,
  JournalListLine,
  PagedJournalLinesResult,
} from '../../../models/accounting/journal-entry.model';
import {
  JournalEntryDialogComponent,
} from '../journal-entry-dialog/journal-entry-dialog.component';

@Component({
  selector: 'app-journal-entries-info',
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
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule,
  ],
  templateUrl: './journal-entries-info.component.html',
  styleUrl: './journal-entries-info.component.scss',
  animations: [fadeInOut],
})
export class JournalEntriesInfoComponent implements OnInit {
  private journalEndpoint = inject(JournalEndpoint);
  private alertService = inject(AlertService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private accountService = inject(AccountService);

  // Only the 'Management' role can delete journal entries.
  // Case-insensitive on purpose — role strings drift in the wild
  // (see MEMORY.md → "Authorization pattern (after journal-entries-info 403)").
  get canDelete(): boolean {
    const roles = this.accountService.currentUser?.roles ?? [];
    return roles.some(r => (r ?? '').trim().toLowerCase() === 'management');
  }

  readonly pageSize = 10;
  // Display order matches the user's preferred vwTranx projection.
  // Slimmed to the eight columns the user wants visible — everything else
  // (tranCat, billNo, costCenter, entryDate, period, userName, sNo, remarks,
  // actions) is intentionally hidden from the grid.
  readonly displayedColumns = [
    'sn',
    'tranDate',
    'accountName',
    'accountNo',
    'debit',
    'credit',
    'description',
    'tranNo',
    'actions',
  ];

  rows = new MatTableDataSource<JournalListLine>([]);
  rowsCache: JournalListLine[] = [];

  currentPage = 1;
  totalCount = 0;
  loadingIndicator = false;

  searchText = '';

  /** Date filter for the default load (today). Cleared when the user searches. */
  filterDate: Date = this.startOfDay(new Date());

  ngOnInit(): void {
    this.loadPage();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadPage();
  }

  onClearFilters(): void {
    this.searchText = '';
    // Reset the implicit date filter back to today.
    this.filterDate = this.startOfDay(new Date());
    this.currentPage = 1;
    this.loadPage();
  }

  onPageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize; // pageSize is server-driven; ignore client pageSize changes
    this.loadPage();
  }

  openNewDialog(): void {
    this.openDialog(null);
  }

  openEditDialog(row: JournalListLine): void {
    const tranNo = row.tranNo;
    if (!tranNo) {
      this.alertService.showMessage('Cannot edit', 'This row has no Tran No.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator = true;
    this.journalEndpoint
      .getJournalEntryEndpoint<JournalEntry>(tranNo)
      .toPromise()
      .then(entry => {
        if (!entry) {
          this.alertService.showMessage('Not found', `Journal entry ${tranNo} was not found.`, MessageSeverity.warn);
          return;
        }
        this.openDialog(entry);
      })
      .catch(err => {
        const msg = err?.error?.title ?? err?.message ?? 'Could not load the journal entry.';
        this.alertService.showStickyMessage('Load failed', msg, MessageSeverity.error, err);
      })
      .finally(() => (this.loadingIndicator = false));
  }

  confirmDelete(row: JournalListLine): void {
    if (!this.canDelete) {
      this.alertService.showMessage(
        'Not allowed',
        'Only the Management role can delete journal entries.',
        MessageSeverity.warn,
      );
      return;
    }

    const tranNo = row.tranNo;
    if (!tranNo) {
      this.alertService.showMessage('Cannot delete', 'This row has no Tran No.', MessageSeverity.warn);
      return;
    }

    const ok = window.confirm(`Delete journal entry ${tranNo}? This cannot be undone.`);
    if (!ok) return;

    this.loadingIndicator = true;
    this.journalEndpoint
      .deleteJournalEntryEndpoint<void>(tranNo)
      .toPromise()
      .then(() => {
        this.snackBar.open(`Journal entry ${tranNo} deleted.`, 'Dismiss', { duration: 4000 });
        this.loadPage();
      })
      .catch(err => {
        const msg = err?.error?.title ?? err?.message ?? 'Could not delete journal entry.';
        this.alertService.showStickyMessage('Delete failed', msg, MessageSeverity.error, err);
      })
      .finally(() => (this.loadingIndicator = false));
  }

  private openDialog(entry: JournalEntry | null): void {
    const ref = this.dialog.open(JournalEntryDialogComponent, {
      data: { entry },
      width: '95vw',
      maxWidth: '1200px',
      disableClose: true, // per AGENTS.md: only X / Cancel closes the dialog
      autoFocus: 'first-tabbable',
      restoreFocus: true,
    });

    ref.afterClosed().subscribe(result => {
      if (result?.saved) {
        this.loadPage();
      }
    });
  }

  private loadPage(): void {
    this.loadingIndicator = true;

    const search = this.searchText?.trim();
    const query = {
      search: search || undefined,
      // Default to today when the user has not entered a search; if they
      // search, the service drops the date filter so they can find any
      // TranNo across all dates.
      tranDate: search ? undefined : this.filterDate.toISOString(),
      page: this.currentPage,
      pageSize: this.pageSize,
    };

    this.journalEndpoint
      .getJournalEntryLinesEndpoint<PagedJournalLinesResult>(query)
      .toPromise()
      .then(result => {
        this.totalCount = result?.totalCount ?? 0;
        const items = result?.items ?? [];
        this.rowsCache = items;
        this.rows.data = items;
      })
      .catch(err => {
        const msg = err?.error?.title ?? err?.message ?? 'Could not load journal entries.';
        this.alertService.showStickyMessage('Load failed', msg, MessageSeverity.error, err);
        this.rows.data = [];
        this.rowsCache = [];
        this.totalCount = 0;
      })
      .finally(() => (this.loadingIndicator = false));
  }

  trackBySNo(_index: number, item: JournalListLine): number {
    return item.sNo;
  }

  private startOfDay(d: Date): Date {
    const copy = new Date(d);
    copy.setHours(0, 0, 0, 0);
    return copy;
  }
}
