import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { BillingEndpoint, SaveReceiptRequest } from '../../../services/billing-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';

export interface ReceiptEntryDialogData {
  billNo?: string;
  patientName?: string;
  balance?: number;
  pNo?: string;
  consultId?: string;
}

interface AttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  label: string;
  photo?: string;
}

@Component({
  selector: 'app-receipt-entry-dialog',
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
    MatDatepickerModule,
    MatNativeDateModule,
    AttendanceSummaryComponent
  ],
  templateUrl: './receipt-entry-dialog.component.html',
  styleUrl: './receipt-entry-dialog.component.scss'
})
export class ReceiptEntryDialogComponent implements OnInit {
  readonly data = inject<ReceiptEntryDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<ReceiptEntryDialogComponent>);
  private readonly fb = inject(FormBuilder);
  private readonly billingEndpoint = inject(BillingEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);

  form!: FormGroup;
  isSaving = false;

  patients: HPatient[] = [];
  attendanceOptions: AttendanceOption[] = [];
  selectedAttendanceKey = '';
  attendanceSummary?: VwhRecord;

  readonly payTypes = ['Cash', 'Cheque', 'Transfer', 'POS'];

  get showChequeFields(): boolean {
    const payType = (this.form?.get('payType')?.value ?? '').toString().toUpperCase();
    return ['CHEQUE', 'TRANSFER'].includes(payType);
  }

  get selectedPatientInfo(): AttendanceOption | undefined {
    return this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
  }

  get selectedPatientPhoto(): string | undefined {
    return this.selectedPatientInfo?.photo;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      billNo: [this.data.billNo ?? '', Validators.required],
      payType: ['Cash', Validators.required],
      accountNo: [''],
      chequeNo: [''],
      bankCode: [''],
      valueDate: [null],
      remarks: [''],
      receivedBy: ['']
    });

    this.loadPatients();

    const initialConsultId = this.data.consultId ?? this.data.billNo;
    if (initialConsultId) {
      this.loadAttendanceSummary(initialConsultId);
    }
  }

  optionKey(option: AttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  onAttendanceSelectionChange(): void {
    const selected = this.selectedPatientInfo;
    if (!selected) {
      this.attendanceSummary = undefined;
      return;
    }

    this.form.patchValue({ billNo: selected.consultId });
    this.loadAttendanceSummary(selected.consultId);
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
        const unique = new Map<string, AttendanceOption>();

        for (const item of todays) {
          const consultId = item.consultId ?? '';
          const pNo = item.pNo ?? '';
          if (!consultId || !pNo) {
            continue;
          }

          const patient = this.patients.find(p => p.pno === pNo);
          const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();

          const option: AttendanceOption = {
            consultId,
            pNo,
            patientName,
            label: `${patientName} [${consultId}]`,
            photo: patient?.patPixBase64
          };

          const key = this.optionKey(option);
          if (!unique.has(key)) {
            unique.set(key, option);
          }
        }

        this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
        this.applyContextDefaultSelection();
      },
      error: () => {
        this.attendanceOptions = [];
        this.applyContextDefaultSelection();
      }
    });
  }

  private applyContextDefaultSelection(): void {
    const dataConsultId = this.data.consultId ?? this.data.billNo;
    const dataPNo = this.data.pNo;

    if (dataConsultId && dataPNo) {
      const matched = this.attendanceOptions.find(x => x.consultId === dataConsultId && x.pNo === dataPNo);
      if (matched) {
        this.selectedAttendanceKey = this.optionKey(matched);
        this.form.patchValue({ billNo: matched.consultId });
        this.loadAttendanceSummary(matched.consultId);
        return;
      }
    }

    if (dataConsultId) {
      this.form.patchValue({ billNo: dataConsultId });
      this.loadAttendanceSummary(dataConsultId);
    }
  }

  private loadAttendanceSummary(consultId: string): void {
    this.billingEndpoint.getVwhRecordSummaryEndpoint<VwhRecord>(consultId).subscribe({
      next: summary => {
        this.attendanceSummary = summary;
      },
      error: () => {
        this.attendanceSummary = undefined;
      }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Bill No and payment type are required.', MessageSeverity.error);
      return;
    }

    const v = this.form.getRawValue();
    const billNo = (v.billNo ?? '').toString().trim();
    if (!billNo) {
      this.alertService.showStickyMessage('Validation Error', 'Bill No is required.', MessageSeverity.error);
      return;
    }

    const payload: SaveReceiptRequest = {
      payType: v.payType,
      accountNo: v.accountNo || undefined,
      chequeNo: v.chequeNo || undefined,
      bankCode: v.bankCode || undefined,
      valueDate: v.valueDate ? (v.valueDate as Date).toISOString() : undefined,
      remarks: v.remarks || undefined,
      receivedBy: v.receivedBy || undefined
    };

    this.isSaving = true;
    this.billingEndpoint.getSaveReceiptEndpoint(billNo, payload).subscribe({
      next: result => {
        this.isSaving = false;
        this.dialogRef.close(result);
      },
      error: (error: unknown) => {
        this.isSaving = false;
        this.alertService.showStickyMessage(
          'Save Error',
          `Unable to save receipt.\r\nError: "${this.getErrorMessage(error)}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  private isToday(dateValue?: string): boolean {
    if (!dateValue) {
      return false;
    }

    const date = new Date(dateValue);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private getErrorMessage(error: unknown): string {
    if (error && typeof error === 'object') {
      const e = error as { error?: { title?: string }; message?: string };
      return e.error?.title ?? e.message ?? 'Unknown error';
    }

    return 'Unknown error';
  }
}
