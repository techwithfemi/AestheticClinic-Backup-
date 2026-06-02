import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { InvoicePrintData } from '../../../models/legacy/invoice-print-data.model';
import { AppConfigService } from '../../../services/app-config.service';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';

export interface InvoicePrintDialogData {
  printData: InvoicePrintData;
}

@Component({
  selector: 'app-billing-invoice-print',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule, MatCardModule],
  templateUrl: './billing-invoice-print.component.html',
  styleUrl: './billing-invoice-print.component.scss'
})
export class BillingInvoicePrintComponent implements OnInit {
  readonly data = inject<InvoicePrintDialogData | null>(MAT_DIALOG_DATA, { optional: true });
  private readonly dialogRef = inject(MatDialogRef<BillingInvoicePrintComponent>, { optional: true });
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly billingEndpoint = inject(BillingEndpoint);
  appConfig = inject(AppConfigService);

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
        this.printData = res;
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

  get logoSrc(): string {
    return this.appConfig.clientLogo;
  }

  onLogoError(event: Event): void {
    const img = event.target as HTMLImageElement | null;
    if (!img) {
      return;
    }

    img.src = this.appConfig.altClientLogo;
    img.onerror = null;
  }

  fmt(val: number): string {
    if (val < 0) {
      return `(${Math.abs(val).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })})`;
    }
    return val.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  get subtotal(): number {
    return this.d.details.reduce((sum, r) => sum + r.subTotal, 0);
  }

  get currentBill(): number {
    return this.subtotal;
  }

  get discountAmount(): number {
    return this.d.discount ?? 0;
  }

  get taxableAmount(): number {
    return Math.max(0, this.currentBill - this.discountAmount);
  }

  get vatAmount(): number {
    return this.taxableAmount * ((this.d.taxPcent ?? 0) / 100);
  }

  get balanceDue(): number {
    return (this.d.debtBF ?? 0)
      + this.currentBill
      + this.vatAmount
      - this.discountAmount
      - (this.d.amountPaid ?? 0);
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
