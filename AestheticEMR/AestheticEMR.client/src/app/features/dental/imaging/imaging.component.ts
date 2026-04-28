import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { DentalEndpoint } from '../../../services/dental-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { DentalImaging } from '../../../models/dental.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { ImagingDialogComponent, ImagingDialogResult, ImagingPatientOption } from './imaging-dialog.component';

@Component({
  selector: 'app-imaging',
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
    <div class="imaging-page">
      <div class="page-header">
        <div>
          <h2>Dental Imaging</h2>
          <p class="subtitle">X-ray and radiographic records including findings and impressions.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Imaging Record
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
        @if (loadingIndicator) {
          <p class="empty-state">Loading...</p>
        }

        @if (!loadingIndicator && filteredImaging().length === 0) {
          <p class="empty-state">No imaging records found.</p>
        }

        @if (filteredImaging().length > 0) {
          <table mat-table [dataSource]="filteredImaging()" class="data-table">

            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row) }}</td>
            </ng-container>

            <ng-container matColumnDef="consultId">
              <th mat-header-cell *matHeaderCellDef>Consult ID</th>
              <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
            </ng-container>

            <ng-container matColumnDef="imagingDate">
              <th mat-header-cell *matHeaderCellDef>Date</th>
              <td mat-cell *matCellDef="let row">{{ row.imagingDate | date:'dd-MMM-yyyy' }}</td>
            </ng-container>

            <ng-container matColumnDef="imagingType">
              <th mat-header-cell *matHeaderCellDef>Type</th>
              <td mat-cell *matCellDef="let row">{{ row.imagingType || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="toothRegion">
              <th mat-header-cell *matHeaderCellDef>Region</th>
              <td mat-cell *matCellDef="let row">{{ row.toothRegion || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="findings">
              <th mat-header-cell *matHeaderCellDef>Findings</th>
              <td mat-cell *matCellDef="let row" class="findings-cell">{{ row.findings || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="fileName">
              <th mat-header-cell *matHeaderCellDef>File</th>
              <td mat-cell *matCellDef="let row">
                @if (row.fileName) {
                  <mat-icon class="icon-file" title="{{ row.fileName }}">image</mat-icon>
                  <span class="file-name">{{ row.fileName }}</span>
                } @else {
                  <span>—</span>
                }
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="deleteImaging(row.id)" title="Delete">
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
    .imaging-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-section { margin-bottom: 16px; }
    .search-input { width: 100%; padding: 10px 12px; border: 1px solid #ddd; border-radius: 6px; font-size: 0.95rem; box-sizing: border-box; }
    .data-table { width: 100%; }
    .findings-cell { max-width: 260px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .file-name { font-size: 0.82rem; margin-left: 4px; vertical-align: middle; }
    .icon-file { color: #1565c0; font-size: 18px; vertical-align: middle; }
    .empty-state { color: #888; padding: 32px; text-align: center; }
  `]
})
export class ImagingComponent {
  private readonly endpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly imagingRecords = signal<DentalImaging[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly searchText = signal<string>('');
  readonly displayedColumns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'toothRegion', 'findings', 'fileName', 'actions'];

  readonly filteredImaging = computed(() => {
    const search = this.searchText().trim().toLowerCase();
    if (!search) return this.imagingRecords();
    return this.imagingRecords().filter(r =>
      this.resolvePatientLabel(r).toLowerCase().includes(search) ||
      (r.pno ?? '').toLowerCase().includes(search) ||
      (r.consultId ?? '').toLowerCase().includes(search)
    );
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading imaging records...');

    Promise.all([
      this.endpoint.getImagingEndpoint<DentalImaging[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([imaging, attendance, legacyPatients]) => {
      this.imagingRecords.set(imaging || []);
      this.attendance.set(attendance || []);
      this.legacyPatients.set(legacyPatients || []);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load imaging records.', MessageSeverity.error, error);
    });
  }

  resolvePatientLabel(row: DentalImaging): string {
    if (row.patientName?.trim()) return `${row.patientName} [${row.pno}]`;
    const legacy = this.legacyPatients().find(p => p.pno === row.pno);
    if (legacy) return `${legacy.pSurName} ${legacy.pFirstname ?? ''} [${row.pno}]`.trim();
    return `[${row.pno}]`;
  }

  openAddDialog(): void {
    const patientOptions = this.getTodayAttendanceOptions();
    if (patientOptions.length === 0) {
      this.alertService.showStickyMessage('No attendance found', 'No attendance records found for today.', MessageSeverity.warn);
      return;
    }

    const dialogRef = this.dialog.open(ImagingDialogComponent, {
      data: { isEdit: false, patientOptions },
      width: '480px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ImagingDialogResult | undefined) => {
      if (!result) return;
      this.saveImaging(result);
    });
  }

  openEditDialog(imaging: DentalImaging): void {
    const dialogRef = this.dialog.open(ImagingDialogComponent, {
      data: { isEdit: true, imaging, patientOptions: [] },
      width: '480px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ImagingDialogResult | undefined) => {
      if (!result) return;
      this.saveImaging(result);
    });
  }

  deleteImaging(id: number): void {
    this.alertService.showDialog('Are you sure you want to delete this imaging record?', DialogType.confirm, () => {
      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Deleting...');

      this.endpoint.deleteImagingEndpoint<void>(id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.load();
          this.alertService.showMessage('Success', 'Imaging record deleted.', MessageSeverity.success);
        },
        error: (error: unknown) => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Delete error', this.getErrorMessage(error), MessageSeverity.error, error);
        }
      });
    });
  }

  onSearch(): void { return; }

  private saveImaging(result: ImagingDialogResult): void {
    this.loadingIndicator = true;
    const isEdit = result.imaging.id > 0;
    this.alertService.startLoadingMessage(isEdit ? 'Updating imaging record...' : 'Saving imaging record...');

    const obs = isEdit
      ? this.endpoint.updateImagingEndpoint<DentalImaging>(result.imaging.id, result.imaging)
      : this.endpoint.createImagingEndpoint<DentalImaging>(result.imaging);

    obs.subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.load();
        this.alertService.showMessage('Success', isEdit ? 'Imaging record updated.' : 'Imaging record saved.', MessageSeverity.success);
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Save error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private getTodayAttendanceOptions(): ImagingPatientOption[] {
    const today = new Date();
    const todayAttendance = this.attendance().filter(a => {
      const d = new Date(a.recDate);
      return d.getFullYear() === today.getFullYear()
        && d.getMonth() === today.getMonth()
        && d.getDate() === today.getDate();
    });

    const unique = new Map<string, Attendance>();
    for (const item of todayAttendance) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) unique.set(key, item);
    }

    return Array.from(unique.values()).map(item => {
      const pNo = item.pNo ?? '';
      const legacy = this.legacyPatients().find(p => p.pno === pNo);
      const firstName = legacy?.pFirstname?.trim() || '';
      const lastName = legacy?.pSurName?.trim() || 'Unknown';
      return {
        pNo,
        consultId: item.consultId ?? '',
        firstName,
        lastName,
        label: `${lastName} ${firstName} [${item.consultId ?? 'N/A'}]`.trim()
      };
    }).sort((a, b) => a.label.localeCompare(b.label));
  }

  private getErrorMessage(error: unknown): string {
    const msg = (error as { error?: { message?: string }; message?: string })?.error?.message
      || (error as { message?: string })?.message;
    return msg || 'Operation failed.';
  }
}
