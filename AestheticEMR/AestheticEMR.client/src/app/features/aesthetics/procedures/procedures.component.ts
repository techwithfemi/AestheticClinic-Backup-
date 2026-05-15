import { Component, OnInit, computed, inject, signal, effect } from '@angular/core';
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
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AestheticConsultation, AestheticPatient, AestheticPhoto } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';

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

@Component({
  selector: 'app-procedures',
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
    MatProgressBarModule
  ],
  template: `
    <div class="procedures-page">
      <div class="page-header">
        <div style="display: flex; justify-content: space-between; align-items: center;">
          <div>
            <h2>Aesthetic Procedures</h2>
            <p class="subtitle">Unified consultation and procedure tabs with integrated safety checks.</p>
          </div>
          <button mat-raised-button color="warn" (click)="openComplicationReport()" [matTooltip]="'Report complications, adverse events, or safety concerns'">
            <mat-icon>error_outline</mat-icon>
            Report Complication
          </button>
        </div>
      </div>

      <!-- Hard-Stop Safety Alerts -->
      @for (alert of safetyAlerts(); track alert.title) {
        @switch (alert.type) {
          @case ('hard-stop') {
            <div class="alert alert-danger hard-stop-alert">
              <mat-icon>do_not_disturb</mat-icon>
              <div class="alert-content">
                <strong>⚠️ HARD STOP: {{ alert.title }}</strong>
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
                <strong>🚫 ALLERGY DETECTED: {{ alert.title }}</strong>
                <p>{{ alert.message }}</p>
              </div>
            </div>
          }
          @case ('duplicate') {
            <div class="alert alert-warning duplicate-alert">
              <mat-icon>warning</mat-icon>
              <div class="alert-content">
                <strong>⚠️ DUPLICATE TREATMENT: {{ alert.title }}</strong>
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
                <strong>ℹ️ {{ alert.title }}</strong>
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
          <div class="patient-row">
            <mat-form-field appearance="outline" class="patient-field">
              <mat-label>Patient</mat-label>
              <mat-select formControlName="patientId" (selectionChange)="onPatientChanged()">
                <mat-option [value]="0">Select Patient</mat-option>
                @for (item of patientAttendanceOptions(); track item.trackKey) {
                  <mat-option [value]="item.patientId" [disabled]="item.disabled">{{ item.label }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>

          <mat-tab-group [selectedIndex]="selectedTabIndex()">
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
                  @for (item of tabPhotos().neuromodulator; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    @for (img of getComparisonImages('neuromodulator').before; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="before" />
                    }
                    @for (img of getComparisonImages('neuromodulator').after; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="after" />
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
                  @for (item of tabPhotos().dermalFiller; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    @for (img of getComparisonImages('dermalFiller').before; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="before" />
                    }
                    @for (img of getComparisonImages('dermalFiller').after; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="after" />
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
                <mat-form-field appearance="outline" class="half"><mat-label>Endpoint</mat-label><mat-select formControlName="endpoint"><mat-option value="erythema">Erythema</mat-option><mat-option value="edema">Edema</mat-option></mat-select></mat-form-field>
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
                  @for (item of tabPhotos().laser; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    @for (img of getComparisonImages('laser').before; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="before" />
                    }
                    @for (img of getComparisonImages('laser').after; track img.id ?? img.fileName) {
                      <img [src]="img.url || ''" alt="after" />
                    }
                  </div>
                </div>
              </div>
            </mat-tab>
          </mat-tab-group>

          <div class="save-row">
            <button mat-raised-button color="primary" type="button" (click)="saveOrUpdate()" [disabled]="loadingIndicator">
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
                  <li>Patient education: takes 2–4 weeks for partial resolution</li>
                  <li>Avoid strong brow movements for 7 days</li>
                  <li>Consider apraclonidine 0.5% eye drops (if approved)</li>
                  <li>Follow-up in 2–4 weeks for potential touch-up</li>
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
    </div>
  `,
  styles: [`
    .procedures-page { padding: 20px; }
    .page-header { margin-bottom: 16px; display: flex; justify-content: space-between; align-items: flex-start; }
    .page-header > div { flex: 1; }
    .subtitle { color: #666; margin: 4px 0 0; }

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
    .patient-row { margin-bottom: 8px; }
    .patient-field { width: min(460px, 100%); }
    .tab-body { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; padding: 14px 2px 2px; }
    .full { grid-column: 1 / -1; }
    .half { grid-column: span 1; }
    .toggle-row { display: flex; align-items: center; justify-content: space-between; padding: 8px 4px; }
    .checkbox-grid .checks { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 8px; }
    .block-title { font-weight: 600; margin-bottom: 8px; display: inline-block; }
    .grid-2 { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    .photo-toolbar { display: grid; grid-template-columns: 170px 170px 1fr; gap: 10px; align-items: center; margin-bottom: 8px; }
    .photo-item { font-size: .9rem; padding: 4px 0; color: #444; }
    .compare-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 220px)); gap: 10px; margin-top: 8px; }
    .compare-grid img { width: 100%; height: 160px; object-fit: cover; border: 1px solid #ddd; border-radius: 6px; }
    .action-row { margin-top: 4px; }
    .save-row { display: flex; justify-content: flex-end; margin-top: 16px; }

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
    }

    @media (max-width: 767.98px) {
      .procedures-page { padding: 12px; }
      .page-header { flex-direction: column; gap: 10px; }
      .page-header > div { width: 100%; }
      .save-row { justify-content: stretch; }
      .save-row button { width: 100%; min-height: 44px; }
      .toggle-row { padding: 6px 2px; }
      .compare-grid { grid-template-columns: 1fr; }
      .compare-grid img { height: auto; min-height: 160px; }
      .emergency-content { flex-direction: column; align-items: flex-start; }
      .emergency-content button { width: 100%; }
    }

    @media (max-width: 575.98px) {
      .procedures-page { padding: 10px; }
      .form-shell { padding: 8px; }
      .tab-body { gap: 8px; }
      .photo-item { font-size: .85rem; }
    }
  `]
})
export class ProceduresComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly hPatientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
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
  readonly attendanceRecords = signal<Attendance[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);

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
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  // Workflow state signals
  readonly isPregnant = signal(false);
  readonly hasHsvHistory = signal(false);
  readonly isFillerSelected = signal(false);

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
      lotNumber: ['', Validators.required],
      expiryDate: ['', Validators.required],
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
    })
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
        this.selectedTabIndex.set(0);
      }
    });

    effect(() => {
      const fillerProduct = this.dermalFillerGroup.get('productName')?.value;
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

  private loadAttendances(): void {
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().subscribe({
      next: attendances => {
        this.attendanceRecords.set(attendances || []);
      },
      error: error => {
        this.alertService.showStickyMessage('Load error', 'Unable to load attendance records.', MessageSeverity.error, error);
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
      this.selectedTabIndex.set(0);
    }
  }

  onHsvHistoryChange(): void {
    const hasHsv = this.consultationGroup.get('hsvHistory')?.value ?? false;
    if (hasHsv) {
      this.alertService.showStickyMessage(
        'HSV History Detected',
        'Antiviral prophylaxis is recommended. Start acyclovir (or valacyclovir) 1–2 days before treatment to reduce outbreak risk.',
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

    // Load recent treatments for duplicate detection
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
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation error', 'Please complete required fields.', MessageSeverity.warn);
      return;
    }

    if (!this.hasMandatoryBaselines()) {
      this.alertService.showStickyMessage('Baseline photos required', 'Baseline (Before) photos are mandatory for Neuromodulator, Dermal Filler, and Laser tabs.', MessageSeverity.warn);
      return;
    }

    // Hard-stop check: allergy incompatibilities
    if (this.hasHardStopAllergies()) {
      this.alertService.showStickyMessage(
        '🚫 HARD STOP: Incompatible Allergy',
        'This patient has documented allergies that are contraindicated for the selected procedures. Clarify with patient or select alternative procedures.',
        MessageSeverity.error
      );
      return;
    }

    const payload = this.buildPayload();
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.currentConsultationId() ? 'Updating procedures...' : 'Saving procedures...' );

    const consultationRequest = this.currentConsultationId()
      ? this.endpoint.updateConsultationEndpoint<AestheticConsultation>(this.currentConsultationId()!, payload)
      : this.endpoint.createConsultationEndpoint<AestheticConsultation>(payload);

    consultationRequest.subscribe({
      next: consultation => {
        this.currentConsultationId.set(consultation.id);
        this.generateProcedureNote(consultation);
        this.uploadPendingPhotos(consultation.id);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save error', 'Unable to save procedures.', MessageSeverity.error, error);
      }
    });
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
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading patients...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients.set(patients || []);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
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

    return {
      id: this.currentConsultationId() ?? 0,
      patientId: this.form.controls.patientId.value,
      consultationDate: new Date().toISOString(),
      procedureType: 'Procedures',
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
    this.loadingIndicator = false;
    this.alertService.stopLoadingMessage();
    const message = failedUploads > 0
      ? `Procedures saved. ${failedUploads} photo upload(s) failed.`
      : 'Procedures saved successfully.';

    this.alertService.showMessage('Success', message, failedUploads > 0 ? MessageSeverity.warn : MessageSeverity.success);

    const patientId = this.form.controls.patientId.value;
    this.loadPatients();
    this.form.controls.patientId.setValue(patientId);
    this.onPatientChanged();
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
    if (normalized === 'neuromodulator' || normalized === 'botox') return 1;
    if (normalized === 'dermalfiller') return 2;
    if (normalized === 'laser') return 3;
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

    const date = value instanceof Date ? value : new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
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

  private resolveAttendancePatientName(attendance: Attendance, aestheticPatient: AestheticPatient | null): string {
    if (aestheticPatient) {
      const name = `${aestheticPatient.firstName ?? ''} ${aestheticPatient.lastName ?? ''}`.trim();
      if (name) {
        return name;
      }
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
}


