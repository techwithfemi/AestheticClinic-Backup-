import { Component, OnInit, computed, inject, signal, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import SignaturePad from 'signature_pad';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { ModuleSettingsService } from '../../../services/module-settings.service';
import {
  AestheticConsentTemplate,
  AestheticSignedConsent,
  SignAestheticConsent,
  AestheticConsentStatus
} from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';

interface ConsentEntryDialogData {
  consentId?: number;
  consultId?: string;
  pNo?: string;
}

@Component({
  selector: 'app-consent-form-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    AttendanceSummaryComponent
  ],
  template: `
    <div class="consent-dialog-container">
      <div class="dialog-header">
        <h2>{{ isEditing() ? 'Edit Consent Form' : 'Add Consent Form' }}</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="closeDialog()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div class="page-header">
        <div>
          <h3>Consent Entry</h3>
          <p class="subtitle">Select patient and capture signature for procedure consent.</p>
          @if (selectedAttendanceSummary()) {
            <div class="header-attendance-summary">
              <app-attendance-summary [attendance]="selectedAttendanceSummary()!" [compact]="true"></app-attendance-summary>
            </div>
          }
        </div>
      </div>

      <mat-card>
        <form [formGroup]="form" class="form-shell">
          <div class="form-section">
            <h4>Patient Selection</h4>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Patient</mat-label>
              <mat-select [value]="selectedConsultId()" (selectionChange)="onPatientChanged($event.value)">
                <mat-option value="">Select Patient</mat-option>
                @for (item of patientAttendanceOptions(); track item.trackKey) {
                  <mat-option [value]="item.consultId">{{ item.label }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Procedure Type</mat-label>
              <mat-select [value]="selectedProcedureType()" (selectionChange)="onProcedureTypeChanged($event.value)">
                @for (procedureType of procedureTypes(); track procedureType) {
                  <mat-option [value]="procedureType">{{ procedureType }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>

          <div class="form-section">
            <h4>Consent Template</h4>

            @if (!selectedAttendance()) {
              <div class="consent-box empty">Select Patient to load consent template.</div>
            } @else if (!activeTemplate()) {
              <div class="consent-box empty">No template available for {{ selectedProcedureType() }}.</div>
            } @else {
              <div class="consent-box">{{ activeTemplate().content || 'No content available.' }}</div>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Signature Name</mat-label>
                <input matInput formControlName="signatureName" placeholder="Patient name or initials" />
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Witnessed By</mat-label>
                <input matInput formControlName="witnessedBy" placeholder="Witness name (optional)" />
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Notes</mat-label>
                <textarea matInput rows="3" formControlName="notes" placeholder="Additional notes (optional)"></textarea>
              </mat-form-field>

              <div class="signature-pad-wrap full-width">
                <div class="signature-pad-header">
                  <span>Patient Signature</span>
                  <button mat-stroked-button type="button" (click)="clearSignature()">Clear</button>
                </div>
                <canvas
                  #signatureCanvas
                  class="signature-canvas"></canvas>
              </div>

              <div class="signature-hint">Draw the patient's signature using mouse, touch, or stylus.</div>
            }
          </div>

          <div class="actions-row">
            <button mat-stroked-button type="button" (click)="closeDialog()" [disabled]="loadingIndicator">
              Cancel
            </button>
            <button mat-raised-button color="primary" type="button" (click)="saveConsent()" 
              [disabled]="!canSave() || loadingIndicator">
              {{ isEditing() ? 'Update' : 'Save' }}
            </button>
          </div>
        </form>
      </mat-card>
    </div>
  `,
  styles: [`
    :host { display: block; }
    .consent-dialog-container { padding: 20px; max-height: 90vh; overflow-y: auto; box-sizing: border-box; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; padding-bottom: 12px; border-bottom: 1px solid #e0e0e0; margin-bottom: 16px; }
    .dialog-header h2 { margin: 0; font-size: 1.25rem; }
    .close-btn { color: #999; }
    .close-btn:hover { color: #333; }

    .page-header { margin-bottom: 16px; }
    .page-header > div { flex: 1; }
    .page-header h3 { margin: 0 0 4px 0; font-size: 1.1rem; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .header-attendance-summary { margin-top: 12px; }

    .form-shell { padding: 12px; }
    .form-section { margin-bottom: 20px; }
    .form-section:last-child { margin-bottom: 0; }
    .form-section h4 { margin: 0 0 12px 0; font-size: 0.95rem; font-weight: 500; color: #333; }

    .full-width { width: 100%; }
    .consent-box { white-space: pre-wrap; border: 1px solid #ddd; border-radius: 8px; padding: 12px; min-height: 120px; background: #fafafa; margin-bottom: 16px; font-size: 0.9rem; line-height: 1.5; }
    .consent-box.empty { color: #999; display: flex; align-items: center; justify-content: center; }

    .signature-pad-wrap { display: grid; gap: 8px; margin: 16px 0; }
    .signature-pad-header { display: flex; align-items: center; justify-content: space-between; }
    .signature-canvas {
      width: 100%;
      height: 180px;
      border: 1px dashed #9aa7bd;
      border-radius: 8px;
      background: #fff !important;
      background-color: #fff !important;
      touch-action: none;
      cursor: crosshair;
    }
    .signature-hint { color: #667085; font-size: 0.85rem; margin-bottom: 16px; }

    .actions-row { display: flex; justify-content: flex-end; gap: 12px; margin-top: 20px; flex-wrap: wrap; }

    @media (max-width: 767.98px) {
      .consent-dialog-container { padding: 12px; }
      .dialog-header h2 { font-size: 1.1rem; }
      .actions-row { justify-content: stretch; }
      .actions-row button { width: 100%; min-height: 44px; }
      .signature-canvas { height: 160px; }
    }

    @media (max-width: 575.98px) {
      .consent-dialog-container { padding: 10px; }
      .form-section h4 { font-size: 0.9rem; }
      .consent-box { min-height: 100px; font-size: 0.85rem; }
    }
  `]
})
export class ConsentFormEntryDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly dialogRef = inject(MatDialogRef<ConsentFormEntryDialogComponent>);
  private readonly data = inject<ConsentEntryDialogData>(MAT_DIALOG_DATA);
  private readonly aestheticEndpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly hPatientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly moduleSettings = inject(ModuleSettingsService);

  loadingIndicator = false;

  readonly todayVisits = signal<QryhvisitsForToday[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly templates = signal<AestheticConsentTemplate[]>([]);
  readonly procedureTypes = signal<string[]>(['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']);
  readonly selectedConsultId = signal<string>('');
  readonly selectedProcedureType = signal<string>('Procedures');
  readonly existingConsent = signal<AestheticSignedConsent | null>(null);

  @ViewChild('signatureCanvas')
  set signatureCanvas(value: ElementRef<HTMLCanvasElement> | undefined) {
    this._signatureCanvas = value;
    if (value) {
      this.initializeSignaturePad();
    }
  }

  private _signatureCanvas?: ElementRef<HTMLCanvasElement>;
  private signaturePad: SignaturePad | null = null;

  readonly selectedAttendance = computed(() => {
    const consultId = this.selectedConsultId();
    const visits = this.todayVisits();
    return visits.find(v => v.consultId === consultId) || null;
  });

  readonly selectedAttendanceSummary = computed<VwhRecord | null>(() => {
    const attendance = this.selectedAttendance();
    if (!attendance) return null;

    const patient = this.legacyPatients().find(p => this.normalizePno(p.pno) === this.normalizePno(attendance.pNo));
    const dob = patient?.dob;
    const fullName = attendance.fullname?.trim() 
      || `${patient?.pSurName || ''} ${patient?.pFirstname || ''}`.trim()
      || attendance.pNo;

    return {
      consultId: attendance.consultId || '—',
      pNo: attendance.pNo,
      clinicType: attendance.clinicType,
      clientCat: attendance.clientCat,
      coyname: attendance.coyName,
      retainName: attendance.retainName,
      fullname: fullName,
      dob,
      age: this.calculateAge(dob)
    };
  });

  readonly activeTemplate = computed(() => {
    const procedureType = this.selectedProcedureType();
    const templates = this.templates();
    const normalized = (procedureType || '').trim().toLowerCase();

    if (normalized) {
      const exact = templates.find(t => (t.procedureType || '').trim().toLowerCase() === normalized);
      if (exact) return exact;
    }

    const general = templates.find(t => !(t.procedureType || '').trim());
    if (general) return general;

    return templates[0] || null;
  });

  readonly patientAttendanceOptions = computed(() => {
    const patients = this.legacyPatients();
    return this.todayVisits()
      .filter(v => !!v.consultId?.trim() && !!v.pNo?.trim())
      .map(v => {
        const patient = patients.find(p => this.normalizePno(p.pno) === this.normalizePno(v.pNo));
        const patientName = (v.fullname || '').trim() 
          || `${patient?.pSurName || ''} ${patient?.pFirstname || ''}`.trim()
          || v.pNo;
        const visitDate = this.formatVisitDate(v.recDate);

        return {
          trackKey: `${v.consultId}-${v.pNo}`,
          consultId: v.consultId,
          pNo: v.pNo,
          label: `${patientName} ${visitDate} [${v.consultId}]`
        };
      })
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  readonly isEditing = computed(() => this.existingConsent() !== null);

  readonly canSave = computed(() => {
    const hasPatient = !!this.selectedConsultId();
    const hasTemplate = !!this.activeTemplate();
    const hasSignature = !!this.form.get('signatureImageBase64')?.value;
    const formValid = this.form.valid;
    return hasPatient && hasTemplate && hasSignature && formValid && !this.loadingIndicator;
  });

  form = this.fb.nonNullable.group({
    signatureName: ['', Validators.required],
    witnessedBy: [''],
    notes: [''],
    signatureImageBase64: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadTemplates();
    this.loadAttendances();
    this.loadPatients();
    this.loadProcedureTypes();

    if (this.data?.consentId) {
      this.loadExistingConsent(this.data.consentId);
    } else if (this.data?.consultId) {
      this.selectedConsultId.set(this.data.consultId);
      this.onPatientChanged(this.data.consultId);
    }
  }

  private loadTemplates(): void {
    this.alertService.startLoadingMessage('Loading consent templates...');
    this.aestheticEndpoint.getConsentTemplatesEndpoint<AestheticConsentTemplate[]>('', true).subscribe({
      next: templates => {
        this.templates.set(templates || []);
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', 'Unable to load consent templates.', MessageSeverity.error, error);
      }
    });
  }

  private loadAttendances(): void {
    this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().subscribe({
      next: visits => this.todayVisits.set(visits || []),
      error: () => this.todayVisits.set([])
    });
  }

  private loadPatients(): void {
    this.hPatientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => this.legacyPatients.set(patients || []),
      error: () => this.legacyPatients.set([])
    });
  }

  private loadProcedureTypes(): void {
    this.moduleSettings.getModuleSettings<{ procedureTypes?: string[] }>('aesthetics', {
      procedureTypes: ['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']
    }).then(settings => {
      const types = (settings.procedureTypes || []).map(x => (x || '').trim()).filter(Boolean);
      this.procedureTypes.set(types.length > 0 ? types : ['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']);
    });
  }

  private loadExistingConsent(consentId: number): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading consent...');
    // Note: Load all consents and filter by ID on client side
    // since the API doesn't support filtering by single consent ID
    this.aestheticEndpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ includeVoided: false }).subscribe({
      next: consents => {
        const consent = (consents || []).find(c => c.id === consentId);
        if (consent) {
          this.existingConsent.set(consent);
          this.selectedConsultId.set(consent.consultId || '');
          this.selectedProcedureType.set(consent.procedureType || 'Procedures');
          this.form.controls.signatureName.setValue(consent.signatureName || '');
          this.form.controls.witnessedBy.setValue(consent.witnessedBy || '');
          this.form.controls.notes.setValue(consent.notes || '');
          this.form.controls.signatureImageBase64.setValue(consent.signatureImageBase64 || '');
        }
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', 'Unable to load consent.', MessageSeverity.error, error);
      }
    });
  }

  onPatientChanged(consultId: string): void {
    this.selectedConsultId.set(consultId);
    this.clearSignature();
    this.form.reset({ signatureName: '', witnessedBy: '', notes: '', signatureImageBase64: '' });
  }

  onProcedureTypeChanged(procedureType: string): void {
    this.selectedProcedureType.set(procedureType);
    this.clearSignature();
    this.form.controls.signatureImageBase64.setValue('');
  }

  private initializeSignaturePad(): void {
    const canvas = this._signatureCanvas?.nativeElement;
    if (!canvas) return;

    requestAnimationFrame(() => {
      const rect = canvas.getBoundingClientRect();
      const width = rect.width || 600;
      const height = rect.height || 180;
      const ratio = window.devicePixelRatio || 1;

      canvas.width = Math.floor(width * ratio);
      canvas.height = Math.floor(height * ratio);
      canvas.style.width = `${width}px`;
      canvas.style.height = `${height}px`;
      canvas.style.backgroundColor = '#ffffff';

      this.signaturePad = new SignaturePad(canvas, {
        backgroundColor: 'rgb(255, 255, 255)',
        penColor: '#000000'
      });

      this.signaturePad.minWidth = 1.8;
      this.signaturePad.maxWidth = 3.0;
      this.signaturePad.addEventListener('endStroke', () => this.persistSignatureImage());

      const ctx = canvas.getContext('2d');
      if (ctx) {
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
      }

      this.signaturePad.clear();

      // If editing, load existing signature
      if (this.isEditing() && this.form.controls.signatureImageBase64.value) {
        this.loadSignatureImageIntoCanvas(this.form.controls.signatureImageBase64.value);
      } else {
        this.persistSignatureImage();
      }
    });
  }

  clearSignature(): void {
    if (this.signaturePad) {
      this.signaturePad.clear();
      const canvas = this._signatureCanvas?.nativeElement;
      const ctx = canvas?.getContext('2d');
      if (canvas && ctx) {
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
      }
    }
    this.form.controls.signatureImageBase64.setValue('');
  }

  private persistSignatureImage(): void {
    if (!this.signaturePad || this.signaturePad.isEmpty()) {
      this.form.controls.signatureImageBase64.setValue('');
      return;
    }

    this.form.controls.signatureImageBase64.setValue(this.signaturePad.toDataURL('image/png'), { emitEvent: false });
  }

  private loadSignatureImageIntoCanvas(base64: string): void {
    const canvas = this._signatureCanvas?.nativeElement;
    if (!canvas || !this.signaturePad) return;

    this.signaturePad.clear();

    const img = new Image();
    img.onload = () => {
      const ctx = canvas.getContext('2d');
      if (!ctx) return;

      ctx.fillStyle = '#ffffff';
      ctx.fillRect(0, 0, canvas.width, canvas.height);

      const canvasRatio = canvas.width / canvas.height;
      const imgRatio = img.width / img.height;
      let drawWidth = canvas.width;
      let drawHeight = canvas.height;

      if (imgRatio > canvasRatio) {
        drawWidth = canvas.width;
        drawHeight = Math.round(canvas.width / imgRatio);
      } else {
        drawHeight = canvas.height;
        drawWidth = Math.round(canvas.height * imgRatio);
      }

      const offsetX = Math.round((canvas.width - drawWidth) / 2);
      const offsetY = Math.round((canvas.height - drawHeight) / 2);

      ctx.drawImage(img, offsetX, offsetY, drawWidth, drawHeight);
      this.form.controls.signatureImageBase64.setValue(base64, { emitEvent: false });
    };
    img.src = base64;
  }

  saveConsent(): void {
    if (!this.canSave()) {
      this.alertService.showStickyMessage('Validation Error', 'Please complete all required fields.', MessageSeverity.warn);
      return;
    }

    const attendance = this.selectedAttendance();
    const template = this.activeTemplate();
    const consultId = attendance?.consultId?.trim() || '';
    const pNo = attendance?.pNo?.trim() || '';

    if (!consultId || !pNo || !template?.id) {
      return;
    }

    const payload: SignAestheticConsent = {
      consultId,
      pNo,
      procedureType: this.selectedProcedureType(),
      consentTemplateId: template.id,
      signatureName: this.form.controls.signatureName.value,
      witnessedBy: this.form.controls.witnessedBy.value || '',
      notes: this.form.controls.notes.value || '',
      signatureImageBase64: this.form.controls.signatureImageBase64.value
    };

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.isEditing() ? 'Updating consent...' : 'Saving consent...');

    const consentId = this.existingConsent()?.id;
    const request = consentId
      ? this.aestheticEndpoint.updateSignedConsentEndpoint<AestheticSignedConsent>(consentId, payload)
      : this.aestheticEndpoint.signConsentEndpoint<AestheticSignedConsent>(payload);

    request.subscribe({
      next: consent => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage(
          'Success',
          this.isEditing() ? 'Consent updated successfully.' : 'Consent saved successfully.',
          MessageSeverity.success
        );
        this.dialogRef.close(consent);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(
          this.isEditing() ? 'Update Error' : 'Save Error',
          this.isEditing() ? 'Unable to update consent.' : 'Unable to save consent.',
          MessageSeverity.error,
          error
        );
      }
    });
  }

  closeDialog(): void {
    this.dialogRef.close(false);
  }

  private calculateAge(dob?: string): number | undefined {
    if (!dob) return undefined;
    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) return undefined;

    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age >= 0 ? age : undefined;
  }

  private normalizePno(value?: string): string {
    return (value || '').trim().toLowerCase();
  }

  private formatVisitDate(value?: string): string {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';

    const day = date.getDate().toString().padStart(2, '0');
    const month = date.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${date.getFullYear()}`;
  }
}
