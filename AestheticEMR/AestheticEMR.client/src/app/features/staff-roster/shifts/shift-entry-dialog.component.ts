import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { ShiftDetail, ShiftDetailsEndpoint, ShiftLookup } from '../../../services/shift-details-endpoint.service';

const PERIOD_OF_DAY_OPTIONS = ['MORNING', 'AFTERNOON', 'NIGHT', 'OFF-DUTY', 'LEAVE'];

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
                (ngModelChange)="onShiftChanged($event)"
                [placeholder]="'Select shift'"
                appendTo=".dialog-host">
                <ng-template ng-option-tmp let-item="item">
                  <div class="option-row">
                    <span>{{ item.shiftJob }}</span>
                  </div>
                </ng-template>
              </ng-select>
            </div>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Period of Day</mat-label>
              <mat-select [(ngModel)]="model.periodOfDay">
                @for (option of periodOfDayOptions; track option) {
                  <mat-option [value]="option">{{ option }}</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Period/Abbreviation</mat-label>
              <input matInput [(ngModel)]="model.evalTo" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Resumption Time</mat-label>
              <input matInput type="time" [(ngModel)]="model.resumptionTime" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Closing Time</mat-label>
              <input matInput type="time" [(ngModel)]="model.closingTime" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Punctuality Remarks</mat-label>
              <input matInput [(ngModel)]="model.punctualityRemarks" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Late Remarks</mat-label>
              <input matInput [(ngModel)]="model.lateRemarks" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Normal Closing Remarks</mat-label>
              <input matInput [(ngModel)]="model.normalClosingRemarks" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-field">
              <mat-label>Abnormal Closing Remarks</mat-label>
              <input matInput [(ngModel)]="model.abnormalClosingRemarks" />
            </mat-form-field>
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

    .dialog-actions {
      padding: 16px 24px;
      flex-shrink: 0;
      border-top: 1px solid rgba(0, 0, 0, 0.12);
      background-color: rgba(0, 0, 0, 0.02);
      gap: 8px;
      display: flex;
      justify-content: flex-end;
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
export class ShiftEntryDialogComponent {
  private readonly endpoint = inject(ShiftDetailsEndpoint);
  private readonly alertService = inject(AlertService);
  readonly dialogRef = inject(MatDialogRef<ShiftEntryDialogComponent, boolean>);
  readonly data = inject<ShiftEntryDialogData>(MAT_DIALOG_DATA);

  readonly periodOfDayOptions = PERIOD_OF_DAY_OPTIONS;
  saving = false;
  readonly isEdit = !!this.data.shift;

  model: ShiftDialogModel = this.data.shift
    ? {
        shiftId: this.data.shift.shiftId,
        shiftJob: this.data.shift.shiftJob,
        periodOfDay: this.data.shift.periodOfDay,
        resumptionTime: this.data.shift.resumptionTime,
        closingTime: this.data.shift.closingTime,
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

  onShiftChanged(shiftId: number | null): void {
    if (!shiftId) {
      this.model.shiftJob = '';
      return;
    }

    const lookup = this.data.lookups.find(item => item.shiftId === shiftId);
    this.model.shiftJob = lookup?.shiftJob ?? '';
  }

  cancel(): void {
    if (this.saving) {
      return;
    }

    this.dialogRef.close(false);
  }

  save(): void {
    if (!this.model.shiftId || !this.model.shiftJob.trim() || !this.model.periodOfDay.trim() || !this.model.resumptionTime.trim() || !this.model.closingTime.trim()) {
      this.alertService.showMessage('Validation', 'Shift, period, resumption time and closing time are required.', MessageSeverity.warn);
      return;
    }

    const payload: ShiftDetail = {
      shiftId: this.model.shiftId,
      shiftJob: this.model.shiftJob.trim(),
      periodOfDay: this.model.periodOfDay.trim(),
      resumptionTime: this.model.resumptionTime.trim(),
      closingTime: this.model.closingTime.trim(),
      punctualityRemarks: this.model.punctualityRemarks.trim() || null,
      lateRemarks: this.model.lateRemarks.trim() || null,
      normalClosingRemarks: this.model.normalClosingRemarks.trim() || null,
      abnormalClosingRemarks: this.model.abnormalClosingRemarks.trim() || null,
      evalTo: this.model.evalTo.trim() || null
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
