import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { ShiftDetail, ShiftDetailsEndpoint, ShiftLookup } from '../../../services/shift-details-endpoint.service';
import { ShiftEntryDialogComponent } from './shift-entry-dialog.component';

@Component({
  selector: 'app-shifts',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatProgressBarModule
  ],
  templateUrl: './shifts.component.html',
  styleUrls: ['./shifts.component.scss'],
  animations: [fadeInOut]
})
export class ShiftsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(ShiftDetailsEndpoint);
  private readonly dialog = inject(MatDialog);
  private readonly cdr = inject(ChangeDetectorRef);

  readonly loading = signal(false);
  readonly deletingId = signal<number | null>(null);
  readonly lookups = signal<ShiftLookup[]>([]);
  readonly rows = signal<ShiftDetail[]>([]);
  readonly searchText = signal('');

  readonly dataSource = new MatTableDataSource<ShiftDetail>([]);
  readonly displayedColumns = ['shiftJob', 'periodOfDay', 'resumptionTime', 'closingTime', 'evalTo', 'actions'];
  readonly filteredCount = computed(() => this.dataSource.filteredData.length);

  ngOnInit(): void {
    this.loadLookups();
    this.loadRows();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  loadLookups(): void {
    this.endpoint.getLookupsEndpoint<ShiftLookup[]>().subscribe({
      next: items => this.lookups.set(items),
      error: error => this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error)
    });
  }

  loadRows(): void {
    this.loading.set(true);
    this.endpoint.getAllEndpoint<ShiftDetail[]>().subscribe({
      next: items => {
        this.rows.set(items);
        this.dataSource.data = items;
        this.dataSource.filterPredicate = (row, filter) => {
          const text = `${row.shiftJob} ${row.periodOfDay} ${row.evalTo ?? ''} ${row.resumptionTime} ${row.closingTime}`.toLowerCase();
          return text.includes(filter);
        };
        this.applyFilter(this.searchText());

        // Reset paginator to first page to ensure updated data is visible
        if (this.paginator) {
          this.paginator.firstPage();
        }

        // Trigger change detection to ensure table renders updated data
        this.cdr.markForCheck();

        this.loading.set(false);
      },
      error: error => {
        this.loading.set(false);
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  applyFilter(value: string): void {
    this.searchText.set(value);
    this.dataSource.filter = value.trim().toLowerCase();
  }

  openCreateDialog(): void {
    this.dialog.open(ShiftEntryDialogComponent, {
      width: '900px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        lookups: this.lookups(),
        shift: null
      }
    }).afterClosed().subscribe(saved => {
      if (saved === true) {
        this.loadRows();
        this.loadLookups();
      }
    });
  }

  openEditDialog(row: ShiftDetail): void {
    this.dialog.open(ShiftEntryDialogComponent, {
      width: '900px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        lookups: this.lookups(),
        shift: row
      }
    }).afterClosed().subscribe(saved => {
      if (saved === true) {
        this.loadRows();
        this.loadLookups();
      }
    });
  }

  deleteRow(row: ShiftDetail): void {
    if (!window.confirm(`Delete shift detail ${row.shiftId}?`)) {
      return;
    }

    this.deletingId.set(row.shiftId);
    this.endpoint.deleteEndpoint<void>(row.shiftId).subscribe({
      next: () => {
        this.deletingId.set(null);
        this.alertService.showMessage('Deleted', 'Shift details deleted.', MessageSeverity.success);
        this.loadRows();
      },
      error: error => {
        this.deletingId.set(null);
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  refresh(): void {
    this.loadLookups();
    this.loadRows();
  }

  formatShiftTime(value?: string | null): string {
    return ShiftEntryDialogComponent.to12HourTime(value);
  }

  private getErrorMessage(error: unknown): string {
    const e = error as { error?: unknown; message?: string; statusText?: string; status?: number };
    if (e?.error) {
      if (typeof e.error === 'string') {
        return e.error;
      }

      if (typeof e.error === 'object') {
        const body = e.error as { detail?: string; title?: string; message?: string; errors?: Record<string, string[]> };
        if (body.detail) {
          return `${body.title ?? 'Error'}: ${body.detail}`;
        }
        if (body.message) {
          return body.message;
        }
        if (body.errors) {
          return Object.entries(body.errors).map(([key, value]) => `${key}: ${(value ?? []).join(', ')}`).join('\n');
        }
      }
    }

    if (e?.status) {
      return `${e.status} ${e.statusText ?? ''}`.trim();
    }

    return e?.message ?? 'An error occurred.';
  }
}
