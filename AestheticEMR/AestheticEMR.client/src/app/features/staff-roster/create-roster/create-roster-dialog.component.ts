import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import {
  RosterEndpoint,
  RosterGridItem,
  RosterLookups,
  RosterSaveRequest,
} from '../../../services/roster-endpoint.service';

// One row in the flat checkbox list: one entry per day × shift combo (VB6 lstDays)
interface DayShiftItem {
  key: string;       // "date|shiftId"
  date: string;      // yyyy-MM-dd
  label: string;     // "14 Jul 2026  Morning [AM]  Monday"
  shiftId: number;
  shiftName: string;
  shiftAbbrv: string;
  dayName: string;
  selected: boolean;
}

export interface CreateRosterDialogData {
  lookups: RosterLookups;
  existingRow?: RosterGridItem | null;
}

@Component({
  selector: 'app-create-roster-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    NgSelectModule
  ],
  template: `
    <div class="dialog-host">
      <!-- Dialog header -->
      <div class="dialog-header" mat-dialog-title>
        <h2>{{ isEdit ? 'Edit Roster Entry' : 'New Roster Entry' }}</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="cancel()" [disabled]="saving()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-content">

        <!-- ── Controls row (VB6 header) ── -->
        <section class="controls-card">
          <div class="controls-row">

            <!-- Month -->
            <div class="ctrl-block">
              <div class="ctrl-label">Month</div>
              <mat-form-field appearance="outline" class="ctrl-field">
                <mat-select [(ngModel)]="selectedMonth" (selectionChange)="onPeriodChanged()">
                  @for (m of monthOptions; track m.value) {
                    <mat-option [value]="m.value">{{ m.label }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <!-- Year -->
            <div class="ctrl-block">
              <div class="ctrl-label">Year</div>
              <mat-form-field appearance="outline" class="ctrl-field">
                <mat-select [(ngModel)]="selectedYear" (selectionChange)="onPeriodChanged()">
                  @for (y of yearOptions; track y) {
                    <mat-option [value]="y">{{ y }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <!-- Roster Group (VB6 cboGroup) -->
            <div class="ctrl-block ctrl-block--group">
              <div class="ctrl-label">Roster Group</div>
              <ng-select
                class="dialog-ng-select"
                [items]="groups()"
                bindLabel="groupName"
                bindValue="groupId"
                [searchable]="true"
                [clearable]="true"
                [(ngModel)]="selectedGroupId"
                placeholder="Select group..."
                (ngModelChange)="onGroupChanged($event)"
                appendTo=".dialog-host">
              </ng-select>
              @if (selectedGroupLabel()) {
                <div class="dept-label">{{ selectedGroupLabel() }}</div>
              }
            </div>

          </div>
        </section>

        <!-- ── Two-panel layout ── -->
        <div class="panels">

          <!-- LEFT: Day-Shift checkbox list (VB6 lstDays) -->
          <section class="panel panel--left">
            <div class="panel__header">
              <span class="panel__title">Duty days: tick one box per day</span>
              <div class="panel__actions">
                <button mat-stroked-button color="primary" type="button" (click)="selectAllDays()"
                  [disabled]="saving() || listItems().length === 0">
                  <mat-icon>done_all</mat-icon> Select All
                </button>
                <button mat-stroked-button type="button" (click)="deselectAllDays()"
                  [disabled]="saving() || listItems().length === 0">
                  <mat-icon>remove_done</mat-icon> Deselect All
                </button>
              </div>
            </div>

            <div class="legend-row">
              <span>Selected: <strong>{{ selectedCount() }}</strong></span>
              <span>Total: <strong>{{ listItems().length }}</strong></span>
            </div>

            @if (loading()) {
              <div class="list-loading">
                <mat-progress-spinner diameter="24" mode="indeterminate"></mat-progress-spinner>
              </div>
            } @else if (listItems().length === 0) {
              <div class="list-empty">Select a Roster Group above to populate.</div>
            } @else {
              <div class="checkbox-list" role="group">
                @for (item of listItems(); track item.key; let i = $index) {
                  <div class="checkbox-row" [class.checked]="item.selected">
                    <mat-checkbox
                      [checked]="item.selected"
                      (change)="toggleItem(i, $event.checked)"
                      [disabled]="saving()">
                      {{ item.label }}
                    </mat-checkbox>
                  </div>
                }
              </div>
            }
          </section>

        </div>
      </mat-dialog-content>

      <!-- Dialog actions -->
      <mat-dialog-actions align="end" class="dialog-actions">
        <button mat-button type="button" (click)="cancel()" [disabled]="saving()">
          <mat-icon>cancel</mat-icon> Cancel
        </button>
        <button mat-flat-button color="primary" type="button" (click)="save()" [disabled]="saving() || loading()">
          @if (saving()) {
            <mat-progress-spinner diameter="18" mode="indeterminate"></mat-progress-spinner>
          } @else {
            <mat-icon>save</mat-icon>
          }
          {{ saving() ? 'Saving...' : 'Save' }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-host {
      width: min(760px, 96vw);
      max-width: 100%;
      box-sizing: border-box;
    }

    /* ── Header ─────────────────── */
    .dialog-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 12px 16px 8px;
      border-bottom: 1px solid rgba(0,0,0,0.08);
      h2 { margin: 0; font-size: 1.1rem; font-weight: 600; }
    }
    .close-btn { flex-shrink: 0; }

    /* ── Content ─────────────────── */
    .dialog-content {
      padding: 16px !important;
      max-height: 82vh;
      overflow-y: auto;
    }

    /* ── Controls card ───────────── */
    .controls-card {
      background: #f5f7fa;
      border: 1px solid rgba(0,0,0,0.08);
      border-radius: 6px;
      padding: 12px 16px;
      margin-bottom: 14px;
    }

    .controls-row {
      display: flex;
      flex-wrap: nowrap;
      gap: 12px;
      align-items: flex-start;
    }

    .ctrl-block {
      display: flex;
      flex-direction: column;
      gap: 3px;
      min-width: 110px;
      flex-shrink: 0;
    }

    .ctrl-block--group {
      flex: 1;
      min-width: 180px;
      flex-shrink: 1;
    }

    .ctrl-block--staff {
      flex: 1;
      min-width: 200px;
    }

    .ctrl-label {
      font-size: 0.74rem;
      font-weight: 500;
      color: rgba(0,0,0,0.55);
    }

    .ctrl-field {
      width: 100%;
      .mat-mdc-form-field-subscript-wrapper { display: none; }
    }

    .validation-error {
      color: #d32f2f;
      font-size: 0.73rem;
      margin-top: 2px;
    }

    /* ── Two-panel layout ────────── */
    .panels {
      display: block;
    }

    .panel {
      width: 95%;
      margin: 0 auto;
      border: 1px solid rgba(0,0,0,0.1);
      border-radius: 6px;
      overflow: hidden;

      &__header {
        display: flex;
        align-items: center;
        flex-wrap: wrap;
        gap: 6px;
        padding: 8px 10px;
        background: #f5f7fa;
        border-bottom: 1px solid rgba(0,0,0,0.08);
      }

      &__title {
        font-size: 0.8rem;
        font-weight: 600;
        flex: 1;
      }

      &__actions {
        display: flex;
        flex-wrap: wrap;
        gap: 5px;
      }
    }

    /* ── Checkbox list ───────────── */
    .legend-row {
      display: flex;
      gap: 16px;
      padding: 5px 12px;
      font-size: 0.8rem;
      color: rgba(0,0,0,0.6);
      border-bottom: 1px solid rgba(0,0,0,0.05);
      background: #fafafa;
    }

    .list-loading, .list-empty {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: 100px;
      color: rgba(0,0,0,0.4);
      font-size: 0.85rem;
      padding: 12px;
    }

    .checkbox-list {
      max-height: 400px;
      overflow-y: auto;
      padding: 3px 0;
    }

    .checkbox-row {
      padding: 2px 12px;
      transition: background 0.1s;
      &:hover { background: rgba(0,0,0,0.03); }
      &.checked { background: rgba(25, 118, 210, 0.06); }
      mat-checkbox { font-size: 0.81rem; }
    }

    /* ── Actions ─────────────────── */
    .dialog-actions {
      padding: 8px 16px;
      border-top: 1px solid rgba(0,0,0,0.08);
      gap: 8px;
    }

    .dialog-ng-select {
      --ng-select-border: 1px solid rgba(0,0,0,0.23);
    }

    .dept-label {
      margin-top: 6px;
      font-size: 0.82rem;
      font-weight: 600;
      color: #1565c0;
      letter-spacing: 0.01em;
    }

    /* ── Responsive ──────────────── */
    @media (max-width: 599px) {
      .controls-row { flex-wrap: wrap; }
      .ctrl-block { min-width: 100px; }
    }
  `]
})
export class CreateRosterDialogComponent {
  readonly dialogRef = inject(MatDialogRef<CreateRosterDialogComponent>);
  readonly data = inject<CreateRosterDialogData>(MAT_DIALOG_DATA);
  private readonly alertService = inject(AlertService);
  private readonly rosterEndpoint = inject(RosterEndpoint);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly listItems = signal<DayShiftItem[]>([]);
  readonly selectedGroupLabel = signal<string | null>(null);

  // Month / Year / Group — default to today
  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();
  selectedGroupId: number | null = null;
  private selectedGroupDeptId: string | null = null;

  readonly isEdit = !!this.data.existingRow;

  readonly monthOptions = Array.from({ length: 12 }, (_, i) => ({
    value: i + 1,
    label: new Date(2000, i, 1).toLocaleString('en', { month: 'long' })
  }));
  readonly yearOptions = Array.from({ length: 3 }, (_, i) => new Date().getFullYear() + i - 1);

  readonly selectedCount = computed(() => this.listItems().filter(i => i.selected).length);
  readonly groups = computed(() => this.data.lookups.groups);

  // VB6: cboGroup_Click — group selection triggers list + grid refresh
  onGroupChanged(groupId: number | null): void {
    this.selectedGroupId = groupId;
    if (!groupId) {
      this.selectedGroupDeptId = null;
      this.selectedGroupLabel.set(null);
      this.listItems.set([]);
      return;
    }
    const group = this.data.lookups.groups.find(g => g.groupId === groupId) ?? null;
    this.selectedGroupDeptId = group?.deptId ?? null;
    this.selectedGroupLabel.set(
      group?.deptId ? `${group.deptName ?? ''} [${group.deptId}]` : null
    );
    this.buildListItems();
  }

  // Month/Year change — refresh if a group is already selected
  onPeriodChanged(): void {
    if (this.selectedGroupId) {
      this.buildListItems();
    }
  }

  // VB6: loadListBoxAll — one row per day × shift, filtered by selected group's DeptId
  buildListItems(): void {
    const month = this.selectedMonth;
    const year = this.selectedYear;
    const totalDays = new Date(year, month, 0).getDate();

    // Filter shifts to only those matching the selected group's DeptId
    const deptId = this.selectedGroupDeptId;
    const shifts = deptId
      ? this.data.lookups.shifts.filter(s => s.deptId === deptId)
      : this.data.lookups.shifts;

    const items: DayShiftItem[] = [];
    for (let d = 1; d <= totalDays; d++) {
      const date = new Date(year, month - 1, d);
      const dateStr = this.formatDate(date);
      const dayName = date.toLocaleDateString('en-US', { weekday: 'long' });
      const dateLabel = date.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' });

      for (const shift of shifts) {
        items.push({
          key: `${dateStr}|${shift.sno}`,
          date: dateStr,
          label: `${dateLabel}  ${shift.shiftName} [${shift.evalTo}]  ${dayName}`,
          shiftId: shift.sno,
          shiftName: shift.shiftName,
          shiftAbbrv: shift.evalTo,
          dayName,
          selected: false
        });
      }
    }

    this.listItems.set(items);
  }

  selectAllDays(): void {
    this.listItems.update(items => {
      const selectedDates = new Set<string>();
      return items.map(i => {
        if (selectedDates.has(i.date)) {
          return { ...i, selected: false };
        }

        selectedDates.add(i.date);
        return { ...i, selected: true };
      });
    });
  }

  deselectAllDays(): void {
    this.listItems.update(items => items.map(i => ({ ...i, selected: false })));
  }

  toggleItem(index: number, selected: boolean): void {
    this.listItems.update(items => {
      const target = items[index];
      if (!target) {
        return items;
      }

      return items.map((i, idx) => {
        if (idx === index) {
          return { ...i, selected };
        }

        if (selected && i.date === target.date) {
          return { ...i, selected: false };
        }

        return i;
      });
    });
  }

  save(): void {
    if (!this.selectedGroupId) {
      this.alertService.showMessage('Validation', 'Please select a Roster Group.', MessageSeverity.warn);
      return;
    }

    const group = this.data.lookups.groups.find(g => g.groupId === this.selectedGroupId);
    if (!group) {
      this.alertService.showMessage('Validation', 'Selected roster group was not found.', MessageSeverity.warn);
      return;
    }

    const deptId = group.deptId?.trim();
    if (!deptId) {
      this.alertService.showMessage('Validation', 'Selected roster group must have a Department.', MessageSeverity.warn);
      return;
    }

    const groupName = group.groupName?.trim();
    if (!groupName) {
      this.alertService.showMessage('Validation', 'Selected roster group must have a Group Name.', MessageSeverity.warn);
      return;
    }

    const selectedItems = this.listItems().filter(i => i.selected);

    const selectedByDate = new Map<string, number>();
    for (const item of selectedItems) {
      selectedByDate.set(item.date, (selectedByDate.get(item.date) ?? 0) + 1);
    }

    const hasMultipleForSameDay = Array.from(selectedByDate.values()).some(v => v > 1);
    if (hasMultipleForSameDay) {
      this.alertService.showMessage('Validation', 'Tick only one box per day.', MessageSeverity.warn);
      return;
    }

    const invalidSelection = selectedItems.find(i => !i.date || i.shiftId <= 0 || !i.shiftAbbrv?.trim() || !i.shiftName?.trim());
    if (invalidSelection) {
      this.alertService.showMessage('Validation', 'Each selected day must have explicit date and shift values.', MessageSeverity.warn);
      return;
    }

    const selectedDays = selectedItems
      .map(i => ({
        date: i.date,
        shiftId: i.shiftId,
        shiftAbbrv: i.shiftAbbrv.trim(),
        shiftName: i.shiftName.trim()
      }));

    if (selectedDays.length === 0) {
      this.alertService.showMessage('Validation', 'No Roster List is Selected', MessageSeverity.warn);
      return;
    }

    this.alertService.showDialog(
      `Are you sure to save Record for ${groupName}?`,
      DialogType.confirm,
      () => this.commitSave({
        deptId,
        groupId: group.groupId,
        groupName,
        selectedDays
      })
    );
  }

  private commitSave(payload: RosterSaveRequest): void {
    this.saving.set(true);
    this.rosterEndpoint.saveRosterEndpoint(payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.alertService.showMessage('Saved', 'Roster saved successfully.', MessageSeverity.success);
        this.dialogRef.close(true);
      },
      error: error => {
        this.saving.set(false);
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  private formatDate(date: Date): string {
    const y = date.getFullYear();
    const m = `${date.getMonth() + 1}`.padStart(2, '0');
    const d = `${date.getDate()}`.padStart(2, '0');
    return `${y}-${m}-${d}`;
  }

  private getErrorMessage(error: unknown): string {
    const e = error as { error?: unknown; message?: string; statusText?: string; status?: number };
    if (e?.error) {
      if (typeof e.error === 'string') return e.error;
      if (typeof e.error === 'object') {
        const body = e.error as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (body.detail) return `${body.title ?? 'Error'}: ${body.detail}`;
        if (body.message) return body.message;
        if (body.errors) return Object.entries(body.errors).map(([k, v]) => `${k}: ${(v ?? []).join(', ')}`).join('\n');
      }
    }
    if (e?.status) return `${e.status} ${e.statusText ?? ''}`.trim();
    return e?.message ?? 'An error occurred.';
  }
}
