import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { LaserDialogComponent, LaserDialogResult, LaserPatientOption } from './laser-dialog.component';
import { BillingInvoiceDialogComponent } from '../../billing/invoices/billing-invoice-dialog.component';

@Component({
  selector: 'app-laser',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ],
  template: `
    <div class="laser-page">
      <div class="page-header">
        <div>
          <h2>Laser Treatments</h2>
          <p class="subtitle">Record laser sessions including device settings, skin assessment, session progress and safety checks.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Laser Session
        </button>
      </div>

      <div class="search-section">
        <input
          type="text"
          class="search-input"
          [ngModel]="searchText()"
          (ngModelChange)="searchText.set($event ?? '')"
          placeholder="Search by patient name or PNO..." />
      </div>

      <mat-card>
        @if (filteredConsultations().length === 0 && !loadingIndicator) {
          <p class="empty-state">No laser sessions recorded yet.</p>
        }

        @if (filteredConsultations().length > 0) {
          <table mat-table [dataSource]="filteredConsultations()" class="data-table">

            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row) }}</td>
            </ng-container>

            <ng-container matColumnDef="date">
              <th mat-header-cell *matHeaderCellDef>Date</th>
              <td mat-cell *matCellDef="let row">{{ row.consultationDate | date:'dd-MMM-yyyy' }}</td>
            </ng-container>

            <ng-container matColumnDef="provider">
              <th mat-header-cell *matHeaderCellDef>Provider</th>
              <td mat-cell *matCellDef="let row">{{ row.provider || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="skin">
              <th mat-header-cell *matHeaderCellDef>Skin Type</th>
              <td mat-cell *matCellDef="let row">{{ row.skinAssessment || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="device">
              <th mat-header-cell *matHeaderCellDef>Device Settings</th>
              <td mat-cell *matCellDef="let row" class="device-cell">{{ row.deviceSettings || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="consentStatus">
              <th mat-header-cell *matHeaderCellDef>Consent Status</th>
              <td mat-cell *matCellDef="let row">
                <mat-icon [class]="row.consentGiven ? 'icon-ok' : 'icon-warn'">
                  {{ row.consentGiven ? 'check_circle' : 'cancel' }}
                </mat-icon>
                <span class="consent-text">{{ row.consentGiven ? 'Given' : 'Not given' }}</span>
              </td>
            </ng-container>

            <ng-container matColumnDef="consentDate">
              <th mat-header-cell *matHeaderCellDef>Consent Date</th>
              <td mat-cell *matCellDef="let row">{{ row.consentDate ? (row.consentDate | date:'dd-MMM-yyyy') : '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="openBilling(row)" title="Add Bill">
                  <mat-icon>receipt_long</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="delete(row.id)" title="Delete">
                  <mat-icon>delete</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
          </table>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .laser-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-section { margin-bottom: 16px; }
    .search-input { width: 100%; padding: 10px 12px; border: 1px solid #ddd; border-radius: 6px; font-size: 0.95rem; }
    .data-table { width: 100%; }
    .device-cell { max-width: 280px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty-state { color: #888; padding: 32px; text-align: center; }
    .icon-ok { color: #2e7d32; }
    .icon-warn { color: #c62828; }
    .consent-text { margin-left: 6px; }
  `]
})
export class LaserComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly searchText = signal<string>('');
  readonly displayedColumns = ['patient', 'date', 'provider', 'skin', 'device', 'consentStatus', 'consentDate', 'actions'];

  readonly filteredConsultations = computed(() => {
    const search = this.searchText().trim().toLowerCase();

    const base = search
      ? this.consultations()
      : this.consultations().filter(c => this.isToday(c.consultationDate));

    if (!search) {
      return base;
    }

    return base.filter(c => {
      const label = this.resolvePatientLabel(c).toLowerCase();
      return label.includes(search);
    });
  });

  readonly todayClinicAttendance = computed(() => {
    const todays = this.attendance().filter(a => this.isToday(a.recDate));

    const clinicMatched = todays.filter(a => {
      const clinic = (a.clinicType ?? '').toLowerCase();
      const purpose = (a.attndStatus ?? '').toLowerCase();
      return clinic.includes('laser')
        || clinic.includes('aesthetic')
        || purpose.includes('laser');
    });

    const source = clinicMatched.length > 0 ? clinicMatched : todays;

    const unique = new Map<string, Attendance>();
    for (const item of source) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) {
        unique.set(key, item);
      }
    }

    return Array.from(unique.values());
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading laser sessions...');

    Promise.all([
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.endpoint.getLaserConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([patients, consultations, attendance, legacyPatients]) => {
      this.patients.set(patients || []);
      this.consultations.set(consultations || []);
      this.attendance.set(attendance || []);
      this.legacyPatients.set(legacyPatients || []);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load laser sessions.', MessageSeverity.error, error);
    });
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(LaserDialogComponent, {
      data: { isEdit: false, patientOptions: this.getTodayAttendancePatientOptions() },
      width: '500px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: LaserDialogResult | undefined) => {
      if (!result) return;
      void this.saveConsultation(result);
    });
  }

  openEditDialog(consultation: AestheticConsultation): void {
    const options = this.getTodayAttendancePatientOptions();

    const dialogRef = this.dialog.open(LaserDialogComponent, {
      data: { isEdit: true, consultation, patientOptions: options },
      width: '500px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: LaserDialogResult | undefined) => {
      if (!result) return;
      void this.saveConsultation(result);
    });
  }

  openBilling(consultation: AestheticConsultation): void {
    const attendance = this.attendance().find(a => a.consultId === consultation.consultId && a.pNo === consultation.pNo);
    const legacy = this.legacyPatients().find(x => x.pno === consultation.pNo);

    const dialogRef = this.dialog.open(BillingInvoiceDialogComponent, {
      data: {
        mode: 'create',
        consultId: consultation.consultId,
        billNo: consultation.consultId,
        pNo: consultation.pNo,
        coyID: attendance?.coyname ?? legacy?.coyName ?? '',
        clientID: attendance?.coyname ?? legacy?.coyName ?? ''
      },
      width: '57vw',
      maxWidth: '780px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(() => {
      // no-op
    });
  }

  delete(id: number): void {
    this.alertService.showDialog('Are you sure you want to delete this laser session?', DialogType.confirm,
      () => {
        this.loadingIndicator = true;
        this.alertService.startLoadingMessage('Deleting laser session...');

        this.endpoint.deleteConsultationEndpoint<void>(id).subscribe({
          next: () => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.load();
            this.alertService.showMessage('Success', 'Laser session deleted.', MessageSeverity.success);
          },
          error: (error: unknown) => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.alertService.showStickyMessage('Delete error', this.getErrorMessage(error), MessageSeverity.error, error);
          }
        });
      });
  }

  onSearch(): void {
    return;
  }

  resolvePatientLabel(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      const patient = this.patients().find(x => x.id === row.patientId);
      return `${row.patientName} [${patient?.pno || 'N/A'}]`;
    }

    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName} [${p.pno || 'N/A'}]` : `Patient #${row.patientId}`;
  }

  private async saveConsultation(result: LaserDialogResult): Promise<void> {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(result.consultation.id ? 'Updating laser session...' : 'Saving laser session...');

    try {
      const consultation = { ...result.consultation };
      let patientId = result.selectedPatient.patientId;

      if (!patientId) {
        const createdPatient = await this.endpoint.createPatientEndpoint<AestheticPatient>({
          firstName: result.selectedPatient.firstName,
          lastName: result.selectedPatient.lastName,
          notes: result.selectedPatient.pNo ? `Legacy PNO: ${result.selectedPatient.pNo}` : ''
        }).toPromise();

        patientId = createdPatient?.id ?? 0;
      }

      if (!patientId) {
        throw new Error('Unable to resolve patient for Laser session.');
      }

      consultation.patientId = patientId;
      consultation.consultId = result.selectedPatient.consultId || consultation.consultId;
      consultation.pNo = result.selectedPatient.pNo || consultation.pNo;

      if (consultation.id) {
        await this.endpoint.updateConsultationEndpoint<AestheticConsultation>(consultation.id, consultation).toPromise();
        this.alertService.showMessage('Success', 'Laser session updated.', MessageSeverity.success);
      } else {
        await this.endpoint.createLaserConsultationEndpoint<AestheticConsultation>(consultation).toPromise();
        this.alertService.showMessage('Success', 'Laser session saved.', MessageSeverity.success);
      }

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.load();
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Save error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  private getTodayAttendancePatientOptions(): LaserPatientOption[] {
    return this.todayClinicAttendance().map(item => {
      const pNo = item.pNo ?? '';
      const legacy = this.legacyPatients().find(x => x.pno === pNo);
      const firstName = legacy?.pFirstname?.trim() || 'Unknown';
      const lastName = legacy?.pSurName?.trim() || 'Patient';

      const matchedAesthetic = this.patients().find(x =>
        x.firstName.trim().toLowerCase() === firstName.toLowerCase()
        && x.lastName.trim().toLowerCase() === lastName.toLowerCase());

      return {
        patientId: matchedAesthetic?.id ?? 0,
        consultId: item.consultId ?? '',
        pNo,
        firstName,
        lastName,
        label: `${lastName} ${firstName} [${item.consultId ?? 'N/A'}]`
      };
    }).sort((a, b) => a.label.localeCompare(b.label));
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private getErrorMessage(error: unknown): string {
    const message = (error as { error?: { message?: string }; message?: string })?.error?.message
      || (error as { message?: string })?.message;
    return message || 'Operation failed.';
  }
}
