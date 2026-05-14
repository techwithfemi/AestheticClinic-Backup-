import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { Billing } from '../../../models/legacy/billing.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { BillingInvoiceDialogComponent, BillingInvoiceDialogData } from './billing-invoice-dialog.component';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { Attendance } from '../../../models/legacy/attendance.model';

interface InvoiceAttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  coyID?: string;
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
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);

  invoices: Billing[] = [];
  invoicesCache: Billing[] = [];
  filteredInvoices: Billing[] = [];
  patients: HPatient[] = [];
  attendanceOptions: InvoiceAttendanceOption[] = [];
  loadingIndicator = false;
  searchText = '';
  currentPage = 1;
  readonly pageSize = 10;
  readonly listColumns = ['billNo', 'bDate', 'patient', 'clientID', 'debtBF', 'amountBilled', 'discount', 'amountPaid', 'balance', 'coyID', 'pNo', 'actions'];
  selectedAttendanceKey = '';

  ngOnInit(): void {
    this.loadPatients();
    this.loadAttendanceOptions();

    this.route.queryParamMap.subscribe(query => {
      const action = (query.get('action') ?? '').toLowerCase();
      const openAdd = (query.get('openAdd') ?? '').toLowerCase();
      if (action !== 'create' && openAdd !== '1' && openAdd !== 'true') {
        return;
      }

      const data: BillingInvoiceDialogData = {
        mode: 'create',
        consultId: query.get('consultId') ?? undefined,
        billNo: query.get('billNo') ?? query.get('consultId') ?? undefined,
        coyID: query.get('coyID') ?? query.get('clientID') ?? undefined,
        pNo: query.get('pNo') ?? undefined,
        clientID: query.get('clientID') ?? undefined
      };

      this.openInvoiceDialog(data);

      if (data.consultId && data.pNo) {
        const matched = this.attendanceOptions.find(x => x.consultId === data.consultId && x.pNo === data.pNo);
        if (matched) {
          this.selectedAttendanceKey = this.optionKey(matched);
        }
      }
    });

    this.loadData();
  }

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.billingEndpoint.getInvoicesEndpoint<Billing[]>()
      .subscribe({
        next: invoices => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.invoices = invoices ?? [];
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

  onSearch(): void {
    const term = this.searchText.trim().toLowerCase();

    if (!term) {
      this.filteredInvoices = this.invoicesCache.filter(item => this.isToday(item.bDate));
      this.currentPage = 1;
      return;
    }

    this.filteredInvoices = this.invoicesCache.filter(item => {
      const patientName = this.getPatientName(item.pNo).toLowerCase();
      return patientName.includes(term)
        || (item.billNo ?? '').toLowerCase().includes(term)
        || (item.clientID ?? '').toLowerCase().includes(term)
        || (item.billType ?? '').toLowerCase().includes(term);
    });

    this.currentPage = 1;
  }

  goToPage(page: number): void {
    const totalPages = this.totalPages;
    if (page < 1 || page > totalPages) {
      return;
    }

    this.currentPage = page;
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredInvoices.length / this.pageSize));
  }

  get pagedInvoices(): Billing[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredInvoices.slice(start, start + this.pageSize);
  }

  getPatientName(pNo: string): string {
    const patient = this.patients.find(x => x.pno === pNo);
    if (!patient) {
      return 'Unknown Patient';
    }

    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim();
  }

  getBalance(invoice: Billing): number {
    const debtBF = invoice.debtBF ?? 0;
    const amountBilled = invoice.amountBilled ?? 0;
    const discount = invoice.discount ?? 0;
    const amountPaid = invoice.amountPaid ?? 0;

    return debtBF + amountBilled - discount - amountPaid;
  }

  openCreate(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      this.openInvoiceDialog({ mode: 'create' });
      return;
    }

    this.openInvoiceDialog({
      mode: 'create',
      consultId: selected.consultId,
      billNo: selected.consultId,
      pNo: selected.pNo,
      coyID: selected.coyID,
      clientID: selected.coyID
    });
  }

  openEdit(invoice: Billing): void {
    this.openInvoiceDialog({ mode: 'edit', billNo: invoice.billNo });
  }

  openAddBill(invoice: Billing): void {
    this.openInvoiceDialog({
      mode: 'create',
      consultId: invoice.consultId ?? invoice.billNo,
      billNo: invoice.billNo,
      pNo: invoice.pNo,
      coyID: invoice.clientID,
      clientID: invoice.clientID
    });
  }

  deleteInvoice(invoice: Billing): void {
    this.alertService.showDialog('Are you sure you want to delete this invoice?', DialogType.confirm,
      () => {
        this.alertService.startLoadingMessage();
        this.billingEndpoint.getDeleteInvoiceEndpoint<void>(invoice.billNo)
          .subscribe({
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

  optionKey(option: InvoiceAttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  private openInvoiceDialog(data: BillingInvoiceDialogData): void {
    const isMobile = typeof window !== 'undefined' && window.innerWidth < 992;

    const ref = this.dialog.open(BillingInvoiceDialogComponent, {
      width: isMobile ? '95vw' : '57vw',
      maxWidth: isMobile ? '95vw' : '780px',
      disableClose: true,
      data
    });

    ref.afterClosed().subscribe((saved: boolean | undefined) => {
      if (saved) {
        this.loadData();
      }
    });
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
        const todays = (attendance ?? []).filter(a => this.isToday(a.recDate));
        const unique = new Map<string, InvoiceAttendanceOption>();

        for (const item of todays) {
          const consultId = item.consultId ?? '';
          const pNo = item.pNo ?? '';
          if (!consultId || !pNo) {
            continue;
          }

          const patient = this.patients.find(p => p.pno === pNo);
          const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();
          const coyID = item.coyname ?? patient?.coyName;

          const option: InvoiceAttendanceOption = {
            consultId,
            pNo,
            patientName,
            coyID,
            label: `${patientName} [${consultId}]`
          };

          const key = this.optionKey(option);
          if (!unique.has(key)) {
            unique.set(key, option);
          }
        }

        this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));

        if (!this.selectedAttendanceKey && this.attendanceOptions.length > 0) {
          this.selectedAttendanceKey = this.optionKey(this.attendanceOptions[0]);
        }
      },
      error: () => {
        this.attendanceOptions = [];
      }
    });
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (!error || typeof error !== 'object') {
      return 'Unknown error';
    }

    const source = error as { error?: unknown; message?: unknown };

    if (typeof source.message === 'string' && source.message) {
      return source.message;
    }

    if (source.error && typeof source.error === 'object') {
      const errorBody = source.error as { errors?: Record<string, string[]>; title?: string; message?: string };
      if (typeof errorBody.message === 'string' && errorBody.message) {
        return errorBody.message;
      }

      if (typeof errorBody.title === 'string' && errorBody.title) {
        return errorBody.title;
      }

      if (errorBody.errors) {
        const firstErrorGroup = Object.values(errorBody.errors)[0];
        if (Array.isArray(firstErrorGroup) && firstErrorGroup.length > 0) {
          return firstErrorGroup[0];
        }
      }
    }

    return 'Unable to process request';
  }
}
