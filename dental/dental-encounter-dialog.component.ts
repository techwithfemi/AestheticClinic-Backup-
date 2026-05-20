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

import { DentalChart, DentalConsulting, DentalEncounter, DentalImaging } from '../../models/dental.model';

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
    MatCheckboxModule
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
        <mat-tab label="Imaging + Clerking">
          <div class="tab-body">
            <mat-form-field appearance="outline"><mat-label>Imaging Date</mat-label><input matInput type="date" [(ngModel)]="imagingDate" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Imaging Type</mat-label><input matInput [(ngModel)]="imaging.imagingType" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Tooth Region</mat-label><input matInput [(ngModel)]="imaging.toothRegion" /></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Findings</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.findings"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Impression</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.impression"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Recommendations</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.recommendations"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Notes</mat-label><textarea matInput rows="2" [(ngModel)]="imaging.notes"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Diagnosis</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.diagnosis"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Prescription</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.prescription"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Services</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.services"></textarea></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Investigation</mat-label><textarea matInput rows="2" [(ngModel)]="consulting.investigate"></textarea></mat-form-field>
          </div>
        </mat-tab>

        <mat-tab label="Odontogram">
          <div class="tab-body">
            <mat-form-field appearance="outline"><mat-label>Treatment Date</mat-label><input matInput type="date" [(ngModel)]="chartDate" /></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Treatment Type</mat-label><input matInput [(ngModel)]="chart.dtype" /></mat-form-field>

            <div class="span-2 chart-section-label">Adult Dentition</div>
            <div class="span-2 quadrant-grid">
              <div class="quadrant">
                <div class="quad-label">Upper Left (UL)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.auli1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.auli2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aulm3" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Upper Right (UR)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.auri1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.auri2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.aurm3" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Lower Left (LL)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alli1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alli2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.allm3" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Lower Right (LR)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alri1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alri2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.alrm3" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
            </div>

            <mat-form-field appearance="outline" class="span-2"><mat-label>Adult Remarks</mat-label><textarea matInput rows="2" [(ngModel)]="chart.aRem"></textarea></mat-form-field>

            <div class="span-2 chart-section-label">Primary (Child) Dentition</div>
            <div class="span-2 quadrant-grid">
              <div class="quadrant">
                <div class="quad-label">Upper Left (UL)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.culi1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.culi2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.culc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.culpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.culpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Upper Right (UR)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.curi1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.curi2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.curc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.curpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.curpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Lower Left (LL)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clli1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clli2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.cllc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.cllpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.cllpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
              <div class="quadrant">
                <div class="quad-label">Lower Right (LR)</div>
                <div class="tooth-row">
                  <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clri1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clri2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clrc" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clrpm1" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                  <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox [(ngModel)]="chart.clrpm2" [ngModelOptions]="{standalone: true}"></mat-checkbox></div></div>
                </div>
              </div>
            </div>

            <mat-form-field appearance="outline" class="span-2"><mat-label>Child Remarks</mat-label><textarea matInput rows="2" [(ngModel)]="chart.cRem"></textarea></mat-form-field>
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
  `]
})
export class DentalEncounterDialogComponent {
  readonly dialogRef = inject(MatDialogRef<DentalEncounterDialogComponent>);
  readonly data = inject<DentalEncounterDialogData>(MAT_DIALOG_DATA);

  selectedTabIndex = this.data.initialTabIndex;
  selectedPatientKey = '';

  chart: DentalChart = { id: 0, pno: '', consultId: '', tDate: new Date().toISOString() };
  imaging: DentalImaging = { id: 0, pno: '', consultId: '', imagingDate: new Date().toISOString() };
  consulting: DentalConsulting = { id: 0, consultId: '', pNo: '', clientCat: 'PRIVATE' };

  chartDate = this.toDateInput(this.chart.tDate);
  imagingDate = this.toDateInput(this.imaging.imagingDate);

  get isEdit(): boolean {
    return !!this.data.encounter;
  }

  constructor() {
    if (this.data.encounter) {
      this.chart = { ...this.chart, ...this.data.encounter.chart };
      this.imaging = { ...this.imaging, ...this.data.encounter.imaging };
      this.consulting = { ...this.consulting, ...this.data.encounter.consulting };
      this.chartDate = this.toDateInput(this.chart.tDate);
      this.imagingDate = this.toDateInput(this.imaging.imagingDate);

      const key = this.data.patientOptions.find(p => p.pNo === this.chart.pno && p.consultId === this.chart.consultId)?.label;
      this.selectedPatientKey = key || '';
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

  save(): void {
    if (!this.chart.pno || !this.chart.consultId) {
      return;
    }

    this.chart.tDate = this.fromDateInput(this.chartDate, this.chart.tDate);
    this.imaging.imagingDate = this.fromDateInput(this.imagingDate, this.imaging.imagingDate);

    this.dialogRef.close({
      chart: this.withoutDefaults(this.chart),
      imaging: this.withoutDefaults(this.imaging),
      consulting: this.withoutDefaults({ ...this.consulting, treatPlan: undefined })
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
