import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ServiceTariffEndpoint } from '../../../services/service-tariff-endpoint.service';
import { ServiceTariff } from '../../../models/legacy/service-tariff.model';
import { TariffCompany } from '../../../models/legacy/tariff-company.model';
import { TariffServiceDialogComponent } from './tariff-service-dialog.component';
import { TariffUploadDialogComponent, TariffUploadDialogResult } from './tariff-upload-dialog.component';

@Component({
  selector: 'app-tariff-services',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatProgressSpinnerModule,
    MatDialogModule
  ],
  animations: [fadeInOut],
  template: `
    <div [@fadeInOut] class="tariff-services-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>Tariff Services</mat-card-title>
          <mat-card-subtitle>Upload and maintain company service tariff items.</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <div class="toolbar-grid">

            <!-- Row 1: Quick Search spanning full width -->
            <div class="search-row">
              <mat-form-field appearance="outline" class="search-field">
                <mat-label>Quick Search</mat-label>
                <mat-icon matPrefix>search</mat-icon>
                <input matInput placeholder="Search service name..." [(ngModel)]="searchText" (input)="onSearchChanged()" />
              </mat-form-field>
            </div>

            <!-- Row 2: Company | Category | Actions -->
            <div class="selectors-row">
              <!-- 1. Company -->
              <mat-form-field appearance="outline">
                <mat-label>Template for</mat-label>
                <mat-select [(ngModel)]="selectedCoyId" (selectionChange)="onCompanyChange()">
                  <mat-option value="">-- Select Company --</mat-option>
                  @for (company of companies; track company.coyId) {
                    <mat-option [value]="company.coyId">{{ company.company }} [{{ company.coyId }}]</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <!-- 2. Tariff Category (ng-select inside mat-form-field outline) -->
              <mat-form-field appearance="outline" class="category-field">
                <mat-label>Tariff Category</mat-label>
                <mat-select [(ngModel)]="selectedCategory" (selectionChange)="onCategoryChange()">
                  <mat-option value="">-- Select Category --</mat-option>
                  @for (cat of tariffCategories; track cat) {
                    <mat-option [value]="cat">{{ cat }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              <!-- 3. Actions -->
              <div class="actions-cell">
                <span [title]="!selectedCoyId || !selectedCategory ? 'Select a company and category first' : ''"
                      class="btn-wrap" [class.btn-disabled]="!selectedCoyId || !selectedCategory">
                  <button mat-raised-button type="button" (click)="openUploadDialog()"
                          [disabled]="!selectedCoyId || !selectedCategory">
                    <mat-icon>upload_file</mat-icon>
                    Upload Data
                  </button>
                </span>

                <span [title]="!selectedCoyId || !selectedCategory ? 'Select a company and category first' : ''"
                      class="btn-wrap" [class.btn-disabled]="!selectedCoyId || !selectedCategory">
                  <button mat-stroked-button type="button" (click)="loadTariffs()"
                          [disabled]="!selectedCoyId || !selectedCategory">
                    <mat-icon>refresh</mat-icon>
                    Refresh
                  </button>
                </span>
              </div>
            </div>

          </div>
        </mat-card-content>
      </mat-card>

      <mat-card class="table-card">
        <mat-card-content>
          @if (!selectedCoyId || !selectedCategory) {
            <p class="empty-text">Select a company and tariff category to view records.</p>
          } @else {
          <div class="table-wrap">
            <table mat-table [dataSource]="pagedTariffs" class="tariff-table">
              <ng-container matColumnDef="service">
                <th mat-header-cell *matHeaderCellDef>Service</th>
                <td mat-cell *matCellDef="let item">{{ item.service }}</td>
              </ng-container>

              <ng-container matColumnDef="price">
                <th mat-header-cell *matHeaderCellDef class="text-end">Price</th>
                <td mat-cell *matCellDef="let item" class="text-end">{{ item.price ?? 0 | number:'1.2-2' }}</td>
              </ng-container>

              <ng-container matColumnDef="company">
                <th mat-header-cell *matHeaderCellDef>Company</th>
                <td mat-cell *matCellDef="let item">{{ item.coyName || item.company }}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef class="text-end">Update</th>
                <td mat-cell *matCellDef="let item" class="text-end">
                  <button mat-icon-button type="button" (click)="openEditDialog(item)" title="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
            </table>
          </div>

          @if (tariffs.length > 0) {
            <div class="pager-wrap">
              <small class="pager-text">
                Showing {{ (currentPage - 1) * pageSize + 1 }}-
                {{ (currentPage * pageSize) < tariffs.length ? (currentPage * pageSize) : tariffs.length }} of {{ tariffs.length }}
              </small>
              <div class="pager-actions">
                <button mat-stroked-button type="button" (click)="goToPage(currentPage - 1)" [disabled]="currentPage === 1">
                  Previous
                </button>
                <button mat-stroked-button type="button" disabled>
                  Page {{ currentPage }} / {{ totalPages }}
                </button>
                <button mat-stroked-button type="button" (click)="goToPage(currentPage + 1)" [disabled]="currentPage === totalPages">
                  Next
                </button>
              </div>
            </div>
          }

          @if (tariffs.length === 0 && !loadingIndicator) {
            <p class="empty-text">No tariff items found</p>
          }

          @if (loadingIndicator) {
            <div class="spinner-wrap">
              <mat-spinner diameter="32"></mat-spinner>
            </div>
          }
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .tariff-services-container { padding: 20px; display: grid; gap: 12px; }

    /* Two-row toolbar layout */
    .toolbar-grid { display: flex; flex-direction: column; gap: 10px; }

    /* Row 1 — full-width search */
    .search-row { display: flex; width: 100%; }
    .search-field { width: 100%; }

    /* Row 2 — company | category | actions */
    .selectors-row { display: grid; grid-template-columns: 2fr 1.5fr auto; gap: 12px; align-items: start; }

    .actions-cell { display: flex; gap: 8px; align-items: center; padding-top: 4px; flex-wrap: wrap; }

    /* Disabled button wrapper — shows not-allowed cursor and tooltip */
    .btn-wrap { display: inline-flex; }
    .btn-wrap.btn-disabled { cursor: not-allowed; }
    .btn-wrap.btn-disabled button { pointer-events: none; }

    .table-wrap { overflow: auto; -webkit-overflow-scrolling: touch; }
    .tariff-table { width: 100%; min-width: 620px; }
    .text-end { text-align: right; }
    .empty-text { margin: 12px 0 0; color: #777; }
    .spinner-wrap { display: flex; justify-content: center; padding: 12px 0 4px; }
    .pager-wrap { display: flex; justify-content: space-between; align-items: center; gap: 12px; margin-top: 12px; flex-wrap: wrap; }
    .pager-text { color: #777; }
    .pager-actions { display: flex; gap: 8px; flex-wrap: wrap; }

    .category-field { width: 100%; }

    @media (max-width: 992px) {
      .tariff-services-container { padding: 16px; }
      .selectors-row { grid-template-columns: 1fr 1fr; }
      .actions-cell { grid-column: 1 / -1; padding-top: 0; }
      .actions-cell .btn-wrap { flex: 1 1 160px; }
      .actions-cell button { width: 100%; min-height: 44px; }
      .pager-wrap { flex-direction: column; align-items: stretch; }
    }

    @media (max-width: 575.98px) {
      .tariff-services-container { padding: 12px; }
      .selectors-row { grid-template-columns: 1fr; }
      .actions-cell .btn-wrap,
      .pager-actions button { width: 100%; }
    }
  `]
})
export class TariffServicesComponent {
  private alertService = inject(AlertService);
  private serviceTariffEndpoint = inject(ServiceTariffEndpoint);
  private dialog = inject(MatDialog);

  companies: TariffCompany[] = [];
  allTariffs: ServiceTariff[] = [];   // full unfiltered set for selected company
  tariffs: ServiceTariff[] = [];      // displayed (search + category filtered)

  selectedCoyId = '';
  selectedCategory = '';
  searchText = '';
  loadingIndicator = false;
  displayedColumns = ['service', 'price', 'company', 'actions'];
  readonly pageSize = 10;
  currentPage = 1;

  readonly tariffCategories = ['Drug', 'Investigation', 'Service', 'Product'];

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.tariffs.length / this.pageSize));
  }

  get pagedTariffs(): ServiceTariff[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.tariffs.slice(start, start + this.pageSize);
  }

  constructor() {
    this.loadCompanies();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.currentPage = page;
  }

  onCompanyChange(): void {
    this.loadTariffs();
  }

  onCategoryChange(): void {
    this.applyFilters();
    this.currentPage = 1;
  }

  onSearchChanged(): void {
    this.applyFilters();
    this.currentPage = 1;
  }

  private applyFilters(): void {
    let result = [...this.allTariffs];

    if (this.selectedCategory) {
      result = result.filter(t =>
        (t.usersCat ?? '').trim().toLowerCase() === this.selectedCategory.toLowerCase()
      );
    }

    const term = this.searchText?.trim().toLowerCase() ?? '';
    if (term) {
      result = result.filter(t => (t.service ?? '').toLowerCase().includes(term));
    }

    this.tariffs = result;
  }

  loadCompanies(): void {
    this.serviceTariffEndpoint.getTariffCompaniesEndpoint<TariffCompany[]>().subscribe({
      next: data => {
        this.companies = [...data].sort((a, b) =>
          `${a.company ?? ''} [${a.coyId ?? ''}]`.localeCompare(`${b.company ?? ''} [${b.coyId ?? ''}]`)
        );
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', `Unable to retrieve companies.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  loadTariffs(): void {
    if (!this.selectedCoyId) {
      this.allTariffs = [];
      this.tariffs = [];
      this.currentPage = 1;
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading service tariff...');

    // Load all items for the company — filtering is done client-side
    this.serviceTariffEndpoint.getServiceTariffsEndpoint<ServiceTariff[]>(this.selectedCoyId).subscribe({
      next: data => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.allTariffs = [...data];
        this.applyFilters();
        this.currentPage = 1;
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', `Unable to retrieve service tariff.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  openEditDialog(item: ServiceTariff): void {
    const dialogRef = this.dialog.open(TariffServiceDialogComponent, {
      data: { isEdit: true, item, allTariffs: this.allTariffs },
      width: '520px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (!result) {
        return;
      }

      this.saveDialogResult(result, item);
    });
  }

  openUploadDialog(): void {
    if (!this.selectedCoyId) {
      this.alertService.showMessage('Validation', 'Please select a company first.', MessageSeverity.warn);
      return;
    }

    if (!this.selectedCategory) {
      this.alertService.showMessage('Validation', 'Please select a tariff category first.', MessageSeverity.warn);
      return;
    }

    this.serviceTariffEndpoint.getTariffSourceCompaniesEndpoint<TariffCompany[]>().subscribe({
      next: sourceCompanies => {
        const dialogRef = this.dialog.open(TariffUploadDialogComponent, {
          width: '560px',
          disableClose: true,
          data: {
            sourceCompanies: sourceCompanies.filter(x => x.coyId !== this.selectedCoyId),
            category: this.selectedCategory
          }
        });

        dialogRef.afterClosed().subscribe((result: TariffUploadDialogResult | undefined) => {
          if (!result || (!result.file && !result.sourceCoyId)) {
            return;
          }

          if (result.file) {
            this.alertService.showDialog(
              `This will replace existing [${this.selectedCategory}] tariff records for the selected company with the uploaded file. Continue?`,
              DialogType.confirm,
              () => {
                this.alertService.startLoadingMessage('Uploading tariff data...');
                this.serviceTariffEndpoint.uploadServiceTariffEndpoint<{ inserted: number }>(
                  this.selectedCoyId, result.file!, true, this.selectedCategory
                ).subscribe({
                  next: response => {
                    this.alertService.stopLoadingMessage();
                    this.loadTariffs();
                    const inserted = response?.inserted ?? 0;
                    this.alertService.showMessage('Success', `${inserted} tariff item(s) uploaded successfully.`, MessageSeverity.success);
                  },
                  error: error => {
                    this.alertService.stopLoadingMessage();
                    this.alertService.showStickyMessage('Upload Error', `Unable to upload tariff data.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
                  }
                });
              }
            );

            return;
          }

          const sourceCompany = sourceCompanies.find(x => x.coyId === result.sourceCoyId);
          const sourceLabel = sourceCompany ? `${sourceCompany.company} [${sourceCompany.coyId}]` : result.sourceCoyId;

          this.alertService.showDialog(
            `The [${this.selectedCategory}] tariff from ${sourceLabel} will be applied to the selected company. Continue?`,
            DialogType.confirm,
            () => {
              this.alertService.startLoadingMessage('Applying tariff from existing company...');
              this.serviceTariffEndpoint.copyServiceTariffEndpoint<{ inserted: number }>(
                this.selectedCoyId, result.sourceCoyId!, true, this.selectedCategory
              ).subscribe({
                next: response => {
                  this.alertService.stopLoadingMessage();
                  this.loadTariffs();
                  const inserted = response?.inserted ?? 0;
                  this.alertService.showMessage('Success', `${inserted} tariff item(s) applied successfully.`, MessageSeverity.success);
                },
                error: error => {
                  this.alertService.stopLoadingMessage();
                  this.alertService.showStickyMessage('Copy Error', `Unable to apply tariff.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
                }
              });
            }
          );
        });
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', `Unable to retrieve source companies.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  private saveDialogResult(result: {
    sno: number;
    service: string;
    price: number;
    capitated: string;
    tariffStatus: string;
    revType: string;
    remarks: string;
    usersCat: string;
  }, currentItem: ServiceTariff): void {
    const selectedCompany = this.companies.find(x => x.coyId === this.selectedCoyId);

    const payload: ServiceTariff = {
      sno: currentItem.sno ?? result.sno,
      service: result.service,
      price: Number(result.price),
      company: this.selectedCoyId,
      coyId: this.selectedCoyId,
      coyName: selectedCompany?.company,
      remarks: result.remarks,
      capitated: result.capitated,
      tariffStatus: result.tariffStatus,
      revType: result.revType,
      usersCat: result.usersCat
    };

    if (!payload.sno) {
      this.alertService.showMessage('Validation Error', 'Unable to determine tariff item to update.', MessageSeverity.error);
      return;
    }

    this.alertService.startLoadingMessage();
    this.serviceTariffEndpoint.getUpdateServiceTariffEndpoint<ServiceTariff>(payload.sno, payload).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadTariffs();
        this.alertService.showMessage('Success', 'Tariff item updated successfully.', MessageSeverity.success);
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Update Error', `Unable to update tariff item.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (!error || typeof error !== 'object') {
      return 'Unknown error';
    }

    const source = error as { error?: unknown; message?: unknown };
    if (source.error && typeof source.error === 'object') {
      const inner = source.error as { title?: unknown; detail?: unknown };
      return String(inner.title ?? inner.detail ?? JSON.stringify(source.error));
    }

    return String(source.message ?? JSON.stringify(error));
  }
}
