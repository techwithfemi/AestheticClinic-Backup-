import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';

@Component({
  selector: 'app-botox',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatTableModule,
    MatIconModule
  ],
  template: `
    <div class="botox-page">
      <mat-card class="form-card">
        <h2>Botox Treatments</h2>
        <form [formGroup]="form" class="form-grid">
          <mat-form-field appearance="outline">
            <mat-label>Patient</mat-label>
            <mat-select formControlName="patientId">
              <mat-option *ngFor="let patient of patients()" [value]="patient.id">
                {{ patient.firstName }} {{ patient.lastName }}
              </mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Treatment Date</mat-label>
            <input matInput type="date" formControlName="consultationDate" />
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Provider</mat-label>
            <input matInput formControlName="provider" />
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

          <mat-slide-toggle formControlName="consentGiven">Consent Given</mat-slide-toggle>
          <mat-slide-toggle formControlName="informationAccepted">Information Accepted</mat-slide-toggle>
        </form>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="save()" [disabled]="loadingIndicator">
            {{ editing() ? 'Update' : 'Add' }} Botox Session
          </button>
          <button mat-stroked-button type="button" (click)="resetForm()">Clear</button>
        </div>
      </mat-card>

      <mat-card>
        <h3>Botox Session History</h3>
        <table mat-table [dataSource]="consultations()" class="data-table">
          <ng-container matColumnDef="patient">
            <th mat-header-cell *matHeaderCellDef>Patient</th>
            <td mat-cell *matCellDef="let row">{{ resolvePatientName(row) }}</td>
          </ng-container>

          <ng-container matColumnDef="date">
            <th mat-header-cell *matHeaderCellDef>Date</th>
            <td mat-cell *matCellDef="let row">{{ row.consultationDate | date:'mediumDate' }}</td>
          </ng-container>

          <ng-container matColumnDef="provider">
            <th mat-header-cell *matHeaderCellDef>Provider</th>
            <td mat-cell *matCellDef="let row">{{ row.provider || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let row">
              <button mat-icon-button type="button" (click)="edit(row)" aria-label="Edit">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button type="button" (click)="remove(row.id)" aria-label="Delete">
                <mat-icon>delete</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
        </table>
      </mat-card>
    </div>
  `,
  styles: [`
    .botox-page { padding: 20px; display: grid; gap: 16px; }
    .form-card h2 { margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    .full-width { grid-column: 1 / -1; }
    .actions { display: flex; gap: 10px; margin-top: 10px; }
    .data-table { width: 100%; }
    @media (max-width: 992px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class BotoxComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly editingId = signal<number | null>(null);
  readonly editing = computed(() => this.editingId() !== null);
  readonly displayedColumns = ['patient', 'date', 'provider', 'actions'];

  readonly form = this.fb.nonNullable.group({
    id: [0],
    patientId: [0, Validators.min(1)],
    consultationDate: ['', Validators.required],
    provider: [''],
    treatmentPlan: [''],
    procedureDescription: [''],
    risksAndComplications: [''],
    postTreatmentInstructions: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading Botox sessions...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients.set(patients);

        this.endpoint.getBotoxConsultationsEndpoint<AestheticConsultation[]>().subscribe({
          next: consultations => {
            this.consultations.set(consultations);
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
          },
          error: error => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage('Load error', 'Unable to load Botox sessions.', MessageSeverity.error, error);
          }
        });
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load error', 'Unable to load patients.', MessageSeverity.error, error);
      }
    });
  }

  save(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation error', 'Patient and treatment date are required.', MessageSeverity.warn);
      return;
    }

    const value = this.form.getRawValue();
    const consultation: AestheticConsultation = {
      id: value.id,
      patientId: value.patientId,
      consultationDate: value.consultationDate,
      procedureType: 'Botox',
      provider: value.provider,
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      risksAndComplications: value.risksAndComplications,
      postTreatmentInstructions: value.postTreatmentInstructions,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.editing() ? 'Updating Botox session...' : 'Saving Botox session...');

    const request = this.editing()
      ? this.endpoint.updateConsultationEndpoint<AestheticConsultation>(value.id, consultation)
      : this.endpoint.createBotoxConsultationEndpoint<AestheticConsultation>(consultation);

    request.subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.resetForm();
        this.load();
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Save error', 'Unable to save Botox session.', MessageSeverity.error, error);
      }
    });
  }

  edit(row: AestheticConsultation): void {
    this.editingId.set(row.id);
    this.form.patchValue({
      id: row.id,
      patientId: row.patientId,
      consultationDate: row.consultationDate ? row.consultationDate.slice(0, 10) : '',
      provider: row.provider ?? '',
      treatmentPlan: row.treatmentPlan ?? '',
      procedureDescription: row.procedureDescription ?? '',
      risksAndComplications: row.risksAndComplications ?? '',
      postTreatmentInstructions: row.postTreatmentInstructions ?? '',
      consentGiven: row.consentGiven ?? true,
      informationAccepted: row.informationAccepted ?? true
    });
  }

  remove(id: number): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting Botox session...');

    this.endpoint.deleteConsultationEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.resetForm();
        this.load();
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete error', 'Unable to delete Botox session.', MessageSeverity.error, error);
      }
    });
  }

  resetForm(): void {
    this.editingId.set(null);
    this.form.reset({
      id: 0,
      patientId: 0,
      consultationDate: '',
      provider: '',
      treatmentPlan: '',
      procedureDescription: '',
      risksAndComplications: '',
      postTreatmentInstructions: '',
      consentGiven: true,
      informationAccepted: true
    });
  }

  resolvePatientName(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      return row.patientName;
    }

    const patient = this.patients().find(p => p.id === row.patientId);
    return patient ? `${patient.firstName} ${patient.lastName}` : `Patient #${row.patientId}`;
  }
}
