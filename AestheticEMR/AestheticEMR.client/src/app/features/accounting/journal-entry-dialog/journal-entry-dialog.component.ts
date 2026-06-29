import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { NgSelectModule } from '@ng-select/ng-select';

import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  JournalAccountLookup,
  JournalCostCenterLookup,
  JournalEntry,
  JournalEntryDialogData,
  JournalEntryDialogResult,
  JournalLine,
  JournalNextTranNo,
} from '../../../models/accounting/journal-entry.model';

interface JournalLineRow extends JournalLine {
  /** client-side row id (for *ngFor trackBy) */
  rowId: string;
}

@Component({
  selector: 'app-journal-entry-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatTooltipModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    NgSelectModule,
  ],
  providers: [
    // Use Nigerian/UK locale for dates (Africa/Lagos). Falls back gracefully.
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
  ],
  templateUrl: './journal-entry-dialog.component.html',
  styleUrl: './journal-entry-dialog.component.scss',
})
export class JournalEntryDialogComponent implements OnInit {
  private journalEndpoint = inject(JournalEndpoint);
  private alertService = inject(AlertService);
  dialogRef = inject<MatDialogRef<JournalEntryDialogComponent, JournalEntryDialogResult>>(MatDialogRef);
  data = inject<JournalEntryDialogData>(MAT_DIALOG_DATA);

  readonly displayedColumns = ['account', 'accountNo', 'debit', 'credit', 'description', 'tranDate', 'actions'];

  isEdit = false;
  loading = false;
  saving = false;

  tranNo = '';
  tranDate: Date = new Date();
  costCenterId = '';

  accounts: JournalAccountLookup[] = [];
  costCenters: JournalCostCenterLookup[] = [];

  lines = new MatTableDataSource<JournalLineRow>([]);
  totalDebit = 0;
  totalCredit = 0;
  balance = 0;

  get canSave(): boolean {
    if (!this.tranNo?.trim()) return false;
    if (!this.costCenterId?.trim()) return false;
    if (!this.tranDate) return false;
    if (this.lines.data.length === 0) return false;
    if (this.totalDebit !== this.totalCredit) return false;
    if (this.totalDebit === 0) return false;
    return this.lines.data.every(l =>
      !!l.accountNo?.trim() &&
      !!l.tranDate &&
      ((l.debit > 0) !== (l.credit > 0)) // exactly one of Dr/Cr non-zero
    );
  }

  ngOnInit(): void {
    this.isEdit = !!this.data?.entry;

    if (this.isEdit) {
      this.hydrateFromEntry(this.data.entry!);
    } else {
      this.tranDate = new Date();
      this.lines.data = [this.makeBlankRow()];
    }

    this.loadLookups();

    if (!this.isEdit) {
      this.generateNextTranNo();
    }

    this.recomputeTotals();
  }

  private hydrateFromEntry(entry: JournalEntry): void {
    this.tranNo = entry.tranNo;
    this.tranDate = entry.tranDate ? new Date(entry.tranDate) : new Date();
    this.costCenterId = entry.costCenterId ?? '';
    const rows: JournalLineRow[] = (entry.lines ?? []).map(l => ({
      accountNo: l.accountNo,
      accountName: l.accountName,
      debit: Number(l.debit ?? 0),
      credit: Number(l.credit ?? 0),
      description: l.description ?? '',
      tranDate: l.tranDate ? new Date(l.tranDate) : this.tranDate,
      rowId: this.makeRowId(),
    }));
    this.lines.data = rows.length > 0 ? rows : [this.makeBlankRow()];
  }

  private loadLookups(): void {
    this.loading = true;
    Promise.all([
      this.journalEndpoint.getJournalAccountsEndpoint<JournalAccountLookup[]>().toPromise(),
      this.journalEndpoint.getJournalCostCentersEndpoint<JournalCostCenterLookup[]>().toPromise(),
    ])
      .then(([accounts, costCenters]) => {
        this.accounts = accounts ?? [];
        this.costCenters = costCenters ?? [];
      })
      .catch(err => {
        this.alertService.showStickyMessage(
          'Load failed',
          'Could not load lookups (accounts / cost centers). ' + (err?.message ?? ''),
          MessageSeverity.error,
          err
        );
      })
      .finally(() => (this.loading = false));
  }

  private generateNextTranNo(): void {
    this.journalEndpoint
      .getNextJournalTranNoEndpoint<JournalNextTranNo>()
      .toPromise()
      .then(res => {
        if (res?.tranNo) {
          this.tranNo = res.tranNo;
        }
      })
      .catch(() => {
        // non-fatal: leave blank so user can type
      });
  }

  addRow(): void {
    this.lines.data = [...this.lines.data, this.makeBlankRow()];
  }

  removeRow(row: JournalLineRow): void {
    const filtered = this.lines.data.filter(r => r.rowId !== row.rowId);
    this.lines.data = filtered.length > 0 ? filtered : [this.makeBlankRow()];
    this.recomputeTotals();
  }

  onAccountChange(row: JournalLineRow): void {
    const acct = this.accounts.find(a => a.accountNo === row.accountNo);
    if (acct) {
      row.accountName = acct.accountName;
    }
    this.lines.data = [...this.lines.data];
  }

  onAmountChange(): void {
    this.recomputeTotals();
  }

  onDateChange(row: JournalLineRow): void {
    // Ensure date is valid; if invalid, fall back to header date.
    const parsed = row.tranDate instanceof Date ? row.tranDate : new Date(row.tranDate);
    if (!row.tranDate || isNaN(parsed.getTime())) {
      row.tranDate = this.tranDate;
    }
    this.lines.data = [...this.lines.data];
  }

  recomputeTotals(): void {
    this.totalDebit = this.lines.data.reduce((sum, l) => sum + (Number(l.debit) || 0), 0);
    this.totalCredit = this.lines.data.reduce((sum, l) => sum + (Number(l.credit) || 0), 0);
    this.balance = Math.abs(this.totalDebit - this.totalCredit);
  }

  cancel(): void {
    this.dialogRef.close({ saved: false });
  }

  save(): void {
    if (!this.canSave) {
      this.alertService.showMessage(
        'Cannot save',
        'Please check the form. Every row needs an account, a date, and exactly one of Debit/Credit. ' +
          'Totals must balance and be greater than zero.',
        MessageSeverity.warn
      );
      return;
    }

    const payload: JournalEntry = {
      tranNo: this.tranNo.trim(),
      tranDate: this.tranDate.toISOString(),
      costCenterId: this.costCenterId,
      lines: this.lines.data.map(l => {
        const date = l.tranDate instanceof Date ? l.tranDate : new Date(l.tranDate);
        return {
          accountNo: l.accountNo,
          accountName: l.accountName,
          debit: Number(l.debit) || 0,
          credit: Number(l.credit) || 0,
          description: l.description ?? '',
          tranDate: date.toISOString(),
        };
      }),
    };

    this.saving = true;
    const obs = this.isEdit
      ? this.journalEndpoint.updateJournalEntryEndpoint<JournalEntry>(this.tranNo, payload)
      : this.journalEndpoint.createJournalEntryEndpoint<JournalEntry>(payload);

    obs.toPromise()
      .then(saved => {
        this.alertService.showMessage('Success', `Journal entry ${saved?.tranNo ?? this.tranNo} saved.`, MessageSeverity.success);
        this.dialogRef.close({ saved: true, tranNo: saved?.tranNo ?? this.tranNo });
      })
      .catch(err => {
        const msg = this.extractErrorMessage(err) ?? 'Could not save journal entry.';
        this.alertService.showStickyMessage('Save failed', msg, MessageSeverity.error, err);
      })
      .finally(() => (this.saving = false));
  }

  private extractErrorMessage(err: any): string | null {
    // Prefer ASP.NET ModelState errors, fall back to statusText, fall back to message.
    const modelErrors = err?.error?.errors;
    if (modelErrors && typeof modelErrors === 'object') {
      const first = Object.values(modelErrors).flat() as string[];
      if (first.length) return first[0];
    }
    return err?.error?.title ?? err?.statusText ?? err?.message ?? null;
  }

  private makeBlankRow(): JournalLineRow {
    const row: JournalLineRow = {
      rowId: this.makeRowId(),
      accountNo: '',
      accountName: '',
      debit: 0,
      credit: 0,
      description: '',
      tranDate: this.tranDate,
    };
    return row;
  }

  private makeRowId(): string {
    return Math.random().toString(36).slice(2, 11);
  }

  trackByRowId(_index: number, row: JournalLineRow): string {
    return row.rowId;
  }
}