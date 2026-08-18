import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpClient } from '@angular/common/http';

import { AestheticConsultation } from '../../../models/aesthetic.model';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';

interface SpaStaticLists {
  serviceTypes?: string[];
  defaultServiceType?: string;
  serviceTypesOrder?: 'ASC' | 'DESC' | 'NONE';
}

export interface SpaPatientOption {
  patientId: number;
  consultId: string;
  pNo: string;
  firstName: string;
  lastName: string;
  label: string;
  fullName?: string;
  photo?: string;
  dateOfBirth?: string;
  company?: string;
  phoneNumber?: string;
}

export interface SpaDialogResult {
  consultation: AestheticConsultation;
  selectedPatient: SpaPatientOption;
}

@Component({
  selector: 'app-spa-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    AttendanceSummaryComponent
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Spa Session' : 'Add Spa Session' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <app-attendance-summary
          [attendance]="selectedAttendanceSummary"
          [photo]="selectedAttendanceSummary?.patientPhotoBase64"
          [compact]="true">
        </app-attendance-summary>

        <form [formGroup]="form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Date</mat-label>
            <input matInput [matDatepicker]="consultDatePicker" formControlName="consultationDate" />
            <mat-datepicker-toggle matIconSuffix [for]="consultDatePicker"></mat-datepicker-toggle>
            <mat-datepicker #consultDatePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Service Type</mat-label>
            <mat-select formControlName="indication">
              @for (serviceType of serviceTypes; track serviceType) {
                <mat-option [value]="serviceType">{{ serviceType }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Type / Product / Scrub Type</mat-label>
            <input matInput formControlName="brandUsed" placeholder="Type of massage, product used, scrub type" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Area of Focus (Body Part)</mat-label>
            <input matInput formControlName="areaTreated" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Skin Type</mat-label>
            <input matInput formControlName="skinAssessment" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Allergies / Health Issues *</mat-label>
            <textarea matInput rows="2" formControlName="allergies" placeholder="List any allergies or health conditions"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Current Medications *</mat-label>
            <textarea matInput rows="2" formControlName="currentMedications" placeholder="List all current medications"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Pain Level / Pressure / Reaction *</mat-label>
            <textarea matInput rows="2" formControlName="risksAndComplications"
              placeholder="Pain level, high/low pressure, skin/client reaction, heat sensitivity, risks and complications, etc."></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Treatment / Recommendation / Result</mat-label>
            <textarea matInput rows="2" formControlName="treatmentPlan"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Monitoring (start/end, duration, water reminder)</mat-label>
            <textarea matInput rows="2" formControlName="deviceSettings"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Notes</mat-label>
            <textarea matInput rows="2" formControlName="procedureDescription"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Consent Notes</mat-label>
            <textarea matInput rows="2" formControlName="consentNotes"
              placeholder="Client confirms sauna is safe, no hidden conditions, accepts risks"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Post-Treatment Instructions *</mat-label>
            <textarea matInput rows="2" formControlName="postTreatmentInstructions" placeholder="Care instructions after treatment"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Services *</mat-label>
            <textarea matInput rows="3" formControlName="services" placeholder="List of services rendered (e.g., Facial, Massage, Body Scrub)"></textarea>
          </mat-form-field>

          <div class="toggles">
            <mat-slide-toggle formControlName="consentGiven" color="primary">Consent Obtained</mat-slide-toggle>
            <mat-slide-toggle formControlName="informationAccepted" color="primary">Information Accepted</mat-slide-toggle>
          </div>
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">Save</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: min(640px, 95vw); max-width: 95vw; box-sizing: border-box; padding: 16px; overflow-x: hidden; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .close-btn { min-width: auto; }
    .full-width { width: 100%; margin-bottom: 10px; box-sizing: border-box; }
    .toggles { display: flex; gap: 16px; margin: 12px 0; flex-wrap: wrap; }
    mat-dialog-content { max-height: 70vh; overflow-y: auto; overflow-x: hidden; padding: 0; margin: 0; }
    mat-form-field { width: 100%; }
  `]
})
export class SpaDialogComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private billingEndpoint = inject(BillingEndpoint);
  dialogRef = inject(MatDialogRef<SpaDialogComponent>);

  serviceTypes: string[] = [];
  patientOptions: SpaPatientOption[] = [];
  selectedAttendanceSummary?: VwhRecord;

  form = this.fb.nonNullable.group({
    id: [0],
    consultationDate: [new Date()],
    indication: ['', Validators.required],
    brandUsed: [''],
    areaTreated: [''],
    skinAssessment: [''],
    allergies: [''],
    risksAndComplications: [''],
    treatmentPlan: [''],
    deviceSettings: [''],
    procedureDescription: [''],
    consentNotes: [''],
    consentGiven: [false, Validators.requiredTrue],
    informationAccepted: [false, Validators.requiredTrue],
    services: ['', Validators.required],
    postTreatmentInstructions: [''],
    currentMedications: ['']
  });

  private _data = inject<{ isEdit: boolean; consultation?: AestheticConsultation; patientOptions: SpaPatientOption[]; selectedPatient?: SpaPatientOption }>(MAT_DIALOG_DATA);
  get data() { return this._data; }

  constructor() {
    this.patientOptions = [...this.data.patientOptions];
    this.loadServiceTypes();

    if (this.data.isEdit && this.data.consultation) {
      const c = this.data.consultation;

      this.form.patchValue({
        id: c.id,
        consultationDate: this.toDate(c.consultationDate) ?? new Date(),
        indication: c.indication ?? '',
        brandUsed: c.brandUsed ?? '',
        areaTreated: c.areaTreated ?? '',
        skinAssessment: c.skinAssessment ?? '',
        allergies: c.allergies ?? '',
        risksAndComplications: c.risksAndComplications ?? '',
        treatmentPlan: c.treatmentPlan ?? '',
        deviceSettings: c.deviceSettings ?? '',
        procedureDescription: c.procedureDescription ?? '',
        consentNotes: c.consentNotes ?? '',
        consentGiven: c.consentGiven ?? false,
        informationAccepted: c.informationAccepted ?? false,
        services: c.services ?? '',
        postTreatmentInstructions: c.postTreatmentInstructions ?? '',
        currentMedications: c.currentMedications ?? ''
      });

      const consultIdFromGridRow = c.consultId?.trim();
      if (consultIdFromGridRow) {
        this.loadAttendanceSummary(consultIdFromGridRow);
      }
      return;
    }

    const consultIdFromPatientDropdown = this.data.selectedPatient?.consultId?.trim();
    if (consultIdFromPatientDropdown) {
      this.loadAttendanceSummary(consultIdFromPatientDropdown);
    }
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const selectedPatient = this.getSelectedPatient();
    if (!selectedPatient) {
      return;
    }

    const value = this.form.getRawValue();
    const editConsultId = this.data.isEdit ? this.data.consultation?.consultId?.trim() : undefined;
    const consultation: AestheticConsultation = {
      id: value.id,
      patientId: selectedPatient.patientId,
      consultId: editConsultId || selectedPatient.consultId || undefined,
      pNo: selectedPatient.pNo || undefined,
      consultationDate: this.toIsoDate(value.consultationDate) ?? this.toIsoDate(new Date()) ?? '',
      procedureType: 'Spa',
      indication: value.indication,
      brandUsed: value.brandUsed,
      areaTreated: value.areaTreated,
      skinAssessment: value.skinAssessment,
      allergies: value.allergies,
      risksAndComplications: value.risksAndComplications,
      treatmentPlan: value.treatmentPlan,
      deviceSettings: value.deviceSettings,
      procedureDescription: value.procedureDescription,
      consentNotes: value.consentNotes,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted,
      services: value.services,
      postTreatmentInstructions: value.postTreatmentInstructions,
      currentMedications: value.currentMedications
    };

    this.dialogRef.close({ consultation, selectedPatient } as SpaDialogResult);
  }

  private loadServiceTypes(): void {
    this.http.get<SpaStaticLists>('/assets/module-settings/spa.json').subscribe({
      next: lists => {
        const cleaned = lists.serviceTypes?.filter(x => !!x?.trim()) ?? [];
        const order = (lists.serviceTypesOrder ?? 'ASC').toUpperCase();

        if (order === 'ASC') {
          this.serviceTypes = [...cleaned].sort((a, b) => a.localeCompare(b));
        } else if (order === 'DESC') {
          this.serviceTypes = [...cleaned].sort((a, b) => b.localeCompare(a));
        } else {
          this.serviceTypes = cleaned;
        }

        const configuredDefault = lists.defaultServiceType?.trim();
        const selected = this.form.controls.indication.value;
        const hasSelected = !!selected && this.serviceTypes.includes(selected);

        if (!hasSelected) {
          if (configuredDefault && this.serviceTypes.includes(configuredDefault)) {
            this.form.controls.indication.setValue(configuredDefault);
          } else {
            this.form.controls.indication.setValue('');
          }
        }
      }
    });
  }

  private loadAttendanceSummary(consultId: string): void {
    this.billingEndpoint.getVwhRecordSummaryEndpoint<VwhRecord>(consultId).subscribe({
      next: summary => {
        this.selectedAttendanceSummary = summary;
      },
      error: () => {
        this.selectedAttendanceSummary = undefined;
      }
    });
  }

  private getSelectedPatient(): SpaPatientOption | undefined {
    if (this.data.isEdit) {
      const consultId = this.data.consultation?.consultId?.trim();
      return this.patientOptions.find(x => x.consultId === consultId)
        ?? this.data.selectedPatient;
    }

    return this.data.selectedPatient;
  }

  private toDate(value?: string): Date | null {
    if (!value) {
      return null;
    }

    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
  }

  private toIsoDate(value: unknown): string | undefined {
    if (!value) {
      return undefined;
    }

    const date = value instanceof Date ? value : new Date(String(value));
    if (Number.isNaN(date.getTime())) {
      return undefined;
    }

    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
  }
}
