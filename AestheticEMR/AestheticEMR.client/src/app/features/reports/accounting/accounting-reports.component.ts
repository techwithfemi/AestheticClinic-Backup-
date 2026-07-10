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

import { AestheticEndpoint, BalanceSheetHeader } from '../../../services/aesthetic-endpoint.service';
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
              <mat-form-field appearance="outline"><mat-label>Company ID</mat-label><input matInput [(ngModel)]="coyID"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Period</mat-label><input matInput [(ngModel)]="period"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Ledger Code</mat-label><input matInput [(ngModel)]="ledgerCode"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Account No</mat-label><input matInput [(ngModel)]="accountNo"></mat-form-field>
            </div>
            <div class="button-row"><button mat-raised-button color="primary" (click)="openGeneralLedger()" [disabled]="loading()">Open PDF</button></div>
          </mat-tab>

          <mat-tab label="Profit & Loss">
            <div class="form-grid">
              <mat-form-field appearance="outline"><mat-label>Company ID</mat-label><input matInput [(ngModel)]="plCoyID"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Period</mat-label><input matInput [(ngModel)]="plPeriod"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Year</mat-label><input matInput [(ngModel)]="plYear"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Report By</mat-label><input matInput [(ngModel)]="plRptBy"></mat-form-field>
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
            <div class="selected-hint" *ngIf="selectedPlHeader() as header">Selected: {{ header.itemName || header.groupID }}</div>
            <div class="button-row">
              <button mat-raised-button color="primary" (click)="openProfitAndLoss()" [disabled]="loading()">Open Summary PDF</button>
              <button mat-stroked-button color="primary" (click)="openProfitAndLossDetails()" [disabled]="loading()">Open Details PDF</button>
            </div>
          </mat-tab>

          <mat-tab label="Balance Sheet">
            <div class="form-grid">
              <mat-form-field appearance="outline"><mat-label>Company ID</mat-label><input matInput [(ngModel)]="bsCoyID"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Period</mat-label><input matInput [(ngModel)]="bsPeriod"></mat-form-field>
              <mat-form-field appearance="outline"><mat-label>Year</mat-label><input matInput [(ngModel)]="bsYear"></mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Report By</mat-label>
                <mat-select [(ngModel)]="bsRptBy" (selectionChange)="onBalanceSheetHeaderSelected()">
                  <mat-option value="Period">Period</mat-option>
                  <mat-option value="Year">Year</mat-option>
                  @for (item of bSHeaders(); track item.period + '-' + item.coyID + '-' + (item.rptType || '')) {
                    <mat-option [value]="item.rptType || 'Period'">{{ item.itemName || item.rptType || 'Period' }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>
            <div class="selected-hint" *ngIf="selectedBsHeader() as header">Selected: {{ header.itemName || header.rptType || 'Period' }}</div>
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
    .button-row { display: flex; gap: 12px; flex-wrap: wrap; }
    .selected-hint { margin: -4px 0 12px; color: rgba(0, 0, 0, 0.66); font-size: 13px; }
    @media (max-width: 768px) { .form-grid { grid-template-columns: 1fr; } }
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
  selectedPlHeader = signal<ProfitAndLossHeader | null>(null);
  selectedBsHeader = signal<BalanceSheetHeader | null>(null);

  coyID = '';
  period = '';
  ledgerCode = 'GL';
  accountNo = '(ALL)';

  plCoyID = '';
  plPeriod = '';
  plYear = '';
  plRptBy = 'Period';
  plGroupID = '';

  bsCoyID = '';
  bsPeriod = '';
  bsYear = '';
  bsRptBy = 'Period';

  constructor() {
    this.loadProfitAndLossHeaders();
    this.loadBalanceSheetHeaders();
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

  onBalanceSheetHeaderSelected(): void {
    const header = this.bSHeaders().find(x => (x.rptType || 'Period') === this.bsRptBy) ?? null;
    this.selectedBsHeader.set(header);
    if (header?.rptType) {
      this.bsRptBy = header.rptType;
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
    this.loading.set(true);
    this.endpoint.getAccountingGeneralLedgerReportEndpoint({ coyID: this.coyID, period: this.period, ledgerCode: this.ledgerCode, accountNo: this.accountNo })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'General Ledger'),
        error: error => this.showError('Unable to load general ledger report', error),
        complete: () => this.loading.set(false)
      });
  }

  openProfitAndLoss(): void {
    this.applyPlSelectionDefaults();
    this.loading.set(true);
    this.endpoint.getAccountingProfitAndLossReportEndpoint({ coyID: this.plCoyID, period: this.plPeriod, year: this.plYear, rptBy: this.plRptBy, isClose: false })
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
    this.endpoint.getAccountingProfitAndLossDetailsReportEndpoint({ coyID: this.plCoyID, period: this.plPeriod, year: this.plYear, rptBy: this.plRptBy, groupID: this.plGroupID, isClose: false })
      .subscribe({
        next: blob => this.openReportDialog(blob, 'Profit & Loss Details'),
        error: error => this.showError('Unable to load profit and loss details report', error),
        complete: () => this.loading.set(false)
      });
  }

  openBalanceSheet(): void {
    this.applyBalanceSheetDefaults();
    this.loading.set(true);
    this.endpoint.getAccountingBalanceSheetReportEndpoint({ coyID: this.bsCoyID, period: this.bsPeriod, year: this.bsYear, rptBy: this.bsRptBy, isClose: false })
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
}
