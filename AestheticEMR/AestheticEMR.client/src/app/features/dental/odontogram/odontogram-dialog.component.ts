import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { DentalChart } from '../../../models/dental.model';

export interface ChartPatientOption {
  pNo: string;
  consultId: string;
  firstName: string;
  lastName: string;
  label: string;
}

export interface ChartDialogResult {
  chart: DentalChart;
  selectedPatient: ChartPatientOption;
}

@Component({
  selector: 'app-odontogram-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Dental Chart' : 'Add Dental Chart' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" class="close-btn" aria-label="Close">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">

          <!-- Patient picker (add mode only) -->
          @if (!data.isEdit) {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Search Patient</mat-label>
              <input matInput [value]="patientSearchText" (input)="onPatientSearch($event)"
                     placeholder="Type name or PNO" />
            </mat-form-field>
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Patient (ConsultID)</mat-label>
              <mat-select formControlName="patientKey" required>
                @for (p of filteredPatientOptions; track p.label) {
                  <mat-option [value]="p.label">{{ p.label }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          } @else {
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Patient (ConsultID)</mat-label>
              <input matInput [value]="data.chart?.patientName || data.chart?.pno || ''" readonly />
            </mat-form-field>
          }

          <div class="row-2">
            <mat-form-field appearance="outline" class="half-width">
              <mat-label>Treatment Date</mat-label>
              <input matInput [matDatepicker]="datePicker" formControlName="tDate" required />
              <mat-datepicker-toggle matIconSuffix [for]="datePicker"></mat-datepicker-toggle>
              <mat-datepicker #datePicker></mat-datepicker>
            </mat-form-field>
            <mat-form-field appearance="outline" class="half-width">
              <mat-label>Treatment Type</mat-label>
              <mat-select formControlName="dtype">
                <mat-option value="Extraction">Extraction</mat-option>
                <mat-option value="Filling">Filling</mat-option>
                <mat-option value="Root Canal">Root Canal</mat-option>
                <mat-option value="Crown">Crown</mat-option>
                <mat-option value="Scaling">Scaling / Cleaning</mat-option>
                <mat-option value="Denture">Denture</mat-option>
                <mat-option value="Orthodontic">Orthodontic</mat-option>
                <mat-option value="Other">Other</mat-option>
              </mat-select>
            </mat-form-field>
          </div>

          <!-- ── Odontogram Grid ─────────────────────────────────────────── -->
          <div class="chart-section-label">Adult Dentition</div>

          <div class="quadrant-grid">
            <div class="quadrant">
              <div class="quad-label">Upper Left (UL)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="auli1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="auli2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="aulc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox formControlName="aulpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox formControlName="aulpm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="aulm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="aulm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox formControlName="aulm3"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Upper Right (UR)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="auri1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="auri2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="aurc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox formControlName="aurpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox formControlName="aurpm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="aurm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="aurm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox formControlName="aurm3"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Lower Left (LL)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="alli1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="alli2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="allc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox formControlName="allpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox formControlName="allpm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="allm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="allm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox formControlName="allm3"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Lower Right (LR)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="alri1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="alri2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="alrc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM1</span><div class="tooth-box"><mat-checkbox formControlName="alrpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">PM2</span><div class="tooth-box"><mat-checkbox formControlName="alrpm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="alrm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="alrm2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M3</span><div class="tooth-box"><mat-checkbox formControlName="alrm3"></mat-checkbox></div></div>
              </div>
            </div>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Adult Remarks</mat-label>
            <textarea matInput rows="2" formControlName="aRem" placeholder="e.g. Caries on upper left molars..."></textarea>
          </mat-form-field>

          <div class="chart-section-label">Primary (Child) Dentition</div>

          <div class="quadrant-grid">
            <div class="quadrant">
              <div class="quad-label">Upper Left (UL)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="culi1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="culi2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="culc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="culpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="culpm2"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Upper Right (UR)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="curi1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="curi2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="curc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="curpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="curpm2"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Lower Left (LL)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="clli1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="clli2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="cllc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="cllpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="cllpm2"></mat-checkbox></div></div>
              </div>
            </div>
            <div class="quadrant">
              <div class="quad-label">Lower Right (LR)</div>
              <div class="tooth-row">
                <div class="tooth"><span class="tooth-label">I1</span><div class="tooth-box"><mat-checkbox formControlName="clri1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">I2</span><div class="tooth-box"><mat-checkbox formControlName="clri2"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">C</span><div class="tooth-box"><mat-checkbox formControlName="clrc"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M1</span><div class="tooth-box"><mat-checkbox formControlName="clrpm1"></mat-checkbox></div></div>
                <div class="tooth"><span class="tooth-label">M2</span><div class="tooth-box"><mat-checkbox formControlName="clrpm2"></mat-checkbox></div></div>
              </div>
            </div>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Child Remarks</mat-label>
            <textarea matInput rows="2" formControlName="cRem"></textarea>
          </mat-form-field>

        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">Save</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 100%; box-sizing: border-box; padding: 16px; overflow: hidden; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .close-btn { position: relative; flex-shrink: 0; }
    .full-width { width: 100%; margin-bottom: 12px; box-sizing: border-box; }
    .half-width { width: calc(50% - 6px); margin-bottom: 12px; box-sizing: border-box; }
    .row-2 { display: flex; gap: 12px; width: 100%; box-sizing: border-box; }
    mat-dialog-content { max-height: 70vh; overflow-y: auto; overflow-x: hidden; padding: 0; margin: 0; width: 100%; box-sizing: border-box; }
    mat-form-field { display: block; width: 100%; box-sizing: border-box; }

    .chart-section-label { font-weight: 600; font-size: 0.85rem; color: #1565c0; margin: 8px 0 6px; text-transform: uppercase; letter-spacing: 0.05em; border-bottom: 1px solid #e0e0e0; padding-bottom: 4px; }

    .quadrant-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; margin-bottom: 10px; width: 100%; box-sizing: border-box; }
    .quadrant { background: #f5f5f5; border-radius: 6px; padding: 8px; box-sizing: border-box; min-width: 0; }
    .quad-label { font-size: 0.72rem; font-weight: 600; color: #555; margin-bottom: 8px; }

    .tooth-row { display: flex; flex-wrap: wrap; gap: 6px 4px; }

    /* each tooth = label text on top, checkbox below, clipped cell */
    .tooth {
      display: flex;
      flex-direction: column;
      align-items: center;
      width: 34px;
      overflow: hidden;
    }
    .tooth-label {
      font-size: 0.65rem;
      font-weight: 600;
      color: #444;
      line-height: 1;
      height: 14px;
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      text-align: center;
      margin-bottom: 2px;
      white-space: nowrap;
    }
    .tooth-box {
      width: 24px;
      height: 24px;
      display: flex;
      align-items: center;
      justify-content: center;
      overflow: hidden;
      flex-shrink: 0;
    }
    .tooth-box mat-checkbox {
      --mdc-checkbox-state-layer-size: 24px;
    }
    .tooth-box mat-checkbox ::ng-deep .mdc-checkbox {
      padding: 0;
      width: 18px;
      height: 18px;
    }
    .tooth-box mat-checkbox ::ng-deep .mdc-checkbox__background {
      width: 18px;
      height: 18px;
      top: 0;
      left: 0;
    }
  `]
})
export class OdontogramDialogComponent {
  private fb = inject(FormBuilder);
  private _dialogRef = inject(MatDialogRef<OdontogramDialogComponent>);
  private _data = inject<{ isEdit: boolean; chart?: DentalChart; patientOptions: ChartPatientOption[] }>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  patientSearchText = '';

  get filteredPatientOptions(): ChartPatientOption[] {
    const term = this.patientSearchText.trim().toLowerCase();
    if (!term) return this.data.patientOptions;
    return this.data.patientOptions.filter(p =>
      p.label.toLowerCase().includes(term) ||
      p.pNo.toLowerCase().includes(term) ||
      p.consultId.toLowerCase().includes(term));
  }

  private c = this.data.chart;

  form = this.fb.nonNullable.group({
    id: [this.c?.id ?? 0],
    patientKey: [this.data.isEdit ? (this.c?.patientName || this.c?.pno || '') : '', Validators.required],
    tDate: [this.c?.tDate ? new Date(this.c.tDate) : new Date(), Validators.required],
    dtype: [this.c?.dtype ?? ''],
    // Adult UL
    auli1: [this.c?.auli1 ?? false], auli2: [this.c?.auli2 ?? false], aulc: [this.c?.aulc ?? false],
    aulpm1: [this.c?.aulpm1 ?? false], aulpm2: [this.c?.aulpm2 ?? false],
    aulm1: [this.c?.aulm1 ?? false], aulm2: [this.c?.aulm2 ?? false], aulm3: [this.c?.aulm3 ?? false],
    // Adult UR
    auri1: [this.c?.auri1 ?? false], auri2: [this.c?.auri2 ?? false], aurc: [this.c?.aurc ?? false],
    aurpm1: [this.c?.aurpm1 ?? false], aurpm2: [this.c?.aurpm2 ?? false],
    aurm1: [this.c?.aurm1 ?? false], aurm2: [this.c?.aurm2 ?? false], aurm3: [this.c?.aurm3 ?? false],
    // Adult LL
    alli1: [this.c?.alli1 ?? false], alli2: [this.c?.alli2 ?? false], allc: [this.c?.allc ?? false],
    allpm1: [this.c?.allpm1 ?? false], allpm2: [this.c?.allpm2 ?? false],
    allm1: [this.c?.allm1 ?? false], allm2: [this.c?.allm2 ?? false], allm3: [this.c?.allm3 ?? false],
    // Adult LR
    alri1: [this.c?.alri1 ?? false], alri2: [this.c?.alri2 ?? false], alrc: [this.c?.alrc ?? false],
    alrpm1: [this.c?.alrpm1 ?? false], alrpm2: [this.c?.alrpm2 ?? false],
    alrm1: [this.c?.alrm1 ?? false], alrm2: [this.c?.alrm2 ?? false], alrm3: [this.c?.alrm3 ?? false],
    // Child UL
    culi1: [this.c?.culi1 ?? false], culi2: [this.c?.culi2 ?? false], culc: [this.c?.culc ?? false],
    culpm1: [this.c?.culpm1 ?? false], culpm2: [this.c?.culpm2 ?? false],
    // Child UR
    curi1: [this.c?.curi1 ?? false], curi2: [this.c?.curi2 ?? false], curc: [this.c?.curc ?? false],
    curpm1: [this.c?.curpm1 ?? false], curpm2: [this.c?.curpm2 ?? false],
    // Child LL
    clli1: [this.c?.clli1 ?? false], clli2: [this.c?.clli2 ?? false], cllc: [this.c?.cllc ?? false],
    cllpm1: [this.c?.cllpm1 ?? false], cllpm2: [this.c?.cllpm2 ?? false],
    // Child LR
    clri1: [this.c?.clri1 ?? false], clri2: [this.c?.clri2 ?? false], clrc: [this.c?.clrc ?? false],
    clrpm1: [this.c?.clrpm1 ?? false], clrpm2: [this.c?.clrpm2 ?? false],
    // Remarks
    aRem: [this.c?.aRem ?? ''],
    cRem: [this.c?.cRem ?? '']
  });

  onPatientSearch(event: Event): void {
    this.patientSearchText = (event.target as HTMLInputElement).value;
  }

  save(): void {
    if (this.form.invalid) return;

    const val = this.form.getRawValue();
    const selectedPatient = this.data.isEdit
      ? { pNo: this.c!.pno, consultId: this.c!.consultId, firstName: '', lastName: '', label: '' }
      : (this.data.patientOptions.find(p => p.label === val.patientKey) ?? {
          pNo: '', consultId: '', firstName: '', lastName: '', label: val.patientKey
        });

    const chart: DentalChart = {
      id: val.id,
      pno: this.data.isEdit ? this.c!.pno : selectedPatient.pNo,
      consultId: this.data.isEdit ? this.c!.consultId : selectedPatient.consultId,
      tDate: val.tDate instanceof Date ? val.tDate.toISOString() : val.tDate,
      dtype: val.dtype || undefined,
      auli1: val.auli1, auli2: val.auli2, aulc: val.aulc,
      aulpm1: val.aulpm1, aulpm2: val.aulpm2,
      aulm1: val.aulm1, aulm2: val.aulm2, aulm3: val.aulm3,
      auri1: val.auri1, auri2: val.auri2, aurc: val.aurc,
      aurpm1: val.aurpm1, aurpm2: val.aurpm2,
      aurm1: val.aurm1, aurm2: val.aurm2, aurm3: val.aurm3,
      alli1: val.alli1, alli2: val.alli2, allc: val.allc,
      allpm1: val.allpm1, allpm2: val.allpm2,
      allm1: val.allm1, allm2: val.allm2, allm3: val.allm3,
      alri1: val.alri1, alri2: val.alri2, alrc: val.alrc,
      alrpm1: val.alrpm1, alrpm2: val.alrpm2,
      alrm1: val.alrm1, alrm2: val.alrm2, alrm3: val.alrm3,
      culi1: val.culi1, culi2: val.culi2, culc: val.culc,
      culpm1: val.culpm1, culpm2: val.culpm2,
      curi1: val.curi1, curi2: val.curi2, curc: val.curc,
      curpm1: val.curpm1, curpm2: val.curpm2,
      clli1: val.clli1, clli2: val.clli2, cllc: val.cllc,
      cllpm1: val.cllpm1, cllpm2: val.cllpm2,
      clri1: val.clri1, clri2: val.clri2, clrc: val.clrc,
      clrpm1: val.clrpm1, clrpm2: val.clrpm2,
      aRem: val.aRem || undefined,
      cRem: val.cRem || undefined
    };

    this._dialogRef.close({ chart, selectedPatient } as ChartDialogResult);
  }
}
