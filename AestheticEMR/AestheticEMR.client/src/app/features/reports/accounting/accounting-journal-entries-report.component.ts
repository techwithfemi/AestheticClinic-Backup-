import { Component } from '@angular/core';
import { AccountingJournalReportBaseComponent } from './accounting-journal-report-base.component';

@Component({
  selector: 'app-accounting-journal-entries-report',
  standalone: true,
  imports: [AccountingJournalReportBaseComponent],
  template: `
    <app-accounting-journal-report-base
      reportType="all"
      title="Accounting Journal Entries Report"
      subtitle="Day Book style listing">
    </app-accounting-journal-report-base>
  `
})
export class AccountingJournalEntriesReportComponent {}
