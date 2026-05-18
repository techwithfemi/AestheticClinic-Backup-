import { HttpClient } from '@angular/common/http';
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
        <div class="patient-header">
          @if (selectedPatientInfo?.photo) {
            <button class="photo-button" type="button" (click)="togglePhotoZoom()" [attr.aria-label]="isPhotoZoomed ? 'Zoom out photo' : 'Zoom in photo'">
              <img class="patient-photo" [class.zoomed]="isPhotoZoomed" [src]="getPatientPhotoSource(selectedPatientInfo?.photo)" alt="Patient photo" />
            </button>
          } @else {
            <div class="patient-photo placeholder">
              <mat-icon>person</mat-icon>
            </div>
          }

          <div class="patient-meta">
            <div class="meta-item"><span class="label">Full Name:</span> <span>{{ selectedPatientFullName }}</span></div>
            <div class="meta-item"><span class="label">Age:</span> <span>{{ selectedPatientAge ?? '—' }}</span></div>
            <div class="meta-item"><span class="label">Company:</span> <span>{{ selectedPatientInfo?.company || '—' }}</span></div>
            <div class="meta-item"><span class="label">Phone:</span> <span>{{ selectedPatientInfo?.phoneNumber || '—' }}</span></div>
          </div>
        </div>

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
    .patient-header { display: flex; gap: 12px; align-items: center; border: 1px solid #e0e0e0; border-radius: 8px; min-height: 88px; padding: 10px 12px; margin-bottom: 12px; background: #fafafa; }
    .photo-button { padding: 0; border: none; background: transparent; cursor: zoom-in; border-radius: 50%; line-height: 0; }
    .photo-button:focus-visible { outline: 2px solid #1976d2; outline-offset: 2px; }
    .patient-photo { width: 64px; height: 64px; border-radius: 50%; object-fit: cover; background: #f1f1f1; border: 1px solid #ddd; display: flex; align-items: center; justify-content: center; color: #888; transition: transform 0.2s ease; transform-origin: center; }
    .patient-photo.zoomed { transform: scale(2.2); }
    .patient-photo.placeholder mat-icon { font-size: 32px; width: 32px; height: 32px; }
    .patient-meta { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 6px 12px; min-width: 0; flex: 1; }
    .meta-item { font-size: 0.9rem; color: #444; display: flex; gap: 6px; min-width: 0; }
    .meta-item span:last-child { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .meta-item .label { color: #666; min-width: 72px; }
  `]
})
export class SpaDialogComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  dialogRef = inject(MatDialogRef<SpaDialogComponent>);

  @ViewChild('patientSelect') patientSelect?: MatSelect;

  patientSearchText = '';
  serviceTypes: string[] = [];

  isPhotoZoomed = false;

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
    consentGiven: [true],
    informationAccepted: [true]
  });

  private _data = inject<{ isEdit: boolean; consultation?: AestheticConsultation; patientOptions: SpaPatientOption[] }>(MAT_DIALOG_DATA);
  get data() { return this._data; }

  constructor() {
    this.loadServiceTypes();

    if (this.data.isEdit && this.data.consultation) {
      const selectedOption = this.data.patientOptions.find(x => x.patientId === this.data.consultation!.patientId);

      this.form.patchValue({
        id: this.data.consultation.id,
        patientKey: selectedOption?.label ?? '',
        consultationDate: this.toDate(this.data.consultation.consultationDate) ?? new Date(),
        indication: this.data.consultation.indication ?? '',
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

  private loadServiceTypes(): void {
    this.http.get<SpaStaticLists>('assets/module-settings/spa.json').subscribe({
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

  get selectedPatientInfo(): SpaPatientOption | undefined {
    const key = this.form.controls.patientKey.value;
    return this.data.patientOptions.find(x => x.label === key);
  }

  get selectedPatientFullName(): string {
    const selected = this.selectedPatientInfo;
    if (!selected) {
      return '—';
    }

    const resolved = selected.fullName ?? `${selected.firstName} ${selected.lastName}`.trim();
    return resolved || '—';
  }

  get selectedPatientAge(): number | null {
    const dob = this.selectedPatientInfo?.dateOfBirth;
    if (!dob) {
      return null;
    }

    const date = new Date(dob);
    if (Number.isNaN(date.getTime())) {
      return null;
    }

    const today = new Date();
    let age = today.getFullYear() - date.getFullYear();
    const monthDiff = today.getMonth() - date.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < date.getDate())) {
      age--;
    }

    return age >= 0 ? age : null;
  }

  togglePhotoZoom(): void {
    this.isPhotoZoomed = !this.isPhotoZoomed;
  }

  getPatientPhotoSource(photo?: string): string {
    const trimmed = photo?.trim();
    if (!trimmed) {
      return '';
    }

    return trimmed.startsWith('data:') ? trimmed : `data:image/jpeg;base64,${trimmed}`;
  }
}
