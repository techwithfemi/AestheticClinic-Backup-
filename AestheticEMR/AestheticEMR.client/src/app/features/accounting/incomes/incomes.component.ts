import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import {
  BatchSaveResult,
  TransactionConfig,
  AccountLookup,
  PagedTransactionResult,
  TranIdResponse,
  TransactionEntry
} from '../shared/models/transaction-config.interface';
import { IncomeEndpoint } from '../../../services/income-endpoint.service';
import { IncomeListQuery } from '../../../models/accounting/income.model';

@Component({
  selector: 'app-incomes',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class IncomesComponent {
  private incomeEndpoint = inject(IncomeEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Incomes',
    translateKeyPrefix: 'incomes',
    debitAccountLabel: 'Receiving Account',
    creditAccountLabel: 'Income Account',

    debitAccountsEndpoint: () => this.incomeEndpoint.getReceivingAccountsEndpoint<AccountLookup[]>(),
    creditAccountsEndpoint: () => this.incomeEndpoint.getIncomeAccountsEndpoint<AccountLookup[]>(),

    listEndpoint: (query) => {
      const incomeQuery: IncomeListQuery = {
        search: query.search,
        fromDate: query.fromDate,
        toDate: query.toDate,
        page: query.page,
        pageSize: query.pageSize
      };
      return this.incomeEndpoint.getIncomesEndpoint<PagedTransactionResult>(incomeQuery);
    },
    nextTranIdEndpoint: () => this.incomeEndpoint.getNextTranIdEndpoint<TranIdResponse>(),
    entriesByTranIdEndpoint: (tranId) => this.incomeEndpoint.getIncomeEntriesByTranIdEndpoint<TransactionEntry[]>(tranId),

    saveBatchEndpoint: (entries, tranId) => this.incomeEndpoint.getNewIncomesBatchEndpoint<BatchSaveResult>(entries, tranId),
    updateByTranIdEndpoint: (tranId, entries) => this.incomeEndpoint.getUpdateIncomeByTranIdEndpoint<BatchSaveResult>(tranId, entries),
    deleteTranIdEndpoint: (tranId, period, coyID) => this.incomeEndpoint.getDeleteIncomeByTranIdEndpoint<void>(tranId, period, coyID),
  };
}
