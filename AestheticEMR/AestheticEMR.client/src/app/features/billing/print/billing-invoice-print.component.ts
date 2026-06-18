import { Component, OnInit, ElementRef, ViewChild, inject } from '@angular/core';
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
  @ViewChild('printRoot') private printRoot?: ElementRef<HTMLElement>;

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
    this.billingEndpoint.getInvoicePrintDataEndpoint<InvoicePrintData>(billNo, true).subscribe({
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
    const printContainer = this.printRoot?.nativeElement;
    if (!printContainer) {
      return;
    }

    const printableContent = printContainer.outerHTML;
    const popupWidth = 900;
    const popupHeight = 675;
    const popupLeft = Math.max(0, Math.round((window.screen.availWidth - popupWidth) / 2));
    const popupTop = Math.max(0, Math.round((window.screen.availHeight - popupHeight) / 2));
    const printWindow = window.open(
      '',
      '_blank',
      `popup=yes,width=${popupWidth},height=${popupHeight},left=${popupLeft},top=${popupTop},resizable=no,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no`
    );
    if (!printWindow) {
      return;
    }

    const styleTags = Array.from(document.querySelectorAll('style, link[rel="stylesheet"]'))
      .map(tag => tag.outerHTML)
      .join('');

    printWindow.document.open();
    printWindow.document.write(`
      <!doctype html>
      <html>
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>Invoice ${this.d.billNo}</title>
          <base href="${document.baseURI}">
          ${styleTags}
          <style>
            @page { size: A4 portrait; margin: 0; }
            html, body {
              margin: 0;
              padding: 0;
              background: #f3f4f6;
              font-family: 'Segoe UI', Arial, sans-serif;
            }
            .preview-shell {
              min-height: 100vh;
              display: flex;
              flex-direction: column;
            }
            .preview-toolbar {
              display: flex;
              align-items: center;
              justify-content: space-between;
              gap: 12px;
              padding: 10px 14px;
              background: #0f172a;
              color: #fff;
              position: sticky;
              top: 0;
              z-index: 10;
            }
            .preview-toolbar .title {
              font-size: 14px;
              font-weight: 600;
            }
            .preview-toolbar .toolbar-actions {
              display: flex;
              gap: 8px;
            }
            .preview-toolbar button {
              border: 0;
              border-radius: 6px;
              padding: 6px 10px;
              cursor: pointer;
              font-size: 13px;
            }
            .preview-toolbar .btn-print {
              background: #2563eb;
              color: #fff;
            }
            .preview-toolbar .btn-close {
              background: #ef4444;
              color: #fff;
              width: 32px;
              height: 32px;
              line-height: 1;
              font-size: 18px;
              padding: 0;
            }
            .preview-content {
              padding: 16px;
              overflow: auto;
            }
            .preview-content .a4-page {
              margin: 0 auto;
            }
            @media print {
              .preview-toolbar { display: none !important; }
              html, body { background: #fff !important; }
              .preview-content { padding: 0; }
            }
          </style>
        </head>
        <body>
          <div class="preview-shell">
            <div class="preview-toolbar">
              <span class="title">Invoice ${this.d.billNo}</span>
              <div class="toolbar-actions">
                <button class="btn-print" onclick="window.print()">Print</button>
                <button class="btn-close" onclick="window.close()" aria-label="Close">×</button>
              </div>
            </div>
            <div class="preview-content">
              ${printableContent}
            </div>
          </div>
        </body>
      </html>
    `);
    printWindow.document.close();

    printWindow.onload = () => {
      printWindow.focus();
      printWindow.print();
    };
  }

  close(): void {
    if (this.dialogRef) {
      this.dialogRef.close();
      return;
    }

    this.router.navigate(['/billing/invoices']);
  }
}
