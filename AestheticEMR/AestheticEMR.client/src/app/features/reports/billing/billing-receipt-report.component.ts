import { Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter, DateAdapter } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import * as XLSX from 'xlsx';

import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { Receipt } from '../../../models/legacy/receipt.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';

export const DD_MMM_YYYY_FORMATS = {
  parse: { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput: 'dd-MMM-yyyy',
    monthYearLabel: 'MMM yyyy',
    dateA11yLabel: 'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

class DdMmmYyyyDateAdapter extends NativeDateAdapter {
  override parse(value: string): Date | null {
    if (!value) return null;
    const parts = value.split('-');
    if (parts.length === 3) {
      const day = parseInt(parts[0], 10);
      const month = new Date(`${parts[1]} 1 2000`).getMonth();
      const year = parseInt(parts[2], 10);
      if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
        return new Date(year, month, day);
      }
    }
    return super.parse(value);
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'dd-MMM-yyyy') {
      const d = date.getDate().toString().padStart(2, '0');
      const m = date.toLocaleString('en', { month: 'short' });
      const y = date.getFullYear();
      return `${d}-${m}-${y}`;
    }
    return super.format(date, displayFormat);
  }
}

interface ReceiptPatientOption {
  consultId: string;
  pNo: string;
  patientName: string;
  label: string;
}

@Component({
  selector: 'app-billing-receipt-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MatPaginatorModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS }
  ],
  templateUrl: './billing-receipt-report.component.html',
  styleUrl: './billing-receipt-report.component.scss'
})
export class BillingReceiptReportComponent implements OnInit {
  private readonly billingEndpoint = inject(BillingEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly router = inject(Router);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  loadingIndicator = false;

  readonly receipts = signal<Receipt[]>([]);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);

  searchText = '';
  selectedConsultId = '';
  dateFrom!: Date;
  dateTo!: Date;

  private readonly appliedDateFrom = signal<Date>(new Date());
  private readonly appliedDateTo = signal<Date>(new Date());
  private readonly appliedConsultId = signal<string>('');
  readonly appliedSearch = signal<string>('');

  readonly pageSize = 10;
  readonly currentPage = signal<number>(0);

  readonly displayedColumns = [
    'receiptDate',
    'receiptNo',
    'billNo',
    'patient',
    'payType',
    'amountBilled',
    'amountPaid',
    'balance',
    'receivedBy',
    'remarks',
    'actions'
  ];

  readonly dateFiltered = computed(() => {
    let data = this.receipts();

    const consultId = this.appliedConsultId();
    if (consultId) {
      data = data.filter(r => (r.billNo ?? '') === consultId || (r.pNo ?? '') === this.getOptionByConsultId(consultId)?.pNo);
    }

    const from = new Date(this.appliedDateFrom());
    from.setHours(0, 0, 0, 0);
    const to = new Date(this.appliedDateTo());
    to.setHours(23, 59, 59, 999);

    data = data.filter(r => {
      if (!r.receiptDate) return false;
      const time = new Date(r.receiptDate).getTime();
      return time >= from.getTime() && time <= to.getTime();
    });

    return data.sort((a, b) => new Date(b.receiptDate ?? 0).getTime() - new Date(a.receiptDate ?? 0).getTime());
  });

  readonly filtered = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    if (!term) return this.dateFiltered();

    return this.dateFiltered().filter(r =>
      (r.receiptNo ?? '').toLowerCase().includes(term) ||
      (r.billNo ?? '').toLowerCase().includes(term) ||
      (r.fullname ?? '').toLowerCase().includes(term) ||
      (r.patNo ?? '').toLowerCase().includes(term) ||
      (r.payType ?? '').toLowerCase().includes(term) ||
      (r.receivedBy ?? '').toLowerCase().includes(term) ||
      (r.remarks ?? '').toLowerCase().includes(term) ||
      (r.coyName ?? '').toLowerCase().includes(term)
    );
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filtered().slice(start, start + this.pageSize);
  });

  readonly totalReceipts = computed(() => this.filtered().length);
  readonly totalAmountPaid = computed(() => this.filtered().reduce((sum, item) => sum + (item.amountPaid ?? 0), 0));
  readonly totalAmountBilled = computed(() => this.filtered().reduce((sum, item) => sum + (item.amountBilled ?? 0), 0));
  readonly uniquePatients = computed(() => new Set(this.filtered().map(item => (item.patNo ?? '').trim()).filter(Boolean)).size);
  readonly todayReceipts = computed(() => this.filtered().filter(item => this.isToday(item.receiptDate)).length);

  readonly patientOptions = computed(() => {
    const seen = new Set<string>();
    const options: ReceiptPatientOption[] = [];

    for (const visit of this.todayVisits()) {
      if (!visit.consultId || seen.has(visit.consultId)) continue;
      seen.add(visit.consultId);
      const patientName = visit.fullname?.trim() || visit.pNo || visit.consultId;
      options.push({
        consultId: visit.consultId,
        pNo: visit.pNo,
        patientName,
        label: `${patientName} [${visit.consultId}]`
      });
    }

    return options.sort((a, b) => a.label.localeCompare(b.label));
  });

  ngOnInit(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.appliedDateFrom.set(new Date(today));
    this.appliedDateTo.set(new Date(today));
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading billing receipt report...');

    Promise.all([
      this.billingEndpoint.getReceiptsEndpoint<Receipt[]>(true).toPromise().catch(() => [] as Receipt[]),
      this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise().catch(() => [] as QryhvisitsForToday[])
    ]).then(([receipts, visits]) => {
      this.receipts.set(receipts ?? []);
      this.todayVisits.set(visits ?? []);
      this.currentPage.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Load Error',
        `Unable to load billing receipt report.\r\nError: "${error?.message ?? error}"`,
        MessageSeverity.error,
        error
      );
    });
  }

  runReport(): void {
    this.appliedDateFrom.set(new Date(this.dateFrom));
    this.appliedDateTo.set(new Date(this.dateTo));
    this.appliedConsultId.set(this.selectedConsultId);
    this.appliedSearch.set(this.searchText);
    this.currentPage.set(0);
  }

  onPageChange(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  clearFilters: () => void = (): void => {
    this.searchText = '';
    this.selectedConsultId = '';
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    this.dateFrom = new Date(today);
    this.dateTo = new Date(today);
    this.runReport();
  };

  printReport(): void {
    window.print();
  }

  openReceiptPreview(receipt: Receipt): void {
    const queryParams = {
      receiptNo: receipt.receiptNo,
      receiptDate: receipt.receiptDate,
      payType: receipt.payType,
      amountPaid: receipt.amountPaid
    };

    this.router.navigate(['/billing/receipts', receipt.billNo, 'preview'], { queryParams });
  }

  exportExcel(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const data = rows.map(row => ({
      'Receipt Date': row.receiptDate,
      'Receipt No': row.receiptNo,
      'Bill No': row.billNo,
      Patient: row.patient,
      'Patient No': row.patientNo,
      'Pay Type': row.payType,
      'Amount Billed': row.amountBilled,
      'Amount Paid': row.amountPaid,
      Balance: row.balance,
      'Received By': row.receivedBy,
      Remarks: row.remarks
    }));

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Billing Receipts');

    const excelArray = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelArray], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    this.downloadBlob(blob, this.buildFileName('billing-receipt-report', 'xlsx'));
  }

  exportCsv(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Receipt Date', 'Receipt No', 'Bill No', 'Patient', 'Patient No', 'Pay Type', 'Amount Billed', 'Amount Paid', 'Balance', 'Received By', 'Remarks'];
    const csvLines = [
      headers.join(','),
      ...rows.map(row => [row.receiptDate, row.receiptNo, row.billNo, row.patient, row.patientNo, row.payType, row.amountBilled, row.amountPaid, row.balance, row.receivedBy, row.remarks]
        .map(value => this.escapeCsv(value)).join(','))
    ];

    const csvContent = '\uFEFF' + csvLines.join('\r\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    this.downloadBlob(blob, this.buildFileName('billing-receipt-report', 'csv'));
  }

  exportPdf(event: Event): void {
    event.preventDefault();

    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const lines = [
      'Billing Receipt Report',
      `Generated: ${this.formatDate(new Date())}`,
      `Records: ${rows.length}`,
      '',
      'Receipt Date | Receipt No | Patient | Pay Type | Amount Paid | Balance'
    ];

    for (const row of rows) {
      const line = `${row.receiptDate} | ${row.receiptNo} | ${row.patient} | ${row.payType} | ${row.amountPaid} | ${row.balance}`;
      lines.push(line.length > 145 ? `${line.slice(0, 142)}...` : line);
    }

    const blob = new Blob([this.buildSimplePdf(lines)], { type: 'application/pdf' });
    this.downloadBlob(blob, this.buildFileName('billing-receipt-report', 'pdf'));
  }

  private getExportRows(): {
    receiptDate: string;
    receiptNo: string;
    billNo: string;
    patient: string;
    patientNo: string;
    payType: string;
    amountBilled: string;
    amountPaid: string;
    balance: string;
    receivedBy: string;
    remarks: string;
  }[] {
    return this.filtered().map(item => ({
      receiptDate: this.formatDate(item.receiptDate),
      receiptNo: item.receiptNo ?? '—',
      billNo: item.billNo ?? '—',
      patient: item.fullname ?? item.patNo ?? '—',
      patientNo: item.patNo ?? '—',
      payType: item.payType ?? '—',
      amountBilled: this.fmt(item.amountBilled ?? 0),
      amountPaid: this.fmt(item.amountPaid ?? 0),
      balance: this.fmt(item.balance ?? ((item.amountBilled ?? 0) - (item.amountPaid ?? 0))),
      receivedBy: item.receivedBy ?? '—',
      remarks: item.remarks ?? '—'
    }));
  }

  resolvePatientInitial(receipt: Receipt): string {
    return (receipt.fullname ?? receipt.patNo ?? '?').trim()[0]?.toUpperCase() ?? '?';
  }

  getOptionByConsultId(consultId: string): ReceiptPatientOption | undefined {
    return this.patientOptions().find(option => option.consultId === consultId);
  }

  private isToday(value?: string): boolean {
    if (!value) return false;
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return false;
    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private fmt(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  private formatDate(value: string | Date | undefined): string {
    if (!value) return '—';
    const date = value instanceof Date ? value : new Date(value);
    if (Number.isNaN(date.getTime())) return '—';
    const day = date.getDate().toString().padStart(2, '0');
    const month = date.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${date.getFullYear()}`;
  }

  private buildFileName(prefix: string, extension: string): string {
    const now = new Date();
    const part = `${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}-${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}${now.getSeconds().toString().padStart(2, '0')}`;
    return `${prefix}-${part}.${extension}`;
  }

  private downloadBlob(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    a.style.display = 'none';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  }

  private escapeCsv(value: string): string {
    const escaped = (value ?? '').replace(/"/g, '""');
    return `"${escaped}"`;
  }

  private escapePdfText(value: string): string {
    return (value ?? '')
      .replaceAll('\\', '\\\\')
      .replaceAll('(', '\\(')
      .replaceAll(')', '\\)');
  }

  private buildSimplePdf(lines: string[]): string {
    const safeLines = lines.slice(0, 220);
    const lineHeight = 14;
    const startY = 800;

    const content = [
      'BT',
      '/F1 10 Tf',
      `40 ${startY} Td`,
      ...safeLines.flatMap((line, index) => {
        const escaped = this.escapePdfText(line);
        if (index === 0) {
          return [`(${escaped}) Tj`];
        }

        return [`0 -${lineHeight} Td`, `(${escaped}) Tj`];
      }),
      'ET'
    ].join('\n');

    const objects = [
      '1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n',
      '2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n',
      '3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n',
      '4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n',
      `5 0 obj\n<< /Length ${content.length} >>\nstream\n${content}\nendstream\nendobj\n`
    ];

    let pdf = '%PDF-1.4\n';
    const offsets = [0];

    for (const obj of objects) {
      offsets.push(pdf.length);
      pdf += obj;
    }

    const xrefOffset = pdf.length;
    pdf += `xref\n0 ${objects.length + 1}\n`;
    pdf += '0000000000 65535 f \n';

    for (let i = 1; i <= objects.length; i++) {
      pdf += `${offsets[i].toString().padStart(10, '0')} 00000 n \n`;
    }

    pdf += `trailer\n<< /Size ${objects.length + 1} /Root 1 0 R >>\nstartxref\n${xrefOffset}\n%%EOF`;
    return pdf;
  }
}




