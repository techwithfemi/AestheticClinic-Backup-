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

import { BillingEndpoint, SaveReceiptRequest } from '../../../services/billing-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

export interface ReceiptEntryDialogData {
  billNo: string;
  patientName?: string;
  balance?: number;
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
    MatNativeDateModule
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

  form!: FormGroup;
  isSaving = false;

  readonly payTypes = ['Cash', 'Cheque', 'Transfer', 'POS'];

  get showChequeFields(): boolean {
    return ['Cheque', 'Transfer'].includes(this.form?.get('payType')?.value ?? '');
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      payType:    ['Cash', Validators.required],
      accountNo:  [''],
      chequeNo:   [''],
      bankCode:   [''],
      valueDate:  [null],
      remarks:    [''],
      receivedBy: ['']
    });
  }

  save(): void {
    if (this.form.invalid) return;

    const v = this.form.getRawValue();
    const payload: SaveReceiptRequest = {
      payType:    v.payType,
      accountNo:  v.accountNo  || undefined,
      chequeNo:   v.chequeNo   || undefined,
      bankCode:   v.bankCode   || undefined,
      valueDate:  v.valueDate  ? (v.valueDate as Date).toISOString() : undefined,
      remarks:    v.remarks    || undefined,
      receivedBy: v.receivedBy || undefined
    };

    this.isSaving = true;
    this.billingEndpoint.getSaveReceiptEndpoint(this.data.billNo, payload).subscribe({
      next: result => {
        this.isSaving = false;
        this.dialogRef.close(result);
      },
      error: error => {
        this.isSaving = false;
        this.alertService.showStickyMessage(
          'Save Error',
          `Unable to save receipt.\r\nError: "${error?.error?.title ?? error?.message}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
