import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AestheticConsultation } from '../../../models/aesthetic.model';
import { VwhRecord } from '../../../models/legacy/vwh-record.model';
import { ProceduresEntryDialogComponent } from './procedures-entry-dialog.component';
import { BillingInvoiceDialogComponent, BillingInvoiceDialogData } from '../../billing/invoices/billing-invoice-dialog.component';
import { User } from '../../../models/user.model';
import { parseUtcDate } from '../../../shared/utils/utc-date.util';

interface ProcedureGridRow {
  record: VwhRecord;
  consultation?: AestheticConsultation;
  consultId: string;
  pNo: string;
  patientName: string;
  consultationDate?: string;
  provider?: string;
  services?: string;
  procedureDescription?: string;
}

@Component({
  selector: 'app-procedures',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule
  ],
  template: `
    <div class="procedures-list-page">
      <div class="page-header">
        <div>
          <h2>Aesthetic Procedures</h2>
          <p class="subtitle">Manage procedure entries. Create and edit entries in a dedicated form dialog.</p>
        </div>
      </div>

      <div class="page-controls-row">
        <input
          type="text"
          class="search-input"
          [ngModel]="searchText()"
          (ngModelChange)="onSearchChanged($event ?? '')"
          placeholder="Search by patient name, PNO, consult ID, provider..." />

        <mat-form-field appearance="outline" class="patient-select">
          <mat-label>Patient *</mat-label>
          <mat-select [value]="selectedVisitConsultId()" (selectionChange)="onPatientSelectionChanged($event.value)">
            <mat-option value="">Select Patient</mat-option>
            @for (item of patientAttendanceOptions(); track item.trackKey) {
              <mat-option [value]="item.consultId">{{ item.label }}</mat-option>
            }
          </mat-select>
        </mat-form-field>

        <button mat-raised-button color="primary" type="button" (click)="openAddDialog()">
          <mat-icon class="ui-icon ui-icon--sm ui-icon--inverse">add</mat-icon>
          Add / New
        </button>
      </div>

      <mat-card>
        @if (filteredRows().length === 0 && !loadingIndicator) {
          <p class="empty">No procedures records found.</p>
        } @else {
          <table mat-table [dataSource]="pagedRows()" class="data-table">
            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row) }}</td>
            </ng-container>

            <ng-container matColumnDef="consultId">
              <th mat-header-cell *matHeaderCellDef>Consult ID</th>
              <td mat-cell *matCellDef="let row">{{ row.consultId || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="date">
              <th mat-header-cell *matHeaderCellDef>Date</th>
              <td mat-cell *matCellDef="let row">{{ row.consultationDate | date:'dd-MMM-yyyy' }}</td>
            </ng-container>

            <ng-container matColumnDef="provider">
              <th mat-header-cell *matHeaderCellDef>Provider</th>
              <td mat-cell *matCellDef="let row">{{ resolveProviderLabel(row) }}</td>
            </ng-container>

            <ng-container matColumnDef="services">
              <th mat-header-cell *matHeaderCellDef>Services</th>
              <td mat-cell *matCellDef="let row" class="truncate">{{ row.services || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="description">
              <th mat-header-cell *matHeaderCellDef>Description</th>
              <td mat-cell *matCellDef="let row" class="truncate">{{ row.procedureDescription || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" class="ui-icon-btn ui-icon-btn--success" (click)="openAddBillDialog(row)" title="Add Bill">
                  <mat-icon class="ui-icon ui-icon--sm">receipt_long</mat-icon>
                </button>
                <button mat-icon-button type="button" class="ui-icon-btn ui-icon-btn--info" (click)="openViewDialog(row)" title="View">
                  <mat-icon class="ui-icon ui-icon--sm">visibility</mat-icon>
                </button>
                <button mat-icon-button type="button" class="ui-icon-btn ui-icon-btn--primary" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon class="ui-icon ui-icon--sm">edit</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
          </table>

          <div class="pager-row">
            <button mat-stroked-button type="button" (click)="changePage(-1)" [disabled]="currentPageIndex() <= 0">Prev</button>
            <span>Page {{ currentPageIndex() + 1 }} / {{ totalPages() }}</span>
            <button mat-stroked-button type="button" (click)="changePage(1)" [disabled]="currentPageIndex() + 1 >= totalPages()">Next</button>
          </div>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .procedures-list-page { padding: 20px; }
    .page-header { margin-bottom: 14px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .page-controls-row { display: grid; grid-template-columns: minmax(180px, 0.7fr) minmax(300px, 1fr) auto; gap: 10px; align-items: start; margin-bottom: 12px; }
    .patient-select { width: 100%; margin: 0; align-self: start; }
    .search-input { width: 100%; height: 56px; padding: 0 12px; border: 1px solid #ddd; border-radius: 6px; box-sizing: border-box; align-self: start; }
    .page-controls-row button { min-height: 56px; align-self: start; }
    .data-table { width: 100%; }
    .truncate { max-width: 340px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty { color: #888; text-align: center; padding: 20px; }
    .pager-row { display: flex; justify-content: flex-end; align-items: center; gap: 10px; padding: 12px 0 4px; }

    @media (max-width: 1100px) {
      .page-controls-row { grid-template-columns: 1fr 1fr; }
      .page-controls-row button { grid-column: 1 / -1; }
    }

    @media (max-width: 700px) {
      .page-controls-row { grid-template-columns: 1fr; }
      .page-controls-row button { grid-column: auto; }
    }
  `]
})
export class ProceduresComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);
  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);

  loadingIndicator = false;
  readonly vwhRecords = signal<VwhRecord[]>([]);
  readonly rows = signal<ProcedureGridRow[]>([]);
  readonly users = signal<User[]>([]);
  readonly searchText = signal('');
  readonly currentPageIndex = signal(0);
  readonly selectedVisitConsultId = signal('');
  readonly pageSize = 10;
  readonly displayedColumns = ['patient', 'consultId', 'date', 'provider', 'services', 'description', 'actions'];

  readonly patientAttendanceOptions = computed(() => {
    return this.vwhRecords()
      .filter(record => !!record.consultId?.trim() && !!record.pNo?.trim() && this.isToday(record.recDate))
      .map(record => ({
        trackKey: `${record.consultId}-${record.pNo}`,
        consultId: record.consultId,
        pNo: record.pNo,
        label: `${(record.fullname || 'Patient').trim()} ${record.recDate ? `(${this.formatVisitDate(record.recDate)}) ` : ''}[${record.consultId}]`
      }))
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  readonly filteredRows = computed(() => {
    const term = this.searchText().trim().toLowerCase();
    const todayRows = this.rows().filter(row => this.isToday(row.consultationDate));

    if (!term) {
      return todayRows;
    }

    return this.rows().filter(row => {
      const patient = this.resolvePatientLabel(row).toLowerCase();
      const consultId = (row.consultId || '').toLowerCase();
      const pno = (row.pNo || '').toLowerCase();
      const provider = this.resolveProviderLabel(row).toLowerCase();
      const services = (row.services || '').toLowerCase();
      const description = (row.procedureDescription || '').toLowerCase();
      return patient.includes(term)
        || consultId.includes(term)
        || pno.includes(term)
        || provider.includes(term)
        || services.includes(term)
        || description.includes(term);
    });
  });

  readonly totalPages = computed(() => Math.max(1, Math.ceil(this.filteredRows().length / this.pageSize)));

  readonly pagedRows = computed(() => {
    const pageIndex = this.currentPageIndex();
    const maxIndex = Math.max(0, this.totalPages() - 1);
    const safePageIndex = Math.min(pageIndex, maxIndex);
    const start = safePageIndex * this.pageSize;
    return this.filteredRows().slice(start, start + this.pageSize);
  });

  ngOnInit(): void {
    this.load();
  }

  openAddDialog(): void {
    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();
    const selectedVisitConsultId = this.selectedVisitConsultId().trim();

    if (!selectedVisitConsultId) {
      this.alertService.showStickyMessage('Validation error', 'Select a patient before adding a procedures entry.', MessageSeverity.warn);
      return;
    }

    const ref = this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab, selectedVisitConsultId }
    });

    ref.afterClosed().subscribe(saved => {
      if (saved) {
        this.load();
      }
    });
  }

  openEditDialog(row: ProcedureGridRow): void {
    if (!row.consultation) {
      this.alertService.showStickyMessage('Validation error', 'Selected procedures row is missing its consultation record.', MessageSeverity.warn);
      return;
    }

    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();

    const ref = this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab, consultation: row.consultation }
    });

    ref.afterClosed().subscribe(saved => {
      if (saved) {
        this.load();
      }
    });
  }

  openViewDialog(row: ProcedureGridRow): void {
    if (!row.consultation) {
      return;
    }

    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();

    this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab, consultation: row.consultation }
    });
  }

  openAddBillDialog(row: ProcedureGridRow): void {
    const consultId = row.consultId.trim();
    const pNo = row.pNo.trim();

    if (!consultId || !pNo || !row.consultation) {
      this.alertService.showStickyMessage('Validation error', 'Consult ID and matching consultation are required before adding a bill.', MessageSeverity.warn);
      return;
    }

    const dialogData = this.buildBillingDialogData(row);

    this.dialog.open(BillingInvoiceDialogComponent, {
      width: '1200px',
      maxWidth: '1200px',
      disableClose: true,
      data: dialogData
    }).afterClosed().subscribe(() => {
      this.load();
    });
  }

  onPatientSelectionChanged(consultId: string): void {
    this.selectedVisitConsultId.set((consultId || '').trim());
  }

  onSearchChanged(value: string): void {
    this.searchText.set(value || '');
    this.currentPageIndex.set(0);
  }

  changePage(step: number): void {
    const next = this.currentPageIndex() + step;
    if (next < 0 || next >= this.totalPages()) {
      return;
    }

    this.currentPageIndex.set(next);
  }

  resolvePatientLabel(row: ProcedureGridRow): string {
    return row.patientName || row.record.fullname || row.pNo || 'Patient';
  }

  resolveProviderLabel(row: ProcedureGridRow): string {
    const providerKey = (row.provider || '').trim();
    if (!providerKey) {
      return '—';
    }

    const provider = this.users().find(user =>
      (user.id || '').trim().toLowerCase() === providerKey.toLowerCase()
      || (user.userName || '').trim().toLowerCase() === providerKey.toLowerCase()
      || (user.empID || '').trim().toLowerCase() === providerKey.toLowerCase());

    if (!provider) {
      return providerKey;
    }

    return provider.fullName || provider.userName || provider.id;
  }

  private buildBillingDialogData(row: ProcedureGridRow): BillingInvoiceDialogData {
    const coyID = (row.record.coyname || row.record.retainName || '').trim();

    return {
      mode: 'create',
      consultId: row.consultId,
      billNo: row.consultId,
      pNo: row.pNo,
      coyID,
      company: coyID,
      clientID: coyID
    };
  }

  private formatVisitDate(value?: string): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return date.toLocaleDateString('en-US', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  private load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading procedures records...');

    Promise.all([
      this.endpoint.getVwhRecordsEndpoint<VwhRecord[]>().toPromise(),
      this.endpoint.getConsultationsEndpoint<AestheticConsultation[]>().toPromise(),
      this.accountEndpoint.getUsersEndpoint<User[]>().toPromise()
    ]).then(([records, consultations, users]) => {
      const allRecords = (records || []).filter(record => !!(record.consultId || '').trim());
      const consultationMap = new Map<string, AestheticConsultation>();

      (consultations || []).forEach(consultation => {
        const key = (consultation.consultId || '').trim().toLowerCase();
        if (key) {
          consultationMap.set(key, consultation);
        }
      });

      const rows = allRecords
        .map(record => {
          const consultKey = (record.consultId || '').trim().toLowerCase();
          const consultation = consultationMap.get(consultKey);
          if (!consultation) {
            return null;
          }

          return {
            record,
            consultation,
            consultId: (record.consultId || '').trim(),
            pNo: (record.pNo || '').trim(),
            patientName: (record.fullname || consultation.patientName || 'Patient').trim(),
            consultationDate: consultation.consultationDate || record.recDate,
            provider: consultation.provider || record.empId || '',
            services: consultation.services || record.remarks || '',
            procedureDescription: consultation.procedureDescription || record.diagnosis || record.remarks || ''
          } as ProcedureGridRow;
        })
        .filter((row): row is ProcedureGridRow => !!row)
        .sort((a, b) => (b.consultationDate || '').localeCompare(a.consultationDate || '') || b.consultId.localeCompare(a.consultId));

      this.vwhRecords.set(allRecords);
      this.users.set(users || []);
      this.rows.set(rows);
      this.selectedVisitConsultId.set(rows[0]?.consultId || '');
      this.currentPageIndex.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.vwhRecords.set([]);
      this.rows.set([]);
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load procedures records.', MessageSeverity.error, error);
    });
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const date = parseUtcDate(value) ?? new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }
}

































