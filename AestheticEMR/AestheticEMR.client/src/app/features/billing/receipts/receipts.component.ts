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
import { MatSelectModule } from '@angular/material/select';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { Receipt } from '../../../models/legacy/receipt.model';
import { ReceiptEntryDialogComponent, ReceiptEntryDialogData } from './receipt-entry-dialog.component';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { HPatient } from '../../../models/legacy/h-patient.model';

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
    MatSelectModule
  ],
  animations: [fadeInOut],
  templateUrl: './receipts.component.html',
  styleUrl: './receipts.component.scss'
})
export class ReceiptsComponent implements OnInit {
  private alertService = inject(AlertService);
  private billingEndpoint = inject(BillingEndpoint);
  private attendanceEndpoint = inject(AttendanceEndpoint);
  private patientEndpoint = inject(HPatientEndpoint);
  private dialog = inject(MatDialog);

  receipts: Receipt[] = [];
  receiptsCache: Receipt[] = [];
  filteredReceipts: Receipt[] = [];
  patients: HPatient[] = [];
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

  ngOnInit(): void {
    this.loadPatients();
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

  private loadPatients(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => {
        this.patients = patients ?? [];
        this.loadAttendanceOptions();
      },
      error: () => {
        this.patients = [];
        this.loadAttendanceOptions();
      }
    });
  }

  private loadAttendanceOptions(): void {
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().subscribe({
      next: attendance => {
        const todays = (attendance ?? []).filter(a => this.isToday(a.recDate));
        const unique = new Map<string, ReceiptAttendanceOption>();

        for (const item of todays) {
          const consultId = item.consultId ?? '';
          const pNo = item.pNo ?? '';
          if (!consultId || !pNo) {
            continue;
          }

          const patient = this.patients.find(p => p.pno === pNo);
          const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();

          const option: ReceiptAttendanceOption = {
            consultId,
            pNo,
            patientName,
            label: `${patientName} [${consultId}]`
          };

          const key = this.optionKey(option);
          if (!unique.has(key)) {
            unique.set(key, option);
          }
        }

        this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
      },
      error: () => {
        this.attendanceOptions = [];
      }
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

  prevPage(): void {
    if (this.currentPage > 1) this.currentPage--;
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  optionKey(option: ReceiptAttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  openNewReceipt(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      this.alertService.showStickyMessage('Validation Error', 'Please select a patient name.', MessageSeverity.error);
      return;
    }

    const data: ReceiptEntryDialogData = {
      consultId: selected.consultId,
      billNo: selected.consultId,
      pNo: selected.pNo,
      patientName: selected.patientName
    };

    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '840px',
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
      billNo: receipt.billNo,
      patientName: receipt.fullname,
      balance: receipt.balance,
      pNo: receipt.pNo,
      consultId: receipt.billNo
    };
    this.dialog.open(ReceiptEntryDialogComponent, {
      data,
      width: '840px',
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

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object') {
      const e = error as { error?: { title?: string }; message?: string };
      return e.error?.title ?? e.message ?? 'Unknown error';
    }
    return 'Unknown error';
  }

  private isToday(dateValue?: string): boolean {
    if (!dateValue) {
      return false;
    }

    const date = new Date(dateValue);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }
}
