import { Component, OnInit, computed, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { ViewConsentComponent } from '../view-consent/view-consent.component';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AestheticConsultation, AestheticPatient, AestheticSignedConsent } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';

interface TabPhotoCollection {
  neuromodulator: string[];
  dermalFiller: string[];
  laser: string[];
}

interface SafetyAlert {
  message: string;
  severity: 'info' | 'warning' | 'critical';
}

@Component({
  selector: 'app-procedures',
  standalone: true,
  templateUrl: './procedures.component.html',
  styleUrls: ['./procedures.component.scss'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatCheckboxModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    MatTooltipModule,
    MatDialogModule,
    MatProgressBarModule,
    AttendanceSummaryComponent,
    ViewConsentComponent
  ]
})
export class ProceduresComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly hPatientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly route = inject(ActivatedRoute);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly currentConsultationId = signal<number | null>(null);
  readonly selectedTabIndex = signal(0);
  readonly procedureTypeOptions = ['Procedures', 'Botox', 'Dermal Filler', 'Laser'];

  readonly tabPhotos = signal<TabPhotoCollection>({
    neuromodulator: [],
    dermalFiller: [],
    laser: []
  });

  readonly generatedProcedureNote = signal('');
  readonly safetyAlerts = signal<SafetyAlert[]>([]);
  readonly showEmergencyProtocols = signal(false);
  readonly reportedComplications = signal<{ tab: string; timestamp: Date }[]>([]);
  readonly attendanceRecords = signal<Attendance[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly isPregnant = signal(false);
  readonly hasHsvHistory = signal(false);
  readonly isFillerSelected = signal(false);
  readonly recentTreatments = signal<{ procedure: string; date: Date }[]>([]);

  readonly selectedPatient = computed(() => this.patients().find(x => x.id === this.form.controls.patientId.value) ?? null);
  readonly selectedPatientPNo = computed(() => this.selectedPatient()?.pno?.trim() || '');
  readonly selectedProcedureType = computed(() => (this.form.controls.procedureType.value || '').trim());
  readonly selectedAttendanceRecord = computed(() => {
    const pNo = this.selectedPatientPNo().toLowerCase();
    if (!pNo) {
      return null;
    }

    return [...this.attendanceRecords()]
      .filter(item => (item.pNo ?? '').trim().toLowerCase() === pNo)
      .sort((a, b) => (b.recDate || '').localeCompare(a.recDate || ''))[0] ?? null;
  });
  readonly selectedPatientSummary = computed(() => this.buildAttendanceSummary(this.selectedPatient(), this.selectedAttendanceRecord()));
  readonly generatedPostCare = computed(() => {
    const type = this.selectedProcedureType().toLowerCase();
    if (type.includes('botox')) {
      return 'Avoid rubbing treated areas for 24 hours and avoid vigorous exercise for the rest of the day.';
    }

    if (type.includes('laser')) {
      return 'Use sunscreen daily, avoid direct heat exposure, and follow advised skin care regimen.';
    }

    if (type.includes('filler')) {
      return 'Avoid pressure on treated areas, maintain hydration, and report persistent swelling.';
    }

    return 'Follow clinic post-procedure guidance and report any unusual symptoms immediately.';
  });

  readonly patientAttendanceOptions = computed<{
    trackKey: string;
    patientId: number;
    label: string;
    disabled: boolean;
  }[]>(() => {
    const todayKey = this.toLocalDateKey(new Date());
    const patients = this.patients();

    return this.attendanceRecords()
      .filter(attendance => this.toLocalDateKey(attendance.recDate) === todayKey)
      .map(attendance => {
        const patient = this.findPatientByAttendancePno(patients, attendance.pNo);
        const patientName = this.resolveAttendancePatientName(attendance, patient);
        const visitDate = this.formatAttendanceDate(attendance.recDate);
        const consultId = attendance.consultId ?? '';

        return {
          trackKey: `${consultId}-${attendance.recId ?? attendance.pNo ?? patient?.id ?? 0}`,
          patientId: patient?.id ?? 0,
          label: `${patientName} ${visitDate} [${consultId}]`,
          disabled: !patient
        };
      })
      .filter(item => item.patientId > 0)
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  form = this.fb.nonNullable.group({
    patientId: [0, Validators.min(1)],
    procedureType: ['Procedures', Validators.required],
    consultation: this.fb.nonNullable.group({
      id: [0],
      patientId: [0, Validators.min(1)],
      chiefComplaint: [''],
      pregnancy: [false],
      hsvHistory: [false],
      allergies: ['']
    }),
    neuromodulator: this.fb.nonNullable.group({
      lotNumber: [''],
      dilution: [0],
      postCareInstructions: ['']
    }),
    dermalFiller: this.fb.nonNullable.group({
      productName: ['']
    }),
    laser: this.fb.nonNullable.group({
      deviceName: [''],
      wavelength: [''],
      fluence: [''],
      pulseDuration: [''],
      spotSize: ['']
    }),
    consent: this.fb.nonNullable.group({
      id: [0],
      patientId: [0],
      consultId: [''],
      isReceived: [false],
      isSigned: [false],
      signatureName: ['']
    })
  });

  get consultationGroup() {
    return this.form.controls.consultation;
  }

  get neuromodulatorGroup() {
    return this.form.controls.neuromodulator;
  }

  get dermalFillerGroup() {
    return this.form.controls.dermalFiller;
  }

  get laserGroup() {
    return this.form.controls.laser;
  }

  constructor() {
    effect(() => {
      const pregnancy = this.consultationGroup.get('pregnancy')?.value ?? false;
      const hsvHistory = this.consultationGroup.get('hsvHistory')?.value ?? false;

      this.isPregnant.set(!!pregnancy);
      this.hasHsvHistory.set(!!hsvHistory);

      if (pregnancy) {
        this.selectedTabIndex.set(1);
      }
    });

    effect(() => {
      const fillerProduct = this.dermalFillerGroup.get('productName')?.value ?? '';
      this.isFillerSelected.set(!!fillerProduct && fillerProduct.trim().length > 0);
    });

    effect(() => {
      this.validateAllergiesAndDuplicates();
    });
  }

  ngOnInit(): void {
    this.loadPatients();
    this.loadLegacyPatients();
    this.loadAttendances();

    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();
    this.selectedTabIndex.set(this.mapTabToIndex(initialTab));

    this.neuromodulatorGroup.valueChanges.subscribe(() => {
      this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
    });
    this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
  }

  onProcedureTypeChanged(): void {
    this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });

    if (this.selectedPatientPNo()) {
      this.selectedTabIndex.set(0);
    }
  }

  onPatientChanged(): void {
    const patientId = this.form.controls.patientId.value;
    const patient = this.patients().find(x => x.id === patientId);
    const existing = (patient?.consultations || [])
      .filter(c => (c.procedureType || '').toLowerCase() === 'procedures')
      .sort((a, b) => (b.consultationDate || '').localeCompare(a.consultationDate || ''))[0];

    const allConsultations = patient?.consultations || [];
    const recent = allConsultations
      .filter(c => c.consultationDate)
      .map(c => ({
        procedure: c.procedureType || 'Unknown',
        date: new Date(c.consultationDate!)
      }))
      .slice(0, 5);
    this.recentTreatments.set(recent);

    this.loadFromConsultation(existing);
    this.selectedTabIndex.set(0);
  }

  save(): void {
    const payload = this.buildPayload();
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Saving procedure...');

    this.endpoint.createConsultationEndpoint<AestheticConsultation>(payload).subscribe({
      next: saved => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.currentConsultationId.set(saved?.id ?? this.currentConsultationId());
        this.alertService.showMessage('Saved', 'Procedure saved successfully.', MessageSeverity.success);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save Error', 'Unable to save procedure.', MessageSeverity.error, error);
      }
    });
  }

  private loadFromConsultation(consultation?: AestheticConsultation): void {
    this.currentConsultationId.set(consultation?.id ?? null);

    if (!consultation) {
      this.resetForm();
      this.generatedProcedureNote.set('');
      this.safetyAlerts.set([]);
      this.reportedComplications.set([]);
      this.showEmergencyProtocols.set(false);
      return;
    }

    const patientId = consultation.patientId ?? 0;
    const procedureType = (consultation.procedureType || 'Procedures').trim() || 'Procedures';

    this.form.controls.patientId.setValue(patientId);
    this.form.controls.procedureType.setValue(procedureType);

    const selectedPatient = this.patients().find(p => p.id === patientId);
    const selectedPNo = selectedPatient?.pno?.trim();

    if (selectedPNo) {
      this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ pNo: selectedPNo, includeVoided: true }).subscribe({
        next: consents => {
          const latest = (consents || [])[0];
          if (latest) {
            this.form.controls.consent.patchValue({
              id: latest.id,
              patientId: latest.patientId ?? patientId,
              consultId: latest.consultId ?? '',
              isSigned: !!latest.signedDate,
              signatureName: latest.signatureName ?? '',
              isReceived: true
            });
          }
        }
      });
    }

    this.generatedProcedureNote.set(this.buildProcedureNote(consultation));
    this.safetyAlerts.set(this.extractSafetyAlerts(consultation));
    this.reportedComplications.set(this.extractReportedComplications(consultation));
    this.showEmergencyProtocols.set(this.shouldShowEmergencyProtocols(consultation));
  }

  private loadPatients(): void {
    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => this.patients.set(patients || []),
      error: () => this.patients.set([])
    });
  }

  private loadLegacyPatients(): void {
    this.hPatientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => this.legacyPatients.set(patients || []),
      error: () => this.legacyPatients.set([])
    });
  }

  private loadAttendances(): void {
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().subscribe({
      next: attendances => this.attendanceRecords.set(attendances || []),
      error: () => this.attendanceRecords.set([])
    });
  }

  private mapTabToIndex(tab: string): number {
    switch ((tab || '').trim().toLowerCase()) {
      case 'botox':
      case 'neuromodulator':
        return 2;
      case 'dermal filler':
      case 'filler':
        return 3;
      case 'laser':
        return 4;
      case 'consultation':
        return 1;
      default:
        return 0;
    }
  }

  private toLocalDateKey(value?: string | Date | null): string {
    if (!value) {
      return '';
    }

    const date = value instanceof Date ? value : new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private findPatientByAttendancePno(patients: AestheticPatient[], pNo?: string): AestheticPatient | undefined {
    const normalized = (pNo ?? '').trim().toLowerCase();
    if (!normalized) {
      return undefined;
    }

    return patients.find(p => (p.pno ?? '').trim().toLowerCase() === normalized);
  }

  private resolveAttendancePatientName(attendance: Attendance, patient?: AestheticPatient): string {
    if (patient) {
      return [patient.firstName, patient.lastName].filter(Boolean).join(' ').trim() || patient.pno || 'Unknown patient';
    }

    const legacy = this.legacyPatients().find(p => (p.pno ?? '').trim().toLowerCase() === (attendance.pNo ?? '').trim().toLowerCase());
    if (legacy) {
      return [legacy.pSurName, legacy.pFirstname].filter(Boolean).join(' ').trim() || attendance.pNo;
    }

    return attendance.pNo || 'Unknown patient';
  }

  private formatAttendanceDate(value?: string): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return date.toLocaleDateString();
  }

  private validateAllergiesAndDuplicates(): void {
    const allergies = (this.consultationGroup.controls.allergies.value || '').toLowerCase();
    const hasRiskyAllergy = allergies.includes('lidocaine') || allergies.includes('anesthetic');

    if (hasRiskyAllergy) {
      this.safetyAlerts.set([
        ...this.safetyAlerts().filter(x => x.message !== 'Allergy note includes potential anesthetic allergy.'),
        { message: 'Allergy note includes potential anesthetic allergy.', severity: 'warning' }
      ]);
    }
  }

  private buildProcedureNote(consultation: AestheticConsultation): string {
    const type = consultation.procedureType || 'Procedure';
    const date = consultation.consultationDate ? new Date(consultation.consultationDate).toLocaleString() : new Date().toLocaleString();
    return `${type} consultation reviewed on ${date}.`;
  }

  private extractSafetyAlerts(consultation: AestheticConsultation): SafetyAlert[] {
    const alerts: SafetyAlert[] = [];
    if ((consultation.allergies || '').trim()) {
      alerts.push({ message: `Allergies: ${consultation.allergies}`, severity: 'warning' });
    }

    if ((consultation.currentMedications || '').trim()) {
      alerts.push({ message: 'Patient has current medications documented.', severity: 'info' });
    }

    return alerts;
  }

  private extractReportedComplications(consultation: AestheticConsultation): { tab: string; timestamp: Date }[] {
    if (!(consultation.risksAndComplications || '').trim()) {
      return [];
    }

    return [{ tab: this.form.controls.procedureType.value, timestamp: new Date() }];
  }

  private shouldShowEmergencyProtocols(consultation: AestheticConsultation): boolean {
    const riskText = (consultation.risksAndComplications || '').toLowerCase();
    return riskText.includes('anaphyl') || riskText.includes('vascular occlusion');
  }

  private resetForm(): void {
    this.form.controls.patientId.setValue(0);
    this.form.controls.procedureType.setValue('Procedures');
    this.form.controls.consultation.reset({
      id: 0,
      patientId: 0,
      chiefComplaint: '',
      pregnancy: false,
      hsvHistory: false,
      allergies: ''
    });
    this.form.controls.neuromodulator.reset({
      lotNumber: '',
      dilution: 0,
      postCareInstructions: this.generatedPostCare()
    });
    this.form.controls.dermalFiller.reset({ productName: '' });
    this.form.controls.laser.reset({
      deviceName: '',
      wavelength: '',
      fluence: '',
      pulseDuration: '',
      spotSize: ''
    });
    this.form.controls.consent.reset({
      id: 0,
      patientId: 0,
      consultId: '',
      isReceived: false,
      isSigned: false,
      signatureName: ''
    });
  }

  private buildPayload(): object {
    const consultation = this.consultationGroup.getRawValue();
    const neuromodulator = this.neuromodulatorGroup.getRawValue();
    const dermalFiller = this.dermalFillerGroup.getRawValue();
    const laser = this.laserGroup.getRawValue();

    return {
      id: this.currentConsultationId() ?? 0,
      patientId: this.form.controls.patientId.value,
      consultationDate: new Date().toISOString(),
      procedureType: this.form.controls.procedureType.value || 'Procedures',
      procedureDescription: consultation.chiefComplaint,
      treatmentPlan: JSON.stringify(consultation),
      injectionMapping: JSON.stringify(neuromodulator),
      risksAndComplications: JSON.stringify(dermalFiller),
      deviceSettings: JSON.stringify(laser),
      lotNumber: neuromodulator.lotNumber,
      dilution: neuromodulator.dilution?.toString() ?? '',
      postTreatmentInstructions: this.generatedPostCare(),
      deviceUsed: laser.deviceName,
      wavelength: laser.wavelength,
      fluence: laser.fluence,
      pulseDuration: laser.pulseDuration,
      spotSize: laser.spotSize
    };
  }

  private buildAttendanceSummary(patient: AestheticPatient | null, attendance: Attendance | null): VwhRecord | null {
    if (!patient && !attendance) {
      return null;
    }

    const fullName = [patient?.firstName, patient?.lastName].filter(Boolean).join(' ').trim();
    const legacyPatient = this.legacyPatients().find(p => (p.pno ?? '').trim().toLowerCase() === (attendance?.pNo ?? patient?.pno ?? '').trim().toLowerCase());

    return {
      consultId: attendance?.consultId ?? String(this.currentConsultationId() ?? ''),
      pNo: patient?.pno ?? attendance?.pNo ?? '',
      clinicType: attendance?.clinicType ?? 'Aesthetic Procedures',
      fullname: fullName || [legacyPatient?.pSurName, legacyPatient?.pFirstname].filter(Boolean).join(' ').trim() || attendance?.pNo || '—',
      clientCat: attendance?.clientCat,
      coyname: attendance?.coyname ?? legacyPatient?.coyName,
      dob: patient?.dateOfBirth,
      sex: patient?.gender,
      age: this.calculateAge(patient?.dateOfBirth),
      patientPhotoBase64: undefined
    };
  }

  private calculateAge(dob?: string): number | undefined {
    if (!dob) {
      return undefined;
    }

    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) {
      return undefined;
    }

    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age >= 0 ? age : undefined;
  }

  private selectedPatientId(): number {
    return this.form.controls.patientId.value;
  }
}


