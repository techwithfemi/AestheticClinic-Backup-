import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslateModule } from '@ngx-translate/core';
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
import { MatCheckboxModule } from '@angular/material/checkbox';

import { BillingEndpoint, SaveReceiptRequest, UpdateReceiptRequest } from '../../../services/billing-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AuthService } from '../../../services/auth.service';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { Billing } from '../../../models/legacy/billing.model';
import { BankAccount } from '../../../models/legacy/bank-account.model';

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
  amountPaid?: number;
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
    TranslateModule,
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
    MatCheckboxModule,
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
  isLoadingBankAccounts = false;
  isExactAmountChecked = false;

  attendanceSummary?: VwhRecord;
  billingSummary?: BillingSummary;
  bankAccounts: BankAccount[] = [];

  readonly payTypes = ['Cash', 'Cheque', 'Transfer', 'POS'];

  get isEditMode(): boolean { return !!this.data.receiptNo; }
  get dialogTitle(): string { return this.isEditMode ? 'Update Receipt' : 'Receipt Entry'; }

  get showChequeFields(): boolean {
    const payType = (this.form?.get('payType')?.value ?? '').toString().toUpperCase();
    return payType === 'CHEQUE' || payType === 'TRANSFER';
  }

  get showBankSelect(): boolean {
    const payType = (this.form?.get('payType')?.value ?? '').toString().toUpperCase();
    return payType !== 'CASH';
  }

  get amountToPayRequired(): boolean {
    return !this.isEditMode;
  }

  get bankAccountRequired(): boolean {
    return this.showBankSelect;
  }

  get isDepositMode(): boolean {
    if (this.isEditMode) return false;
    const totalBill = this.billingSummary?.totalBill ?? 0;
    const balance = this.billingSummary?.balance ?? 0;
    return totalBill === 0 && balance === 0;
  }

  get saveActionLabel(): string {
    if (this.isSaving) return 'billingReceipts.Saving';
    if (this.isEditMode) return 'billingReceipts.UpdateReceipt';
    return this.isDepositMode ? 'billingReceipts.AcceptDeposit' : 'billingReceipts.IssueReceipt';
  }

  ngOnInit(): void {
    this.isExactAmountChecked = false;

    this.form = this.fb.group({
      amountToPay: [this.isEditMode ? this.fmt(0) : null],
      payType:     [this.data.payType ?? 'Cash', Validators.required],
      bankAccount: [this.data.accountNo ?? ''],
      chequeNo:    [this.data.chequeNo ?? ''],
      bankCode:    [this.data.bankCode ?? ''],
      valueDate:   [this.data.valueDate ? new Date(this.data.valueDate) : new Date()],
      remarks:     [this.data.remarks ?? '']
    }, {
      validators: [this.receiptFormValidator()]
    });

    this.form.get('payType')?.valueChanges.subscribe(() => {
      this.form.get('bankAccount')?.updateValueAndValidity({ emitEvent: false });
      this.form.get('amountToPay')?.updateValueAndValidity({ emitEvent: false });
      this.form.updateValueAndValidity({ emitEvent: false });
    });

    const summaryKey = this.resolveAttendanceSummaryKey();
    this.loadAttendanceSummary(summaryKey);
    this.loadBillingSummary(this.data.billNo);
    this.loadBankAccounts();
  }

  private resolveAttendanceSummaryKey(): string {
    return (this.data.billNo ?? '').trim();
  }

  private loadAttendanceSummary(consultId: string): void {
    const resolvedConsultId = consultId.trim();
    if (!resolvedConsultId) {
      this.attendanceSummary = undefined;
      return;
    }

    this.billingEndpoint.getVwhRecordSummaryEndpoint<VwhRecord>(resolvedConsultId).subscribe({
      next: summary => {
        this.attendanceSummary = summary ?? undefined;
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

  private loadBankAccounts(): void {
    this.isLoadingBankAccounts = true;
    this.billingEndpoint.getBankAccountsEndpoint<BankAccount[]>().subscribe({
      next: accounts => {
        this.isLoadingBankAccounts = false;
        // Deduplicate by accountId in case the view returns multiple period rows
        const seen = new Set<string>();
        this.bankAccounts = (accounts ?? []).filter(a => {
          if (seen.has(a.accountId)) return false;
          seen.add(a.accountId);
          return true;
        });
      },
      error: () => { this.isLoadingBankAccounts = false; }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.alertService.showStickyMessage('Validation Error', this.getValidationMessage(), MessageSeverity.error);
      return;
    }

    this.isSaving = true;
    const v = this.form.getRawValue();
    const currentUser = this.authService.currentUser;

    const payType = (v.payType ?? '').toString().toUpperCase();
    const resolvedAccountNo = payType !== 'CASH' ? (v.bankAccount || undefined) : undefined;

    if (this.isEditMode) {
      const payload: UpdateReceiptRequest = {
        payType:    v.payType,
        accountNo:  resolvedAccountNo,
        chequeNo:   v.chequeNo || undefined,
        bankCode:   v.bankCode || undefined,
        valueDate:  this.toIsoDate(v.valueDate),
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
      const rawAmt = String(v.amountToPay ?? '').replace(/,/g, '');
      const parsedAmt = parseFloat(rawAmt);
      const payload: SaveReceiptRequest = {
        payType:      v.payType,
        amountToPay:  parsedAmt,
        accountNo:    resolvedAccountNo,
        chequeNo:     v.chequeNo || undefined,
        bankCode:     v.bankCode || undefined,
        valueDate:    this.toIsoDate(v.valueDate),
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

  onExactAmountToggle(checked: boolean): void {
    this.isExactAmountChecked = checked;
    const balance = this.billingSummary?.balance ?? 0;
    if (checked) {
      this.form.get('amountToPay')?.setValue(this.fmt(balance), { emitEvent: false });
    } else {
      this.form.get('amountToPay')?.setValue(this.fmt(0), { emitEvent: false });
    }
  }

  formatAmountToPay(): void {
    const raw = this.form.get('amountToPay')?.value;
    // Strip any existing formatting then parse
    const num = parseFloat(String(raw ?? '').replace(/,/g, ''));
    if (!isNaN(num) && num > 0) {
      this.form.get('amountToPay')?.setValue(this.fmt(num), { emitEvent: false });
    }
  }

  parseAmountToPay(): number {
    const raw = String(this.form.get('amountToPay')?.value ?? '').replace(/,/g, '');
    const num = parseFloat(raw);
    return isNaN(num) ? 0 : num;
  }

  private getErrorMessage(error: unknown): string {
    const httpError = error instanceof HttpErrorResponse ? error : null;
    if (httpError) {
      const modelState = httpError.error as string | { errors?: Record<string, string[]>; title?: string; detail?: string; message?: string } | null | undefined;
      if (typeof modelState === 'string' && modelState.trim()) return modelState;
      if (modelState && typeof modelState === 'object') {
        const validationErrors = modelState.errors;
        if (validationErrors) {
          const messages: string[] = [];
          for (const key in validationErrors) {
            if (Object.prototype.hasOwnProperty.call(validationErrors, key)) {
              const fieldMessages = validationErrors[key] ?? [];
              for (const fieldMessage of fieldMessages) {
                if (fieldMessage) {
                  messages.push(fieldMessage);
                }
              }
            }
          }

          if (messages.length > 0) return messages.join(' ');
        }

        const title = modelState.title;
        const detail = modelState.detail;
        const message = modelState.message;
        if (title && detail) return `${title}: ${detail}`;
        if (detail) return detail;
        if (title) return title;
        if (message) return message;
      }

      if (typeof httpError.message === 'string' && httpError.message) return httpError.message;
      return `Request failed with status ${httpError.status}`;
    }

    if (error instanceof Error) return error.message;
    return String(error);
  }

  private getValidationMessage(): string {
    if (this.form.hasError('amountRequired')) {
      return 'Amount to Pay is required.';
    }

    if (this.form.hasError('amountInvalid')) {
      return 'Amount to Pay must be greater than zero.';
    }

    if (this.form.hasError('bankAccountRequired')) {
      return 'Bank Account is required for non-cash payment.';
    }

    if (this.form.get('payType')?.hasError('required')) {
      return 'Payment type is required.';
    }

    return 'Please complete the required fields.';
  }

  private receiptFormValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const payType = (control.get('payType')?.value ?? '').toString().trim().toUpperCase();
      const bankAccount = (control.get('bankAccount')?.value ?? '').toString().trim();
      const rawAmount = String(control.get('amountToPay')?.value ?? '').replace(/,/g, '').trim();
      const errors: ValidationErrors = {};

      if (!this.isEditMode) {
        if (!rawAmount) {
          errors['amountRequired'] = true;
        } else {
          const amount = parseFloat(rawAmount);
          if (Number.isNaN(amount) || amount <= 0) {
            errors['amountInvalid'] = true;
          }
        }
      }

      if (payType && payType !== 'CASH' && !bankAccount) {
        errors['bankAccountRequired'] = true;
      }

      return Object.keys(errors).length > 0 ? errors : null;
    };
  }

  private toIsoDate(value: unknown): string | undefined {
    if (!(value instanceof Date) || Number.isNaN(value.getTime())) return undefined;
    const localDate = new Date(value.getTime() - (value.getTimezoneOffset() * 60000));
    return localDate.toISOString();
  }
}
