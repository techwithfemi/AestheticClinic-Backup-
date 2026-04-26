import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { LaserDialogComponent } from './laser-dialog.component';

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
          [(ngModel)]="searchText"
          (input)="onSearch()"
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
              <td mat-cell *matCellDef="let row">{{ row.consultationDate | date:'mediumDate' }}</td>
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

            <ng-container matColumnDef="consent">
              <th mat-header-cell *matHeaderCellDef>Consent</th>
              <td mat-cell *matCellDef="let row">
                <mat-icon [class]="row.consentGiven ? 'icon-ok' : 'icon-warn'">
                  {{ row.consentGiven ? 'check_circle' : 'cancel' }}
                </mat-icon>
              </td>
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
  `]
})
export class LaserComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly searchText = signal<string>('');
  readonly displayedColumns = ['patient', 'date', 'provider', 'skin', 'device', 'consent', 'actions'];

  readonly filteredConsultations = computed(() => {
    const search = this.searchText().toLowerCase();
    if (!search) return this.consultations();

    return this.consultations().filter(c => {
      const label = this.resolvePatientLabel(c).toLowerCase();
      return label.includes(search);
    });
  });

  readonly todayAttendancePatients = computed(() => {
    const today = new Date().toISOString().split('T')[0];
    return this.attendance()
      .filter(a => a.recDate?.startsWith(today) && a.clinicType?.toLowerCase() === 'aesthetics')
      .map(a => a.pNo);
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
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise()
    ]).then(([patients, consultations, attendance]) => {
      this.patients.set(patients || []);
      this.consultations.set(consultations || []);
      this.attendance.set(attendance || []);
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
      data: { isEdit: false, patients: this.getTodayAttendancePatients() },
      width: '520px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: AestheticConsultation | undefined) => {
      if (!result) return;

      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Saving laser session...');

      this.endpoint.createLaserConsultationEndpoint<AestheticConsultation>(result).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.load();
          this.alertService.showMessage('Success', 'Laser session saved.', MessageSeverity.success);
        },
        error: (error: unknown) => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Save error', 'Unable to save laser session.', MessageSeverity.error, error);
        }
      });
    });
  }

  openEditDialog(consultation: AestheticConsultation): void {
    const dialogRef = this.dialog.open(LaserDialogComponent, {
      data: { isEdit: true, consultation, patients: this.getTodayAttendancePatients() },
      width: '520px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: AestheticConsultation | undefined) => {
      if (!result) return;

      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Updating laser session...');

      this.endpoint.updateConsultationEndpoint<AestheticConsultation>(result.id, result).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.load();
          this.alertService.showMessage('Success', 'Laser session updated.', MessageSeverity.success);
        },
        error: (error: unknown) => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Update error', 'Unable to update laser session.', MessageSeverity.error, error);
        }
      });
    });
  }

  delete(id: number): void {
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
        this.alertService.showStickyMessage('Delete error', 'Unable to delete laser session.', MessageSeverity.error, error);
      }
    });
  }

  onSearch(): void {
    // Search is handled by computed filteredConsultations
  }

  resolvePatientLabel(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      const patient = this.patients().find(x => x.id === row.patientId);
      return `${row.patientName} [${patient?.pno || 'N/A'}]`;
    }

    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName} [${p.pno || 'N/A'}]` : `Patient #${row.patientId}`;
  }

  private getTodayAttendancePatients(): AestheticPatient[] {
    const todayPNOs = this.todayAttendancePatients();
    return this.patients().filter(p => todayPNOs.includes(p.pno || ''));
  }
}
