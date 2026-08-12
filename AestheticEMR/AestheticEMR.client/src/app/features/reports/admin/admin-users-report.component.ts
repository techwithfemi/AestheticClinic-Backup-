import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import * as XLSX from 'xlsx';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint, AdminUsersReportRow } from '../../../services/aesthetic-endpoint.service';
import { fadeInOut } from '../../../services/animations';
import { UtcDisplayPipe } from '../../../pipes/utc-display.pipe';

@Component({
  selector: 'app-admin-users-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    NgSelectModule,
    TranslateModule,
    UtcDisplayPipe
  ],
  animations: [fadeInOut],
  templateUrl: './admin-users-report.component.html',
  styleUrl: './admin-users-report.component.scss'
})
export class AdminUsersReportComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;

  readonly rows = signal<AdminUsersReportRow[]>([]);
  readonly rowsCache = signal<AdminUsersReportRow[]>([]);

  searchText = '';
  selectedStatus = '';
  readonly appliedSearch = signal('');
  readonly appliedStatus = signal('');

  readonly pageSize = 10;
  readonly currentPage = signal(0);
  readonly displayedColumns = ['user', 'name', 'jobTitle', 'email', 'phone', 'enabled', 'createdDate', 'updatedDate'];

  readonly statusOptions = computed(() => ['Enabled', 'Disabled']);

  readonly filteredRows = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    const status = this.appliedStatus().trim().toLowerCase();

    return this.rowsCache().filter(row => {
      if (status === 'enabled' && !row.isEnabled) {
        return false;
      }

      if (status === 'disabled' && row.isEnabled) {
        return false;
      }

      if (!term) {
        return true;
      }

      return [
        row.fullName,
        row.userName,
        row.jobTitle,
        row.email,
        row.phoneNumber,
        row.configuration
      ].some(value => (value ?? '').toString().toLowerCase().includes(term));
    });
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filteredRows().slice(start, start + this.pageSize);
  });

  readonly totalUsers = computed(() => this.filteredRows().length);
  readonly enabledUsers = computed(() => this.filteredRows().filter(row => row.isEnabled).length);
  readonly disabledUsers = computed(() => this.filteredRows().filter(row => !row.isEnabled).length);
  readonly twoFactorUsers = computed(() => this.filteredRows().filter(row => row.twoFactorEnabled).length);

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading admin users report...');

    this.endpoint.getAdminUsersReportEndpoint<AdminUsersReportRow[]>()
      .subscribe({
        next: rows => {
          const orderedRows = [...(rows ?? [])].sort((a, b) => (a.fullName ?? a.userName).localeCompare(b.fullName ?? b.userName));
          this.rowsCache.set(orderedRows);
          this.rows.set(orderedRows);
          this.currentPage.set(0);
          this.runReport();
        },
        error: error => {
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to load admin users report.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        },
        complete: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
        }
      });
  }

  runReport(): void {
    this.appliedSearch.set(this.searchText);
    this.appliedStatus.set(this.selectedStatus);
    this.currentPage.set(0);
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedStatus = '';
    this.runReport();
  }

  onPageChanged(event: PageEvent): void {
    this.currentPage.set(event.pageIndex);
  }

  printReport(): void {
    window.print();
  }

  exportExcel(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const worksheet = XLSX.utils.json_to_sheet(rows.map(row => ({
      'Job Title': row.jobTitle,
      'Full Name': row.fullName,
      'User Name': row.userName,
      Email: row.email ?? '',
      'Phone Number': row.phoneNumber ?? '',
      Enabled: row.isEnabled ? 'Yes' : 'No',
      'Email Confirmed': row.emailConfirmed ? 'Yes' : 'No',
      'Phone Confirmed': row.phoneNumberConfirmed ? 'Yes' : 'No',
      'Two Factor Enabled': row.twoFactorEnabled ? 'Yes' : 'No',
      'Created Date': this.formatDate(row.createdDate),
      'Updated Date': this.formatDate(row.updatedDate)
    })));
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'AdminUsers');
    XLSX.writeFile(workbook, 'admin-users-report.xlsx');
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Job Title', 'Full Name', 'User Name', 'Email', 'Phone Number', 'Enabled', 'Email Confirmed', 'Phone Confirmed', 'Two Factor Enabled', 'Created Date', 'Updated Date'];
    const csvRows = rows.map(row => [
      row.jobTitle,
      row.fullName,
      row.userName,
      row.email ?? '',
      row.phoneNumber ?? '',
      row.isEnabled ? 'Yes' : 'No',
      row.emailConfirmed ? 'Yes' : 'No',
      row.phoneNumberConfirmed ? 'Yes' : 'No',
      row.twoFactorEnabled ? 'Yes' : 'No',
      this.formatDate(row.createdDate),
      this.formatDate(row.updatedDate)
    ]);

    const csv = [headers, ...csvRows]
      .map(cols => cols.map(value => `"${(value ?? '').toString().replaceAll('"', '""')}"`).join(','))
      .join('\r\n');

    this.downloadFile(csv, 'admin-users-report.csv', 'text/csv;charset=utf-8;');
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    this.printReport();
  }

  getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (error && typeof error === 'object' && 'message' in error) {
      return String((error as { message?: unknown }).message ?? 'Unknown error');
    }

    return 'Unknown error';
  }

  formatDate(value?: string | Date): string {
    if (!value) {
      return '—';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '—';
    }

    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private getExportRows(): AdminUsersReportRow[] {
    return this.filteredRows();
  }

  private downloadFile(content: string, fileName: string, contentType: string): void {
    const blob = new Blob([content], { type: contentType });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    URL.revokeObjectURL(url);
  }
}



