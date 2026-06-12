import { Component, Input, OnInit, computed, inject, signal } from '@angular/core';
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
import { ConfigurationService } from '../../../services/configuration.service';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';

@Component({
  selector: 'app-view-consent',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule, MatTableModule],
  template: `
    <div class="page-shell" [class.embedded]="isEmbedded()">

      @if (!isEmbedded()) {
        <div class="page-header">
          <div>
            <h2>View Consent</h2>
            <p class="subtitle">Review signed consents — printer-friendly consent report</p>
          </div>
          <div class="header-actions">
            <button mat-stroked-button type="button" (click)="refresh()">Refresh</button>
          </div>
        </div>

        <mat-card>
          <div class="toolbar-grid">
            <mat-form-field appearance="outline">
              <mat-label>Search by Patient / ConsultId / Procedure</mat-label>
              <input matInput [value]="searchText()" (input)="searchText.set(($any($event.target).value || '').trim())" />
            </mat-form-field>
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
      }

      @if (activeConsent()) {
        <div class="report-wrap" [class.embedded-report]="isEmbedded()">

          <!-- A4-portrait consent document -->
          <div class="report-paper">

            <!-- Teal header banner -->
            <div class="report-banner">
              <div class="banner-brand">AesthForm EMR</div>
              <div class="banner-sub">Electronic Medical Records for Aesthetic Practices</div>
            </div>

            <!-- Document title -->
            <div class="form-title">CONSENT FORM</div>

            <!-- Patient info block -->
            <div class="info-block">
              <div class="info-row"><strong>Patient Name:</strong> {{ resolvePatientName(activeConsent()?.pNo) }}</div>
              <div class="info-row"><strong>Procedure Detail:</strong> {{ activeConsent()?.procedureType }}</div>
            </div>

            <!-- Read-only consent acknowledgement checkboxes -->
            <div class="check-row">
              <span class="chk">{{ activeConsent()?.consentContent ? '☑' : '☐' }}</span>
              <span class="chk-label">I have read and understand the information provided</span>
              <span class="chk chk-gap">{{ !activeConsent()?.isVoided ? '☑' : '☐' }}</span>
              <span class="chk-label">I consent to the treatment</span>
            </div>

            <!-- Procedure Description -->
            <div class="doc-section">
              <div class="doc-heading">Procedure Description</div>
              <div class="doc-body">{{ activeConsent()?.consentContent || 'No procedure description provided.' }}</div>
            </div>
            <hr class="doc-rule" />

            <!-- Risks and Complications -->
            <div class="doc-section">
              <div class="doc-heading">Risks and Complications</div>
              <div class="doc-body">The risks and possible complications associated with this procedure were discussed with the patient. These include but are not limited to bruising, swelling, infection, asymmetry, and adverse reactions. The patient acknowledges understanding and accepts these risks.</div>
            </div>
            <hr class="doc-rule" />

            <!-- Post-Treatment Care Instructions -->
            <div class="doc-section">
              <div class="doc-heading">Post-Treatment Care Instructions</div>
              <div class="doc-body">{{ activeConsent()?.notes || 'Follow all post-treatment instructions provided by your practitioner. Avoid sun exposure and strenuous activity as directed. Contact the clinic immediately if you experience unusual pain, swelling, or any adverse reactions.' }}</div>
            </div>

            <!-- Full-width separator between content and signatures -->
            <hr class="sig-section-rule" />

            <!-- Signature section -->
            <div class="signatures">
              <div class="sig-col">
                <div class="sig-label">Patient Signature:</div>
                @if (activeConsent()?.signatureImageBase64) {
                  <img [src]="activeConsent()!.signatureImageBase64" alt="Patient signature" class="sig-image" />
                } @else if (activeConsent()?.signatureImagePath) {
                  <img [src]="resolveImageUrl(activeConsent()!.signatureImagePath)" alt="Patient signature" class="sig-image" />
                } @else {
                  <div class="sig-spacer"></div>
                }
                <div class="sig-underline"></div>
                <div class="sig-name">{{ activeConsent()?.signatureName || '—' }}</div>
                @if (activeConsent()?.witnessedBy) {
                  <div class="sig-witness">Witnessed by: {{ activeConsent()!.witnessedBy }}</div>
                }
              </div>
              <div class="sig-col">
                <div class="sig-label">Provider Signature:</div>
                <div class="sig-spacer"></div>
                <div class="sig-underline"></div>
                <div class="sig-provider">{{ resolveProviderName(activeConsent()?.signedBy) }}</div>
                <div class="sig-date-row">
                  <strong>Date:</strong>
                  <span>{{ activeConsent()?.doctorViewedDate ? (activeConsent()!.doctorViewedDate | date:'MM/dd/yyyy') : 'MM/DD/YYYY' }}</span>
                </div>
              </div>
            </div>

            @if (activeConsent()?.isVoided) {
              <div class="void-stamp">VOIDED — {{ activeConsent()?.voidReason || 'No reason provided' }}</div>
            }

          </div>

          @if (!isEmbedded()) {
            <form [formGroup]="voidForm" class="void-form">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Void Reason</mat-label>
                <textarea matInput rows="2" formControlName="voidReason"></textarea>
              </mat-form-field>
              <div class="actions-row">
                <button mat-stroked-button color="warn" type="button" (click)="voidConsent()" [disabled]="activeConsent()?.isVoided || voidForm.invalid">Void Consent</button>
                <button mat-stroked-button type="button" (click)="print()">Print</button>
              </div>
            </form>
          }

        </div>
      }

    </div>
  `,
  styles: [`
    /* Shell */
    .page-shell { padding: 20px; font-family: Roboto, "Helvetica Neue", Arial, sans-serif; background: #f3f4f6; min-height: 100vh; }
    .page-shell.embedded { padding: 0; background: transparent; min-height: auto; }

    /* Page header (standalone only) */
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .header-actions { display: flex; gap: 8px; }

    /* Toolbar / table */
    .toolbar-grid { display: grid; grid-template-columns: 1fr; gap: 12px; margin-bottom: 12px; }
    .data-table { width: 100%; display: block; overflow-x: auto; -webkit-overflow-scrolling: touch; }

    /* Report wrap */
    .report-wrap { margin-top: 20px; display: flex; flex-direction: column; align-items: center; gap: 16px; }
    .report-wrap.embedded-report { margin-top: 8px; }

    /* ─── CONSENT PAPER ─── */
    .report-paper {
      width: 760px;
      max-width: 100%;
      background: #ffffff;
      border-radius: 6px;
      box-shadow: 0 4px 28px rgba(0,0,0,.11);
      overflow: hidden;
      color: #1a1a2e;
      font-size: 14px;
      -webkit-print-color-adjust: exact;
      print-color-adjust: exact;
    }

    /* Banner */
    .report-banner {
      background: linear-gradient(135deg, #0e7490 0%, #0284c7 100%);
      padding: 22px 32px;
      text-align: center;
      color: #fff;
    }
    .banner-brand { font-size: 28px; font-weight: 700; letter-spacing: .5px; }
    .banner-sub { font-size: 12.5px; margin-top: 5px; opacity: .88; }

    /* Document title */
    .form-title {
      font-size: 17px;
      font-weight: 700;
      text-align: center;
      letter-spacing: 3px;
      padding: 18px 32px 10px;
      color: #1a1a2e;
    }

    /* Patient info block */
    .info-block { padding: 0 32px 4px; }
    .info-row { padding: 3px 0; font-size: 13.5px; color: #1a1a2e; }
    .info-row strong { font-weight: 600; }

    /* Acknowledgement checkboxes */
    .check-row {
      display: flex;
      align-items: center;
      flex-wrap: wrap;
      gap: 5px;
      padding: 10px 32px;
      font-size: 13px;
      color: #374151;
    }
    .chk { font-size: 17px; color: #0e7490; flex-shrink: 0; }
    .chk-label { margin-right: 8px; }
    .chk-gap { margin-left: 14px; }

    /* Content sections */
    .doc-section { padding: 10px 32px 6px; }
    .doc-heading { font-weight: 700; font-size: 14px; color: #1a1a2e; margin-bottom: 6px; }
    .doc-body { font-size: 13px; color: #374151; line-height: 1.65; white-space: pre-wrap; }
    .doc-rule { border: none; border-top: 1px solid #e5e7eb; margin: 6px 32px; }

    /* Full-width rule above signatures */
    .sig-section-rule { border: none; border-top: 1.5px solid #374151; margin: 10px 0 0; }

    /* Signatures */
    .signatures { display: flex; justify-content: space-between; gap: 20px; padding: 14px 32px 24px; }
    .sig-col { flex: 1; }
    .sig-label { font-size: 13px; font-weight: 700; color: #1a1a2e; margin-bottom: 8px; }
    .sig-image { max-width: 220px; max-height: 80px; border: 1px solid #e5e7eb; padding: 4px; border-radius: 4px; display: block; }
    .sig-spacer { min-height: 56px; }
    .sig-underline { border-bottom: 1.5px solid #374151; width: 260px; max-width: 100%; margin: 4px 0 6px; }
    .sig-name { font-size: 13px; font-weight: 600; color: #374151; text-align: center; width: 260px; max-width: 100%; }
    .sig-witness { font-size: 12px; color: #6b7280; margin-top: 3px; text-align: center; width: 260px; max-width: 100%; }
    .sig-provider { font-size: 13px; font-weight: 500; color: #374151; }
    .sig-date-row { display: flex; gap: 8px; align-items: center; font-size: 13px; color: #374151; margin-top: 6px; }

    /* Void stamp */
    .void-stamp {
      margin: 4px 32px 20px;
      padding: 8px 16px;
      border: 2px solid #dc2626;
      color: #dc2626;
      font-weight: 700;
      font-size: 13px;
      text-align: center;
      letter-spacing: 1px;
      border-radius: 4px;
      background: #fef2f2;
    }

    /* Void form / actions (standalone only) */
    .void-form { width: 760px; max-width: 100%; display: flex; gap: 12px; align-items: flex-start; }
    .full-width { flex: 1; }
    .actions-row { display: flex; gap: 8px; align-items: center; padding-top: 4px; }

    /* Print */
    @media print {
      .page-shell { background: #fff; padding: 0; }
      .report-wrap { margin: 0; }
      .report-paper { box-shadow: none; border-radius: 0; width: 100%; }
      .void-form { display: none !important; }
      mat-card { display: none !important; }
      .page-header { display: none !important; }
    }

    @media (max-width: 820px) {
      .report-paper, .void-form { width: 100%; }
      .report-banner { padding: 16px 16px; }
      .info-block, .check-row, .doc-section, .signatures { padding-left: 16px; padding-right: 16px; }
      .doc-rule { margin-left: 16px; margin-right: 16px; }
    }

    @media (max-width: 575.98px) {
      .page-shell { padding: 12px; }
      .signatures { flex-direction: column; }
      .sig-col { width: 100%; }
      .actions-row { flex-direction: column; }
      .actions-row button { width: 100%; min-height: 44px; }
    }
  `]
})
export class ViewConsentComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly configurations = inject(ConfigurationService);

  loadingIndicator = false;
  readonly consents = signal<AestheticSignedConsent[]>([]);
  readonly selectedConsent = signal<AestheticSignedConsent | null>(null);
  readonly searchText = signal('');
  readonly displayedColumns = ['consultId', 'patient', 'procedureType', 'signedDate', 'doctorViewed', 'actions'];

  private readonly _embeddedConsent = signal<AestheticSignedConsent | null>(null);

  @Input() set embeddedConsent(value: AestheticSignedConsent | null) {
    this._embeddedConsent.set(value ?? null);
  }

  readonly isEmbedded = computed(() => this._embeddedConsent() !== null);
  readonly activeConsent = computed(() => this._embeddedConsent() ?? this.selectedConsent());

  readonly filteredConsents = computed(() => {
    const term = this.searchText().toLowerCase();
    if (!term) return this.consents();
    return this.consents().filter(item =>
      `${item.consultId} ${item.pNo} ${item.procedureType} ${this.resolvePatientName(item.pNo)}`.toLowerCase().includes(term)
    );
  });

  readonly voidForm = this.fb.nonNullable.group({
    voidReason: ['', Validators.required]
  });

  private readonly patients = signal<HPatient[]>([]);

  ngOnInit(): void {
    this.loadPatients();
    if (!this._embeddedConsent()) {
      this.refresh();
    }
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
    if (!consent || this.voidForm.invalid) return;
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

  print(): void {
    window.print();
  }

  resolvePatientName(pNo?: string): string {
    const normalized = (pNo ?? '').trim().toLowerCase();
    if (!normalized) return 'Unknown patient';
    const patient = this.patients().find(p => (p.pno ?? '').trim().toLowerCase() === normalized);
    if (!patient) return pNo ?? 'Unknown patient';
    return [patient.pSurName, patient.pFirstname].filter(Boolean).join(' ').trim() || (pNo ?? 'Unknown patient');
  }

  resolveImageUrl(path?: string): string {
    if (!path) return '';
    if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('data:')) return path;
    const base = (this.configurations.baseUrl || '').replace(/\/$/, '');
    return `${base}${path.startsWith('/') ? '' : '/'}${path}`;
  }

  resolveProviderName(value?: string): string {
    if (!value) return '';
    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return guidPattern.test(value.trim()) ? '' : value;
  }
}
