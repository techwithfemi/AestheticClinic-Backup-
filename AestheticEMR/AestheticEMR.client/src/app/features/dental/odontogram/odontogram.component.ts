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
import { DentalChart } from '../../../models/dental.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { OdontogramDialogComponent, ChartDialogResult, ChartPatientOption } from './odontogram-dialog.component';

@Component({
  selector: 'app-odontogram',
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
    <div class="chart-page">
      <div class="page-header">
        <div>
          <h2>Odontogram / Dental Chart</h2>
          <p class="subtitle">Dental treatment records with tooth-by-tooth charting per quadrant.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Dental Chart
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
        @if (!loadingIndicator && filteredCharts().length === 0) {
          <p class="empty-state">No dental chart records found.</p>
        }
        @if (filteredCharts().length > 0) {
          <table mat-table [dataSource]="filteredCharts()" class="data-table">

            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row) }}</td>
            </ng-container>

            <ng-container matColumnDef="consultId">
              <th mat-header-cell *matHeaderCellDef>Consult ID</th>
              <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
            </ng-container>

            <ng-container matColumnDef="tDate">
              <th mat-header-cell *matHeaderCellDef>Treatment Date</th>
              <td mat-cell *matCellDef="let row">{{ row.tDate | date:'dd-MMM-yyyy' }}</td>
            </ng-container>

            <ng-container matColumnDef="dtype">
              <th mat-header-cell *matHeaderCellDef>Treatment Type</th>
              <td mat-cell *matCellDef="let row">{{ row.dtype || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="aRem">
              <th mat-header-cell *matHeaderCellDef>Remarks</th>
              <td mat-cell *matCellDef="let row" class="remark-cell">{{ row.aRem || row.cRem || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="deleteChart(row.id)" title="Delete">
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
    .chart-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-section { margin-bottom: 16px; }
    .search-input { width: 100%; padding: 10px 12px; border: 1px solid #ddd; border-radius: 6px; font-size: 0.95rem; box-sizing: border-box; }
    .data-table { width: 100%; }
    .remark-cell { max-width: 280px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty-state { color: #888; padding: 32px; text-align: center; }
  `]
})
export class OdontogramComponent {
  private readonly endpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly charts = signal<DentalChart[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly searchText = signal<string>('');
  readonly displayedColumns = ['patient', 'consultId', 'tDate', 'dtype', 'aRem', 'actions'];

  readonly filteredCharts = computed(() => {
    const search = this.searchText().trim().toLowerCase();
    if (!search) return this.charts();
    return this.charts().filter(c =>
      this.resolvePatientLabel(c).toLowerCase().includes(search) ||
      (c.pno ?? '').toLowerCase().includes(search) ||
      (c.consultId ?? '').toLowerCase().includes(search)
    );
  });

  constructor() { this.load(); }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading dental charts...');

    Promise.all([
      this.endpoint.getChartsEndpoint<DentalChart[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([charts, attendance, legacyPatients]) => {
      this.charts.set(charts || []);
      this.attendance.set(attendance || []);
      this.legacyPatients.set(legacyPatients || []);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load dental charts.', MessageSeverity.error, error);
    });
  }

  resolvePatientLabel(row: DentalChart): string {
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
    const dialogRef = this.dialog.open(OdontogramDialogComponent, {
      data: { isEdit: false, patientOptions },
      width: '98vw',
      maxWidth: '640px',
      disableClose: true
    });
    dialogRef.afterClosed().subscribe((result: ChartDialogResult | undefined) => {
      if (result) this.saveChart(result);
    });
  }

  openEditDialog(chart: DentalChart): void {
    const dialogRef = this.dialog.open(OdontogramDialogComponent, {
      data: { isEdit: true, chart, patientOptions: [] },
      width: '98vw',
      maxWidth: '640px',
      disableClose: true
    });
    dialogRef.afterClosed().subscribe((result: ChartDialogResult | undefined) => {
      if (result) this.saveChart(result);
    });
  }

  deleteChart(id: number): void {
    this.alertService.showDialog('Are you sure you want to delete this dental chart?', DialogType.confirm, () => {
      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Deleting...');
      this.endpoint.deleteChartEndpoint<void>(id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.load();
          this.alertService.showMessage('Success', 'Dental chart deleted.', MessageSeverity.success);
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

  private saveChart(result: ChartDialogResult): void {
    this.loadingIndicator = true;
    const isEdit = result.chart.id > 0;
    this.alertService.startLoadingMessage(isEdit ? 'Updating...' : 'Saving...');

    const obs = isEdit
      ? this.endpoint.updateChartEndpoint<DentalChart>(result.chart.id, result.chart)
      : this.endpoint.createChartEndpoint<DentalChart>(result.chart);

    obs.subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.load();
        this.alertService.showMessage('Success', isEdit ? 'Dental chart updated.' : 'Dental chart saved.', MessageSeverity.success);
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Save error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  private getTodayAttendanceOptions(): ChartPatientOption[] {
    const today = new Date();
    const todayAtt = this.attendance().filter(a => {
      const d = new Date(a.recDate);
      return d.getFullYear() === today.getFullYear()
        && d.getMonth() === today.getMonth()
        && d.getDate() === today.getDate();
    });
    const unique = new Map<string, Attendance>();
    for (const item of todayAtt) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) unique.set(key, item);
    }
    return Array.from(unique.values()).map(item => {
      const pNo = item.pNo ?? '';
      const legacy = this.legacyPatients().find(p => p.pno === pNo);
      const firstName = legacy?.pFirstname?.trim() || '';
      const lastName = legacy?.pSurName?.trim() || 'Unknown';
      return { pNo, consultId: item.consultId ?? '', firstName, lastName,
        label: `${lastName} ${firstName} [${item.consultId ?? 'N/A'}]`.trim() };
    }).sort((a, b) => a.label.localeCompare(b.label));
  }

  private getErrorMessage(error: unknown): string {
    const msg = (error as { error?: { message?: string }; message?: string })?.error?.message
      || (error as { message?: string })?.message;
    return msg || 'Operation failed.';
  }
}
