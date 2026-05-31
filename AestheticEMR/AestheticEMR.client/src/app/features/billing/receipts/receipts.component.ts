import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { Receipt } from '../../../models/legacy/receipt.model';
import { ReceiptEntryDialogComponent, ReceiptEntryDialogData } from './receipt-entry-dialog.component';

@Component({
  selector: 'app-receipts',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatDialogModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule
  ],
  animations: [fadeInOut],
  templateUrl: './receipts.component.html',
  styleUrl: './receipts.component.scss'
})
export class ReceiptsComponent implements OnInit {
  private alertService = inject(AlertService);
  private billingEndpoint = inject(BillingEndpoint);
  private dialog = inject(MatDialog);

  receipts: Receipt[] = [];
  receiptsCache: Receipt[] = [];
  filteredReceipts: Receipt[] = [];
  loadingIndicator = false;
  searchText = '';

  currentPage = 1;
  readonly pageSize = 10;

  readonly listColumns = [
    'receiptDate', 'receiptNo', 'billNo', 'patientName',
    'payType', 'amountBilled', 'amountPaid', 'receivedBy', 'actions'
  ];

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredReceipts.length / this.pageSize));
  }

  get pagedReceipts(): Receipt[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredReceipts.slice(start, start + this.pageSize);
  }

  // ─────────────────────────────────────────────
  // Lifecycle
  // ─────────────────────────────────────────────

  ngOnInit(): void {
    this.loadData();
  }

  // ─────────────────────────────────────────────
  // Data loading
  // ─────────────────────────────────────────────

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.billingEndpoint.getReceiptsEndpoint<Receipt[]>().subscribe({
      next: receipts => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.receipts      = receipts ?? [];
        this.receiptsCache = [...this.receipts];
        this.applyFilter();
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to retrieve receipts.\r\nError: "${this.getErrorMessage(error)}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  refresh(): void {
    this.loadData();
  }

  // ─────────────────────────────────────────────
  // Search / filter
  // ─────────────────────────────────────────────

  onSearch(): void {
    this.currentPage = 1;
    this.applyFilter();
  }

  private applyFilter(): void {
    const term = this.searchText.trim().toLowerCase();
    if (!term) {
      this.filteredReceipts = [...this.receiptsCache];
    } else {
      this.filteredReceipts = this.receiptsCache.filter(r =>
        r.receiptNo.toLowerCase().includes(term) ||
        r.billNo.toLowerCase().includes(term) ||
        (r.patientName ?? '').toLowerCase().includes(term) ||
        r.payType.toLowerCase().includes(term)
      );
    }
  }

  // ─────────────────────────────────────────────
  // Pagination
  // ─────────────────────────────────────────────

  prevPage(): void {
    if (this.currentPage > 1) this.currentPage--;
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  // ─────────────────────────────────────────────
  // CRUD
  // ─────────────────────────────────────────────

  openNewReceipt(): void {
    const data: ReceiptEntryDialogData = { billNo: '' };
    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '520px',
      disableClose: true
    }).afterClosed().subscribe(result => {
      if (result) {
        this.alertService.showMessage('Success', 'Receipt saved successfully.', MessageSeverity.success);
        this.loadData();
      }
    });
  }

  openReceiptEntry(receipt: Receipt): void {
    const data: ReceiptEntryDialogData = {
      billNo:      receipt.billNo,
      patientName: receipt.patientName,
      balance:     receipt.amountBilled - receipt.amountPaid
    };
    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '520px',
      disableClose: true
    }).afterClosed().subscribe(result => {
      if (result) {
        this.alertService.showMessage('Success', 'Receipt saved successfully.', MessageSeverity.success);
        this.loadData();
      }
    });
  }

  deleteReceipt(receipt: Receipt): void {
    this.alertService.showDialog(
      `Delete receipt ${receipt.receiptNo}?`,
      DialogType.confirm,
      () => this.confirmDelete(receipt)
    );
  }

  private confirmDelete(receipt: Receipt): void {
    this.billingEndpoint.getDeleteReceiptEndpoint(receipt.receiptNo).subscribe({
      next: () => {
        this.alertService.showMessage('Deleted', 'Receipt has been removed.', MessageSeverity.success);
        this.receiptsCache = this.receiptsCache.filter(r => r.receiptNo !== receipt.receiptNo);
        this.applyFilter();
      },
      error: (error: unknown) => {
        this.alertService.showStickyMessage(
          'Delete Error',
          `Unable to delete receipt.\r\nError: "${this.getErrorMessage(error)}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  // ─────────────────────────────────────────────
  // Helpers
  // ─────────────────────────────────────────────

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object') {
      const e = error as { error?: { title?: string }; message?: string };
      return e.error?.title ?? e.message ?? 'Unknown error';
    }
    return 'Unknown error';
  }
}
