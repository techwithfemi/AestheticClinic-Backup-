import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { DentalImaging } from '../../../models/dental.model';

export interface ImagingPatientOption {
  pNo: string;
  consultId: string;
  firstName: string;
  lastName: string;
  label: string;
}

export interface ImagingDialogResult {
  imaging: DentalImaging;
  selectedPatient: ImagingPatientOption;
}

@Component({
  selector: 'app-imaging-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Imaging Record' : 'Add Imaging Record' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">
          @if (!data.isEdit) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Search Patient</mat-label>
              <input matInput [value]="patientSearchText" (input)="onPatientSearch($event)"
                     placeholder="Type patient name or PNO" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Patient (ConsultID)</mat-label>
              <mat-select formControlName="patientKey" required>
                @for (p of filteredPatientOptions; track p.label) {
                  <mat-option [value]="p.label">{{ p.label }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          } @else {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Patient (ConsultID)</mat-label>
              <input matInput [value]="data.imaging?.patientName || data.imaging?.pno || ''" readonly />
            </mat-form-field>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Imaging Date</mat-label>
            <input matInput [matDatepicker]="datePicker" formControlName="imagingDate" required />
            <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
            <mat-datepicker #datePicker></mat-datepicker>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Imaging Type</mat-label>
            <mat-select formControlName="imagingType">
              <mat-option value="Periapical">Periapical X-ray</mat-option>
              <mat-option value="Bitewing">Bitewing X-ray</mat-option>
              <mat-option value="Panoramic">Panoramic (OPG)</mat-option>
              <mat-option value="CBCT">CBCT / Cone Beam CT</mat-option>
              <mat-option value="Occlusal">Occlusal X-ray</mat-option>
              <mat-option value="Other">Other</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Tooth / Region</mat-label>
            <input matInput formControlName="toothRegion" placeholder="e.g. Upper right quadrant, Tooth 16" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Findings</mat-label>
            <textarea matInput rows="3" formControlName="findings" placeholder="Radiographic findings..."></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Impression / Diagnosis</mat-label>
            <textarea matInput rows="2" formControlName="impression"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Recommendations</mat-label>
            <textarea matInput rows="2" formControlName="recommendations"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>File Name / Reference</mat-label>
            <input matInput formControlName="fileName" placeholder="e.g. IMG-20250101-001.jpg" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Notes</mat-label>
            <textarea matInput rows="2" formControlName="notes"></textarea>
          </mat-form-field>
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">Save</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 480px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .close-btn { position: relative; right: 0; top: 0; }
    .full-width { width: 100%; margin-bottom: 12px; box-sizing: border-box; }
    mat-dialog-content { max-height: 65vh; overflow-y: auto; padding: 0; margin: 0; overflow-x: hidden; }
    mat-form-field { display: block; width: 100%; }
  `]
})
export class ImagingDialogComponent {
  private fb = inject(FormBuilder);
  private _dialogRef = inject(MatDialogRef<ImagingDialogComponent>);
  private _data = inject<{ isEdit: boolean; imaging?: DentalImaging; patientOptions: ImagingPatientOption[] }>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  patientSearchText = '';

  get filteredPatientOptions(): ImagingPatientOption[] {
    const term = this.patientSearchText.trim().toLowerCase();
    if (!term) return this.data.patientOptions;
    return this.data.patientOptions.filter(p =>
      p.label.toLowerCase().includes(term) ||
      p.pNo.toLowerCase().includes(term) ||
      p.consultId.toLowerCase().includes(term));
  }

  form = this.fb.nonNullable.group({
    id: [this.data.imaging?.id ?? 0],
    patientKey: [this.data.isEdit ? (this.data.imaging?.patientName || this.data.imaging?.pno || '') : '', Validators.required],
    imagingDate: [this.data.imaging?.imagingDate ? new Date(this.data.imaging.imagingDate) : new Date(), Validators.required],
    imagingType: [this.data.imaging?.imagingType ?? ''],
    toothRegion: [this.data.imaging?.toothRegion ?? ''],
    findings: [this.data.imaging?.findings ?? ''],
    impression: [this.data.imaging?.impression ?? ''],
    recommendations: [this.data.imaging?.recommendations ?? ''],
    fileName: [this.data.imaging?.fileName ?? ''],
    notes: [this.data.imaging?.notes ?? '']
  });

  onPatientSearch(event: Event): void {
    this.patientSearchText = (event.target as HTMLInputElement).value;
  }

  save(): void {
    if (this.form.invalid) return;

    const val = this.form.getRawValue();
    const selectedPatient = this.data.isEdit
      ? { pNo: this.data.imaging!.pno, consultId: this.data.imaging!.consultId, firstName: '', lastName: '', label: '' }
      : (this.data.patientOptions.find(p => p.label === val.patientKey) ?? {
          pNo: '', consultId: '', firstName: '', lastName: '', label: val.patientKey
        });

    const imaging: DentalImaging = {
      id: val.id,
      pno: this.data.isEdit ? this.data.imaging!.pno : selectedPatient.pNo,
      consultId: this.data.isEdit ? this.data.imaging!.consultId : selectedPatient.consultId,
      imagingDate: val.imagingDate instanceof Date ? val.imagingDate.toISOString() : val.imagingDate,
      imagingType: val.imagingType || undefined,
      toothRegion: val.toothRegion || undefined,
      findings: val.findings || undefined,
      impression: val.impression || undefined,
      recommendations: val.recommendations || undefined,
      fileName: val.fileName || undefined,
      notes: val.notes || undefined
    };

    const result: ImagingDialogResult = { imaging, selectedPatient };
    this._dialogRef.close(result);
  }
}
