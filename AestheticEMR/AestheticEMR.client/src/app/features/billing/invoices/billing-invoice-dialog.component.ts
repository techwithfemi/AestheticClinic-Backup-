import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

import { Billing, BillingDetail } from '../../../models/legacy/billing.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

export interface BillingInvoiceDialogData {
  mode: 'create' | 'edit';
  billNo?: string;
  consultId?: string;
  pNo?: string;
  company?: string;
  clientID?: string;
}

interface AttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  company?: string;
  debtBf?: number;
  label: string;
}

@Component({
  selector: 'app-billing-invoice-dialog',
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
    MatCardModule,
    MatTableModule
  ],
  templateUrl: './billing-invoice-dialog.component.html',
  styleUrl: './billing-invoice-dialog.component.scss'
})
export class BillingInvoiceDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private billingEndpoint = inject(BillingEndpoint);
  private attendanceEndpoint = inject(AttendanceEndpoint);
  private patientEndpoint = inject(HPatientEndpoint);
  private alertService = inject(AlertService);

  private dialogRef = inject(MatDialogRef<BillingInvoiceDialogComponent>);
  data = inject<BillingInvoiceDialogData>(MAT_DIALOG_DATA);

  readonly detailsColumns = ['drgName', 'price', 'qty', 'lineTotal', 'actions'];
  loadingIndicator = false;

  attendanceOptions: AttendanceOption[] = [];
  selectedAttendanceKey = '';

  invoiceForm: FormGroup = this.fb.group({
    bDate: [this.today(), Validators.required],
    discount: [0, [Validators.min(0)]],
    amountPaid: [0, [Validators.min(0)]],
    billType: ['', Validators.maxLength(50)],
    details: this.fb.array([this.createDetailGroup()])
  });

  headerInfo = {
    consultId: '',
    billNo: '',
    company: '',
    pNo: '',
    patientName: '',
    debtBF: 0
  };

  get detailsArray(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  get isEditing(): boolean {
    return this.data.mode === 'edit';
  }

  ngOnInit(): void {
    this.loadAttendanceOptions();

    if (this.isEditing && this.data.billNo) {
      this.loadInvoice(this.data.billNo);
      return;
    }

    this.applyContextDefaults();
  }

  addDetailRow(): void {
    this.detailsArray.push(this.createDetailGroup());
  }

  removeDetailRow(index: number): void {
    if (this.detailsArray.length <= 1) {
      return;
    }

    this.detailsArray.removeAt(index);
  }

  onAttendanceSelectionChanged(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      return;
    }

    this.headerInfo.consultId = selected.consultId;
    this.headerInfo.billNo = selected.consultId;
    this.headerInfo.pNo = selected.pNo;
    this.headerInfo.patientName = selected.patientName;
    this.headerInfo.company = selected.company ?? '';
    this.headerInfo.debtBF = selected.debtBf ?? 0;
  }

  save(): void {
    if (this.invoiceForm.invalid || this.detailsArray.length === 0 || !this.headerInfo.billNo || !this.headerInfo.pNo) {
      this.alertService.showStickyMessage('Validation Error', 'Please select Patient [ConsultID] and complete required fields.', MessageSeverity.error);
      return;
    }

    const payload = this.mapFormToInvoice();
    this.alertService.startLoadingMessage();

    if (this.isEditing && this.data.billNo) {
      this.billingEndpoint.getUpdateInvoiceEndpoint<Billing>(this.data.billNo, payload).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.alertService.showMessage('Success', 'Invoice updated successfully.', MessageSeverity.success);
          this.dialogRef.close(true);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Update Error', this.getErrorMessage(error), MessageSeverity.error, error);
        }
      });
      return;
    }

    this.billingEndpoint.getNewInvoiceEndpoint<Billing>(payload).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage('Success', 'Invoice created successfully.', MessageSeverity.success);
        this.dialogRef.close(true);
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Create Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  computeLineTotal(index: number): number {
    const group = this.detailsArray.at(index) as FormGroup;
    const price = Number(group.get('price')?.value ?? 0);
    const qty = Number(group.get('qty')?.value ?? 0);
    return price * qty;
  }

  computeGrandTotal(): number {
    return this.detailsArray.controls.reduce((sum, _, index) => sum + this.computeLineTotal(index), 0);
  }

  optionKey(option: AttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  private applyContextDefaults(): void {
    this.headerInfo.consultId = this.data.consultId ?? '';
    this.headerInfo.billNo = this.data.billNo ?? this.data.consultId ?? '';
    this.headerInfo.company = this.data.company ?? this.data.clientID ?? '';
    this.headerInfo.pNo = this.data.pNo ?? '';
  }

  private loadInvoice(billNo: string): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo).subscribe({
      next: invoice => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.headerInfo.billNo = invoice.billNo;
        this.headerInfo.consultId = invoice.consultId ?? invoice.billNo;
        this.headerInfo.pNo = invoice.pNo;
        this.headerInfo.company = invoice.company ?? invoice.clientID ?? '';
        this.headerInfo.debtBF = invoice.debtBF ?? 0;

        this.invoiceForm.patchValue({
          bDate: this.normalizeDateInput(invoice.bDate),
          discount: invoice.discount ?? 0,
          amountPaid: invoice.amountPaid ?? 0,
          billType: invoice.billType ?? ''
        });

        this.resetDetailsArray(invoice.details?.length ? invoice.details : [{ drgName: '', price: 0, qty: 1 }]);

        if (this.headerInfo.pNo) {
          const option = this.attendanceOptions.find(x => x.pNo === this.headerInfo.pNo && x.consultId === this.headerInfo.consultId);
          if (option) {
            this.selectedAttendanceKey = this.optionKey(option);
            this.headerInfo.patientName = option.patientName;
            this.headerInfo.debtBF = option.debtBf ?? this.headerInfo.debtBF;
          }
        }
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private loadAttendanceOptions(): void {
    Promise.all([
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([attendance, patients]) => {
      const patientMap = new Map((patients ?? []).map(p => [p.pno ?? '', p]));
      const todays = (attendance ?? []).filter(a => this.isToday(a.recDate));

      const unique = new Map<string, AttendanceOption>();
      for (const item of todays) {
        const consultId = item.consultId ?? '';
        const pNo = item.pNo ?? '';
        if (!consultId || !pNo) {
          continue;
        }

        const patient = patientMap.get(pNo);
        const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();
        const company = item.coyname ?? patient?.coyName;
        const option: AttendanceOption = {
          consultId,
          pNo,
          patientName,
          company,
          debtBf: patient?.debtBf,
          label: `${patientName} [${consultId}]`
        };

        const key = this.optionKey(option);
        if (!unique.has(key)) {
          unique.set(key, option);
        }
      }

      this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));

      if (this.headerInfo.consultId && this.headerInfo.pNo) {
        const preselected = this.attendanceOptions.find(x => x.consultId === this.headerInfo.consultId && x.pNo === this.headerInfo.pNo);
        if (preselected) {
          this.selectedAttendanceKey = this.optionKey(preselected);
          this.headerInfo.patientName = preselected.patientName;
          this.headerInfo.debtBF = preselected.debtBf ?? this.headerInfo.debtBF;
          return;
        }
      }

      if (!this.isEditing && this.attendanceOptions.length > 0) {
        const first = this.attendanceOptions[0];
        this.selectedAttendanceKey = this.optionKey(first);
        this.onAttendanceSelectionChanged();
      }
    });
  }

  private resetDetailsArray(details: BillingDetail[]): void {
    while (this.detailsArray.length) {
      this.detailsArray.removeAt(0);
    }

    details.forEach(item => {
      this.detailsArray.push(this.createDetailGroup(item));
    });
  }

  private createDetailGroup(item?: BillingDetail): FormGroup {
    return this.fb.group({
      drgName: [item?.drgName ?? '', [Validators.required, Validators.maxLength(200)]],
      price: [item?.price ?? 0, [Validators.required, Validators.min(0)]],
      qty: [item?.qty ?? 1, [Validators.required, Validators.min(1)]],
      billType: [item?.billType ?? '', [Validators.maxLength(50)]],
      conID: [item?.conID ?? '', [Validators.maxLength(50)]]
    });
  }

  private mapFormToInvoice(): Billing {
    const raw = this.invoiceForm.getRawValue();

    return {
      billNo: this.headerInfo.billNo,
      bDate: this.normalizeDateInput(raw.bDate),
      pNo: this.headerInfo.pNo,
      clientID: this.headerInfo.company || undefined,
      debtBF: Number(this.headerInfo.debtBF ?? 0),
      amountBilled: this.computeGrandTotal(),
      discount: Number(raw.discount ?? 0),
      amountPaid: Number(raw.amountPaid ?? 0),
      billType: (raw.billType ?? '').trim() || undefined,
      consultId: this.headerInfo.consultId || undefined,
      company: this.headerInfo.company || undefined,
      details: this.detailsArray.controls.map(control => {
        const value = control.getRawValue();
        return {
          drgName: (value.drgName ?? '').trim(),
          price: Number(value.price ?? 0),
          qty: Number(value.qty ?? 1),
          billType: (value.billType ?? '').trim() || undefined,
          conID: (value.conID ?? '').trim() || this.headerInfo.consultId || undefined
        } as BillingDetail;
      })
    };
  }

  private normalizeDateInput(value: string | Date | null | undefined): string {
    if (!value) {
      return this.today();
    }

    if (typeof value === 'string') {
      return value.length >= 10 ? value.substring(0, 10) : this.today();
    }

    return value.toISOString().substring(0, 10);
  }

  private today(): string {
    return new Date().toISOString().substring(0, 10);
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
