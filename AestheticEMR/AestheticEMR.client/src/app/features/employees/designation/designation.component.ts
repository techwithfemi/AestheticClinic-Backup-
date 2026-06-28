import { Component, OnInit, AfterViewInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { DesignationEndpoint } from '../../../services/designation-endpoint.service';
import { Designation } from '../../../models/employee.model';

// ─── Dialog Component ────────────────────────────────────────────────────────

export interface DesignationDialogData {
  designation: Designation | null;
}

@Component({
  selector: 'app-designation-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  template: `
    <div class="des-dialog">
      <div class="dialog-header">
        <h2 class="dialog-title mat-dialog-title">
          <mat-icon>{{ isNew ? 'add_circle' : 'edit' }}</mat-icon>
          {{ isNew ? 'New Designation' : 'Edit Designation' }}
        </h2>
        <button mat-icon-button (click)="close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div mat-dialog-content class="dialog-content">
        <!-- Name -->
        <div class="form-row">
          <mat-form-field appearance="outline" class="field-full">
            <mat-label>Designation Name *</mat-label>
            <input matInput [(ngModel)]="form.designationName" name="designationName" required maxlength="150" #nameField />
            @if (!form.designationName?.trim()) {
              <mat-error>Designation name is required.</mat-error>
            }
          </mat-form-field>
        </div>
      </div>

      <div mat-dialog-actions class="dialog-actions">
        <button mat-stroked-button (click)="close()" [disabled]="saving">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || loadingId || !form.designationName?.trim()">
          <mat-icon>save</mat-icon>
          {{ saving ? 'Saving...' : (loadingId ? 'Preparing...' : 'Save') }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .des-dialog { min-width: 340px; }
    .dialog-header { display: flex; align-items: center; justify-content: space-between; padding: 16px 16px 0; }
    .dialog-title { margin: 0; font-size: 1.1rem; display: flex; align-items: center; gap: 8px; }
    .close-btn { margin-left: auto; }
    .dialog-content { padding: 16px; }
    .form-row { margin-bottom: 12px; }
    .field-full { width: 100%; }
    mat-form-field { width: 100%; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; padding: 8px 16px 16px; }
    @media (max-width: 575.98px) {
      .des-dialog { min-width: unset; }
    }
  `]
})
export class DesignationDialogComponent {
  data: DesignationDialogData = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<DesignationDialogComponent>);
  private alertService = inject(AlertService);
  private endpoint = inject(DesignationEndpoint);

  isNew = !this.data.designation?.designationId;
  saving = false;
  /** True while the auto-generated id is still being fetched for a new designation. */
  loadingId = false;

  form: Designation = {
    designationId: this.data.designation?.designationId ?? '',
    designationName: this.data.designation?.designationName ?? '',
  };

  constructor() {
    if (this.isNew && !this.form.designationId) {
      this.loadingId = true;
      this.endpoint.generateIdEndpoint().subscribe({
        next: id => {
          this.form.designationId = id;
          this.loadingId = false;
        },
        error: err => {
          this.loadingId = false;
          // Surface the API error so the user understands why no id arrived
          // (e.g. "Designation id limit reached").
          this.alertService.showStickyMessage(
            'Cannot start a new designation',
            this.getApiError(err),
            MessageSeverity.error
          );
        }
      });
    }
  }

  save() {
    if (!this.form.designationName?.trim()) {
      this.alertService.showMessage('Validation', 'Designation name is required.', MessageSeverity.warn);
      return;
    }

    // For a new designation we need the auto-generated id before we can POST.
    // The Save button is disabled while loadingId is true, but defend against
    // keyboard / programmatic submit too.
    if (this.isNew && !this.form.designationId) {
      if (this.loadingId) {
        this.alertService.showMessage(
          'Please wait',
          'The designation id is still being generated.',
          MessageSeverity.warn
        );
      } else {
        this.alertService.showStickyMessage(
          'No designation id available',
          'Could not generate a designation id. Close the dialog and try again.',
          MessageSeverity.error
        );
      }
      return;
    }

    this.saving = true;
    const payload: Designation = {
      designationId: this.form.designationId,
      designationName: this.form.designationName.trim(),
    };

    const obs = this.isNew
      ? this.endpoint.createDesignationEndpoint(payload)
      : this.endpoint.updateDesignationEndpoint(this.form.designationId, payload);

    obs.subscribe({
      next: (saved: Designation) => {
        this.saving = false;
        this.dialogRef.close(saved);
      },
      error: (err: unknown) => {
        this.saving = false;
        const msg = this.getApiError(err);
        this.alertService.showStickyMessage('Save Error', msg, MessageSeverity.error);
      }
    });
  }

  private getApiError(err: unknown): string {
    const e = err as { error?: unknown; message?: string; status?: number; statusText?: string };
    if (e?.error) {
      const body = e.error;
      if (typeof body === 'string') return body;
      if (typeof body === 'object') {
        const b = body as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (b.detail) return `${b.title ?? 'Error'}: ${b.detail}`;
        if (b.message) return b.message;
        if (b.errors) {
          const flat = Object.entries(b.errors).map(([k, v]) => `${k}: ${(v ?? []).join(', ')}`).join('\n');
          if (flat) return flat;
        }
        try { return JSON.stringify(body); } catch { /* fall through */ }
      }
    }
    if (e?.status) return `${e.status} ${e.statusText ?? ''} - ${e.message ?? 'Request failed'}`.trim();
    return (err as { message?: string })?.message ?? 'An error occurred.';
  }

  close() { this.dialogRef.close(); }
}

// ─── List Page Component ──────────────────────────────────────────────────────

@Component({
  selector: 'app-designation',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatTooltipModule,
    TranslateModule,
  ],
  animations: [fadeInOut],
  template: `
    <div @fadeInOut class="des-page">
      <div class="page-header">
        <div>
          <h2>Designations</h2>
          <p class="subtitle">Manage job titles used to classify employees.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add_circle</mat-icon>
          Add Designation
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          (ngModelChange)="onSearchChange($event)"
          placeholder="Search by id or name..." />
      </div>

      <mat-card>
        @if (loadingIndicator) {
          <p class="empty">Loading...</p>
        } @else if (dataSource.data.length === 0 && !searchText) {
          <p class="empty">No designations found. Click <strong>Add Designation</strong> to create one.</p>
        } @else if (dataSource.data.length === 0) {
          <p class="empty">No designations match "<strong>{{ searchText }}</strong>".</p>
        } @else {
          <div class="table-container">
            <table mat-table [dataSource]="dataSource" class="des-table">
              <ng-container matColumnDef="designationName">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.designationName }}</td>
              </ng-container>
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let row" class="actions-cell">
                  <button mat-icon-button color="primary" (click)="openEditDialog(row)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button color="warn" (click)="deleteDesignation(row)" matTooltip="Delete" [disabled]="true">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>
          </div>
        }
        <!-- Paginator always rendered so @ViewChild resolves on first ngAfterViewInit -->
        <mat-paginator [class.hidden]="loadingIndicator || dataSource.data.length === 0" [pageSize]="10" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `,
  styles: [`
    .des-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; flex-wrap: wrap; gap: 12px; }
    .page-header h2 { margin: 0; }
    .subtitle { margin: 4px 0 0; color: #666; font-size: 0.9rem; }
    .search-row { margin-bottom: 16px; }
    .search-input { width: 100%; max-width: 500px; padding: 8px 12px; border: 1px solid #ccc; border-radius: 4px; font-size: 14px; box-sizing: border-box; }
    .table-container { overflow-x: auto; }
    .des-table { width: 100%; }
    .actions-cell { white-space: nowrap; }
    .actions-cell button[disabled] { opacity: 0.4; }
    .empty { padding: 24px; text-align: center; color: #888; }
    .hidden { display: none; }
    @media (max-width: 992px) { .des-page { padding: 16px; } }
    @media (max-width: 575.98px) { .des-page { padding: 12px; } .page-header { flex-direction: column; } }
  `]
})
export class DesignationComponent implements OnInit, AfterViewInit {
  private dialog = inject(MatDialog);
  private alertService = inject(AlertService);
  private endpoint = inject(DesignationEndpoint);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedColumns = ['designationName', 'actions'];
  dataSource = new MatTableDataSource<Designation>([]);
  rowsCache: Designation[] = [];
  searchText = '';
  loadingIndicator = false;

  ngOnInit() {
    this.loadData();
  }

  ngAfterViewInit() {
    // Hook paginator once the view is ready, even if data already loaded.
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
      this.dataSource.paginator.pageSize = 10;
    }
  }

  loadData() {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;
    this.endpoint.getDesignationsEndpoint().subscribe({
      next: designations => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.rowsCache = designations ?? [];
        this.dataSource.data = this.rowsCache;
        if (this.paginator) this.dataSource.paginator = this.paginator;
        if (this.dataSource.paginator) this.dataSource.paginator.pageSize = 10;
      },
      error: err => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to load designations.\r\nError: "${this.getErrorMessage(err)}"`,
          MessageSeverity.error
        );
      }
    });
  }

  onSearchChange(text: string) {
    const q = text.toLowerCase();
    this.dataSource.data = this.rowsCache.filter(d =>
      (d.designationId ?? '').toLowerCase().includes(q) ||
      (d.designationName ?? '').toLowerCase().includes(q)
    );
  }

  openAddDialog() {
    const ref = this.dialog.open(DesignationDialogComponent, {
      width: '480px',
      maxWidth: '98vw',
      disableClose: true, // dialog closes only via X icon or Cancel button
      data: { designation: null } as DesignationDialogData
    });
    ref.afterClosed().subscribe((result: Designation | undefined) => {
      if (result) {
        this.rowsCache = [result, ...this.rowsCache];
        this.dataSource.data = this.rowsCache;
        this.alertService.showMessage('Success', `Designation ${result.designationId} created.`, MessageSeverity.success);
      }
    });
  }

  openEditDialog(designation: Designation) {
    const ref = this.dialog.open(DesignationDialogComponent, {
      width: '480px',
      maxWidth: '98vw',
      disableClose: true,
      data: { designation: { ...designation } } as DesignationDialogData
    });
    ref.afterClosed().subscribe((result: Designation | undefined) => {
      if (result) {
        const idx = this.rowsCache.findIndex(d => d.designationId === result.designationId);
        if (idx !== -1) this.rowsCache[idx] = result;
        this.dataSource.data = [...this.rowsCache];
        this.alertService.showMessage('Success', `Designation ${result.designationId} updated.`, MessageSeverity.success);
      }
    });
  }

  deleteDesignation(designation: Designation) {
    // Delete is currently disabled in the UI (matches the Employee Info pattern).
    // Backend DELETE endpoint is kept intact in case it is re-enabled later.
    this.alertService.showMessage(
      'Delete disabled',
      `Delete is currently disabled. Contact your administrator to remove designation "${designation.designationName}".`,
      MessageSeverity.info
    );

    this.alertService.showDialog(
      `Delete designation "${designation.designationName}"?`,
      DialogType.confirm,
      () => {
        this.endpoint.deleteDesignationEndpoint(designation.designationId).subscribe({
          next: () => {
            this.rowsCache = this.rowsCache.filter(d => d.designationId !== designation.designationId);
            this.dataSource.data = [...this.rowsCache];
            this.alertService.showMessage('Deleted', `Designation ${designation.designationId} removed.`, MessageSeverity.success);
          },
          error: err => {
            this.alertService.showStickyMessage(
              'Delete Error',
              `Could not delete designation.\r\nError: "${this.getErrorMessage(err)}"`,
              MessageSeverity.error
            );
          }
        });
      }
    );
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (error instanceof Error) return error.message;
    return JSON.stringify(error);
  }
}