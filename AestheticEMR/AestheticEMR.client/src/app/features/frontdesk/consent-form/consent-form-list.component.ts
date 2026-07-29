import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

import { AlertService, MessageSeverity, DialogType } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import {
  AestheticSignedConsent
} from '../../../models/aesthetic.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { ConsentFormEntryDialogComponent } from './consent-form-entry-dialog.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { formatUtcForDisplay, parseUtcDate } from '../../../shared/utils/utc-date.util';

@Component({
  selector: 'app-consent-form-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatTooltipModule,
    MatDialogModule,
    MatPaginatorModule,
    MatSlideToggleModule
  ],
  template: `
    <div class="page-shell">
      <div class="page-header">
        <div>
          <h2>Consent Forms</h2>
          <p class="subtitle">Manage patient consent records. Create new consent forms or view existing entries.</p>
        </div>
        <button mat-raised-button color="primary" type="button" (click)="openAddDialog()" [disabled]="loadingIndicator">
          <mat-icon>add</mat-icon>
          Add Consent Form
        </button>
      </div>

      <mat-card class="filters-card">
        <div class="filters-header">
          <mat-form-field appearance="outline" class="search-field">
            <mat-label>Search</mat-label>
            <mat-icon matPrefix>search</mat-icon>
            <input matInput [value]="searchText()" (input)="onSearchChanged($any($event.target).value)" 
              placeholder="Patient, procedure, signed by…" />
          </mat-form-field>

          <div class="filter-toggle">
            <span>Show today's records only</span>
            <mat-slide-toggle [checked]="todayOnly()" (change)="todayOnly.set($event.checked)"></mat-slide-toggle>
          </div>

          <div class="filter-toggle">
            <span>Show voided</span>
            <mat-slide-toggle [checked]="showVoided()" (change)="showVoided.set($event.checked)"></mat-slide-toggle>
          </div>

          <button mat-stroked-button type="button" (click)="refreshList()" [disabled]="loadingIndicator">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
        </div>

        @if (isDateFilterActive()) {
          <div class="filter-info">
            <mat-icon>event</mat-icon>
            <span>
              Showing records signed on <strong>{{ todayLabel() }}</strong>.
              @if (searchText()) {
                Search is also applied.
              } @else {
                Clear the search box to keep only today's records visible.
              }
            </span>
          </div>
        }
      </mat-card>

      <mat-card class="table-card">
        <div class="table-wrap">
          <table mat-table [dataSource]="pagedEntries()">
            <ng-container matColumnDef="consultId">
              <th mat-header-cell *matHeaderCellDef>Consult ID</th>
              <td mat-cell *matCellDef="let row">{{ row.consultId || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="patient">
              <th mat-header-cell *matHeaderCellDef>Patient</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientName(row.pNo) }}</td>
            </ng-container>

            <ng-container matColumnDef="procedureType">
              <th mat-header-cell *matHeaderCellDef>Procedure</th>
              <td mat-cell *matCellDef="let row">{{ row.procedureType || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="signedDate">
              <th mat-header-cell *matHeaderCellDef>Signed Date</th>
              <td mat-cell *matCellDef="let row">{{ formatSignedDate(row.signedDate) }}</td>
            </ng-container>

            <ng-container matColumnDef="signatureName">
              <th mat-header-cell *matHeaderCellDef>Signed By</th>
              <td mat-cell *matCellDef="let row">{{ row.signatureName || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="witnessedBy">
              <th mat-header-cell *matHeaderCellDef>Witness</th>
              <td mat-cell *matCellDef="let row">{{ row.witnessedBy || '—' }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let row">
                <span [class]="row.isVoided ? 'badge badge-voided' : 'badge badge-signed'">
                  {{ row.isVoided ? 'Voided' : 'Signed' }}
                </span>
              </td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                @if (row.pendingVoid) {
                  <button mat-stroked-button color="warn" type="button" (click)="undoDelete(row.id)" 
                    matTooltip="Undo void (5 sec timeout)">
                    Undo
                  </button>
                } @else {
                  <button mat-icon-button type="button" (click)="openEditDialog(row)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button type="button" (click)="deleteEntry(row)" matTooltip="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                }
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="tableColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: tableColumns;"></tr>

            @if (filteredEntries().length === 0) {
              <tr class="no-data-row">
                <td [attr.colspan]="tableColumns.length" class="no-data-cell">
                  {{ loadingIndicator ? 'Loading…' : 'No consent entries found.' }}
                </td>
              </tr>
            }
          </table>
        </div>

        @if (filteredEntries().length > 0) {
          <mat-paginator
            [length]="filteredEntries().length"
            [pageSize]="pageSize()"
            [pageSizeOptions]="[10, 25, 50]"
            (page)="onPageChanged($event)">
          </mat-paginator>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    .page-header { display: flex; align-items: flex-start; justify-content: space-between; gap: 16px; margin-bottom: 16px; flex-wrap: wrap; }
    .page-header > div { flex: 1; }
    .page-header h2 { margin: 0 0 4px 0; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }

    .filters-card { margin-bottom: 16px; }
    .filters-header { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; }
    .search-field { width: 320px; flex: 1; min-width: 240px; }
    .filter-toggle { display: flex; align-items: center; gap: 8px; }
    .filter-toggle span { font-size: 0.9rem; color: #666; }

    .table-card { }
    .table-wrap { overflow-x: auto; }
    table { width: 100%; }
    th { font-weight: 500; }
    td { padding: 12px; }
    .no-data-row td { text-align: center; padding: 24px; color: #999; }
    .no-data-cell { text-align: center; }

    .badge { display: inline-block; padding: 4px 12px; border-radius: 12px; font-size: 0.75rem; font-weight: 500; }
    .badge-signed { background: #e6f4ea; color: #1e7e34; }
    .badge-voided { background: #fce8e6; color: #c5221f; }

    .filter-info {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-top: 8px;
      padding: 8px 12px;
      background: #e3f2fd;
      border-radius: 6px;
      color: #0d47a1;
      font-size: 0.85rem;
    }
    .filter-info mat-icon { font-size: 18px; height: 18px; width: 18px; }

    @media (max-width: 992px) {
      .page-shell { padding: 16px; }
      .page-header { flex-direction: column; }
      .search-field { width: 100%; }
      .filters-header { flex-direction: column; }
    }

    @media (max-width: 767.98px) {
      .page-shell { padding: 12px; }
      .page-header button { width: 100%; }
      .filter-toggle { width: 100%; }
    }

    @media (max-width: 575.98px) {
      .page-shell { padding: 10px; }
      td, th { padding: 8px; font-size: 0.85rem; }
    }
  `]
})
export class ConsentFormListComponent implements OnInit {
  private readonly aestheticEndpoint = inject(AestheticEndpoint);
  private readonly hPatientEndpoint = inject(HPatientEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly entries = signal<AestheticSignedConsent[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly searchText = signal<string>('');
  readonly showVoided = signal<boolean>(false);
  readonly todayOnly = signal<boolean>(true);
  readonly pageIndex = signal<number>(0);
  readonly pageSize = signal<number>(10);

  readonly tableColumns = ['consultId', 'patient', 'procedureType', 'signedDate', 'signatureName', 'witnessedBy', 'status', 'actions'];

  private pendingVoidTimers = new Map<number, ReturnType<typeof setTimeout>>();

  readonly filteredEntries = computed(() => {
    const term = this.searchText().toLowerCase().trim();
    const includeVoided = this.showVoided();
    const hasSearch = term.length > 0;
    const filterByToday = this.todayOnly() && !hasSearch;
    const today = this.startOfToday();

    return this.entries()
      .filter(e => includeVoided || !e.isVoided)
      .filter(e => {
        if (filterByToday) {
          const signed = this.parseSignedDate(e.signedDate);
          if (!signed) return false;
          if (signed.getTime() < today) return false;
        }
        return true;
      })
      .filter(e => {
        if (!hasSearch) return true;
        return (
          (e.consultId || '').toLowerCase().includes(term) ||
          (e.procedureType || '').toLowerCase().includes(term) ||
          (e.signatureName || '').toLowerCase().includes(term) ||
          (e.witnessedBy || '').toLowerCase().includes(term) ||
          this.resolvePatientName(e.pNo).toLowerCase().includes(term)
        );
      });
  });

  readonly isDateFilterActive = computed(() => this.todayOnly() && !this.searchText().trim());

  readonly todayLabel = computed(() => {
    const d = new Date();
    return d.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' });
  });

  readonly pagedEntries = computed(() => {
    const start = this.pageIndex() * this.pageSize();
    return this.filteredEntries().slice(start, start + this.pageSize());
  });

  ngOnInit(): void {
    this.loadPatients();
    this.loadEntries();
  }

  private loadPatients(): void {
    this.hPatientEndpoint.getHPatientsEndpoint<HPatient[]>().subscribe({
      next: patients => this.patients.set(patients || []),
      error: () => this.patients.set([])
    });
  }

  loadEntries(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading consent entries...');

    this.aestheticEndpoint.getSignedConsentsEndpoint<AestheticSignedConsent[]>({ includeVoided: true }).subscribe({
      next: entries => {
        this.entries.set(entries || []);
        this.pageIndex.set(0);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', 'Unable to load consent entries.', MessageSeverity.error, error);
      }
    });
  }

  refreshList(): void {
    this.loadEntries();
  }

  onSearchChanged(value: string): void {
    this.searchText.set((value || '').trim());
    this.pageIndex.set(0);
  }

  private startOfToday(): number {
    const now = new Date();
    return new Date(now.getFullYear(), now.getMonth(), now.getDate()).getTime();
  }

  private parseSignedDate(value?: string): Date | null {
    if (!value) return null;
    let date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      const parts = (value || '').split('T');
      const datePart = parts[0] || value;
      const timePart = parts[1] ? parts[1].split('Z')[0] : '';
      date = new Date(datePart + (timePart ? 'T' + timePart : ''));
    }
    return Number.isNaN(date.getTime()) ? null : date;
  }

  onPageChanged(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(ConsentFormEntryDialogComponent, {
      width: '100%',
      maxWidth: '800px',
      disableClose: true,
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  openEditDialog(entry: AestheticSignedConsent): void {
    const dialogRef = this.dialog.open(ConsentFormEntryDialogComponent, {
      width: '100%',
      maxWidth: '800px',
      disableClose: true,
      data: { consentId: entry.id }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadEntries();
      }
    });
  }

  deleteEntry(entry: AestheticSignedConsent): void {
    if (!entry.id) return;

    const patientName = this.resolvePatientName(entry.pNo);
    const message = `This will void the consent for ${patientName}. You will have 5 seconds to undo.`;

    this.alertService.showDialog(message, DialogType.confirm, () => {
      const id = entry.id!;
      // Flag as pending locally
      this.entries.update(list =>
        list.map(e => e.id === id ? { ...e, pendingVoid: true } : e)
      );
      this.alertService.showMessage('Pending void', 'Consent will be voided in 5 seconds. Click Undo to cancel.', MessageSeverity.warn);

      const timer = setTimeout(() => {
        this.pendingVoidTimers.delete(id);
        this.aestheticEndpoint.voidConsentEndpoint<AestheticSignedConsent>(id, { voidReason: 'Voided from consent list' }).subscribe({
          next: () => {
            this.alertService.showMessage('Success', 'Consent voided.', MessageSeverity.success);
            this.loadEntries();
          },
          error: error => {
            this.alertService.showStickyMessage('Void Error', 'Unable to void consent.', MessageSeverity.error, error);
            this.loadEntries();
          }
        });
      }, 5000);

      this.pendingVoidTimers.set(id, timer);
    });
  }

  undoDelete(consentId: number): void {
    const timer = this.pendingVoidTimers.get(consentId);
    if (timer) {
      clearTimeout(timer);
      this.pendingVoidTimers.delete(consentId);
    }
    this.entries.update(list =>
      list.map(e => e.id === consentId ? { ...e, pendingVoid: false } : e)
    );
    this.alertService.showMessage('Undo', 'Void cancelled.', MessageSeverity.info);
  }

  resolvePatientName(pNo?: string): string {
    const normalized = (pNo || '').trim().toLowerCase();
    if (!normalized) return 'Unknown patient';

    const patient = this.patients().find(p => (p.pno || '').trim().toLowerCase() === normalized);
    if (!patient) return pNo || 'Unknown patient';

    return `${patient.pSurName || ''} ${patient.pFirstname || ''}`.trim() || (pNo || 'Unknown patient');
  }

  formatSignedDate(value?: string): string {
    return formatUtcForDisplay(value);
  }
}
