import { Component } from '@angular/core';

/**
 * Income wrapper component - uses TransactionListComponent with income-specific configuration.
 * 
 * Note: IncomeEndpoint needs to be created with methods:
 * - getIncomeAccountsEndpoint()
 * - getIncomeBankAccountsEndpoint()
 * - getIncomeEndpoint(query)
 * - getNextTranIdEndpoint()
 * - getIncomeEntriesByTranIdEndpoint(tranId)
 * - getNewIncomeBatchEndpoint(entries, tranId)
 * - getUpdateIncomeByTranIdEndpoint(tranId, entries)
 * - getDeleteIncomeByTranIdEndpoint(tranId, period, coyID)
 * 
 * For now, this component is a stub ready for IncomeEndpoint implementation.
 */
@Component({
  selector: 'app-incomes',
  standalone: true,
  imports: [],
  template: `
    <div class="page-shell">
      <h2>Income Transactions - Coming Soon</h2>
      <p>Waiting for IncomeEndpoint service implementation</p>
      <p style="color: #666; font-size: 0.9rem;">
        The reusable TransactionListComponent is ready. 
        Create IncomeEndpoint with the required methods to enable this page.
      </p>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
  `]
})
export class IncomesComponent {
}
