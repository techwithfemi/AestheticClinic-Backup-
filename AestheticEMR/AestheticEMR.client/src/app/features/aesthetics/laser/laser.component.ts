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
import { MatChipsModule } from '@angular/material/chips';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';

@Component({
  selector: 'app-laser',
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
    MatIconModule,
    MatChipsModule
  ],
  template: `
    <div class="laser-page">

      <mat-card class="form-card">
        <h2>Laser Treatments</h2>
        <p class="subtitle">Record laser sessions including device settings, skin assessment, session progress and safety checks.</p>

        <form [formGroup]="form" class="form-grid">

          <!-- Patient & Session Info -->
          <mat-form-field appearance="outline">
            <mat-label>Patient</mat-label>
            <mat-select formControlName="patientId" required>
              @for (p of patients(); track p.id) {
                <mat-option [value]="p.id">{{ p.firstName }} {{ p.lastName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Session Date</mat-label>
            <input matInput type="date" formControlName="consultationDate" required />
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Provider / Operator</mat-label>
            <input matInput formControlName="provider" />
          </mat-form-field>

          <mat-form-field appearance="outline">
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

          <!-- Device & Settings -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Device & Settings (wavelength, fluence, pulse width, spot size)</mat-label>
            <textarea matInput rows="2" formControlName="deviceSettings"
              placeholder="e.g. Nd:YAG 1064nm | Fluence 18 J/cm² | Pulse 10ms | Spot 18mm"></textarea>
          </mat-form-field>

          <!-- Treatment Plan -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Treatment Plan &amp; Target Area</mat-label>
            <textarea matInput rows="2" formControlName="treatmentPlan"
              placeholder="e.g. Full-face rejuvenation, 6-session package, session 2/6"></textarea>
          </mat-form-field>

          <!-- Session Notes -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Session Notes / Observations</mat-label>
            <textarea matInput rows="2" formControlName="procedureDescription"></textarea>
          </mat-form-field>

          <!-- Post-treatment -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Post-Treatment Instructions</mat-label>
            <textarea matInput rows="2" formControlName="postTreatmentInstructions"></textarea>
          </mat-form-field>

          <!-- Contraindications / Risks -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Contraindications &amp; Adverse Events</mat-label>
            <textarea matInput rows="2" formControlName="risksAndComplications"
              placeholder="Document any immediate reactions, contraindications checked..."></textarea>
          </mat-form-field>

          <!-- Consent toggles -->
          <div class="toggles">
            <mat-slide-toggle formControlName="consentGiven" color="primary">Consent Obtained</mat-slide-toggle>
            <mat-slide-toggle formControlName="informationAccepted" color="primary">Patient Information Accepted</mat-slide-toggle>
          </div>

        </form>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="save()" [disabled]="loadingIndicator">
            <mat-icon>save</mat-icon>
            {{ editing() ? 'Update Session' : 'Record Session' }}
          </button>
          <button mat-stroked-button type="button" (click)="resetForm()">
            <mat-icon>clear</mat-icon>
            Clear
          </button>
        </div>
      </mat-card>

      <!-- Session History -->
      <mat-card>
        <h3>Laser Session History</h3>

        @if (consultations().length === 0 && !loadingIndicator) {
          <p class="empty-state">No laser sessions recorded yet.</p>
        }

        @if (consultations().length > 0) {
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

            <ng-container matColumnDef="skin">
              <th mat-header-cell *matHeaderCellDef>Skin Type</th>
              <td mat-cell *matCellDef="let row">{{ row.skinAssessment || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="device">
              <th mat-header-cell *matHeaderCellDef>Device Settings</th>
              <td mat-cell *matCellDef="let row" class="device-cell">{{ row.deviceSettings || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="consent">
              <th mat-header-cell *matHeaderCellDef>Consent</th>
              <td mat-cell *matCellDef="let row">
                <mat-icon [class]="row.consentGiven ? 'icon-ok' : 'icon-warn'">
                  {{ row.consentGiven ? 'check_circle' : 'cancel' }}
                </mat-icon>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="edit(row)" aria-label="Edit session">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="remove(row.id)" aria-label="Delete session">
                  <mat-icon>delete</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
          </table>
        }
      </mat-card>

    </div>
  `,
  styles: [`
    .laser-page { padding: 20px; display: grid; gap: 16px; }
    .subtitle { color: #666; margin: 0 0 16px; font-size: 0.9rem; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 14px; }
    .full-width { grid-column: 1 / -1; }
    .toggles { grid-column: 1 / -1; display: flex; gap: 24px; flex-wrap: wrap; padding: 4px 0; }
    .actions { display: flex; gap: 12px; margin-top: 12px; }
    .data-table { width: 100%; }
    .device-cell { max-width: 280px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .empty-state { color: #888; padding: 16px 0; }
    .icon-ok { color: #2e7d32; }
    .icon-warn { color: #c62828; }
    @media (max-width: 992px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class LaserComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly editingId = signal<number | null>(null);
  readonly editing = computed(() => this.editingId() !== null);
  readonly displayedColumns = ['patient', 'date', 'provider', 'skin', 'device', 'consent', 'actions'];

  readonly form = this.fb.nonNullable.group({
    id: [0],
    patientId: [0, Validators.min(1)],
    consultationDate: ['', Validators.required],
    provider: [''],
    skinAssessment: [''],
    deviceSettings: [''],
    treatmentPlan: [''],
    procedureDescription: [''],
    postTreatmentInstructions: [''],
    risksAndComplications: [''],
    consentGiven: [true],
    informationAccepted: [true]
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading laser sessions...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients.set(patients);

        this.endpoint.getLaserConsultationsEndpoint<AestheticConsultation[]>().subscribe({
          next: consultations => {
            this.consultations.set(consultations);
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
          },
          error: (error: unknown) => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage('Load error', 'Unable to load laser sessions.', MessageSeverity.error, error);
          }
        });
      },
      error: (error: unknown) => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load error', 'Unable to load patients.', MessageSeverity.error, error);
      }
    });
  }

  save(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation error', 'Patient and session date are required.', MessageSeverity.warn);
      return;
    }

    const value = this.form.getRawValue();
    const consultation: AestheticConsultation = {
      id: value.id,
      patientId: value.patientId,
      consultationDate: value.consultationDate,
      procedureType: 'Laser',
      provider: value.provider,
      skinAssessment: value.skinAssessment,
      deviceSettings: value.deviceSettings,
      treatmentPlan: value.treatmentPlan,
      procedureDescription: value.procedureDescription,
      postTreatmentInstructions: value.postTreatmentInstructions,
      risksAndComplications: value.risksAndComplications,
      consentGiven: value.consentGiven,
      informationAccepted: value.informationAccepted
    };

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.editing() ? 'Updating session...' : 'Recording session...');

    const request = this.editing()
      ? this.endpoint.updateConsultationEndpoint<AestheticConsultation>(value.id, consultation)
      : this.endpoint.createLaserConsultationEndpoint<AestheticConsultation>(consultation);

    request.subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.resetForm();
        this.load();
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Save error', 'Unable to save laser session.', MessageSeverity.error, error);
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
      skinAssessment: row.skinAssessment ?? '',
      deviceSettings: row.deviceSettings ?? '',
      treatmentPlan: row.treatmentPlan ?? '',
      procedureDescription: row.procedureDescription ?? '',
      postTreatmentInstructions: row.postTreatmentInstructions ?? '',
      risksAndComplications: row.risksAndComplications ?? '',
      consentGiven: row.consentGiven ?? true,
      informationAccepted: row.informationAccepted ?? true
    });
  }

  remove(id: number): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting laser session...');

    this.endpoint.deleteConsultationEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.resetForm();
        this.load();
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete error', 'Unable to delete laser session.', MessageSeverity.error, error);
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
      skinAssessment: '',
      deviceSettings: '',
      treatmentPlan: '',
      procedureDescription: '',
      postTreatmentInstructions: '',
      risksAndComplications: '',
      consentGiven: true,
      informationAccepted: true
    });
  }

  resolvePatientName(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      return row.patientName;
    }

    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName}` : `Patient #${row.patientId}`;
  }
}
