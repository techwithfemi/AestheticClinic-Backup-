import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { AccountEndpoint } from '../../../services/account-endpoint.service';
import { AestheticConsultation, AestheticPatient } from '../../../models/aesthetic.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { ProceduresEntryDialogComponent } from './procedures-entry-dialog.component';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-procedures',
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
    <div class="procedures-list-page">
      <div class="page-header">
        <div>
          <h2>Aesthetic Procedures</h2>
          <p class="subtitle">Manage procedure entries. Create and edit entries in a dedicated form dialog.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Procedures Entry
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [ngModel]="searchText()"
          (ngModelChange)="onSearchChanged($event ?? '')"
          placeholder="Search by patient name, PNO, consult ID, provider..." />

        <select class="date-filter" [ngModel]="dateFilter()" (ngModelChange)="onDateFilterChanged($event)">
          <option value="today">Today</option>
          <option value="all">All dates</option>
        </select>
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
                <button mat-icon-button type="button" (click)="openViewDialog(row)" title="View">
                  <mat-icon>visibility</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
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
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-row { margin-bottom: 12px; display: grid; grid-template-columns: 1fr 160px; gap: 10px; }
    .search-input { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; }
    .date-filter { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; background: #fff; }
    .data-table { width: 100%; }
    .truncate { max-width: 340px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .empty { color: #888; text-align: center; padding: 20px; }
    .pager-row { display: flex; justify-content: flex-end; align-items: center; gap: 10px; padding: 12px 0 4px; }
  `]
})
export class ProceduresComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);
  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly accountEndpoint = inject(AccountEndpoint);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly legacyPatients = signal<HPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly users = signal<User[]>([]);
  readonly searchText = signal('');
  readonly dateFilter = signal<'today' | 'all'>('today');
  readonly currentPageIndex = signal(0);
  readonly pageSize = 10;
  readonly displayedColumns = ['patient', 'consultId', 'date', 'provider', 'services', 'description', 'actions'];

  readonly filteredRows = computed(() => {
    const term = this.searchText().trim().toLowerCase();
    const isTodayOnly = this.dateFilter() === 'today';

    const baseRows = isTodayOnly
      ? this.consultations().filter(row => this.isToday(row.consultationDate))
      : this.consultations();

    if (!term) {
      return baseRows;
    }

    return baseRows.filter(row => {
      const patient = this.resolvePatientLabel(row).toLowerCase();
      const consultId = (row.consultId || '').toLowerCase();
      const pno = (row.pNo || '').toLowerCase();
      const provider = this.resolveProviderLabel(row).toLowerCase();
      const services = (row.services || '').toLowerCase();
      return patient.includes(term) || consultId.includes(term) || pno.includes(term) || provider.includes(term) || services.includes(term);
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

    const ref = this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab }
    });

    ref.afterClosed().subscribe(saved => {
      if (saved) {
        this.load();
      }
    });
  }

  openEditDialog(consultation: AestheticConsultation): void {
    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();

    const ref = this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab, consultation }
    });

    ref.afterClosed().subscribe(saved => {
      if (saved) {
        this.load();
      }
    });
  }

  openViewDialog(consultation: AestheticConsultation): void {
    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();

    this.dialog.open(ProceduresEntryDialogComponent, {
      width: '98vw',
      maxWidth: '1100px',
      disableClose: true,
      data: { initialTab, consultation }
    });
  }

  onSearchChanged(value: string): void {
    this.searchText.set(value || '');
    this.currentPageIndex.set(0);
  }

  onDateFilterChanged(value: 'today' | 'all'): void {
    this.dateFilter.set(value || 'today');
    this.currentPageIndex.set(0);
  }

  changePage(step: number): void {
    const next = this.currentPageIndex() + step;
    if (next < 0 || next >= this.totalPages()) {
      return;
    }

    this.currentPageIndex.set(next);
  }

  resolvePatientLabel(row: AestheticConsultation): string {
    const pno = (row.pNo || '').trim();

    const aesthetic = this.patients().find(p => p.id === row.patientId);
    if (aesthetic) {
      const name = `${aesthetic.firstName ?? ''} ${aesthetic.lastName ?? ''}`.trim();
      if (name) {
        return name;
      }
    }

    const legacy = this.legacyPatients().find(p => (p.pno || '').trim().toLowerCase() === pno.toLowerCase());
    if (legacy) {
      const name = `${legacy.pSurName ?? ''} ${legacy.pFirstname ?? ''}`.trim();
      if (name) {
        return name;
      }
    }

    return pno || `Patient #${row.patientId}`;
  }

  resolveProviderLabel(row: AestheticConsultation): string {
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

  private load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading procedures records...');

    Promise.all([
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise(),
      this.accountEndpoint.getUsersEndpoint<User[]>().toPromise()
    ]).then(([patients, legacyPatients, users]) => {
      const allPatients = patients || [];
      const rows = allPatients
        .flatMap(patient => (patient.consultations || []).map(c => ({ ...c, patientId: patient.id })))
        .sort((a, b) => (b.consultationDate || '').localeCompare(a.consultationDate || ''));

      this.patients.set(allPatients);
      this.legacyPatients.set(legacyPatients || []);
      this.users.set(users || []);
      this.consultations.set(rows);
      this.currentPageIndex.set(0);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load procedures records.', MessageSeverity.error, error);
    });
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

































