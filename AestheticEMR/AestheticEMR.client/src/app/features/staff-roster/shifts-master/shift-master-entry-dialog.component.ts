import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  DepartmentLookup,
  ShiftMasterDetail,
  ShiftMasterEndpoint
} from '../../../services/shift-master-endpoint.service';

interface ShiftMasterDialogData {
  departments: DepartmentLookup[];
  shift: ShiftMasterDetail | null;
}

@Component({
  selector: 'app-shift-master-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatCheckboxModule
  ],
  template: `
    <div class="dialog-host">
      <div class="dialog-header" mat-dialog-title>
        <h2>{{ isEdit ? 'Edit Shift Master' : 'New Shift Master' }}</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="cancel()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-content">
        <section class="header-card">
          <div class="header-card__title">Shift Master Entry</div>
          <div class="form-grid">
            <mat-form-field appearance="outline" class="span-2">
              <mat-label>Shift Name</mat-label>
              <input matInput [(ngModel)]="shiftName" maxlength="200" />
            </mat-form-field>

            <div class="field-block span-2">
              <div class="field-label">Assigned Departments</div>
              <div class="listbox-actions">
                <button mat-button type="button" (click)="selectAllDepartments()" [disabled]="data.departments.length === 0">
                  Select All
                </button>
                <button mat-button type="button" (click)="clearAllDepartments()" [disabled]="deptIds.length === 0">
                  Deselect All
                </button>
              </div>
              <div class="checked-listbox" role="group" aria-label="Assigned Departments">
                @for (department of data.departments; track department.deptId) {
                  <mat-checkbox
                    class="department-checkbox"
                    [ngModel]="isDepartmentSelected(department.deptId)"
                    [ngModelOptions]="{ standalone: true }"
                    (ngModelChange)="toggleDepartment(department.deptId, $event)">
                    {{ department.deptName }}
                  </mat-checkbox>
                }
              </div>
            </div>
          </div>
        </section>
      </mat-dialog-content>

      <mat-dialog-actions align="end" class="dialog-actions">
        <button mat-button type="button" (click)="cancel()" [disabled]="saving">Cancel</button>
        <button mat-flat-button color="primary" type="button" (click)="save()" [disabled]="saving">
          @if (saving) {
            <mat-spinner diameter="18"></mat-spinner>
          } @else {
            <mat-icon>{{ isEdit ? 'save_as' : 'save' }}</mat-icon>
          }
          {{ saving ? 'Saving...' : (isEdit ? 'Update' : 'Save') }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-host {
      width: min(820px, 95vw);
      max-width: 100%;
      box-sizing: border-box;
    }

    .dialog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-right: 0.5rem;
    }

    .dialog-content {
      display: block;
      overflow: visible;
      padding-top: 0.5rem;
    }

    .header-card {
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 10px;
      padding: 12px;
    }

    .header-card__title {
      font-weight: 600;
      margin-bottom: 12px;
    }

    .form-grid {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 12px;
    }

    .field-block {
      display: flex;
      flex-direction: column;
      gap: 6px;
    }

    .field-label {
      font-size: 12px;
      color: rgba(0, 0, 0, 0.65);
    }

    .span-2 {
      grid-column: 1 / -1;
    }

    .checked-listbox {
      border: 1px solid rgba(0, 0, 0, 0.24);
      border-radius: 4px;
      min-height: 140px;
      max-height: 240px;
      overflow-y: auto;
      padding: 10px 12px;
      display: grid;
      gap: 8px;
      background-color: #fff;
    }

    .listbox-actions {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
      margin-bottom: 4px;
    }

    .listbox-actions button {
      min-width: 0;
      padding-inline: 8px;
      line-height: 30px;
    }

    .department-checkbox {
      width: 100%;
    }

    .dialog-actions {
      padding: 8px 24px 16px;
    }

    @media (max-width: 767.98px) {
      .form-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class ShiftMasterEntryDialogComponent {
  private readonly endpoint = inject(ShiftMasterEndpoint);
  private readonly alertService = inject(AlertService);
  readonly dialogRef = inject(MatDialogRef<ShiftMasterEntryDialogComponent, boolean>);
  readonly data = inject<ShiftMasterDialogData>(MAT_DIALOG_DATA);

  saving = false;
  readonly isEdit = !!this.data.shift;

  shiftName = this.data.shift?.shiftName ?? '';
  deptIds = [...(this.data.shift?.deptIds ?? [])].map(id => String(id));

  isDepartmentSelected(deptId: string | number): boolean {
    const normalizedDeptId = String(deptId);
    return this.deptIds.includes(normalizedDeptId);
  }

  toggleDepartment(deptId: string | number, checked: boolean): void {
    const normalizedDeptId = String(deptId);

    if (checked) {
      if (!this.deptIds.includes(normalizedDeptId)) {
        this.deptIds = [...this.deptIds, normalizedDeptId];
      }
      return;
    }

    this.deptIds = this.deptIds.filter(id => id !== normalizedDeptId);
  }

  selectAllDepartments(): void {
    this.deptIds = this.data.departments.map(department => String(department.deptId));
  }

  clearAllDepartments(): void {
    this.deptIds = [];
  }

  cancel(): void {
    if (this.saving) {
      return;
    }

    this.dialogRef.close(false);
  }

  save(): void {
    if (!this.shiftName.trim()) {
      this.alertService.showMessage('Validation', 'Shift name is required.', MessageSeverity.warn);
      return;
    }

    if (this.deptIds.length === 0) {
      this.alertService.showMessage('Validation', 'Select at least one department.', MessageSeverity.warn);
      return;
    }

    const payload: ShiftMasterDetail = {
      shiftId: this.data.shift?.shiftId ?? 0,
      shiftName: this.shiftName.trim(),
      deptIds: this.deptIds
    };

    this.saving = true;

    const request = this.isEdit
      ? this.endpoint.updateEndpoint<ShiftMasterDetail>(payload.shiftId, payload)
      : this.endpoint.createEndpoint<ShiftMasterDetail>(payload);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.alertService.showMessage('Saved', 'Shift master saved successfully.', MessageSeverity.success);
        this.dialogRef.close(true);
      },
      error: error => {
        this.saving = false;
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  private getErrorMessage(error: unknown): string {
    const e = error as { error?: unknown; message?: string; statusText?: string; status?: number };
    if (e?.error) {
      if (typeof e.error === 'string') {
        return e.error;
      }

      if (typeof e.error === 'object') {
        const body = e.error as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (body.detail) {
          return `${body.title ?? 'Error'}: ${body.detail}`;
        }
        if (body.message) {
          return body.message;
        }
        if (body.errors) {
          return Object.entries(body.errors).map(([key, value]) => `${key}: ${(value ?? []).join(', ')}`).join('\n');
        }
      }
    }

    if (e?.status) {
      return `${e.status} ${e.statusText ?? ''}`.trim();
    }

    return e?.message ?? 'An error occurred.';
  }
}
