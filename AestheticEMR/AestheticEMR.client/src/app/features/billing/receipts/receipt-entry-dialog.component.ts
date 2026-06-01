import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';

import { BillingEndpoint, SaveReceiptRequest, UpdateReceiptRequest } from '../../../services/billing-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AuthService } from '../../../services/auth.service';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { Billing } from '../../../models/legacy/billing.model';

export interface ReceiptEntryDialogData {
  billNo: string;
  patientName?: string;
  balance?: number;
  pNo?: string;
  /** When set, the dialog is in edit/update mode for this receipt */
  receiptNo?: string;
  payType?: string;
  accountNo?: string;
  chequeNo?: string;
  bankCode?: string;
  valueDate?: string;
  remarks?: string;
}

/** Live financial summary loaded from the billing record */
interface BillingSummary {
  debtBF: number;
  amountBilled: number;
  discount: number;
  tax: number;
  totalBill: number;   // debtBF + amountBilled + tax - discount
  amountPaid: number;
  balance: number;     // totalBill - amountPaid
  bDate: string;
  patientName: string;
}

@Component({
  selector: 'app-receipt-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    AttendanceSummaryComponent
  ],
  templateUrl: './receipt-entry-dialog.component.html',
  styleUrl: './receipt-entry-dialog.component.scss'
})
export class ReceiptEntryDialogComponent implements OnInit {
  readonly data = inject<ReceiptEntryDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<ReceiptEntryDialogComponent>);
  private readonly fb = inject(FormBuilder);
  private readonly billingEndpoint = inject(BillingEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly authService = inject(AuthService);

  form!: FormGroup;
  isSaving = false;
  isLoadingBilling = false;

  attendanceSummary?: VwhRecord;
  billingSummary?: BillingSummary;

  readonly payTypes = ['Cash', 'Cheque', 'Transfer', 'POS'];

  get isEditMode(): boolean { return !!this.data.receiptNo; }
  get dialogTitle(): string { return this.isEditMode ? 'Update Receipt' : 'New Receipt Entry'; }

  get showChequeFields(): boolean {
    const payType = (this.form?.get('payType')?.value ?? '').toString().toUpperCase();
    return ['CHEQUE', 'TRANSFER'].includes(payType);
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      amountToPay: [null],   // null = pay full balance (resolved on save)
      payType:     [this.data.payType ?? 'Cash', Validators.required],
      accountNo:   [this.data.accountNo ?? ''],
      chequeNo:    [this.data.chequeNo ?? ''],
      bankCode:    [this.data.bankCode ?? ''],
      valueDate:   [this.data.valueDate ? new Date(this.data.valueDate) : null],
      remarks:     [this.data.remarks ?? '']
    });

    this.loadAttendanceSummary(this.data.billNo);
    this.loadBillingSummary(this.data.billNo);
  }

  private loadAttendanceSummary(billNo: string): void {
    this.billingEndpoint.getVwhRecordSummaryEndpoint<VwhRecord>(billNo).subscribe({
      next: summary => {
        this.attendanceSummary = summary;
      },
      error: () => { this.attendanceSummary = undefined; }
    });
  }

  private loadBillingSummary(billNo: string): void {
    this.isLoadingBilling = true;
    this.billingSummary = undefined;
    this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo).subscribe({
      next: billing => {
        this.isLoadingBilling = false;
        if (!billing) return;
        const debtBF       = billing.debtBF      ?? 0;
        const amountBilled = billing.amountBilled ?? 0;
        const discount     = billing.discount     ?? 0;
        const tax          = billing.tax          ?? 0;
        const amountPaid   = billing.amountPaid   ?? 0;
        const totalBill    = debtBF + amountBilled + tax - discount;
        const balance      = totalBill - amountPaid;
        this.billingSummary = {
          debtBF, amountBilled, discount, tax, totalBill, amountPaid, balance,
          bDate: billing.bDate ?? '',
          patientName: billing.patientName ?? ''
        };
      },
      error: () => { this.isLoadingBilling = false; this.billingSummary = undefined; }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Payment type is required.', MessageSeverity.error);
      return;
    }

    this.isSaving = true;
    const v = this.form.getRawValue();
    const currentUser = this.authService.currentUser;

    if (this.isEditMode) {
      const payload: UpdateReceiptRequest = {
        payType:    v.payType,
        accountNo:  v.accountNo || undefined,
        chequeNo:   v.chequeNo || undefined,
        bankCode:   v.bankCode || undefined,
        valueDate:  v.valueDate ? (v.valueDate as Date).toISOString() : undefined,
        remarks:    v.remarks || undefined,
        receivedBy: currentUser?.empID
      };
      this.billingEndpoint.getUpdateReceiptEndpoint(this.data.receiptNo!, payload).subscribe({
        next: () => { this.isSaving = false; this.dialogRef.close(true); },
        error: (error: unknown) => {
          this.isSaving = false;
          this.alertService.showStickyMessage('Update Error', 'Unable to update receipt. Error: ' + this.getErrorMessage(error), MessageSeverity.error, error);
        }
      });
    } else {
      const payload: SaveReceiptRequest = {
        payType:      v.payType,
        amountToPay:  v.amountToPay ? Number(v.amountToPay) : undefined,
        accountNo:    v.accountNo || undefined,
        chequeNo:     v.chequeNo || undefined,
        bankCode:     v.bankCode || undefined,
        valueDate:    v.valueDate ? (v.valueDate as Date).toISOString() : undefined,
        remarks:      v.remarks || undefined,
        receivedBy:   currentUser?.empID
      };
      this.billingEndpoint.getSaveReceiptEndpoint(this.data.billNo, payload).subscribe({
        next: result => { this.isSaving = false; this.dialogRef.close(result); },
        error: (error: unknown) => {
          this.isSaving = false;
          this.alertService.showStickyMessage('Save Error', 'Unable to save receipt. Error: ' + this.getErrorMessage(error), MessageSeverity.error, error);
        }
      });
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  fmt(val: number): string {
    if (val < 0) return `(${Math.abs(val).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })})`;
    return val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof Error) return error.message;
    return String(error);
  }
}
