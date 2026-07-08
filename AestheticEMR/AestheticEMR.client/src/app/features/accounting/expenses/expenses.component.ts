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
import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';
import { ExpenseListQuery } from '../../../models/accounting/expense.model';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class ExpensesComponent {
  private expenseEndpoint = inject(ExpenseEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Expenses',
    translateKeyPrefix: 'expenses',
    debitAccountLabel: 'Expense Account',
    creditAccountLabel: 'Paying Account',

    // Dropdown endpoints
    debitAccountsEndpoint: () => this.expenseEndpoint.getExpenseAccountsEndpoint<AccountLookup[]>(),
    creditAccountsEndpoint: () => this.expenseEndpoint.getPayingAccountsEndpoint<AccountLookup[]>(),

    // List endpoints
    listEndpoint: (query) => {
      const expenseQuery: ExpenseListQuery = {
        search: query.search,
        fromDate: query.fromDate,
        toDate: query.toDate,
        viewMode: query.viewMode as ExpenseListQuery['viewMode'],
        page: query.page,
        pageSize: query.pageSize
      };

      return this.expenseEndpoint.getExpensesEndpoint<PagedTransactionResult>(expenseQuery);
    },
    nextTranIdEndpoint: () => this.expenseEndpoint.getNextTranIdEndpoint<TranIdResponse>(),
    entriesByTranIdEndpoint: (tranId) => this.expenseEndpoint.getExpenseEntriesByTranIdEndpoint<TransactionEntry[]>(tranId),

    // Save/Update/Delete endpoints
    saveBatchEndpoint: (entries, tranId) => this.expenseEndpoint.getNewExpensesBatchEndpoint<BatchSaveResult>(entries, tranId),
    updateByTranIdEndpoint: (tranId, entries) => this.expenseEndpoint.getUpdateExpenseByTranIdEndpoint<BatchSaveResult>(tranId, entries),
    deleteTranIdEndpoint: (tranId, period, coyID) => this.expenseEndpoint.getDeleteExpenseByTranIdEndpoint<void>(tranId, period, coyID),
  };
}
