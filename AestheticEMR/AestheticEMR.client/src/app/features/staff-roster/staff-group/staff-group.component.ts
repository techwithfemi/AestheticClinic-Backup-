import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { fadeInOut } from '../../../services/animations';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import {
  RosterGroupAvailableStaffItem,
  RosterGroupEndpoint,
  RosterGroupItem,
  RosterGroupSaveRequest
} from '../../../services/roster-group-endpoint.service';

interface StaffGroupFormState {
  rosterGrpId: number | null;
  rosterGrpName: string;
  exempted: string;
  selectedEmpIds: string[];
}

@Component({
  selector: 'app-staff-group',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatCardModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatProgressBarModule,
    NgSelectModule
  ],
  templateUrl: './staff-group.component.html',
  styleUrls: ['./staff-group.component.scss'],
  animations: [fadeInOut]
})
export class StaffGroupComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(RosterGroupEndpoint);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly deletingId = signal<number | null>(null);
  readonly currentDepartment = signal('');
  readonly rows = signal<RosterGroupItem[]>([]);
  readonly availableStaff = signal<RosterGroupAvailableStaffItem[]>([]);
  readonly searchText = signal('');
  readonly form = signal<StaffGroupFormState>({
    rosterGrpId: null,
    rosterGrpName: '',
    exempted: 'NO',
    selectedEmpIds: []
  });

  readonly dataSource = new MatTableDataSource<RosterGroupItem>([]);
  readonly displayedColumns = ['rosterGrpName', 'deptName', 'exempted', 'employeeCount', 'actions'];
  readonly filteredCount = computed(() => this.dataSource.filteredData.length);

  ngOnInit(): void {
    this.loadDepartment();
    this.loadAvailableStaff();
    this.loadRows();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  loadDepartment(): void {
    this.endpoint.getCurrentDepartmentNameEndpoint<string>().subscribe({
      next: dept => this.currentDepartment.set(dept ?? ''),
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
    this.endpoint.getAllEndpoint<RosterGroupItem[]>().subscribe({
      next: items => {
        this.rows.set(items);
        this.dataSource.data = items;
        this.dataSource.filterPredicate = (row, filter) => {
          const text = `${row.rosterGrpName} ${row.deptName ?? ''} ${row.exempted ?? ''}`.toLowerCase();
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

  startNew(): void {
    this.form.set({ rosterGrpId: null, rosterGrpName: '', exempted: 'NO', selectedEmpIds: [] });
  }

  editRow(row: RosterGroupItem): void {
    this.form.set({
      rosterGrpId: row.rosterGrpId,
      rosterGrpName: row.rosterGrpName,
      exempted: row.exempted ?? 'NO',
      selectedEmpIds: this.availableStaff()
        .filter(item => item.rosterGrpId === row.rosterGrpId)
        .map(item => item.empId)
    });
  }

  toggleEmployee(empId: string, checked: boolean): void {
    this.form.update(state => {
      const selected = new Set(state.selectedEmpIds);
      if (checked) {
        selected.add(empId);
      } else {
        selected.delete(empId);
      }
      return { ...state, selectedEmpIds: [...selected] };
    });
  }

  onRosterGroupNameChanged(value: string): void {
    this.form.update(state => ({ ...state, rosterGrpName: value }));
  }

  onExemptedChanged(value: string): void {
    this.form.update(state => ({ ...state, exempted: value }));
  }

  save(): void {
    const form = this.form();
    if (!form.rosterGrpName.trim()) {
      this.alertService.showMessage('Validation', 'Roster group name is required.', MessageSeverity.warn);
      return;
    }

    if (form.selectedEmpIds.length === 0) {
      this.alertService.showMessage('Validation', 'Select at least one employee.', MessageSeverity.warn);
      return;
    }

    const payload: RosterGroupSaveRequest = {
      deptId: '',
      rosterGrpName: form.rosterGrpName.trim(),
      exempted: form.exempted,
      empIds: form.selectedEmpIds
    };

    this.saving.set(true);
    const request = form.rosterGrpId
      ? this.endpoint.updateEndpoint<RosterGroupItem>(form.rosterGrpId, payload)
      : this.endpoint.createEndpoint<RosterGroupItem>(payload);

    request.subscribe({
      next: () => {
        this.saving.set(false);
        this.alertService.showMessage('Saved', 'Roster group saved successfully.', MessageSeverity.success);
        this.startNew();
        this.loadRows();
        this.loadAvailableStaff();
      },
      error: error => {
        this.saving.set(false);
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  deleteRow(row: RosterGroupItem): void {
    if (!window.confirm(`Delete roster group ${row.rosterGrpName}?`)) {
      return;
    }

    this.deletingId.set(row.rosterGrpId);
    this.endpoint.deleteEndpoint<void>(row.rosterGrpId).subscribe({
      next: () => {
        this.deletingId.set(null);
        this.alertService.showMessage('Deleted', 'Roster group deleted.', MessageSeverity.success);
        this.loadRows();
        this.loadAvailableStaff();
      },
      error: error => {
        this.deletingId.set(null);
        this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  refresh(): void {
    this.loadDepartment();
    this.loadAvailableStaff();
    this.loadRows();
  }

  isSelected(empId: string): boolean {
    return this.form().selectedEmpIds.includes(empId);
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
