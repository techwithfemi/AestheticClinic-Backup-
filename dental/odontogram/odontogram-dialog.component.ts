import { Component, inject, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTabsModule } from '@angular/material/tabs';
import { MatRadioModule } from '@angular/material/radio';

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
    MatCheckboxModule,
    MatTabsModule,
    MatRadioModule
  ],
  templateUrl: './odontogram-dialog.component.html',
  styleUrls: ['./odontogram-dialog.component.scss']
})
export class OdontogramDialogComponent implements OnInit {
  form: FormGroup;
  selectedTabIndex = 0;
  private alertService = inject(AlertService);
  patientSearchText: string = '';
  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<OdontogramDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
    this.form = this.fb.group({
      tDate: ['', Validators.required],
      dtype: ['', Validators.required],
      inflammationOfGingiva: ['', Validators.required],
      presenceOfDebris: ['', Validators.required],
      presenceOfCalculus: ['', Validators.required],
      presenceOfStains: ['', Validators.required],
      underOrthodonticTreatment: ['', Validators.required],
      otherClinicalFindings: ['', Validators.maxLength(200)],

      // Adult UL
      auli1: [false], auli2: [false], aulc: [false],
      aulpm1: [false], aulpm2: [false],
      aulm1: [false], aulm2: [false], aulm3: [false],
      // Adult UR
      auri1: [false], auri2: [false], aurc: [false],
      aurpm1: [false], aurpm2: [false],
      aurm1: [false], aurm2: [false], aurm3: [false],
      // Adult LL
      alli1: [false], alli2: [false], allc: [false],
      allpm1: [false], allpm2: [false],
      allm1: [false], allm2: [false], allm3: [false],
      // Adult LR
      alri1: [false], alri2: [false], alrc: [false],
      alrpm1: [false], alrpm2: [false],
      alrm1: [false], alrm2: [false], alrm3: [false],
      // Remarks
      aRem: [''],
      cRem: [''],
    });
  }

  ngOnInit() {
    if (this.data?.isEdit && this.data.chart) {
      this.form.patchValue(this.data.chart);
    }
  }

  get filteredPatientOptions(): ChartPatientOption[] {
    const term = this.patientSearchText.trim().toLowerCase();
    if (!term) return this.data.patientOptions;
    return this.data.patientOptions.filter((p: ChartPatientOption) =>
      p.label.toLowerCase().includes(term) ||
      p.pNo.toLowerCase().includes(term) ||
      p.consultId.toLowerCase().includes(term));
  }

  onPatientSearch(event: Event): void {
    this.patientSearchText = (event.target as HTMLInputElement).value;
  }

  filterPatientOptions(): ChartPatientOption[] {
    return this.data.patientOptions.filter((p: ChartPatientOption) =>
      p.label.toLowerCase().includes(this.patientSearchText.toLowerCase())
    );
  }

  onSave() {
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Please fill all required fields correctly.', MessageSeverity.error);
      this.form.markAllAsTouched();
      return;
    }
    const chart: DentalChart = { ...this.data.chart, ...this.form.value };
    this.dialogRef.close({ chart });
    this.alertService.showMessage('Dental record saved', '', MessageSeverity.success);
  }
}
