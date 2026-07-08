import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import {
  AccountLookup,
  BatchSaveResult,
  PagedTransactionResult,
  TransactionConfig,
  TransactionEntry,
  TranIdResponse,
} from '../shared/models/transaction-config.interface';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { map } from 'rxjs/operators';
import {
  JournalEntry,
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

    debitAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<AccountLookup[]>(),
    creditAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<AccountLookup[]>(),

    listEndpoint: (query) => {
      const lineQuery: JournalListLineQuery = {
        search: query.search,
        tranDate: query.search?.trim() ? undefined : (query.fromDate ?? query.toDate ?? undefined),
        page: query.page,
        pageSize: query.pageSize,
      };

      return this.journalEndpoint.getJournalEntryLinesEndpoint<PagedJournalLinesResult>(lineQuery)
        .pipe(
          map((result): PagedTransactionResult => ({
            totalCount: result?.totalCount ?? 0,
            page: result?.page ?? 1,
            pageSize: result?.pageSize ?? 10,
            items: (result?.items ?? []).map(item => ({
              ...item,
              period: item.period ?? '',
            })),
          }))
        );
    },
    nextTranIdEndpoint: () => this.journalEndpoint.getNextJournalTranNoEndpoint<JournalNextTranNo>()
      .pipe(map((r): TranIdResponse => ({ tranId: r?.tranNo ?? '' }))),
    entriesByTranIdEndpoint: (tranId) => this.journalEndpoint.getJournalEntryEndpoint<JournalEntry>(tranId)
      .pipe(map(entry => this.mapJournalEntryToTransaction(entry))),
    saveBatchEndpoint: (entries, tranId) => {
      const journalEntry = this.convertTransactionEntryToJournalEntry(entries[0], tranId);
      return this.journalEndpoint.createJournalEntryEndpoint<JournalEntry>(journalEntry)
        .pipe(map((): BatchSaveResult => ({ entries })));
    },
    updateByTranIdEndpoint: (tranId, entries) => {
      const journalEntry = this.convertTransactionEntryToJournalEntry(entries[0], tranId);
      return this.journalEndpoint.updateJournalEntryEndpoint<JournalEntry>(tranId, journalEntry)
        .pipe(map((): BatchSaveResult => ({ entries })));
    },
    deleteTranIdEndpoint: (tranId) => this.journalEndpoint.deleteJournalEntryEndpoint<void>(tranId),
  };

  private mapJournalEntryToTransaction(entry: JournalEntry | null | undefined): TransactionEntry[] {
    if (!entry) {
      return [];
    }

    return (entry.lines ?? []).map(line => ({
      sNo: undefined,
      tranDate: line.tranDate,
      accountDebit: (line.debit ?? 0) > 0 ? (line.accountNo ?? '') : '',
      accountCredit: (line.credit ?? 0) > 0 ? (line.accountNo ?? '') : '',
      debitAccountName: (line.debit ?? 0) > 0 ? (line.accountName ?? '') : '',
      creditAccountName: (line.credit ?? 0) > 0 ? (line.accountName ?? '') : '',
      amount: (line.debit ?? 0) > 0 ? (line.debit ?? 0) : (line.credit ?? 0),
      description: line.description ?? '',
      isPost: false,
      isClose: false,
      userName: undefined,
      tranId: entry.tranNo,
      period: undefined,
      coyID: undefined,
      remarks: undefined,
    }));
  }

  private convertTransactionEntryToJournalEntry(entry: TransactionEntry, tranNo: string): JournalEntry {
    const tranDate = entry.tranDate instanceof Date
      ? entry.tranDate.toISOString().split('T')[0]
      : (typeof entry.tranDate === 'string' ? entry.tranDate : new Date().toISOString().split('T')[0]);

    return {
      tranNo,
      tranDate,
      costCenterId: '',
      lines: [{
        accountNo: entry.accountDebit || entry.accountCredit || '',
        accountName: entry.debitAccountName || entry.creditAccountName || '',
        debit: entry.accountDebit ? entry.amount : 0,
        credit: entry.accountCredit ? entry.amount : 0,
        description: entry.description || undefined,
        tranDate
      }]
    };
  }
}
