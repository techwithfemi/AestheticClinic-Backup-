import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

import { AestheticConsentStatus, SignAestheticConsent } from '../../../models/aesthetic.model';
import { QryhvisitsForToday } from '../../../models/legacy/qryhvisits-for-today.model';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { AttendanceSummaryComponent } from '../../../components/attendance-summary/attendance-summary.component';

export interface ConsentDialogData {
  status: AestheticConsentStatus;
  patientName: string;
}

@Component({
  selector: 'app-consent-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatIconModule,
    AttendanceSummaryComponent
  ],
  template: `
    <div class="dialog-content">
      <h2 mat-dialog-title>Patient Consent</h2>

      <mat-dialog-content class="dialog-body">
        <!-- Header: attendance summary (visible once a patient is selected) -->
        @if (selectedVisit()) {
          <section class="dialog-header" data-testid="consent-dialog-header">
            <app-attendance-summary
              [attendance]="attendance()"
              [photo]="attendance()?.patientPhotoBase64">
            </app-attendance-summary>
          </section>
        } @else {
          <p class="hint">Select a patient from today's visits to begin.</p>
        }

        <!-- Patient picker: today's visits table (pageSize = 10) -->
        <section class="patient-picker">
          <div class="picker-toolbar">
            <h3>Today's Visits</h3>
            <button mat-stroked-button type="button" (click)="loadVisits()" [disabled]="loadingVisits()">
              <mat-icon>refresh</mat-icon>
              Refresh
            </button>
          </div>

          @if (loadingVisits()) {
            <div class="loading-row">
              <mat-spinner diameter="28"></mat-spinner>
              <span>Loading today's visits…</span>
            </div>
          }

          <div class="table-wrap">
            <table mat-table [dataSource]="pagedVisits()" class="visits-table">
              <ng-container matColumnDef="patient">
                <th mat-header-cell *matHeaderCellDef>Patient</th>
                <td mat-cell *matCellDef="let row">
                  <div class="patient-cell">
                    <div class="patient-name">{{ row.fullname }}</div>
                    <div class="patient-sub">{{ row.pNo }}</div>
                  </div>
                </td>
              </ng-container>

              <ng-container matColumnDef="recDate">
                <th mat-header-cell *matHeaderCellDef>Visit Date</th>
                <td mat-cell *matCellDef="let row">{{ formatRecDate(row.recDate) }}</td>
              </ng-container>

              <ng-container matColumnDef="consultId">
                <th mat-header-cell *matHeaderCellDef>Consult ID</th>
                <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
              </ng-container>

              <ng-container matColumnDef="clinicType">
                <th mat-header-cell *matHeaderCellDef>Clinic</th>
                <td mat-cell *matCellDef="let row">{{ row.clinicType || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="select">
                <th mat-header-cell *matHeaderCellDef class="select-col"></th>
                <td mat-cell *matCellDef="let row" class="select-col">
                  <button mat-stroked-button color="primary" type="button"
                          (click)="selectVisit(row)"
                          [disabled]="loadingAttendance() && selectedVisit()?.consultId === row.consultId">
                    @if (loadingAttendance() && selectedVisit()?.consultId === row.consultId) {
                      <mat-spinner diameter="16"></mat-spinner>
                    } @else {
                      Select
                    }
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="visitsColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: visitsColumns;"
                  [class.selected]="selectedVisit()?.consultId === row.consultId"></tr>

              @if (!loadingVisits() && pagedVisits().length === 0) {
                <tr class="no-data-row">
                  <td [attr.colspan]="visitsColumns.length" class="no-data-cell">
                    No visits for today.
                  </td>
                </tr>
              }
            </table>
          </div>

          @if (filteredVisits().length > 0) {
            <mat-paginator
              [length]="filteredVisits().length"
              [pageSize]="pageSize()"
              [pageSizeOptions]="[10, 25, 50]"
              (page)="onPageChanged($event)">
            </mat-paginator>
          }
        </section>

        <!-- Consent signing form (visible only after a patient is selected) -->
        @if (selectedVisit()) {
          @if (!data.status.attendanceTaken) {
            <p class="warning">Attendance must be taken before the patient can sign consent.</p>
          } @else {
            <section class="consent-section">
              <p class="kv"><strong>ConsultId:</strong> {{ data.status.consultId }}</p>
              <p class="kv"><strong>PNO:</strong> {{ data.status.pNo }}</p>
              <p class="kv"><strong>Procedure:</strong> {{ data.status.procedureType }}</p>

              <div class="consent-box">
                <h3>{{ data.status.activeTemplate?.title || 'Consent' }}</h3>
                <p>{{ data.status.activeTemplate?.content }}</p>
              </div>

              <form [formGroup]="form">
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

                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Signature Image (Base64, optional)</mat-label>
                  <textarea matInput rows="3" formControlName="signatureImageBase64"></textarea>
                </mat-form-field>

                <mat-checkbox formControlName="accepted">I confirm the patient has reviewed and accepted this consent</mat-checkbox>
              </form>
            </section>
          }
        }
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="submit()"
                [disabled]="!data.status.attendanceTaken || form.invalid || !selectedVisit()">
          Sign Consent
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 1000px; max-width: 100%; }
    .dialog-body { display: flex; flex-direction: column; gap: 16px; max-height: 70vh; }

    .dialog-header {
      background: #fafafa;
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 8px 12px;
    }

    .hint { color: #666; font-style: italic; margin: 0; }

    .patient-picker { display: flex; flex-direction: column; gap: 8px; }
    .picker-toolbar { display: flex; align-items: center; justify-content: space-between; gap: 12px; }
    .picker-toolbar h3 { margin: 0; font-size: 1rem; font-weight: 500; }

    .loading-row { display: flex; align-items: center; gap: 12px; padding: 12px; color: #666; }

    .table-wrap { overflow-x: auto; }
    table.visits-table { width: 100%; }
    .visits-table th { font-weight: 500; }
    .visits-table td { padding: 10px 12px; }

    .visits-table tr.selected { background: #e3f2fd; }

    .patient-cell .patient-name { font-weight: 500; }
    .patient-cell .patient-sub { font-size: 0.8rem; color: #666; }
    .select-col { width: 110px; text-align: right; }

    .no-data-row td { text-align: center; padding: 24px; color: #999; }

    .consent-section { display: flex; flex-direction: column; gap: 8px; }
    .kv { margin: 0; }
    .full-width { width: 100%; margin-bottom: 12px; }
    .consent-box { border: 1px solid #ddd; border-radius: 8px; padding: 12px; margin: 8px 0 16px; background: #fafafa; }
    .warning { color: #c62828; font-weight: 600; margin: 0; }
  `]
})
export class ConsentDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly billingEndpoint = inject(BillingEndpoint);

  readonly dialogRef = inject(MatDialogRef<ConsentDialogComponent>);
  readonly data = inject<ConsentDialogData>(MAT_DIALOG_DATA);

  // Today's visits (QryhvisitsForToday)
  readonly visits = signal<QryhvisitsForToday[]>([]);
  readonly loadingVisits = signal<boolean>(false);
  readonly pageIndex = signal<number>(0);
  readonly pageSize = signal<number>(10);

  // Patient selection + attendance summary (VwhRecord)
  readonly selectedVisit = signal<QryhvisitsForToday | null>(null);
  readonly attendance = signal<VwhRecord | null>(null);
  readonly loadingAttendance = signal<boolean>(false);

  readonly visitsColumns = ['patient', 'recDate', 'consultId', 'clinicType', 'select'];

  readonly filteredVisits = computed(() => this.visits());
  readonly pagedVisits = computed(() => {
    const start = this.pageIndex() * this.pageSize();
    return this.filteredVisits().slice(start, start + this.pageSize());
  });

  readonly form = this.fb.nonNullable.group({
    signatureName: ['', Validators.required],
    witnessedBy: [''],
    notes: [''],
    signatureImageBase64: [''],
    accepted: [false, Validators.requiredTrue]
  });

  ngOnInit(): void {
    this.loadVisits();
  }

  loadVisits(): void {
    this.loadingVisits.set(true);
    this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().subscribe({
      next: visits => {
        this.visits.set(visits || []);
        this.pageIndex.set(0);
        this.loadingVisits.set(false);
      },
      error: () => {
        this.visits.set([]);
        this.loadingVisits.set(false);
      }
    });
  }

  onPageChanged(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  selectVisit(visit: QryhvisitsForToday): void {
    if (!visit?.consultId) {
      return;
    }

    this.selectedVisit.set(visit);
    this.attendance.set(null);
    this.loadingAttendance.set(true);

    this.billingEndpoint.getVwhRecordSummaryEndpoint<VwhRecord>(visit.consultId).subscribe({
      next: record => {
        this.attendance.set(record || null);
        this.loadingAttendance.set(false);
      },
      error: () => {
        this.attendance.set(null);
        this.loadingAttendance.set(false);
      }
    });
  }

  formatRecDate(value?: string): string {
    if (!value) {
      return '—';
    }

    // Try ISO parse first
    let date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      // Try yyyy-MM-dd
      const parts = value.split('-');
      if (parts.length === 3) {
        date = new Date(+parts[0], +parts[1] - 1, +parts[2]);
      }
    }

    if (Number.isNaN(date.getTime())) {
      return value;
    }

    const day = date.getDate().toString().padStart(2, '0');
    const month = date.toLocaleString('en', { month: 'short' });
    const year = date.getFullYear();
    return `${day} ${month} ${year}`;
  }

  submit(): void {
    if (this.form.invalid
        || !this.data.status.activeTemplate?.id
        || !this.data.status.consultId
        || !this.data.status.pNo
        || !this.data.status.procedureType
        || !this.selectedVisit()) {
      return;
    }

    const value = this.form.getRawValue();
    const payload: SignAestheticConsent = {
      patientId: this.data.status.latestSignedConsent?.patientId,
      consultId: this.data.status.consultId,
      pNo: this.data.status.pNo,
      procedureType: this.data.status.procedureType,
      consentTemplateId: this.data.status.activeTemplate.id,
      signatureName: value.signatureName,
      witnessedBy: value.witnessedBy,
      notes: value.notes,
      signatureImageBase64: value.signatureImageBase64
    };

    this.dialogRef.close(payload);
  }
}
