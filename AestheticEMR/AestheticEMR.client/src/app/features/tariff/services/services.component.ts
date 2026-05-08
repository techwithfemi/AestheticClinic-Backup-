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
            <mat-form-field appearance="outline">
              <mat-label>Template for</mat-label>
              <mat-select [(ngModel)]="selectedCoyId" (selectionChange)="loadTariffs()">
                <mat-option value="">-- Select Company --</mat-option>
                @for (company of companies; track company.coyId) {
                  <mat-option [value]="company.coyId">{{ company.company }} [{{ company.coyId }}]</mat-option>
                }
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Quick Search</mat-label>
              <input matInput placeholder="Service name..." [(ngModel)]="searchText" (input)="onSearchChanged()" />
            </mat-form-field>

            <div class="actions-cell">
              <button mat-raised-button type="button" (click)="openUploadDialog()" [disabled]="!selectedCoyId">
                <mat-icon>upload_file</mat-icon>
                Upload Data
              </button>

              <button mat-stroked-button type="button" (click)="loadTariffs()" [disabled]="!selectedCoyId">
                <mat-icon>refresh</mat-icon>
                Refresh
              </button>
            </div>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card class="table-card">
        <mat-card-content>
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
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .tariff-services-container { padding: 20px; display: grid; gap: 12px; }
    .toolbar-grid { display: grid; grid-template-columns: 2fr 2fr auto; gap: 12px; align-items: start; }
    .actions-cell { display: flex; gap: 8px; align-items: center; padding-top: 4px; flex-wrap: wrap; }
    .table-wrap { overflow: auto; }
    .tariff-table { width: 100%; }
    .text-end { text-align: right; }
    .empty-text { margin: 12px 0 0; color: #777; }
    .spinner-wrap { display: flex; justify-content: center; padding: 12px 0 4px; }
    .pager-wrap { display: flex; justify-content: space-between; align-items: center; gap: 12px; margin-top: 12px; flex-wrap: wrap; }
    .pager-text { color: #777; }
    .pager-actions { display: flex; gap: 8px; flex-wrap: wrap; }
    @media (max-width: 992px) {
      .toolbar-grid { grid-template-columns: 1fr; }
      .actions-cell { padding-top: 0; }
      .pager-wrap { flex-direction: column; align-items: stretch; }
    }
  `]
})
export class TariffServicesComponent {
  private alertService = inject(AlertService);
  private serviceTariffEndpoint = inject(ServiceTariffEndpoint);
  private dialog = inject(MatDialog);

  companies: TariffCompany[] = [];
  tariffs: ServiceTariff[] = [];
  selectedCoyId = '';
  searchText = '';
  loadingIndicator = false;
  displayedColumns = ['service', 'price', 'company', 'actions'];
  readonly pageSize = 10;
  currentPage = 1;

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
      this.tariffs = [];
      this.currentPage = 1;
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading service tariff...');

    this.serviceTariffEndpoint.getServiceTariffsEndpoint<ServiceTariff[]>(this.selectedCoyId, this.searchText).subscribe({
      next: data => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.tariffs = [...data];
        this.currentPage = 1;
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', `Unable to retrieve service tariff.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  onSearchChanged(): void {
    if (!this.selectedCoyId) {
      return;
    }

    this.loadTariffs();
  }

  openEditDialog(item: ServiceTariff): void {
    const dialogRef = this.dialog.open(TariffServiceDialogComponent, {
      data: { isEdit: true, item },
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
      this.alertService.showMessage('Validation', 'Please select company tariff template first.', MessageSeverity.warn);
      return;
    }

    this.serviceTariffEndpoint.getTariffSourceCompaniesEndpoint<TariffCompany[]>().subscribe({
      next: sourceCompanies => {
        const dialogRef = this.dialog.open(TariffUploadDialogComponent, {
          width: '560px',
          disableClose: true,
          data: {
            sourceCompanies: sourceCompanies.filter(x => x.coyId !== this.selectedCoyId)
          }
        });

        dialogRef.afterClosed().subscribe((result: TariffUploadDialogResult | undefined) => {
          if (!result || (!result.file && !result.sourceCoyId)) {
            return;
          }

          if (result.file) {
            this.alertService.showDialog(
              'This will replace any existing tariff records for the selected company with the uploaded file. Do you want to continue?',
              DialogType.confirm,
              () => {
                this.alertService.startLoadingMessage('Uploading tariff data...');
                this.serviceTariffEndpoint.uploadServiceTariffEndpoint<{ inserted: number }>(this.selectedCoyId, result.file!, true).subscribe({
                  next: response => {
                    this.alertService.stopLoadingMessage();
                    this.loadTariffs();
                    const inserted = response?.inserted ?? 0;
                    this.alertService.showMessage('Success', `${inserted} tariff item(s) uploaded successfully. Existing company tariff upload was replaced.`, MessageSeverity.success);
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
            `No file was selected. The tariff for ${sourceLabel} will be used for the selected company. Do you want to continue?`,
            DialogType.confirm,
            () => {
              this.alertService.startLoadingMessage('Applying tariff from existing company...');
              this.serviceTariffEndpoint.copyServiceTariffEndpoint<{ inserted: number }>(this.selectedCoyId, result.sourceCoyId!, true).subscribe({
                next: response => {
                  this.alertService.stopLoadingMessage();
                  this.loadTariffs();
                  const inserted = response?.inserted ?? 0;
                  this.alertService.showMessage('Success', `${inserted} tariff item(s) applied successfully from the selected company.`, MessageSeverity.success);
                },
                error: error => {
                  this.alertService.stopLoadingMessage();
                  this.alertService.showStickyMessage('Copy Error', `Unable to apply tariff from the selected company.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
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

    if (typeof source.message === 'string' && source.message) {
      return source.message;
    }

    if (source.error && typeof source.error === 'object') {
      const errorBody = source.error as { errors?: Record<string, string[]>; title?: string; message?: string };

      if (typeof errorBody.message === 'string' && errorBody.message) {
        return errorBody.message;
      }

      if (typeof errorBody.title === 'string' && errorBody.title) {
        return errorBody.title;
      }

      if (errorBody.errors) {
        const firstErrorGroup = Object.values(errorBody.errors)[0];
        if (Array.isArray(firstErrorGroup) && firstErrorGroup.length > 0) {
          return firstErrorGroup[0];
        }
      }
    }

    return 'Unable to process request';
  }
}
