import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { ChartOfAccountEndpoint } from '../../../services/chart-of-account-endpoint.service';
import {
  ChartOfAccountDialogData,
  ChartOfAccountDialogResult,
  ChartOfAccountEntry,
  ChartOfAccountGroupLookup,
} from '../../../models/accounting/chart-of-account.model';

@Component({
  selector: 'app-chart-of-account-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    NgSelectModule,
    TranslateModule,
  ],
  template: `
    <div class="coa-dialog">
      <div class="dialog-header">
        <div class="dialog-title-wrap">
          <h2 class="dialog-title">{{ isEdit ? ('chartOfAccounts.EditAccount' | translate) : ('chartOfAccounts.NewAccount' | translate) }}</h2>
          <p class="dialog-subtitle">{{ 'chartOfAccounts.DialogSubtitle' | translate }}</p>
        </div>
        <button mat-icon-button type="button" (click)="onCancel()" [disabled]="saving" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div class="dialog-body">
        <div class="row-grid">
          <mat-form-field appearance="outline">
            <mat-label>{{ 'chartOfAccounts.AccountNo' | translate }}</mat-label>
            <input matInput [(ngModel)]="model.accountNo" [readonly]="isAutoAccountNo" maxlength="50" />
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>{{ 'chartOfAccounts.AccountName' | translate }}</mat-label>
            <input matInput [(ngModel)]="model.accountName" maxlength="200" />
          </mat-form-field>
        </div>

        <div class="row-grid">
          <div class="field-wrap">
            <div class="field-label">{{ 'chartOfAccounts.AccountGroup' | translate }}</div>
            <ng-select
              [items]="groups"
              bindLabel="groupName"
              bindValue="groupID"
              [(ngModel)]="model.groupID"
              [searchable]="true"
              [clearable]="false"
              (change)="onGroupChanged()"
              [placeholder]="'chartOfAccounts.SelectGroup' | translate">
            </ng-select>
          </div>

          <mat-form-field appearance="outline">
            <mat-label>{{ 'chartOfAccounts.Description' | translate }}</mat-label>
            <input matInput [(ngModel)]="model.accountDesc" maxlength="500" />
          </mat-form-field>
        </div>
      </div>

      <div class="dialog-actions">
        <button mat-button type="button" (click)="onCancel()" [disabled]="saving">
          {{ 'chartOfAccounts.Cancel' | translate }}
        </button>
        <button mat-flat-button color="primary" type="button" (click)="onSave()" [disabled]="saving">
          <mat-icon>save</mat-icon>
          {{ saving ? ('chartOfAccounts.Saving' | translate) : ('chartOfAccounts.Save' | translate) }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .coa-dialog { display: flex; flex-direction: column; max-height: 85vh; }
    .dialog-header { display: flex; justify-content: space-between; align-items: flex-start; gap: 12px; padding: 8px 4px 4px; }
    .dialog-title-wrap { min-width: 0; }
    .dialog-title { margin: 0; font-size: 1.2rem; font-weight: 600; }
    .dialog-subtitle { margin: 4px 0 0; color: rgba(0,0,0,.6); font-size: .85rem; }
    .dialog-body { display: flex; flex-direction: column; gap: 12px; padding: 8px 4px; }
    .row-grid { display: grid; grid-template-columns: repeat(2, minmax(0,1fr)); gap: 12px; }
    .field-wrap { display: flex; flex-direction: column; gap: 6px; }
    .field-label { font-size: .8rem; color: rgba(0,0,0,.65); }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; padding: 8px 4px 0; }
    @media (max-width: 768px) { .row-grid { grid-template-columns: 1fr; } }
  `]
})
export class ChartOfAccountDialogComponent implements OnInit {
  private readonly data = inject<ChartOfAccountDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<ChartOfAccountDialogComponent, ChartOfAccountDialogResult>);
  private readonly endpoint = inject(ChartOfAccountEndpoint);
  private readonly alertService = inject(AlertService);

  model: ChartOfAccountEntry = {
    accountNo: '',
    accountName: '',
    groupID: '',
    accountDesc: '',
    accountOpAmt: 0,
    accountClAmt: 0,
  };

  groups: ChartOfAccountGroupLookup[] = [];
  saving = false;
  isEdit = false;
  isAutoAccountNo = true;

  private originalGroupID = '';

  ngOnInit(): void {
    this.groups = this.data.groups.filter(g => !!g.groupID?.trim());
    this.isAutoAccountNo = (this.data.defaults.autoAccountNo || 'YES').toUpperCase() === 'YES';

    if (this.data.entry) {
      this.isEdit = true;
      this.model = {
        ...this.data.entry,
        accountDesc: this.data.entry.accountDesc ?? '',
      };
      this.originalGroupID = this.data.entry.groupID;
    }
  }

  onGroupChanged(): void {
    if (!this.model.groupID) {
      return;
    }

    // Prevent creation of Fixed Assets accounts via this form (only during new entry)
    if (this.model.groupID.startsWith('11') && !this.isEdit) {
      this.alertService.showMessage(
        'Not Allowed',
        'Cannot Create Fixed Assets here, Use the Fixed Assets Module',
        MessageSeverity.warn,
      );
      // Reset the invalid selection
      this.model.groupID = '';
      this.model.accountNo = '';
      return;
    }

    // Don't auto-generate if not enabled
    if (!this.isAutoAccountNo) {
      return;
    }

    // Auto-generate account number for new entries or when group changes during edit
    const groupChangedOnEdit = this.isEdit && this.model.groupID !== this.originalGroupID;
    if (!this.isEdit || groupChangedOnEdit) {
      this.endpoint.getNextChartOfAccountNoEndpoint<{ accountNo: string }>(this.model.groupID)
        .subscribe({
          next: result => {
            this.model.accountNo = result?.accountNo ?? '';
          },
          error: error => {
            this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
          }
        });
    }
  }

  onSave(): void {
    if (!this.model.groupID?.trim()) {
      this.alertService.showMessage('Validation', 'Please enter Account Group Name', MessageSeverity.warn);
      return;
    }

    if (!this.model.accountName?.trim()) {
      this.alertService.showMessage('Validation', 'Please enter Account Name', MessageSeverity.warn);
      return;
    }

    if (!this.isAutoAccountNo && !this.model.accountNo?.trim()) {
      this.alertService.showMessage('Validation', 'Account No is required.', MessageSeverity.warn);
      return;
    }

    this.saving = true;

    const payload: ChartOfAccountEntry = {
      ...this.model,
      accountNo: (this.model.accountNo || '').trim(),
      accountName: this.model.accountName.trim().toUpperCase(),
      groupID: this.model.groupID.trim(),
      accountDesc: (this.model.accountDesc || '').trim().toUpperCase(),
    };

    const request$ = this.isEdit && this.model.sNo
      ? this.endpoint.getUpdateChartOfAccountEndpoint<ChartOfAccountEntry>(this.model.sNo, payload)
      : this.endpoint.getNewChartOfAccountEndpoint<ChartOfAccountEntry>(payload);

    request$.subscribe({
      next: saved => {
        this.dialogRef.close({
          saved: true,
          sNo: saved?.sNo ?? undefined,
          operation: this.isEdit ? 'update' : 'create',
          entry: saved,
        });
      },
      error: error => {
        this.saving = false;
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close({ saved: false });
  }

  private getErrorMessage(error: unknown): string {
    const err = (error ?? {}) as { error?: { errors?: Record<string, string[]>; title?: string }; message?: string };
    const errors = err.error?.errors ? Object.values(err.error.errors).flat() : [];
    return errors[0] ?? err.error?.title ?? err.message ?? 'Unknown error';
  }
}
