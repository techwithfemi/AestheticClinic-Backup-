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
  RosterGroupAvailableStaffItem,
  RosterGroupDepartmentItem,
  RosterGroupEndpoint,
  RosterGroupGridItem,
  RosterGroupItem
} from '../../../services/roster-group-endpoint.service';
import { StaffGroupEntryDialogComponent } from './staff-group-entry-dialog.component';

@Component({
  selector: 'app-staff-group',
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
  templateUrl: './staff-group.component.html',
  styleUrls: ['./staff-group.component.scss'],
  animations: [fadeInOut]
})
export class StaffGroupComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(RosterGroupEndpoint);
  private readonly dialog = inject(MatDialog);

  readonly loading = signal(false);
  readonly deletingId = signal<number | null>(null);
  readonly rows = signal<RosterGroupGridItem[]>([]);
  readonly departments = signal<RosterGroupDepartmentItem[]>([]);
  readonly availableStaff = signal<RosterGroupAvailableStaffItem[]>([]);
  readonly searchText = signal('');

  readonly dataSource = new MatTableDataSource<RosterGroupGridItem>([]);
  readonly displayedColumns = ['groupName', 'staffName', 'deptName', 'assigned', 'actions'];
  readonly filteredCount = computed(() => this.dataSource.filteredData.length);

  readonly editDeleteBlockedMessage = 'Edit / Delete Not Necessary! Group Auto Generated';

  ngOnInit(): void {
    this.loadDepartments();
    this.loadAvailableStaff();
    this.loadRows();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  loadDepartments(): void {
    this.endpoint.getDepartmentsEndpoint<RosterGroupDepartmentItem[]>().subscribe({
      next: items => this.departments.set(items),
      error: error => this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error)
    });
  }

  loadAvailableStaff(): void {
    this.endpoint.getAvailableStaffEndpoint<RosterGroupAvailableStaffItem[]>().subscribe({
      next: items => this.availableStaff.set(items),
      error: error => this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error)
    });
  }

  loadRows(): void {
    this.loading.set(true);
    this.endpoint.getAllEndpoint<RosterGroupGridItem[]>().subscribe({
      next: items => {
        this.rows.set(items);
        this.dataSource.data = items;
        this.dataSource.filterPredicate = (row, filter) => {
          const text = `${row.groupName} ${row.staffName} ${row.deptName} ${row.assigned}`.toLowerCase();
          return text.includes(filter);
        };
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
    this.dialog.open(StaffGroupEntryDialogComponent, {
      width: '920px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        rosterGroup: null,
        departments: this.departments(),
        availableStaff: this.availableStaff()
      }
    }).afterClosed().subscribe(saved => {
      if (saved === true) {
        this.refresh();
      }
    });
  }

  openEditDialog(row: RosterGroupGridItem): void {
    const rosterGroup: RosterGroupItem = {
      rosterGrpId: row.groupID,
      rosterGrpName: row.groupName,
      deptName: row.deptName
    };

    this.dialog.open(StaffGroupEntryDialogComponent, {
      width: '920px',
      maxWidth: '95vw',
      disableClose: true,
      data: {
        rosterGroup,
        departments: this.departments(),
        availableStaff: this.availableStaff()
      }
    }).afterClosed().subscribe(saved => {
      if (saved === true) {
        this.refresh();
      }
    });
  }

  deleteRow(row: RosterGroupGridItem): void {
    if (!window.confirm(`Delete roster group ${row.groupName}?`)) {
      return;
    }

    this.deletingId.set(row.groupID);
    this.endpoint.deleteEndpoint<void>(row.groupID).subscribe({
      next: () => {
        this.deletingId.set(null);
        this.alertService.showMessage('Deleted', 'Roster group deleted.', MessageSeverity.success);
        this.refresh();
      },
      error: error => {
        this.deletingId.set(null);
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  showEditDeleteNotAllowedMessage(): void {
    this.alertService.showMessage('Notice', this.editDeleteBlockedMessage, MessageSeverity.warn);
  }

  refresh(): void {
    this.loadDepartments();
    this.loadAvailableStaff();
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
