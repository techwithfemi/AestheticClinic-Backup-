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
import { MatTableModule } from '@angular/material/table';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';

import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  ExpenseAccountLookup,
  ExpenseBatchSaveResult,
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

interface ExpenseGridEntry extends ExpenseEntry {
  rowId: string;
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
    MatTableModule,
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
              <mat-label>{{ 'expenses.TranId' | translate }}</mat-label>
              <input matInput [ngModel]="tranId" readonly />
              @if (showFieldError('tranId')) {
                <mat-error>{{ 'expenses.Required' | translate }}</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>{{ 'expenses.TranDate' | translate }}</mat-label>
              <input
                matInput
                [matDatepicker]="tranDatePicker"
                [(ngModel)]="model.tranDate"
                [required]="true"
                (ngModelChange)="onDraftChanged()"
                (blur)="markTouched('tranDate')" />
              <mat-datepicker-toggle matIconSuffix [for]="tranDatePicker"></mat-datepicker-toggle>
              <mat-datepicker #tranDatePicker></mat-datepicker>
              @if (showFieldError('tranDate')) {
                <mat-error>{{ 'expenses.Required' | translate }}</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>{{ 'expenses.Amount' | translate }}</mat-label>
              <input
                matInput
                type="text"
                inputmode="decimal"
                [ngModel]="amountInput"
                (ngModelChange)="onAmountInputChange($event)"
                (focus)="onAmountInputFocus()"
                (blur)="onAmountInputBlur()"
                [required]="true" />
              @if (showFieldError('amount')) {
                <mat-error>{{ 'expenses.Required' | translate }}</mat-error>
              }
            </mat-form-field>

            <div class="field-block">
              <div class="field-label">{{ 'expenses.ExpenseAccount' | translate }}</div>
              <ng-select
                [items]="expenseAccounts"
                bindLabel="accountName"
                bindValue="accountNo"
                [(ngModel)]="model.accountDebit"
                (ngModelChange)="onDebitAccountChange()"
                [searchable]="true"
                [clearable]="false"
                [loading]="loadingExpenseAccounts"
                [placeholder]="translationKey('expenses.SelectExpenseAccount')"
                appendTo=".dialog-host">
                <ng-template ng-option-tmp let-item="item">
                  <div class="option-row">
                    <span>{{ item.accountName }}</span>
                    <small>{{ item.accountNo }}</small>
                  </div>
                </ng-template>
              </ng-select>
              @if (showFieldError('accountDebit')) {
                <small class="error-text">{{ 'expenses.Required' | translate }}</small>
              }
            </div>

            <div class="field-block">
              <div class="field-label">{{ 'expenses.PayingAccount' | translate }}</div>
              <ng-select
                [items]="payingAccounts"
                bindLabel="accountName"
                bindValue="accountNo"
                [(ngModel)]="model.accountCredit"
                (ngModelChange)="onCreditAccountChange()"
                [searchable]="true"
                [clearable]="false"
                [loading]="loadingPayingAccounts"
                [placeholder]="translationKey('expenses.SelectPayingAccount')"
                appendTo=".dialog-host">
                <ng-template ng-option-tmp let-item="item">
                  <div class="option-row">
                    <span>{{ item.accountName }}</span>
                    <small>{{ item.accountNo }}</small>
                  </div>
                </ng-template>
              </ng-select>
              @if (showFieldError('accountCredit')) {
                <small class="error-text">{{ 'expenses.Required' | translate }}</small>
              }
            </div>

            <mat-form-field appearance="outline" class="span-2">
              <mat-label>{{ 'expenses.Description' | translate }}</mat-label>
              <textarea
                matInput
                rows="3"
                [(ngModel)]="model.description"
                [required]="true"
                (ngModelChange)="onDraftChanged()"
                (blur)="markTouched('description')"></textarea>
              @if (showFieldError('description')) {
                <mat-error>{{ 'expenses.Required' | translate }}</mat-error>
              }
            </mat-form-field>

            <div class="grid-action-row span-2">
              <button mat-flat-button color="primary" type="button" (click)="addOrUpdateGrid()" [disabled]="saving || loadingExpenseAccounts || loadingPayingAccounts">
                <mat-icon>{{ editingRowId ? 'edit' : 'add' }}</mat-icon>
                {{ (editingRowId ? 'expenses.UpdateLine' : 'expenses.AddToGrid') | translate }}
              </button>
              <button mat-stroked-button type="button" (click)="resetDraftForm()" [disabled]="saving">
                <mat-icon>refresh</mat-icon>
                {{ 'expenses.ClearLine' | translate }}
              </button>
            </div>
          </div>
        </section>

        <section class="lines-section">
          <div class="section-title">{{ 'expenses.GridEntries' | translate }}</div>

          <div class="lines-grid">
            <table mat-table [dataSource]="gridEntries" class="lines-table">
              <ng-container matColumnDef="tranId">
                <th mat-header-cell *matHeaderCellDef>{{ 'expenses.TranId' | translate }}</th>
                <td mat-cell *matCellDef="let row" class="mono">{{ row.tranId }}</td>
              </ng-container>

              <ng-container matColumnDef="tranDate">
                <th mat-header-cell *matHeaderCellDef>{{ 'expenses.TranDate' | translate }}</th>
                <td mat-cell *matCellDef="let row">{{ row.tranDate | date:'dd-MMM-yyyy' }}</td>
              </ng-container>

              <ng-container matColumnDef="debitAccountName">
                <th mat-header-cell *matHeaderCellDef>{{ 'expenses.ExpenseAccount' | translate }}</th>
                <td mat-cell *matCellDef="let row">
                  <div class="account-cell">
                    <span>{{ row.debitAccountName }}</span>
                    <small>{{ row.accountDebit }}</small>
                  </div>
                </td>
              </ng-container>

              <ng-container matColumnDef="creditAccountName">
                <th mat-header-cell *matHeaderCellDef>{{ 'expenses.PayingAccount' | translate }}</th>
                <td mat-cell *matCellDef="let row">
                  <div class="account-cell">
                    <span>{{ row.creditAccountName }}</span>
                    <small>{{ row.accountCredit }}</small>
                  </div>
                </td>
              </ng-container>

              <ng-container matColumnDef="description">
                <th mat-header-cell *matHeaderCellDef>{{ 'expenses.Description' | translate }}</th>
                <td mat-cell *matCellDef="let row">{{ row.description }}</td>
              </ng-container>

              <ng-container matColumnDef="amount">
                <th mat-header-cell *matHeaderCellDef class="num">{{ 'expenses.Amount' | translate }}</th>
                <td mat-cell *matCellDef="let row" class="num">{{ row.amount | number:'1.2-2' }}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef class="action-col">{{ 'expenses.Actions' | translate }}</th>
                <td mat-cell *matCellDef="let row" class="action-col">
                  <button mat-icon-button type="button" color="primary" (click)="editGridEntry(row)" [disabled]="saving" [attr.aria-label]="translationKey('expenses.EditLine')">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button type="button" (click)="deleteGridEntry(row)" [disabled]="saving" [attr.aria-label]="translationKey('expenses.Delete')">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedGridColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedGridColumns;"></tr>

              <tr class="mat-row no-data-row" *matNoDataRow>
                <td class="mat-cell empty-cell" [attr.colspan]="displayedGridColumns.length">
                  {{ 'expenses.NoGridEntries' | translate }}
                </td>
              </tr>
            </table>
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
    .dialog-host { display: flex; flex-direction: column; min-width: 900px; max-width: 100vw; max-height: 90vh; }
    .dialog-header { display: flex; align-items: center; justify-content: space-between; padding: 16px 24px; border-bottom: 1px solid rgba(0,0,0,.08); }
    .title-block { display: flex; align-items: center; gap: 8px; }
    .title-icon { color: #3f51b5; }
    .title-block h2 { margin: 0; font-size: 1.2rem; font-weight: 600; }
    .close-btn { margin: -8px; }
    .dialog-content { padding: 20px 24px; overflow-y: auto; flex: 1; }
    .header-card { border: 1px solid rgba(0,0,0,.08); border-radius: 10px; padding: 16px; background: #fff; margin-bottom: 20px; }
    .header-card__title { font-size: .95rem; font-weight: 600; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px; }
    .span-2 { grid-column: span 2; }
    .field-block { display: flex; flex-direction: column; gap: 6px; min-width: 0; }
    .field-label { font-size: .85rem; font-weight: 500; color: rgba(0,0,0,.7); }
    .option-row { display: flex; justify-content: space-between; gap: 12px; }
    .option-row small { color: rgba(0,0,0,.6); }
    .grid-action-row { display: flex; justify-content: flex-start; gap: 12px; }
    .error-text { color: #b00020; font-size: .75rem; margin-top: -2px; }
    .mono { font-family: 'Consolas', 'Menlo', monospace; font-size: .85rem; }
    mat-form-field { width: 100%; }
    .lines-section { border: 1px solid rgba(0,0,0,.08); border-radius: 10px; padding: 16px; background: #f9f9f9; }
    .section-title { font-size: .95rem; font-weight: 600; margin-bottom: 12px; }
    .lines-grid { overflow-x: auto; }
    .lines-table { width: 100%; }
    .lines-table th { padding: 8px 12px; background: rgba(0,0,0,.04); font-weight: 600; font-size: .85rem; text-align: left; }
    .lines-table td { padding: 8px 12px; font-size: .9rem; }
    .lines-table tr:hover { background: rgba(0,0,0,.02); }
    .account-cell { display: flex; flex-direction: column; gap: 2px; }
    .account-cell small { color: rgba(0,0,0,.6); }
    .num { text-align: right; font-variant-numeric: tabular-nums; }
    .action-col { width: 108px; text-align: center; white-space: nowrap; }
    .empty-cell { text-align: center; padding: 20px 12px; color: rgba(0,0,0,.5); }
    .dialog-actions { padding: 12px 24px; border-top: 1px solid rgba(0,0,0,.08); margin: 0; }
    :host ::ng-deep .ng-select { width: 100%; }
    :host ::ng-deep .ng-select .ng-select-container { min-height: 56px; }
    :host ::ng-deep .ng-dropdown-panel { z-index: 3000 !important; }
    @media (max-width: 767.98px) {
      .dialog-host { min-width: 0; }
      .form-grid { grid-template-columns: 1fr; }
      .span-2 { grid-column: span 1; }
      .grid-action-row { justify-content: stretch; flex-direction: column; }
      .grid-action-row button { width: 100%; }
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
  validationRequested = false;
  editingRowId: string | null = null;
  tranId = '';

  expenseAccounts: ExpenseAccountLookup[] = [];
  payingAccounts: ExpenseAccountLookup[] = [];
  gridEntries: ExpenseGridEntry[] = [];
  amountInput = '0.00';

  readonly displayedGridColumns = ['tranId', 'tranDate', 'debitAccountName', 'creditAccountName', 'description', 'amount', 'actions'];

  readonly touched: Record<string, boolean> = {
    tranId: false,
    tranDate: false,
    amount: false,
    accountDebit: false,
    accountCredit: false,
    description: false,
  };

  model: ExpenseEntry = {
    tranDate: new Date(),
    accountDebit: '',
    accountCredit: '',
    amount: 0,
    description: '',
    isPost: false,
    isClose: false,
    tranId: '',
  };

  ngOnInit(): void {
    this.isEdit = !!(this.data?.isEdit || this.data?.entries?.length || this.data?.entry);
    this.tranId = this.data?.tranId?.trim()
      ?? this.data?.entry?.tranId?.trim()
      ?? this.data?.entries?.[0]?.tranId?.trim()
      ?? '';

    const entries = this.data?.entries?.length
      ? this.data.entries
      : this.data?.entry
        ? [this.data.entry]
        : [];

    this.gridEntries = entries.map(entry => this.createGridEntry({ ...entry, tranId: this.tranId || entry.tranId }));
    this.resetDraftForm(false);
    this.loadLookups();
  }

  translationKey(key: string): string {
    return key;
  }

  markTouched(field: string): void {
    this.touched[field] = true;
  }

  onDebitAccountChange(): void {
    this.markTouched('accountDebit');
    const selected = this.findAccountByValue(this.expenseAccounts, this.model.accountDebit);
    this.model.accountDebit = selected?.accountNo ?? this.model.accountDebit?.trim() ?? '';
    this.model.debitAccountName = selected?.accountName ?? '';
    this.onDraftChanged();
  }

  onCreditAccountChange(): void {
    this.markTouched('accountCredit');
    const selected = this.findAccountByValue(this.payingAccounts, this.model.accountCredit);
    this.model.accountCredit = selected?.accountNo ?? this.model.accountCredit?.trim() ?? '';
    this.model.creditAccountName = selected?.accountName ?? '';
    this.onDraftChanged();
  }

  onDraftChanged(): void {
    this.model.tranId = this.tranId;
  }

  onAmountInputChange(value: string): void {
    this.touched['amount'] = true;
    this.amountInput = value;
    this.model.amount = this.parseAmount(value);
    this.onDraftChanged();
  }

  onAmountInputFocus(): void {
    this.amountInput = this.model.amount ? String(this.model.amount) : '';
  }

  onAmountInputBlur(): void {
    this.markTouched('amount');
    this.amountInput = this.formatAmount(this.model.amount);
  }

  addOrUpdateGrid(): void {
    this.validationRequested = true;
    this.model.tranId = this.tranId;
    this.onDebitAccountChange();
    this.onCreditAccountChange();
    this.markTouched('tranDate');
    this.markTouched('description');

    if (!this.isCurrentDraftValid()) {
      this.alertService.showMessage('Validation', 'Please complete all required expense fields before adding to the grid.', MessageSeverity.warn);
      return;
    }

    const gridEntry = this.createGridEntry(this.model, this.editingRowId ?? undefined);

    if (this.editingRowId) {
      this.gridEntries = this.gridEntries.map(entry => entry.rowId === this.editingRowId ? gridEntry : entry);
    } else {
      this.gridEntries = [...this.gridEntries, gridEntry];
    }

    this.resetDraftForm();
  }

  editGridEntry(entry: ExpenseGridEntry): void {
    this.editingRowId = entry.rowId;
    this.model = {
      ...entry,
      tranDate: entry.tranDate instanceof Date ? new Date(entry.tranDate) : new Date(entry.tranDate),
      tranId: this.tranId,
    };
    this.amountInput = this.formatAmount(entry.amount);
    this.validationRequested = false;
    this.resetTouched();
  }

  deleteGridEntry(entry: ExpenseGridEntry): void {
    this.gridEntries = this.gridEntries.filter(x => x.rowId !== entry.rowId);
    if (this.editingRowId === entry.rowId) {
      this.resetDraftForm();
    }
  }

  cancel(): void {
    this.dialogRef.close({ saved: false });
  }

  save(): void {
    this.validationRequested = true;
    this.touched['tranId'] = true;

    if (!this.tranId.trim()) {
      this.alertService.showMessage('Validation', 'Transaction id is required.', MessageSeverity.warn);
      return;
    }

    if (this.gridEntries.length === 0) {
      this.alertService.showMessage('Validation', 'Please add at least one expense entry to the grid.', MessageSeverity.warn);
      return;
    }

    const payload = this.gridEntries.map(entry => this.buildPayload(entry));
    if (payload.some(entry => !this.isValid(entry))) {
      this.alertService.showMessage('Validation', 'Please complete all required expense fields.', MessageSeverity.warn);
      return;
    }

    if (this.isEdit && payload.some(entry => !entry.period?.trim() || !entry.coyID?.trim())) {
      this.alertService.showMessage('Validation', 'Period and CoyID are required from the grid record for edit save.', MessageSeverity.warn);
      return;
    }

    this.saving = true;

    if (this.isEdit) {
      this.expenseEndpoint.getUpdateExpenseByTranIdEndpoint<ExpenseBatchSaveResult>(this.tranId, payload).subscribe({
        next: saved => {
          this.onSaveSuccess(saved.entries[0]?.sNo ?? undefined, this.tranId);
        },
        error: (error: unknown) => {
          this.onSaveError(error);
        },
        complete: () => {
          this.saving = false;
        }
      });
      return;
    }

    this.expenseEndpoint.getNewExpensesBatchEndpoint<ExpenseBatchSaveResult>(payload, this.tranId).subscribe({
      next: saved => {
        this.onSaveSuccess(saved.entries[0]?.sNo ?? undefined, this.tranId);
      },
      error: (error: unknown) => {
        this.onSaveError(error);
      },
      complete: () => {
        this.saving = false;
      }
    });
  }

  showFieldError(field: string): boolean {
    return (this.validationRequested || this.touched[field]) && !this.isFieldValid(field);
  }

  private onSaveSuccess(sNo?: number, tranId?: string): void {
    this.alertService.showMessage('Success', 'Expense saved successfully.', MessageSeverity.success);
    this.dialogRef.close({ saved: true, sNo, tranId });
  }

  private onSaveError(error: unknown): void {
    this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
    this.saving = false;
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
        this.syncGridAccountNames();
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
        this.syncGridAccountNames();
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', 'Unable to load paying accounts.', MessageSeverity.error, error);
      },
      complete: () => {
        this.loadingPayingAccounts = false;
      }
    });
  }

  private syncGridAccountNames(): void {
    this.gridEntries = this.gridEntries.map(entry => this.createGridEntry(entry, entry.rowId));
    if (this.editingRowId) {
      const editing = this.gridEntries.find(entry => entry.rowId === this.editingRowId);
      if (editing) {
        this.model = { ...editing, tranDate: new Date(editing.tranDate) };
      }
    } else {
      this.onDebitAccountChange();
      this.onCreditAccountChange();
    }
  }

  resetDraftForm(preserveSelections = true): void {
    const firstGridEntry = this.gridEntries[0];
    const currentTranDate = this.model.tranDate instanceof Date ? this.model.tranDate : new Date(this.model.tranDate);
    const tranDate = preserveSelections
      ? currentTranDate
      : firstGridEntry?.tranDate instanceof Date
        ? new Date(firstGridEntry.tranDate)
        : firstGridEntry?.tranDate
          ? new Date(firstGridEntry.tranDate)
          : new Date();

    const accountCredit = preserveSelections
      ? this.model.accountCredit
      : firstGridEntry?.accountCredit ?? '';

    const creditAccountName = preserveSelections
      ? this.model.creditAccountName
      : firstGridEntry?.creditAccountName ?? '';

    this.model = {
      tranDate,
      accountDebit: '',
      debitAccountName: '',
      accountCredit,
      creditAccountName,
      amount: 0,
      description: '',
      isPost: false,
      isClose: false,
      tranId: this.tranId,
    };

    this.editingRowId = null;
    this.validationRequested = false;
    this.amountInput = this.formatAmount(0);
    this.resetTouched();
  }

  private resetTouched(): void {
    Object.keys(this.touched).forEach(key => {
      this.touched[key] = false;
    });
  }

  private createGridEntry(entry: ExpenseEntry, rowId?: string): ExpenseGridEntry {
    const tranDate = entry.tranDate instanceof Date ? entry.tranDate : new Date(entry.tranDate);
    const debitAccount = this.findAccountByValue(this.expenseAccounts, entry.accountDebit);
    const creditAccount = this.findAccountByValue(this.payingAccounts, entry.accountCredit);

    return {
      ...entry,
      tranId: this.tranId || entry.tranId?.trim() || '',
      period: entry.period?.trim() ?? null,
      coyID: entry.coyID?.trim() ?? null,
      tranDate,
      accountDebit: entry.accountDebit?.trim() ?? '',
      accountCredit: entry.accountCredit?.trim() ?? '',
      debitAccountName: debitAccount?.accountName ?? entry.debitAccountName?.trim() ?? '',
      creditAccountName: creditAccount?.accountName ?? entry.creditAccountName?.trim() ?? '',
      description: entry.description?.trim() ?? '',
      amount: Number(entry.amount) || 0,
      rowId: rowId ?? this.makeRowId(),
    };
  }

  private buildPayload(entry: ExpenseGridEntry): ExpenseEntry {
    const tranDate = entry.tranDate instanceof Date ? entry.tranDate : new Date(entry.tranDate);
    return {
      sNo: entry.sNo,
      tranDate: this.toDateOnlyParam(tranDate),
      accountDebit: entry.accountDebit.trim(),
      accountCredit: entry.accountCredit.trim(),
      debitAccountName: entry.debitAccountName?.trim() ?? '',
      creditAccountName: entry.creditAccountName?.trim() ?? '',
      amount: Number(entry.amount) || 0,
      description: entry.description.trim(),
      isPost: entry.isPost,
      isClose: entry.isClose,
      userName: entry.userName,
      tranId: this.tranId,
      period: entry.period?.trim() ?? null,
      coyID: entry.coyID?.trim() ?? null,
      remarks: entry.remarks,
    };
  }

  private toDateOnlyParam(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private isCurrentDraftValid(): boolean {
    return this.isValid({ ...this.model, tranId: this.tranId });
  }

  private isFieldValid(field: string): boolean {
    switch (field) {
      case 'tranId':
        return !!this.tranId.trim();
      case 'tranDate':
        return !!this.model.tranDate;
      case 'amount':
        return Number(this.model.amount) > 0;
      case 'accountDebit':
        return !!this.findAccountByValue(this.expenseAccounts, this.model.accountDebit);
      case 'accountCredit':
        return !!this.findAccountByValue(this.payingAccounts, this.model.accountCredit);
      case 'description':
        return !!this.model.description?.trim();
      default:
        return true;
    }
  }

  private isValid(entry: ExpenseEntry): boolean {
    return !!entry.tranId?.trim()
      && !!entry.tranDate
      && !!this.findAccountByValue(this.expenseAccounts, entry.accountDebit)
      && !!this.findAccountByValue(this.payingAccounts, entry.accountCredit)
      && !!entry.description?.trim()
      && Number(entry.amount) > 0;
  }

  private parseAmount(value: string | number | null | undefined): number {
    const text = String(value ?? '').replace(/,/g, '').trim();
    const parsed = Number(text);
    return Number.isFinite(parsed) ? parsed : 0;
  }

  private formatAmount(value: number | null | undefined): string {
    const amount = Number(value) || 0;
    return new Intl.NumberFormat('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(amount);
  }

  private makeRowId(): string {
    return Math.random().toString(36).slice(2, 11);
  }

  private findAccountByValue(accounts: ExpenseAccountLookup[], value: string | null | undefined): ExpenseAccountLookup | undefined {
    const normalized = value?.trim() ?? '';
    if (!normalized) {
      return undefined;
    }

    return accounts.find(x => x.accountNo === normalized)
      ?? accounts.find(x => x.accountName.toLowerCase() === normalized.toLowerCase());
  }

  private getErrorMessage(error: unknown): string {
    const err = (error ?? {}) as { error?: { errors?: Record<string, string[]>; title?: string }; message?: string };
    const errors = err.error?.errors ? Object.values(err.error.errors).flat() : [];
    return errors[0] ?? err.error?.title ?? err.message ?? 'Unknown error';
  }
}
