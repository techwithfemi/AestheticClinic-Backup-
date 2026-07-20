import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import {
  AestheticEndpoint,
  AccountingAccountLookup,
  AccountingLedgerLookup,
  AccountingReportPeriod,
  AccountingReportYear,
  BalanceSheetHeader
} from '../../../services/aesthetic-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { ReportPdfDialogComponent } from '../shared/report-pdf-dialog.component';
import { ProfitAndLossHeader } from '../../../models/accounting/profit-and-loss-header.model';

@Component({
  selector: 'app-accounting-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatIconModule, MatTabsModule, MatDialogModule],
  template: `
    <mat-card class="report-card">
      <mat-card-header>
        <mat-card-title>Accounting Reports</mat-card-title>
        <mat-card-subtitle>Crystal Reports via legacy .NET Framework service</mat-card-subtitle>
      </mat-card-header>

      <mat-card-content>
        <div class="report-picker">
          <button mat-stroked-button color="primary" [class.active]="activeReport() === 'gl'" (click)="activeReport.set('gl')">
            <mat-icon>account_tree</mat-icon>
            General Ledger
          </button>
          <button mat-stroked-button color="primary" [class.active]="activeReport() === 'pl'" (click)="activeReport.set('pl')">
            <mat-icon>query_stats</mat-icon>
            Profit & Loss
          </button>
          <button mat-stroked-button color="primary" [class.active]="activeReport() === 'bs'" (click)="activeReport.set('bs')">
            <mat-icon>account_balance</mat-icon>
            Balance Sheet
          </button>
        </div>

        <mat-tab-group [selectedIndex]="activeReport() === 'pl' ? 1 : activeReport() === 'bs' ? 2 : 0" (selectedIndexChange)="activeReport.set($event === 1 ? 'pl' : $event === 2 ? 'bs' : 'gl')">
          <mat-tab label="General Ledger">
            <div class="form-grid">
              <mat-form-field appearance="outline">
                <mat-label>Year</mat-label>
                <mat-select [(ngModel)]="selectedGlYear" (selectionChange)="onGlYearChanged()">
                  @for (item of glYears(); track item.periodYr) {
                    <mat-option [value]="item.periodYr">{{ item.periodYr }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Period</mat-label>
                <mat-select [(ngModel)]="period" (selectionChange)="onGlPeriodChanged()">
                  @for (item of glPeriods(); track item.period + '-' + (item.periodVal || '') + '-' + item.prdClose) {
                    <mat-option [value]="item.period">{{ item.period || '-- Select Period --' }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Ledger Type</mat-label>
                <mat-select [(ngModel)]="ledgerCode" (selectionChange)="onGlLedgerChanged()">
                  @for (item of glLedgers(); track item.ledgerCode + '-' + item.ledger) {
                    <mat-option [value]="item.ledgerCode">{{ item.ledger || '-- Select Ledger --' }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Account</mat-label>
                <mat-select [(ngModel)]="accountNo" (selectionChange)="onGlAccountChanged()">
                  @for (item of glAccounts(); track item.accountNo + '-' + item.accountName) {
                    <mat-option [value]="item.accountNo">{{ item.accountName }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>
            @if (selectedGlLedger(); as ledger) {
              <div class="selected-hint">Ledger: {{ ledger.ledger }}</div>
            }
            @if (selectedGlAccount(); as account) {
              <div class="selected-hint">Account: {{ account.accountName }}</div>
            }
            <div class="button-row"><button mat-raised-button color="primary" (click)="openGeneralLedger()" [disabled]="loading()">Open PDF</button></div>
          </mat-tab>

          <mat-tab label="Profit & Loss">
            <div class="form-grid">
              <mat-form-field appearance="outline">
                <mat-label>Year</mat-label>
                <mat-select [(ngModel)]="plYear" (selectionChange)="onPlYearChanged()">
                  @for (item of glYears(); track item.periodYr) {
                    <mat-option [value]="item.periodYr">{{ item.periodYr }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Period</mat-label>
                <mat-select [(ngModel)]="plPeriod" (selectionChange)="onPlPeriodChanged()">
                  @for (item of plPeriods(); track item.period) {
                    <mat-option [value]="item.period">{{ item.period }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Report By</mat-label>
                <mat-select [(ngModel)]="plRptBy">
                  @for (opt of plRptByOptions(); track opt) {
                    <mat-option [value]="opt">{{ opt }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Detail Group</mat-label>
                <mat-select [(ngModel)]="plGroupID" (selectionChange)="onProfitAndLossHeaderSelected()">
                  <mat-option value="">-- Select Header --</mat-option>
                  @for (item of pAndLHeaders(); track item.groupID) {
                    <mat-option [value]="item.groupID">{{ item.itemName || item.groupID }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>
            @if (selectedPlHeader(); as header) {
              <div class="selected-hint">Selected: {{ header.itemName || header.groupID }}</div>
            }
            <div class="button-row">
              <button mat-raised-button color="primary" (click)="openProfitAndLoss()" [disabled]="loading()">Open Summary PDF</button>
              <button mat-stroked-button color="primary" (click)="openProfitAndLossDetails()" [disabled]="loading()">Open Details PDF</button>
            </div>
          </mat-tab>

          <mat-tab label="Balance Sheet">
            <div class="form-grid">
              <mat-form-field appearance="outline">
                <mat-label>Year</mat-label>
                <mat-select [(ngModel)]="bsYear" (selectionChange)="onBsYearChanged()">
                  @for (item of glYears(); track item.periodYr) {
                    <mat-option [value]="item.periodYr">{{ item.periodYr }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Period</mat-label>
                <mat-select [(ngModel)]="bsPeriod">
                  @for (item of bsPeriods(); track item.period) {
                    <mat-option [value]="item.period">{{ item.period }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Report By</mat-label>
                <mat-select [(ngModel)]="bsRptBy">
                  <mat-option value="Period">Period</mat-option>
                  <mat-option value="Year">Year</mat-option>
                </mat-select>
              </mat-form-field>
            </div>
            <div class="button-row">
              <button mat-raised-button color="primary" (click)="openBalanceSheet()" [disabled]="loading()">Open PDF</button>
            </div>
          </mat-tab>
        </mat-tab-group>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .report-card { margin: 16px; }
    .report-picker { display: flex; gap: 12px; flex-wrap: wrap; margin-bottom: 16px; }
    .report-picker button.active { border-width: 2px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; margin: 16px 0; }
    .span-2 { grid-column: span 2; }
    .button-row { display: flex; gap: 12px; flex-wrap: wrap; }
    .selected-hint { margin: -4px 0 12px; color: rgba(0, 0, 0, 0.66); font-size: 13px; }
    @media (max-width: 768px) { .form-grid { grid-template-columns: 1fr; } .span-2 { grid-column: auto; } }
  `]
})
export class AccountingReportsComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loading = signal(false);
  activeReport = signal<'gl' | 'pl' | 'bs'>('gl');
  pAndLHeaders = signal<ProfitAndLossHeader[]>([]);
  bSHeaders = signal<BalanceSheetHeader[]>([]);
  glYears = signal<AccountingReportYear[]>([]);
  glPeriods = signal<AccountingReportPeriod[]>([]);
  plPeriods = signal<AccountingReportPeriod[]>([]);
  bsPeriods = signal<AccountingReportPeriod[]>([]);
  glLedgers = signal<AccountingLedgerLookup[]>([]);
  glAccounts = signal<AccountingAccountLookup[]>([]);
  selectedPlHeader = signal<ProfitAndLossHeader | null>(null);
  selectedBsHeader = signal<BalanceSheetHeader | null>(null);
  selectedGlLedger = signal<AccountingLedgerLookup | null>(null);
  selectedGlAccount = signal<AccountingAccountLookup | null>(null);

  coyID = '';
  selectedGlYear = '';
  period = '';
  ledgerCode = '';
  accountNo = '';

  plCoyID = '';
  plPeriod = '';
  plYear = '';
  plRptBy = 'Period';
  plGroupID = '';

  bsCoyID = '';
  bsPeriod = '';
  bsYear = '';
  bsRptBy = 'Period';

  plRptByOptions = signal<string[]>(['Period', 'Year']);

  constructor() {
    this.loadAccountingDefaults();
    this.loadGeneralLedgerYears();
    this.loadGeneralLedgerLedgers();
    this.loadProfitAndLossHeaders();
    this.loadBalanceSheetHeaders();
  }

  loadAccountingDefaults(): void {
    this.endpoint.getAccountingReportDefaultsEndpoint().subscribe({
      next: defaults => {
        const coyID = defaults?.coyID?.trim() ?? '';
        this.coyID = coyID;
        this.plCoyID = coyID;
        this.bsCoyID = coyID;

        if (this.coyID && this.selectedGlYear) {
          this.loadGeneralLedgerPeriods();
        }
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load Accounting defaults.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadGeneralLedgerYears(): void {
    this.endpoint.getAccountingGeneralLedgerYearsEndpoint().subscribe({
      next: years => {
        this.glYears.set((years ?? []).filter(x => !!x.periodYr));
        if (!this.selectedGlYear && this.glYears().length > 0) {
          this.selectedGlYear = this.glYears()[0].periodYr;
        }

        if (!this.plYear && this.selectedGlYear) {
          this.plYear = this.selectedGlYear;
        }
        if (!this.bsYear && this.selectedGlYear) {
          this.bsYear = this.selectedGlYear;
        }

        if (this.coyID.trim() && this.selectedGlYear) {
          this.loadGeneralLedgerPeriods();
          this.loadPlPeriods();
          this.loadBsPeriods();
        }
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load General Ledger years.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadGeneralLedgerLedgers(): void {
    this.endpoint.getAccountingGeneralLedgerLedgersEndpoint().subscribe({
      next: ledgers => {
        this.glLedgers.set((ledgers ?? []).filter(x => !!x.ledgerCode && !!x.ledger));
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load General Ledger ledgers.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadGeneralLedgerPeriods(): void {
    this.endpoint.getAccountingGeneralLedgerPeriodsEndpoint(this.coyID.trim(), this.selectedGlYear).subscribe({
      next: periods => {
        const validPeriods = (periods ?? []).filter(x => !!x.period);
        this.glPeriods.set(validPeriods);

        if (validPeriods.length === 0) {
          this.period = '';
          this.glAccounts.set([]);
          this.accountNo = '';
          this.selectedGlAccount.set(null);
          return;
        }

        this.period = validPeriods[0].period;
        this.onGlPeriodChanged();
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load General Ledger periods.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadPlPeriods(): void {
    if (!this.coyID.trim() || !this.plYear) return;
    this.endpoint.getAccountingGeneralLedgerPeriodsEndpoint(this.coyID.trim(), this.plYear).subscribe({
      next: periods => {
        const valid = (periods ?? []).filter(x => !!x.period);
        // VB: cboYr_SelectedIndexChanged adds blank + CONSOLIDATED + actual periods
        const withConsolidated: AccountingReportPeriod[] = [
          { period: 'CONSOLIDATED', periodVal: null, prdClose: '', isClose: false },
          ...valid
        ];
        this.plPeriods.set(withConsolidated);
        if (!this.plPeriod && valid.length > 0) {
          this.plPeriod = valid[0].period;
        }
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load P&L periods.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadBsPeriods(): void {
    if (!this.coyID.trim() || !this.bsYear) return;
    this.endpoint.getAccountingGeneralLedgerPeriodsEndpoint(this.coyID.trim(), this.bsYear).subscribe({
      next: periods => {
        const valid = (periods ?? []).filter(x => !!x.period);
        this.bsPeriods.set(valid);
        if (!this.bsPeriod && valid.length > 0) {
          this.bsPeriod = valid[0].period;
        }
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load Balance Sheet periods.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  onPlYearChanged(): void {
    this.plPeriod = '';
    this.plPeriods.set([]);
    this.loadPlPeriods();
  }

  onBsYearChanged(): void {
    this.bsPeriod = '';
    this.bsPeriods.set([]);
    this.loadBsPeriods();
  }

  onGlCompanyChanged(): void {
    this.period = '';
    this.glPeriods.set([]);
    this.ledgerCode = '';
    this.glAccounts.set([]);
    this.accountNo = '';
    this.selectedGlLedger.set(null);
    this.selectedGlAccount.set(null);

    if (this.coyID.trim() && this.selectedGlYear) {
      this.loadGeneralLedgerPeriods();
    }
  }

  onGlYearChanged(): void {
    this.period = '';
    this.glPeriods.set([]);
    this.ledgerCode = '';
    this.glAccounts.set([]);
    this.accountNo = '';
    this.selectedGlLedger.set(null);
    this.selectedGlAccount.set(null);

    if (this.coyID.trim() && this.selectedGlYear) {
      this.loadGeneralLedgerPeriods();
    }
  }

  onGlPeriodChanged(): void {
    this.glAccounts.set([]);
    this.accountNo = '';
    this.selectedGlAccount.set(null);

    if (!this.ledgerCode) {
      const firstLedger = this.glLedgers()[0] ?? null;
      if (firstLedger) {
        this.ledgerCode = firstLedger.ledgerCode;
        this.selectedGlLedger.set(firstLedger);
      }
    }

    if (this.ledgerCode) {
      this.onGlLedgerChanged();
    }
  }

  onGlLedgerChanged(): void {
    const ledger = this.glLedgers().find(x => x.ledgerCode === this.ledgerCode) ?? null;
    this.selectedGlLedger.set(ledger);
    this.glAccounts.set([]);
    this.accountNo = '';
    this.selectedGlAccount.set(null);

    if (!this.coyID.trim() || !this.period || !this.ledgerCode) {
      return;
    }

    this.endpoint.getAccountingGeneralLedgerAccountsEndpoint(this.coyID.trim(), this.period, this.ledgerCode).subscribe({
      next: accounts => {
        const validAccounts = (accounts ?? []).filter(x => !!x.accountNo && !!x.accountName);
        this.glAccounts.set(validAccounts);
        const first = validAccounts[0] ?? null;
        this.accountNo = first?.accountNo ?? '';
        this.selectedGlAccount.set(first);
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load General Ledger accounts.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  onGlAccountChanged(): void {
    const account = this.glAccounts().find(x => x.accountNo === this.accountNo) ?? null;
    this.selectedGlAccount.set(account);
  }

  loadProfitAndLossHeaders(): void {
    this.endpoint.getAccountingProfitAndLossHeadersEndpoint().subscribe({
      next: headers => {
        this.pAndLHeaders.set(headers ?? []);
        this.applyPlSelectionDefaults();
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load Profit & Loss headers.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  loadBalanceSheetHeaders(): void {
    this.endpoint.getAccountingBalanceSheetHeadersEndpoint().subscribe({
      next: headers => {
        this.bSHeaders.set(headers ?? []);
        this.applyBalanceSheetDefaults();
      },
      error: error => this.alertService.showStickyMessage('Load Error', `Unable to load Balance Sheet headers.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.warn, error)
    });
  }

  onProfitAndLossHeaderSelected(): void {
    const header = this.pAndLHeaders().find(x => x.groupID === this.plGroupID) ?? null;
    this.selectedPlHeader.set(header);
    if (header) {
      this.plGroupID = header.groupID;
    }
  }

  applyPlSelectionDefaults(): void {
    if (!this.plGroupID) {
      const headers = this.pAndLHeaders();
      if (headers.length > 0) {
        this.plGroupID = headers[0].groupID;
        this.selectedPlHeader.set(headers[0]);
      }
    }
  }

  applyBalanceSheetDefaults(): void {
    const headers = this.bSHeaders();
    if (headers.length > 0 && !this.selectedBsHeader()) {
      const header = headers[0];
      this.selectedBsHeader.set(header);
      if (header.rptType) {
        this.bsRptBy = header.rptType;
      }
    }
  }

  openGeneralLedger(): void {
    if (!this.coyID.trim()) {
      this.alertService.showMessage('Validation', 'Specify Company ID.', MessageSeverity.warn);
      return;
    }

    if (!this.selectedGlYear) {
      this.alertService.showMessage('Validation', 'Specify Fin Year.', MessageSeverity.warn);
      return;
    }

    if (!this.period) {
      this.alertService.showMessage('Validation', 'Specify Fin Period.', MessageSeverity.warn);
      return;
    }

    if (!this.ledgerCode) {
      this.alertService.showMessage('Validation', 'Specify Ledger Type.', MessageSeverity.warn);
      return;
    }

    if (!this.accountNo) {
      this.alertService.showMessage('Validation', 'Specify Account Name or Choose (ALL) to Display all Accounts under this Ledger.', MessageSeverity.warn);
      return;
    }

    const ledgerDisplayText = this.selectedGlLedger()?.ledger ?? '';
    const accountDisplayText = this.selectedGlAccount()?.accountName ?? '';

    this.loading.set(true);
    this.endpoint.getAccountingGeneralLedgerReportEndpoint({
      coyID: this.coyID.trim(),
      period: this.period,
      ledgerCode: this.ledgerCode,
      accountNo: this.accountNo,
      ledgerDisplayText,
      accountDisplayText
    })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'General Ledger'),
        error: error => this.showError('Unable to load general ledger report', error),
        complete: () => this.loading.set(false)
      });
  }

  openProfitAndLoss(): void {
    this.applyPlSelectionDefaults();
    this.loading.set(true);
    this.endpoint.getAccountingProfitAndLossReportEndpoint({ coyID: this.plCoyID.trim(), period: this.plPeriod, year: this.plYear, rptBy: this.plRptBy, isClose: false })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'Profit & Loss Summary'),
        error: error => this.showError('Unable to load profit and loss report', error),
        complete: () => this.loading.set(false)
      });
  }

  openProfitAndLossDetails(): void {
    this.applyPlSelectionDefaults();
    if (!this.plGroupID) {
      this.alertService.showMessage('Validation', 'Group ID is required for report details.', MessageSeverity.warn);
      return;
    }

    this.loading.set(true);
    this.endpoint.getAccountingProfitAndLossDetailsReportEndpoint({ coyID: this.plCoyID.trim(), period: this.plPeriod, year: this.plYear, rptBy: this.plRptBy, groupID: this.plGroupID, isClose: false })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'Profit & Loss Details'),
        error: error => this.showError('Unable to load profit and loss details report', error),
        complete: () => this.loading.set(false)
      });
  }

  openBalanceSheet(): void {
    this.applyBalanceSheetDefaults();
    this.loading.set(true);
    this.endpoint.getAccountingBalanceSheetReportEndpoint({ coyID: this.bsCoyID.trim(), period: this.bsPeriod, year: this.bsYear, rptBy: this.bsRptBy, isClose: false })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'Balance Sheet'),
        error: error => this.showError('Unable to load balance sheet report', error),
        complete: () => this.loading.set(false)
      });
  }

  private openReportDialog(blob: Blob, title: string): void {
    const url = URL.createObjectURL(blob);
    const dialogRef = this.dialog.open(ReportPdfDialogComponent, {
      data: { title, blobUrl: url },
      width: 'min(1200px, 96vw)',
      maxWidth: '96vw',
      height: 'min(900px, 96vh)',
      autoFocus: false,
      disableClose: false
    });

    dialogRef.afterClosed().subscribe(() => URL.revokeObjectURL(url));
  }

  private showError(message: string, error: unknown): void {
    this.loading.set(false);
    this.alertService.showStickyMessage('Load Error', `${message}.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (error && typeof error === 'object' && 'message' in error) return String((error as { message?: unknown }).message ?? error);
    return String(error);
  }

  onPlPeriodChanged(): void {
    if (this.plPeriod === 'CONSOLIDATED') {
      this.plRptByOptions.set(['QTR_1', 'QTR_2', 'QTR_3', 'QTR_4', 'HALF_YR_1', 'HALF_YR_2']);
      this.plRptBy = 'QTR_1';
    } else {
      this.plRptByOptions.set(['Period', 'Year']);
      this.plRptBy = 'Period';
    }
  }
}
