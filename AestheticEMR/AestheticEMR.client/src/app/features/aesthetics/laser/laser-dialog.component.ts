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

export interface LaserPatientOption {
  patientId: number;
  consultId: string;
  pNo: string;
  firstName: string;
  lastName: string;
  label: string;
}

export interface LaserDialogResult {
  consultation: AestheticConsultation;
  selectedPatient: LaserPatientOption;
}

@Component({
  selector: 'app-laser-dialog',
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
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Laser Session' : 'Add Laser Session' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Search Patient</mat-label>
            <input matInput [value]="patientSearchText" (input)="onPatientSearchChange($event)" placeholder="Type patient name / PNO" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Patient</mat-label>
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
            <mat-label>Skin Type (Fitzpatrick I–VI)</mat-label>
            <mat-select formControlName="skinAssessment">
              <mat-option value="Type I">Type I – Very fair, always burns</mat-option>
              <mat-option value="Type II">Type II – Fair, usually burns</mat-option>
              <mat-option value="Type III">Type III – Medium, sometimes burns</mat-option>
              <mat-option value="Type IV">Type IV – Olive, rarely burns</mat-option>
              <mat-option value="Type V">Type V – Brown, very rarely burns</mat-option>
              <mat-option value="Type VI">Type VI – Dark, never burns</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Device & Settings (wavelength, fluence, pulse width, spot size)</mat-label>
            <textarea matInput rows="2" formControlName="deviceSettings"
              placeholder="e.g. Nd:YAG 1064nm | Fluence 18 J/cm² | Pulse 10ms | Spot 18mm"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Treatment Plan &amp; Target Area</mat-label>
            <textarea matInput rows="2" formControlName="treatmentPlan"
              placeholder="e.g. Full-face rejuvenation, 6-session package, session 2/6"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Notes / Observations</mat-label>
            <textarea matInput rows="2" formControlName="procedureDescription"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Post-Treatment Instructions</mat-label>
            <textarea matInput rows="2" formControlName="postTreatmentInstructions"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Contraindications &amp; Adverse Events</mat-label>
            <textarea matInput rows="2" formControlName="risksAndComplications"
              placeholder="Document any immediate reactions, contraindications checked..."></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Area Treated</mat-label>
            <input matInput formControlName="areaTreated" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Device Used</mat-label>
            <input matInput formControlName="deviceUsed" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Wavelength</mat-label>
            <input matInput formControlName="wavelength" placeholder="e.g. 1064nm" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Spot Size</mat-label>
            <input matInput formControlName="spotSize" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Fluence</mat-label>
            <input matInput formControlName="fluence" placeholder="e.g. 18 J/cm²" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Pulse Duration</mat-label>
            <input matInput formControlName="pulseDuration" placeholder="e.g. 10ms" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Cooling Method</mat-label>
            <input matInput formControlName="coolingMethod" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Number of Shots</mat-label>
            <input matInput type="number" min="0" formControlName="numberOfShots" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Skin Reaction</mat-label>
            <textarea matInput rows="2" formControlName="skinReaction"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Next Session Date</mat-label>
            <input matInput [matDatepicker]="nextSessionDatePicker" formControlName="nextSessionDate" />
            <mat-datepicker-toggle matIconSuffix [for]="nextSessionDatePicker"></mat-datepicker-toggle>
            <mat-datepicker #nextSessionDatePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Services</mat-label>
            <textarea matInput rows="2" formControlName="services"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Consent Date</mat-label>
            <input matInput [matDatepicker]="consentDatePicker" formControlName="consentDate" />
            <mat-datepicker-toggle matIconSuffix [for]="consentDatePicker"></mat-datepicker-toggle>
            <mat-datepicker #consentDatePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Consent Notes</mat-label>
            <textarea matInput rows="2" formControlName="consentNotes"></textarea>
          </mat-form-field>

          <div class="toggles">
            <mat-slide-toggle formControlName="consentGiven" color="primary">Consent Obtained</mat-slide-toggle>
            <mat-slide-toggle formControlName="informationAccepted" color="primary">Patient Information Accepted</mat-slide-toggle>
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
    .dialog-content { width: 500px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .close-btn { position: relative; right: 0; top: 0; min-width: auto; }
    .full-width { width: 100%; margin-bottom: 12px; box-sizing: border-box; }
    .toggles { display: flex; gap: 16px; margin: 12px 0; flex-wrap: wrap; }
    mat-dialog-content { max-height: 65vh; overflow-y: auto; padding: 0; margin: 0; overflow-x: hidden; }
    mat-form-field { display: block; width: 100%; }
    mat-form-field ::ng-deep .mat-mdc-form-field-infix { padding: 8px 0; }
  `]
})
export class LaserDialogComponent {
  private fb = inject(FormBuilder);
  dialogRef = inject(MatDialogRef<LaserDialogComponent>);

  @ViewChild('patientSelect') patientSelect?: MatSelect;

  patientSearchText = '';

  get filteredPatientOptions(): LaserPatientOption[] {
    const term = this.patientSearchText.trim().toLowerCase();
    if (!term) {
      return this.data.patientOptions;
    }

    return this.data.patientOptions.filter(p =>
      p.label.toLowerCase().includes(term)
      || p.firstName.toLowerCase().includes(term)
      || p.lastName.toLowerCase().includes(term)
      || p.pNo.toLowerCase().includes(term));
  }

  form = this.fb.nonNullable.group({
    id: [0],
    patientKey: ['', Validators.required],
    consultationDate: [new Date(), Validators.required],
    areaTreated: [''],
    deviceUsed: [''],
    wavelength: [''],
    spotSize: [''],
    fluence: [''],
    pulseDuration: [''],
    coolingMethod: [''],
    numberOfShots: [0],
    skinReaction: [''],
    nextSessionDate: [new Date()],
    skinAssessment: [''],
    deviceSettings: [''],
    treatmentPlan: [''],
    procedureDescription: [''],
    postTreatmentInstructions: [''],
    risksAndComplications: [''],
    services: [''],
    consentDate: [new Date()],
    consentNotes: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  private _data = inject<{ isEdit: boolean; consultation?: AestheticConsultation; patientOptions: LaserPatientOption[] }>(MAT_DIALOG_DATA);
  get data() { return this._data; }

  constructor() {
    if (this.data.isEdit && this.data.consultation) {
      const selectedOption = this.data.patientOptions.find(x => x.patientId === this.data.consultation!.patientId);

      this.form.patchValue({
        id: this.data.consultation.id,
        patientKey: selectedOption?.label ?? '',
        consultationDate: this.toDate(this.data.consultation.consultationDate) ?? new Date(),
        areaTreated: this.data.consultation.areaTreated ?? '',
        deviceUsed: this.data.consultation.deviceUsed ?? '',
        wavelength: this.data.consultation.wavelength ?? '',
        spotSize: this.data.consultation.spotSize ?? '',
        fluence: this.data.consultation.fluence ?? '',
        pulseDuration: this.data.consultation.pulseDuration ?? '',
        coolingMethod: this.data.consultation.coolingMethod ?? '',
        numberOfShots: this.data.consultation.numberOfShots ?? 0,
        skinReaction: this.data.consultation.skinReaction ?? '',
        nextSessionDate: this.toDate(this.data.consultation.nextSessionDate) ?? new Date(),
        skinAssessment: this.data.consultation.skinAssessment ?? '',
        deviceSettings: this.data.consultation.deviceSettings ?? '',
        treatmentPlan: this.data.consultation.treatmentPlan ?? '',
        procedureDescription: this.data.consultation.procedureDescription ?? '',
        postTreatmentInstructions: this.data.consultation.postTreatmentInstructions ?? '',
        risksAndComplications: this.data.consultation.risksAndComplications ?? '',
        services: this.data.consultation.services ?? '',
        consentDate: this.toDate(this.data.consultation.consentDate) ?? new Date(),
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
      procedureType: 'Laser',
      areaTreated: value.areaTreated,
      deviceUsed: value.deviceUsed,
      wavelength: value.wavelength,
      spotSize: value.spotSize,
      fluence: value.fluence,
      pulseDuration: value.pulseDuration,
      coolingMethod: value.coolingMethod,
      numberOfShots: value.numberOfShots,
      skinReaction: value.skinReaction,
      nextSessionDate: this.toIsoDate(value.nextSessionDate),
      skinAssessment: value.skinAssessment,
      deviceSettings: value.deviceSettings,
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      postTreatmentInstructions: value.postTreatmentInstructions,
      risksAndComplications: value.risksAndComplications,
      services: value.services,
      consentDate: this.toIsoDate(value.consentDate),
      consentNotes: value.consentNotes,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.dialogRef.close({ consultation, selectedPatient } as LaserDialogResult);
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
