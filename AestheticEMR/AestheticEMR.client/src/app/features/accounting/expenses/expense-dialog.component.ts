import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MatNativeDateModule,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';

import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  ExpenseAccountLookup,
  ExpenseDialogData,
  ExpenseDialogResult,
  ExpenseEntry,
} from '../../../models/accounting/expense.model';

export const DD_MMM_YYYY_FORMATS = {
  parse: { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput: 'dd-MMM-yyyy',
    monthYearLabel: 'MMM yyyy',
    dateA11yLabel: 'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

class DdMmmYyyyDateAdapter extends NativeDateAdapter {
  override parse(value: string): Date | null {
    if (!value) return null;
    const parts = value.split('-');
    if (parts.length === 3) {
      const day = parseInt(parts[0], 10);
      const month = new Date(`${parts[1]} 1 2000`).getMonth();
      const year = parseInt(parts[2], 10);
      if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
        return new Date(year, month, day);
      }
    }
    return super.parse(value);
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'dd-MMM-yyyy') {
      const d = date.getDate().toString().padStart(2, '0');
      const m = date.toLocaleString('en', { month: 'short' });
      const y = date.getFullYear();
      return `${d}-${m}-${y}`;
    }
    return super.format(date, displayFormat);
  }
}

@Component({
  selector: 'app-expense-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    NgSelectModule,
    TranslateModule,
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter }
  ],
  template: `
    <div class="dialog-host">
      <div class="dialog-header" mat-dialog-title>
        <div class="title-block">
          <mat-icon class="title-icon">receipt</mat-icon>
          <h2>{{ (isEdit ? 'expenses.EditExpense' : 'expenses.NewExpense') | translate }}</h2>
        </div>
        <button mat-icon-button type="button" class="close-btn" (click)="cancel()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-content">
        <section class="header-card">
          <div class="header-card__title">{{ 'expenses.EntryHeader' | translate }}</div>
          <div class="form-grid">
            <mat-form-field appearance="outline">
              <mat-label>{{ 'expenses.TranDate' | translate }}</mat-label>
              <input matInput [matDatepicker]="tranDatePicker" [(ngModel)]="model.tranDate" />
              <mat-datepicker-toggle matIconSuffix [for]="tranDatePicker"></mat-datepicker-toggle>
              <mat-datepicker #tranDatePicker></mat-datepicker>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>{{ 'expenses.Amount' | translate }}</mat-label>
              <input matInput type="number" min="0.01" step="0.01" [(ngModel)]="model.amount" />
            </mat-form-field>

            <div class="field-block span-2">
              <div class="field-label">{{ 'expenses.ExpenseAccount' | translate }}</div>
              <ng-select
                [items]="expenseAccounts"
                bindLabel="accountName"
                bindValue="accountNo"
                [(ngModel)]="model.accountDebit"
                (ngModelChange)="onDebitAccountChange()"
                [searchable]="true"
                [clearable]="true"
                [loading]="loadingExpenseAccounts"
                [placeholder]="translationKey('expenses.SelectExpenseAccount')"
                appendTo="body">
                <ng-template ng-option-tmp let-item="item">
                  <div class="option-row">
                    <span>{{ item.accountName }}</span>
                    <small>{{ item.accountNo }}</small>
                  </div>
                </ng-template>
              </ng-select>
            </div>

            <div class="field-block span-2">
              <div class="field-label">{{ 'expenses.PayingAccount' | translate }}</div>
              <ng-select
                [items]="payingAccounts"
                bindLabel="accountName"
                bindValue="accountNo"
                [(ngModel)]="model.accountCredit"
                (ngModelChange)="onCreditAccountChange()"
                [searchable]="true"
                [clearable]="true"
                [loading]="loadingPayingAccounts"
                [placeholder]="translationKey('expenses.SelectPayingAccount')"
                appendTo="body">
                <ng-template ng-option-tmp let-item="item">
                  <div class="option-row">
                    <span>{{ item.accountName }}</span>
                    <small>{{ item.accountNo }}</small>
                  </div>
                </ng-template>
              </ng-select>
            </div>

            <mat-form-field appearance="outline" class="span-2">
              <mat-label>{{ 'expenses.Description' | translate }}</mat-label>
              <textarea matInput rows="3" [(ngModel)]="model.description"></textarea>
            </mat-form-field>

            <div class="toggle-row span-2">
              <mat-slide-toggle [(ngModel)]="model.postDirectly">
                {{ 'expenses.PostDirectly' | translate }}
              </mat-slide-toggle>
              <span class="toggle-help">
                {{ model.postDirectly ? ('expenses.PostDirectlyHelp' | translate) : ('expenses.SaveAsUnpostedHelp' | translate) }}
              </span>
            </div>
          </div>
        </section>
      </mat-dialog-content>

      <mat-dialog-actions class="dialog-actions" align="end">
        <button mat-button type="button" (click)="cancel()" [disabled]="saving">{{ 'expenses.Cancel' | translate }}</button>
        <button mat-flat-button color="primary" type="button" (click)="save()" [disabled]="saving || loadingExpenseAccounts || loadingPayingAccounts">
          @if (!saving) {
            <mat-icon>save</mat-icon>
          } @else {
            <mat-progress-spinner diameter="16" mode="indeterminate"></mat-progress-spinner>
          }
          {{ (saving ? 'expenses.Saving' : 'expenses.Save') | translate }}
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-host { display: flex; flex-direction: column; min-width: 640px; max-width: 100vw; }
    .dialog-header { display: flex; align-items: center; justify-content: space-between; padding: 16px 24px; border-bottom: 1px solid rgba(0,0,0,.08); }
    .title-block { display: flex; align-items: center; gap: 8px; }
    .title-icon { color: #3f51b5; }
    .title-block h2 { margin: 0; font-size: 1.2rem; font-weight: 600; }
    .dialog-content { padding: 20px 24px 0; max-height: 75vh; overflow-y: auto; }
    .header-card { border: 1px solid rgba(0,0,0,.08); border-radius: 10px; padding: 16px; background: #fff; }
    .header-card__title { font-size: .95rem; font-weight: 600; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px; }
    .span-2 { grid-column: span 2; }
    .field-block { display: flex; flex-direction: column; gap: 6px; min-width: 0; }
    .field-label { font-size: .85rem; font-weight: 500; color: rgba(0,0,0,.7); }
    .toggle-row { display: flex; flex-direction: column; gap: 6px; padding-top: 4px; }
    .toggle-help { font-size: .8rem; color: rgba(0,0,0,.6); }
    .option-row { display: flex; justify-content: space-between; gap: 12px; }
    .option-row small { color: rgba(0,0,0,.6); }
    .dialog-actions { padding: 12px 24px; border-top: 1px solid rgba(0,0,0,.08); margin: 0; }
    mat-form-field { width: 100%; }
    :host ::ng-deep .ng-select { width: 100%; }
    :host ::ng-deep .ng-select .ng-select-container { min-height: 56px; }
    :host ::ng-deep .ng-dropdown-panel { z-index: 3000 !important; }
    @media (max-width: 767.98px) {
      .dialog-host { min-width: 0; }
      .form-grid { grid-template-columns: 1fr; }
      .span-2 { grid-column: span 1; }
    }
  `]
})
export class ExpenseDialogComponent implements OnInit {
  private expenseEndpoint = inject(ExpenseEndpoint);
  private alertService = inject(AlertService);
  dialogRef = inject<MatDialogRef<ExpenseDialogComponent, ExpenseDialogResult>>(MatDialogRef);
  data = inject<ExpenseDialogData>(MAT_DIALOG_DATA);

  isEdit = false;
  saving = false;
  loadingExpenseAccounts = false;
  loadingPayingAccounts = false;

  expenseAccounts: ExpenseAccountLookup[] = [];
  payingAccounts: ExpenseAccountLookup[] = [];

  model: ExpenseEntry = {
    tranDate: new Date(),
    accountDebit: '',
    accountCredit: '',
    amount: 0,
    description: '',
    isPost: false,
    postDirectly: false,
    isClose: false,
  };

  ngOnInit(): void {
    this.isEdit = !!this.data?.entry;
    if (this.data?.entry) {
      this.model = {
        ...this.data.entry,
        postDirectly: !!this.data.entry.isPost,
        tranDate: this.data.entry.tranDate ? new Date(this.data.entry.tranDate) : new Date(),
      };
    }

    this.loadLookups();
  }

  translationKey(key: string): string {
    return key;
  }

  onDebitAccountChange(): void {
    const selected = this.expenseAccounts.find(x => x.accountNo === this.model.accountDebit);
    this.model.debitAccountName = selected?.accountName ?? '';
  }

  onCreditAccountChange(): void {
    const selected = this.payingAccounts.find(x => x.accountNo === this.model.accountCredit);
    this.model.creditAccountName = selected?.accountName ?? '';
  }

  cancel(): void {
    this.dialogRef.close({ saved: false });
  }

  save(): void {
    if (!this.isValid()) {
      this.alertService.showMessage('Validation', 'Please complete all required expense fields.', MessageSeverity.warn);
      return;
    }

    const tranDate = this.model.tranDate instanceof Date ? this.model.tranDate : new Date(this.model.tranDate);
    const payload: ExpenseEntry = {
      ...this.model,
      tranDate: tranDate.toISOString(),
      amount: Number(this.model.amount) || 0,
      description: this.model.description.trim(),
      accountDebit: this.model.accountDebit.trim(),
      accountCredit: this.model.accountCredit.trim(),
      postDirectly: !!this.model.postDirectly,
    };

    this.saving = true;
    const request = this.isEdit && this.model.sNo
      ? this.expenseEndpoint.getUpdateExpenseEndpoint<ExpenseEntry>(this.model.sNo, payload)
      : this.expenseEndpoint.getNewExpenseEndpoint<ExpenseEntry>(payload);

    request.subscribe({
      next: saved => {
        this.alertService.showMessage('Success', 'Expense saved successfully.', MessageSeverity.success);
        this.dialogRef.close({ saved: true, sNo: saved.sNo ?? undefined });
      },
      error: error => {
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
        this.saving = false;
      },
      complete: () => {
        this.saving = false;
      }
    });
  }

  private loadLookups(): void {
    this.loadingExpenseAccounts = true;
    this.loadingPayingAccounts = true;

    this.expenseEndpoint.getExpenseAccountsEndpoint<ExpenseAccountLookup[]>().subscribe({
      next: accounts => {
        this.expenseAccounts = (accounts ?? []).map(account => ({
          accountNo: account.accountNo?.trim() ?? '',
          accountName: account.accountName?.trim() ?? ''
        }));
        this.onDebitAccountChange();
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', 'Unable to load expense accounts.', MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingExpenseAccounts = false;
      }
    });

    this.expenseEndpoint.getPayingAccountsEndpoint<ExpenseAccountLookup[]>().subscribe({
      next: accounts => {
        this.payingAccounts = (accounts ?? []).map(account => ({
          accountNo: account.accountNo?.trim() ?? '',
          accountName: account.accountName?.trim() ?? ''
        }));
        this.onCreditAccountChange();
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', 'Unable to load paying accounts.', MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingPayingAccounts = false;
      }
    });
  }

  private isValid(): boolean {
    return !!this.model.tranDate
      && !!this.model.accountDebit?.trim()
      && !!this.model.accountCredit?.trim()
      && !!this.model.description?.trim()
      && Number(this.model.amount) > 0;
  }

  private getErrorMessage(error: unknown): string {
    const err = (error ?? {}) as { error?: { errors?: Record<string, string[]>; title?: string }; message?: string };
    const errors = err.error?.errors ? Object.values(err.error.errors).flat() : [];
    return errors[0] ?? err.error?.title ?? err.message ?? 'Unknown error';
  }
}
