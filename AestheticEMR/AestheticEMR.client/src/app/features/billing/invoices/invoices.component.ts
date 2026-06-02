import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
import { BillingEndpoint, ReceiptSaved, SaveReceiptRequest } from '../../../services/billing-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { Billing } from '../../../models/legacy/billing.model';
import { InvoicePrintData } from '../../../models/legacy/invoice-print-data.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { BillingInvoiceDialogComponent, BillingInvoiceDialogData } from './billing-invoice-dialog.component';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HRetainershipEndpoint } from '../../../services/h-retainership-endpoint.service';
import { HRetainership } from '../../../models/legacy/h-retainership.model';

interface InvoiceAttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  coyID?: string;
  clinicType?: string;
  label: string;
}

@Component({
  selector: 'app-invoices',
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
  templateUrl: './invoices.component.html',
  styleUrl: './invoices.component.scss'
})
export class InvoicesComponent implements OnInit {
  private alertService = inject(AlertService);
  private billingEndpoint = inject(BillingEndpoint);
  private patientEndpoint = inject(HPatientEndpoint);
  private attendanceEndpoint = inject(AttendanceEndpoint);
  private hRetainershipEndpoint = inject(HRetainershipEndpoint);
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  invoices: Billing[] = [];
  invoicesCache: Billing[] = [];
  filteredInvoices: Billing[] = [];
  patients: HPatient[] = [];
  attendanceOptions: InvoiceAttendanceOption[] = [];
  loadingIndicator = false;
  searchText = '';
  currentPage = 1;
  readonly pageSize = 10;
  readonly listColumns = [
    'bDate', 'patient', 'clinic', 'debtBF', 'amountBilled',
    'discount', 'tax', 'amountPaid', 'balance',
    'actions', 'add-discount', 'print-invoice', 'print-receipt', 'clientID', 'billNo'
  ];
  selectedAttendanceKey = '';
  private attendanceByConsultId = new Map<string, Attendance>();
  private retainershipByCode = new Map<string, string>();

  // ─────────────────────────────────────────────
  // Lifecycle
  // ─────────────────────────────────────────────

  ngOnInit(): void {
    this.loadPatients();
    this.loadRetainerships();

    this.route.queryParamMap.subscribe(query => {
      const action  = (query.get('action')  ?? '').toLowerCase();
      const openAdd = (query.get('openAdd') ?? '').toLowerCase();
      if (action !== 'create' && openAdd !== '1' && openAdd !== 'true') {
        return;
      }

      const data: BillingInvoiceDialogData = {
        mode:     'create',
        consultId: query.get('consultId') ?? undefined,
        billNo:    query.get('billNo')    ?? query.get('consultId') ?? undefined,
        coyID:     query.get('coyID')     ?? query.get('clientID')  ?? undefined,
        pNo:       query.get('pNo')       ?? undefined,
        clientID:  query.get('clientID')  ?? undefined
      };

      this.openInvoiceDialog(data);

      if (data.consultId && data.pNo) {
        const matched = this.attendanceOptions.find(
          x => x.consultId === data.consultId && x.pNo === data.pNo
        );
        if (matched) {
          this.selectedAttendanceKey = this.optionKey(matched);
        }
      }
    });

    this.loadData();
  }

  // ─────────────────────────────────────────────
  // Data loading
  // ─────────────────────────────────────────────

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.billingEndpoint.getInvoicesEndpoint<Billing[]>().subscribe({
      next: invoices => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.invoices      = invoices ?? [];
        this.invoicesCache = [...this.invoices];
        this.onSearch();
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to retrieve invoices.\r\nError: "${this.getErrorMessage(error)}"`,

          MessageSeverity.error,
          error
        );
      }
    });
  }

  refresh(): void {
    this.loadData();
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
        // Build full lookup map (all records, for clinic display)
        for (const item of attendance ?? []) {
          const consultId = item.consultId ?? '';
          if (consultId && !this.attendanceByConsultId.has(consultId)) {
            this.attendanceByConsultId.set(consultId, item);
          }
        }

        // Dropdown: today's attendances only
        const todays = (attendance ?? []).filter(a => this.isToday(a.recDate));
        const unique  = new Map<string, InvoiceAttendanceOption>();

        for (const item of todays) {
          const consultId = item.consultId ?? '';
          const pNo       = item.pNo       ?? '';
          if (!consultId || !pNo) continue;

          const patient     = this.patients.find(p => p.pno === pNo);
          const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();
          const coyID       = item.coyname ?? patient?.coyName;

          const option: InvoiceAttendanceOption = {
            consultId, pNo, patientName, coyID,
            clinicType: item.clinicType,
            label: `${patientName} [${consultId}]`
          };

          const key = this.optionKey(option);
          if (!unique.has(key)) unique.set(key, option);
        }

        this.attendanceOptions     = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
        this.selectedAttendanceKey = '';
      },
      error: () => {
        this.attendanceOptions     = [];
        this.selectedAttendanceKey = '';
      }
    });
  }

  private loadRetainerships(): void {
    this.hRetainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>().subscribe({
      next: retainerships => {
        const map = new Map<string, string>();
        for (const item of retainerships ?? []) {
          const name = item.retainName?.trim();
          if (!name) continue;
          for (const key of [item.retainCode, item.retainId, item.clientCatId]
            .map(x => (x ?? '').trim()).filter(x => !!x)) {
            map.set(key.toLowerCase(), name);
          }
        }
        this.retainershipByCode = map;
      },
      error: () => { this.retainershipByCode = new Map(); }
    });
  }

  // ─────────────────────────────────────────────
  // Search & pagination
  // ─────────────────────────────────────────────

  onSearch(): void {
    const term = this.searchText.trim().toLowerCase();

    if (!term) {
      this.filteredInvoices = this.invoicesCache.filter(item => this.isToday(item.bDate));
      this.currentPage = 1;
      return;
    }

    this.filteredInvoices = this.invoicesCache.filter(item => {
      const patientName = this.getPatientName(item.pNo).toLowerCase();
      const clientName  = this.getClientDisplay(item).toLowerCase();
      return patientName.includes(term)
        || clientName.includes(term)
        || (item.billNo   ?? '').toLowerCase().includes(term)
        || (item.billType ?? '').toLowerCase().includes(term);
    });

    this.currentPage = 1;
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredInvoices.length / this.pageSize));
  }

  get pagedInvoices(): Billing[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredInvoices.slice(start, start + this.pageSize);
  }

  // ─────────────────────────────────────────────
  // Display helpers
  // ─────────────────────────────────────────────

  getPatientName(pNo: string): string {
    const patientNo = (pNo ?? '').trim().toLowerCase();
    const patient   = this.patients.find(x => (x.pno ?? '').trim().toLowerCase() === patientNo);
    return patient ? `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() : 'Unknown Patient';
  }

  getBalance(invoice: Billing): number {
    return (invoice.debtBF ?? 0)
      + (invoice.amountBilled ?? 0)
      + (invoice.tax ?? 0)
      - (invoice.discount ?? 0)
      - (invoice.amountPaid ?? 0);
  }

  getClinic(invoice: Billing): string {
    const consultId  = invoice.consultId ?? invoice.billNo;
    const attendance = consultId ? this.attendanceByConsultId.get(consultId) : undefined;
    return attendance?.clinicType?.trim() || 'N/A';
  }

  getClientDisplay(invoice: Billing): string {
    const patientNo       = (invoice.pNo ?? '').trim().toLowerCase();
    const patient         = this.patients.find(x => (x.pno ?? '').trim().toLowerCase() === patientNo);
    const consultId       = invoice.consultId ?? invoice.billNo;
    const attendanceCoy   = consultId ? this.attendanceByConsultId.get(consultId)?.coyname?.trim() : '';

    const candidates = [
      invoice.company?.trim(),
      patient?.coyName?.trim(),
      attendanceCoy,
      this.lookupRetainershipName(invoice.clientID)
    ];

    const companyName = candidates.find(x => !!x && !this.isLikelyClientCode(x));
    return companyName || this.lookupRetainershipName(invoice.clientID) || 'N/A';
  }

  optionKey(option: InvoiceAttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  // ─────────────────────────────────────────────
  // Dialog actions
  // ─────────────────────────────────────────────

  openCreate(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      this.alertService.showStickyMessage('Validation Error', 'Please select a patient name.', MessageSeverity.error);
      return;
    }

    this.openInvoiceDialog({
      mode:      'create',
      consultId: selected.consultId,
      billNo:    selected.consultId,
      pNo:       selected.pNo,
      coyID:     selected.coyID,
      clientID:  selected.coyID
    });
  }

  openEdit(invoice: Billing): void {
    this.openInvoiceDialog({ mode: 'edit', billNo: invoice.billNo });
  }

  openAddBill(invoice: Billing): void {
    this.openInvoiceDialog({
      mode:      'create',
      consultId: invoice.consultId ?? invoice.billNo,
      billNo:    invoice.billNo,
      pNo:       invoice.pNo,
      coyID:     invoice.clientID,
      clientID:  invoice.clientID
    });
  }

  deleteInvoice(invoice: Billing): void {
    this.alertService.showDialog('Are you sure you want to delete this invoice?', DialogType.confirm, () => {
      this.alertService.startLoadingMessage();
      this.billingEndpoint.getDeleteInvoiceEndpoint<void>(invoice.billNo).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadData();
          this.alertService.showMessage('Success', 'Invoice deleted successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Delete Error',
            `Unable to delete invoice.\r\nError: "${this.getErrorMessage(error)}"`,

            MessageSeverity.error,
            error
          );
        }
      });
    });
  }

  // ─────────────────────────────────────────────
  // Add Discount
  // ─────────────────────────────────────────────

  addDiscount(invoice: Billing): void {
    const current = invoice.discount ?? 0;
    const input = window.prompt(
      `Enter discount amount for Bill No: ${invoice.billNo}\n` +
      `Current discount: ${current.toLocaleString('en-US', { minimumFractionDigits: 2 })}`,
      current.toFixed(2)
    );

    if (input === null) return; // cancelled

    const value = parseFloat(input);
    if (isNaN(value) || value < 0) {
      this.alertService.showStickyMessage('Validation Error', 'Please enter a valid non-negative discount amount.', MessageSeverity.error);
      return;
    }

    const amountBilled = invoice.amountBilled ?? 0;
    if (value > amountBilled) {
      this.alertService.showStickyMessage(
        'Validation Error',
        `Discount (${value.toFixed(2)}) cannot exceed the billed amount (${amountBilled.toFixed(2)}).`,
        MessageSeverity.error
      );
      return;
    }

    this.alertService.startLoadingMessage('Applying discount...');
    this.billingEndpoint.getUpdateDiscountEndpoint<Billing>(invoice.billNo, value).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage('Success', `Discount of ${value.toFixed(2)} applied to Bill No: ${invoice.billNo}.`, MessageSeverity.success);
        this.loadData();
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(
          'Discount Error',
          `Unable to apply discount.\r\nError: "${this.getErrorMessage(error)}"`,

          MessageSeverity.error,
          error
        );
      }
    });
  }

  // ─────────────────────────────────────────────
  // Print — Invoice
  // ─────────────────────────────────────────────

  generateInvoice(invoice: Billing): void {
    if (!invoice.billNo) {
      this.alertService.showStickyMessage('Error', 'No bill number found.', MessageSeverity.error);
      return;
    }

    this.router.navigate(['/billing/invoices', invoice.billNo, 'preview']);
  }

  // ─────────────────────────────────────────────
  // Print — Receipt (PRIVATE patients only)
  // clientCat is sourced from VwhRecord via the backend print-data endpoint
  // ─────────────────────────────────────────────

  generateReceipt(invoice: Billing): void {
    if (!invoice.billNo) {
      this.alertService.showStickyMessage('Error', 'No bill number found.', MessageSeverity.error);
      return;
    }

    this.alertService.startLoadingMessage('Checking eligibility...');
    this.billingEndpoint.getInvoicePrintDataEndpoint<InvoicePrintData>(invoice.billNo).subscribe({
      next: printData => {
        this.alertService.stopLoadingMessage();

        // clientCat comes from VwhRecord.ClientCat (populated by the backend print-data endpoint)
        const clientCat = (printData.clientCat ?? '').trim().toUpperCase();
        if (clientCat !== 'PRIVATE') {
          this.alertService.showStickyMessage(
            'Receipt Not Allowed',
            `Receipts can only be issued to PRIVATE patients. ` +
            `This patient's billing category is "${printData.clientCat || 'Unknown'}".`,
            MessageSeverity.error
          );
          return;
        }

        // Prompt cashier for payment method
        const payType = window.prompt(
          `Payment Method for Bill No: ${invoice.billNo}\n` +
          `Amount Due: ${printData.balance.toLocaleString('en-US', { minimumFractionDigits: 2 })}\n\n` +
          `Enter payment type (Cash / Cheque / Transfer / POS):`,
          'Cash'
        );

        if (payType === null) return; // cancelled

        const trimmedPayType = payType.trim();
        if (!trimmedPayType) {
          this.alertService.showStickyMessage('Validation Error', 'Payment type is required.', MessageSeverity.error);
          return;
        }

        const payload: SaveReceiptRequest = { payType: trimmedPayType };

        // Collect cheque/transfer details if applicable
        const upper = trimmedPayType.toUpperCase();
        if (upper === 'CHEQUE' || upper === 'TRANSFER') {
          const chequeNo = window.prompt('Cheque / Reference No:') ?? '';
          const bankCode = window.prompt('Bank Code:') ?? '';
          payload.chequeNo = chequeNo.trim() || undefined;
          payload.bankCode = bankCode.trim() || undefined;
        }

        // Save receipt to database, then open print dialog
        this.alertService.startLoadingMessage('Saving receipt...');
        this.billingEndpoint.getSaveReceiptEndpoint<ReceiptSaved>(invoice.billNo, payload).subscribe({
          next: saved => {
            this.alertService.stopLoadingMessage();

            this.router.navigate(['/billing/receipts', invoice.billNo, 'preview'], {
              queryParams: {
                receiptNo: saved.receiptNo,
                receiptDate: saved.receiptDate,
                payType: saved.payType,
                amountPaid: saved.amountPaid
              }
            });

            // Refresh list so updated amountPaid reflects
            this.loadData();
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage(
              'Receipt Error',
              `Cannot save receipt.\r\nError: "${this.getErrorMessage(error)}"`,
              MessageSeverity.error,
              error
            );
          }
        });
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(
          'Receipt Error',
          `Cannot generate receipt.\r\nError: "${this.getErrorMessage(error)}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  // ─────────────────────────────────────────────
  // Private helpers
  // ─────────────────────────────────────────────

  private async openInvoiceDialog(data: BillingInvoiceDialogData): Promise<void> {
    let width = '1200px';
    let maxWidth = '1200px';

    try {
      const response = await fetch('/assets/module-settings/billing.json');
      if (response.ok) {
        const config = await response.json();
        const w = window.innerWidth;
        const dim = config?.addInvoiceDialogDimensions;
        if (dim) {
          if (w < 600 && dim.mobile) {
            width    = dim.mobile.width    || width;
            maxWidth = dim.mobile.maxWidth || maxWidth;
          } else if (w < 992 && dim.tablet) {
            width    = dim.tablet.width    || width;
            maxWidth = dim.tablet.maxWidth || maxWidth;
          } else if (dim.desktop) {
            width    = dim.desktop.width    || width;
            maxWidth = dim.desktop.maxWidth || maxWidth;
          }
        }
      }
    } catch { /* use defaults */ }

    this.dialog.open(BillingInvoiceDialogComponent, {
      width,
      maxWidth,
      disableClose: true,
      data
    }).afterClosed().subscribe((changed: boolean) => {
      if (changed) this.loadData();
    });
  }

  private lookupRetainershipName(clientId?: string): string {
    const key = (clientId ?? '').trim().toLowerCase();
    return key ? (this.retainershipByCode.get(key) ?? '') : '';
  }

  private isLikelyClientCode(value?: string): boolean {
    if (!value) return false;
    const n = value.trim();
    return /^\d+$/.test(n) || /^[A-Z]{1,6}\d+$/.test(n);
  }

  private isToday(value?: string): boolean {
    if (!value) return false;
    const d = new Date(value);
    if (Number.isNaN(d.getTime())) return false;
    const t = new Date();
    return d.getFullYear() === t.getFullYear()
        && d.getMonth()    === t.getMonth()
        && d.getDate()     === t.getDate();
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (!error || typeof error !== 'object') return 'Unknown error';

    const src = error as { error?: unknown; message?: unknown };
    if (typeof src.message === 'string' && src.message) return src.message;

    if (src.error && typeof src.error === 'object') {
      const body = src.error as { errors?: Record<string, string[]>; title?: string; message?: string };
      if (typeof body.message === 'string' && body.message) return body.message;
      if (typeof body.title   === 'string' && body.title)   return body.title;
      if (body.errors) {
        return Object.values(body.errors).flat().join(', ');
      }
    }

    return 'Unknown error';
  }
}
