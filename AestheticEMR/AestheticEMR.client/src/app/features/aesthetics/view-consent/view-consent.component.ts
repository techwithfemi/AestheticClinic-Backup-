import { Component, OnInit, computed, inject, signal } from '@angular/core';
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
            <mat-label>Search by ConsultId / PNO / Procedure</mat-label>
            <input matInput [value]="searchText()" (input)="searchText.set(($any($event.target).value || '').trim())" />
          </mat-form-field>
          <button mat-stroked-button type="button" (click)="refresh()">Refresh</button>
        </div>

        <table mat-table [dataSource]="filteredConsents()" class="data-table">
          <ng-container matColumnDef="consultId">
            <th mat-header-cell *matHeaderCellDef>ConsultId</th>
            <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
          </ng-container>
          <ng-container matColumnDef="pNo">
            <th mat-header-cell *matHeaderCellDef>PNO</th>
            <td mat-cell *matCellDef="let row">{{ row.pNo }}</td>
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

      @if (selectedConsent()) {
        <mat-card class="detail-card">
          <h3>Consent Detail</h3>
          <p><strong>Patient Signature:</strong> {{ selectedConsent()?.signatureName }}</p>
          <p><strong>Witness:</strong> {{ selectedConsent()?.witnessedBy || '—' }}</p>
          <p><strong>Notes:</strong> {{ selectedConsent()?.notes || '—' }}</p>
          <p><strong>Void Status:</strong> {{ selectedConsent()?.isVoided ? selectedConsent()?.voidReason : 'Active' }}</p>
          <div class="content-box">{{ selectedConsent()?.consentContent }}</div>
          @if (selectedConsent()?.signatureImagePath) {
            <img [src]="selectedConsent()?.signatureImagePath" alt="Signature" class="signature-img" />
          }

          <form [formGroup]="voidForm" class="void-form">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Void Reason</mat-label>
              <textarea matInput rows="3" formControlName="voidReason"></textarea>
            </mat-form-field>
            <div class="actions-row">
              <button mat-stroked-button color="warn" type="button" (click)="voidConsent()" [disabled]="selectedConsent()?.isVoided || voidForm.invalid">Void Consent</button>
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

  loadingIndicator = false;
  readonly consents = signal<AestheticSignedConsent[]>([]);
  readonly selectedConsent = signal<AestheticSignedConsent | null>(null);
  readonly searchText = signal('');
  readonly displayedColumns = ['consultId', 'pNo', 'procedureType', 'signedDate', 'doctorViewed', 'actions'];

  readonly filteredConsents = computed(() => {
    const term = this.searchText().toLowerCase();
    if (!term) {
      return this.consents();
    }

    return this.consents().filter(item => `${item.consultId} ${item.pNo} ${item.procedureType}`.toLowerCase().includes(term));
  });

  readonly voidForm = this.fb.nonNullable.group({
    voidReason: ['', Validators.required]
  });

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading consents...');
    this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ includeVoided: true }).subscribe({
      next: consents => {
        this.consents.set(consents || []);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
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
    const consent = this.selectedConsent();
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
}
