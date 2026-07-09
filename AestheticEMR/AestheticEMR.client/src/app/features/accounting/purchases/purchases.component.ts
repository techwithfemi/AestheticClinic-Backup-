import { Component, inject } from '@angular/core';
import { map } from 'rxjs/operators';

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
import {
  JournalEntry,
  JournalLine,
  JournalListLineQuery,
  JournalNextTranNo,
  PagedJournalLinesResult,
} from '../../../models/accounting/journal-entry.model';

@Component({
  selector: 'app-purchases',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class PurchasesComponent {
  private journalEndpoint = inject(JournalEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Purchases',
    translateKeyPrefix: 'journal',
    debitAccountLabel: 'Debit Account',
    creditAccountLabel: 'Credit Account',

    debitAccountsEndpoint: () => this.journalEndpoint.getPurchasesDebitAccountsEndpoint<AccountLookup[]>(),
    creditAccountsEndpoint: () => this.journalEndpoint.getPurchasesCreditAccountsEndpoint<AccountLookup[]>(),

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

    entriesByTranIdEndpoint: (tranId) =>
      this.journalEndpoint.getJournalEntryEndpoint<JournalEntry>(tranId).pipe(
        map(entry => this.mapJournalEntryToTransactionEntries(entry))
      ),

    saveBatchEndpoint: (entries, tranId) => {
      const journalEntry = this.buildJournalEntry(entries, tranId);
      return this.journalEndpoint.createJournalEntryEndpoint<JournalEntry>(journalEntry).pipe(
        map((saved): BatchSaveResult => ({
          entries: this.mapJournalEntryToTransactionEntries(saved),
        }))
      );
    },

    updateByTranIdEndpoint: (tranId, entries) => {
      const journalEntry = this.buildJournalEntry(entries, tranId);
      return this.journalEndpoint.updateJournalEntryEndpoint<JournalEntry>(tranId, journalEntry).pipe(
        map((saved): BatchSaveResult => ({
          entries: this.mapJournalEntryToTransactionEntries(saved),
        }))
      );
    },

    deleteTranIdEndpoint: (tranId) =>
      this.journalEndpoint.deleteJournalEntryEndpoint<void>(tranId),
  };

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

  private buildJournalEntry(entries: TransactionEntry[], tranNo: string): JournalEntry {
    const first = entries[0];
    const tranDate = this.toDateString(first?.tranDate ?? new Date());

    const lines: JournalLine[] = [];

    for (const entry of entries) {
      const lineDate = this.toDateString(entry.tranDate ?? new Date());
      const amount = Number(entry.amount) || 0;

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
