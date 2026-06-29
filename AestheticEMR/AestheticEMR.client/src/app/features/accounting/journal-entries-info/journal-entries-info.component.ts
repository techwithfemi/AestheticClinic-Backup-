import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import {
  JournalEntry,
  JournalListItem,
  PagedJournalResult,
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
    MatDatepickerModule,
    MatNativeDateModule,
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

  readonly pageSize = 10;
  readonly displayedColumns = ['tranNo', 'tranDate', 'lineCount', 'totalDebit', 'totalCredit', 'costCenter', 'actions'];

  rows = new MatTableDataSource<JournalListItem>([]);
  rowsCache: JournalListItem[] = [];

  currentPage = 1;
  totalCount = 0;
  loadingIndicator = false;

  searchText = '';
  fromDate: Date | null = null;
  toDate: Date | null = null;

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  ngOnInit(): void {
    this.loadPage();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadPage();
  }

  onClearFilters(): void {
    this.searchText = '';
    this.fromDate = null;
    this.toDate = null;
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

  openEditDialog(item: JournalListItem): void {
    this.loadingIndicator = true;
    this.journalEndpoint
      .getJournalEntryEndpoint<JournalEntry>(item.tranNo)
      .toPromise()
      .then(entry => {
        if (!entry) {
          this.alertService.showMessage('Not found', `Journal entry ${item.tranNo} was not found.`, MessageSeverity.warn);
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

  confirmDelete(item: JournalListItem): void {
    const ok = window.confirm(`Delete journal entry ${item.tranNo}? This cannot be undone.`);
    if (!ok) return;

    this.loadingIndicator = true;
    this.journalEndpoint
      .deleteJournalEntryEndpoint<void>(item.tranNo)
      .toPromise()
      .then(() => {
        this.snackBar.open(`Journal entry ${item.tranNo} deleted.`, 'Dismiss', { duration: 4000 });
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

    const query = {
      search: this.searchText?.trim() || undefined,
      fromDate: this.fromDate?.toISOString() || undefined,
      toDate: this.toDate?.toISOString() || undefined,
      page: this.currentPage,
      pageSize: this.pageSize,
    };

    this.journalEndpoint
      .getJournalEntriesEndpoint<PagedJournalResult>(query)
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

  trackByTranNo(_index: number, item: JournalListItem): string {
    return item.tranNo;
  }
}