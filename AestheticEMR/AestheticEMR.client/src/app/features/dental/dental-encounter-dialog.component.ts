import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatIconModule } from '@angular/material/icon';

import { DentalChart, DentalConsulting, DentalEncounter, DentalImaging, ToothStatus } from '../../models/dental.model';
import { DentalEndpoint } from '../../services/dental-endpoint.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

export interface DentalPatientOption {
  pNo: string;
  consultId: string;
  clientCat?: string;
  label: string;
  fullName?: string;
  attendDate?: string;
  photo?: string;
  dateOfBirth?: string;
  companyName?: string;
  coyId?: string;
  clinic?: string;
}

export interface DentalEncounterDialogData {
  initialTabIndex: number;
  patientOptions: DentalPatientOption[];
  encounter?: DentalEncounter;
}

@Component({
  selector: 'app-dental-encounter-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatIconModule
  ],
  template: `
    <div class="dialog-header">
      <h2 mat-dialog-title>{{ isEdit ? 'Edit Dental Info' : 'Add Dental Info' }}</h2>
      <button mat-icon-button type="button" class="close-btn" (click)="dialogRef.close()" aria-label="Close dialog">
        ×
      </button>
    </div>

    <mat-dialog-content>
      <div class="patient-header">
        @if (selectedPatientInfo?.photo) {
          <img class="patient-photo" [src]="getPatientPhotoSource(selectedPatientInfo?.photo)" alt="Patient photo" />
        } @else {
          <div class="patient-photo placeholder">
            <mat-icon>person</mat-icon>
          </div>
        }

        <div class="patient-meta">
          <div class="meta-item"><span class="label">Patient:</span> <span>{{ selectedPatientInfo?.fullName || '—' }}</span></div>
          <div class="meta-item"><span class="label">Age:</span> <span>{{ selectedPatientAge ?? '—' }}</span></div>
          <div class="meta-item"><span class="label">Company:</span> <span>{{ selectedPatientInfo?.companyName || '—' }}</span></div>
          <div class="meta-item"><span class="label">ConsultID:</span> <span>{{ selectedPatientInfo?.consultId || '—' }}</span></div>
          <div class="meta-item"><span class="label">Clinic:</span> <span>{{ selectedPatientInfo?.clinic || '—' }}</span></div>
        </div>
      </div>

      <mat-tab-group [(selectedIndex)]="selectedTabIndex">

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>assignment</mat-icon>
            <span>History Taking</span>
          </ng-template>
          <div class="tab-body">
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Select patient</mat-label>
              <mat-select [(ngModel)]="selectedPatientKey" (selectionChange)="onPatientSelected()">
                <mat-option value="">Select patient</mat-option>
                @for (p of data.patientOptions; track p.label) {
                  <mat-option [value]="p.label">{{ p.label }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Complaints</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.complaints"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>History of Presenting Complaints</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.hpc"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Past Medical History</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.pmh"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Drug History</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.drugHx"></textarea></mat-form-field>
          </div>
        </mat-tab>

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>medical_services</mat-icon>
            <span>Treatment</span>
          </ng-template>
          <div class="tab-body">
            <div class="span-2 section-title">Dental Health Status (FDI)</div>

            <div class="span-2 fdi-matrix-wrap">
              @for (status of statusRows; track status.key) {
                <div class="fdi-category">
                  <div class="fdi-row-title">{{ status.label }}</div>

                  <div class="fdi-grid-row">
                    @for (tooth of fdiTopRow; track tooth) {
                      <div class="fdi-cell">
                        <span class="fdi-tooth">{{ tooth }}</span>
                        <mat-checkbox
                          [checked]="isStatusChecked(tooth, status.key)"
                          (change)="onStatusToggle(tooth, status.key, $event.checked)"></mat-checkbox>
                      </div>
                    }
                  </div>

                  <div class="fdi-grid-row">
                    @for (tooth of fdiBottomRow; track tooth) {
                      <div class="fdi-cell">
                        <span class="fdi-tooth">{{ tooth }}</span>
                        <mat-checkbox
                          [checked]="isStatusChecked(tooth, status.key)"
                          (change)="onStatusToggle(tooth, status.key, $event.checked)"></mat-checkbox>
                      </div>
                    }
                  </div>
                </div>
              }
            </div>

            <mat-form-field appearance="outline" class="span-2"><mat-label>Adult Remarks</mat-label><textarea matInput rows="2" [(ngModel)]="chart.aRem"></textarea></mat-form-field>

            <div class="span-2 section-title treatment-bottom">Oral Examination & Recommendations</div>
            <div class="span-2 oral-exam-grid">
              <mat-checkbox [(ngModel)]="chart.oralExam!.caries">Caries</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.poorOralHygiene">Poor Oral Hygiene</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.indicatedForRestorationFilling">Indicated for Restoration/Filling</mat-checkbox>

              <div class="filling-types">
                <span class="small-label">Filling Type:</span>
                <mat-checkbox [(ngModel)]="chart.oralExam!.fillingGic">GIC</mat-checkbox>
                <mat-checkbox [(ngModel)]="chart.oralExam!.fillingComposite">Composite</mat-checkbox>
                <mat-checkbox [(ngModel)]="chart.oralExam!.fissureSealant">Fissure Sealant</mat-checkbox>
              </div>

              <mat-checkbox [(ngModel)]="chart.oralExam!.indicatedForExtraction">Indicated for Extraction</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.gingivalInflammation">Gingival Inflammation</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.needsOralProphylaxis">Needs Oral Prophylaxis</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.needsProsthesisDenture">Needs Prosthesis/Denture</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.forEndodonticTreatment">For Endodontic Treatment</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.forOrthodonticConsultation">For Orthodontic Consultation</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.oralExam!.noDentalTreatmentNeededAtPresent">No Dental Treatment Needed At Present</mat-checkbox>
            </div>

            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Others</mat-label>
              <input matInput [(ngModel)]="chart.oralExam!.others" placeholder="Other oral examination findings/recommendations" />
            </mat-form-field>

            <div class="span-2 section-title treatment-bottom">Clinical Examination</div>

            <mat-form-field appearance="outline"><mat-label>Treatment Date</mat-label><input matInput type="date" [(ngModel)]="chartDate" /></mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Treatment Type</mat-label>
              <mat-select [(ngModel)]="chart.dtype">
                <mat-option value="Teeth Present">Teeth Present</mat-option>
                <mat-option value="Carious Teeth">Carious Teeth</mat-option>
                <mat-option value="Decayed Teeth">Decayed Teeth</mat-option>
                <mat-option value="Missing Teeth">Missing Teeth</mat-option>
                <mat-option value="Filled Teeth">Filled Teeth</mat-option>
              </mat-select>
            </mat-form-field>

            <div class="span-2 radio-row">
              <div class="radio-group">
                <span class="radio-label">Inflammation of Gingiva</span>
                <mat-radio-group [(ngModel)]="chart.inflammationOfGingiva">
                  <mat-radio-button value="Yes">Yes</mat-radio-button>
                  <mat-radio-button value="No">No</mat-radio-button>
                </mat-radio-group>
              </div>
              <div class="radio-group">
                <span class="radio-label">Presence of Debris</span>
                <mat-radio-group [(ngModel)]="chart.presenceOfDebris">
                  <mat-radio-button value="Yes">Yes</mat-radio-button>
                  <mat-radio-button value="No">No</mat-radio-button>
                </mat-radio-group>
              </div>
              <div class="radio-group">
                <span class="radio-label">Presence of Calculus</span>
                <mat-radio-group [(ngModel)]="chart.presenceOfCalculus">
                  <mat-radio-button value="Yes">Yes</mat-radio-button>
                  <mat-radio-button value="No">No</mat-radio-button>
                </mat-radio-group>
              </div>
              <div class="radio-group">
                <span class="radio-label">Presence of Stains</span>
                <mat-radio-group [(ngModel)]="chart.presenceOfStains">
                  <mat-radio-button value="Yes">Yes</mat-radio-button>
                  <mat-radio-button value="No">No</mat-radio-button>
                </mat-radio-group>
              </div>
              <div class="radio-group">
                <span class="radio-label">Under Orthodontic Treatment</span>
                <mat-radio-group [(ngModel)]="chart.underOrthodonticTreatment">
                  <mat-radio-button value="Yes">Yes</mat-radio-button>
                  <mat-radio-button value="No">No</mat-radio-button>
                </mat-radio-group>
              </div>
            </div>

            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Other Clinical Findings</mat-label>
              <input matInput [(ngModel)]="chart.otherClinicalFindings" placeholder="Free text observation" />
            </mat-form-field>
          </div>
        </mat-tab>

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>straighten</mat-icon>
            <span>Orthodontics</span>
          </ng-template>
          <div class="tab-body">
            <div class="span-2 section-title">Orthodontic Assessment</div>

            <div class="span-2 ortho-grid">
              <mat-checkbox [(ngModel)]="chart.orthodontics!.classI">Class I</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.classII">Class II</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.classIII">Class III</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.crowdingUpper">Crowding (Upper)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.crowdingLower">Crowding (Lower)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.spacingUpper">Spacing (Upper)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.spacingLower">Spacing (Lower)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.crossbiteAnterior">Crossbite (Anterior)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.crossbitePosterior">Crossbite (Posterior)</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.overjetIncreased">Overjet Increased</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.overbiteDeep">Overbite Deep</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.openbite">Open Bite</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.midlineShift">Midline Shift</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.impactedTeeth">Impacted Teeth</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.tmjSymptoms">TMJ Symptoms</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.extractionRequired">Extraction Required</mat-checkbox>
            </div>

            <div class="span-2 section-title">Overjet / Overbite / Impaction</div>
            <mat-form-field appearance="outline">
              <mat-label>Overjet</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.overjet" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Overbite</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.overbite" />
            </mat-form-field>
            <div class="span-2 ortho-grid">
              <mat-checkbox [(ngModel)]="chart.orthodontics!.teethImpaction">Teeth Impaction</mat-checkbox>
            </div>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Teeth Impaction Details</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.teethImpactionDetails" />
            </mat-form-field>

            <div class="span-2 section-title">Molar / Canine Relation</div>
            <mat-form-field appearance="outline">
              <mat-label>Molar Relation (R)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.molarRelationRight" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Molar Relation (L)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.molarRelationLeft" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Canine Relation (R)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.canineRelationRight" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Canine Relation (L)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.canineRelationLeft" />
            </mat-form-field>

            <div class="span-2 section-title">Lips / Habits</div>
            <div class="span-2 ortho-grid">
              <mat-checkbox [(ngModel)]="chart.orthodontics!.lipsCompetent">Lips Competent</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.lipsIncompetent">Lips Incompetent</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.thumbSucking">Thumb Sucking</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.tongueThrusting">Tongue Thrusting</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.mouthBreathing">Mouth Breathing</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.nailBiting">Nail Biting</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.lipBiting">Lip Biting</mat-checkbox>
            </div>

            <div class="span-2 section-title">Skeletal Pattern</div>
            <mat-form-field appearance="outline">
              <mat-label>Antero-Posterior</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.skeletalPatternAnteroPosterior" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Vertical</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.skeletalPatternVertical" />
            </mat-form-field>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Transverse</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.skeletalPatternTransverse" />
            </mat-form-field>

            <div class="span-2 section-title">Arch / Occlusion</div>
            <mat-form-field appearance="outline">
              <mat-label>Arch Width (Upper)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.archWidthUpper" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Arch Width (Lower)</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.archWidthLower" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Curve of Spee</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.curveOfSpee" />
            </mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Dental Midline</mat-label>
              <input matInput [(ngModel)]="chart.orthodontics!.dentalMidline" />
            </mat-form-field>

            <div class="span-2 section-title">Rotations / Tooth Anomalies</div>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Rotations</mat-label>
              <textarea matInput rows="2" [(ngModel)]="chart.orthodontics!.rotations"></textarea>
            </mat-form-field>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Tooth Anomalies</mat-label>
              <textarea matInput rows="2" [(ngModel)]="chart.orthodontics!.toothAnomalies"></textarea>
            </mat-form-field>

            <div class="span-2 section-title">Summary / Investigations</div>
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Summary of Orthodontic Analysis</mat-label>
              <textarea matInput rows="3" [(ngModel)]="chart.orthodontics!.summaryOfOrthodonticAnalysis"></textarea>
            </mat-form-field>
            <div class="span-2 ortho-grid">
              <mat-checkbox [(ngModel)]="chart.orthodontics!.investigationOpg">Investigation: OPG</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.investigationCeph">Investigation: CEPH</mat-checkbox>
            </div>

            <div class="span-2 section-title">Records</div>
            <div class="span-2 ortho-grid">
              <mat-checkbox [(ngModel)]="chart.orthodontics!.clinicalStudyModel">Clinical Study Model</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.extraoralPhotographs">Extraoral Photographs</mat-checkbox>
              <mat-checkbox [(ngModel)]="chart.orthodontics!.intraoralPhotographs">Intraoral Photographs</mat-checkbox>
            </div>

            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Orthodontic Notes</mat-label>
              <textarea matInput rows="3" [(ngModel)]="chart.orthodontics!.notes"></textarea>
            </mat-form-field>
          </div>
        </mat-tab>

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>manage_accounts</mat-icon>
            <span>Management</span>
          </ng-template>
          <div class="tab-body">
            <mat-form-field appearance="outline" class="span-2"><mat-label>Investigations</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.investigate"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Diagnosis</mat-label><textarea matInput rows="3" [(ngModel)]="consulting.diagnosis"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Treatment Plan</mat-label><textarea matInput rows="3" [(ngModel)]="consulting.treatPlan"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Prescription</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.prescription"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Services</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.services"></textarea></mat-form-field>
          </div>
        </mat-tab>

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>image</mat-icon>
            <span>Imaging</span>
          </ng-template>
          <div class="tab-body">
            <div class="span-2 section-title">Imaging Write-up</div>

            <mat-form-field appearance="outline"><mat-label>Imaging Date</mat-label><input matInput type="date" [(ngModel)]="imagingDate" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Imaging Type</mat-label><input matInput [(ngModel)]="imaging.imagingType" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Tooth Region</mat-label><input matInput [(ngModel)]="imaging.toothRegion" /></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Findings</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.findings"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Impression</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.impression"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Recommendations</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.recommendations"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Notes</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.notes"></textarea></mat-form-field>

            <div class="span-2 section-title">Upload Dental Image</div>
            <div class="span-2 image-toolbar">
              <button mat-stroked-button type="button" (click)="fileInput.click()">Upload Dental Image</button>
              <input #fileInput type="file" accept="image/*" multiple (change)="onImageSelected(fileInput.files); fileInput.value = ''" style="display:none" />
              <span class="image-count" *ngIf="selectedImageFiles.length">{{ selectedImageFiles.length }} image(s) selected</span>
            </div>

            <div class="span-2 image-panel">
              @if (imagingPreviewUrls.length) {
                <div class="image-grid">
                  @for (img of imagingPreviewUrls; track img; let i = $index) {
                    <div class="image-item">
                      <img [src]="img" [alt]="'Dental image ' + (i + 1)" class="dental-image" />
                      <div class="image-item-actions">
                        <button mat-icon-button type="button" (click)="zoomImage(img)" title="Zoom image">
                          <mat-icon>zoom_in</mat-icon>
                        </button>
                        <button mat-icon-button type="button" (click)="removeImageAt(i)" title="Remove image">
                          <mat-icon>close</mat-icon>
                        </button>
                      </div>
                    </div>
                  }
                </div>
              } @else {
                <p class="image-empty">No uploaded dental image for this encounter.</p>
              }
            </div>

            @if (zoomImageUrl) {
              <div class="image-zoom-overlay" (click)="closeZoom()">
                <div class="image-zoom-dialog" (click)="$event.stopPropagation()">
                  <button mat-icon-button type="button" class="image-zoom-close" (click)="closeZoom()">
                    <mat-icon>close</mat-icon>
                  </button>
                  <img [src]="zoomImageUrl" alt="Dental image zoom" class="image-zoom-img" />
                </div>
              </div>
            }
          </div>
        </mat-tab>

      </mat-tab-group>
    </mat-dialog-content>

    <mat-dialog-actions align="end" class="sticky-actions">
      <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
      <button mat-raised-button color="primary" type="button" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content { width: min(1000px, 95vw); max-width: 95vw; overflow-x: hidden; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px; }
    .close-btn { font-size: 20px; font-weight: 700; line-height: 1; width: 34px; height: 34px; border-radius: 50%; }
    .top-row { margin-bottom: 10px; }
    .full { width: 100%; }
    .tab-body { padding-top: 12px; display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .span-2 { grid-column: span 2; }
    mat-form-field { width: 100%; }
    .sticky-actions {
      position: sticky;
      bottom: 0;
      z-index: 10;
      background: #0b1220;
      border-top: 1px solid #334155;
      margin: 0 -24px -24px;
      padding: 12px 24px;
    }

    .section-title { font-weight: 700; font-size: 0.9rem; color: #90caf9; text-transform: uppercase; letter-spacing: 0.04em; }
    .treatment-bottom { margin-top: 12px; }

    .patient-header {
      display: flex;
      gap: 12px;
      align-items: center;
      border: 1px solid #334155;
      border-radius: 8px;
      min-height: 88px;
      padding: 10px 12px;
      margin-bottom: 12px;
      background: rgba(15, 23, 42, 0.45);
    }
    .patient-photo {
      width: 64px;
      height: 64px;
      min-width: 64px;
      min-height: 64px;
      max-width: 64px;
      max-height: 64px;
      aspect-ratio: 1 / 1;
      border-radius: 50%;
      object-fit: cover;
      background: #111827;
      border: 1px solid #475569;
      display: flex;
      align-items: center;
      justify-content: center;
      color: #94a3b8;
      flex: 0 0 64px;
      overflow: hidden;
    }
    .patient-photo.placeholder .mat-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
    }
    .patient-meta {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 6px 12px;
      min-width: 0;
      flex: 1;
    }
    .meta-item {
      font-size: 0.88rem;
      color: #cbd5e1;
      display: flex;
      gap: 6px;
      min-width: 0;
    }
    .meta-item span:last-child {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    .meta-item .label {
      color: #94a3b8;
      min-width: 72px;
    }

    ::ng-deep .mat-mdc-tab .mdc-tab__text-label {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      color: #b8d7ff;
      font-weight: 600;
    }
    ::ng-deep .mat-mdc-tab.mdc-tab--active .mdc-tab__text-label {
      color: #4dd0e1;
    }
    ::ng-deep .mat-mdc-tab .mdc-tab__text-label .mat-icon {
      color: #7c4dff;
      font-size: 18px;
      width: 18px;
      height: 18px;
    }
    ::ng-deep .mat-mdc-tab.mdc-tab--active .mdc-tab__text-label .mat-icon {
      color: #00e5ff;
    }

    mat-checkbox {
      --mdc-checkbox-unselected-icon-color: #7ea5c7;
      --mdc-checkbox-selected-icon-color: #00bcd4;
      --mdc-checkbox-selected-hover-icon-color: #00acc1;
      --mdc-checkbox-selected-focus-icon-color: #00acc1;
      --mdc-checkbox-selected-checkmark-color: #ffffff;
    }

    mat-radio-button {
      --mdc-radio-unselected-icon-color: #7ea5c7;
      --mdc-radio-selected-icon-color: #7c4dff;
      --mdc-radio-selected-hover-icon-color: #651fff;
      --mdc-radio-selected-focus-icon-color: #651fff;
    }

    .radio-label { font-size: 0.82rem; font-weight: 600; color: #9fc3e7; }
    mat-radio-group { display: inline-flex; gap: 12px; }

    .image-toolbar { display: flex; gap: 12px; align-items: center; flex-wrap: wrap; }
    .image-select { min-width: 280px; }
    .image-actions { display: flex; gap: 10px; justify-content: flex-end; }
    .image-count { font-size: 0.85rem; color: #9fc3e7; }

    .image-panel { border: 1px dashed #4a5568; border-radius: 8px; padding: 16px; min-height: 220px; display: flex; flex-direction: column; align-items: center; justify-content: center; gap: 8px; }
    .image-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(140px, 1fr)); gap: 10px; width: 100%; }
    .image-item { position: relative; }
    .image-item-actions { position: absolute; top: 6px; right: 6px; display: flex; gap: 4px; }
    .image-item-actions button { width: 28px; height: 28px; background: rgba(0, 0, 0, 0.55); }
    .image-item-actions .mat-icon { color: #fff; font-size: 18px; width: 18px; height: 18px; }
    .dental-image { width: 100%; max-height: 180px; object-fit: cover; border-radius: 6px; border: 1px solid #374151; }

    .image-zoom-overlay {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.75);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 1200;
      padding: 24px;
    }
    .image-zoom-dialog {
      position: relative;
      max-width: min(95vw, 1100px);
      max-height: 92vh;
      background: #0b1220;
      border: 1px solid #334155;
      border-radius: 10px;
      padding: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .image-zoom-img {
      max-width: 100%;
      max-height: calc(92vh - 56px);
      border-radius: 6px;
      object-fit: contain;
    }
    .image-zoom-close {
      position: absolute;
      top: 6px;
      right: 6px;
      background: rgba(0, 0, 0, 0.45);
    }
    .image-zoom-close .mat-icon {
      color: #fff;
    }

    .chart-section-label { font-weight: 600; font-size: 0.85rem; color: #1565c0; margin: 6px 0; text-transform: uppercase; letter-spacing: 0.05em; border-bottom: 1px solid #e0e0e0; padding-bottom: 4px; }
    .quadrant-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; width: 100%; box-sizing: border-box; }
    .quadrant { background: #f5f5f5; border-radius: 6px; padding: 8px; box-sizing: border-box; min-width: 0; }
    .quad-label { font-size: 0.72rem; font-weight: 600; color: #555; margin-bottom: 8px; }
    .tooth-row { display: flex; flex-wrap: wrap; gap: 6px 4px; }
    .tooth { display: flex; flex-direction: column; align-items: center; width: 34px; overflow: hidden; }
    .tooth-label { font-size: 0.65rem; font-weight: 600; color: #444; line-height: 1; height: 14px; display: flex; align-items: center; justify-content: center; width: 100%; text-align: center; margin-bottom: 2px; white-space: nowrap; }
    .tooth-box { width: 24px; height: 24px; display: flex; align-items: center; justify-content: center; overflow: hidden; flex-shrink: 0; }
    .tooth-box mat-checkbox { --mdc-checkbox-state-layer-size: 24px; }
    .tooth-box mat-checkbox ::ng-deep .mdc-checkbox { padding: 0; width: 18px; height: 18px; }
    .tooth-box mat-checkbox ::ng-deep .mdc-checkbox__background { width: 18px; height: 18px; top: 0; left: 0; }

    .fdi-matrix-wrap { display: flex; flex-direction: column; gap: 14px; width: 100%; }
    .fdi-category { display: flex; flex-direction: column; gap: 6px; }
    .fdi-row-title { font-size: 0.8rem; font-weight: 700; color: #90caf9; text-transform: uppercase; }
    .fdi-grid-row {
      display: grid;
      grid-template-columns: repeat(16, minmax(0, 1fr));
      gap: 2px;
      width: 100%;
    }
    .fdi-cell {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: flex-start;
      min-width: 0;
      padding: 2px 0;
    }
    .fdi-tooth { font-size: 0.68rem; font-weight: 600; color: #111; line-height: 1; }
    .fdi-cell mat-checkbox { transform: scale(0.82); margin-top: -2px; }

    .oral-exam-grid {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 8px 16px;
      align-items: center;
    }
    .ortho-grid {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 8px 16px;
      align-items: center;
    }
    .filling-types {
      grid-column: span 2;
      display: flex;
      align-items: center;
      gap: 10px;
      flex-wrap: wrap;
      padding: 4px 0;
    }

    @media (max-width: 1200px) {
      .fdi-tooth { font-size: 0.62rem; }
      .fdi-cell mat-checkbox { transform: scale(0.74); }
      .fdi-grid-row { gap: 1px; }
    }

    @media (max-width: 992px) {
      mat-dialog-content { width: min(100%, 95vw); }
      .tab-body { grid-template-columns: 1fr; }
      .span-2 { grid-column: span 1; }
      .radio-row { gap: 10px; }
      .radio-group { min-width: 0; width: 100%; }
      .image-select { min-width: 0; width: 100%; }
      .image-actions { justify-content: stretch; }
      .image-actions button { flex: 1; }
      .patient-meta { grid-template-columns: 1fr; }
      .oral-exam-grid { grid-template-columns: 1fr; }
      .ortho-grid { grid-template-columns: 1fr; }
      .filling-types { grid-column: span 1; }
      .image-grid { grid-template-columns: repeat(auto-fill, minmax(120px, 1fr)); }
    }

    @media (max-width: 768px) {
      .dialog-header h2 { font-size: 1rem; }
      .patient-header { flex-direction: column; align-items: flex-start; }
      .patient-photo,
      .patient-photo.placeholder { width: 56px; height: 56px; min-width: 56px; min-height: 56px; flex-basis: 56px; }
      .meta-item { font-size: 0.84rem; }
      .meta-item .label { min-width: 64px; }
      .meta-item span:last-child { white-space: normal; word-break: break-word; }

      .fdi-row-title { font-size: 0.72rem; }
      .fdi-tooth { font-size: 0.56rem; }
      .fdi-cell mat-checkbox { transform: scale(0.62); margin-top: -4px; }
      .fdi-grid-row { gap: 0; }

      .image-toolbar { flex-direction: column; align-items: stretch; }
      .image-toolbar button { min-height: 40px; }
      .image-item-actions button { width: 32px; height: 32px; }
      .image-item-actions .mat-icon { font-size: 20px; width: 20px; height: 20px; }
      .image-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }

      ::ng-deep .mat-mdc-tab-header { overflow-x: auto; }
      ::ng-deep .mat-mdc-tab .mdc-tab__text-label { font-size: 0.82rem; }
      .image-actions { flex-direction: column; }
    }

    @media (max-width: 480px) {
      .fdi-tooth { font-size: 0.5rem; }
      .fdi-cell { padding: 0; }
      .fdi-cell mat-checkbox { transform: scale(0.54); margin-top: -5px; }
      .close-btn { width: 30px; height: 30px; }

      mat-dialog-actions { flex-direction: column; align-items: stretch; gap: 8px; }
      mat-dialog-actions button { width: 100%; min-height: 40px; }
      .sticky-actions {
        margin: 0 -16px -16px;
        padding: 10px 16px;
      }
    }

    .patient-header {
      display: flex;
      gap: 12px;
      align-items: center;
      border: 1px solid #334155;
      border-radius: 8px;
      min-height: 88px;
      padding: 10px 12px;
      margin-bottom: 12px;
      background: rgba(15, 23, 42, 0.45);
    }
    .patient-photo {
      width: 64px;
      height: 64px;
      min-width: 64px;
      min-height: 64px;
      max-width: 64px;
      max-height: 64px;
      aspect-ratio: 1 / 1;
      border-radius: 50%;
      object-fit: cover;
      background: #111827;
      border: 1px solid #475569;
      display: flex;
      align-items: center;
      justify-content: center;
      color: #94a3b8;
      flex: 0 0 64px;
      overflow: hidden;
    }
    .patient-photo.placeholder .mat-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
    }
    .patient-meta {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 6px 12px;
      min-width: 0;
      flex: 1;
    }
    .meta-item {
      font-size: 0.88rem;
      color: #cbd5e1;
      display: flex;
      gap: 6px;
      min-width: 0;
    }
    .meta-item span:last-child {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    .meta-item .label {
      color: #94a3b8;
      min-width: 72px;
    }
  `]
})
export class DentalEncounterDialogComponent {
  readonly dialogRef = inject(MatDialogRef<DentalEncounterDialogComponent>);
  readonly data = inject<DentalEncounterDialogData>(MAT_DIALOG_DATA);
  private readonly dentalEndpoint = inject(DentalEndpoint);
  private readonly alertService = inject(AlertService);

  readonly fdiTopRow = ['18', '17', '16', '15', '14', '13', '12', '11', '21', '22', '23', '24', '25', '26', '27', '28'] as const;
  readonly fdiBottomRow = ['48', '47', '46', '45', '44', '43', '42', '41', '31', '32', '33', '34', '35', '36', '37', '38'] as const;
  readonly fdiTeethOrder = [...this.fdiTopRow, ...this.fdiBottomRow] as const;
  readonly statusRows: { key: keyof ToothStatus; label: string }[] = [
    { key: 'present', label: 'Teeth Present' },
    { key: 'carious', label: 'Carious Teeth' },
    { key: 'decayed', label: 'Decayed Teeth' },
    { key: 'missing', label: 'Missing Teeth' },
    { key: 'filled', label: 'Filled Teeth' }
  ];

  private readonly legacyToothMap: Record<string, keyof DentalChart> = {
    '18': 'aurm3', '17': 'aurm2', '16': 'aurm1', '15': 'aurpm2', '14': 'aurpm1', '13': 'aurc', '12': 'auri2', '11': 'auri1',
    '21': 'auli1', '22': 'auli2', '23': 'aulc', '24': 'aulpm1', '25': 'aulpm2', '26': 'aulm1', '27': 'aulm2', '28': 'aulm3',
    '48': 'alrm3', '47': 'alrm2', '46': 'alrm1', '45': 'alrpm2', '44': 'alrpm1', '43': 'alrc', '42': 'alri2', '41': 'alri1',
    '31': 'alli1', '32': 'alli2', '33': 'allc', '34': 'allpm1', '35': 'allpm2', '36': 'allm1', '37': 'allm2', '38': 'allm3'
  };

  selectedTabIndex = this.data.initialTabIndex;
  selectedPatientKey = '';

  chart: DentalChart = { id: 0, pno: '', consultId: '', tDate: new Date().toISOString(), teethStatus: {}, oralExam: {}, orthodontics: {} };
  imaging: DentalImaging = { id: 0, pno: '', consultId: '', imagingDate: new Date().toISOString() };
  consulting: DentalConsulting = { id: 0, consultId: '', pNo: '', clientCat: 'PRIVATE' };

  chartDate = this.toDateInput(this.chart.tDate);
  imagingDate = this.toDateInput(this.imaging.imagingDate);

  imagingPreviewUrls: string[] = [];
  selectedImageFiles: File[] = [];
  zoomImageUrl = '';

  get isEdit(): boolean {
    return !!this.data.encounter;
  }

  get selectedPatientInfo(): DentalPatientOption | undefined {
    return this.data.patientOptions.find(x => x.label === this.selectedPatientKey);
  }

  get selectedPatientAge(): number | null {
    const dob = this.selectedPatientInfo?.dateOfBirth;
    if (!dob) return null;

    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) return null;

    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age >= 0 ? age : null;
  }

  getPatientPhotoSource(photo?: string): string {
    if (!photo) return '';
    return photo.startsWith('data:') ? photo : `data:image/jpeg;base64,${photo}`;
  }

  constructor() {
    if (this.data.encounter) {
      this.chart = {
        ...this.chart,
        ...this.data.encounter.chart,
        teethStatus: this.data.encounter.chart.teethStatus || {},
        oralExam: this.data.encounter.chart.oralExam || {},
        orthodontics: this.data.encounter.chart.orthodontics || {}
      };
      this.imaging = { ...this.imaging, ...this.data.encounter.imaging };
      this.consulting = { ...this.consulting, ...this.data.encounter.consulting };
      this.chartDate = this.toDateInput(this.chart.tDate);
      this.imagingDate = this.toDateInput(this.imaging.imagingDate);
      this.imagingPreviewUrls = this.imaging.filePath ? [this.imaging.filePath] : [];

      const key = this.data.patientOptions.find(p => p.pNo === this.chart.pno && p.consultId === this.chart.consultId)?.label;
      this.selectedPatientKey = key || '';
    }

    this.applyLegacyFlagsToStatusMap();
  }

  isStatusChecked(tooth: string, key: keyof ToothStatus): boolean {
    return !!this.chart.teethStatus?.[tooth]?.[key];
  }

  onStatusToggle(tooth: string, key: keyof ToothStatus, checked: boolean): void {
    const current = this.chart.teethStatus?.[tooth] || {};
    const next: ToothStatus = { ...current, [key]: checked };

    if (key === 'missing' && checked) {
      next.present = false;
    }

    if (key === 'present' && checked) {
      next.missing = false;
    }

    this.chart.teethStatus = {
      ...(this.chart.teethStatus || {}),
      [tooth]: next
    };
  }

  clearStatusRow(key: keyof ToothStatus): void {
    const status = this.chart.teethStatus || {};
    for (const tooth of this.fdiTeethOrder) {
      if (!status[tooth]) continue;
      status[tooth] = { ...status[tooth], [key]: false };
    }
    this.chart.teethStatus = { ...status };
  }

  private applyLegacyFlagsToStatusMap(): void {
    const status = { ...(this.chart.teethStatus || {}) } as Record<string, ToothStatus>;
    const dtype = (this.chart.dtype || '').toLowerCase();

    const dtypeKey: keyof ToothStatus | null =
      dtype === 'teeth present' ? 'present'
        : dtype === 'carious teeth' ? 'carious'
          : dtype === 'decayed teeth' ? 'decayed'
            : dtype === 'missing teeth' ? 'missing'
              : dtype === 'filled teeth' ? 'filled'
                : null;

    if (!dtypeKey) {
      this.chart.teethStatus = status;
      return;
    }

    for (const tooth of this.fdiTeethOrder) {
      const legacyField = this.legacyToothMap[tooth];
      const isMarked = !!this.chart[legacyField];
      if (!status[tooth]) status[tooth] = {};
      status[tooth][dtypeKey] = isMarked;

      if (dtypeKey === 'missing' && isMarked) {
        status[tooth].present = false;
      }
      if (dtypeKey === 'present' && isMarked) {
        status[tooth].missing = false;
      }
    }

    this.chart.teethStatus = status;
  }

  private syncLegacyFlagsFromStatusMap(): void {
    const dtype = (this.chart.dtype || '').toLowerCase();
    const dtypeKey: keyof ToothStatus | null =
      dtype === 'teeth present' ? 'present'
        : dtype === 'carious teeth' ? 'carious'
          : dtype === 'decayed teeth' ? 'decayed'
            : dtype === 'missing teeth' ? 'missing'
              : dtype === 'filled teeth' ? 'filled'
                : null;

    const chartRecord = this.chart as unknown as Record<string, unknown>;

    for (const tooth of this.fdiTeethOrder) {
      const legacyField = this.legacyToothMap[tooth] as string;
      const value = dtypeKey ? !!this.chart.teethStatus?.[tooth]?.[dtypeKey] : false;
      chartRecord[legacyField] = value;
    }
  }

  onPatientSelected(): void {
    const selected = this.data.patientOptions.find(x => x.label === this.selectedPatientKey);
    if (!selected) return;

    this.chart.pno = selected.pNo;
    this.chart.consultId = selected.consultId;
    this.imaging.pno = selected.pNo;
    this.imaging.consultId = selected.consultId;
    this.consulting.pNo = selected.pNo;
    this.consulting.consultId = selected.consultId;
    this.consulting.clientCat = selected.clientCat || 'PRIVATE';
  }

  onImageSelected(files: FileList | null): void {
    if (!files?.length) return;

    for (let i = 0; i < files.length; i++) {
      const file = files.item(i);
      if (!file) continue;

      this.selectedImageFiles.push(file);

      const reader = new FileReader();
      reader.onload = () => {
        const url = (reader.result as string) || '';
        if (url) this.imagingPreviewUrls = [...this.imagingPreviewUrls, url];
      };
      reader.readAsDataURL(file);
    }

    this.imaging.fileName = this.selectedImageFiles[this.selectedImageFiles.length - 1]?.name;
  }

  removeImageAt(index: number): void {
    if (index < 0 || index >= this.imagingPreviewUrls.length) return;

    const previewToRemove = this.imagingPreviewUrls[index];
    this.imagingPreviewUrls = this.imagingPreviewUrls.filter((_, i) => i !== index);

    if (index < this.selectedImageFiles.length) {
      this.selectedImageFiles = this.selectedImageFiles.filter((_, i) => i !== index);
      this.imaging.fileName = this.selectedImageFiles[this.selectedImageFiles.length - 1]?.name;
    } else if (this.imaging.filePath === previewToRemove) {
      this.imaging.filePath = undefined;
      this.imaging.fileName = this.selectedImageFiles[this.selectedImageFiles.length - 1]?.name;
    }

    if (this.zoomImageUrl === previewToRemove) {
      this.zoomImageUrl = '';
    }
  }

  zoomImage(url: string): void {
    this.zoomImageUrl = url;
  }

  closeZoom(): void {
    this.zoomImageUrl = '';
  }

  save(): void {
    if (!this.chart.pno || !this.chart.consultId) {
      return;
    }

    this.chart.tDate = this.fromDateInput(this.chartDate, this.chart.tDate);
    this.imaging.imagingDate = this.fromDateInput(this.imagingDate, this.imaging.imagingDate);
    this.syncLegacyFlagsFromStatusMap();

    const closeWithPayload = () => {
      this.dialogRef.close({
        chart: this.withoutDefaults(this.chart),
        imaging: this.withoutDefaults(this.imaging),
        consulting: this.withoutDefaults(this.consulting)
      });
    };

    if (this.selectedImageFiles.length) {
      this.uploadSelectedImages(0, closeWithPayload);
      return;
    }

    closeWithPayload();
  }

  private uploadSelectedImages(index: number, done: () => void): void {
    const file = this.selectedImageFiles[index];
    if (!file) {
      this.selectedImageFiles = [];
      done();
      return;
    }

    this.dentalEndpoint.uploadImagingEndpoint<DentalImaging>({
      file,
      pno: this.chart.pno,
      consultId: this.chart.consultId,
      imagingDate: this.imaging.imagingDate,
      imagingType: this.imaging.imagingType,
      toothRegion: this.imaging.toothRegion,
      findings: this.imaging.findings,
      impression: this.imaging.impression,
      recommendations: this.imaging.recommendations,
      notes: this.imaging.notes
    }).subscribe({
      next: saved => {
        this.imaging = { ...this.imaging, ...saved };
        if (saved.filePath) {
          this.imagingPreviewUrls = [...this.imagingPreviewUrls.filter(x => x !== saved.filePath), saved.filePath];
        }

        this.uploadSelectedImages(index + 1, done);
      },
      error: error => {
        this.alertService.showStickyMessage('Upload error', 'Unable to upload one or more dental images.', MessageSeverity.error, error);
      }
    });
  }

  private toDateInput(value?: string): string {
    if (!value) return '';
    const d = new Date(value);
    if (Number.isNaN(d.getTime())) return '';
    const mm = `${d.getMonth() + 1}`.padStart(2, '0');
    const dd = `${d.getDate()}`.padStart(2, '0');
    return `${d.getFullYear()}-${mm}-${dd}`;
  }

  private fromDateInput(value: string, fallbackIso?: string): string {
    if (!value) return this.ensureValidIsoDate(fallbackIso);

    const parsed = new Date(`${value}T00:00:00`);
    if (Number.isNaN(parsed.getTime())) return this.ensureValidIsoDate(fallbackIso);

    return parsed.toISOString();
  }

  private ensureValidIsoDate(value?: string): string {
    if (value) {
      const d = new Date(value);
      if (!Number.isNaN(d.getTime())) {
        return d.toISOString();
      }
    }

    return new Date().toISOString();
  }

  private withoutDefaults<T>(obj: T): Partial<T> {
    const source = obj as Record<string, unknown>;
    const cleaned = Object.entries(source)
      .filter(([, v]) => !(v === null || v === undefined || v === '' || v === false || v === 0))
      .reduce((acc, [k, v]) => ({ ...acc, [k]: v }), {} as Record<string, unknown>);

    return cleaned as Partial<T>;
  }
}
