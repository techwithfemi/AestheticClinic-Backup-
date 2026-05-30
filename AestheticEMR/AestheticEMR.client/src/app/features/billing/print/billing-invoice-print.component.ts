import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { InvoicePrintData } from '../../../models/legacy/invoice-print-data.model';

export interface InvoicePrintDialogData {
  printData: InvoicePrintData;
}

@Component({
  selector: 'app-billing-invoice-print',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './billing-invoice-print.component.html',
  styleUrl: './billing-invoice-print.component.scss'
})
export class BillingInvoicePrintComponent {
  readonly data = inject<InvoicePrintDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<BillingInvoicePrintComponent>);

  get d(): InvoicePrintData {
    return this.data.printData;
  }

  /** Format a number: negatives use (1,234.00) accounting notation */
  fmt(val: number): string {
    if (val < 0) {
      return `(${Math.abs(val).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })})`;
    }
    return val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  get subtotal(): number {
    return this.d.details.reduce((sum, r) => sum + r.subTotal, 0);
  }

  print(): void {
    window.print();
  }

  close(): void {
    this.dialogRef.close();
  }
}
