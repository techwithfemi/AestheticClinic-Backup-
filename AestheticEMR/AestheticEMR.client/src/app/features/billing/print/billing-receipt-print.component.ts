import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { InvoicePrintData } from '../../../models/legacy/invoice-print-data.model';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';

export type ReceiptPaperSize = 'a4' | 'pos';

export interface ReceiptPrintDialogData {
  printData: InvoicePrintData;
}

@Component({
  selector: 'app-billing-receipt-print',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule, MatButtonToggleModule, MatCardModule, FormsModule],
  templateUrl: './billing-receipt-print.component.html',
  styleUrl: './billing-receipt-print.component.scss'
})
export class BillingReceiptPrintComponent implements OnInit {
  readonly data = inject<ReceiptPrintDialogData | null>(MAT_DIALOG_DATA, { optional: true });
  private readonly dialogRef = inject(MatDialogRef<BillingReceiptPrintComponent>, { optional: true });
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly billingEndpoint = inject(BillingEndpoint);

  paperSize: ReceiptPaperSize = 'a4';
  loadingIndicator = false;
  isDialogMode = false;
  private printData: InvoicePrintData | null = null;

  private readonly emptyPrintData: InvoicePrintData = {
    billHead: '',
    billHead2: '',
    billHead3: '',
    billHead4: '',
    billNo: '',
    billDate: '',
    taxName: 'VAT',
    tin: '',
    taxPcent: 0,
    patientName: '',
    patientNo: '',
    clientCat: '',
    payerName: '',
    payerAddress: '',
    payerPhone: '',
    debtBF: 0,
    amountBilled: 0,
    discount: 0,
    tax: 0,
    amountPaid: 0,
    balance: 0,
    details: []
  };

  ngOnInit(): void {
    this.isDialogMode = !!this.dialogRef;

    if (this.data?.printData) {
      this.printData = this.data.printData;
      return;
    }

    const billNo = this.route.snapshot.paramMap.get('billNo');
    if (!billNo) {
      this.router.navigate(['/billing/invoices']);
      return;
    }

    this.loadingIndicator = true;
    this.billingEndpoint.getInvoicePrintDataEndpoint<InvoicePrintData>(billNo).subscribe({
      next: res => {
        const receiptNo = this.route.snapshot.queryParamMap.get('receiptNo') ?? res.receiptNo;
        const receiptDate = this.route.snapshot.queryParamMap.get('receiptDate') ?? res.receiptDate;
        const payType = this.route.snapshot.queryParamMap.get('payType') ?? res.payType;
        const amountPaidParam = this.route.snapshot.queryParamMap.get('amountPaid');
        const amountPaid = amountPaidParam !== null ? Number(amountPaidParam) : res.amountPaid;

        this.printData = {
          ...res,
          receiptNo: receiptNo ?? undefined,
          receiptDate: receiptDate ?? undefined,
          payType: payType ?? undefined,
          amountPaid: Number.isFinite(amountPaid as number) ? (amountPaid as number) : res.amountPaid
        };
        this.loadingIndicator = false;
      },
      error: () => {
        this.loadingIndicator = false;
        this.router.navigate(['/billing/invoices']);
      }
    });
  }

  get d(): InvoicePrintData {
    return this.printData ?? this.emptyPrintData;
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
    if (this.dialogRef) {
      this.dialogRef.close();
      return;
    }

    this.router.navigate(['/billing/invoices']);
  }
}
