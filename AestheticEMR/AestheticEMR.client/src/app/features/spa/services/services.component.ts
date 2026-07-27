import { Component, ViewChild, computed, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../../services/h-retainership-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HRetainership } from '../../../models/legacy/h-retainership.model';
import { SpaDialogComponent, SpaDialogResult, SpaPatientOption } from './spa-dialog.component';
import { BillingInvoiceDialogComponent } from '../../billing/invoices/billing-invoice-dialog.component';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule
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

      <mat-form-field appearance="outline" class="search-field">
        <mat-label>Search by patient name</mat-label>
        <mat-icon matPrefix>search</mat-icon>
        <input matInput [ngModel]="searchText()" (ngModelChange)="searchText.set($event)" placeholder="Type to filter..." />
        @if (searchText()) {
          <button mat-icon-button matSuffix type="button" (click)="searchText.set('')" aria-label="Clear">
            <mat-icon>close</mat-icon>
          </button>
        }
      </mat-form-field>

      <mat-card class="table-card">
        <table mat-table [dataSource]="dataSource" class="data-table">

          <ng-container matColumnDef="patient">
            <th mat-header-cell *matHeaderCellDef>Patient</th>
            <td mat-cell *matCellDef="let row">{{ resolvePatientName(row) }}</td>
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

          <ng-container matColumnDef="consultId">
            <th mat-header-cell *matHeaderCellDef>Consult ID</th>
            <td mat-cell *matCellDef="let row">{{ row.consultId || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="services">
            <th mat-header-cell *matHeaderCellDef>Services</th>
            <td mat-cell *matCellDef="let row" class="truncate">{{ row.services || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="notes">
            <th mat-header-cell *matHeaderCellDef>Notes</th>
            <td mat-cell *matCellDef="let row" class="truncate">{{ row.procedureDescription || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let row" style="white-space:nowrap">
              <button mat-icon-button type="button" (click)="openBilling(row)" title="Add Bill">
                <mat-icon>receipt_long</mat-icon>
              </button>
              <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button type="button" (click)="delete(row.id)" title="Delete" [disabled]="true">
                <mat-icon>delete</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
          <tr class="mat-row" *matNoDataRow>
            <td class="mat-cell no-data-cell" [attr.colspan]="displayedColumns.length">
              <mat-icon>spa</mat-icon>
              <span>No spa sessions recorded yet.</span>
            </td>
          </tr>
        </table>

        <mat-paginator [pageSize]="10" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `,
  styles: [`
    .spa-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; gap: 12px; }
    .page-header h2 { margin: 0; font-size: 1.6rem; }
    .subtitle { color: #888; margin: 4px 0 0; font-size: 0.9rem; }
    .search-field { width: 100%; margin-bottom: 8px; }
    .table-card { padding: 0; overflow: hidden; }
    .data-table { width: 100%; }
    .truncate { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .no-data-cell { text-align: center; padding: 40px 16px; color: #888; }
    .no-data-cell mat-icon { display: block; font-size: 2.5rem; width: 2.5rem; height: 2.5rem; margin: 0 auto 8px; opacity: 0.4; }

    ::ng-deep .mat-column-patient    { width: 16%; }
    ::ng-deep .mat-column-date       { width: 10%; }
    ::ng-deep .mat-column-service    { width: 11%; }
    ::ng-deep .mat-column-consultId  { width: 11%; font-size: 0.85rem; color: #666; }
    ::ng-deep .mat-column-services   { width: 18%; }
    ::ng-deep .mat-column-focus      { width: 10%; }
    ::ng-deep .mat-column-notes      { width: 14%; }
    ::ng-deep .mat-column-actions    { width: 10%; white-space: nowrap; }
    ::ng-deep .mat-column-actions .mat-cell { white-space: nowrap; }

    @media (max-width: 992px) {
      .spa-page { padding: 16px; }
      .page-header { flex-direction: column; align-items: stretch; }
      .page-header button { width: 100%; min-height: 44px; }
    }

    @media (max-width: 575.98px) {
      .spa-page { padding: 12px; }
    }
  `]
})
export class ServicesComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly retainerships = signal<HRetainership[]>([]);
  readonly searchText = signal('');
  readonly displayedColumns = ['patient', 'date', 'service', 'consultId', 'services', 'focus', 'notes', 'actions'];
  readonly dataSource = new MatTableDataSource<AestheticConsultation>([]);

  @ViewChild(MatPaginator) set matPaginator(p: MatPaginator) {
    if (p) this.dataSource.paginator = p;
  }

  readonly filteredConsultations = computed(() => {
    const term = this.searchText().trim().toLowerCase();
    const source = term
      ? this.consultations()
      : this.consultations().filter(c => this.isToday(c.consultationDate));

    if (!term) return source;

    return source.filter(c => this.resolvePatientLabel(c).toLowerCase().includes(term));
  });

  readonly todayClinicAttendance = computed(() => {
    const todays = this.attendance().filter(a => this.isToday(a.recDate));

    const unique = new Map<string, Attendance>();

    for (const item of todays) {
      const key = `${item.consultId ?? ''}|${item.pNo ?? ''}`;
      if (!unique.has(key)) unique.set(key, item);
    }

    return Array.from(unique.values());
  });

  constructor() {
    this.load();
    effect(() => {
      this.dataSource.data = this.filteredConsultations();
    });
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading spa sessions...');

    Promise.all([
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.endpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise(),
      this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>().toPromise()
    ]).then(([patients, consultations, attendance, legacyPatients, retainerships]) => {
      this.patients.set(patients || []);
      this.consultations.set(consultations || []);
      this.attendance.set(attendance || []);
      this.legacyPatients.set(legacyPatients || []);
      this.retainerships.set(retainerships || []);
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

  openBilling(consultation: AestheticConsultation): void {
    const legacy = this.legacyPatients().find(x => x.pno === consultation.pNo);

    const dialogRef = this.dialog.open(BillingInvoiceDialogComponent, {
      data: {
        mode: 'create',
        consultId: consultation.consultId,
        billNo: consultation.consultId,
        pNo: consultation.pNo,
        coyID: legacy?.coyName ?? '',
        clientID: legacy?.coyName
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

  resolvePatientName(row: AestheticConsultation): string {
    if (row.patientName?.trim()) return row.patientName.trim();
    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName}`.trim() : `Patient #${row.patientId}`;
  }

  private resolvePatientLabel(row: AestheticConsultation): string {
    if (row.patientName?.trim()) {
      const patient = this.patients().find(x => x.id === row.patientId);
      return `${row.patientName} ${patient?.pno || ''}`;
    }
    const p = this.patients().find(x => x.id === row.patientId);
    return p ? `${p.firstName} ${p.lastName} ${p.pno || ''}` : `Patient #${row.patientId}`;
  }

  private async saveConsultation(result: SpaDialogResult): Promise<void> {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(result.consultation.id ? 'Updating spa session...' : 'Saving spa session...');

    try {
      const consultation = { ...result.consultation };

      // PatientId is resolved server-side from PNo (HPatients is source of truth).
      // Send pNo so the backend can look up / auto-create the AestheticPatient FK row.
      consultation.pNo = result.selectedPatient.pNo || consultation.pNo;
      consultation.consultId = result.selectedPatient.consultId || consultation.consultId;

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

      const matchedAesthetic = this.findAestheticPatient(firstName, lastName, pNo);

      return {
        patientId: matchedAesthetic?.id ?? 0,
        consultId: item.consultId ?? '',
        pNo,
        firstName,
        lastName,
        fullName: `${firstName} ${lastName}`.trim(),
        label: `${lastName} ${firstName} [${item.consultId ?? 'N/A'}]`,
        photo: legacy?.patPixBase64,
        dateOfBirth: matchedAesthetic?.dateOfBirth ?? legacy?.dob,
        company: this.getCompanyDisplayName(legacy),
        phoneNumber: matchedAesthetic?.phoneNumber ?? legacy?.pPhoneNo
      };
    });

    const unique = new Map<string, SpaPatientOption>();
    for (const item of options) {
      const key = (item.consultId ?? '').trim().toLowerCase();
      if (key && !unique.has(key)) {
        unique.set(key, item);
      }
    }

    return Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));
  }

  private findAestheticPatient(firstName: string, lastName: string, pNo: string): AestheticPatient | undefined {
    const normalizedFirst = firstName.trim().toLowerCase();
    const normalizedLast = lastName.trim().toLowerCase();
    const normalizedPNo = pNo.trim().toLowerCase();

    return this.patients().find(x =>
      (normalizedPNo && (x.pno ?? '').trim().toLowerCase() === normalizedPNo)
      || (x.firstName.trim().toLowerCase() === normalizedFirst
        && x.lastName.trim().toLowerCase() === normalizedLast));
  }

  private getCompanyDisplayName(patient?: HPatient): string | undefined {
    if (!patient) {
      return undefined;
    }

    const companyCode = patient.coyName?.trim();
    if (!companyCode) {
      return undefined;
    }

    const companyName = this.retainerships().find(x => x.retainId === companyCode)?.retainName?.trim();
    return companyName || companyCode;
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

