import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import {
  AccountLookup,
  BatchSaveResult,
  PagedTransactionResult,
  TransactionConfig,
  TransactionEntry,
  TransactionListItem,
  TranIdResponse,
} from '../shared/models/transaction-config.interface';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { map } from 'rxjs/operators';
import {
  JournalEntry,
  JournalLine,
  JournalListLineQuery,
  JournalNextTranNo,
  PagedJournalLinesResult,
} from '../../../models/accounting/journal-entry.model';

@Component({
  selector: 'app-journal',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class JournalComponent {
  private journalEndpoint = inject(JournalEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Journal Entries',
    translateKeyPrefix: 'journal',
    debitAccountLabel: 'Debit Account',
    creditAccountLabel: 'Credit Account',

    // Both dropdowns use the full journal accounts list
    debitAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<AccountLookup[]>(),
    creditAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<AccountLookup[]>(),

    listEndpoint: (query) => {
      const lineQuery: JournalListLineQuery = {
        search: query.search ?? undefined,
        fromDate: query.fromDate ?? undefined,
        toDate: query.toDate ?? undefined,
        page: query.page,
        pageSize: query.pageSize,
      };
      return this.journalEndpoint.getJournalEntryLinesEndpoint<PagedJournalLinesResult>(lineQuery).pipe(
        map((result): PagedTransactionResult => ({
          totalCount: result?.totalCount ?? 0,
          page: result?.page ?? 1,
          pageSize: result?.pageSize ?? 10,
          items: (result?.items ?? []).map((item): TransactionListItem => ({
            sn: item.sn,
            sNo: item.sNo,
            tranNo: item.tranNo,
            tranDate: item.tranDate,
            accountName: item.accountName,
            accountNo: item.accountNo,
            debit: item.debit,
            credit: item.credit,
            description: item.description,
            tranCat: item.tranCat,
            billNo: item.billNo,
            costCenter: item.costCenter,
            entryDate: item.entryDate,
            period: item.period ?? '',
            userName: item.userName,
            remarks: item.remarks,
            coyID: item.coyID,
            isClose: item.isClose,
          })),
        }))
      );
    },

    nextTranIdEndpoint: () =>
      this.journalEndpoint.getNextJournalTranNoEndpoint<JournalNextTranNo>().pipe(
        map((r): TranIdResponse => ({ tranId: r?.tranNo ?? '' }))
      ),

    // Load all lines for a tranNo, pair debit+credit into grid rows
    entriesByTranIdEndpoint: (tranId) =>
      this.journalEndpoint.getJournalEntryEndpoint<JournalEntry>(tranId).pipe(
        map(entry => this.mapJournalEntryToTransactionEntries(entry))
      ),

    // Build a proper two-line JournalEntry per grid row and POST
    saveBatchEndpoint: (entries, tranId) => {
      const journalEntry = this.buildJournalEntry(entries, tranId);
      return this.journalEndpoint.createJournalEntryEndpoint<JournalEntry>(journalEntry).pipe(
        map((saved): BatchSaveResult => ({
          entries: this.mapJournalEntryToTransactionEntries(saved),
        }))
      );
    },

    // Build and PUT
    updateByTranIdEndpoint: (tranId, entries) => {
      const journalEntry = this.buildJournalEntry(entries, tranId);
      return this.journalEndpoint.updateJournalEntryEndpoint<JournalEntry>(tranId, journalEntry).pipe(
        map((saved): BatchSaveResult => ({
          entries: this.mapJournalEntryToTransactionEntries(saved),
        }))
      );
    },

    // Journal backend resolves period/coyID internally — ignore those params
    deleteTranIdEndpoint: (tranId) =>
      this.journalEndpoint.deleteJournalEntryEndpoint<void>(tranId),
  };

  /**
   * Pairs debit and credit lines from JournalEntry into TransactionEntry[] grid rows.
   * Debit lines (Amount > 0) are matched by position with credit lines (Amount < 0).
   */
  private mapJournalEntryToTransactionEntries(entry: JournalEntry | null | undefined): TransactionEntry[] {
    if (!entry) return [];

    const lines = entry.lines ?? [];
    const debitLines = lines.filter(l => (l.debit ?? 0) > 0);
    const creditLines = lines.filter(l => (l.credit ?? 0) > 0);
    const count = Math.max(debitLines.length, creditLines.length, 1);

    return Array.from({ length: count }, (_, i): TransactionEntry => {
      const dr = debitLines[i];
      const cr = creditLines[i];
      const tranDate = dr?.tranDate ?? cr?.tranDate ?? entry.tranDate;
      return {
        sNo: undefined,
        tranDate,
        accountDebit: dr?.accountNo ?? '',
        debitAccountName: dr?.accountName ?? '',
        accountCredit: cr?.accountNo ?? '',
        creditAccountName: cr?.accountName ?? '',
        amount: dr?.debit ?? cr?.credit ?? 0,
        description: dr?.description ?? cr?.description ?? '',
        isPost: false,
        isClose: false,
        userName: undefined,
        tranId: entry.tranNo,
        period: undefined,
        coyID: undefined,
        remarks: undefined,
      };
    });
  }

  /**
   * Converts TransactionEntry[] grid rows into a JournalEntry with two lines per row
   * (one debit line + one credit line) so the backend can balance-check them.
   */
  private buildJournalEntry(entries: TransactionEntry[], tranNo: string): JournalEntry {
    const first = entries[0];
    const tranDate = this.toDateString(first?.tranDate ?? new Date());

    const lines: JournalLine[] = [];

    for (const entry of entries) {
      const lineDate = this.toDateString(entry.tranDate ?? new Date());
      const amount = Number(entry.amount) || 0;

      // Debit line (positive amount)
      if (entry.accountDebit?.trim()) {
        lines.push({
          accountNo: entry.accountDebit.trim(),
          accountName: entry.debitAccountName?.trim() ?? '',
          debit: amount,
          credit: 0,
          description: entry.description?.trim() || 'NIL',
          tranDate: lineDate,
        });
      }

      // Credit line (negative amount stored as positive credit)
      if (entry.accountCredit?.trim()) {
        lines.push({
          accountNo: entry.accountCredit.trim(),
          accountName: entry.creditAccountName?.trim() ?? '',
          debit: 0,
          credit: amount,
          description: entry.description?.trim() || 'NIL',
          tranDate: lineDate,
        });
      }
    }

    return {
      tranNo,
      tranDate,
      costCenterId: '',
      lines,
    };
  }

  private toDateString(date: string | Date): string {
    if (date instanceof Date) {
      return date.toISOString().split('T')[0];
    }
    if (typeof date === 'string' && date.includes('T')) {
      return date.split('T')[0];
    }
    return date ?? new Date().toISOString().split('T')[0];
  }
}
