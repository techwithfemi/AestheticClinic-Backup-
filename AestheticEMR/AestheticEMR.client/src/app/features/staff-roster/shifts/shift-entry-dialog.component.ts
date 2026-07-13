import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbTimeStruct, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
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
  resumptionTimeDisplay: string;
  closingTime: string;
  closingTimeDisplay: string;
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
    NgSelectModule,
    NgbTimepickerModule
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
              <div class="field-block">
                <div class="field-label">Shift</div>
                <ng-select
                  class="dialog-ng-select"
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

              <div class="field-block">
                <div class="field-label">Period of Day</div>
                <ng-select
                  class="dialog-ng-select"
                  [items]="periodOfDayOptions()"
                  [searchable]="true"
                  [clearable]="false"
                  [(ngModel)]="model.periodOfDay"
                  name="periodOfDay"
                  required
                  (ngModelChange)="onPeriodOfDayChanged($event)"
                  [placeholder]="'Select period of day'"
                  appendTo=".dialog-host">
                </ng-select>
                @if (submitted && !model.periodOfDay.trim()) {
                  <div class="validation-error">Period of Day is required</div>
                }
              </div>

              <div class="field-block">
                <div class="field-label">Resumption Time *</div>
                <div class="timepicker-shell">
                  <ngb-timepicker
                    [(ngModel)]="resumptionTimeValue"
                    name="resumptionTimeValue"
                    [seconds]="false"
                    [spinners]="true"
                    [meridian]="true">
                  </ngb-timepicker>
                </div>
                @if (submitted && !resumptionTimeValue) {
                  <div class="validation-error">Resumption Time is required</div>
                }
              </div>

              <div class="field-block">
                <div class="field-label">Closing Time *</div>
                <div class="timepicker-shell">
                  <ngb-timepicker
                    [(ngModel)]="closingTimeValue"
                    name="closingTimeValue"
                    [seconds]="false"
                    [spinners]="true"
                    [meridian]="true">
                  </ngb-timepicker>
                </div>
                @if (submitted && !closingTimeValue) {
                  <div class="validation-error">Closing Time is required</div>
                }
              </div>

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

    :host ::ng-deep .dialog-ng-select .ng-select-container {
      min-height: 56px;
      height: 56px;
      border-radius: 4px;
      background-color: transparent !important;
      border: 1px solid rgba(0, 0, 0, 0.24) !important;
      box-shadow: none !important;
    }

    :host ::ng-deep .dialog-ng-select:hover .ng-select-container {
      border-color: rgba(0, 0, 0, 0.42) !important;
    }

    :host ::ng-deep .dialog-ng-select.ng-select-focused .ng-select-container,
    :host ::ng-deep .dialog-ng-select.ng-select-opened .ng-select-container,
    :host ::ng-deep .dialog-ng-select.ng-select-single .ng-select-container {
      background-color: transparent !important;
      border-color: rgba(25, 118, 210, 0.7) !important;
      box-shadow: none !important;
    }

    :host ::ng-deep .dialog-ng-select .ng-value-container {
      min-height: 56px;
      padding-top: 0;
      padding-bottom: 0;
      align-items: center;
      background-color: transparent !important;
    }

    :host ::ng-deep .dialog-ng-select .ng-input > input,
    :host ::ng-deep .dialog-ng-select .ng-value,
    :host ::ng-deep .dialog-ng-select .ng-placeholder,
    :host ::ng-deep .dialog-ng-select .ng-arrow-wrapper,
    :host ::ng-deep .dialog-ng-select .ng-clear-wrapper {
      background-color: transparent !important;
      color: inherit !important;
    }

    :host ::ng-deep .dialog-ng-select .ng-input {
      top: 50%;
      transform: translateY(-50%);
    }

    :host ::ng-deep .ng-dropdown-panel {
      background: #ffffff !important;
      border: 1px solid rgba(0, 0, 0, 0.16) !important;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12) !important;
      border-radius: 6px !important;
    }

    :host ::ng-deep .ng-dropdown-panel .ng-option {
      background: transparent !important;
      color: rgba(0, 0, 0, 0.87) !important;
    }

    :host ::ng-deep .ng-dropdown-panel .ng-option.ng-option-marked {
      background: rgba(25, 118, 210, 0.08) !important;
      color: rgba(0, 0, 0, 0.87) !important;
    }

    :host ::ng-deep .ng-dropdown-panel .ng-option.ng-option-selected {
      background: rgba(25, 118, 210, 0.14) !important;
      color: rgba(0, 0, 0, 0.87) !important;
      font-weight: 600;
    }

    .hidden-time-input {
      position: absolute;
      width: 1px;
      height: 1px;
      padding: 0;
      margin: -1px;
      overflow: hidden;
      clip: rect(0, 0, 0, 0);
      white-space: nowrap;
      border: 0;
      opacity: 0;
      pointer-events: none;
    }

    :host ::ng-deep .timepicker-shell {
      border: 1px solid rgba(0, 0, 0, 0.24);
      border-radius: 4px;
      padding: 4px 8px;
      background: transparent;
      min-height: 56px;
      display: flex;
      align-items: center;
    }

    :host ::ng-deep .timepicker-shell .ngb-tp {
      width: 100%;
      justify-content: space-between;
    }

    :host ::ng-deep .timepicker-shell .ngb-tp-input-container {
      width: 84px;
    }

    :host ::ng-deep .timepicker-shell .ngb-tp-input {
      height: 34px;
      font-size: 1rem;
      border: 1px solid rgba(0, 0, 0, 0.24);
      background: transparent;
    }

    :host ::ng-deep .timepicker-shell .btn-link {
      color: rgba(0, 0, 0, 0.65);
      padding: 0;
      min-width: 20px;
      line-height: 1;
    }

    :host ::ng-deep .timepicker-shell .ngb-tp-meridian .btn {
      height: 34px;
      min-width: 52px;
      border: 1px solid rgba(0, 0, 0, 0.24);
      background: transparent;
      color: inherit;
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
  readonly resumptionTimeError = signal<string | null>(null);
  readonly closingTimeError = signal<string | null>(null);
  resumptionTimeValue: NgbTimeStruct | null = null;
  closingTimeValue: NgbTimeStruct | null = null;
  saving = false;
  submitted = false;
  readonly isEdit = !!this.data.shift;

  ngOnInit(): void {
    this.loadPeriodOfDayOptions();
    this.resumptionTimeValue = this.toTimeStruct(this.model.resumptionTime);
    this.closingTimeValue = this.toTimeStruct(this.model.closingTime);
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
        periodOfDay: (this.data.shift.evalTo ?? this.data.shift.periodOfDay ?? '').trim(),
        resumptionTime: this.normalizeTimeForInput(this.data.shift.resumptionTime),
        resumptionTimeDisplay: ShiftEntryDialogComponent.to12HourTime(this.normalizeTimeForInput(this.data.shift.resumptionTime)),
        closingTime: this.normalizeTimeForInput(this.data.shift.closingTime),
        closingTimeDisplay: ShiftEntryDialogComponent.to12HourTime(this.normalizeTimeForInput(this.data.shift.closingTime)),
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
        resumptionTimeDisplay: '',
        closingTime: '',
        closingTimeDisplay: '',
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

    // Prefer 12-hour with AM/PM first so values like "1/1/1900 1:00:00 PM"
    // do not get misread as 01:00 (AM).
    const amPm = raw.match(/(\d{1,2}):(\d{2})(?::(\d{2}))?\s*([AaPp][Mm])/i);
    if (amPm) {
      let hour = Number.parseInt(amPm[1], 10);
      const minute = amPm[2];
      const period = amPm[4].toUpperCase();

      if (period === 'AM') {
        hour = hour === 12 ? 0 : hour;
      } else {
        hour = hour === 12 ? 12 : hour + 12;
      }

      return `${hour.toString().padStart(2, '0')}:${minute}`;
    }

    const timePattern = /(\d{1,2}):(\d{2})(?::(\d{2}))?/;
    const timeMatch = raw.match(timePattern);
    if (timeMatch) {
      const hour24 = Number.parseInt(timeMatch[1], 10);
      const minute = timeMatch[2];
      if (hour24 >= 0 && hour24 <= 23) {
        return `${hour24.toString().padStart(2, '0')}:${minute}`;
      }
    }

    const parsed = new Date(raw);
    if (!Number.isNaN(parsed.getTime())) {
      const hours = parsed.getHours();
      const minutes = parsed.getMinutes();
      return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
    }

    return raw;
  }

  /** Shared 12-hour formatter used by the grid and by this dialog's visible
   *  text input. Accepts 24-hour HH:mm[:ss] (including the time portion of
   *  date-prefixed strings like "1899-12-30T08:30:00") and renders "h:mm AM/PM". */
  static to12HourTime(value?: string | null): string {
    const raw = (value ?? '').trim();
    if (!raw) {
      return '';
    }

    // PRIORITY 1: Look for an explicit 12-hour "h:mm[:ss] AM/PM" anywhere
    // in the string. The server (Dapper + .NET DateTime.ToString on an
    // en-US culture) returns datetimes like "1/1/1900 1:00:00 PM" for 1 PM.
    // The bare HH:mm regex below would otherwise match the "1:" in the
    // date portion and incorrectly read 1 PM as 01:00 AM. The explicit
    // "PM" suffix disambiguates, so it MUST be checked first.
    const twelveHour = raw.match(/(\d{1,2}):(\d{2})(?::(\d{2}))?\s*([AaPp][Mm])\b/);
    if (twelveHour) {
      let hour = Number.parseInt(twelveHour[1], 10);
      const minute = twelveHour[2];
      const period = twelveHour[4].toUpperCase();
      if (period === 'AM') {
        hour = hour === 12 ? 0 : hour;
      } else {
        hour = hour === 12 ? 12 : hour + 12;
      }
      const hour12 = hour % 12 === 0 ? 12 : hour % 12;
      return `${hour12.toString().padStart(2, '0')}:${minute.padStart(2, '0')} ${period}`;
    }

    // PRIORITY 2: 24-hour HH:mm[:ss] (no AM/PM in the string). ISO-style
    // datetimes like "1900-01-01 13:00:00.000" fall here.
    const timePattern = /(\d{1,2}):(\d{2})(?::(\d{2}))?/;
    const timeMatch = raw.match(timePattern);

    if (timeMatch) {
      const hour24 = Number.parseInt(timeMatch[1], 10);
      const minute = timeMatch[2];
      if (hour24 >= 0 && hour24 <= 23) {
        const period = hour24 >= 12 ? 'PM' : 'AM';
        const hour12 = hour24 % 12 === 0 ? 12 : hour24 % 12;
        return `${hour12.toString().padStart(2, '0')}:${minute.padStart(2, '0')} ${period}`;
      }
    }

    // PRIORITY 3: try parsing as a Date. Render ONLY the time portion so
    // the date can never leak into the grid. If parsing fails, return the
    // raw string unchanged.
    const parsed = new Date(raw);
    if (!Number.isNaN(parsed.getTime())) {
      const hours = parsed.getHours();
      const minutes = parsed.getMinutes();
      const period = hours >= 12 ? 'PM' : 'AM';
      const hour12 = hours % 12 === 0 ? 12 : hours % 12;
      return `${hour12.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')} ${period}`;
    }

    return raw;
  }

  /** Template-friendly alias for the static formatter. */
  to12HourTime(value?: string | null): string {
    return ShiftEntryDialogComponent.to12HourTime(value);
  }

  /**
   * Handle typing in the visible 12-hour text input. We accept a wide range
   * of shapes (e.g. "8", "8:30", "8:30am", "830 PM", "20:30") and, when we
   * can make sense of them, write a normalized HH:mm value back to the
   * underlying model. Unparseable input is stored as-is and surfaced as a
   * field error so the user gets feedback rather than a silent failure.
   *
   * Note: the visible text input is bound to a separate "display" model
   * property via [(ngModel)] so the formatter does NOT re-write what the
   * user typed mid-keystroke. We must NOT touch the display here — doing so
   * would reset partial input (e.g. "1:00" → "01:00 AM") and block the user
   * from typing a trailing " PM".
   */
  on12HourTimeInput(field: 'resumptionTime' | 'closingTime', raw: string | unknown): void {
    const input = typeof raw === 'string' ? raw : '';
    const parsed = ShiftEntryDialogComponent.parseTimeInput(input);

    if (parsed === null) {
      this.model[field] = input;
      const errorSignal = field === 'resumptionTime' ? this.resumptionTimeError : this.closingTimeError;
      errorSignal.set(input.trim() ? 'Use formats like 8:30 AM, 8:30 PM, 08:30' : null);
      return;
    }

    this.model[field] = parsed;
    const errorSignal = field === 'resumptionTime' ? this.resumptionTimeError : this.closingTimeError;
    errorSignal.set(null);
  }

  /**
   * Re-format the visible text input to canonical 12-hour form on blur. If
   * the user typed a 24-hour value ("13:00") or an unparseable string, we
   * normalize the display to match the parsed model — but only if parsing
   * succeeded. Unparseable input is left alone so the user can fix it.
   */
  onTimeFieldBlur(field: 'resumptionTime' | 'closingTime'): void {
    const displayField = field === 'resumptionTime' ? 'resumptionTimeDisplay' : 'closingTimeDisplay';
    const parsed = ShiftEntryDialogComponent.parseTimeInput(this.model[displayField]);
    if (parsed !== null) {
      this.model[field] = parsed;
      this.model[displayField] = ShiftEntryDialogComponent.to12HourTime(parsed);
      const errorSignal = field === 'resumptionTime' ? this.resumptionTimeError : this.closingTimeError;
      errorSignal.set(null);
    }
  }

  /**
   * Handle selection in the hidden native time picker. The browser hands us
   * 24-hour HH:mm, which is the exact format the model already expects —
   * we just keep the visible text input in sync.
   */
  onNativeTimeChange(field: 'resumptionTime' | 'closingTime', value: string): void {
    const normalized = this.normalizeTimeForInput(value);
    this.model[field] = normalized;
    const displayField = field === 'resumptionTime' ? 'resumptionTimeDisplay' : 'closingTimeDisplay';
    this.model[displayField] = ShiftEntryDialogComponent.to12HourTime(normalized);
    const errorSignal = field === 'resumptionTime' ? this.resumptionTimeError : this.closingTimeError;
    errorSignal.set(null);
  }

  /**
   * Parse user-typed time into canonical HH:mm (24-hour). Returns null when
   * the input isn't a recognizable time — the caller decides what to do
   * (we surface a validation error rather than coercing to a wrong value).
   */
  static parseTimeInput(raw: string): string | null {
    const trimmed = (raw ?? '').trim();
    if (!trimmed) {
      return null;
    }

    // 12-hour with AM/PM: "8:30am", "8:30 PM", "8 PM", "8pm", "5PM", "5:00 pm"
    // Make regex more flexible to handle various spacing
    const twelveHour = trimmed.match(/^(1[0-2]|0?[1-9])(?::([0-5]\d))?\s*([AaPp][Mm])$/i);
    if (twelveHour) {
      let hour = Number.parseInt(twelveHour[1], 10);
      const minute = twelveHour[2] ?? '00';
      const period = twelveHour[3].toUpperCase();
      
      // Convert 12-hour to 24-hour format
      if (period === 'AM') {
        // 12 AM = 00:00, 1 AM = 01:00, ..., 11 AM = 11:00
        hour = hour === 12 ? 0 : hour;
      } else {
        // 12 PM = 12:00, 1 PM = 13:00, ..., 11 PM = 23:00
        hour = hour === 12 ? 12 : hour + 12;
      }
      return `${hour.toString().padStart(2, '0')}:${minute}`;
    }

    // 24-hour HH:mm or H:mm
    const twentyFour = trimmed.match(/^([01]?\d|2[0-3]):([0-5]\d)$/);
    if (twentyFour) {
      return `${twentyFour[1].padStart(2, '0')}:${twentyFour[2]}`;
    }

    // Compact "830" or "0830" — treat as 12-hour when ambiguous
    const compact = trimmed.match(/^(\d{3,4})$/);
    if (compact) {
      const digits = compact[1];
      const hourStr = digits.length === 3 ? digits[0] : digits.slice(0, 2);
      const minuteStr = digits.length === 3 ? digits.slice(1) : digits.slice(2);
      const hour = Number.parseInt(hourStr, 10);
      const minute = Number.parseInt(minuteStr, 10);
      if (hour >= 1 && hour <= 12 && minute >= 0 && minute <= 59) {
        // The user has only typed digits, so AM/PM is unknowable. Default to
        // the 12-hour hour as-if it were AM; they can corrected it with a tap
        // of the picker or by typing AM/PM. We return null so the validation
        // error guides them toward an unambiguous value.
        return null;
      }
    }

    return null;
  }

  onShiftChanged(shiftId: number | null): void {
    if (!shiftId) {
      this.model.shiftJob = '';
      return;
    }

    const lookup = this.data.lookups.find(item => item.shiftId === shiftId);
    this.model.shiftJob = lookup?.shiftJob ?? '';
  }

  onPeriodOfDayChanged(value: string): void {
    this.model.evalTo = (value ?? '').trim();
  }

  cancel(): void {
    if (this.saving) {
      return;
    }

    this.dialogRef.close(false);
  }

  save(form?: NgForm): void {
    this.submitted = true;

    this.model.resumptionTime = this.to24HourTime(this.resumptionTimeValue);
    this.model.closingTime = this.to24HourTime(this.closingTimeValue);

    const hasRequiredValues = !!this.model.shiftId
      && !!this.model.shiftJob.trim()
      && !!this.model.periodOfDay.trim()
      && !!this.model.resumptionTime.trim()
      && !!this.model.closingTime.trim()
      && !!this.model.punctualityRemarks.trim()
      && !!this.model.lateRemarks.trim()
      && !!this.model.normalClosingRemarks.trim()
      && !!this.model.abnormalClosingRemarks.trim();

    if (!hasRequiredValues || (form && form.invalid) || !this.resumptionTimeValue || !this.closingTimeValue) {
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
      evalTo: this.model.periodOfDay.trim()
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

  private toTimeStruct(value?: string | null): NgbTimeStruct | null {
    const normalized = this.normalizeTimeForInput(value);
    const match = normalized.match(/^(\d{2}):(\d{2})$/);
    if (!match) {
      return null;
    }

    const hour = Number.parseInt(match[1], 10);
    const minute = Number.parseInt(match[2], 10);

    if (Number.isNaN(hour) || Number.isNaN(minute)) {
      return null;
    }

    return { hour, minute, second: 0 };
  }

  private to24HourTime(value: NgbTimeStruct | null): string {
    if (!value) {
      return '';
    }

    return `${value.hour.toString().padStart(2, '0')}:${value.minute.toString().padStart(2, '0')}`;
  }
}
