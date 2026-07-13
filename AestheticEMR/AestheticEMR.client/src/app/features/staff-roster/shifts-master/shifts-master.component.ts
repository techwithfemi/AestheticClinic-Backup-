import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
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
import {
  DepartmentLookup,
  ShiftMasterDetail,
  ShiftMasterEndpoint,
  ShiftMasterItem
} from '../../../services/shift-master-endpoint.service';
import { ShiftMasterEntryDialogComponent } from './shift-master-entry-dialog.component';

@Component({
  selector: 'app-shifts-master',
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
  templateUrl: './shifts-master.component.html',
  styleUrls: ['./shifts-master.component.scss'],
  animations: [fadeInOut]
})
export class ShiftsMasterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(ShiftMasterEndpoint);
  private readonly dialog = inject(MatDialog);

  readonly loading = signal(false);
  readonly deletingId = signal<number | null>(null);
  readonly departments = signal<DepartmentLookup[]>([]);
  readonly rows = signal<ShiftMasterItem[]>([]);
  readonly searchText = signal('');

  readonly dataSource = new MatTableDataSource<ShiftMasterItem>([]);
  readonly displayedColumns = ['shiftName', 'actions'];
  readonly filteredCount = computed(() => this.dataSource.filteredData.length);

  ngOnInit(): void {
    this.loadDepartments();
    this.loadRows();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  loadDepartments(): void {
    this.endpoint.getDepartmentsEndpoint<DepartmentLookup[]>().subscribe({
      next: items => this.departments.set(items),
      error: error => this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error)
    });
  }

  loadRows(): void {
    this.loading.set(true);
    this.endpoint.getAllEndpoint<ShiftMasterItem[]>().subscribe({
      next: items => {
        this.rows.set(items);
        this.dataSource.data = items;
        this.dataSource.filterPredicate = (row, filter) => row.shiftName.toLowerCase().includes(filter);
        this.applyFilter(this.searchText());
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
    this.dialog.open(ShiftMasterEntryDialogComponent, {
      width: '820px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        departments: this.departments(),
        shift: null
      }
    }).afterClosed().subscribe(saved => {
      if (saved === true) {
        this.loadRows();
      }
    });
  }

  openEditDialog(row: ShiftMasterItem): void {
    this.endpoint.getByIdEndpoint<ShiftMasterDetail>(row.shiftId).subscribe({
      next: detail => {
        this.dialog.open(ShiftMasterEntryDialogComponent, {
          width: '820px',
          maxWidth: '95vw',
          disableClose: true,
          data: {
            departments: this.departments(),
            shift: detail
          }
        }).afterClosed().subscribe(saved => {
          if (saved === true) {
            this.loadRows();
          }
        });
      },
      error: error => this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error)
    });
  }

  deleteRow(row: ShiftMasterItem): void {
    if (!window.confirm(`Delete shift master ${row.shiftName}?`)) {
      return;
    }

    this.deletingId.set(row.shiftId);
    this.endpoint.deleteEndpoint<void>(row.shiftId).subscribe({
      next: () => {
        this.deletingId.set(null);
        this.alertService.showMessage('Deleted', 'Shift master deleted.', MessageSeverity.success);
        this.loadRows();
      },
      error: error => {
        this.deletingId.set(null);
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  refresh(): void {
    this.loadDepartments();
    this.loadRows();
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
