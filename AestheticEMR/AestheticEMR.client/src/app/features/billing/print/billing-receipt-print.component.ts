import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { InvoicePrintData } from '../../../models/legacy/invoice-print-data.model';

export type ReceiptPaperSize = 'a4' | 'pos';

export interface ReceiptPrintDialogData {
  printData: InvoicePrintData;
}

@Component({
  selector: 'app-billing-receipt-print',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule, MatButtonToggleModule, FormsModule],
  templateUrl: './billing-receipt-print.component.html',
  styleUrl: './billing-receipt-print.component.scss'
})
export class BillingReceiptPrintComponent {
  readonly data = inject<ReceiptPrintDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<BillingReceiptPrintComponent>);

  paperSize: ReceiptPaperSize = 'a4';

  get d(): InvoicePrintData {
    return this.data.printData;
  }

  /** Negative values rendered as (1,234.00) — accounting notation */
  fmt(val: number): string {
    if (val < 0) {
      return `(${Math.abs(val).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })})`;
    }
    return val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  get receiptNo(): string {
    return this.d.receiptNo ?? `RCT-${this.d.billNo}`;
  }

  get receiptDate(): string {
    const raw = this.d.receiptDate;
    const date = raw ? new Date(raw) : new Date();
    return date.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  get payType(): string {
    return this.d.payType ?? 'Cash';
  }

  print(): void {
    window.print();
  }

  close(): void {
    this.dialogRef.close();
  }
}
