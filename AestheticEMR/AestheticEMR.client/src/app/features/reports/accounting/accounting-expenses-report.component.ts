import { Component } from '@angular/core';
import { AccountingJournalReportBaseComponent } from './accounting-journal-report-base.component';

@Component({
  selector: 'app-accounting-expenses-report',
  standalone: true,
  imports: [AccountingJournalReportBaseComponent],
  template: `
    <app-accounting-journal-report-base
      reportType="expense"
      title="Accounting Expenses Report"
      subtitle="Journal lines filtered by Expense accounts">
    </app-accounting-journal-report-base>
  `
})
export class AccountingExpensesReportComponent {}
