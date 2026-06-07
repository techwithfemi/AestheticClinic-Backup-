import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { AestheticConsentStatus, SignAestheticConsent } from '../../../models/aesthetic.model';

export interface ConsentDialogData {
  status: AestheticConsentStatus;
  patientName: string;
}

@Component({
  selector: 'app-consent-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule
  ],
  template: `
    <div class="dialog-content">
      <h2 mat-dialog-title>Patient Consent</h2>

      <mat-dialog-content>
        @if (!data.status.attendanceTaken) {
          <p class="warning">Attendance must be taken before the patient can sign consent.</p>
        } @else {
          <p><strong>Patient:</strong> {{ data.patientName }}</p>
          <p><strong>ConsultId:</strong> {{ data.status.consultId }}</p>
          <p><strong>PNO:</strong> {{ data.status.pNo }}</p>
          <p><strong>Procedure:</strong> {{ data.status.procedureType }}</p>

          <div class="consent-box">
            <h3>{{ data.status.activeTemplate?.title || 'Consent' }}</h3>
            <p>{{ data.status.activeTemplate?.content }}</p>
          </div>

          <form [formGroup]="form">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Signature Name</mat-label>
              <input matInput formControlName="signatureName" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Witnessed By</mat-label>
              <input matInput formControlName="witnessedBy" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Notes</mat-label>
              <textarea matInput rows="3" formControlName="notes"></textarea>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Signature Image (Base64, optional)</mat-label>
              <textarea matInput rows="3" formControlName="signatureImageBase64"></textarea>
            </mat-form-field>

            <mat-checkbox formControlName="accepted">I confirm the patient has reviewed and accepted this consent</mat-checkbox>
          </form>
        }
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="submit()" [disabled]="!data.status.attendanceTaken || form.invalid">Sign Consent</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 560px; max-width: 100%; }
    .full-width { width: 100%; margin-bottom: 12px; }
    .consent-box { border: 1px solid #ddd; border-radius: 8px; padding: 12px; margin-bottom: 16px; background: #fafafa; }
    .warning { color: #c62828; font-weight: 600; }
  `]
})
export class ConsentDialogComponent {
  private readonly fb = inject(FormBuilder);
  readonly dialogRef = inject(MatDialogRef<ConsentDialogComponent>);
  readonly data = inject<ConsentDialogData>(MAT_DIALOG_DATA);

  readonly form = this.fb.nonNullable.group({
    signatureName: ['', Validators.required],
    witnessedBy: [''],
    notes: [''],
    signatureImageBase64: [''],
    accepted: [false, Validators.requiredTrue]
  });

  submit(): void {
    if (this.form.invalid || !this.data.status.activeTemplate?.id || !this.data.status.consultId || !this.data.status.pNo || !this.data.status.procedureType) {
      return;
    }

    const value = this.form.getRawValue();
    const payload: SignAestheticConsent = {
      patientId: this.data.status.latestSignedConsent?.patientId,
      consultId: this.data.status.consultId,
      pNo: this.data.status.pNo,
      procedureType: this.data.status.procedureType,
      consentTemplateId: this.data.status.activeTemplate.id,
      signatureName: value.signatureName,
      witnessedBy: value.witnessedBy,
      notes: value.notes,
      signatureImageBase64: value.signatureImageBase64
    };

    this.dialogRef.close(payload);
  }
}
