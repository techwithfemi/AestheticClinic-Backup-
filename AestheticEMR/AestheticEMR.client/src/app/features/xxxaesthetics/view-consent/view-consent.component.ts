import { Component, OnInit, computed, inject, signal, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';

import { AestheticSignedConsent, VoidAestheticConsent } from '../../../models/aesthetic.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';

@Component({
  selector: 'app-view-consent',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule],
  template: `
    <div class="page-shell">
      <div class="page-header">
        <div>
          <h2>View Consent</h2>
          <p class="subtitle">Review signed consents, doctor acknowledgements, and void/re-sign workflow status.</p>
        </div>
      </div>

      <mat-card>
        <div class="toolbar-grid">
          <mat-form-field appearance="outline">
            <mat-label>Search by Patient / ConsultId / Procedure</mat-label>
            <input matInput [value]="searchText()" (input)="searchText.set(($any($event.target).value || '').trim())" />
          </mat-form-field>
          <button mat-stroked-button type="button" (click)="refresh()">Refresh</button>
        </div>

        <table mat-table [dataSource]="filteredConsents()" class="data-table">
          <ng-container matColumnDef="consultId">
            <th mat-header-cell *matHeaderCellDef>ConsultId</th>
            <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
          </ng-container>
          <ng-container matColumnDef="patient">
            <th mat-header-cell *matHeaderCellDef>Patient</th>
            <td mat-cell *matCellDef="let row">{{ resolvePatientName(row.pNo) }}</td>
          </ng-container>
          <ng-container matColumnDef="procedureType">
            <th mat-header-cell *matHeaderCellDef>Procedure</th>
            <td mat-cell *matCellDef="let row">{{ row.procedureType }}</td>
          </ng-container>
          <ng-container matColumnDef="signedDate">
            <th mat-header-cell *matHeaderCellDef>Signed</th>
            <td mat-cell *matCellDef="let row">{{ row.signedDate | date:'medium' }}</td>
          </ng-container>
          <ng-container matColumnDef="doctorViewed">
            <th mat-header-cell *matHeaderCellDef>Doctor Viewed</th>
            <td mat-cell *matCellDef="let row">{{ row.doctorViewedDate ? (row.doctorViewedDate | date:'short') : 'No' }}</td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let row">
              <button mat-button type="button" (click)="selectConsent(row)">Open</button>
              <button mat-button type="button" (click)="markViewed(row)" [disabled]="!!row.doctorViewedDate || row.isVoided">Mark Viewed</button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
        </table>
      </mat-card>

      @if (activeConsent()) {
        <mat-card class="detail-card">
          <h3>Consent Detail</h3>
          <p><strong>Patient Signature:</strong> {{ activeConsent().signatureName }}</p>
          <p><strong>Witness:</strong> {{ activeConsent().witnessedBy || '—' }}</p>
          <p><strong>Notes:</strong> {{ activeConsent().notes || '—' }}</p>
          <p><strong>Void Status:</strong> {{ activeConsent().isVoided ? activeConsent().voidReason : 'Active' }}</p>
          <div class="content-box">{{ activeConsent().consentContent }}</div>
          @if (activeConsent().signatureImagePath) {
            <img [src]="activeConsent().signatureImagePath" alt="Signature" class="signature-img" />
          }

          <form [formGroup]="voidForm" class="void-form">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Void Reason</mat-label>
              <textarea matInput rows="3" formControlName="voidReason"></textarea>
            </mat-form-field>
            <div class="actions-row">
              <button mat-stroked-button color="warn" type="button" (click)="voidConsent()" [disabled]="activeConsent().isVoided || voidForm.invalid">Void Consent</button>
            </div>
          </form>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .toolbar-grid { display: grid; grid-template-columns: 1fr auto; gap: 12px; align-items: center; margin-bottom: 12px; }
    .data-table { width: 100%; display: block; overflow-x: auto; -webkit-overflow-scrolling: touch; }
    .detail-card { margin-top: 16px; }
    .content-box { white-space: pre-wrap; border: 1px solid #ddd; border-radius: 8px; padding: 12px; background: #fafafa; margin: 12px 0; }
    .signature-img { max-width: 260px; max-height: 120px; object-fit: contain; border: 1px solid #ddd; border-radius: 6px; padding: 8px; background: #fff; }
    .void-form { margin-top: 16px; }
    .full-width { width: 100%; }
    .actions-row { display: flex; justify-content: flex-end; }
    @media (max-width: 992px) {
      .page-shell { padding: 16px; }
      .toolbar-grid { grid-template-columns: 1fr; }
    }
    @media (max-width: 575.98px) {
      .page-shell { padding: 12px; }
      .actions-row { justify-content: stretch; }
      .actions-row button { width: 100%; min-height: 44px; }
      .signature-img { max-width: 100%; }
    }
  `]
})
export class ViewConsentComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);
  private readonly patientEndpoint = inject(HPatientEndpoint);

  @Input() pNo = '';
  @Input() procedureType = '';

  loadingIndicator = false;
  readonly consents = signal<AestheticSignedConsent[]>([]);
  readonly selectedConsent = signal<AestheticSignedConsent | null>(null);
  readonly searchText = signal('');
  readonly displayedColumns = ['consultId', 'patient', 'procedureType', 'signedDate', 'doctorViewed', 'actions'];

  readonly filteredConsents = computed(() => {
    const term = this.searchText().toLowerCase();
    const patientFilter = this.pNo.trim().toLowerCase();
    const procedureFilter = this.procedureType.trim().toLowerCase();

    return this.consents().filter(item => {
      const matchesSearch = !term || `${item.consultId} ${item.pNo} ${item.procedureType} ${this.resolvePatientName(item.pNo)}`.toLowerCase().includes(term);
      const matchesPatient = !patientFilter || (item.pNo ?? '').trim().toLowerCase() === patientFilter;
      const matchesProcedure = !procedureFilter || (item.procedureType ?? '').trim().toLowerCase() === procedureFilter;
      return matchesSearch && matchesPatient && matchesProcedure;
    });
  });

  readonly activeConsent = computed(() => {
    const current = this.selectedConsent();
    const filtered = this.filteredConsents();

    if (current) {
      const matchesPatient = !this.pNo.trim() || (current.pNo ?? '').trim().toLowerCase() === this.pNo.trim().toLowerCase();
      const matchesProcedure = !this.procedureType.trim() || (current.procedureType ?? '').trim().toLowerCase() === this.procedureType.trim().toLowerCase();
      if (matchesPatient && matchesProcedure) {
        return current;
      }
    }

    return filtered[0] ?? null;
  });

  readonly voidForm = this.fb.nonNullable.group({
    voidReason: ['', Validators.required]
  });

  private readonly patients = signal<HPatient[]>([]);

  ngOnInit(): void {
    this.loadPatients();
    this.refresh();
  }

  private loadPatients(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => this.patients.set(patients || []),
      error: () => this.patients.set([])
    });
  }

  refresh(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading consents...');
    this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ includeVoided: true }).subscribe({
      next: consents => {
        this.consents.set(consents || []);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.selectedConsent.set(this.filteredConsents()[0] ?? null);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', 'Unable to retrieve signed consents.', MessageSeverity.error, error);
      }
    });
  }

  selectConsent(consent: AestheticSignedConsent): void {
    this.selectedConsent.set(consent);
    this.voidForm.reset({ voidReason: '' });
  }

  markViewed(consent: AestheticSignedConsent): void {
    this.endpoint.markConsentViewedEndpoint<AestheticSignedConsent>(consent.id).subscribe({
      next: updated => {
        this.selectedConsent.set(updated);
        this.refresh();
        this.alertService.showMessage('Updated', 'Consent marked as viewed.', MessageSeverity.success);
      },
      error: error => {
        this.alertService.showStickyMessage('Update Error', 'Unable to mark consent as viewed.', MessageSeverity.error, error);
      }
    });
  }

  voidConsent(): void {
    const consent = this.activeConsent();
    if (!consent || this.voidForm.invalid) {
      return;
    }

    const payload: VoidAestheticConsent = this.voidForm.getRawValue();
    this.endpoint.voidConsentEndpoint<AestheticSignedConsent>(consent.id, payload).subscribe({
      next: updated => {
        this.selectedConsent.set(updated);
        this.refresh();
        this.alertService.showMessage('Voided', 'Consent voided successfully. Patient can now re-sign if required.', MessageSeverity.success);
      },
      error: error => {
        this.alertService.showStickyMessage('Void Error', 'Unable to void consent.', MessageSeverity.error, error);
      }
    });
  }

  resolvePatientName(pNo?: string): string {
    const normalized = (pNo ?? '').trim().toLowerCase();
    if (!normalized) {
      return 'Unknown patient';
    }

    const patient = this.patients().find(p => (p.pno ?? '').trim().toLowerCase() === normalized);
    if (!patient) {
      return pNo ?? 'Unknown patient';
    }

    return [patient.pSurName, patient.pFirstname].filter(Boolean).join(' ').trim() || (pNo ?? 'Unknown patient');
  }
}
