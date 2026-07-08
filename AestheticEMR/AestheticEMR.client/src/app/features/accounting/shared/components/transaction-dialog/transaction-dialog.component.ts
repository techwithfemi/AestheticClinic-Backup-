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

import { AlertService, MessageSeverity } from '../../../../../services/alert.service';
import {
  TransactionConfig,
  AccountLookup,
  TransactionDialogData,
  TransactionDialogResult,
  TransactionEntry,
} from '../../models/transaction-config.interface';

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

interface GridEntry extends TransactionEntry {
  rowId: string;
}

@Component({
  selector: 'app-transaction-dialog',
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
  templateUrl: './transaction-dialog.component.html',
  styleUrl: './transaction-dialog.component.scss',
})
export class TransactionDialogComponent implements OnInit {
  private alertService = inject(AlertService);
  dialogRef = inject<MatDialogRef<TransactionDialogComponent, TransactionDialogResult>>(MatDialogRef);
  data = inject<TransactionDialogData>(MAT_DIALOG_DATA);

  isEdit = false;
  saving = false;
  loadingDebitAccounts = false;
  loadingCreditAccounts = false;
  validationRequested = false;
  editingRowId: string | null = null;
  tranId = '';

  config!: TransactionConfig;
  debitAccounts: AccountLookup[] = [];
  creditAccounts: AccountLookup[] = [];
  gridEntries: GridEntry[] = [];
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

  model: TransactionEntry = {
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
    this.config = this.data?.config;
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
    const selected = this.findAccountByValue(this.debitAccounts, this.model.accountDebit);
    this.model.accountDebit = selected?.accountNo ?? this.model.accountDebit?.trim() ?? '';
    this.model.debitAccountName = selected?.accountName ?? '';
    this.onDraftChanged();
  }

  onCreditAccountChange(): void {
    this.markTouched('accountCredit');
    const selected = this.findAccountByValue(this.creditAccounts, this.model.accountCredit);
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
      this.alertService.showMessage('Validation', 'Please complete all required fields before adding to the grid.', MessageSeverity.warn);
      return;
    }

    // Validate that debit and credit accounts are not the same
    if (this.model.accountDebit?.trim() === this.model.accountCredit?.trim()) {
      this.alertService.showMessage('Validation', 'Debit account and credit account cannot be the same.', MessageSeverity.warn);
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

  editGridEntry(entry: GridEntry): void {
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

  deleteGridEntry(entry: GridEntry): void {
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
      this.alertService.showMessage('Validation', 'Please add at least one entry to the grid.', MessageSeverity.warn);
      return;
    }

    const payload = this.gridEntries.map(entry => this.buildPayload(entry));
    if (payload.some(entry => !this.isValid(entry))) {
      this.alertService.showMessage('Validation', 'Please complete all required fields.', MessageSeverity.warn);
      return;
    }

    if (this.isEdit && payload.some(entry => !entry.period?.trim() || !entry.coyID?.trim())) {
      this.alertService.showMessage('Validation', 'Period and CoyID are required for edit save.', MessageSeverity.warn);
      return;
    }

    this.saving = true;

    if (this.isEdit) {
      this.config.updateByTranIdEndpoint(this.tranId, payload).subscribe({
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

    this.config.saveBatchEndpoint(payload, this.tranId).subscribe({
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
    this.alertService.showMessage('Success', 'Transaction saved successfully.', MessageSeverity.success);
    this.dialogRef.close({ saved: true, sNo, tranId });
  }

  private onSaveError(error: unknown): void {
    this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
    this.saving = false;
  }

  private loadLookups(): void {
    // Load debit accounts
    if (this.config.debitAccountsEndpoint) {
      this.loadingDebitAccounts = true;
      this.config.debitAccountsEndpoint().subscribe({
        next: accounts => {
          this.debitAccounts = (accounts ?? []).map(account => ({
            accountNo: account.accountNo?.trim() ?? '',
            accountName: account.accountName?.trim() ?? ''
          }));
          this.syncGridAccountNames();
        },
        error: error => {
          this.alertService.showStickyMessage('Load Error', 'Unable to load debit accounts.', MessageSeverity.error, error);
        },
        complete: () => {
          this.loadingDebitAccounts = false;
        }
      });
    } else if (this.config.allAccountsEndpoint) {
      // Fallback to all accounts endpoint if debit endpoint not provided
      this.loadingDebitAccounts = true;
      this.config.allAccountsEndpoint().subscribe({
        next: accounts => {
          this.debitAccounts = (accounts ?? []).map(account => ({
            accountNo: account.accountNo?.trim() ?? '',
            accountName: account.accountName?.trim() ?? ''
          }));
          this.syncGridAccountNames();
        },
        error: error => {
          this.alertService.showStickyMessage('Load Error', 'Unable to load accounts.', MessageSeverity.error, error);
        },
        complete: () => {
          this.loadingDebitAccounts = false;
        }
      });
    }

    // Load credit accounts
    if (this.config.creditAccountsEndpoint) {
      this.loadingCreditAccounts = true;
      this.config.creditAccountsEndpoint().subscribe({
        next: accounts => {
          this.creditAccounts = (accounts ?? []).map(account => ({
            accountNo: account.accountNo?.trim() ?? '',
            accountName: account.accountName?.trim() ?? ''
          }));
          this.syncGridAccountNames();
        },
        error: error => {
          this.alertService.showStickyMessage('Load Error', 'Unable to load credit accounts.', MessageSeverity.error, error);
        },
        complete: () => {
          this.loadingCreditAccounts = false;
        }
      });
    } else if (this.config.allAccountsEndpoint) {
      // Fallback to all accounts endpoint if credit endpoint not provided
      this.loadingCreditAccounts = true;
      this.config.allAccountsEndpoint().subscribe({
        next: accounts => {
          this.creditAccounts = (accounts ?? []).map(account => ({
            accountNo: account.accountNo?.trim() ?? '',
            accountName: account.accountName?.trim() ?? ''
          }));
          this.syncGridAccountNames();
        },
        error: error => {
          this.alertService.showStickyMessage('Load Error', 'Unable to load accounts.', MessageSeverity.error, error);
        },
        complete: () => {
          this.loadingCreditAccounts = false;
        }
      });
    }
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

  private createGridEntry(entry: TransactionEntry, rowId?: string): GridEntry {
    const tranDate = entry.tranDate instanceof Date ? entry.tranDate : new Date(entry.tranDate);
    const debitAccount = this.findAccountByValue(this.debitAccounts, entry.accountDebit);
    const creditAccount = this.findAccountByValue(this.creditAccounts, entry.accountCredit);

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

  private buildPayload(entry: GridEntry): TransactionEntry {
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
        return !!this.findAccountByValue(this.debitAccounts, this.model.accountDebit);
      case 'accountCredit':
        return !!this.findAccountByValue(this.creditAccounts, this.model.accountCredit);
      case 'description':
        return !!this.model.description?.trim();
      default:
        return true;
    }
  }

  private isValid(entry: TransactionEntry): boolean {
    return !!entry.tranId?.trim()
      && !!entry.tranDate
      && !!this.findAccountByValue(this.debitAccounts, entry.accountDebit)
      && !!this.findAccountByValue(this.creditAccounts, entry.accountCredit)
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

  private findAccountByValue(accounts: AccountLookup[], value: string | null | undefined): AccountLookup | undefined {
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
