import { Component, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { AestheticConsultation } from '../../../models/aesthetic.model';

export interface SpaPatientOption {
  patientId: number;
  consultId: string;
  pNo: string;
  firstName: string;
  lastName: string;
  label: string;
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
    MatNativeDateModule
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
        <form [formGroup]="form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Search Patient</mat-label>
            <input matInput [value]="patientSearchText" (input)="onPatientSearchChange($event)" placeholder="Type patient name / ConsultID" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Patient (ConsultID)</mat-label>
            <mat-select #patientSelect formControlName="patientKey" required>
              @for (p of filteredPatientOptions; track p.label) {
                <mat-option [value]="p.label">{{ p.label }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Date</mat-label>
            <input matInput [matDatepicker]="consultDatePicker" formControlName="consultationDate" required />
            <mat-datepicker-toggle matIconSuffix [for]="consultDatePicker"></mat-datepicker-toggle>
            <mat-datepicker #consultDatePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Service Type</mat-label>
            <mat-select formControlName="indication" required>
              <mat-option value="Massage">Massage</mat-option>
              <mat-option value="Facials">Facials</mat-option>
              <mat-option value="Body Scrub">Body Scrub</mat-option>
              <mat-option value="Sauna">Sauna</mat-option>
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
            <mat-label>Allergies / Health Issues</mat-label>
            <textarea matInput rows="2" formControlName="allergies"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Pain Level / Pressure / Reaction</mat-label>
            <textarea matInput rows="2" formControlName="risksAndComplications"
              placeholder="Pain level, high/low pressure, skin/client reaction, heat sensitivity, etc."></textarea>
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
  dialogRef = inject(MatDialogRef<SpaDialogComponent>);

  @ViewChild('patientSelect') patientSelect?: MatSelect;

  patientSearchText = '';

  get filteredPatientOptions(): SpaPatientOption[] {
    const term = this.patientSearchText.trim().toLowerCase();
    if (!term) {
      return this.data.patientOptions;
    }

    return this.data.patientOptions.filter(p =>
      p.label.toLowerCase().includes(term)
      || p.firstName.toLowerCase().includes(term)
      || p.lastName.toLowerCase().includes(term)
      || p.pNo.toLowerCase().includes(term)
      || p.consultId.toLowerCase().includes(term));
  }

  form = this.fb.nonNullable.group({
    id: [0],
    patientKey: ['', Validators.required],
    consultationDate: [new Date(), Validators.required],
    indication: ['Massage', Validators.required],
    brandUsed: [''],
    areaTreated: [''],
    skinAssessment: [''],
    allergies: [''],
    risksAndComplications: [''],
    treatmentPlan: [''],
    deviceSettings: [''],
    procedureDescription: [''],
    consentNotes: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  private _data = inject<{ isEdit: boolean; consultation?: AestheticConsultation; patientOptions: SpaPatientOption[] }>(MAT_DIALOG_DATA);
  get data() { return this._data; }

  constructor() {
    if (this.data.isEdit && this.data.consultation) {
      const selectedOption = this.data.patientOptions.find(x => x.patientId === this.data.consultation!.patientId);

      this.form.patchValue({
        id: this.data.consultation.id,
        patientKey: selectedOption?.label ?? '',
        consultationDate: this.toDate(this.data.consultation.consultationDate) ?? new Date(),
        indication: this.data.consultation.indication ?? 'Massage',
        brandUsed: this.data.consultation.brandUsed ?? '',
        areaTreated: this.data.consultation.areaTreated ?? '',
        skinAssessment: this.data.consultation.skinAssessment ?? '',
        allergies: this.data.consultation.allergies ?? '',
        risksAndComplications: this.data.consultation.risksAndComplications ?? '',
        treatmentPlan: this.data.consultation.treatmentPlan ?? '',
        deviceSettings: this.data.consultation.deviceSettings ?? '',
        procedureDescription: this.data.consultation.procedureDescription ?? '',
        consentNotes: this.data.consultation.consentNotes ?? '',
        consentGiven: this.data.consultation.consentGiven ?? true,
        informationAccepted: this.data.consultation.informationAccepted ?? true
      });

      this.patientSearchText = selectedOption?.label ?? '';
    }
  }

  onPatientSearchChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value ?? '';
    this.patientSearchText = value;

    const matches = this.filteredPatientOptions;
    if (matches.length === 1) {
      this.form.controls.patientKey.setValue(matches[0].label);
    }

    if (matches.length > 0) {
      queueMicrotask(() => this.patientSelect?.open());
    }
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const value = this.form.getRawValue();
    const selectedPatient = this.data.patientOptions.find(x => x.label === value.patientKey);
    if (!selectedPatient) {
      return;
    }

    const consultation: AestheticConsultation = {
      id: value.id,
      patientId: selectedPatient.patientId,
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
      informationAccepted: value.informationAccepted
    };

    this.dialogRef.close({ consultation, selectedPatient } as SpaDialogResult);
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
