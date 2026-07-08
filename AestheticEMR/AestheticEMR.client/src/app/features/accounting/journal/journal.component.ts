import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import { TransactionConfig } from '../shared/models/transaction-config.interface';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { JournalEntry } from '../../../models/accounting/journal-entry.model';

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

    // Dropdown endpoints - Use journal accounts endpoint for both (journal is dual-use)
    debitAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<unknown>(),
    creditAccountsEndpoint: () => this.journalEndpoint.getJournalAccountsEndpoint<unknown>(),

    // List endpoints - Map journal list structure to transaction list structure
    listEndpoint: (query) => this.mapJournalListToTransaction(
      this.journalEndpoint.getJournalEntriesEndpoint<unknown>(query)
    ),
    nextTranIdEndpoint: () => this.journalEndpoint.getNextJournalTranNoEndpoint<unknown>(),
    entriesByTranIdEndpoint: (tranId) => this.mapJournalEntryToTransaction(
      this.journalEndpoint.getJournalEntryEndpoint<unknown>(tranId)
    ),

    // Save/Update/Delete endpoints
    saveBatchEndpoint: (entries, tranId) => {
      const journalEntry = this.convertTransactionEntryToJournalEntry(entries[0], tranId);
      return this.journalEndpoint.createJournalEntryEndpoint<unknown>(journalEntry);
    },
    updateByTranIdEndpoint: (tranId, entries) => {
      const journalEntry = this.convertTransactionEntryToJournalEntry(entries[0], tranId);
      return this.journalEndpoint.updateJournalEntryEndpoint<unknown>(tranId, journalEntry);
    },
    deleteTranIdEndpoint: (tranId) => this.journalEndpoint.deleteJournalEntryEndpoint<unknown>(tranId),
  };

  /**
   * Map JournalListQuery to the transaction list format
   */
  private mapJournalListToTransaction(obs: Observable<unknown>) {
    return obs.pipe(
      map((result: unknown) => {
        const typedResult = result as Record<string, unknown>;
        return {
          totalCount: (typedResult.totalCount as number) ?? 0,
          page: (typedResult.page as number) ?? 1,
          pageSize: (typedResult.pageSize as number) ?? 10,
          items: ((typedResult.items as unknown[]) ?? []).map((item: unknown) => {
            const typedItem = item as Record<string, unknown>;
            return {
              sn: (typedItem.sn as number) ?? 0,
              tranDate: typedItem.tranDate as string,
              accountName: (typedItem.costCenterName as string) ?? '',
              accountNo: (typedItem.costCenterId as string) ?? '',
              debit: (typedItem.totalDebit as number) ?? 0,
              credit: (typedItem.totalCredit as number) ?? 0,
              description: '',
              tranNo: typedItem.tranNo as string,
              tranCat: undefined,
              billNo: undefined,
              costCenter: typedItem.costCenterName as string | undefined,
              entryDate: typedItem.tranDate as string,
              period: '',
              userName: undefined,
              sNo: 0,
              remarks: undefined,
              coyID: undefined,
              isClose: false,
            };
          })
        };
      })
    );
  }

  /**
   * Map JournalEntry to TransactionEntry array format
   */
  private mapJournalEntryToTransaction(obs: Observable<unknown>) {
    return obs.pipe(
      map((entry: unknown) => {
        if (!entry) return [];
        
        // Journal entries come as single JournalEntry, convert lines to TransactionEntry array
        const journalEntry = entry as JournalEntry;
        return (journalEntry.lines ?? []).map(line => ({
          sNo: undefined,
          tranDate: line.tranDate,
          accountDebit: line.accountNo ?? '',
          accountCredit: line.accountNo ?? '',
          debitAccountName: line.accountName ?? '',
          creditAccountName: line.accountName ?? '',
          amount: line.debit ?? line.credit ?? 0,
          description: line.description ?? '',
          isPost: false,
          isClose: false,
          userName: undefined,
          tranId: journalEntry.tranNo,
          period: undefined,
          coyID: undefined,
          remarks: undefined,
        }));
      })
    );
  }

  /**
   * Convert TransactionEntry back to JournalEntry format for save/update
   */
  private convertTransactionEntryToJournalEntry(entry: unknown, tranNo: string): JournalEntry {
    const typedEntry = entry as Record<string, unknown>;
    const tranDate = typedEntry.tranDate instanceof Date 
      ? (typedEntry.tranDate as Date).toISOString().split('T')[0]
      : (typeof typedEntry.tranDate === 'string' ? typedEntry.tranDate : new Date().toISOString().split('T')[0]);

    return {
      tranNo,
      tranDate,
      costCenterId: '',
      lines: [{
        accountNo: (typedEntry.accountDebit as string) || (typedEntry.accountCredit as string) || '',
        accountName: (typedEntry.debitAccountName as string) || (typedEntry.creditAccountName as string) || '',
        debit: typedEntry.accountDebit ? (typedEntry.amount as number) : 0,
        credit: typedEntry.accountCredit ? (typedEntry.amount as number) : 0,
        description: (typedEntry.description as string) || undefined,
        tranDate
      }]
    };
  }
}
