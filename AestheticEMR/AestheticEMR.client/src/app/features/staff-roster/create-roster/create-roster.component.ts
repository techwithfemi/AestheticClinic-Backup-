import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
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
    MatPaginatorModule,
    MatTooltipModule,
    MatProgressBarModule,
  ],
  templateUrl: './create-roster.component.html',
  styleUrls: ['./create-roster.component.scss'],
  animations: [fadeInOut]
})
export class CreateRosterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly rosterEndpoint = inject(RosterEndpoint);
  private readonly dialog = inject(MatDialog);

  readonly loading = signal(false);
  readonly deletingSno = signal<number | null>(null);
  readonly lookups = signal<RosterLookups>({ groups: [], sourceStaff: [], targetStaff: [], shifts: [] });
  readonly rows = signal<RosterGridItem[]>([]);

  readonly dataSource = new MatTableDataSource<RosterGridItem>([]);
  readonly displayedColumns = ['date', 'staffName', 'groupName', 'shiftName', 'status', 'actions'];

  readonly currentMonth = signal(new Date().getMonth() + 1);
  readonly currentYear = signal(new Date().getFullYear());

  readonly monthOptions = Array.from({ length: 12 }, (_, i) => ({
    value: i + 1,
    label: new Date(2000, i, 1).toLocaleString('en', { month: 'long' })
  }));
  readonly yearOptions = Array.from({ length: 3 }, (_, i) => new Date().getFullYear() + i - 1);

  ngOnInit(): void {
    this.loadLookups();
    this.loadGrid();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  onMonthOrYearChanged(): void {
    this.loadGrid();
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
    const month = this.currentMonth();
    const year = this.currentYear();
    const fromDate = this.formatDate(new Date(year, month - 1, 1));
    const toDate = this.formatDate(new Date(year, month, 0));

    this.rosterEndpoint.getGridEndpoint<RosterGridItem[]>({
      deptId: '',
      fromDate,
      toDate,
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

  deleteRow(row: RosterGridItem): void {
    if (!window.confirm(`Delete roster row ${row.sno}?`)) {
      return;
    }

    this.deletingSno.set(row.sno);
    this.rosterEndpoint.deleteRosterEntryEndpoint<void>(row.sno).subscribe({
      next: () => {
        this.deletingSno.set(null);
        this.alertService.showMessage('Deleted', 'Roster row deleted.', MessageSeverity.success);
        this.loadGrid();
      },
      error: error => {
        this.deletingSno.set(null);
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error);
      }
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
