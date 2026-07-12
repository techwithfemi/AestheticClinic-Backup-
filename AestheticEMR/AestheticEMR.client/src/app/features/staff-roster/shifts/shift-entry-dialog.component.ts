import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { ModuleSettingsService } from '../../../services/module-settings.service';
import { ShiftDetail, ShiftDetailsEndpoint, ShiftLookup } from '../../../services/shift-details-endpoint.service';

interface StaffRosterSettings {
  periodOfDayOptions: string[];
}

interface ShiftEntryDialogData {
  lookups: ShiftLookup[];
  shift: ShiftDetail | null;
}

interface ShiftDialogModel {
  shiftId: number | null;
  shiftJob: string;
  periodOfDay: string;
  resumptionTime: string;
  closingTime: string;
  punctualityRemarks: string;
  lateRemarks: string;
  normalClosingRemarks: string;
  abnormalClosingRemarks: string;
  evalTo: string;
}

@Component({
  selector: 'app-shift-entry-dialog',
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
    MatSelectModule,
    NgSelectModule
  ],
  template: `
    <div class="dialog-host">
      <div class="dialog-header" mat-dialog-title>
        <h2>{{ isEdit ? 'Edit Shift' : 'New Shift' }}</h2>
        <button mat-icon-button type="button" class="close-btn" (click)="cancel()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <form #shiftForm="ngForm" novalidate>
        <mat-dialog-content class="dialog-content">
          <section class="header-card">
            <div class="header-card__title">Shift Entry</div>
            <div class="form-grid">
              <div class="field-block span-2">
                <div class="field-label">Shift</div>
                <ng-select
                  [items]="data.lookups"
                  bindLabel="shiftJob"
                  bindValue="shiftId"
                  [searchable]="true"
                  [clearable]="false"
                  [(ngModel)]="model.shiftId"
                  name="shiftId"
                  required
                  (ngModelChange)="onShiftChanged($event)"
                  [placeholder]="'Select shift'"
                  appendTo=".dialog-host">
                  <ng-template ng-option-tmp let-item="item">
                    <div class="option-row">
                      <span>{{ item.shiftJob }}</span>
                    </div>
                  </ng-template>
                </ng-select>
                @if (submitted && !model.shiftId) {
                  <div class="validation-error">Shift is required</div>
                }
              </div>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Period of Day</mat-label>
                <mat-select [(ngModel)]="model.periodOfDay" name="periodOfDay" required #periodOfDay="ngModel">
                  @for (option of periodOfDayOptions(); track option) {
                    <mat-option [value]="option">{{ option }}</mat-option>
                  }
                </mat-select>
                @if ((periodOfDay.invalid && (periodOfDay.touched || submitted))) {
                  <mat-error>Period of Day is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Period/Abbreviation</mat-label>
                <input matInput [(ngModel)]="model.evalTo" name="evalTo" required #evalTo="ngModel" />
                @if (evalTo.invalid && (evalTo.touched || submitted)) {
                  <mat-error>Period/Abbreviation is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Resumption Time</mat-label>
                <input #resumptionTimeInput matInput type="time" step="60" [(ngModel)]="model.resumptionTime" name="resumptionTime" required #resumptionTime="ngModel" />
                <button
                  mat-icon-button
                  matSuffix
                  type="button"
                  class="time-toggle"
                  aria-label="Open time picker"
                  (click)="openTimePicker(resumptionTimeInput)">
                  <mat-icon>schedule</mat-icon>
                </button>
                @if (isEdit && model.resumptionTime) {
                  <mat-hint class="time-hint">{{ to12HourTime(model.resumptionTime) }}</mat-hint>
                }
                @if (resumptionTime.invalid && (resumptionTime.touched || submitted)) {
                  <mat-error>Resumption Time is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Closing Time</mat-label>
                <input #closingTimeInput matInput type="time" step="60" [(ngModel)]="model.closingTime" name="closingTime" required #closingTime="ngModel" />
                <button
                  mat-icon-button
                  matSuffix
                  type="button"
                  class="time-toggle"
                  aria-label="Open time picker"
                  (click)="openTimePicker(closingTimeInput)">
                  <mat-icon>schedule</mat-icon>
                </button>
                @if (isEdit && model.closingTime) {
                  <mat-hint class="time-hint">{{ to12HourTime(model.closingTime) }}</mat-hint>
                }
                @if (closingTime.invalid && (closingTime.touched || submitted)) {
                  <mat-error>Closing Time is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Punctuality Remarks</mat-label>
                <input matInput [(ngModel)]="model.punctualityRemarks" name="punctualityRemarks" required #punctualityRemarks="ngModel" />
                @if (punctualityRemarks.invalid && (punctualityRemarks.touched || submitted)) {
                  <mat-error>Punctuality Remarks is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Late Remarks</mat-label>
                <input matInput [(ngModel)]="model.lateRemarks" name="lateRemarks" required #lateRemarks="ngModel" />
                @if (lateRemarks.invalid && (lateRemarks.touched || submitted)) {
                  <mat-error>Late Remarks is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Normal Closing Remarks</mat-label>
                <input matInput [(ngModel)]="model.normalClosingRemarks" name="normalClosingRemarks" required #normalClosingRemarks="ngModel" />
                @if (normalClosingRemarks.invalid && (normalClosingRemarks.touched || submitted)) {
                  <mat-error>Normal Closing Remarks is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline" class="form-field">
                <mat-label>Abnormal Closing Remarks</mat-label>
                <input matInput [(ngModel)]="model.abnormalClosingRemarks" name="abnormalClosingRemarks" required #abnormalClosingRemarks="ngModel" />
                @if (abnormalClosingRemarks.invalid && (abnormalClosingRemarks.touched || submitted)) {
                  <mat-error>Abnormal Closing Remarks is required</mat-error>
                }
              </mat-form-field>
            </div>
          </section>
        </mat-dialog-content>

        <mat-dialog-actions align="end" class="dialog-actions">
          <button mat-button type="button" (click)="cancel()" [disabled]="saving">Cancel</button>
          <button mat-flat-button color="primary" type="button" (click)="save(shiftForm)" [disabled]="saving">
            @if (saving) {
              <mat-spinner diameter="18"></mat-spinner>
            } @else {
              <mat-icon>{{ isEdit ? 'save_as' : 'save' }}</mat-icon>
            }
            {{ saving ? 'Saving...' : (isEdit ? 'Update' : 'Save') }}
          </button>
        </mat-dialog-actions>
      </form>
    </div>
  `,
  styles: [`
    .dialog-host {
      width: min(920px, 95vw);
      max-width: 100%;
      box-sizing: border-box;
      display: flex;
      flex-direction: column;
      max-height: 90vh;
    }

    .dialog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-right: 0.5rem;
      flex-shrink: 0;
    }

    .dialog-header h2 {
      margin: 0;
    }

    .dialog-content {
      display: block;
      overflow-y: auto;
      padding: 12px 24px;
      flex: 1;
      min-height: 0;
    }

    .header-card {
      border: 1px solid rgba(0, 0, 0, 0.12);
      border-radius: 10px;
      padding: 16px;
    }

    .header-card__title {
      font-weight: 600;
      margin-bottom: 16px;
    }

    .form-grid {
      display: grid;
      grid-template-columns: repeat(2, minmax(0, 1fr));
      gap: 16px;
    }

    .field-block {
      display: flex;
      flex-direction: column;
      gap: 6px;
    }

    .field-label {
      font-size: 12px;
      color: rgba(0, 0, 0, 0.65);
      font-weight: 500;
    }

    .span-2 {
      grid-column: 1 / -1;
    }

    .ng-select-container {
      z-index: 1000;
    }

    .option-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
    }

    .option-row span {
      flex: 1;
    }

    .option-row small {
      color: rgba(0, 0, 0, 0.45);
      font-size: 11px;
    }

    .form-field {
      width: 100%;
    }

    .time-toggle {
      color: rgba(0, 0, 0, 0.54);
    }

    .time-hint {
      color: rgba(0, 0, 0, 0.6);
      font-size: 12px;
    }

    /* Material's outline hides the native time-picker indicator. Force it
       back into view so the user has a visible clickable picker affordance. */
    .form-field input[type="time"]::-webkit-calendar-picker-indicator {
      opacity: 1;
      cursor: pointer;
    }

    .dialog-actions {
      padding: 16px 24px;
      flex-shrink: 0;
      border-top: 1px solid rgba(0, 0, 0, 0.12);
      background-color: rgba(0, 0, 0, 0.02);
      gap: 8px;
      display: flex;
      justify-content: flex-end;
    }

    .validation-error {
      font-size: 12px;
      color: #d32f2f;
      margin-top: 4px;
    }

    @media (max-width: 767.98px) {
      .form-grid {
        grid-template-columns: 1fr;
      }

      .dialog-host {
        width: 95vw;
      }
    }
  `]
})
export class ShiftEntryDialogComponent implements OnInit {
  private readonly endpoint = inject(ShiftDetailsEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly moduleSettingsService = inject(ModuleSettingsService);
  readonly dialogRef: MatDialogRef<ShiftEntryDialogComponent, boolean> = inject(MatDialogRef);
  readonly data = inject<ShiftEntryDialogData>(MAT_DIALOG_DATA);

  readonly periodOfDayOptions = signal<string[]>([]);
  saving = false;
  submitted = false;
  readonly isEdit = !!this.data.shift;

  ngOnInit(): void {
    this.loadPeriodOfDayOptions();
  }

  private async loadPeriodOfDayOptions(): Promise<void> {
    const settings = await this.moduleSettingsService.getModuleSettings<StaffRosterSettings>('staffRoster', {
      periodOfDayOptions: ['MORNING', 'AFTERNOON', 'NIGHT', 'OFF-DUTY', 'LEAVE']
    });
    this.periodOfDayOptions.set(settings.periodOfDayOptions);
  }

  model: ShiftDialogModel = this.data.shift
    ? {
        shiftId: this.data.shift.shiftId,
        shiftJob: this.data.shift.shiftJob,
        periodOfDay: this.data.shift.periodOfDay,
        resumptionTime: this.normalizeTimeForInput(this.data.shift.resumptionTime),
        closingTime: this.normalizeTimeForInput(this.data.shift.closingTime),
        punctualityRemarks: this.data.shift.punctualityRemarks ?? '',
        lateRemarks: this.data.shift.lateRemarks ?? '',
        normalClosingRemarks: this.data.shift.normalClosingRemarks ?? '',
        abnormalClosingRemarks: this.data.shift.abnormalClosingRemarks ?? '',
        evalTo: this.data.shift.evalTo ?? ''
      }
    : {
        shiftId: null,
        shiftJob: '',
        periodOfDay: '',
        resumptionTime: '',
        closingTime: '',
        punctualityRemarks: '',
        lateRemarks: '',
        normalClosingRemarks: '',
        abnormalClosingRemarks: '',
        evalTo: ''
      };

  private normalizeTimeForInput(value?: string | null): string {
    const raw = (value ?? '').trim();
    if (!raw) {
      return '';
    }

    const hhMm = raw.match(/^([01]?\d|2[0-3]):([0-5]\d)(?::[0-5]\d)?$/);
    if (hhMm) {
      return `${hhMm[1].padStart(2, '0')}:${hhMm[2]}`;
    }

    const amPm = raw.match(/^(1[0-2]|0?[1-9]):([0-5]\d)\s*([AaPp][Mm])$/);
    if (amPm) {
      let hour = Number.parseInt(amPm[1], 10);
      const minute = amPm[2];
      const period = amPm[3].toUpperCase();

      if (period === 'AM') {
        hour = hour % 12;
      } else {
        hour = hour % 12 + 12;
      }

      return `${hour.toString().padStart(2, '0')}:${minute}`;
    }

    return raw;
  }

  /** Shared 12-hour formatter used by both the grid and this dialog's edit-mode hint. */
  static to12HourTime(value?: string | null): string {
    const raw = (value ?? '').trim();
    if (!raw) {
      return '';
    }

    // Pull any HH:mm[:ss] in the string. Anchored only at the end so we
    // accept plain "08:30", ISO-style "08:30:00", and the time portion of
    // anything that has a date prefix (e.g. "1899-12-30T08:30:00").
    const hhMm = raw.match(/(\d{1,2}):(\d{2})(?::(\d{2}))?(?:\.\d+)?$/);
    if (hhMm) {
      const hour24 = Number.parseInt(hhMm[1], 10);
      const minute = hhMm[2];
      if (hour24 >= 0 && hour24 <= 23) {
        const period = hour24 >= 12 ? 'PM' : 'AM';
        const hour12 = hour24 % 12 === 0 ? 12 : hour24 % 12;
        return `${hour12}:${minute} ${period}`;
      }
    }

    // Compact "8:30AM" / "8:30 PM" — already in 12-hour form, just normalize spacing.
    const compactAmPm = raw.match(/^(\d{1,2}:\d{2})\s*([AaPp][Mm])$/);
    if (compactAmPm) {
      return `${compactAmPm[1]} ${compactAmPm[2].toUpperCase()}`;
    }

    // Last resort: try parsing as a Date. Render ONLY the time portion so
    // the date can never leak into the grid. If parsing fails, return the
    // raw string unchanged.
    const parsed = new Date(raw);
    if (!Number.isNaN(parsed.getTime())) {
      const hours = parsed.getHours();
      const minutes = parsed.getMinutes();
      const period = hours >= 12 ? 'PM' : 'AM';
      const hour12 = hours % 12 === 0 ? 12 : hours % 12;
      return `${hour12}:${minutes.toString().padStart(2, '0')} ${period}`;
    }

    return raw;
  }

  /** Template-friendly alias for the static formatter. */
  to12HourTime(value?: string | null): string {
    return ShiftEntryDialogComponent.to12HourTime(value);
  }

  onShiftChanged(shiftId: number | null): void {
    if (!shiftId) {
      this.model.shiftJob = '';
      return;
    }

    const lookup = this.data.lookups.find(item => item.shiftId === shiftId);
    this.model.shiftJob = lookup?.shiftJob ?? '';
  }

  openTimePicker(input: HTMLInputElement): void {
    // Material's outline form field hides the native picker chrome.
    // The clock icon button is the user-facing trigger; clicking it focuses
    // the input and invokes the browser's HTML5 time picker (showPicker is
    // widely supported, with a focus fallback for older browsers).
    input.focus();

    const picker = input as HTMLInputElement & { showPicker?: () => void | Promise<void> };
    if (typeof picker.showPicker === 'function') {
      // Some browsers return a Promise that rejects when the user cancels;
      // swallow the rejection so it doesn't surface as an unhandled error.
      void Promise.resolve(picker.showPicker()).catch(() => undefined);
    }
  }

  cancel(): void {
    if (this.saving) {
      return;
    }

    this.dialogRef.close(false);
  }

  save(form?: NgForm): void {
    this.submitted = true;

    const hasRequiredValues = !!this.model.shiftId
      && !!this.model.shiftJob.trim()
      && !!this.model.periodOfDay.trim()
      && !!this.model.resumptionTime.trim()
      && !!this.model.closingTime.trim()
      && !!this.model.punctualityRemarks.trim()
      && !!this.model.lateRemarks.trim()
      && !!this.model.normalClosingRemarks.trim()
      && !!this.model.abnormalClosingRemarks.trim()
      && !!this.model.evalTo.trim();

    if (!hasRequiredValues || (form && form.invalid)) {
      this.alertService.showMessage('Validation', 'All fields are required.', MessageSeverity.warn);
      return;
    }

    const shiftId = this.model.shiftId;
    if (!shiftId) {
      this.alertService.showMessage('Validation', 'Shift is required.', MessageSeverity.warn);
      return;
    }

    const payload: ShiftDetail = {
      shiftId,
      shiftJob: this.model.shiftJob.trim(),
      periodOfDay: this.model.periodOfDay.trim(),
      resumptionTime: this.model.resumptionTime.trim(),
      closingTime: this.model.closingTime.trim(),
      punctualityRemarks: this.model.punctualityRemarks.trim(),
      lateRemarks: this.model.lateRemarks.trim(),
      normalClosingRemarks: this.model.normalClosingRemarks.trim(),
      abnormalClosingRemarks: this.model.abnormalClosingRemarks.trim(),
      evalTo: this.model.evalTo.trim()
    };

    this.saving = true;

    const request = this.isEdit
      ? this.endpoint.updateEndpoint<ShiftDetail>(payload.shiftId, payload)
      : this.endpoint.createEndpoint<ShiftDetail>(payload);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.alertService.showMessage('Saved', 'Shift details saved successfully.', MessageSeverity.success);
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
