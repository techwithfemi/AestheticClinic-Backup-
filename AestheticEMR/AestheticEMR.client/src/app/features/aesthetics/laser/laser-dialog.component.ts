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
    MatIconModule
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
            <mat-label>Patient</mat-label>
            <mat-select formControlName="patientKey" required>
              @for (p of data.patientOptions; track p.label) {
                <mat-option [value]="p.label">{{ p.label }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Date</mat-label>
            <input matInput type="date" formControlName="consultationDate" required />
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

  form = this.fb.nonNullable.group({
    id: [0],
    patientKey: ['', Validators.required],
    consultationDate: ['', Validators.required],
    skinAssessment: [''],
    deviceSettings: [''],
    treatmentPlan: [''],
    procedureDescription: [''],
    postTreatmentInstructions: [''],
    risksAndComplications: [''],
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
        consultationDate: this.data.consultation.consultationDate ? this.data.consultation.consultationDate.slice(0, 10) : '',
        skinAssessment: this.data.consultation.skinAssessment ?? '',
        deviceSettings: this.data.consultation.deviceSettings ?? '',
        treatmentPlan: this.data.consultation.treatmentPlan ?? '',
        procedureDescription: this.data.consultation.procedureDescription ?? '',
        postTreatmentInstructions: this.data.consultation.postTreatmentInstructions ?? '',
        risksAndComplications: this.data.consultation.risksAndComplications ?? '',
        consentGiven: this.data.consultation.consentGiven ?? true,
        informationAccepted: this.data.consultation.informationAccepted ?? true
      });
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
      consultationDate: value.consultationDate,
      procedureType: 'Laser',
      skinAssessment: value.skinAssessment,
      deviceSettings: value.deviceSettings,
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      postTreatmentInstructions: value.postTreatmentInstructions,
      risksAndComplications: value.risksAndComplications,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.dialogRef.close({ consultation, selectedPatient } as LaserDialogResult);
  }
}
