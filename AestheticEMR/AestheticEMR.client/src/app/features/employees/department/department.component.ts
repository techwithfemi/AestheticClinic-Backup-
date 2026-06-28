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
import { DepartmentEndpoint } from '../../../services/department-endpoint.service';
import { Department } from '../../../models/employee.model';

// ─── Dialog Component ────────────────────────────────────────────────────────

export interface DepartmentDialogData {
  department: Department | null;
}

@Component({
  selector: 'app-department-dialog',
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
    <div class="dept-dialog">
      <div class="dialog-header">
        <h2 class="dialog-title mat-dialog-title">
          <mat-icon>{{ isNew ? 'add_circle' : 'edit' }}</mat-icon>
          {{ isNew ? 'New Department' : 'Edit Department' }}
        </h2>
        <button mat-icon-button (click)="close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div mat-dialog-content class="dialog-content">
        <!--
          Department ID is intentionally hidden in the dialog.
          For new rows the backend auto-generates it (legacy VB.NET genIDNo() behaviour);
          for edits it is the route segment / PK and must not be changed from the UI.
          Address and Location are also hidden in the dialog per latest spec
          (data still round-trips through the model; only the form fields are dropped).
        -->
        @if (!isNew) {
          <div class="form-row">
            <mat-form-field appearance="outline" class="field-full">
              <mat-label>Department ID</mat-label>
              <input matInput [value]="form.deptId" disabled readonly />
            </mat-form-field>
          </div>
        }

        <!-- Name -->
        <div class="form-row">
          <mat-form-field appearance="outline" class="field-full">
            <mat-label>Department Name *</mat-label>
            <input matInput [(ngModel)]="form.deptName" name="deptName" required maxlength="150" #nameField />
            @if (!form.deptName?.trim()) {
              <mat-error>Department name is required.</mat-error>
            }
          </mat-form-field>
        </div>
      </div>

      <div mat-dialog-actions class="dialog-actions">
        <button mat-stroked-button (click)="close()" [disabled]="saving">Cancel</button>
        <button mat-raised-button color="primary"
                (click)="save()"
                [disabled]="saving || loadingId || !form.deptName?.trim()">
          <mat-icon>save</mat-icon>
          {{ saving ? 'Saving...' : (loadingId ? 'Preparing...' : 'Save') }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .dept-dialog { min-width: 360px; }
    .dialog-header { display: flex; align-items: center; justify-content: space-between; padding: 16px 16px 0; }
    .dialog-title { margin: 0; font-size: 1.1rem; display: flex; align-items: center; gap: 8px; }
    .close-btn { margin-left: auto; }
    .dialog-content { padding: 16px; }
    .form-row { margin-bottom: 12px; }
    .field-full { width: 100%; }
    mat-form-field { width: 100%; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; padding: 8px 16px 16px; }
    @media (max-width: 575.98px) {
      .dept-dialog { min-width: unset; }
    }
  `]
})
export class DepartmentDialogComponent {
  data: DepartmentDialogData = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<DepartmentDialogComponent>);
  private alertService = inject(AlertService);
  private endpoint = inject(DepartmentEndpoint);

  isNew = !this.data.department?.deptId;
  saving = false;
  /** True while the auto-generated id is still being fetched for a new department. */
  loadingId = false;

  form: Department = {
    deptId: this.data.department?.deptId ?? '',
    deptName: this.data.department?.deptName ?? '',
  };

  constructor() {
    if (this.isNew && !this.form.deptId) {
      this.loadingId = true;
      this.endpoint.generateIdEndpoint().subscribe({
        next: id => {
          this.form.deptId = id;
          this.loadingId = false;
        },
        error: err => {
          this.loadingId = false;
          // Surface the API error so the user understands why no id arrived
          // (e.g. "Department id limit reached").
          this.alertService.showStickyMessage(
            'Cannot start a new department',
            this.getApiError(err),
            MessageSeverity.error
          );
        }
      });
    }
  }

  save() {
    if (!this.form.deptName?.trim()) {
      this.alertService.showMessage('Validation', 'Department name is required.', MessageSeverity.warn);
      return;
    }

    // For a new department we need the auto-generated id before we can POST.
    // The Save button is disabled while loadingId is true, but defend against
    // keyboard / programmatic submit too.
    if (this.isNew && !this.form.deptId) {
      if (this.loadingId) {
        this.alertService.showMessage(
          'Please wait',
          'The department id is still being generated.',
          MessageSeverity.warn
        );
      } else {
        this.alertService.showStickyMessage(
          'No department id available',
          'Could not generate a department id. Close the dialog and try again.',
          MessageSeverity.error
        );
      }
      return;
    }

    this.saving = true;
    // Address and Location are hidden in the dialog, so the form values are always empty.
    // Preserve any pre-existing address/location on edit; send null on new.
    const isEdit = !this.isNew;
    const existing = this.data.department;
    const payload: Department = {
      deptId: this.form.deptId,
      deptName: this.form.deptName.trim(),
      deptAddress: isEdit ? (existing?.deptAddress ?? undefined) : undefined,
      location: isEdit ? (existing?.location ?? undefined) : undefined,
    };

    const obs = this.isNew
      ? this.endpoint.createDepartmentEndpoint(payload)
      : this.endpoint.updateDepartmentEndpoint(this.form.deptId!, payload);

    obs.subscribe({
      next: (saved: Department) => {
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
  selector: 'app-department',
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
    <div @fadeInOut class="dept-page">
      <div class="page-header">
        <div>
          <h2>Departments</h2>
          <p class="subtitle">Manage organizational departments employees can be assigned to.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add_circle</mat-icon>
          Add Department
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          (ngModelChange)="onSearchChange($event)"
          placeholder="Search by name, address, or location..." />
      </div>

      <mat-card>
        @if (loadingIndicator) {
          <p class="empty">Loading...</p>
        } @else if (dataSource.data.length === 0 && !searchText) {
          <p class="empty">No departments found. Click <strong>Add Department</strong> to create one.</p>
        } @else if (dataSource.data.length === 0) {
          <p class="empty">No departments match "<strong>{{ searchText }}</strong>".</p>
        } @else {
          <div class="table-container">
            <table mat-table [dataSource]="dataSource" class="dept-table">
              <!--
                Department ID column is intentionally NOT shown in the list (per spec):
                "hide ID col in table grid". The id still travels in the row model so
                edit/delete actions keep working.
                Address and Location columns are also hidden per latest spec
                (data still round-trips through the model; only the grid columns are dropped).
              -->
              <ng-container matColumnDef="deptName">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.deptName }}</td>
              </ng-container>
              <ng-container matColumnDef="inUseCount">
                <th mat-header-cell *matHeaderCellDef class="num-col">In Use</th>
                <td mat-cell *matCellDef="let row" class="num-col">
                  <span [class.badge-warn]="(row.inUseCount ?? 0) > 0">{{ row.inUseCount ?? 0 }}</span>
                </td>
              </ng-container>
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let row" class="actions-cell">
                  <button mat-icon-button color="primary" (click)="openEditDialog(row)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <!--
                    Delete is disabled per spec ("disable delete icon under action col").
                    Backend DELETE endpoint is kept intact in case it is re-enabled later.
                  -->
                  <button mat-icon-button color="warn" (click)="deleteDepartment(row)" matTooltip="Delete disabled" [disabled]="true">
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
    .dept-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; flex-wrap: wrap; gap: 12px; }
    .page-header h2 { margin: 0; }
    .subtitle { margin: 4px 0 0; color: #666; font-size: 0.9rem; }
    .search-row { margin-bottom: 16px; }
    .search-input { width: 100%; max-width: 500px; padding: 8px 12px; border: 1px solid #ccc; border-radius: 4px; font-size: 14px; box-sizing: border-box; }
    .table-container { overflow-x: auto; }
    .dept-table { width: 100%; }
    .num-col { text-align: right; width: 90px; }
    .badge-warn { display: inline-block; min-width: 24px; padding: 2px 8px; border-radius: 10px; background: #fff4e5; color: #b26a00; font-weight: 600; font-size: 0.85rem; }
    .actions-cell { white-space: nowrap; }
    .actions-cell button[disabled] { opacity: 0.4; }
    .empty { padding: 24px; text-align: center; color: #888; }
    .hidden { display: none; }
    @media (max-width: 992px) { .dept-page { padding: 16px; } }
    @media (max-width: 575.98px) { .dept-page { padding: 12px; } .page-header { flex-direction: column; } }
  `]
})
export class DepartmentComponent implements OnInit, AfterViewInit {
  private dialog = inject(MatDialog);
  private alertService = inject(AlertService);
  private endpoint = inject(DepartmentEndpoint);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  /** No `deptId` column — the spec says "hide ID col in table grid". */
  displayedColumns = ['deptName', 'inUseCount', 'actions'];
  dataSource = new MatTableDataSource<Department>([]);
  rowsCache: Department[] = [];
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
    this.endpoint.getDepartmentsEndpoint().subscribe({
      next: departments => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.rowsCache = departments ?? [];
        this.dataSource.data = this.rowsCache;
        if (this.paginator) this.dataSource.paginator = this.paginator;
        if (this.dataSource.paginator) this.dataSource.paginator.pageSize = 10;
      },
      error: err => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to load departments.\r\nError: "${this.getErrorMessage(err)}"`,
          MessageSeverity.error
        );
      }
    });
  }

  onSearchChange(text: string) {
    const q = text.toLowerCase();
    this.dataSource.data = this.rowsCache.filter(d =>
      (d.deptId ?? '').toLowerCase().includes(q) ||
      (d.deptName ?? '').toLowerCase().includes(q)
    );
  }

  openAddDialog() {
    const ref = this.dialog.open(DepartmentDialogComponent, {
      width: '520px',
      maxWidth: '98vw',
      disableClose: true, // dialog closes only via X icon or Cancel button
      data: { department: null } as DepartmentDialogData
    });
    ref.afterClosed().subscribe((result: Department | undefined) => {
      if (result) {
        this.rowsCache = [result, ...this.rowsCache];
        this.dataSource.data = this.rowsCache;
        this.alertService.showMessage('Success', `Department ${result.deptId} created.`, MessageSeverity.success);
      }
    });
  }

  openEditDialog(department: Department) {
    const ref = this.dialog.open(DepartmentDialogComponent, {
      width: '520px',
      maxWidth: '98vw',
      disableClose: true,
      data: { department: { ...department } } as DepartmentDialogData
    });
    ref.afterClosed().subscribe((result: Department | undefined) => {
      if (result) {
        const idx = this.rowsCache.findIndex(d => d.deptId === result.deptId);
        if (idx !== -1) this.rowsCache[idx] = result;
        this.dataSource.data = [...this.rowsCache];
        this.alertService.showMessage('Success', `Department ${result.deptId} updated.`, MessageSeverity.success);
      }
    });
  }

  deleteDepartment(department: Department) {
    // Delete is currently disabled in the UI (matches the Employee Info pattern).
    // Backend DELETE endpoint is kept intact in case it is re-enabled later.
    this.alertService.showMessage(
      'Delete disabled',
      `Delete is currently disabled. Contact your administrator to remove department "${department.deptName}".`,
      MessageSeverity.info
    );

    this.alertService.showDialog(
      `Delete department "${department.deptName}"?`,
      DialogType.confirm,
      () => {
        this.endpoint.deleteDepartmentEndpoint(department.deptId!).subscribe({
          next: () => {
            this.rowsCache = this.rowsCache.filter(d => d.deptId !== department.deptId);
            this.dataSource.data = [...this.rowsCache];
            this.alertService.showMessage('Deleted', `Department ${department.deptId} removed.`, MessageSeverity.success);
          },
          error: err => {
            this.alertService.showStickyMessage(
              'Delete Error',
              `Could not delete department.\r\nError: "${this.getErrorMessage(err)}"`,
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
