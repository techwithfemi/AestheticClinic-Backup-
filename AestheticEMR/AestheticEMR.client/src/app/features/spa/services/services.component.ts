import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { SpaDialogComponent, SpaDialogResult, SpaPatientOption } from './spa-dialog.component';

@Component({
  selector: 'app-services',
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
    <div class="spa-page">
      <div class="page-header">
        <div>
          <h2>Spa Service Menu</h2>
          <p class="subtitle">Massage, facials, body scrub and sauna session capture.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Spa Session
        </button>
      </div>

      <div class="search-section">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          placeholder="Search by patient name or PNO..." />
      </div>

      <mat-card>
        @if (filteredConsultations().length === 0 && !loadingIndicator) {
          <p class="empty-state">No spa sessions recorded yet.</p>
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

            <ng-container matColumnDef="service">
              <th mat-header-cell *matHeaderCellDef>Service</th>
              <td mat-cell *matCellDef="let row">{{ row.indication || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="focus">
              <th mat-header-cell *matHeaderCellDef>Area / Focus</th>
              <td mat-cell *matCellDef="let row">{{ row.areaTreated || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="notes">
              <th mat-header-cell *matHeaderCellDef>Notes</th>
              <td mat-cell *matCellDef="let row" class="truncate">{{ row.procedureDescription || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
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
    .spa-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-section { margin-bottom: 16px; }
    .search-input { width: 100%; padding: 10px 12px; border: 1px solid #ddd; border-radius: 6px; font-size: 0.95rem; }
    .data-table { width: 100%; }
    .truncate { max-width: 320px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty-state { color: #888; padding: 32px; text-align: center; }
  `]
})
export class ServicesComponent {
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
  searchText = '';
  readonly displayedColumns = ['patient', 'date', 'service', 'focus', 'notes', 'actions'];

  readonly filteredConsultations = computed(() => {
    const term = this.searchText.trim().toLowerCase();
    const source = term
      ? this.consultations()
      : this.consultations().filter(c => this.isToday(c.consultationDate));

    if (!term) return source;

    return source.filter(c => this.resolvePatientLabel(c).toLowerCase().includes(term));
  });

  readonly todayClinicAttendance = computed(() => {
    const todays = this.attendance().filter(a => this.isToday(a.recDate));

    const clinicMatched = todays.filter(a => {
      const clinic = (a.clinicType ?? '').toLowerCase();
      const status = (a.attndStatus ?? '').toLowerCase();
      return clinic.includes('spa')
        || clinic.includes('massage')
        || clinic.includes('facial')
        || clinic.includes('sauna')
        || status.includes('spa');
    });

    const source = clinicMatched.length > 0 ? clinicMatched : todays;
    const unique = new Map<string, Attendance>();

    for (const item of source) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) unique.set(key, item);
    }

    return Array.from(unique.values());
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading spa sessions...');

    Promise.all([
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.endpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
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
      this.alertService.showStickyMessage('Load error', 'Unable to load spa sessions.', MessageSeverity.error, error);
    });
  }

  openAddDialog(): void {
    const options = this.getTodayAttendancePatientOptions();

    const dialogRef = this.dialog.open(SpaDialogComponent, {
      data: { isEdit: false, patientOptions: options },
      width: '95vw',
      maxWidth: '640px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: SpaDialogResult | undefined) => {
      if (!result) return;
      void this.saveConsultation(result);
    });
  }

  openEditDialog(consultation: AestheticConsultation): void {
    const options = this.getTodayAttendancePatientOptions();
    const existingPatient = this.patients().find(x => x.id === consultation.patientId);

    if (existingPatient && !options.some(x => x.patientId === existingPatient.id)) {
      options.unshift({
        patientId: existingPatient.id,
        consultId: '',
        pNo: existingPatient.pno ?? '',
        firstName: existingPatient.firstName,
        lastName: existingPatient.lastName,
        label: `${existingPatient.lastName} ${existingPatient.firstName} [${existingPatient.pno ?? 'N/A'}]`
      });
    }

    const dialogRef = this.dialog.open(SpaDialogComponent, {
      data: { isEdit: true, consultation, patientOptions: options },
      width: '95vw',
      maxWidth: '640px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: SpaDialogResult | undefined) => {
      if (!result) return;
      void this.saveConsultation(result);
    });
  }

  delete(id: number): void {
    this.alertService.showDialog('Are you sure you want to delete this spa session?', DialogType.confirm,
      () => {
        this.loadingIndicator = true;
        this.alertService.startLoadingMessage('Deleting spa session...');

        this.endpoint.deleteConsultationEndpoint<void>(id).subscribe({
          next: () => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.load();
            this.alertService.showMessage('Success', 'Spa session deleted.', MessageSeverity.success);
          },
          error: (error: unknown) => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.alertService.showStickyMessage('Delete error', this.getErrorMessage(error), MessageSeverity.error, error);
          }
        });
      });
  }

  resolvePatientLabel(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      const patient = this.patients().find(x => x.id === row.patientId);
      return `${row.patientName} [${patient?.pno || 'N/A'}]`;
    }

    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName} [${p.pno || 'N/A'}]` : `Patient #${row.patientId}`;
  }

  private async saveConsultation(result: SpaDialogResult): Promise<void> {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(result.consultation.id ? 'Updating spa session...' : 'Saving spa session...');

    try {
      const consultation = { ...result.consultation };
      let patientId = result.selectedPatient.patientId;

      if (!patientId) {
        const createdPatient = await this.endpoint.createPatientEndpoint<AestheticPatient>({
          firstName: result.selectedPatient.firstName,
          lastName: result.selectedPatient.lastName,
          pno: result.selectedPatient.pNo,
          notes: result.selectedPatient.pNo ? `Legacy PNO: ${result.selectedPatient.pNo}` : ''
        }).toPromise();

        patientId = createdPatient?.id ?? 0;
      }

      if (!patientId) {
        throw new Error('Unable to resolve patient for spa session.');
      }

      consultation.patientId = patientId;

      if (consultation.id) {
        await this.endpoint.updateConsultationEndpoint<AestheticConsultation>(consultation.id, consultation).toPromise();
        this.alertService.showMessage('Success', 'Spa session updated.', MessageSeverity.success);
      } else {
        await this.endpoint.createSpaConsultationEndpoint<AestheticConsultation>(consultation).toPromise();
        this.alertService.showMessage('Success', 'Spa session saved.', MessageSeverity.success);
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

  private getTodayAttendancePatientOptions(): SpaPatientOption[] {
    const options: SpaPatientOption[] = this.todayClinicAttendance().map(item => {
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
    });

    if (options.length === 0) {
      const attendanceFallback = this.attendance().map(item => {
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
        } as SpaPatientOption;
      });

      options.push(...attendanceFallback);
    }

    if (options.length === 0) {
      const legacyFallback = this.legacyPatients().map(p => {
        const firstName = p.pFirstname?.trim() || 'Unknown';
        const lastName = p.pSurName?.trim() || 'Patient';

        const matchedAesthetic = this.patients().find(x =>
          x.firstName.trim().toLowerCase() === firstName.toLowerCase()
          && x.lastName.trim().toLowerCase() === lastName.toLowerCase());

        return {
          patientId: matchedAesthetic?.id ?? 0,
          consultId: '',
          pNo: p.pno ?? '',
          firstName,
          lastName,
          label: `${lastName} ${firstName} [${p.pno ?? 'N/A'}]`
        } as SpaPatientOption;
      });

      options.push(...legacyFallback);
    }

    if (options.length === 0) {
      const aestheticFallback = this.patients().map(p => ({
        patientId: p.id,
        consultId: '',
        pNo: p.pno ?? '',
        firstName: p.firstName,
        lastName: p.lastName,
        label: `${p.lastName} ${p.firstName} [${p.pno ?? 'N/A'}]`
      } as SpaPatientOption));

      options.push(...aestheticFallback);
    }

    const unique = new Map<string, SpaPatientOption>();
    for (const item of options) {
      const key = `${item.consultId}|${item.pNo}|${item.firstName}|${item.lastName}`.toLowerCase();
      if (!unique.has(key)) {
        unique.set(key, item);
      }
    }

    return Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
  }

  private isToday(value?: string): boolean {
    if (!value) return false;

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return false;

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
