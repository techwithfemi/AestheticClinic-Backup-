import { Component, OnInit, computed, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';
import { ViewConsentComponent } from '../view-consent/view-consent.component';
import { AestheticConsultation, AestheticPatient, AestheticPhoto, AestheticSignedConsent } from '../../../models/aesthetic.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';
import { ModuleSettingsService } from '../../../services/module-settings.service';
import { AuthService } from '../../../services/auth.service';

type PhotoTab = 'neuromodulator' | 'dermalFiller' | 'laser';
type PhotoPhase = 'Before' | 'After';
type StandardTag = 'frontal' | 'left' | 'right' | 'profile';

interface TabPhotoItem {
  id?: number;
  consultationId?: number;
  fileName: string;
  phase: PhotoPhase;
  tag: StandardTag;
  url?: string;
  file?: File;
}

type TabPhotoCollection = Record<PhotoTab, TabPhotoItem[]>;

interface SafetyAlert {
  type: 'hard-stop' | 'allergy' | 'duplicate' | 'warning';
  title: string;
  message: string;
  action?: () => void;
  actionLabel?: string;
}

interface ProceduresEntryDialogData {
  initialTab?: string;
  consultation?: AestheticConsultation;
}

@Component({
  selector: 'app-procedures-entry-dialog',
  standalone: true,
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
  ],
  template: `
    <div class="procedures-page">
      <div class="dialog-header">
        <h2>{{ currentConsultationId() ? 'Edit Procedure Entry' : 'Add Procedure Entry' }}</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="closeDialog()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div class="page-header">
        <div>
          <h2>Aesthetic Procedures</h2>
          <p class="subtitle">Unified consultation and procedure tabs with integrated safety checks.</p>
          @if (selectedAttendanceSummary()) {
            <div class="header-attendance-summary">
              <app-attendance-summary [attendance]="selectedAttendanceSummary()!" [photo]="selectedAttendanceSummary()?.patientPhotoBase64" [compact]="true"></app-attendance-summary>
            </div>
          }
        </div>
      </div>

      <!-- Hard-Stop Safety Alerts -->
      @for (alert of safetyAlerts(); track alert.title) {
        @switch (alert.type) {
          @case ('hard-stop') {
            <div class="alert alert-danger hard-stop-alert">
              <mat-icon>do_not_disturb</mat-icon>
              <div class="alert-content">
                <strong>ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¸ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â HARD STOP: {{ alert.title }}</strong>
                <p>{{ alert.message }}</p>
                @if (alert.actionLabel && alert.action) {
                  <button mat-stroked-button (click)="alert.action()" class="alert-action">{{ alert.actionLabel }}</button>
                }
              </div>
            </div>
          }
          @case ('allergy') {
            <div class="alert alert-danger allergy-alert">
              <mat-icon>no_meals</mat-icon>
              <div class="alert-content">
                <strong>ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â°ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¸ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â« ALLERGY DETECTED: {{ alert.title }}</strong>
                <p>{{ alert.message }}</p>
              </div>
            </div>
          }
          @case ('duplicate') {
            <div class="alert alert-warning duplicate-alert">
              <mat-icon>warning</mat-icon>
              <div class="alert-content">
                <strong>ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¸ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â DUPLICATE TREATMENT: {{ alert.title }}</strong>
                <p>{{ alert.message }}</p>
                @if (alert.actionLabel && alert.action) {
                  <button mat-stroked-button (click)="alert.action()" class="alert-action">{{ alert.actionLabel }}</button>
                }
              </div>
            </div>
          }
          @case ('warning') {
            <div class="alert alert-info">
              <mat-icon>info</mat-icon>
              <div class="alert-content">
                <strong>ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¾ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¹ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¯ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¸ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â {{ alert.title }}</strong>
                <p>{{ alert.message }}</p>
              </div>
            </div>
          }
        }
      }

      <!-- Emergency Quick-Access Bar -->
      @if (hasActiveComplications()) {
        <div class="emergency-bar">
          <mat-progress-bar mode="indeterminate" color="warn"></mat-progress-bar>
          <div class="emergency-content">
            <mat-icon class="pulse-icon">emergency</mat-icon>
            <span><strong>Active Complication(s) Reported</strong> - Emergency protocols available below</span>
            <button mat-raised-button color="warn" (click)="scrollToEmergencyProtocols()">
              View Emergency Protocols
            </button>
          </div>
        </div>
      }

      <mat-card>
        <form [formGroup]="form" class="form-shell">
          <mat-tab-group [selectedIndex]="selectedTabIndex()">
            <mat-tab label="Consent">
              <div class="tab-body consent-tab">
                <div class="consent-grid full">
                  <mat-form-field appearance="outline" class="patient-field">
                    <mat-label>Patient *</mat-label>
                    <mat-select [value]="selectedVisitConsultId()" (selectionChange)="onPatientVisitChanged($event.value)">
                      <mat-option value="">Select Patient</mat-option>
                      @for (item of patientAttendanceOptions(); track item.trackKey) {
                        <mat-option [value]="item.consultId">{{ item.label }}</mat-option>
                      }
                    </mat-select>
                    @if (saveAttempted() && !selectedVisitPNo().trim()) {
                      <mat-error>Patient is required.</mat-error>
                    }
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="patient-field">
                    <mat-label>Procedure Type *</mat-label>
                    <mat-select [value]="selectedConsentProcedureType()" (selectionChange)="onConsentProcedureTypeChanged($event.value)">
                      <mat-option value="">Select procedure type</mat-option>
                      @for (procedureType of procedureTypes(); track procedureType) {
                        <mat-option [value]="procedureType">{{ procedureType }}</mat-option>
                      }
                    </mat-select>
                    @if (saveAttempted() && !selectedConsentProcedureType().trim()) {
                      <mat-error>Procedure type is required.</mat-error>
                    }
                  </mat-form-field>
                </div>

                <div class="full consent-preview">
                  <div class="block-title">Signed Consent for Selected Procedure</div>
                  @if (!hasConsentSelection()) {
                    <div class="empty-consent">Select patient and procedure type to view signed consent.</div>
                  } @else if (!selectedProcedureSignedConsent()) {
                    <div class="empty-consent">No signed consent found for {{ selectedConsentProcedureType() }}.</div>
                  } @else {
                    <app-view-consent [embeddedConsent]="selectedProcedureSignedConsent()"></app-view-consent>
                  }
                </div>

                <div class="full consent-table-card">
                  <div class="consent-table-toolbar">
                    <mat-form-field appearance="outline" class="consent-search-field">
                      <mat-label>Search by Patient / ConsultId / Procedure</mat-label>
                      <input matInput [value]="consentSearchText()" (input)="onConsentSearchChanged(($any($event.target).value || '').trim())" />
                    </mat-form-field>
                    <button mat-stroked-button type="button" (click)="refreshConsentTable()">Refresh</button>
                  </div>

                  <table mat-table [dataSource]="pagedConsentRows()" class="data-table">
                    <ng-container matColumnDef="consultId">
                      <th mat-header-cell *matHeaderCellDef>ConsultId</th>
                      <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
                    </ng-container>
                    <ng-container matColumnDef="patient">
                      <th mat-header-cell *matHeaderCellDef>Patient</th>
                      <td mat-cell *matCellDef="let row">{{ resolveConsentPatientName(row.pNo) }}</td>
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

                    <tr mat-header-row *matHeaderRowDef="consentTableColumns"></tr>
                    <tr mat-row *matRowDef="let row; columns: consentTableColumns"></tr>
                  </table>

                  @if (filteredConsentRows().length === 0) {
                    <div class="empty-consent">No consent records found.</div>
                  } @else {
                    <div class="pager-row">
                      <button mat-stroked-button type="button" (click)="changeConsentPage(-1)" [disabled]="consentPageIndex() <= 0">Prev</button>
                      <span>Page {{ consentPageIndex() + 1 }} / {{ consentTotalPages() }}</span>
                      <button mat-stroked-button type="button" (click)="changeConsentPage(1)" [disabled]="consentPageIndex() + 1 >= consentTotalPages()">Next</button>
                    </div>
                  }
                </div>
              </div>
            </mat-tab>

            <mat-tab label="Consultation">
              <div class="tab-body" [formGroup]="consultationGroup">
                <mat-form-field appearance="outline" class="full">
                  <mat-label>Chief Complaint</mat-label>
                  <textarea matInput rows="2" formControlName="chiefComplaint"></textarea>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Duration</mat-label>
                  <mat-select formControlName="duration">
                    <mat-option value="<1 week">&lt;1 week</mat-option>
                    <mat-option value="1-4 weeks">1-4 weeks</mat-option>
                    <mat-option value="1-3 months">1-3 months</mat-option>
                    <mat-option value=">3 months">&gt;3 months</mat-option>
                  </mat-select>
                </mat-form-field>

                <div class="half toggle-row">
                  <span>Expectation</span>
                  <mat-slide-toggle formControlName="expectationRealistic">Realistic</mat-slide-toggle>
                </div>

                <div class="full checkbox-grid">
                  <div class="block-title">Medical conditions</div>
                  <div class="checks" [formGroup]="medicalConditionsGroup">
                    <mat-checkbox formControlName="diabetes">Diabetes</mat-checkbox>
                    <mat-checkbox formControlName="hypertension">Hypertension</mat-checkbox>
                    <mat-checkbox formControlName="keloid">Keloid tendency</mat-checkbox>
                    <mat-checkbox formControlName="autoimmune">Autoimmune disease</mat-checkbox>
                    <mat-checkbox formControlName="bleedingDisorder">Bleeding disorder</mat-checkbox>
                  </div>
                </div>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Medications</mat-label>
                  <mat-select formControlName="medications" multiple>
                    <mat-option value="anticoagulants">Anticoagulants</mat-option>
                    <mat-option value="retinoids">Retinoids</mat-option>
                    <mat-option value="steroids">Steroids</mat-option>
                    <mat-option value="immunosuppressants">Immunosuppressants</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Allergies</mat-label>
                  <mat-select formControlName="allergySelections" multiple (selectionChange)="onAllergiesChanged()">
                    <mat-option value="lidocaine">Lidocaine</mat-option>
                    <mat-option value="latex">Latex</mat-option>
                    <mat-option value="antibiotics">Antibiotics</mat-option>
                    <mat-option value="other">Other</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="full">
                  <mat-label>Allergies (free text)</mat-label>
                  <textarea matInput rows="2" formControlName="allergyNotes"></textarea>
                </mat-form-field>

                <div class="half toggle-row"><span>HSV history</span><mat-slide-toggle formControlName="hsvHistory" (change)="onHsvHistoryChange()"></mat-slide-toggle></div>
                <div class="half toggle-row"><span>Pregnancy</span><mat-slide-toggle formControlName="pregnancy" (change)="onPregnancyChange()"></mat-slide-toggle></div>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Fitzpatrick skin type</mat-label>
                  <mat-select formControlName="fitzpatrickSkinType">
                    @for (skin of [1,2,3,4,5,6]; track skin) {
                      <mat-option [value]="skin">Type {{ skin }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Acne severity</mat-label>
                  <mat-select formControlName="acneSeverity">
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Pigmentation</mat-label>
                  <mat-select formControlName="pigmentation">
                    <mat-option value="none">None</mat-option>
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Scarring (type + severity)</mat-label>
                  <input matInput formControlName="scarring" />
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Volume loss</mat-label>
                  <mat-select formControlName="volumeLoss">
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>
            </mat-tab>

            <mat-tab label="Neuromodulator" [disabled]="isNeuromodulatorDisabled()">
              <div class="tab-body" [formGroup]="neuromodulatorGroup">
                @if (isNeuromodulatorDisabled()) {
                  <div class="disabled-notice">
                    <mat-icon>block</mat-icon>
                    <span>Neuromodulator procedures are contraindicated during pregnancy.</span>
                  </div>
                }
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Product name</mat-label>
                  <mat-select formControlName="productName">
                    <mat-option value="Botox">Botox</mat-option>
                    <mat-option value="Dysport">Dysport</mat-option>
                    <mat-option value="Xeomin">Xeomin</mat-option>
                  </mat-select>
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Lot number</mat-label>
                  <input matInput formControlName="lotNumber" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Expiry date</mat-label>
                  <input matInput type="date" formControlName="expiryDate" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Dilution</mat-label>
                  <input matInput type="number" formControlName="dilution" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Total units drawn</mat-label>
                  <input matInput type="number" formControlName="totalUnitsDrawn" />
                </mat-form-field>

                <div class="full" [formGroup]="unitsAreaGroup">
                  <div class="block-title">Units injected per area</div>
                  <div class="grid-2">
                    <mat-form-field appearance="outline"><mat-label>Glabella</mat-label><input matInput type="number" formControlName="glabella" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Forehead</mat-label><input matInput type="number" formControlName="forehead" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Crow's feet</mat-label><input matInput type="number" formControlName="crowsFeet" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Masseter</mat-label><input matInput type="number" formControlName="masseter" /></mat-form-field>
                  </div>
                </div>

                <mat-form-field appearance="outline" class="half"><mat-label>Needle type</mat-label><input matInput formControlName="needleType" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Injection technique</mat-label><input matInput formControlName="injectionTechnique" /></mat-form-field>
                <div class="half toggle-row"><span>Complications</span><mat-slide-toggle formControlName="complications" (change)="onComplicationsToggled('neuromodulator')"></mat-slide-toggle></div>
                <mat-form-field appearance="outline" class="full"><mat-label>Post-care instructions</mat-label><textarea matInput rows="2" formControlName="postCareInstructions" readonly></textarea></mat-form-field>

                <div class="full">
                  <div class="block-title">Photos</div>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #nPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #nTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <button mat-stroked-button type="button" (click)="triggerPhotoUpload('neuromodulator', nPhoto)">Upload Photo</button>
                    <input #nPhoto type="file" accept="image/*" (change)="onPhotoSelected('neuromodulator', nPhoto.files, nPhase.value || 'Before', nTag.value || 'frontal')" style="display:none" />
                  </div>
                  <div class="photo-grid">
                    @for (img of tabPhotos().neuromodulator; track $index) {
                      <div class="photo-card">
                        <div class="photo-badge">{{ img.phase }} ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â· {{ img.tag }}</div>
                        <img [src]="img.url || ''" [alt]="img.phase + ' ' + img.tag" />
                        <div class="photo-actions">
                          <button mat-icon-button type="button" (click)="zoomPhoto(img.url || '')" matTooltip="Zoom"><mat-icon>zoom_in</mat-icon></button>
                          <button mat-icon-button type="button" (click)="removePhoto('neuromodulator', $index)" matTooltip="Remove" color="warn"><mat-icon>close</mat-icon></button>
                        </div>
                      </div>
                    }
                  </div>
                </div>
              </div>
            </mat-tab>

            <mat-tab label="Dermal Filler" [disabled]="isDermalFillerDisabled()">
              <div class="tab-body" [formGroup]="dermalFillerGroup">
                @if (isDermalFillerDisabled()) {
                  <div class="disabled-notice">
                    <mat-icon>block</mat-icon>
                    <span>Dermal filler procedures are contraindicated during pregnancy.</span>
                  </div>
                }
                <mat-form-field appearance="outline" class="half"><mat-label>Product name</mat-label><input matInput formControlName="productName" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Volume per syringe</mat-label><input matInput type="number" formControlName="volumePerSyringe" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Total volume used</mat-label><input matInput type="number" formControlName="totalVolumeUsed" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Injection areas</mat-label><mat-select formControlName="injectionAreas" multiple><mat-option value="lips">Lips</mat-option><mat-option value="cheeks">Cheeks</mat-option><mat-option value="nasolabial">Nasolabial folds</mat-option><mat-option value="jawline">Jawline</mat-option></mat-select></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Plane</mat-label><mat-select formControlName="plane"><mat-option value="subdermal">Subdermal</mat-option><mat-option value="supraperiosteal">Supraperiosteal</mat-option><mat-option value="deep-dermal">Deep dermal</mat-option></mat-select></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Cannula or needle</mat-label><mat-select formControlName="cannulaOrNeedle"><mat-option value="cannula">Cannula</mat-option><mat-option value="needle">Needle</mat-option></mat-select></mat-form-field>
                <div class="half toggle-row"><span>Aspiration performed</span><mat-slide-toggle formControlName="aspirationPerformed"></mat-slide-toggle></div>
                <mat-form-field appearance="outline" class="full"><mat-label>Immediate outcome</mat-label><textarea matInput rows="2" formControlName="immediateOutcome"></textarea></mat-form-field>
                <div class="full action-row"><button mat-raised-button color="warn" type="button" (click)="showVascularProtocol()">Vascular Occlusion Protocol</button></div>
                <div class="half toggle-row"><span>Complications</span><mat-slide-toggle formControlName="complications" (change)="onComplicationsToggled('dermalFiller')"></mat-slide-toggle></div>

                <div class="full">
                  <div class="block-title">Photos</div>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #dPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #dTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <button mat-stroked-button type="button" (click)="triggerPhotoUpload('dermalFiller', dPhoto)">Upload Photo</button>
                    <input #dPhoto type="file" accept="image/*" (change)="onPhotoSelected('dermalFiller', dPhoto.files, dPhase.value || 'Before', dTag.value || 'frontal')" style="display:none" />
                  </div>
                  <div class="photo-grid">
                    @for (img of tabPhotos().dermalFiller; track $index) {
                      <div class="photo-card">
                        <div class="photo-badge">{{ img.phase }} ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â· {{ img.tag }}</div>
                        <img [src]="img.url || ''" [alt]="img.phase + ' ' + img.tag" />
                        <div class="photo-actions">
                          <button mat-icon-button type="button" (click)="zoomPhoto(img.url || '')" matTooltip="Zoom"><mat-icon>zoom_in</mat-icon></button>
                          <button mat-icon-button type="button" (click)="removePhoto('dermalFiller', $index)" matTooltip="Remove" color="warn"><mat-icon>close</mat-icon></button>
                        </div>
                      </div>
                    }
                  </div>
                </div>
              </div>
            </mat-tab>

            <mat-tab label="Laser" [disabled]="isLaserDisabled()">
              <div class="tab-body" [formGroup]="laserGroup">
                @if (isLaserDisabled()) {
                  <div class="disabled-notice">
                    <mat-icon>block</mat-icon>
                    <span>Laser procedures are contraindicated during pregnancy.</span>
                  </div>
                }
                <mat-form-field appearance="outline" class="half"><mat-label>Device name</mat-label><input matInput formControlName="deviceName" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Wavelength</mat-label><input matInput formControlName="wavelength" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Fluence</mat-label><input matInput formControlName="fluence" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Pulse duration</mat-label><input matInput formControlName="pulseDuration" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Spot size</mat-label><input matInput formControlName="spotSize" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Endpoint</mat-label><mat-select formControlName="endpoint"><mat-option value="erythema">Erythema</mat-option><mat-option value="edema">Edema</mat-option></mat-select>
                </mat-form-field>
                <div class="half toggle-row"><span>Test patch</span><mat-slide-toggle formControlName="testPatch"></mat-slide-toggle></div>
                <div class="half toggle-row"><span>Complications</span><mat-slide-toggle formControlName="complications" (change)="onComplicationsToggled('laser')"></mat-slide-toggle></div>

                <div class="full">
                  <div class="block-title">Photos</div>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #lPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #lTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <button mat-stroked-button type="button" (click)="triggerPhotoUpload('laser', lPhoto)">Upload Photo</button>
                    <input #lPhoto type="file" accept="image/*" (change)="onPhotoSelected('laser', lPhoto.files, lPhase.value || 'Before', lTag.value || 'frontal')" style="display:none" />
                  </div>
                  <div class="photo-grid">
                    @for (img of tabPhotos().laser; track $index) {
                      <div class="photo-card">
                        <div class="photo-badge">{{ img.phase }} ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â· {{ img.tag }}</div>
                        <img [src]="img.url || ''" [alt]="img.phase + ' ' + img.tag" />
                        <div class="photo-actions">
                          <button mat-icon-button type="button" (click)="zoomPhoto(img.url || '')" matTooltip="Zoom"><mat-icon>zoom_in</mat-icon></button>
                          <button mat-icon-button type="button" (click)="removePhoto('laser', $index)" matTooltip="Remove" color="warn"><mat-icon>close</mat-icon></button>
                        </div>
                      </div>
                    }
                  </div>
                </div>
              </div>
            </mat-tab>
          </mat-tab-group>

          <!-- Services rendered for this session (REQUIRED) -->
          <div class="services-section">
            <mat-form-field appearance="outline" class="full">
              <mat-label>Services Rendered for This Session *</mat-label>
              <textarea matInput rows="4" formControlName="services" required placeholder="Enter all services rendered during this consultation session..."></textarea>
              @if (saveAttempted() && form.controls.services.invalid) {
                <mat-error>Services rendered are required.</mat-error>
              }
            </mat-form-field>
            @if (saveAttempted() && !selectedVisitConsultId().trim()) {
              <div class="required-inline-error">ConsultId is required.</div>
            }
            @if (saveAttempted() && !selectedVisitPNo().trim()) {
              <div class="required-inline-error">PNo is required.</div>
            }
            @if (saveAttempted() && !providerEmpId()) {
              <div class="required-inline-error">Provider (logged-in EmpID) is required.</div>
            }
          </div>

          <div class="save-row">
            @if (lastSaveError()) {
              <div class="save-error" data-testid="procedures-save-error">{{ lastSaveError() }}</div>
            }
            <button mat-stroked-button type="button" (click)="closeDialog()" [disabled]="loadingIndicator()">
              Cancel
            </button>
            <button mat-raised-button color="primary" type="button" (click)="saveOrUpdate()" [disabled]="loadingIndicator()" data-testid="procedures-save-button">
              {{ currentConsultationId() ? 'Update All Tabs' : 'Save All Tabs' }}
            </button>
          </div>
        </form>
      </mat-card>

      <!-- Emergency Protocols Section -->
      <div #emergencyProtocols>
        @if (showEmergencyProtocols()) {
          <mat-card class="emergency-protocols-card">
            <mat-card-header>
              <mat-card-title>Emergency Protocols & Complication Management</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="protocol-section">
                <h4><mat-icon>local_hospital</mat-icon> Vascular Occlusion (Filler)</h4>
                <ul>
                  <li><strong>Stop injection immediately</strong></li>
                  <li>Massage area gently for 15 minutes</li>
                  <li>Apply warm compress (not hot)</li>
                  <li>Consider hyaluronidase injection protocol</li>
                  <li>Monitor perfusion every 5 minutes</li>
                  <li><strong>Call emergency if: skin blanching, prolonged pain, or ischemia</strong></li>
                </ul>
              </div>
              <div class="protocol-section">
                <h4><mat-icon>local_hospital</mat-icon> Ptosis or Brow Droop (Botox)</h4>
                <ul>
                  <li>Patient education: takes 2ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦ÃƒÂ¢Ã¢â€šÂ¬Ã…â€œ4 weeks for partial resolution</li>
                  <li>Avoid strong brow movements for 7 days</li>
                  <li>Consider apraclonidine 0.5% eye drops (if approved)</li>
                  <li>Follow-up in 2ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦ÃƒÂ¢Ã¢â€šÂ¬Ã…â€œ4 weeks for potential touch-up</li>
                  <li>Document thoroughly</li>
                </ul>
              </div>
              <div class="protocol-section">
                <h4><mat-icon>local_hospital</mat-icon> Allergic Reaction or Anaphylaxis</h4>
                <ul>
                  <li>Stop procedure immediately</li>
                  <li>Position patient upright (or supine if needed)</li>
                  <li><strong>Administer epinephrine IM if anaphylaxis</strong></li>
                  <li>Provide oxygen if available</li>
                  <li><strong>Call 911</strong></li>
                  <li>Monitor vitals continuously</li>
                </ul>
              </div>
              <div class="protocol-section">
                <h4><mat-icon>local_hospital</mat-icon> Post-Laser Complications</h4>
                <ul>
                  <li>Excessive erythema/swelling: Cool compress, NSAIDs, corticosteroid cream if authorized</li>
                  <li>Hyperpigmentation: Recommend SPF 50+, avoid sun, consider depigmenting agents</li>
                  <li>Infection signs (fever, pus, spreading erythema): Start antibiotics, document, consider dermatology referral</li>
                  <li>Scarring or atrophy: Early referral to dermatology or plastic surgeon</li>
                </ul>
              </div>
            </mat-card-content>
          </mat-card>
        }
      </div>

      <!-- Auto-generated Procedure Note -->
      @if (generatedProcedureNote()) {
        <mat-card class="procedure-note-card">
          <mat-card-header>
            <mat-card-title>Procedure Summary</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <pre>{{ generatedProcedureNote() }}</pre>
          </mat-card-content>
        </mat-card>
      }

      @if (zoomedPhotoUrl()) {
        <div class="photo-lightbox" (click)="zoomedPhotoUrl.set(null)">
          <img [src]="zoomedPhotoUrl()!" alt="zoomed photo" (click)="$event.stopPropagation()" />
          <button mat-icon-button type="button" class="lightbox-close" (click)="zoomedPhotoUrl.set(null)" matTooltip="Close">
            <mat-icon>close</mat-icon>
          </button>
        </div>
      }
    </div>
  `,
  styles: [`
    :host { display: block; }
    .procedures-page { padding: 20px; max-height: 90vh; overflow-y: auto; box-sizing: border-box; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; padding-bottom: 12px; border-bottom: 1px solid #e0e0e0; }
    .dialog-header h2 { margin: 0; font-size: 1.25rem; }
    .close-btn { color: #999; }
    .close-btn:hover { color: #333; }

    .page-header { margin-bottom: 16px; display: flex; justify-content: space-between; align-items: flex-start; }
    .page-header > div { flex: 1; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .header-attendance-summary { margin-top: 10px; max-width: 980px; }

    .alert { display: flex; align-items: flex-start; gap: 12px; padding: 12px 16px; border-radius: 6px; margin-bottom: 12px; font-size: 0.95rem; }
    .alert-warning { background: #fff3cd; border: 1px solid #ffc107; color: #856404; }
    .alert-info { background: #d1ecf1; border: 1px solid #17a2b8; color: #0c5460; }
    .alert-danger { background: #f8d7da; border: 1px solid #f5c6cb; color: #721c24; }
    .alert mat-icon { flex-shrink: 0; margin-top: 2px; }
    .alert-content { flex: 1; }
    .alert-action { margin-top: 8px; }

    .hard-stop-alert { border-left: 6px solid #dc3545; font-weight: 600; }
    .allergy-alert { border-left: 6px solid #e74c3c; font-weight: 600; animation: pulse 1.5s infinite; }
    .duplicate-alert { border-left: 6px solid #ff9800; }

    .emergency-bar { background: #ff6b6b; color: white; padding: 12px 16px; border-radius: 6px; margin-bottom: 12px; display: flex; align-items: center; gap: 12px; }
    .emergency-content { display: flex; align-items: center; gap: 12px; flex: 1; }
    .pulse-icon { animation: pulse 1.5s infinite; }

    .disabled-notice { display: flex; align-items: center; gap: 8px; padding: 12px; background: #f5f5f5; border-left: 4px solid #f44336; color: #d32f2f; font-weight: 500; }

    .form-shell { padding: 12px; }
    .services-section { width: 100%; padding: 12px; margin: 12px 0; background: #fafafa; border-radius: 4px; border: 1px solid #e8e8e8; box-sizing: border-box; }
    .services-section .mat-mdc-form-field { width: 100%; }
    .services-section textarea[matInput] { width: 100%; box-sizing: border-box; }
    .save-row {
      position: sticky;
      bottom: 0;
      z-index: 6;
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 14px;
      padding: 12px 0;
      background: var(--mat-app-surface, #1e1e1e);
      border-top: 1px solid rgba(255, 255, 255, 0.08);
    }
    .save-error {
      color: #ff6b6b;
      font-size: 12px;
      margin-right: auto;
      align-self: center;
    }
    .patient-field { width: min(460px, 100%); }
    .tab-body { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; padding: 14px 2px 2px; }
    .consent-tab { grid-template-columns: 1fr; }
    .consent-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    .consent-preview { border: 1px solid #e0e0e0; border-radius: 8px; padding: 0; background: #f5f5f5; overflow: hidden; }
    .consent-preview .block-title { padding: 10px 14px 8px; font-weight: 600; font-size: 13px; color: #374151; }
    .empty-consent { color: #666; padding: 10px 14px; }
    .consent-item { border: 1px solid #ddd; border-radius: 8px; background: #fff; padding: 10px; margin-bottom: 10px; }
    .consent-content { margin-top: 8px; white-space: pre-wrap; }
    .full { grid-column: 1 / -1; }
    .consent-table-card { border: 1px solid #e0e0e0; border-radius: 8px; padding: 12px; background: #fff; }
    .consent-table-toolbar { display: grid; grid-template-columns: 1fr auto; gap: 10px; align-items: center; margin-bottom: 10px; }
    .consent-search-field { width: 100%; }
    .data-table { width: 100%; }
    .pager-row { display: flex; align-items: center; justify-content: flex-end; gap: 10px; margin-top: 10px; }

    .emergency-protocols-card { margin-top: 20px; border-left: 6px solid #ff6b6b; }
    .protocol-section { margin-bottom: 16px; padding: 12px; background: #f5f5f5; border-radius: 4px; }
    .protocol-section h4 { display: flex; align-items: center; gap: 8px; margin: 0 0 8px 0; color: #d32f2f; }
    .protocol-section ul { margin: 8px 0; padding-left: 20px; }
    .protocol-section li { margin: 4px 0; }

    .procedure-note-card { margin-top: 20px; }
    .procedure-note-card pre { white-space: pre-wrap; word-wrap: break-word; font-size: 0.85rem; line-height: 1.5; background: #f5f5f5; padding: 12px; border-radius: 4px; }

    @keyframes pulse {
      0%, 100% { opacity: 1; }
      50% { opacity: 0.7; }
    }

    @media (max-width: 992px) {
      .tab-body, .grid-2 { grid-template-columns: 1fr; }
      .photo-toolbar { grid-template-columns: 1fr; }
      .half { grid-column: 1 / -1; }
      .consent-grid { grid-template-columns: 1fr; }
      .consent-table-toolbar { grid-template-columns: 1fr; }
    }

    @media (max-width: 767.98px) {
      .procedures-page { padding: 12px; }
      .page-header { flex-direction: column; gap: 10px; }
      .page-header > div { width: 100%; }
      .save-row { justify-content: stretch; }
      .save-row button { width: 100%; min-height: 44px; }
      .toggle-row { padding: 6px 2px; }
      .emergency-content { flex-direction: column; align-items: flex-start; }
      .emergency-content button { width: 100%; }
    }

    .photo-grid { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 8px; }
    .photo-card { position: relative; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; width: 160px; flex-shrink: 0; background: #f9f9f9; }
    .photo-card img { display: block; width: 100%; height: 140px; object-fit: cover; }
    .photo-badge { font-size: 11px; font-weight: 600; padding: 4px 8px; background: rgba(0,0,0,0.55); color: #fff; position: absolute; top: 0; left: 0; right: 0; }
    .photo-actions { position: absolute; bottom: 0; left: 0; right: 0; display: flex; justify-content: space-between; background: rgba(0,0,0,0.45); padding: 2px 4px; }
    .photo-actions button { color: #fff; width: 32px; height: 32px; line-height: 32px; }
    .photo-actions button mat-icon { font-size: 18px; width: 18px; height: 18px; line-height: 18px; }

    .photo-lightbox { position: fixed; inset: 0; background: rgba(0,0,0,0.88); z-index: 9999; display: flex; align-items: center; justify-content: center; }
    .photo-lightbox img { max-width: 90vw; max-height: 88vh; object-fit: contain; border-radius: 6px; box-shadow: 0 4px 32px rgba(0,0,0,0.6); }
    .lightbox-close { position: absolute; top: 16px; right: 16px; color: #fff; background: rgba(0,0,0,0.5); }

    @media (max-width: 575.98px) {
      .procedures-page { padding: 10px; }
      .form-shell { padding: 8px; }
      .tab-body { gap: 8px; }
      .photo-card { width: 140px; }
      .photo-card img { height: 120px; }
    }

    .required-inline-error { color: #d32f2f; font-size: 12px; margin-top: 4px; }
  `]
})
export class ProceduresEntryDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly hPatientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialogRef = inject(MatDialogRef<ProceduresEntryDialogComponent>);
  private readonly data = inject<ProceduresEntryDialogData>(MAT_DIALOG_DATA);
  private readonly moduleSettings = inject(ModuleSettingsService);
  private readonly authService = inject(AuthService);

  readonly loadingIndicator = signal(false);
  readonly patients = signal<AestheticPatient[]>([]);
  readonly currentConsultationId = signal<number | null>(null);
  readonly selectedTabIndex = signal(0);

  readonly tabPhotos = signal<TabPhotoCollection>({
    neuromodulator: [],
    dermalFiller: [],
    laser: []
  });

  readonly generatedProcedureNote = signal<string>('');
  readonly safetyAlerts = signal<SafetyAlert[]>([]);
  readonly showEmergencyProtocols = signal(false);
  readonly reportedComplications = signal<{ tab: string; timestamp: Date }[]>([]);
  readonly zoomedPhotoUrl = signal<string | null>(null);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly selectedConsentProcedureType = signal('');
  readonly selectedVisitConsultId = signal('');
  readonly signedConsents = signal<AestheticSignedConsent[]>([]);
  readonly procedureTypes = signal<string[]>(['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']);
  readonly consentTableRows = signal<AestheticSignedConsent[]>([]);
  readonly consentSearchText = signal('');
  readonly consentPageIndex = signal(0);
  readonly consentPageSize = 10;
  readonly consentTableColumns = ['consultId', 'patient', 'procedureType', 'signedDate', 'doctorViewed'];

  readonly selectedAttendanceSummary = computed<VwhRecord | null>(() => {
    const consultId = this.selectedVisitConsultId().trim();
    if (!consultId) {
      return null;
    }

    const visit = this.todayVisits().find(x => (x.consultId || '').trim() === consultId);
    if (!visit) {
      return null;
    }

    const patient = this.findPatientByAttendancePno(this.patients(), visit.pNo) ?? null;
    return this.buildAttendanceSummary(visit, patient);
  });

  readonly selectedProcedureSignedConsents = computed(() => {
    const normalizedProcedure = this.selectedConsentProcedureType().trim().toLowerCase();
    return this.signedConsents().filter(c => (c.procedureType ?? '').trim().toLowerCase() === normalizedProcedure);
  });

  readonly selectedProcedureSignedConsent = computed<AestheticSignedConsent | null>(() => {
    const consents = this.selectedProcedureSignedConsents();
    if (!consents.length) return null;
    const active = consents.filter(c => !c.isVoided);
    const pool = active.length ? active : consents;
    return pool.reduce((latest, c) =>
      (c.signedDate ?? '') >= (latest.signedDate ?? '') ? c : latest
    );
  });

  readonly patientAttendanceOptions = computed<{
    trackKey: string;
    consultId: string;
    patientId: number;
    pNo: string;
    label: string;
  }[]>(() => {
    const patients = this.patients();

    return this.todayVisits()
      .filter(visit => !!visit.consultId?.trim() && !!visit.pNo?.trim())
      .map(visit => {
        const patient = this.findPatientByAttendancePno(patients, visit.pNo);
        const patientName = (visit.fullname || '').trim() || this.resolveAttendancePatientName(visit, patient);
        const visitDate = this.formatAttendanceDate(visit.recDate);

        return {
          trackKey: `${visit.consultId}-${visit.pNo}`,
          consultId: visit.consultId,
          patientId: patient?.id ?? 0,
          pNo: visit.pNo,
          label: `${patientName} ${visitDate} [${visit.consultId}]`
        };
      })
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  readonly filteredConsentRows = computed(() => {
    const term = this.consentSearchText().toLowerCase();
    const rows = this.consentTableRows();

    if (term) {
      return rows.filter(item =>
        `${item.consultId} ${item.pNo} ${item.procedureType} ${this.resolveConsentPatientName(item.pNo)}`
          .toLowerCase()
          .includes(term)
      );
    }

    const todayKey = this.toLocalDateKey(new Date());
    return rows.filter(item => this.toLocalDateKey(item.signedDate) === todayKey);
  });

  readonly consentTotalPages = computed(() => {
    const total = this.filteredConsentRows().length;
    return Math.max(1, Math.ceil(total / this.consentPageSize));
  });

  readonly pagedConsentRows = computed(() => {
    const rows = this.filteredConsentRows();
    const pageIndex = Math.min(this.consentPageIndex(), Math.max(0, this.consentTotalPages() - 1));
    const start = pageIndex * this.consentPageSize;
    return rows.slice(start, start + this.consentPageSize);
  });

  // Workflow state signals
  readonly isPregnant = signal(false);
  readonly hasHsvHistory = signal(false);
  readonly isFillerSelected = signal(false);
  readonly saveAttempted = signal(false);
  readonly lastSaveError = signal<string>('');
  readonly selectedVisitPNo = signal('');
  readonly selectedClinic = signal('');
  readonly consultationDateIso = signal(this.toLocalIsoString(new Date()));
  readonly providerEmpId = computed(() => (this.authService.currentUser?.empID || '').trim());

  // Safety state
  readonly patientAllergies = signal<string[]>([]);
  readonly recentTreatments = signal<{ procedure: string; date: Date }[]>([]);

  // Computed states
  readonly workflowAlerts = computed(() => ({
    pregnancy: this.isPregnant(),
    hsvHistory: this.hasHsvHistory(),
    fillerVascularity: this.isFillerSelected()
  }));

  readonly isNeuromodulatorDisabled = computed(() => this.isPregnant());
  readonly isDermalFillerDisabled = computed(() => this.isPregnant());
  readonly isLaserDisabled = computed(() => this.isPregnant());

  readonly hasActiveComplications = computed(() => this.reportedComplications().length > 0);

  form = this.fb.nonNullable.group({
    patientId: [0, Validators.min(1)],
    consultation: this.fb.nonNullable.group({
      chiefComplaint: [''],
      duration: [''],
      expectationRealistic: [true],
      medicalConditions: this.fb.nonNullable.group({
        diabetes: [false],
        hypertension: [false],
        keloid: [false],
        autoimmune: [false],
        bleedingDisorder: [false]
      }),
      medications: [[] as string[]],
      allergySelections: [[] as string[]],
      allergyNotes: [''],
      hsvHistory: [false],
      pregnancy: [false],
      fitzpatrickSkinType: [1],
      acneSeverity: ['mild'],
      pigmentation: ['none'],
      scarring: [''],
      volumeLoss: ['mild']
    }),
    neuromodulator: this.fb.nonNullable.group({
      productName: ['Botox'],
      lotNumber: [''],
      expiryDate: [''],
      dilution: [0],
      totalUnitsDrawn: [0],
      unitsPerArea: this.fb.nonNullable.group({
        glabella: [0],
        forehead: [0],
        crowsFeet: [0],
        masseter: [0]
      }),
      needleType: [''],
      injectionTechnique: [''],
      complications: [false],
      postCareInstructions: ['']
    }),
    dermalFiller: this.fb.nonNullable.group({
      productName: [''],
      volumePerSyringe: [0],
      totalVolumeUsed: [0],
      injectionAreas: [[] as string[]],
      plane: ['subdermal'],
      cannulaOrNeedle: ['cannula'],
      aspirationPerformed: [false],
      immediateOutcome: [''],
      complications: [false]
    }),
    laser: this.fb.nonNullable.group({
      deviceName: [''],
      wavelength: [''],
      fluence: [''],
      pulseDuration: [''],
      spotSize: [''],
      endpoint: ['erythema'],
      testPatch: [false],
      complications: [false]
    }),
    services: ['', Validators.required]
  });

  get consultationGroup() { return this.form.controls.consultation; }
  get medicalConditionsGroup() { return this.form.controls.consultation.controls.medicalConditions; }
  get neuromodulatorGroup() { return this.form.controls.neuromodulator; }
  get unitsAreaGroup() { return this.form.controls.neuromodulator.controls.unitsPerArea; }
  get dermalFillerGroup() { return this.form.controls.dermalFiller; }
  get laserGroup() { return this.form.controls.laser; }

  readonly generatedPostCare = computed(() => {
    const n = this.neuromodulatorGroup.getRawValue();
    const units = this.unitsAreaGroup.getRawValue();
    const areaCount = Object.values(units).filter(x => Number(x) > 0).length;
    const advice = [
      'Remain upright for 4 hours.',
      'Avoid facial massage for 24 hours.',
      'No strenuous activity for 24 hours.'
    ];

    if (areaCount > 2) {
      advice.push('Use cool compress if mild swelling occurs.');
    }

    if (n.complications) {
      advice.push('Return immediately if pain, visual changes, or severe asymmetry occur.');
    }

    return advice.join(' ');
  });

  constructor() {
    effect(() => {
      const pregnancy = this.consultationGroup.get('pregnancy')?.value ?? false;
      const hsvHistory = this.consultationGroup.get('hsvHistory')?.value ?? false;

      this.isPregnant.set(pregnancy);
      this.hasHsvHistory.set(hsvHistory);

      if (pregnancy) {
        this.selectedTabIndex.set(1);
      }
    });

    effect(() => {
      const fillerProduct = this.dermalFillerGroup.get('productName')?.value;
      this.isFillerSelected.set(!!fillerProduct && fillerProduct.trim().length > 0);
    });

    effect(() => {
      this.validateAllergiesAndDuplicates();
    });

    effect(() => {
      if (this.data && this.data.consultation) return;
      const visits = this.todayVisits();
      if (this.selectedVisitConsultId().trim() || visits.length === 0) return;
      const first = visits.find(v => !!v.consultId && !!v.consultId.trim() && !!v.pNo && !!v.pNo.trim());
      if (!first) return;
      this.selectedVisitConsultId.set(first.consultId.trim());
      this.selectedVisitPNo.set(first.pNo.trim());
      if (!this.selectedClinic()) this.selectedClinic.set("Aesthetic");
      const match = this.findPatientByAttendancePno(this.patients(), first.pNo);
      if (match && match.id) this.form.controls.patientId.setValue(match.id, { emitEvent: false });
    }, { allowSignalWrites: true });
  }

  ngOnInit(): void {
    this.loadPatients();
    this.loadLegacyPatients();
    this.loadAttendances();
    this.loadProcedureTypes();
    this.refreshConsentTable();

    const initialTab = (this.data.initialTab || '').toString();
    this.selectedTabIndex.set(this.mapTabToIndex(initialTab));

    if (this.data.consultation) {
      this.currentConsultationId.set(this.data.consultation.id ?? null);
      this.form.controls.patientId.setValue(this.data.consultation.patientId ?? 0);
      this.selectedVisitConsultId.set((this.data.consultation.consultId || '').trim());
      this.selectedConsentProcedureType.set((this.data.consultation.procedureType || '').trim());
      this.selectedVisitPNo.set((this.data.consultation.pNo || '').trim());
      if (!this.selectedClinic()) this.selectedClinic.set('Aesthetic');
      if (!this.form.controls.services.value) this.form.controls.services.setValue(this.data.consultation.services || 'Standard aesthetics consultation services.');
      if (!this.form.controls.consultation.get('allergyNotes')?.value && !this.form.controls.consultation.get('allergySelections')?.value?.length) this.form.controls.consultation.patchValue({ allergyNotes: this.data.consultation.allergies || 'None reported', medications: this.data.consultation.currentMedications ? this.data.consultation.currentMedications.split(',').map((x: string) => x.trim()).filter(Boolean) : ['none'] });

      this.loadFromConsultation(this.data.consultation);
      this.loadSignedConsentsForConsultation(this.data.consultation);
    }

    this.neuromodulatorGroup.valueChanges.subscribe(() => {
      this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
    });
    this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
  }

  private loadAttendances(): void {
    this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().subscribe({
      next: visits => {
        this.todayVisits.set(visits || []);
      },
      error: error => {
        this.alertService.showStickyMessage('Load error', 'Unable to load today\'s attendance records.', MessageSeverity.error, error);
      }
    });
  }

  private loadLegacyPatients(): void {
    this.hPatientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => {
        this.legacyPatients.set(patients || []);
      },
      error: () => {
        this.legacyPatients.set([]);
      }
    });
  }

  private loadProcedureTypes(): void {
    this.moduleSettings.getModuleSettings<{ autoFollowUpDays: number; procedureTypes?: string[] }>('aesthetics', {
      autoFollowUpDays: 14,
      procedureTypes: ['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']
    }).then(settings => {
      const list = (settings.procedureTypes || []).map(x => (x || '').trim()).filter(Boolean);
      this.procedureTypes.set(list.length > 0 ? list : ['Procedures', 'Neuromodulator', 'Dermal Filler', 'Laser']);
    });
  }

  refreshConsentTable(): void {
    this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ includeVoided: true }).subscribe({
      next: consents => {
        this.consentTableRows.set(consents || []);
        this.consentPageIndex.set(0);
      },
      error: () => {
        this.consentTableRows.set([]);
        this.consentPageIndex.set(0);
      }
    });
  }

  onConsentSearchChanged(value: string): void {
    this.consentSearchText.set(value || '');
    this.consentPageIndex.set(0);
  }

  changeConsentPage(step: number): void {
    const next = this.consentPageIndex() + step;
    if (next < 0 || next >= this.consentTotalPages()) {
      return;
    }

    this.consentPageIndex.set(next);
  }

  private loadSignedConsentsForSelection(): void {
    const consultId = this.selectedVisitConsultId().trim();
    const option = this.patientAttendanceOptions().find(x => x.consultId === consultId);
    const pNo = (option?.pNo || '').trim();
    if (!pNo) {
      this.signedConsents.set([]);
      return;
    }

    this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({
      consultId,
      pNo,
      procedureType: this.selectedConsentProcedureType(),
      includeVoided: true
    }).subscribe({
      next: consents => this.signedConsents.set(consents || []),
      error: () => this.signedConsents.set([])
    });
  }

  private loadSignedConsentsForConsultation(consultation: AestheticConsultation): void {
    const consultId = (consultation.consultId || '').trim();
    const pNo = (consultation.pNo || '').trim();
    if (!consultId || !pNo) {
      this.signedConsents.set([]);
      return;
    }

    this.endpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({
      consultId,
      pNo,
      procedureType: consultation.procedureType,
      includeVoided: true
    }).subscribe({
      next: consents => this.signedConsents.set(consents || []),
      error: () => this.signedConsents.set([])
    });
  }

  private buildAttendanceSummary(visit: QryhvisitsForToday, patient: AestheticPatient | null): VwhRecord {
    const legacyPatient = this.legacyPatients().find(p => this.normalizePno(p.pno) === this.normalizePno(visit.pNo));
    const dob = legacyPatient?.dob || patient?.dateOfBirth;
    const fullName = (visit.fullname || '').trim()
      || `${legacyPatient?.pSurName ?? patient?.lastName ?? ''} ${legacyPatient?.pFirstname ?? patient?.firstName ?? ''}`.trim()
      || visit.pNo;

    return {
      consultId: visit.consultId ?? 'ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â',
      pNo: visit.pNo,
      clinicType: visit.clinicType,
      clientCat: visit.clientCat,
      coyname: visit.coyName,
      retainName: visit.retainName,
      fullname: fullName,
      dob,
      age: this.calculateAge(dob),
      patientPhotoBase64: legacyPatient?.patPixBase64
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

  onAllergiesChanged(): void {
    const selected = this.consultationGroup.get('allergySelections')?.value || [];
    this.patientAllergies.set(selected);
    this.validateAllergiesAndDuplicates();
  }

  onComplicationsToggled(tab: PhotoTab): void {
    const tabControls = this.form.controls[tab];
    const hasComplications = tabControls.controls.complications.value ?? false;
    if (hasComplications) {
      this.reportedComplications.update(c => [...c, { tab, timestamp: new Date() }]);
      this.showEmergencyProtocols.set(true);
      this.alertService.showStickyMessage(
        'Complication Reported',
        `Complication noted in ${tab}. Emergency protocols are available below.`,
        MessageSeverity.warn
      );
    }
  }

  onPregnancyChange(): void {
    const isPregnant = this.consultationGroup.get('pregnancy')?.value ?? false;
    if (isPregnant) {
      this.alertService.showStickyMessage(
        'Pregnancy status',
        'Neuromodulator, Dermal Filler, and Laser procedures are contraindicated. Switching to Consultation tab.',
        MessageSeverity.warn);
      this.selectedTabIndex.set(1);
    }
  }

  onHsvHistoryChange(): void {
    const hasHsv = this.consultationGroup.get('hsvHistory')?.value ?? false;
    if (hasHsv) {
      this.alertService.showStickyMessage(
        'HSV History Detected',
        'Antiviral prophylaxis is recommended. Start acyclovir (or valacyclovir) 1ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬ÃƒÂ¢Ã¢â‚¬Å¾Ã‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã‚Â¦ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Â ÃƒÂ¢Ã¢â€šÂ¬Ã¢â€žÂ¢ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦Ãƒâ€šÃ‚Â¡ÃƒÆ’Ã†â€™ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€šÃ‚Â¦ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Â¦ÃƒÂ¢Ã¢â€šÂ¬Ã…â€œ2 days before treatment to reduce outbreak risk.',
        MessageSeverity.info);
    }
  }

  openComplicationReport(): void {
    const message = `Report Safety Concern/Complication:
- Use this to document any adverse event, near-miss, or safety concern
- Include timestamp, affected area, symptoms, and action taken
- This creates a safety incident record for review`;

    const concern = prompt(message, '');
    if (concern && concern.trim()) {
      this.alertService.showStickyMessage(
        'Complication Logged',
        `Safety incident recorded and flagged for review: "${concern.substring(0, 50)}..."`,
        MessageSeverity.warn
      );
      this.reportedComplications.update(c => [...c, { tab: 'manual-report', timestamp: new Date() }]);
      this.showEmergencyProtocols.set(true);
    }
  }

  scrollToEmergencyProtocols(): void {
    const el = document.querySelector('[emergencyProtocols]');
    el?.scrollIntoView({ behavior: 'smooth' });
  }

  showVascularProtocol(): void {
    this.showEmergencyProtocols.set(true);
    this.alertService.showStickyMessage(
      'Vascular Occlusion Protocol',
      'Emergency protocol displayed below. Ensure vascular emergency kit is immediately accessible.',
      MessageSeverity.warn);
  }

  private validateAllergiesAndDuplicates(): void {
    const newAlerts: SafetyAlert[] = [];

    const allergies = this.patientAllergies();
    if (allergies.length > 0) {
      if (allergies.includes('lidocaine')) {
        newAlerts.push({
          type: 'allergy',
          title: 'Lidocaine Allergy',
          message: 'Lidocaine is contraindicated. This patient cannot receive local anesthesia with lidocaine. Use alternative anesthetic (mepivacaine, prilocaine) or proceed without local anesthetic if procedure allows.'
        });
      }
      if (allergies.includes('latex')) {
        newAlerts.push({
          type: 'allergy',
          title: 'Latex Allergy',
          message: 'Latex-free supplies required. Ensure all gloves, equipment, and materials are latex-free to prevent allergic reaction.'
        });
      }
    }

    // Check for duplicate treatments within last 2 weeks
    const recentProcs = this.recentTreatments();
    const twoWeeksAgo = new Date();
    twoWeeksAgo.setDate(twoWeeksAgo.getDate() - 14);

    for (const recent of recentProcs) {
      if (recent.date > twoWeeksAgo) {
        newAlerts.push({
          type: 'duplicate',
          title: `Recent ${recent.procedure}`,
          message: `This patient received ${recent.procedure} on ${recent.date.toLocaleDateString()}. Proceeding with same treatment within 2 weeks may increase adverse event risk. Review patient expectations and benefits carefully.`,
          action: () => this.alertService.showMessage('Confirmed', 'Duplicate treatment risk noted. Proceed with caution.', MessageSeverity.info),
          actionLabel: 'Acknowledged'
        });
      }
    }

    this.safetyAlerts.set(newAlerts);
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
    this.loadSignedConsentsForSelection();
  }

  onPatientVisitChanged(consultId: string): void {
    const normalizedConsultId = (consultId || '').trim();
    this.selectedVisitConsultId.set(normalizedConsultId);

    const selected = this.patientAttendanceOptions().find(x => x.consultId === normalizedConsultId);
    this.form.controls.patientId.setValue(selected?.patientId ?? 0);
    this.selectedVisitPNo.set(selected?.pNo ?? '');
    this.selectedClinic.set('Aesthetic');

    this.loadSignedConsentsForSelection();

    if (!selected?.patientId) {
      return;
    }

    this.onPatientChanged();
  }

  hasConsentSelection(): boolean {
    return !!this.selectedVisitConsultId().trim() && !!this.selectedConsentProcedureType().trim();
  }

  onConsentProcedureTypeChanged(procedureType: string): void {
    this.selectedConsentProcedureType.set(procedureType || '');
    this.loadSignedConsentsForSelection();
  }

  closeDialog(): void {
    this.dialogRef.close(false);
  }

  zoomPhoto(url: string): void {
    this.zoomedPhotoUrl.set(url || null);
  }

  removePhoto(tab: PhotoTab, index: number): void {
    this.tabPhotos.update(current => ({
      ...current,
      [tab]: current[tab].filter((_, i) => i !== index)
    }));
  }

  triggerPhotoUpload(tab: PhotoTab, input: HTMLInputElement): void {
    input.click();
  }

  onPhotoSelected(tab: PhotoTab, files: FileList | null, phase: string, tag: string): void {
    const file = files?.item(0);
    if (!file) return;

    const safePhase: PhotoPhase = phase === 'After' ? 'After' : 'Before';
    const safeTag: StandardTag = this.toStandardTag(tag);

    this.tabPhotos.update(current => ({
      ...current,
      [tab]: [...current[tab], { file, fileName: file.name, phase: safePhase, tag: safeTag, url: URL.createObjectURL(file) }]
    }));
  }

  getComparisonImages(tab: PhotoTab): { before: TabPhotoItem[]; after: TabPhotoItem[] } {
    const items = this.tabPhotos()[tab];
    return {
      before: items.filter(x => x.phase === 'Before'),
      after: items.filter(x => x.phase === 'After')
    };
  }

  saveOrUpdate(): void {
    // Defensive: prevent re-entrant clicks while a save is already in flight.
    if (this.loadingIndicator()) {
      return;
    }

    this.saveAttempted.set(true);
    this.lastSaveError.set('');
    // Clear any leftover sticky growl from a previous failed save so the user
    // can see the new loading overlay instead of an old error covering the dialog.
    this.alertService.resetStickyMessage();

    this.ensureDefaultSafetyValues();

    const consultId = this.selectedVisitConsultId().trim();
    const pNo = this.selectedVisitPNo().trim();
    const procedureType = this.selectedConsentProcedureType().trim() || 'Procedures';
    const consultationDate = this.consultationDateIso();

    if (
      this.form.controls.patientId.invalid ||
      this.form.controls.services.invalid ||
      !consultId ||
      !pNo ||
      !procedureType ||
      !consultationDate
    ) {
      const detail = this.describeMissingFields();
      this.lastSaveError.set(detail);
      this.alertService.showStickyMessage('Validation error', detail, MessageSeverity.warn);
      return;
    }

    if (this.hasHardStopAllergies()) {
      const msg = 'This patient has documented allergies (lidocaine) that are contraindicated for the selected procedures. Clear the allergy or select an alternative procedure.';
      this.lastSaveError.set(msg);
      this.alertService.showStickyMessage('HARD STOP: Incompatible Allergy', msg, MessageSeverity.error);
      return;
    }

    const payload = this.buildPayload();
    this.loadingIndicator.set(true);
    this.alertService.startLoadingMessage(this.currentConsultationId() ? 'Updating procedures...' : 'Saving procedures...');

    const consultationRequest = this.currentConsultationId()
      ? this.endpoint.updateConsultationEndpoint<AestheticConsultation>(this.currentConsultationId()!, payload)
      : this.endpoint.createConsultationEndpoint<AestheticConsultation>(payload);

    consultationRequest.subscribe({
      next: consultation => {
        // Wrap synchronous post-success work in try/catch so a throw here can't
        // strand the Save button in a disabled state.
        try {
          this.currentConsultationId.set(consultation.id);
          this.generateProcedureNote(consultation);
          this.uploadPendingPhotos(consultation.id);
        } catch (postError) {
          this.loadingIndicator.set(false);
          this.alertService.stopLoadingMessage();
          const detail = this.extractBackendError(postError, 'Unable to finalise save.');
          this.lastSaveError.set(detail);
          this.alertService.showStickyMessage('Save error', detail, MessageSeverity.error, postError);
        }
      },
      error: error => {
        this.loadingIndicator.set(false);
        this.alertService.stopLoadingMessage();
        const detail = this.extractBackendError(error, 'Unable to save procedures.');
        this.lastSaveError.set(detail);
        this.alertService.showStickyMessage('Save error', detail, MessageSeverity.error, error);
      },
      // Defensive backstop: tear down loading state on completion as well.
      // Teardown is idempotent so this is safe even if `error` already ran.
      complete: () => {
        this.loadingIndicator.set(false);
        this.alertService.stopLoadingMessage();
      }
    });
  }

  // Build an ISO-8601 string with the user's local offset (e.g.
  // "2026-07-27T09:30:00.000+01:00") so the server stores the wall-clock the
  // user actually sees. Unlike `new Date().toISOString()` (which always emits
  // "Z" / UTC), this preserves the offset so the backend serializer can round-
  // trip without drifting by an hour.
  private toLocalIsoString(d: Date): string {
    const pad = (n: number) => `${n}`.padStart(2, '0');
    const y = d.getFullYear();
    const mo = pad(d.getMonth() + 1);
    const da = pad(d.getDate());
    const hh = pad(d.getHours());
    const mm = pad(d.getMinutes());
    const ss = pad(d.getSeconds());
    const ms = `${d.getMilliseconds()}`.padStart(3, '0');
    const offMin = -d.getTimezoneOffset();
    const sign = offMin >= 0 ? '+' : '-';
    const aOff = Math.abs(offMin);
    const oh = pad(Math.floor(aOff / 60));
    const om = pad(aOff % 60);
    return `${y}-${mo}-${da}T${hh}:${mm}:${ss}.${ms}${sign}${oh}:${om}`;
  }

  private ensureDefaultSafetyValues(): void {
    const consultation = this.consultationGroup;

    const medications = consultation.get('medications')?.value as string[] | null;
    if (!medications || medications.length === 0) {
      consultation.patchValue({ medications: ['none'] }, { emitEvent: false });
    }

    const allergySelections = consultation.get('allergySelections')?.value as string[] | null;
    const allergyNotes = (consultation.get('allergyNotes')?.value as string | null) ?? '';
    if ((!allergySelections || allergySelections.length === 0) && !allergyNotes.trim()) {
      consultation.patchValue({ allergyNotes: 'None reported' }, { emitEvent: false });
      this.patientAllergies.set([]);
    }

    const postCare = this.neuromodulatorGroup.get('postCareInstructions')?.value as string | null;
    if (!postCare || !postCare.trim()) {
      this.neuromodulatorGroup.patchValue(
        { postCareInstructions: this.generatedPostCare() || 'Standard post-care applies.' },
        { emitEvent: false }
      );
    }
  }

  private describeMissingFields(): string {
    const missing: string[] = [];
    if (this.form.controls.patientId.invalid) missing.push('Patient');
    if (!this.selectedVisitConsultId().trim()) missing.push('ConsultId');
    if (!this.selectedVisitPNo().trim()) missing.push('PNo');
    if (this.form.controls.services.invalid) missing.push('Services Rendered');
    if (!this.consultationDateIso()) missing.push('Consultation Date');
    if (!this.selectedConsentProcedureType().trim()) missing.push('Procedure Type');
    if (missing.length === 0) return 'Please complete required fields.';
    return 'Missing required field(s): ' + missing.join(', ') + '.';
  }

  private extractBackendError(error: any, fallback: string): string {
    if (!error) return fallback;
    if (error.error) {
      const body = error.error;
      if (typeof body === 'string' && body.trim()) return body;
      if (body.error_description && typeof body.error_description === 'string') return body.error_description;
      if (body.error && typeof body.error === 'string' && body.error !== 'invalid_grant') return body.error;
      if (body.title && typeof body.title === 'string') return body.title;
      if (body.detail && typeof body.detail === 'string') return body.detail;
      if (body.message && typeof body.message === 'string') return body.message;
      if (body.errors) {
        try {
          const flattened = Object.values(body.errors).flat().filter(x => typeof x === 'string') as string[];
          if (flattened.length) return flattened.join('; ');
        } catch { /* ignore */ }
      }
    }
    if (error.message && typeof error.message === 'string') return error.message;
    return fallback;
  }
  private generateProcedureNote(consultation: AestheticConsultation): void {
    const consultation_data = this.tryParseJson<{ chiefComplaint?: string; duration?: string; expectationRealistic?: boolean; fitzpatrickSkinType?: number; hsvHistory?: boolean; pregnancy?: boolean; }>(consultation.treatmentPlan);
    const neuromodulator_data = this.tryParseJson<{ productName?: string; lotNumber?: string; totalUnitsDrawn?: number; unitsPerArea?: { glabella?: number; forehead?: number; crowsFeet?: number; masseter?: number; }; }>(consultation.injectionMapping);
    const dermalFiller_data = this.tryParseJson<{ productName?: string; totalVolumeUsed?: number; injectionAreas?: string[]; complications?: boolean; }>(consultation.risksAndComplications);
    const laser_data = this.tryParseJson<{ deviceName?: string; wavelength?: string; fluence?: string; testPatch?: boolean; }>(consultation.deviceSettings);

    const complications = this.reportedComplications().join(', ') || 'None reported';

    const note = `
=============================
AESTHETIC PROCEDURE NOTE
=============================
Date: ${new Date(consultation.consultationDate || '').toLocaleDateString()}
Provider: ${consultation.provider || 'Not recorded'}

SAFETY SUMMARY
--------------
Allergies: ${this.patientAllergies().join(', ') || 'None reported'}
Complications Reported: ${complications}

CONSULTATION
-----------
Chief Complaint: ${consultation_data?.chiefComplaint || 'N/A'}
Duration: ${consultation_data?.duration || 'N/A'}
Expectations: ${consultation_data?.expectationRealistic ? 'Realistic' : 'Unrealistic'}
Fitzpatrick Type: ${consultation_data?.fitzpatrickSkinType || 'N/A'}
HSV History: ${consultation_data?.hsvHistory ? 'Yes' : 'No'}
Pregnancy: ${consultation_data?.pregnancy ? 'Yes' : 'No'}

NEUROMODULATOR PROCEDURE
------------------------
Product: ${neuromodulator_data?.productName || 'Not performed'}
Lot Number: ${neuromodulator_data?.lotNumber || '-'}
Total Units: ${neuromodulator_data?.totalUnitsDrawn || 0}
Glabella: ${neuromodulator_data?.unitsPerArea?.glabella || 0} units
Forehead: ${neuromodulator_data?.unitsPerArea?.forehead || 0} units
Crow's feet: ${neuromodulator_data?.unitsPerArea?.crowsFeet || 0} units
Masseter: ${neuromodulator_data?.unitsPerArea?.masseter || 0} units

DERMAL FILLER PROCEDURE
-----------------------
Product: ${dermalFiller_data?.productName || 'Not performed'}
Volume Used: ${dermalFiller_data?.totalVolumeUsed || 0} mL
Areas: ${(dermalFiller_data?.injectionAreas || []).join(', ') || 'None'}
Complications: ${dermalFiller_data?.complications ? 'Yes' : 'No'}

LASER PROCEDURE
---------------
Device: ${laser_data?.deviceName || 'Not performed'}
Wavelength: ${laser_data?.wavelength || '-'}
Fluence: ${laser_data?.fluence || '-'}
Test Patch: ${laser_data?.testPatch ? 'Performed' : 'Not performed'}

POST-CARE INSTRUCTIONS
----------------------
${consultation.postTreatmentInstructions || 'Standard post-care applies.'}

PHOTOS UPLOADED
---------------
Baseline (Before): ${this.tabPhotos().neuromodulator.filter(x => x.phase === 'Before').length + this.tabPhotos().dermalFiller.filter(x => x.phase === 'Before').length + this.tabPhotos().laser.filter(x => x.phase === 'Before').length} photos
Follow-up (After): ${this.tabPhotos().neuromodulator.filter(x => x.phase === 'After').length + this.tabPhotos().dermalFiller.filter(x => x.phase === 'After').length + this.tabPhotos().laser.filter(x => x.phase === 'After').length} photos
=============================
`;
    this.generatedProcedureNote.set(note);
  }

  private hasHardStopAllergies(): boolean {
    const allergies = this.patientAllergies();
    // If critical allergies present and patient is undergoing procedure, hard-stop
    if (allergies.includes('lidocaine') && (this.neuromodulatorGroup.get('productName')?.value || this.dermalFillerGroup.get('productName')?.value)) {
      return true; // Procedure requires anesthesia
    }
    return false;
  }

  private loadPatients(): void {
    this.loadingIndicator.set(true);
    this.alertService.startLoadingMessage('Loading patients...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients.set(patients || []);
        this.loadingIndicator.set(false);
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator.set(false);
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load error', 'Unable to load patients.', MessageSeverity.error, error);
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

    const consultationData = this.tryParseJson<{
      chiefComplaint?: string;
      duration?: string;
      expectationRealistic?: boolean;
      medicalConditions?: { diabetes?: boolean; hypertension?: boolean; keloid?: boolean; autoimmune?: boolean; bleedingDisorder?: boolean };
      medications?: string[];
      allergySelections?: string[];
      allergyNotes?: string;
      hsvHistory?: boolean;
      pregnancy?: boolean;
      fitzpatrickSkinType?: number;
      acneSeverity?: string;
      pigmentation?: string;
      scarring?: string;
      volumeLoss?: string;
    }>(consultation.treatmentPlan);

    const neuromodulatorData = this.tryParseJson<{
      productName?: string;
      lotNumber?: string;
      expiryDate?: string;
      dilution?: number | string;
      totalUnitsDrawn?: number;
      unitsPerArea?: { glabella?: number; forehead?: number; crowsFeet?: number; masseter?: number };
      needleType?: string;
      injectionTechnique?: string;
      complications?: boolean;
    }>(consultation.injectionMapping);

    const dermalFillerData = this.tryParseJson<{
      productName?: string;
      volumePerSyringe?: number;
      totalVolumeUsed?: number;
      injectionAreas?: string[];
      plane?: string;
      cannulaOrNeedle?: string;
      aspirationPerformed?: boolean;
      immediateOutcome?: string;
      complications?: boolean;
    }>(consultation.risksAndComplications);

    const laserData = this.tryParseJson<{
      deviceName?: string;
      wavelength?: string;
      fluence?: string;
      pulseDuration?: string;
      spotSize?: string;
      endpoint?: string;
      testPatch?: boolean;
      complications?: boolean;
    }>(consultation.deviceSettings);

    this.form.controls.consultation.patchValue({
      chiefComplaint: consultation.procedureDescription || consultationData?.chiefComplaint || '',
      duration: consultationData?.duration || '',
      expectationRealistic: consultationData?.expectationRealistic ?? true,
      medicalConditions: consultationData?.medicalConditions || {},
      medications: consultationData?.medications || [],
      allergySelections: consultationData?.allergySelections || [],
      allergyNotes: consultationData?.allergyNotes || '',
      hsvHistory: consultationData?.hsvHistory ?? false,
      pregnancy: consultationData?.pregnancy ?? false,
      fitzpatrickSkinType: consultationData?.fitzpatrickSkinType ?? 1,
      acneSeverity: consultationData?.acneSeverity || 'mild',
      pigmentation: consultationData?.pigmentation || 'none',
      scarring: consultationData?.scarring || '',
      volumeLoss: consultationData?.volumeLoss || 'mild'
    });

    this.form.controls.neuromodulator.patchValue({
      productName: neuromodulatorData?.productName || 'Botox',
      lotNumber: consultation.lotNumber || neuromodulatorData?.lotNumber || '',
      expiryDate: neuromodulatorData?.expiryDate || '',
      dilution: Number(neuromodulatorData?.dilution ?? consultation.dilution ?? 0),
      totalUnitsDrawn: Number(neuromodulatorData?.totalUnitsDrawn ?? 0),
      unitsPerArea: neuromodulatorData?.unitsPerArea || { glabella: 0, forehead: 0, crowsFeet: 0, masseter: 0 },
      needleType: neuromodulatorData?.needleType || '',
      injectionTechnique: neuromodulatorData?.injectionTechnique || '',
      complications: neuromodulatorData?.complications ?? false,
      postCareInstructions: consultation.postTreatmentInstructions || this.generatedPostCare()
    });

    this.form.controls.dermalFiller.patchValue({
      productName: dermalFillerData?.productName || '',
      volumePerSyringe: Number(dermalFillerData?.volumePerSyringe ?? 0),
      totalVolumeUsed: Number(dermalFillerData?.totalVolumeUsed ?? 0),
      injectionAreas: dermalFillerData?.injectionAreas || [],
      plane: dermalFillerData?.plane || 'subdermal',
      cannulaOrNeedle: dermalFillerData?.cannulaOrNeedle || 'cannula',
      aspirationPerformed: dermalFillerData?.aspirationPerformed ?? false,
      immediateOutcome: dermalFillerData?.immediateOutcome || '',
      complications: dermalFillerData?.complications ?? false
    });

    this.form.controls.laser.patchValue({
      deviceName: consultation.deviceUsed || laserData?.deviceName || '',
      wavelength: consultation.wavelength || laserData?.wavelength || '',
      fluence: consultation.fluence || laserData?.fluence || '',
      pulseDuration: consultation.pulseDuration || laserData?.pulseDuration || '',
      spotSize: consultation.spotSize || laserData?.spotSize || '',
      endpoint: laserData?.endpoint || 'erythema',
      testPatch: laserData?.testPatch ?? false,
      complications: laserData?.complications ?? false
    });

    this.tabPhotos.set(this.mapPhotosByTab(consultation.photos || []));
  }

  private resetForm(): void {
    this.form.controls.consultation.reset({
      chiefComplaint: '', duration: '', expectationRealistic: true,
      medicalConditions: { diabetes: false, hypertension: false, keloid: false, autoimmune: false, bleedingDisorder: false },
      medications: [], allergySelections: [], allergyNotes: '', hsvHistory: false, pregnancy: false,
      fitzpatrickSkinType: 1, acneSeverity: 'mild', pigmentation: 'none', scarring: '', volumeLoss: 'mild'
    });
    this.form.controls.neuromodulator.reset({
      productName: 'Botox', lotNumber: '', expiryDate: '', dilution: 0, totalUnitsDrawn: 0,
      unitsPerArea: { glabella: 0, forehead: 0, crowsFeet: 0, masseter: 0 }, needleType: '', injectionTechnique: '', complications: false,
      postCareInstructions: this.generatedPostCare()
    });
    this.form.controls.dermalFiller.reset({
      productName: '', volumePerSyringe: 0, totalVolumeUsed: 0, injectionAreas: [], plane: 'subdermal', cannulaOrNeedle: 'cannula', aspirationPerformed: false, immediateOutcome: '', complications: false
    });
    this.form.controls.laser.reset({ deviceName: '', wavelength: '', fluence: '', pulseDuration: '', spotSize: '', endpoint: 'erythema', testPatch: false, complications: false });
    this.tabPhotos.set({ neuromodulator: [], dermalFiller: [], laser: [] });
  }

  private buildPayload(): object {
    const consultation = this.consultationGroup.getRawValue();
    const neuromodulator = this.neuromodulatorGroup.getRawValue();
    const dermalFiller = this.dermalFillerGroup.getRawValue();
    const laser = this.laserGroup.getRawValue();
    const services = this.form.controls.services.value || '';

    return {
      id: this.currentConsultationId() ?? 0,
      patientId: this.form.controls.patientId.value,
      consultationDate: this.consultationDateIso(),
      procedureType: this.selectedConsentProcedureType().trim() || 'Procedures',
      provider: this.providerEmpId(),
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
      spotSize: laser.spotSize,
      Services: services,
      Allergies: consultation.allergyNotes || (consultation.allergySelections ? consultation.allergySelections.join(', ') : ''),
      CurrentMedications: consultation.medications ? consultation.medications.join(', ') : '',
      ConsultId: this.selectedVisitConsultId().trim(),
      PNo: this.selectedVisitPNo().trim(),
      Clinic: this.selectedClinic(),
      Remarks: ''
    };
  }

  private uploadPendingPhotos(consultationId: number): void {
    const items = [
      ...this.tabPhotos().neuromodulator.map(x => ({ tab: 'neuromodulator' as const, item: x })),
      ...this.tabPhotos().dermalFiller.map(x => ({ tab: 'dermalFiller' as const, item: x })),
      ...this.tabPhotos().laser.map(x => ({ tab: 'laser' as const, item: x }))
    ].filter(x => !!x.item.file);

    if (items.length === 0) {
      this.finishSaveSuccess();
      return;
    }

    let completed = 0;
    let failed = 0;

    for (const entry of items) {
      const formData = new FormData();
      formData.append('consultationId', String(consultationId));
      formData.append('type', `${entry.tab}|${entry.item.phase}|${entry.item.tag}`);
      formData.append('file', entry.item.file!, entry.item.fileName);

      this.endpoint.uploadPhotoEndpoint<AestheticPhoto>(formData).subscribe({
        next: () => {
          completed += 1;
          if (completed + failed === items.length) {
            this.finishSaveSuccess(failed);
          }
        },
        error: () => {
          failed += 1;
          if (completed + failed === items.length) {
            this.finishSaveSuccess(failed);
          }
        }
      });
    }
  }

  private finishSaveSuccess(failedUploads = 0): void {
    this.loadingIndicator.set(false);
    this.alertService.stopLoadingMessage();
    const message = failedUploads > 0
      ? `Procedures saved. ${failedUploads} photo upload(s) failed.`
      : 'Procedures saved successfully.';

    this.alertService.showMessage('Success', message, failedUploads > 0 ? MessageSeverity.warn : MessageSeverity.success);
    this.dialogRef.close(true);
  }

  private hasMandatoryBaselines(): boolean {
    const photos = this.tabPhotos();
    return (['neuromodulator', 'dermalFiller', 'laser'] as PhotoTab[])
      .every(tab => photos[tab].some(x => x.phase === 'Before'));
  }

  private mapPhotosByTab(photos: AestheticPhoto[]): TabPhotoCollection {
    const mapped: TabPhotoCollection = {
      neuromodulator: [],
      dermalFiller: [],
      laser: []
    };

    for (const photo of photos) {
      const [tabRaw, phaseRaw, tagRaw] = (photo.type || '').split('|');
      if (!tabRaw || !phaseRaw || !tagRaw) continue;

      const tab = this.toPhotoTab(tabRaw);
      if (!tab) continue;

      mapped[tab].push({
        id: photo.id,
        consultationId: photo.consultationId,
        fileName: photo.fileName || 'photo',
        phase: phaseRaw === 'After' ? 'After' : 'Before',
        tag: this.toStandardTag(tagRaw),
        url: photo.url
      });
    }

    return mapped;
  }

  private toPhotoTab(raw: string): PhotoTab | null {
    if (raw === 'neuromodulator' || raw === 'dermalFiller' || raw === 'laser') {
      return raw;
    }

    return null;
  }

  private toStandardTag(raw: string): StandardTag {
    const normalized = raw.toLowerCase();
    if (normalized === 'left' || normalized === 'right' || normalized === 'profile') {
      return normalized;
    }

    return 'frontal';
  }

  private mapTabToIndex(tab: string): number {
    const normalized = tab.toLowerCase();
    if (normalized === 'neuromodulator' || normalized === 'botox') return 2;
    if (normalized === 'dermalfiller') return 3;
    if (normalized === 'laser') return 4;
    if (normalized === 'consultation') return 1;
    return 0;
  }

  private tryParseJson<T>(value?: string): T | null {
    if (!value) return null;
    try {
      return JSON.parse(value) as T;
    } catch {
      return null;
    }
  }

  private formatAttendanceDate(value?: string): string {
    const date = this.parseDate(value);
    if (!date) {
      return '';
    }

    const day = `${date.getDate()}`.padStart(2, '0');
    const month = date.toLocaleString('en', { month: 'short' });
    return `${day}-${month}-${date.getFullYear()}`;
  }

  private toLocalDateKey(value?: string | Date): string {
    const date = this.parseDate(value);
    if (!date) {
      return '';
    }

    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private parseDate(value?: string | Date): Date | null {
    if (!value) {
      return null;
    }

    if (value instanceof Date) {
      return Number.isNaN(value.getTime()) ? null : value;
    }

    const raw = `${value}`.trim();
    if (!raw) {
      return null;
    }

    const direct = new Date(raw);
    if (!Number.isNaN(direct.getTime())) {
      return direct;
    }

    const datePart = raw.split('T')[0].split(' ')[0].trim();

    const ymdMatch = datePart.match(/^(\d{4})[-/](\d{1,2})[-/](\d{1,2})$/);
    if (ymdMatch) {
      const year = Number(ymdMatch[1]);
      const month = Number(ymdMatch[2]);
      const day = Number(ymdMatch[3]);
      const parsed = new Date(year, month - 1, day);
      return Number.isNaN(parsed.getTime()) ? null : parsed;
    }

    const dmyMatch = datePart.match(/^(\d{1,2})[-/](\d{1,2})[-/](\d{2,4})$/);
    if (dmyMatch) {
      const day = Number(dmyMatch[1]);
      const month = Number(dmyMatch[2]);
      const year = Number(dmyMatch[3].length === 2 ? `20${dmyMatch[3]}` : dmyMatch[3]);
      const parsed = new Date(year, month - 1, day);
      return Number.isNaN(parsed.getTime()) ? null : parsed;
    }

    const ddMmmYyyyMatch = datePart.match(/^(\d{1,2})-([A-Za-z]{3})-(\d{2,4})$/);
    if (ddMmmYyyyMatch) {
      const day = Number(ddMmmYyyyMatch[1]);
      const monthToken = ddMmmYyyyMatch[2].toLowerCase();
      const year = Number(ddMmmYyyyMatch[3].length === 2 ? `20${ddMmmYyyyMatch[3]}` : ddMmmYyyyMatch[3]);
      const monthLookup: Record<string, number> = {
        jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5,
        jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11
      };

      const month = monthLookup[monthToken];
      if (month !== undefined) {
        const parsed = new Date(year, month, day);
        return Number.isNaN(parsed.getTime()) ? null : parsed;
      }
    }

    return null;
  }

  private findPatientByAttendancePno(patients: AestheticPatient[], attendancePno?: string): AestheticPatient | null {
    const normalizedAttendancePno = this.normalizePno(attendancePno);
    if (!normalizedAttendancePno) {
      return null;
    }

    return patients.find(patient => {
      const patientPno = this.normalizePno(patient.pno);
      if (!patientPno) {
        return false;
      }

      if (patientPno === normalizedAttendancePno) {
        return true;
      }

      const attendanceTail = normalizedAttendancePno.split('-').pop() ?? normalizedAttendancePno;
      const patientTail = patientPno.split('-').pop() ?? patientPno;

      return attendanceTail === patientTail;
    }) ?? null;
  }

  private normalizePno(value?: string): string {
    return (value ?? '').trim().toLowerCase();
  }

  private resolveAttendancePatientName(attendance: { pNo?: string; fullname?: string }, aestheticPatient: AestheticPatient | null): string {
    if (aestheticPatient) {
      const name = `${aestheticPatient.firstName ?? ''} ${aestheticPatient.lastName ?? ''}`.trim();
      if (name) {
        return name;
      }
    }

    const fullName = (attendance.fullname || '').trim();
    if (fullName) {
      return fullName;
    }

    const attendancePno = this.normalizePno(attendance.pNo);
    if (attendancePno) {
      const legacyPatient = this.legacyPatients().find(p => this.normalizePno(p.pno) === attendancePno);
      if (legacyPatient) {
        const legacyName = `${legacyPatient.pSurName ?? ''} ${legacyPatient.pFirstname ?? ''}`.trim();
        if (legacyName) {
          return legacyName;
        }
      }
    }

    return attendance.pNo || 'Patient';
  }

  resolveConsentPatientName(pNo?: string): string {
    const normalized = (pNo ?? '').trim().toLowerCase();
    if (!normalized) {
      return 'Unknown patient';
    }

    const patient = this.legacyPatients().find(p => (p.pno ?? '').trim().toLowerCase() === normalized);
    if (!patient) {
      return pNo ?? 'Unknown patient';
    }

    return [patient.pSurName, patient.pFirstname].filter(Boolean).join(' ').trim() || (pNo ?? 'Unknown patient');
  }
}
















































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































