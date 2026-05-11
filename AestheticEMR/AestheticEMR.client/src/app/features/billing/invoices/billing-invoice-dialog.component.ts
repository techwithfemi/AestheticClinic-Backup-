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
import { firstValueFrom } from 'rxjs';

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
  coyID?: string;
  company?: string;
  clientID?: string;
}

interface AttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  coyID?: string;
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
  hasChanges = false;

  attendanceOptions: AttendanceOption[] = [];
  selectedAttendanceKey = '';
  persistedDetails: BillingDetail[] = [];

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
    pNo: '',
    patientName: '',
    coyID: ''
  };

  get detailsArray(): FormArray {
    return this.invoiceForm.get('details') as FormArray;
  }

  get lineItemGroup(): FormGroup {
    return this.detailsArray.at(0) as FormGroup;
  }

  get isEditing(): boolean {
    return this.data.mode === 'edit';
  }

  ngOnInit(): void {
    this.loadAttendanceOptions();

    if (this.isEditing && this.data.billNo) {
      void this.loadInvoice(this.data.billNo);
      return;
    }

    this.applyContextDefaults();
  }

  async addToGrid(): Promise<void> {
    if (this.lineItemGroup.invalid || !this.headerInfo.billNo || !this.headerInfo.pNo) {
      this.alertService.showStickyMessage('Validation Error', 'Please select Patient [ConsultID] and complete Bill Item, Price and Qty.', MessageSeverity.error);
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Adding bill item...');

    try {
      const newDetail = this.mapCurrentLineItem();
      const existing = await this.getInvoiceByBillNo(this.headerInfo.billNo);

      if (!existing) {
        const createPayload = this.buildPayload([newDetail]);
        await firstValueFrom(this.billingEndpoint.getNewInvoiceEndpoint<Billing>(createPayload));
      } else {
        const updatedDetails = [...(existing.details ?? []), newDetail];
        const updatePayload = this.buildPayload(updatedDetails);
        await firstValueFrom(this.billingEndpoint.getUpdateInvoiceEndpoint<Billing>(existing.billNo, updatePayload));
      }

      await this.refreshPersistedDetails();
      this.resetLineItemForm();
      this.hasChanges = true;

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showMessage('Success', 'Bill item added to grid.', MessageSeverity.success);
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Add Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  async deleteDetail(index: number): Promise<void> {
    if (!this.headerInfo.billNo) {
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting bill item...');

    try {
      const existing = await this.getInvoiceByBillNo(this.headerInfo.billNo);
      if (!existing) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        return;
      }

      const remaining = (existing.details ?? []).filter((_, i) => i !== index);
      if (remaining.length === 0) {
        await firstValueFrom(this.billingEndpoint.getDeleteInvoiceEndpoint<void>(existing.billNo));
      } else {
        const updatePayload = this.buildPayload(remaining);
        await firstValueFrom(this.billingEndpoint.getUpdateInvoiceEndpoint<Billing>(existing.billNo, updatePayload));
      }

      await this.refreshPersistedDetails();
      this.hasChanges = true;

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showMessage('Success', 'Bill item deleted.', MessageSeverity.success);
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
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
    this.headerInfo.coyID = selected.coyID ?? '';
    void this.refreshPersistedDetails();
  }

  save(): void {
    this.dialogRef.close(this.hasChanges);
  }

  cancel(): void {
    this.dialogRef.close(this.hasChanges);
  }

  computeLineTotal(index: number): number {
    const group = this.detailsArray.at(index) as FormGroup;
    const price = Number(group.get('price')?.value ?? 0);
    const qty = Number(group.get('qty')?.value ?? 0);
    return price * qty;
  }

  computePersistedLineTotal(detail: BillingDetail): number {
    return Number(detail.price ?? 0) * Number(detail.qty ?? 0);
  }

  computeGrandTotal(): number {
    return this.persistedDetails.reduce((sum, item) => sum + this.computePersistedLineTotal(item), 0);
  }

  optionKey(option: AttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  private applyContextDefaults(): void {
    this.headerInfo.consultId = this.data.consultId ?? '';
    this.headerInfo.billNo = this.data.billNo ?? this.data.consultId ?? '';
    this.headerInfo.pNo = this.data.pNo ?? '';
    this.headerInfo.coyID = this.data.coyID ?? this.data.clientID ?? '';
    void this.refreshPersistedDetails();
  }

  private async loadInvoice(billNo: string): Promise<void> {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    try {
      const invoice = await firstValueFrom(this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo));

      this.headerInfo.billNo = invoice.billNo;
      this.headerInfo.consultId = invoice.consultId ?? invoice.billNo;
      this.headerInfo.pNo = invoice.pNo;
      this.headerInfo.coyID = invoice.clientID ?? '';

      this.invoiceForm.patchValue({
        bDate: this.normalizeDateInput(invoice.bDate),
        discount: invoice.discount ?? 0,
        amountPaid: invoice.amountPaid ?? 0,
        billType: invoice.billType ?? ''
      });

      this.persistedDetails = [...(invoice.details ?? [])];
      this.resetLineItemForm();

      if (this.headerInfo.pNo) {
        const option = this.attendanceOptions.find(x => x.pNo === this.headerInfo.pNo && x.consultId === this.headerInfo.consultId);
        if (option) {
          this.selectedAttendanceKey = this.optionKey(option);
          this.headerInfo.patientName = option.patientName;
          this.headerInfo.coyID = option.coyID ?? this.headerInfo.coyID;
        }
      }

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
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
          coyID: company,
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
          this.headerInfo.coyID = preselected.coyID ?? this.headerInfo.coyID;
          return;
        }
      }

      if (!this.isEditing && this.attendanceOptions.length > 0 && !this.headerInfo.consultId) {
        const first = this.attendanceOptions[0];
        this.selectedAttendanceKey = this.optionKey(first);
        this.onAttendanceSelectionChanged();
      }
    });
  }

  private async refreshPersistedDetails(): Promise<void> {
    if (!this.headerInfo.billNo) {
      this.persistedDetails = [];
      return;
    }

    const existing = await this.getInvoiceByBillNo(this.headerInfo.billNo);
    this.persistedDetails = [...(existing?.details ?? [])];
  }

  private async getInvoiceByBillNo(billNo: string): Promise<Billing | null> {
    try {
      return await firstValueFrom(this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo));
    } catch {
      return null;
    }
  }

  private resetLineItemForm(): void {
    this.lineItemGroup.reset({
      drgName: '',
      price: 0,
      qty: 1,
      billType: '',
      conID: this.headerInfo.consultId ?? ''
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

  private mapCurrentLineItem(): BillingDetail {
    const value = this.lineItemGroup.getRawValue();
    return {
      drgName: (value.drgName ?? '').trim(),
      price: Number(value.price ?? 0),
      qty: Number(value.qty ?? 1),
      billType: (value.billType ?? '').trim() || undefined,
      conID: (value.conID ?? '').trim() || this.headerInfo.consultId || undefined
    } as BillingDetail;
  }

  private buildPayload(details: BillingDetail[]): Billing {
    const raw = this.invoiceForm.getRawValue();

    return {
      billNo: this.headerInfo.billNo,
      bDate: this.normalizeDateInput(raw.bDate),
      pNo: this.headerInfo.pNo,
      clientID: this.headerInfo.coyID || undefined,
      debtBF: Number(0),
      amountBilled: details.reduce((sum, item) => sum + (Number(item.price ?? 0) * Number(item.qty ?? 0)), 0),
      discount: Number(raw.discount ?? 0),
      amountPaid: Number(raw.amountPaid ?? 0),
      billType: (raw.billType ?? '').trim() || undefined,
      consultId: this.headerInfo.consultId || undefined,
      company: undefined,
      details
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
