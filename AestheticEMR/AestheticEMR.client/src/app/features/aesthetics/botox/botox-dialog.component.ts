import { Component, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

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
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule
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
            <mat-label>Treatment Date</mat-label>
            <input matInput [matDatepicker]="consultDatePicker" formControlName="consultationDate" required />
            <mat-datepicker-toggle matIconSuffix [for]="consultDatePicker"></mat-datepicker-toggle>
            <mat-datepicker #consultDatePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Indication</mat-label>
            <input matInput formControlName="indication" placeholder="e.g. Forehead lines" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Brand Used</mat-label>
            <input matInput formControlName="brandUsed" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Dilution</mat-label>
            <input matInput formControlName="dilution" placeholder="e.g. 2.5ml per vial" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Units Used</mat-label>
            <input matInput type="number" min="0" step="0.1" formControlName="unitsUsed" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Injection Mapping</mat-label>
            <textarea matInput rows="2" formControlName="injectionMapping" placeholder="Sites and units by area"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Lot Number</mat-label>
            <input matInput formControlName="lotNumber" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Follow-up Review</mat-label>
            <textarea matInput rows="2" formControlName="followUpReview"></textarea>
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

  @ViewChild('patientSelect') patientSelect?: MatSelect;

  patientSearchText = '';

  get filteredPatientOptions(): BotoxPatientOption[] {
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
    indication: [''],
    brandUsed: [''],
    dilution: [''],
    unitsUsed: [0],
    injectionMapping: [''],
    lotNumber: [''],
    followUpReview: [''],
    treatmentPlan: [''],
    procedureDescription: [''],
    risksAndComplications: [''],
    postTreatmentInstructions: [''],
    consentDate: [new Date()],
    consentNotes: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  constructor() {
    if (this.data.isEdit && this.data.consultation) {
      const selectedOption = this.data.patientOptions.find(x => x.patientId === this.data.consultation!.patientId);

      this.form.patchValue({
        id: this.data.consultation.id,
        patientKey: selectedOption?.label ?? '',
        consultationDate: this.toDate(this.data.consultation.consultationDate) ?? new Date(),
        indication: this.data.consultation.indication ?? '',
        brandUsed: this.data.consultation.brandUsed ?? '',
        dilution: this.data.consultation.dilution ?? '',
        unitsUsed: this.data.consultation.unitsUsed ?? 0,
        injectionMapping: this.data.consultation.injectionMapping ?? '',
        lotNumber: this.data.consultation.lotNumber ?? '',
        followUpReview: this.data.consultation.followUpReview ?? '',
        treatmentPlan: this.data.consultation.treatmentPlan ?? '',
        procedureDescription: this.data.consultation.procedureDescription ?? '',
        risksAndComplications: this.data.consultation.risksAndComplications ?? '',
        postTreatmentInstructions: this.data.consultation.postTreatmentInstructions ?? '',
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
      procedureType: 'Botox',
      indication: value.indication,
      brandUsed: value.brandUsed,
      dilution: value.dilution,
      unitsUsed: value.unitsUsed,
      injectionMapping: value.injectionMapping,
      lotNumber: value.lotNumber,
      followUpReview: value.followUpReview,
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      risksAndComplications: value.risksAndComplications,
      postTreatmentInstructions: value.postTreatmentInstructions,
      consentDate: this.toIsoDate(value.consentDate),
      consentNotes: value.consentNotes,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.dialogRef.close({ consultation, selectedPatient } as BotoxDialogResult);
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
