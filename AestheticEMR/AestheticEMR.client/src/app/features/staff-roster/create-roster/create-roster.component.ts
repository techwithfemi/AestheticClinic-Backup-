import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  RosterEndpoint,
  RosterGridItem,
  RosterLookups,
} from '../../../services/roster-endpoint.service';
import { CreateRosterDialogComponent, CreateRosterDialogData } from './create-roster-dialog.component';

export const DD_MMM_YYYY_FORMATS = {
  parse:   { dateInput: 'dd-MMM-yyyy' },
  display: {
    dateInput:          'dd-MMM-yyyy',
    monthYearLabel:     'MMM yyyy',
    dateA11yLabel:      'dd-MMM-yyyy',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

class DdMmmYyyyDateAdapter extends NativeDateAdapter {
  override parse(value: string): Date | null {
    if (!value) return null;
    const parts = value.split('-');
    if (parts.length === 3) {
      const day   = parseInt(parts[0], 10);
      const month = new Date(`${parts[1]} 1 2000`).getMonth();
      const year  = parseInt(parts[2], 10);
      if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
        return new Date(year, month, day);
      }
    }
    return super.parse(value);
  }

  override format(date: Date, displayFormat: string): string {
    if (displayFormat === 'dd-MMM-yyyy') {
      const d = date.getDate().toString().padStart(2, '0');
      const m = date.toLocaleString('en', { month: 'short' });
      const y = date.getFullYear();
      return `${d}-${m}-${y}`;
    }
    return super.format(date, displayFormat);
  }
}

@Component({
  selector: 'app-create-roster',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatCardModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatProgressBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  templateUrl: './create-roster.component.html',
  styleUrls: ['./create-roster.component.scss'],
  animations: [fadeInOut],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: DdMmmYyyyDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: DD_MMM_YYYY_FORMATS },
  ]
})
export class CreateRosterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;
  @ViewChild(MatSort) sort?: MatSort;

  private readonly alertService = inject(AlertService);
  private readonly rosterEndpoint = inject(RosterEndpoint);
  private readonly dialog = inject(MatDialog);

  readonly loading = signal(false);
  readonly lookups = signal<RosterLookups>({ groups: [], sourceStaff: [], targetStaff: [], shifts: [] });
  readonly rows = signal<RosterGridItem[]>([]);

  readonly dataSource = new MatTableDataSource<RosterGridItem>([]);
  readonly displayedColumns = ['date', 'staffName', 'clockIn', 'clockOut', 'status', 'fine', 'shiftName', 'deptName', 'exempted', 'actions'];
  readonly hiddenColumns = ['sno', 'groupName', 'startDate', 'endDate', 'groupID', 'rosterGrpShiftID'];

  readonly selectedDate = signal<Date>(new Date());
  selectedDateModel: Date = new Date();
  searchText = '';

  ngOnInit(): void {
    this.dataSource.filterPredicate = (row: RosterGridItem, filter: string) => {
      const term = filter.trim().toLowerCase();
      return (
        (row.date ?? '').toLowerCase().includes(term) ||
        (row.staffName ?? '').toLowerCase().includes(term) ||
        (row.clockIn ?? '').toLowerCase().includes(term) ||
        (row.clockOut ?? '').toLowerCase().includes(term) ||
        (row.status ?? '').toLowerCase().includes(term) ||
        `${row.fine ?? ''}`.toLowerCase().includes(term) ||
        (row.shiftName ?? '').toLowerCase().includes(term) ||
        (row.deptName ?? '').toLowerCase().includes(term) ||
        (row.exempted ?? '').toLowerCase().includes(term)
      );
    };

    this.dataSource.sortingDataAccessor = (row: RosterGridItem, col: string) => {
      switch (col) {
        case 'date':
          return row.date ?? '';
        case 'staffName':
          return (row.staffName ?? '').toLowerCase();
        case 'clockIn':
          return row.clockIn ?? '';
        case 'clockOut':
          return row.clockOut ?? '';
        case 'status':
          return (row.status ?? '').toLowerCase();
        case 'fine':
          return row.fine ?? 0;
        case 'shiftName':
          return (row.shiftName ?? '').toLowerCase();
        case 'deptName':
          return (row.deptName ?? '').toLowerCase();
        case 'exempted':
          return (row.exempted ?? '').toLowerCase();
        default:
          return '';
      }
    };

    this.loadLookups();
    this.loadGrid();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
    this.dataSource.sort      = this.sort ?? null;
  }

  onDateChanged(date: Date | null): void {
    if (date) {
      this.selectedDateModel = date;
      this.selectedDate.set(date);
      this.searchText = '';
      this.dataSource.filter = '';
      this.loadGrid();
    }
  }

  applySearch(value: string): void {
    this.searchText = value;
    this.dataSource.filter = value.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  clearSearch(): void {
    this.applySearch('');
  }

  loadLookups(): void {
    this.loading.set(true);
    this.rosterEndpoint.getLookupsEndpoint<RosterLookups>('').subscribe({
      next: result => {
        this.lookups.set(result);
        this.loading.set(false);
      },
      error: error => {
        this.loading.set(false);
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  loadGrid(): void {
    const date = this.formatDate(this.selectedDate());

    this.rosterEndpoint.getGridEndpoint<RosterGridItem[]>({
      deptId: '',
      fromDate: date,
      toDate: date,
      latestOnly: true
    }).subscribe({
      next: rows => {
        this.rows.set(rows);
        this.dataSource.data = rows;
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  openNewDialog(): void {
    const dialogData: CreateRosterDialogData = {
      lookups: this.lookups(),
      existingRow: null
    };
    const ref = this.dialog.open(CreateRosterDialogComponent, {
      data: dialogData,
      disableClose: true,
      maxWidth: '95vw'
    });
    ref.afterClosed().subscribe(saved => {
      if (saved) { this.loadGrid(); }
    });
  }

  openEditDialog(row: RosterGridItem): void {
    const dialogData: CreateRosterDialogData = {
      lookups: this.lookups(),
      existingRow: row
    };
    const ref = this.dialog.open(CreateRosterDialogComponent, {
      data: dialogData,
      disableClose: true,
      maxWidth: '95vw'
    });
    ref.afterClosed().subscribe(saved => {
      if (saved) { this.loadGrid(); }
    });
  }

  refresh(): void {
    this.loadGrid();
  }

  private formatDate(date: Date): string {
    const y = date.getFullYear();
    const m = `${date.getMonth() + 1}`.padStart(2, '0');
    const d = `${date.getDate()}`.padStart(2, '0');
    return `${y}-${m}-${d}`;
  }

  private formatDateLabel(date: Date): string {
    const d = date.getDate().toString().padStart(2, '0');
    const m = date.toLocaleString('en', { month: 'short' });
    const y = date.getFullYear();
    return `${d}-${m}-${y}`;
  }

  private getErrorMessage(error: unknown): string {
    const e = error as { error?: unknown; message?: string; statusText?: string; status?: number };
    if (e?.error) {
      if (typeof e.error === 'string') return e.error;
      if (typeof e.error === 'object') {
        const body = e.error as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (body.detail) return `${body.title ?? 'Error'}: ${body.detail}`;
        if (body.message) return body.message;
        if (body.errors) return Object.entries(body.errors).map(([k, v]) => `${k}: ${(v ?? []).join(', ')}`).join('\n');
      }
    }
    if (e?.status) return `${e.status} ${e.statusText ?? ''}`.trim();
    return e?.message ?? 'An error occurred.';
  }
}
