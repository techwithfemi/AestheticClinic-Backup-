import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';

import { AestheticConsultation } from '../../../models/aesthetic.model';

export interface BotoxPatientOption {
  patientId: number;
  consultId: string;
  pNo: string;
  firstName: string;
  lastName: string;
  label: string;
}

export interface BotoxDialogResult {
  consultation: AestheticConsultation;
  selectedPatient: BotoxPatientOption;
}

@Component({
  selector: 'app-botox-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatCardModule,
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
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Botox Session' : 'Add Botox Session' }}</h2>
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
            <mat-label>Treatment Date</mat-label>
            <input matInput type="date" formControlName="consultationDate" required />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Treatment Plan</mat-label>
            <textarea matInput rows="3" formControlName="treatmentPlan"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Injection Notes</mat-label>
            <textarea matInput rows="3" formControlName="procedureDescription"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Adverse Events / Risks</mat-label>
            <textarea matInput rows="3" formControlName="risksAndComplications"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Follow-up Notes</mat-label>
            <textarea matInput rows="3" formControlName="postTreatmentInstructions"></textarea>
          </mat-form-field>

          <div class="toggles">
            <mat-slide-toggle formControlName="consentGiven" color="primary">Consent Given</mat-slide-toggle>
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
    .dialog-content { width: 460px; box-sizing: border-box; padding: 16px; }
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
export class BotoxDialogComponent {
  private fb = inject(FormBuilder);
  private _matDialogRef = inject(MatDialogRef<BotoxDialogComponent>);
  private _data = inject<{ isEdit: boolean; consultation?: AestheticConsultation; patientOptions: BotoxPatientOption[] }>(MAT_DIALOG_DATA);

  get dialogRef() { return this._matDialogRef; }
  get data() { return this._data; }

  form = this.fb.nonNullable.group({
    id: [0],
    patientKey: ['', Validators.required],
    consultationDate: ['', Validators.required],
    treatmentPlan: [''],
    procedureDescription: [''],
    risksAndComplications: [''],
    postTreatmentInstructions: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  constructor() {
    if (this.data.isEdit && this.data.consultation) {
      const selectedOption = this.data.patientOptions.find(x => x.patientId === this.data.consultation!.patientId);

      this.form.patchValue({
        id: this.data.consultation.id,
        patientKey: selectedOption?.label ?? '',
        consultationDate: this.data.consultation.consultationDate ? this.data.consultation.consultationDate.slice(0, 10) : '',
        treatmentPlan: this.data.consultation.treatmentPlan ?? '',
        procedureDescription: this.data.consultation.procedureDescription ?? '',
        risksAndComplications: this.data.consultation.risksAndComplications ?? '',
        postTreatmentInstructions: this.data.consultation.postTreatmentInstructions ?? '',
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
      procedureType: 'Botox',
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      risksAndComplications: value.risksAndComplications,
      postTreatmentInstructions: value.postTreatmentInstructions,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.dialogRef.close({ consultation, selectedPatient } as BotoxDialogResult);
  }
}
