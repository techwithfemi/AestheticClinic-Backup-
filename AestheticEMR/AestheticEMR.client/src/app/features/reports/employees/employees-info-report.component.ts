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

import { EmployeeReportRow } from '../../../models/employee.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { EmployeeEndpoint } from '../../../services/employee-endpoint.service';
import { fadeInOut } from '../../../services/animations';
import { UtcDisplayPipe } from '../../../pipes/utc-display.pipe';

@Component({
  selector: 'app-employees-info-report',
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
  templateUrl: './employees-info-report.component.html',
  styleUrl: './employees-info-report.component.scss'
})
export class EmployeesInfoReportComponent implements OnInit {
  private readonly employeeEndpoint = inject(EmployeeEndpoint);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;

  readonly rows = signal<EmployeeReportRow[]>([]);
  readonly rowsCache = signal<EmployeeReportRow[]>([]);

  searchText = '';
  selectedDepartment = '';
  readonly appliedSearch = signal('');
  readonly appliedDepartment = signal('');

  readonly pageSize = 10;
  readonly currentPage = signal(0);
  readonly displayedColumns = ['employee', 'department', 'designation', 'phone', 'dob', 'age'];

  readonly departmentOptions = computed(() => this.getUniqueValues(this.rowsCache().map(row => row.dept)));

  readonly filteredRows = computed(() => {
    const term = this.appliedSearch().trim().toLowerCase();
    const department = this.appliedDepartment().trim().toLowerCase();

    return this.rowsCache().filter(row => {
      if (department && (row.dept ?? '').trim().toLowerCase() !== department) {
        return false;
      }

      if (!term) {
        return true;
      }

      return [
        row.empId,
        row.fullname,
        row.dept,
        row.designation,
        row.phone,
        row.age?.toString()
      ].some(value => (value ?? '').toString().toLowerCase().includes(term));
    });
  });

  readonly pagedRows = computed(() => {
    const start = this.currentPage() * this.pageSize;
    return this.filteredRows().slice(start, start + this.pageSize);
  });

  readonly totalEmployees = computed(() => this.filteredRows().length);
  readonly departmentsCount = computed(() => new Set(this.filteredRows().map(row => (row.dept ?? '').trim()).filter(Boolean)).size);
  readonly withPhoneCount = computed(() => this.filteredRows().filter(row => !!row.phone?.trim()).length);
  readonly averageAge = computed(() => {
    const ages = this.filteredRows().map(row => row.age).filter((value): value is number => typeof value === 'number');
    if (!ages.length) {
      return 0;
    }

    return Math.round(ages.reduce((sum, value) => sum + value, 0) / ages.length);
  });

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading employees report...');

    this.employeeEndpoint.getEmployeeReportRowsEndpoint<EmployeeReportRow[]>()
      .subscribe({
        next: rows => {
          const orderedRows = [...(rows ?? [])].sort((a, b) => (a.fullname ?? a.empId).localeCompare(b.fullname ?? b.empId));
          this.rowsCache.set(orderedRows);
          this.rows.set(orderedRows);
          this.currentPage.set(0);
          this.runReport();
        },
        error: error => {
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to load employees report.\r\nError: "${this.getErrorMessage(error)}"`,
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
    this.appliedDepartment.set(this.selectedDepartment);
    this.currentPage.set(0);
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedDepartment = '';
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
      'Employee ID': row.empId,
      Fullname: row.fullname ?? '',
      Department: row.dept ?? '',
      Designation: row.designation ?? '',
      Phone: row.phone ?? '',
      'Date of Birth': this.formatDate(row.dob),
      Age: row.age ?? ''
    })));
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Employees');
    XLSX.writeFile(workbook, 'employees-info-report.xlsx');
  }

  exportCsv(event: Event): void {
    event.preventDefault();
    const rows = this.getExportRows();
    if (!rows.length) {
      this.alertService.showMessage('Export', 'No records available to export.', MessageSeverity.warn);
      return;
    }

    const headers = ['Employee ID', 'Fullname', 'Department', 'Designation', 'Phone', 'Date of Birth', 'Age'];
    const csvRows = rows.map(row => [
      row.empId,
      row.fullname ?? '',
      row.dept ?? '',
      row.designation ?? '',
      row.phone ?? '',
      this.formatDate(row.dob),
      (row.age ?? '').toString()
    ]);

    const csv = [headers, ...csvRows]
      .map(cols => cols.map(value => `"${(value ?? '').toString().replaceAll('"', '""')}"`).join(','))
      .join('\r\n');

    this.downloadFile(csv, 'employees-info-report.csv', 'text/csv;charset=utf-8;');
  }

  exportPdf(event: Event): void {
    event.preventDefault();
    this.printReport();
  }

  getFullName(row: EmployeeReportRow): string {
    return row.fullname?.trim() || row.empId;
  }

  getInitials(row: EmployeeReportRow): string {
    return this.getFullName(row)
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0]?.toUpperCase() ?? '')
      .join('') || 'EM';
  }

  formatDate(value?: string | null): string {
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

  getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (error && typeof error === 'object' && 'message' in error) {
      return String((error as { message?: unknown }).message ?? 'Unknown error');
    }

    return 'Unknown error';
  }

  private getUniqueValues(values: (string | undefined | null)[]): string[] {
    return [...new Set(values.map(value => value?.trim()).filter((value): value is string => !!value))].sort((a, b) => a.localeCompare(b));
  }

  private getExportRows(): EmployeeReportRow[] {
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
