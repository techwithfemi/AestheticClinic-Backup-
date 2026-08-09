import { Component } from '@angular/core';
import { AccountingJournalReportBaseComponent } from './accounting-journal-report-base.component';

@Component({
  selector: 'app-accounting-incomes-report',
  standalone: true,
  imports: [AccountingJournalReportBaseComponent],
  template: `
    <app-accounting-journal-report-base
      reportType="income"
      title="Accounting Incomes Report"
      subtitle="Journal lines filtered by Income accounts">
    </app-accounting-journal-report-base>
  `
})
export class AccountingIncomesReportComponent {}
