import { Component, OnInit, inject, signal, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent, MatPaginator } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { DentalEndpoint } from '../../services/dental-endpoint.service';
import { AttendanceEndpoint } from '../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../services/h-retainership-endpoint.service';
import { Attendance } from '../../models/legacy/attendance.model';
import { HPatient } from '../../models/legacy/h-patient.model';
import { HRetainership } from '../../models/legacy/h-retainership.model';
import { QryhvisitsForToday } from '../../models/legacy/qryhvisits-for-today.model';
import { DentalChart, DentalEncounter } from '../../models/dental.model';
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
    MatIconModule,
    MatPaginatorModule,
    MatTooltipModule
  ],
  template: `
    <div class="dental-page">
      <div class="page-header">
        <div>
          <h2>Dental Clinic</h2>
          <p class="subtitle">Treatment records with odontogram and clinical findings.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Treatment
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          (ngModelChange)="onSearchChange($event)"
          placeholder="Search by patient name, PNO, Consult ID or treatment type..." />
      </div>

      <mat-card>
        @if (tableDataSource.data.length === 0) {
          <p class="empty">No dental treatment records found.</p>
        } @else {
          <div class="table-container">
            <table mat-table [dataSource]="tableDataSource" class="dental-table">
              <!-- Patient Column -->
              <ng-container matColumnDef="patient">
                <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
                <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row.pno) }}</td>
              </ng-container>

              <!-- Consult ID Column -->
              <ng-container matColumnDef="consultId">
                <th mat-header-cell *matHeaderCellDef>Consult ID</th>
                <td mat-cell *matCellDef="let row">{{ row.consultId }}</td>
              </ng-container>

              <!-- Treatment Date Column -->
              <ng-container matColumnDef="treatmentDate">
                <th mat-header-cell *matHeaderCellDef>Treatment Date</th>
                <td mat-cell *matCellDef="let row">{{ row.tDate | date:'dd-MMM-yyyy' }}</td>
              </ng-container>

              <!-- Treatment Time Column -->
              <ng-container matColumnDef="treatmentTime">
                <th mat-header-cell *matHeaderCellDef>Treatment Time</th>
                <td mat-cell *matCellDef="let row">{{ row.tTime | date:'HH:mm' }}</td>
              </ng-container>

              <!-- Treatment Type Column -->
              <ng-container matColumnDef="treatmentType">
                <th mat-header-cell *matHeaderCellDef>Treatment Type</th>
                <td mat-cell *matCellDef="let row">{{ row.dtype || '—' }}</td>
              </ng-container>

              <!-- Actions Column -->
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Actions</th>
                <td mat-cell *matCellDef="let row">
                  <button 
                    mat-icon-button 
                    type="button" 
                    (click)="openBilling(row)" 
                    matTooltip="Create Bill"
                    color="accent">
                    <mat-icon>receipt_long</mat-icon>
                  </button>
                  <button 
                    mat-icon-button 
                    type="button" 
                    (click)="openEditDialog(row)" 
                    matTooltip="Edit Treatment">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button 
                    mat-icon-button 
                    type="button" 
                    (click)="deleteChart(row.id)" 
                    matTooltip="Delete Record"
                    color="warn">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="columns"></tr>
              <tr mat-row *matRowDef="let row; columns: columns"></tr>
            </table>
          </div>

          <mat-paginator
            #paginator
            [length]="totalRecords"
            [pageSize]="10"
            [pageSizeOptions]="[5, 10, 25, 50]"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .dental-page { padding: 20px; }
    .page-header { 
      display: flex; 
      justify-content: space-between; 
      align-items: flex-start; 
      margin-bottom: 20px; 
    }
    .subtitle { 
      color: #666; 
      margin: 4px 0 0; 
      font-size: 0.9rem; 
    }
    .search-row { margin-bottom: 12px; }
    .search-input { 
      width: 100%; 
      padding: 10px; 
      border: 1px solid #ddd; 
      border-radius: 6px;
      font-size: 14px;
    }
    .table-container { 
      overflow-x: auto;
      border-radius: 4px;
    }
    .dental-table { 
      width: 100%; 
      border-collapse: collapse;
    }
    .dental-table thead th {
      background-color: #f5f5f5;
      border-bottom: 2px solid #e0e0e0;
      font-weight: 600;
      text-align: left;
      padding: 12px;
      color: #333;
    }
    .dental-table tbody td {
      padding: 12px;
      border-bottom: 1px solid #e0e0e0;
      color: #555;
    }
    .dental-table tbody tr:hover {
      background-color: #fafafa;
    }
    .dental-table tbody tr:nth-child(odd) {
      background-color: #f9f9f9;
    }
    .empty { 
      color: #888; 
      text-align: center; 
      padding: 20px; 
    }
    button[matTooltip] {
      margin: 0 4px;
    }
  `]
})
export class DentalPageComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);
  private readonly alertService = inject(AlertService);
  private readonly dentalEndpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);

  // PRIMARY DATA SOURCE: Treatment records (HDentalTreat / DentalChart)
  readonly dentalCharts = signal<DentalChart[]>([]);
  
  // SUPPORTING DATA
  readonly attendance = signal<Attendance[]>([]);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly retainerships = signal<HRetainership[]>([]);
  readonly patientOptions = signal<DentalPatientOption[]>([]);

  readonly columns = ['patient', 'consultId', 'treatmentDate', 'treatmentTime', 'treatmentType', 'actions'];
  readonly searchText = signal('');
  readonly totalRecords = signal(0);
  
  tableDataSource = new MatTableDataSource<DentalChart>([]);

  ngOnInit(): void {
    this.load();
  }

  ngAfterViewInit(): void {
    this.tableDataSource.paginator = this.paginator;
  }

  onSearchChange(query: string): void {
    this.searchText.set(query);
    this.filterData();
  }

  onPageChange(_event: PageEvent): void {
    // MatTableDataSource handles pagination automatically
  }

  private filterData(): void {
    const s = this.searchText().trim().toLowerCase();
    
    let filtered = this.dentalCharts();
    
    if (!s) {
      // Empty search: show only today's treatment records
      filtered = filtered.filter(r => this.isToday(r.tDate));
    } else {
      // Search across treatment record fields
      filtered = filtered.filter(r =>
        (r.pno || '').toLowerCase().includes(s)
        || (r.consultId || '').toLowerCase().includes(s)
        || (r.dtype || '').toLowerCase().includes(s)
        || this.resolvePatientLabel(r.pno).toLowerCase().includes(s));
    }

    this.tableDataSource.data = filtered;
    this.totalRecords.set(filtered.length);
  }

  openAddDialog(): void {
    const initialTabIndex = 0;

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

  openEditDialog(row: DentalChart): void {
    const initialTabIndex = 0;

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

  openBilling(row: DentalChart): void {
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

  deleteChart(id: number): void {
    this.alertService.showDialog('Delete this dental treatment record?', DialogType.confirm, () => {
      this.alertService.startLoadingMessage('Deleting...');
      this.dentalEndpoint.deleteChartEndpoint<void>(id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.load();
          this.alertService.showMessage('Success', 'Dental treatment deleted.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Delete error', 'Unable to delete record.', MessageSeverity.error, error);
        }
      });
    });
  }

  private saveEncounter(payload: DentalEncounter): void {
    this.alertService.startLoadingMessage('Saving dental encounter...');
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
    this.alertService.startLoadingMessage('Loading dental treatment records...');
    Promise.all([
      this.dentalEndpoint.getChartsEndpoint<DentalChart[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise(),
      this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>().toPromise()
    ]).then(([charts, attendance, todayVisits, patients, retainerships]) => {
      this.dentalCharts.set(charts || []);
      this.attendance.set(attendance || []);
      this.todayVisits.set(todayVisits || []);
      this.patients.set(patients || []);
      this.retainerships.set(retainerships || []);
      this.patientOptions.set(this.buildPatientOptions());
      this.filterData();
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load dental treatment records.', MessageSeverity.error, error);
    });
  }

  resolvePatientLabel(pno: string): string {
    const p = this.patients().find(x => x.pno === pno);
    return p ? `${p.pSurName} ${p.pFirstname ?? ''} [${pno}]`.trim() : `[${pno}]`;
  }

  private buildPatientOptions(): DentalPatientOption[] {
    const unique = new Map<string, QryhvisitsForToday>();
    for (const item of this.todayVisits()) {
      if (!item.consultId || !item.pNo) continue;
      const key = `${item.consultId}|${item.pNo}`;
      if (!unique.has(key)) unique.set(key, item);
    }

    return Array.from(unique.values()).map(item => {
      const p = this.patients().find(x => x.pno === item.pNo);
      const fullName = (item.fullname || `${p?.pSurName ?? 'Unknown'} ${p?.pFirstname ?? ''}`).trim();
      const attendDate = this.formatAttendDate(item.recDate);
      const retainership = this.retainerships().find(x => x.retainId === item.coyName);
      const companyName = item.retainName || retainership?.retainName || p?.coyName || item.coyName;
      return {
        pNo: item.pNo,
        consultId: item.consultId,
        clientCat: item.clientCat,
        label: `${fullName} ${attendDate} [${item.consultId}]`,
        fullName,
        attendDate,
        photo: p?.patPixBase64,
        dateOfBirth: p?.dob,
        companyName,
        coyId: item.coyName,
        clinic: item.clinicType
      } as DentalPatientOption;
    }).sort((a, b) => a.label.localeCompare(b.label));
  }

  private formatAttendDate(value?: string): string {
    if (!value) return '';

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';

    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    }).replace(/ /g, '-');
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const recordDate = new Date(value);
    const today = new Date();

    return recordDate.toDateString() === today.toDateString();
  }
}
