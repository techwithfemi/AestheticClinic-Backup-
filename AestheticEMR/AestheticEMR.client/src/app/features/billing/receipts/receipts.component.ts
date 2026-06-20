import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { Receipt } from '../../../models/legacy/receipt.model';
import { ReceiptEntryDialogComponent, ReceiptEntryDialogData } from './receipt-entry-dialog.component';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';

interface ReceiptAttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  label: string;
}

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
    MatTableModule,
    MatSelectModule,
    MatTooltipModule
  ],
  animations: [fadeInOut],
  templateUrl: './receipts.component.html',
  styleUrl: './receipts.component.scss'
})
export class ReceiptsComponent implements OnInit {
  private alertService = inject(AlertService);
  private billingEndpoint = inject(BillingEndpoint);
  private attendanceEndpoint = inject(AttendanceEndpoint);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  receipts: Receipt[] = [];
  receiptsCache: Receipt[] = [];
  filteredReceipts: Receipt[] = [];
  attendanceOptions: ReceiptAttendanceOption[] = [];
  loadingIndicator = false;
  searchText = '';
  selectedAttendanceKey = '';

  currentPage = 1;
  readonly pageSize = 10;

  readonly listColumns = [
    'receiptDate', 'receiptNo', 'billNo', 'fullname',
    'payType', 'amountBilled', 'amountPaid', 'balance', 'receivedBy', 'actions'
  ];

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredReceipts.length / this.pageSize));
  }

  get pagedReceipts(): Receipt[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredReceipts.slice(start, start + this.pageSize);
  }

  get totalReceiptCount(): number {
    return this.filteredReceipts.length;
  }

  get totalBilled(): number {
    return this.filteredReceipts.reduce((sum, r) => sum + ((r.amountBilled ?? 0) + (r.tax ?? 0)), 0);
  }

  get totalPaid(): number {
    return this.filteredReceipts.reduce((sum, r) => sum + (r.amountPaid ?? 0), 0);
  }

  get totalOutstanding(): number {
    const outstanding = this.totalBilled - this.totalPaid;
    return Math.round(outstanding * 100) / 100;
  }

  ngOnInit(): void {
    this.loadAttendanceOptions();
    this.loadData();
  }

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.billingEndpoint.getReceiptsEndpoint<Receipt[]>(true).subscribe({
      next: receipts => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.receipts = receipts ?? [];
        this.receiptsCache = [...this.receipts];
        this.onSearch();
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
    this.loadAttendanceOptions();
  }

  private loadAttendanceOptions(): void {
    this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().subscribe({
      next: visits => {
        const unique = new Map<string, ReceiptAttendanceOption>();

        for (const item of visits ?? []) {
          const consultId = item.consultId ?? '';
          const pNo = item.pNo ?? '';
          if (!consultId || !pNo) continue;

          const patientName = (item.fullname ?? '').trim() || 'Unknown Patient';
          const option: ReceiptAttendanceOption = { consultId, pNo, patientName, label: `${patientName} [${consultId}]` };
          const key = this.optionKey(option);
          if (!unique.has(key)) unique.set(key, option);
        }

        this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
      },
      error: () => { this.attendanceOptions = []; }
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.applyFilter();
  }

  private applyFilter(): void {
    const term = this.searchText.trim().toLowerCase();

    if (!term) {
      this.filteredReceipts = this.receiptsCache.filter(r => this.isToday(r.receiptDate));
      return;
    }

    this.filteredReceipts = this.receiptsCache.filter(r =>
      r.receiptNo.toLowerCase().includes(term) ||
      r.billNo.toLowerCase().includes(term) ||
      (r.fullname ?? '').toLowerCase().includes(term) ||
      r.payType.toLowerCase().includes(term) ||
      (r.patNo ?? '').toLowerCase().includes(term)
    );
  }

  prevPage(): void { if (this.currentPage > 1) this.currentPage--; }
  nextPage(): void { if (this.currentPage < this.totalPages) this.currentPage++; }

  optionKey(option: ReceiptAttendanceOption): string { return `${option.consultId}|${option.pNo}`; }

  openNewReceipt(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      this.alertService.showStickyMessage('Validation Error', 'Please select a patient from today\'s attendance.', MessageSeverity.error);
      return;
    }

    const data: ReceiptEntryDialogData = {
      billNo: selected.consultId,
      pNo: selected.pNo,
      patientName: selected.patientName
    };

    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '760px',
      disableClose: true
    }).afterClosed().subscribe(result => {
      if (result) {
        this.alertService.showMessage('Success', 'Receipt saved successfully.', MessageSeverity.success);
        this.loadData();
      }
    });
  }

  openEditReceipt(receipt: Receipt): void {
    const data: ReceiptEntryDialogData = {
      receiptNo:   receipt.receiptNo,
      billNo:      receipt.billNo,
      pNo:         receipt.pNo,
      patientName: receipt.fullname,
      balance:     receipt.balance,
      payType:     receipt.payType,
      accountNo:   receipt.accountNo,
      chequeNo:    receipt.chequeNo,
      bankCode:    receipt.bankCode,
      valueDate:   receipt.valueDate,
      remarks:     receipt.remarks,
      amountPaid:  receipt.amountPaid
    };

    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '840px',
      disableClose: true
    }).afterClosed().subscribe(result => {
      if (result) {
        this.alertService.showMessage('Success', 'Receipt updated successfully.', MessageSeverity.success);
        this.loadData();
      }
    });
  }

  previewReceipt(receipt: Receipt): void {
    this.router.navigate(['/billing/receipts', receipt.billNo, 'preview'], {
      queryParams: {
        receiptNo: receipt.receiptNo,
        receiptDate: receipt.receiptDate,
        payType: receipt.payType,
        amountPaid: receipt.amountPaid
      }
    });
  }

  deleteReceipt(receipt: Receipt): void {
    if (receipt.canDelete === false) return;

    this.alertService.showDialog(
      `Delete receipt ${receipt.receiptNo}?\n\nThis action cannot be undone.`,
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

  private isToday(dateValue?: string): boolean {
    if (!dateValue) return false;
    const date = new Date(dateValue);
    if (Number.isNaN(date.getTime())) return false;
    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object') {
      const e = error as { error?: { title?: string }; message?: string };
      return e.error?.title ?? e.message ?? 'Unknown error';
    }
    return 'Unknown error';
  }
}
