import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { NgSelectModule } from '@ng-select/ng-select';
import { forkJoin } from 'rxjs';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  RosterGroupAvailableStaffItem,
  RosterGroupDepartmentItem,
  RosterGroupEndpoint,
  RosterGroupItem,
  RosterGroupSaveRequest
} from '../../../services/roster-group-endpoint.service';

interface StaffGroupDialogData {
  rosterGroup: RosterGroupItem | null;
  departments: RosterGroupDepartmentItem[];
  availableStaff: RosterGroupAvailableStaffItem[];
}

const AllDepartmentValue = '__ALL__';

@Component({
  selector: 'app-staff-group-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    NgSelectModule
  ],
  template: `
    <div class="dialog-host">
      <div class="dialog-header" mat-dialog-title>
        <h2>Staff Group</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="cancel()" [disabled]="saving()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-content">
        <section class="header-card">
          <div class="header-card__title">Department</div>
          <div class="form-grid">
            <div class="field-block span-2">
              <ng-select
                [items]="data.departments"
                bindLabel="deptName"
                bindValue="deptId"
                [clearable]="false"
                [searchable]="true"
                [ngModel]="selectedDeptId()"
                (ngModelChange)="onDeptChanged($event)">
              </ng-select>
            </div>

            <div class="field-block span-2">
              <div class="field-label">Employees</div>
              <div class="listbox-actions">
                <button mat-button type="button" (click)="selectAllFiltered()" [disabled]="filteredEmployees().length === 0 || saving()">
                  Select All
                </button>
                <button mat-button type="button" (click)="clearAllFiltered()" [disabled]="filteredEmployees().length === 0 || saving()">
                  Deselect All
                </button>
              </div>
              <div class="checked-listbox" role="group" aria-label="Employees list">
                @for (item of filteredEmployees(); track item.empId) {
                  <mat-checkbox [checked]="isSelected(item.empId)" (change)="toggleEmployee(item.empId, $event.checked)" [disabled]="saving()">
                    {{ item.fullName }}
                  </mat-checkbox>
                }

                @if (noEmployeesFeedback()) {
                  <div class="empty-feedback">No employees found for the selected department.</div>
                }
              </div>
            </div>
          </div>
        </section>
      </mat-dialog-content>

      <mat-dialog-actions align="end" class="dialog-actions">
        <button mat-button type="button" (click)="cancel()" [disabled]="saving()">Cancel</button>
        <button mat-flat-button color="primary" type="button" (click)="save()" [disabled]="saving()">Save</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-host {
      width: min(920px, 95vw);
      max-width: 100%;
      box-sizing: border-box;
    }

    .dialog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-right: 0.5rem;
    }

    .close-btn {
      flex: 0 0 auto;
    }

    .dialog-content {
      display: block;
      overflow: visible;
      padding-top: 0.5rem;
      padding-bottom: 1rem;
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
      min-height: 160px;
      max-height: 280px;
      overflow-y: auto;
      padding: 10px 12px;
      display: grid;
      gap: 8px;
      background-color: #fff;
      align-content: start;
    }

    .empty-feedback {
      color: rgba(0, 0, 0, 0.65);
      font-size: 13px;
      font-style: italic;
      padding: 4px 0;
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

    .dialog-actions {
      padding: 8px 24px 16px;
    }

    :host ::ng-deep .ng-select {
      width: 100%;
    }

    :host ::ng-deep .ng-select .ng-select-container {
      min-height: 40px;
      border-radius: 4px;
      border-color: rgba(0, 0, 0, 0.24);
      box-shadow: none;
      background: transparent;
    }

    :host ::ng-deep .ng-select.ng-select-focused .ng-select-container {
      border-color: #1976d2;
      box-shadow: 0 0 0 1px rgba(25, 118, 210, 0.2);
    }

    @media (max-width: 767.98px) {
      .form-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class StaffGroupEntryDialogComponent implements OnInit {
  readonly data = inject<StaffGroupDialogData>(MAT_DIALOG_DATA);
  readonly dialogRef = inject(MatDialogRef<StaffGroupEntryDialogComponent, boolean>);

  private readonly endpoint = inject(RosterGroupEndpoint);
  private readonly alertService = inject(AlertService);

  readonly selectedDeptId = signal(this.resolveInitialDepartmentId());
  readonly selectedEmpIds = signal<string[]>([]);
  readonly saving = signal(false);
  readonly availableStaff = signal<RosterGroupAvailableStaffItem[]>([]);

  readonly filteredEmployees = computed(() => this.availableStaff());

  readonly noEmployeesFeedback = computed(() => !!this.selectedDeptId() && this.filteredEmployees().length === 0);

  ngOnInit(): void {
    this.loadStaffByDepartment(this.selectedDeptId());
  }

  onDeptChanged(value: string): void {
    this.selectedDeptId.set(value);
    this.selectedEmpIds.set([]);
    this.loadStaffByDepartment(value);
  }

  toggleEmployee(empId: string, checked: boolean): void {
    this.selectedEmpIds.update(ids => {
      const selected = new Set(ids);
      if (checked) {
        selected.add(empId);
      } else {
        selected.delete(empId);
      }

      return [...selected];
    });
  }

  isSelected(empId: string): boolean {
    return this.selectedEmpIds().includes(empId);
  }

  selectAllFiltered(): void {
    this.selectedEmpIds.set(this.filteredEmployees().map(x => x.empId));
  }

  clearAllFiltered(): void {
    this.selectedEmpIds.set([]);
  }

  cancel(): void {
    if (this.saving()) {
      return;
    }

    this.dialogRef.close(false);
  }

  save(): void {
    const available = this.filteredEmployees();
    if (available.length <= 0) {
      this.alertService.showMessage('Validation', 'No Staff is Available for Selection', MessageSeverity.warn);
      return;
    }

    const selectedIds = this.selectedEmpIds();
    if (selectedIds.length <= 0) {
      this.alertService.showMessage('Validation', 'No Staff is Selected', MessageSeverity.warn);
      return;
    }

    const selectedStaff = available.filter(x => selectedIds.includes(x.empId));
    if (selectedStaff.length <= 0) {
      this.alertService.showMessage('Validation', 'No Staff is Selected', MessageSeverity.warn);
      return;
    }

    const deptId = this.selectedDeptId();
    const requests = selectedStaff.map(staff => {
      const resolvedDeptId = deptId === AllDepartmentValue
        ? ((staff.deptId ?? '').trim() || this.data.departments[0]?.deptId || '')
        : deptId;

      const payload: RosterGroupSaveRequest = {
        deptId: resolvedDeptId,
        rosterGrpName: `${(staff.fullName || '').trim()}_Group`,
        exempted: 'NO',
        empIds: [staff.empId]
      };

      return this.endpoint.createEndpoint<RosterGroupItem>(payload);
    });

    this.saving.set(true);
    forkJoin(requests).subscribe({
      next: () => {
        this.saving.set(false);
        this.alertService.showMessage('Saved', 'Record Succesfully saved', MessageSeverity.success);
        this.dialogRef.close(true);
      },
      error: error => {
        this.saving.set(false);
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  private resolveInitialDepartmentId(): string {
    const deptFromRow = this.data.rosterGroup?.deptId;
    if (deptFromRow) {
      return deptFromRow;
    }

    const allOption = this.data.departments.find(x => (x.deptId ?? '').trim() === AllDepartmentValue || (x.deptName ?? '').trim() === '(ALL)');
    if (allOption) {
      return allOption.deptId;
    }

    return this.data.departments[0]?.deptId ?? '';
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

  private loadStaffByDepartment(deptId: string): void {
    this.endpoint.getAvailableStaffEndpoint<RosterGroupAvailableStaffItem[]>(deptId).subscribe({
      next: items => this.availableStaff.set(items),
      error: error => {
        this.availableStaff.set([]);
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }
}
