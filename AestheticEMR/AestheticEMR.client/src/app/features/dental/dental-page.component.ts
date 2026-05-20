import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { DentalEndpoint } from '../../services/dental-endpoint.service';
import { AttendanceEndpoint } from '../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../services/h-patient-endpoint.service';
import { Attendance } from '../../models/legacy/attendance.model';
import { HPatient } from '../../models/legacy/h-patient.model';
import { DentalEncounter, DentalImaging } from '../../models/dental.model';
import { DentalEncounterDialogComponent, DentalPatientOption } from './dental-encounter-dialog.component';
import { BillingInvoiceDialogComponent } from '../billing/invoices/billing-invoice-dialog.component';

@Component({
  selector: 'app-dental-page',
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
    <div class="dental-page">
      <div class="page-header">
        <div>
          <h2>Dental Clinic</h2>
          <p class="subtitle">Imaging and odontogram captured together in one dental encounter.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Dental Info
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          placeholder="Search by patient name, PNO or ConsultID..." />
      </div>

      <mat-card>
        @if (filteredRows().length === 0) {
          <p class="empty">No dental records found.</p>
        } @else {
          <table mat-table [dataSource]="filteredRows()" class="data-table">
            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row.pno) }}</td>
            </ng-container>

            <ng-container matColumnDef="consultId">
              <th mat-header-cell *matHeaderCellDef>Consult ID</th>
              <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
            </ng-container>

            <ng-container matColumnDef="imagingDate">
              <th mat-header-cell *matHeaderCellDef>Imaging Date</th>
              <td mat-cell *matCellDef="let row">{{ row.imagingDate | date:'dd-MMM-yyyy' }}</td>
            </ng-container>

            <ng-container matColumnDef="imagingType">
              <th mat-header-cell *matHeaderCellDef>Imaging Type</th>
              <td mat-cell *matCellDef="let row">{{ row.imagingType || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="findings">
              <th mat-header-cell *matHeaderCellDef>Findings</th>
              <td mat-cell *matCellDef="let row" class="truncate">{{ row.findings || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="openBilling(row)" title="Bill Patient">
                  <mat-icon>receipt_long</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="deleteImaging(row.id)" title="Delete" [disabled]="true">
                  <mat-icon>delete</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="columns"></tr>
            <tr mat-row *matRowDef="let row; columns: columns"></tr>
          </table>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .dental-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-row { margin-bottom: 12px; }
    .search-input { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; }
    .data-table { width: 100%; }
    .truncate { max-width: 320px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty { color: #888; text-align: center; padding: 20px; }
  `]
})
export class DentalPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);
  private readonly alertService = inject(AlertService);
  private readonly dentalEndpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);

  readonly imagingRecords = signal<DentalImaging[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly patientOptions = signal<DentalPatientOption[]>([]);

  readonly columns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'findings', 'actions'];
  readonly searchText = signal('');

  readonly filteredRows = computed(() => {
    const s = this.searchText().trim().toLowerCase();
    const base = s
      ? this.imagingRecords()
      : this.imagingRecords().filter(r => this.isToday(r.imagingDate));

    if (!s) {
      return base;
    }

    return base.filter(r =>
      (r.pno || '').toLowerCase().includes(s)
      || (r.consultId || '').toLowerCase().includes(s)
      || this.resolvePatientLabel(r.pno).toLowerCase().includes(s));
  });

  ngOnInit(): void {
    this.load();
  }

  openAddDialog(): void {
    const initialTabIndex = this.route.snapshot.routeConfig?.path === 'chart' ? 1 : 0;

    const ref = this.dialog.open(DentalEncounterDialogComponent, {
      width: '98vw',
      maxWidth: '980px',
      disableClose: true,
      data: {
        initialTabIndex,
        patientOptions: this.patientOptions()
      }
    });

    ref.afterClosed().subscribe((result: DentalEncounter | undefined) => {
      if (!result) return;
      this.saveEncounter(result);
    });
  }

  openEditDialog(row: DentalImaging): void {
    const initialTabIndex = this.route.snapshot.routeConfig?.path === 'chart' ? 1 : 0;

    this.dentalEndpoint.getEncounterEndpoint<DentalEncounter>(row.consultId, row.pno).subscribe({
      next: encounter => {
        const ref = this.dialog.open(DentalEncounterDialogComponent, {
          width: '98vw',
          maxWidth: '980px',
          disableClose: true,
          data: {
            initialTabIndex,
            patientOptions: this.patientOptions(),
            encounter
          }
        });

        ref.afterClosed().subscribe((result: DentalEncounter | undefined) => {
          if (!result) return;
          this.saveEncounter(result);
        });
      },
      error: error => {
        this.alertService.showStickyMessage('Load error', 'Unable to open dental encounter.', MessageSeverity.error, error);
      }
    });
  }

  openBilling(row: DentalImaging): void {
    const attendance = this.attendance().find(a => a.consultId === row.consultId && a.pNo === row.pno);

    const ref = this.dialog.open(BillingInvoiceDialogComponent, {
      width: '57vw',
      maxWidth: '780px',
      disableClose: true,
      data: {
        mode: 'create',
        consultId: row.consultId,
        billNo: row.consultId,
        coyID: attendance?.coyname ?? '',
        pNo: row.pno,
        clientID: attendance?.coyname ?? ''
      }
    });

    ref.afterClosed().subscribe(() => {
      // no-op; billing dialog handles save feedback internally
    });
  }

  deleteImaging(id: number): void {
    this.alertService.showDialog('Delete this dental record?', DialogType.confirm, () => {
      this.alertService.startLoadingMessage('Deleting...');
      this.dentalEndpoint.deleteImagingEndpoint<void>(id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.load();
          this.alertService.showMessage('Success', 'Dental record deleted.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Delete error', 'Unable to delete record.', MessageSeverity.error, error);
        }
      });
    });
  }

  private saveEncounter(payload: DentalEncounter): void {
    this.alertService.startLoadingMessage('Saving dental info...');
    this.dentalEndpoint.saveEncounterEndpoint<DentalEncounter>(payload).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.load();
        this.alertService.showMessage('Success', 'Dental encounter saved.', MessageSeverity.success);
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save error', 'Unable to save dental encounter.', MessageSeverity.error, error);
      }
    });
  }

  private load(): void {
    this.alertService.startLoadingMessage('Loading dental records...');
    Promise.all([
      this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([imaging, attendance, patients]) => {
      this.imagingRecords.set(imaging || []);
      this.attendance.set(attendance || []);
      this.patients.set(patients || []);
      this.patientOptions.set(this.buildPatientOptions());
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load dental records.', MessageSeverity.error, error);
    });
  }

  resolvePatientLabel(pno: string): string {
    const p = this.patients().find(x => x.pno === pno);
    return p ? `${p.pSurName} ${p.pFirstname ?? ''} [${pno}]`.trim() : `[${pno}]`;
  }

  private buildPatientOptions(): DentalPatientOption[] {
    const today = new Date();
    const items = this.attendance().filter(a => {
      const d = new Date(a.recDate);
      return d.getFullYear() === today.getFullYear()
        && d.getMonth() === today.getMonth()
        && d.getDate() === today.getDate();
    });

    const unique = new Map<string, Attendance>();
    for (const item of items) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) unique.set(key, item);
    }

    return Array.from(unique.values()).map(item => {
      const p = this.patients().find(x => x.pno === item.pNo);
      const fullName = `${p?.pSurName ?? 'Unknown'} ${p?.pFirstname ?? ''}`.trim();
      return {
        pNo: item.pNo,
        consultId: item.consultId,
        clientCat: item.clientCat,
        label: `${fullName} [${item.consultId}]`
      } as DentalPatientOption;
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
}
