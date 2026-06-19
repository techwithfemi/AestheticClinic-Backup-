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
import { AppConfigService } from '../../../services/app-config.service';

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
  appConfig = inject(AppConfigService);

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
    this.billingEndpoint.getInvoicePrintDataEndpoint<InvoicePrintData>(billNo, true).subscribe({
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
        this.router.navigate(['/billing/receipts']);
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

  get totalAmountDue(): number {
    return (this.d.amountBilled ?? 0) + (this.d.debtBF ?? 0) + (this.d.tax ?? 0) - (this.d.discount ?? 0);
  }

  get displayedAmountPaid(): number {
    return this.d.balance < 0 ? this.totalAmountDue : (this.d.amountPaid ?? 0);
  }

  get normalizedBalance(): number {
    return Math.round(((this.d.balance ?? 0) * 100)) / 100;
  }

  get isPaidInFull(): boolean {
    return this.normalizedBalance === 0;
  }

  print(): void {
    const selector = this.paperSize === 'pos' ? '.pos-page.printable' : '.a4-page.printable';
    const printContainer = document.querySelector(selector) as HTMLElement | null;
    if (!printContainer) {
      return;
    }

    const printableContent = printContainer.outerHTML;
    const printWindow = window.open('', '_blank', 'popup=yes,width=1200,height=900,resizable=yes,scrollbars=yes');
    if (!printWindow) {
      return;
    }

    const styleTags = Array.from(document.querySelectorAll('style, link[rel="stylesheet"]'))
      .map(tag => tag.outerHTML)
      .join('');

    const pageRule = this.paperSize === 'pos'
      ? '@page { size: 80mm auto; margin: 0; }'
      : '@page { size: A4 portrait; margin: 0; }';

    printWindow.document.open();
    printWindow.document.write(`
      <!doctype html>
      <html>
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>Receipt ${this.d.billNo}</title>
          <base href="${document.baseURI}">
          ${styleTags}
          <style>
            ${pageRule}
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
              background: #14532d;
              color: #fff;
              position: sticky;
              top: 0;
              z-index: 100;
            }
            .preview-toolbar .title {
              font-size: 16px;
              font-weight: 600;
            }
            .preview-toolbar .toolbar-actions {
              display: flex;
              gap: 8px;
            }
            .preview-toolbar .btn-print {
              background: #16a34a;
              color: #fff;
              border: 0;
              padding: 6px 12px;
              border-radius: 4px;
              cursor: pointer;
              font-size: 14px;
            }
            .preview-toolbar .btn-print:hover {
              background: #15803d;
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
            .preview-content .a4-page,
            .preview-content .pos-page {
              margin: 0 auto;
            }
            @media print {
              .preview-toolbar { display: none !important; }
              html, body { background: #fff !important; }
              .preview-content { padding: 0; }
            }

            /* ===== A4 Receipt layout — explicit override to bypass Angular view encapsulation ===== */
            .a4-page {
              width: 210mm;
              min-height: 297mm;
              padding: 14mm 18mm 18mm;
              margin: 0 auto;
              background: #fff;
              color: #111;
              font-family: 'Segoe UI', Arial, sans-serif;
              font-size: 9.5pt;
              font-weight: 500;
              line-height: 1.35;
              text-rendering: optimizeLegibility;
              -webkit-font-smoothing: antialiased;
              print-color-adjust: exact;
              -webkit-print-color-adjust: exact;
              box-sizing: border-box;
              box-shadow: 0 2px 16px rgba(0,0,0,.15);
              overflow-x: hidden;
            }
            .rct-header {
              display: flex;
              justify-content: space-between;
              align-items: flex-start;
              gap: 12px;
              margin-bottom: 4mm;
            }
            .rct-header-main {
              flex: 1;
              text-align: center;
            }
            .rct-logo-area {
              display: flex;
              align-items: flex-start;
              justify-content: flex-end;
              flex: 0 0 auto;
            }
            .clinic-logo {
              width: 52px;
              height: 52px;
              object-fit: contain;
              flex: 0 0 auto;
            }
            .large-logo {
              width: 120px;
              height: 120px;
            }
            .clinic-name { font-size: 15pt; font-weight: 700; color: #1b5e20; margin: 0 0 2px; }
            .clinic-addr { margin: 1px 0; font-size: 9pt; color: #111; font-weight: 500; }
            .rct-title-label {
              margin-top: 6px;
              display: inline-block;
              border: 2px solid #1b5e20;
              padding: 3px 20px;
              font-size: 13pt;
              font-weight: 700;
              color: #1b5e20;
              letter-spacing: 1px;
            }
            .divider { border: none; border-top: 1.5px solid #1b5e20; margin: 3mm 0; }
            .meta-row {
              display: flex !important;
              justify-content: space-between !important;
              gap: 10mm;
              margin: 3mm 0;
            }
            .meta-block { flex: 1; }
            .meta-right { flex: 0 0 auto; text-align: left; }
            .section-label { font-size: 8pt; text-transform: uppercase; color: #222; font-weight: 600; margin: 0 0 3px; }
            .meta-value { margin: 1px 0; color: #111; }
            .meta-value.bold { font-weight: 700; }
            .meta-table { border-collapse: collapse; margin-left: auto; }
            .meta-table td { padding: 2px 6px 2px 0; font-size: 9.5pt; color: #111; }
            .meta-table .meta-key { color: #111; white-space: nowrap; padding-right: 10px; font-weight: 600; }
            .meta-table .meta-val { font-weight: 600; text-align: right; color: #111; }
            .meta-table .bold { font-weight: 700; }
            .items-table { width: 100%; border-collapse: collapse; margin: 4mm 0 2mm; table-layout: fixed; }
            .items-table th {
              background: #1b5e20;
              color: #fff;
              padding: 5px 8px;
              text-align: left;
              font-size: 8.5pt;
              font-weight: 700;
              white-space: nowrap;
            }
            .items-table th.text-right { text-align: right !important; }
            .items-table td { padding: 4px 8px; border-bottom: .5px solid #d0d0d0; font-size: 9pt; color: #111; vertical-align: top; }
            .items-table tbody tr:nth-child(even) td { background: #f7fbf7; }
            .items-table .text-right { text-align: right !important; }
            .items-table .col-sno { width: 5%; text-align: center; }
            .items-table .col-desc { width: 48%; }
            .items-table .col-qty { width: 10%; text-align: right; font-variant-numeric: tabular-nums; }
            .items-table .col-total { width: 37%; text-align: right !important; font-variant-numeric: tabular-nums; padding-right: 8px; }
            .items-table tbody tr td.col-desc {
              color: #1a1a1a;
            }
            .summary-wrapper {
              display: flex !important;
              justify-content: flex-end !important;
              margin: 4mm 0;
            }
            .summary-table { border-collapse: collapse; width: auto; }
            .summary-table td { padding: 3px 8px; font-size: 9.5pt; color: #111; }
            .summary-table .sum-key { color: #111; white-space: nowrap; font-weight: 600; }
            .summary-table .sum-val { text-align: right !important; white-space: normal; min-width: 80px; color: #111; font-weight: 600; }
            .summary-table .bold { font-weight: 700; }
            .summary-table .total-due-row td { border-top: 1px solid #9e9e9e; padding-top: 4px; }
            .summary-table .paid-row td { background: #e8f5e9; }
            .summary-table .paid-amount { color: #1b5e20; font-size: 11pt; font-weight: 700; }
            .summary-table .balance-row td { border-top: 1.5px solid #1b5e20; }
            .summary-table .outstanding { color: #b71c1c; }
            .summary-table .credit { color: #1b5e20; }
            .sign-row {
              display: flex !important;
              justify-content: space-between !important;
              gap: 20mm;
              margin: 8mm 0 4mm;
            }
            .sign-block { flex: 1; text-align: center; }
            .sign-block .sign-line { border-top: 1px solid #333; margin-bottom: 4px; }
            .sign-block .sign-label { font-size: 8pt; color: #111; font-weight: 600; margin: 0; }
            .rct-footer { margin-top: 6mm; border-top: .5px solid #bdbdbd; padding-top: 3mm; text-align: center; }
            .rct-footer .footer-note { margin: 2px 0; color: #111; font-size: 8.5pt; font-weight: 500; }
            .rct-footer .small { font-size: 8pt; color: #111; font-weight: 500; }

            .pos-items {
              margin: 2mm 0;
            }
            .pos-items .pos-item-header,
            .pos-items .pos-item {
              display: flex;
              align-items: flex-start;
              column-gap: 10px;
              font-size: 9.5pt;
              color: #000;
            }
            .pos-items .pi-desc {
              flex: 1;
              min-width: 0;
              padding-right: 6px;
              overflow-wrap: anywhere;
              color: #000;
              font-weight: 600;
            }
            .pos-items .pi-qty {
              width: 38px;
              min-width: 38px;
              text-align: right;
              padding-right: 10px;
              font-variant-numeric: tabular-nums;
              white-space: nowrap;
              color: #000;
              font-weight: 700;
            }
            .pos-items .pi-amt {
              width: 76px;
              min-width: 76px;
              text-align: right;
              font-variant-numeric: tabular-nums;
              white-space: nowrap;
              color: #000;
              font-weight: 700;
            }

            .pos-page {
              width: 80mm;
              margin: 0 auto;
              padding: 6mm 4mm;
              background: #fff;
              font-family: Consolas, 'Courier New', monospace;
              font-size: 10pt;
              font-weight: 600;
              line-height: 1.45;
              letter-spacing: 0;
              color: #000;
              opacity: 1;
              filter: none;
              text-rendering: geometricPrecision;
              -webkit-font-smoothing: subpixel-antialiased;
              print-color-adjust: exact;
              -webkit-print-color-adjust: exact;
            }
            .pos-center { text-align: center; color: #000; }
            .pos-clinic-name { font-size: 12pt; font-weight: 700; margin: 0 0 2px; color: #000; }
            .pos-line { margin: 1px 0; font-size: 9.5pt; color: #000; font-weight: 600; }
            .pos-divider {
              text-align: center;
              font-size: 9pt;
              color: #111;
              margin: 3mm 0;
              white-space: pre;
              font-weight: 700;
            }
            .pos-receipt-title { font-weight: 700; font-size: 10pt; margin: 2px 0; color: #000; }
            .pos-meta { margin: 2mm 0; }
            .pos-meta .pos-meta-row {
              display: flex;
              justify-content: space-between;
              font-size: 9.5pt;
            }
            .pos-meta .pos-meta-row span:first-child { flex: 0 0 auto; color: #000; font-weight: 700; }
            .pos-meta .pos-meta-row span:last-child { flex: 1; text-align: right; color: #000; font-weight: 700; }
            .pos-summary { color: #000; }
            .pos-summary .pos-sum-row {
              display: flex;
              justify-content: space-between;
              font-size: 9.5pt;
              margin: 1px 0;
            }
            .pos-summary .pos-sum-row span:first-child { color: #000; font-weight: 700; }
            .pos-summary .pos-sum-row span:last-child { color: #000; font-weight: 700; }
            .pos-summary .pos-total {
              font-weight: 700;
              font-size: 10.5pt;
              border-top: 1px solid #222;
              padding-top: 2px;
            }
            .pos-summary .pos-paid { font-weight: 700; font-size: 10.5pt; }
            .pos-summary .pos-paid span:last-child { text-decoration: underline; }
            .pos-summary .pos-outstanding span:last-child { color: #8b0000; }
            .pos-summary .pos-credit span:last-child { color: #0b5e20; }
            .pos-thanks p { margin: 2px 0; font-size: 9.5pt; font-weight: 700; color: #000; }
            .pos-thanks .pos-small { font-size: 9pt; font-weight: 700; color: #000; }
          </style>
        </head>
        <body>
          <div class="preview-shell">
            <div class="preview-toolbar">
              <span class="title">Receipt ${this.d.billNo} (${this.paperSize.toUpperCase()})</span>
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
    } else {
      this.router.navigate(['/billing/receipts']);
    }
  }
}
