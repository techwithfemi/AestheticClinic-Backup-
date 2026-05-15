import { Component, ElementRef, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import SignaturePad from 'signature_pad';

import { AestheticConsentStatus, AestheticConsentTemplate, AestheticSignedConsent, SignAestheticConsent } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { ModuleSettingsService } from '../../../services/module-settings.service';

interface FrontdeskModuleSettings {
  autoFollowUpDays?: number;
  consentProcedureTypes?: string[];
}

const DEFAULT_PROCEDURE_TYPES = ['Botox', 'Laser', 'Spa', 'Procedures'];

@Component({
  selector: 'app-consent-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule],
  template: `
    <div class="page-shell">
      <div class="page-header">
        <div>
          <h2>Consent Form</h2>
          <p class="subtitle">Select an attended patient visit, review the procedure consent template, and capture the signature.</p>
        </div>
      </div>

      <div class="layout-grid">
        <mat-card>
          <h3>Header Information</h3>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Search Patient</mat-label>
            <input matInput [value]="attendanceSearchText()" (input)="attendanceSearchText.set(($any($event.target).value || '').trim())" placeholder="Search by patient name" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Select Patient</mat-label>
            <mat-select [value]="selectedConsultId()" (selectionChange)="selectAttendance($event.value)">
              <mat-option value="">Select Patient</mat-option>
              @for (item of attendanceOptions(); track item.consultId) {
                <mat-option [value]="item.consultId">{{ item.label }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          @if (selectedAttendance()) {
            <div class="header-grid">
              <div><span class="label">ConsultId</span><span>{{ selectedAttendance()?.consultId }}</span></div>
              <div><span class="label">PNO</span><span>{{ selectedAttendance()?.pNo }}</span></div>
              <div><span class="label">Patient</span><span>{{ selectedPatientName() }}</span></div>
              <div><span class="label">Date</span><span>{{ selectedAttendance()?.recDate | date:'mediumDate' }}</span></div>
              <div><span class="label">Clinic</span><span>{{ selectedAttendance()?.clinicType }}</span></div>
              <div><span class="label">Purpose</span><span>{{ selectedAttendance()?.attndStatus || 'NORMAL' }}</span></div>
            </div>
          }

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Procedure Type</mat-label>
            <mat-select [value]="selectedProcedureType()" (selectionChange)="changeProcedureType($event.value)">
              @for (procedureType of procedureTypes(); track procedureType) {
                <mat-option [value]="procedureType">{{ procedureType }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Consent Template</mat-label>
            <mat-select [value]="selectedTemplateId()" (selectionChange)="selectTemplate($event.value)">
              @for (template of templates(); track template.id) {
                <mat-option [value]="template.id">{{ template.title || template.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </mat-card>

        <mat-card>
          <h3>Consent Body</h3>

          @if (!selectedAttendance()) {
            <div class="consent-box">Select Patient to load consent content and signature controls.</div>
          } @else {
            <div class="consent-box">{{ activeTemplate()?.content || 'No active template found for the selected procedure.' }}</div>

            <form [formGroup]="form" class="form-stack">
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

              <div class="actions-row">
                <button mat-raised-button color="primary" type="button" (click)="signConsent()" [disabled]="form.invalid || !status()?.canSign || !activeTemplate() || loadingIndicator">
                  Sign Consent
                </button>
              </div>
            </form>
          }
        </mat-card>
      </div>

      @if (latestSignedConsent()) {
        <mat-card class="summary-card">
          <h3>Latest Signed Consent</h3>
          <p><strong>Signed:</strong> {{ latestSignedConsent()?.signedDate | date:'medium' }}</p>
          <p><strong>Signed By:</strong> {{ latestSignedConsent()?.signedBy || latestSignedConsent()?.signatureName }}</p>
          <p><strong>Witness:</strong> {{ latestSignedConsent()?.witnessedBy || '—' }}</p>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .selector-card, .summary-card { margin-bottom: 16px; }
    .layout-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px; }
    .full-width { width: 100%; }
    .header-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; margin-bottom: 16px; }
    .label { display: block; color: #666; font-size: .8rem; margin-bottom: 4px; }
    .consent-box { white-space: pre-wrap; border: 1px solid #ddd; border-radius: 8px; padding: 12px; min-height: 180px; background: #fafafa; margin-bottom: 16px; }
    .form-stack { display: flex; flex-direction: column; gap: 12px; }
    .actions-row { display: flex; justify-content: flex-end; }
    .signature-pad-wrap { display: grid; gap: 8px; }
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
    .signature-hint { color: #667085; font-size: .85rem; }
    @media (max-width: 992px) { .layout-grid, .header-grid { grid-template-columns: 1fr; } }
  `]
})
export class ConsentFormComponent implements OnInit {
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly aestheticEndpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private readonly moduleSettingsService = inject(ModuleSettingsService);

  loadingIndicator = false;
  readonly attendances = signal<Attendance[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly templates = signal<AestheticConsentTemplate[]>([]);
  readonly status = signal<AestheticConsentStatus | null>(null);
  readonly latestSignedConsent = signal<AestheticSignedConsent | null>(null);
  readonly selectedConsultId = signal<string>('');
  readonly selectedProcedureType = signal<string>(DEFAULT_PROCEDURE_TYPES[0]);
  readonly selectedTemplateId = signal<number | null>(null);
  readonly procedureTypes = signal<string[]>([...DEFAULT_PROCEDURE_TYPES]);
  readonly attendanceSearchText = signal<string>('');

  @ViewChild('signatureCanvas')
  set signatureCanvas(value: ElementRef<HTMLCanvasElement> | undefined) {
    this._signatureCanvas = value;
    if (value) {
      this.initializeSignaturePad();
    }
  }

  private _signatureCanvas?: ElementRef<HTMLCanvasElement>;
  private signaturePad: SignaturePad | null = null;

  readonly selectedAttendance = computed(() => this.attendances().find(x => x.consultId === this.selectedConsultId()) ?? null);
  readonly selectedPatientName = computed(() => {
    const pNo = this.selectedAttendance()?.pNo;
    const patient = this.patients().find(x => x.pno === pNo);
    return patient ? `${patient.pSurName} ${patient.pFirstname || ''}`.trim() : (pNo || 'Unknown patient');
  });
  readonly activeTemplate = computed(() => this.templates().find(x => x.id === this.selectedTemplateId()) ?? this.status()?.activeTemplate ?? null);
  readonly attendanceOptions = computed(() => {
    const term = this.attendanceSearchText().toLowerCase();

    return this.attendances()
      .filter(item => !!item.consultId?.trim() && !!item.pNo?.trim())
      .filter(item => this.isToday(item.recDate))
      .map(item => {
        const patientName = this.resolvePatientNameByPNo(item.pNo).trim() || 'Unknown patient';
        return {
          consultId: item.consultId || '',
          label: `${patientName} ${this.formatAttendanceDate(item.recDate)} [${item.consultId || 'N/A'}]`
        };
      })
      .filter(item => !term || item.label.toLowerCase().includes(term) || item.consultId.toLowerCase().includes(term))
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  readonly form = this.fb.nonNullable.group({
    signatureName: ['', Validators.required],
    witnessedBy: [''],
    notes: [''],
    signatureImageBase64: ['']
  });

  ngOnInit(): void {
    this.loadModuleSettings();
    this.loadPatients();
    this.loadAttendance();

    this.route.queryParamMap.subscribe(params => {
      const consultId = params.get('consultId');
      if (consultId && this.attendances().some(x => x.consultId === consultId)) {
        this.selectAttendance(consultId);
      }
    });
  }

  private loadModuleSettings(): void {
    const defaults: FrontdeskModuleSettings = {
      autoFollowUpDays: 14,
      consentProcedureTypes: [...DEFAULT_PROCEDURE_TYPES]
    };

    this.moduleSettingsService.getModuleSettings<FrontdeskModuleSettings>('frontdesk', defaults)
      .then(settings => {
        const configuredTypes = (settings.consentProcedureTypes || []).map(x => (x || '').trim()).filter(Boolean);
        const effectiveTypes = configuredTypes.length ? configuredTypes : [...DEFAULT_PROCEDURE_TYPES];
        this.procedureTypes.set(effectiveTypes);

        if (!effectiveTypes.includes(this.selectedProcedureType())) {
          this.selectedProcedureType.set(effectiveTypes[0]);
        }
      })
      .catch(() => {
        this.procedureTypes.set([...DEFAULT_PROCEDURE_TYPES]);
      });
  }

  loadPatients(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({ next: patients => this.patients.set(patients || []), error: () => this.patients.set([]) });
  }

  loadAttendance(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading attendance records...');
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().subscribe({
      next: attendances => {
        const records = (attendances || []).filter(x => !!x.consultId?.trim() && !!x.pNo?.trim());
        this.attendances.set(records);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();

        const requested = this.route.snapshot.queryParamMap.get('consultId');
        if (requested && this.attendanceOptions().some(x => x.consultId === requested)) {
          this.selectAttendance(requested);
        } else {
          this.selectAttendance('');
        }
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', 'Unable to retrieve attendance records.', MessageSeverity.error, error);
      }
    });
  }

  selectAttendance(consultId: string): void {
    this.selectedConsultId.set(consultId);
    this.selectedTemplateId.set(null);
    this.latestSignedConsent.set(null);
    this.clearSignature();

    if (!consultId) {
      this.status.set(null);
      return;
    }

    this.loadTemplatesAndStatus();
  }

  changeProcedureType(procedureType: string): void {
    this.selectedProcedureType.set(procedureType);
    this.selectedTemplateId.set(null);
    this.loadTemplatesAndStatus();
  }

  selectTemplate(templateId: number): void {
    this.selectedTemplateId.set(templateId);
  }

  private loadTemplatesAndStatus(): void {
    const attendance = this.selectedAttendance();
    const consultId = attendance?.consultId?.trim() || '';
    const pNo = attendance?.pNo?.trim() || '';
    if (!consultId || !pNo) {
      this.status.set(null);
      this.latestSignedConsent.set(null);
      return;
    }

    const procedureType = this.selectedProcedureType();

    // Attendance is the entry point for this page, so patient can sign when a valid attendance row is selected.
    this.status.set({
      consultId,
      pNo,
      procedureType,
      attendanceTaken: true,
      canSign: true,
      hasValidConsent: false,
      activeTemplate: undefined,
      latestSignedConsent: undefined
    });

    this.aestheticEndpoint.getConsentTemplatesEndpoint<AestheticConsentTemplate[]>('', true).subscribe({
      next: templates => {
        const list = templates || [];
        this.templates.set(list);
        this.selectedTemplateId.set(list[0]?.id ?? null);

        this.status.update(current => current
          ? { ...current, activeTemplate: list[0], hasValidConsent: !!this.latestSignedConsent() }
          : current);
      },
      error: () => this.templates.set([])
    });

    this.aestheticEndpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ consultId, pNo, procedureType, includeVoided: false }).subscribe({
      next: consents => {
        const latest = (consents || [])[0] || null;
        this.latestSignedConsent.set(latest);
        this.status.update(current => current
          ? { ...current, hasValidConsent: !!latest, latestSignedConsent: latest ?? undefined }
          : current);
      },
      error: error => {
        this.latestSignedConsent.set(null);
        this.alertService.showStickyMessage('Status Error', 'Unable to retrieve consent status.', MessageSeverity.error, error);
      }
    });
  }

  signConsent(): void {
    const attendance = this.selectedAttendance();
    const consultId = attendance?.consultId?.trim() || '';
    const pNo = attendance?.pNo?.trim() || '';
    const template = this.activeTemplate();
    if (!consultId || !pNo || !template?.id || this.form.invalid) {
      return;
    }

    const payload: SignAestheticConsent = {
      consultId,
      pNo,
      procedureType: this.selectedProcedureType(),
      consentTemplateId: template.id,
      signatureName: this.form.controls.signatureName.value,
      witnessedBy: this.form.controls.witnessedBy.value,
      notes: this.form.controls.notes.value,
      signatureImageBase64: this.form.controls.signatureImageBase64.value
    };

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Signing consent...');
    this.aestheticEndpoint.signConsentEndpoint<AestheticSignedConsent>(payload).subscribe({
      next: consent => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.latestSignedConsent.set(consent);
        this.alertService.showMessage('Success', 'Consent signed successfully.', MessageSeverity.success);
        this.loadTemplatesAndStatus();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Sign Error', 'Unable to sign consent.', MessageSeverity.error, error);
      }
    });
  }

  private initializeSignaturePad(): void {
    const canvas = this._signatureCanvas?.nativeElement;
    if (!canvas) {
      return;
    }

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
      this.persistSignatureImage();
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

  private getCanvasPoint(event: MouseEvent | TouchEvent): { x: number; y: number } {
    const canvas = this._signatureCanvas?.nativeElement;
    const rect = canvas?.getBoundingClientRect();
    if (!rect) {
      return { x: 0, y: 0 };
    }

    if ('touches' in event && event.touches.length > 0) {
      const touch = event.touches[0];
      return { x: touch.clientX - rect.left, y: touch.clientY - rect.top };
    }

    const mouse = event as MouseEvent;
    return { x: mouse.clientX - rect.left, y: mouse.clientY - rect.top };
  }

  private clearSignatureCanvas(): void {
    const canvas = this._signatureCanvas?.nativeElement;
    if (!canvas || !this.signaturePad) {
      return;
    }

    this.signaturePad.clear();
    const ctx = canvas.getContext('2d');
    if (ctx) {
      ctx.fillStyle = '#ffffff';
      ctx.fillRect(0, 0, canvas.width, canvas.height);
    }
  }

  private resolvePatientNameByPNo(pNo?: string): string {
    const normalized = (pNo ?? '').trim().toLowerCase();
    if (!normalized) {
      return 'Unknown patient';
    }

    const patient = this.patients().find(x => (x.pno ?? '').trim().toLowerCase() === normalized);
    if (!patient) {
      return pNo ?? 'Unknown patient';
    }

    return `${patient.pSurName || ''} ${patient.pFirstname || ''}`.trim() || (pNo ?? 'Unknown patient');
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const d = new Date(value);
    if (isNaN(d.getTime())) {
      return false;
    }

    const today = new Date();
    return d.getFullYear() === today.getFullYear()
      && d.getMonth() === today.getMonth()
      && d.getDate() === today.getDate();
  }

  private formatAttendanceDate(value?: string): string {
    if (!value) {
      return '';
    }

    const d = new Date(value);
    if (isNaN(d.getTime())) {
      return '';
    }

    const day = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en', { month: 'short' });
    const year = d.getFullYear();
    return `${day}-${month}-${year}`;
  }
}
