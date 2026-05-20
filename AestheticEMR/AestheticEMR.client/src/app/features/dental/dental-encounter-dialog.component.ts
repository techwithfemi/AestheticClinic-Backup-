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
      <div class="top-row">
        <mat-form-field appearance="outline" class="full">
          <mat-label>Patient (ConsultID)</mat-label>
          <mat-select [(ngModel)]="selectedPatientKey" (selectionChange)="onPatientSelected()">
            @for (p of data.patientOptions; track p.label) {
              <mat-option [value]="p.label">{{ p.label }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
      </div>

      <mat-tab-group [(selectedIndex)]="selectedTabIndex">

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>assignment</mat-icon>
            <span>Clerking</span>
          </ng-template>
          <div class="tab-body">
            <mat-form-field appearance="outline"><mat-label>Imaging Date</mat-label><input matInput type="date" [(ngModel)]="imagingDate" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Imaging Type</mat-label><input matInput [(ngModel)]="imaging.imagingType" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Tooth Region</mat-label><input matInput [(ngModel)]="imaging.toothRegion" /></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Findings</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.findings"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Impression</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.impression"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Recommendations</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.recommendations"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Notes</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.notes"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Diagnosis</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.diagnosis"></textarea></mat-form-field>
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

            <div class="span-2 section-title treatment-bottom">Clinical Examination</div>

            <mat-form-field appearance="outline"><mat-label>Treatment Date</mat-label><input matInput type="date" [(ngModel)]="chartDate" /></mat-form-field>
            <mat-form-field appearance="outline">
              <mat-label>Treatment Type</mat-label>
              <mat-select [(ngModel)]="chart.dtype">
                <mat-option value="Teeth Present">Teeth Present</mat-option>
                <mat-option value="Carious Teeth">Carious Teeth</mat-option>
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
            <mat-icon>manage_accounts</mat-icon>
            <span>Management</span>
          </ng-template>
          <div class="tab-body">
            <mat-form-field appearance="outline" class="span-2"><mat-label>Prescription</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.prescription"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Investigations</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.investigate"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Services</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.services"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Treatment Plan</mat-label><textarea matInput rows="3" [(ngModel)]="consulting.treatPlan"></textarea></mat-form-field>
          </div>
        </mat-tab>

        <mat-tab>
          <ng-template mat-tab-label>
            <mat-icon>image</mat-icon>
            <span>Imaging</span>
          </ng-template>
          <div class="tab-body">
            <div class="span-2 section-title">Uploaded Dental Images</div>

            <div class="span-2 image-toolbar">
              <button mat-stroked-button type="button" (click)="fileInput.click()">Upload Dental Image</button>
              <input #fileInput type="file" accept="image/*" (change)="onImageSelected(fileInput.files)" style="display:none" />

              <mat-form-field appearance="outline" class="image-select">
                <mat-label>Saved Images</mat-label>
                <mat-select [(ngModel)]="selectedImageId" (selectionChange)="onSelectImage()">
                  <mat-option [value]="0">Current Draft</mat-option>
                  @for (img of patientImagingRecords; track img.id) {
                    <mat-option [value]="img.id">{{ imageOptionLabel(img) }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <div class="span-2 image-panel">
              @if (imagingPreviewUrl) {
                <img [src]="imagingPreviewUrl" [alt]="imaging.fileName || 'Dental image'" class="dental-image" />
              } @else {
                <p class="image-empty">No uploaded dental images for this patient.</p>
              }
              @if (imaging.fileName) {
                <p class="image-name">{{ imaging.fileName }}</p>
              }
            </div>

            <mat-form-field appearance="outline"><mat-label>Imaging Date</mat-label><input matInput type="date" [(ngModel)]="imagingDate" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Imaging Type</mat-label><input matInput [(ngModel)]="imaging.imagingType" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Tooth Region</mat-label><input matInput [(ngModel)]="imaging.toothRegion" /></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Findings</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.findings"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Impression</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.impression"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Recommendations</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.recommendations"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Notes</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.notes"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>File Name</mat-label><input matInput [(ngModel)]="imaging.fileName" /></mat-form-field>

            <div class="span-2 image-actions">
              <button mat-raised-button color="primary" type="button" (click)="saveImagingRecord()">{{ imaging.id ? 'Update Image Record' : 'Save Image Record' }}</button>
              <button mat-stroked-button color="warn" type="button" (click)="deleteImagingRecord()" [disabled]="!imaging.id">Delete Image Record</button>
            </div>
          </div>
        </mat-tab>

      </mat-tab-group>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
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

    .section-title { font-weight: 700; font-size: 0.9rem; color: #90caf9; text-transform: uppercase; letter-spacing: 0.04em; }
    .treatment-bottom { margin-top: 12px; }

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

    .image-panel { border: 1px dashed #4a5568; border-radius: 8px; padding: 16px; min-height: 220px; display: flex; flex-direction: column; align-items: center; justify-content: center; gap: 8px; }

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
    }

    @media (max-width: 768px) {
      .dialog-header h2 { font-size: 1rem; }
      .fdi-row-title { font-size: 0.72rem; }
      .fdi-tooth { font-size: 0.56rem; }
      .fdi-cell mat-checkbox { transform: scale(0.62); margin-top: -4px; }
      .fdi-grid-row { gap: 0; }
      .image-toolbar { flex-direction: column; align-items: stretch; }
      .image-actions { flex-direction: column; }
    }

    @media (max-width: 480px) {
      .fdi-tooth { font-size: 0.5rem; }
      .fdi-cell { padding: 0; }
      .fdi-cell mat-checkbox { transform: scale(0.54); margin-top: -5px; }
      .close-btn { width: 30px; height: 30px; }
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

  chart: DentalChart = { id: 0, pno: '', consultId: '', tDate: new Date().toISOString(), teethStatus: {} };
  imaging: DentalImaging = { id: 0, pno: '', consultId: '', imagingDate: new Date().toISOString() };
  consulting: DentalConsulting = { id: 0, consultId: '', pNo: '', clientCat: 'PRIVATE' };

  chartDate = this.toDateInput(this.chart.tDate);
  imagingDate = this.toDateInput(this.imaging.imagingDate);

  patientImagingRecords: DentalImaging[] = [];
  selectedImageId = 0;
  imagingPreviewUrl = '';
  selectedImageFile: File | null = null;

  get isEdit(): boolean {
    return !!this.data.encounter;
  }

  constructor() {
    if (this.data.encounter) {
      this.chart = { ...this.chart, ...this.data.encounter.chart, teethStatus: this.data.encounter.chart.teethStatus || {} };
      this.imaging = { ...this.imaging, ...this.data.encounter.imaging };
      this.consulting = { ...this.consulting, ...this.data.encounter.consulting };
      this.chartDate = this.toDateInput(this.chart.tDate);
      this.imagingDate = this.toDateInput(this.imaging.imagingDate);
      this.imagingPreviewUrl = this.imaging.filePath || '';

      const key = this.data.patientOptions.find(p => p.pNo === this.chart.pno && p.consultId === this.chart.consultId)?.label;
      this.selectedPatientKey = key || '';

      this.loadPatientImagingRecords();
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

    this.loadPatientImagingRecords();
  }

  imageOptionLabel(img: DentalImaging): string {
    return `${img.fileName || 'Untitled image'} (${this.toDateInput(img.imagingDate) || 'No date'})`;
  }

  onSelectImage(): void {
    if (!this.selectedImageId) {
      this.imaging = {
        ...this.imaging,
        id: 0,
        fileName: undefined,
        filePath: undefined,
        imagingType: undefined,
        toothRegion: undefined,
        findings: undefined,
        impression: undefined,
        recommendations: undefined,
        notes: undefined
      };
      this.imagingDate = this.toDateInput(new Date().toISOString());
      this.imagingPreviewUrl = '';
      this.selectedImageFile = null;
      return;
    }

    const selected = this.patientImagingRecords.find(x => x.id === this.selectedImageId);
    if (!selected) return;

    this.imaging = { ...selected };
    this.imagingDate = this.toDateInput(selected.imagingDate);
    this.imagingPreviewUrl = selected.filePath || '';
    this.selectedImageFile = null;
  }

  onImageSelected(files: FileList | null): void {
    const file = files?.item(0);
    if (!file) return;

    this.selectedImageFile = file;

    const reader = new FileReader();
    reader.onload = () => {
      const url = (reader.result as string) || '';
      this.imagingPreviewUrl = url;
      this.imaging.fileName = file.name;
    };
    reader.readAsDataURL(file);
  }

  saveImagingRecord(): void {
    if (!this.imaging.pno || !this.imaging.consultId) {
      this.alertService.showStickyMessage('Validation', 'Select a patient before saving imaging.', MessageSeverity.warn);
      return;
    }

    this.imaging.imagingDate = this.fromDateInput(this.imagingDate, this.imaging.imagingDate);

    const done = (saved: DentalImaging) => {
      this.imaging = { ...this.imaging, ...saved };
      this.imagingDate = this.toDateInput(this.imaging.imagingDate);
      this.imagingPreviewUrl = this.imaging.filePath || this.imagingPreviewUrl;
      this.selectedImageFile = null;
      this.alertService.showMessage('Imaging record saved', '', MessageSeverity.success);
      this.loadPatientImagingRecords(saved.id);
    };

    if (this.selectedImageFile) {
      this.dentalEndpoint.uploadImagingEndpoint<DentalImaging>({
        file: this.selectedImageFile,
        pno: this.imaging.pno,
        consultId: this.imaging.consultId,
        id: this.imaging.id > 0 ? this.imaging.id : undefined,
        imagingDate: this.imaging.imagingDate,
        imagingType: this.imaging.imagingType,
        toothRegion: this.imaging.toothRegion,
        findings: this.imaging.findings,
        impression: this.imaging.impression,
        recommendations: this.imaging.recommendations,
        notes: this.imaging.notes
      }).subscribe({
        next: done,
        error: error => {
          this.alertService.showStickyMessage('Upload error', 'Unable to upload and save imaging record.', MessageSeverity.error, error);
        }
      });
      return;
    }

    const payload = this.withoutDefaults(this.imaging);
    const request$ = this.imaging.id
      ? this.dentalEndpoint.updateImagingEndpoint<DentalImaging>(this.imaging.id, payload)
      : this.dentalEndpoint.createImagingEndpoint<DentalImaging>(payload);

    request$.subscribe({
      next: done,
      error: error => {
        this.alertService.showStickyMessage('Save error', 'Unable to save imaging record.', MessageSeverity.error, error);
      }
    });
  }

  deleteImagingRecord(): void {
    if (!this.imaging.id) return;

    this.dentalEndpoint.deleteImagingEndpoint<void>(this.imaging.id).subscribe({
      next: () => {
        this.alertService.showMessage('Imaging record deleted', '', MessageSeverity.success);
        this.selectedImageId = 0;
        this.onSelectImage();
        this.loadPatientImagingRecords();
      },
      error: error => {
        this.alertService.showStickyMessage('Delete error', 'Unable to delete imaging record.', MessageSeverity.error, error);
      }
    });
  }

  private loadPatientImagingRecords(preferredId?: number): void {
    if (!this.chart.pno || !this.chart.consultId) {
      this.patientImagingRecords = [];
      return;
    }

    this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().subscribe({
      next: rows => {
        this.patientImagingRecords = (rows || []).filter(x => x.pno === this.chart.pno && x.consultId === this.chart.consultId);
        if (preferredId && this.patientImagingRecords.some(x => x.id === preferredId)) {
          this.selectedImageId = preferredId;
          this.onSelectImage();
        }
      },
      error: () => {
        this.patientImagingRecords = [];
      }
    });
  }

  save(): void {
    if (!this.chart.pno || !this.chart.consultId) {
      return;
    }

    this.chart.tDate = this.fromDateInput(this.chartDate, this.chart.tDate);
    this.imaging.imagingDate = this.fromDateInput(this.imagingDate, this.imaging.imagingDate);
    this.syncLegacyFlagsFromStatusMap();

    this.dialogRef.close({
      chart: this.withoutDefaults(this.chart),
      imaging: this.withoutDefaults(this.imaging),
      consulting: this.withoutDefaults(this.consulting)
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
