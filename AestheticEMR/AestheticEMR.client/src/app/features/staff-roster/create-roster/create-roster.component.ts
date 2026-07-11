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
  RosterEndpoint,
  RosterGridItem,
  RosterGroupLookup,
  RosterLookups,
  RosterSaveRequest,
  RosterShiftLookup,
  RosterStaffLookup
} from '../../../services/roster-endpoint.service';

interface RosterDayVm {
  date: string;
  label: string;
  dayName: string;
  selected: boolean;
  shiftId: number | null;
  shiftName: string;
  shiftAbbrv: string;
}

interface RosterFormState {
  deptId: string;
  groupId: number | null;
  sourceEmpId: string | null;
  targetEmpId: string | null;
  selectedDate: string | null;
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
  templateUrl: './create-roster.component.html',
  styleUrls: ['./create-roster.component.scss'],
  animations: [fadeInOut]
})
export class CreateRosterComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  private readonly alertService = inject(AlertService);
  private readonly rosterEndpoint = inject(RosterEndpoint);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly deletingSno = signal<number | null>(null);
  readonly lookups = signal<RosterLookups>({ groups: [], sourceStaff: [], targetStaff: [], shifts: [] });
  readonly rows = signal<RosterGridItem[]>([]);
  readonly days = signal<RosterDayVm[]>([]);
  readonly selectedGridRow = signal<RosterGridItem | null>(null);
  readonly form = signal<RosterFormState>({
    deptId: '',
    groupId: null,
    sourceEmpId: null,
    targetEmpId: null,
    selectedDate: null
  });

  readonly dataSource = new MatTableDataSource<RosterGridItem>([]);
  readonly displayedColumns = ['date', 'staffName', 'groupName', 'shiftName', 'status', 'actions'];

  readonly currentMonth = signal(new Date().getMonth() + 1);
  readonly currentYear = signal(new Date().getFullYear());
  readonly monthOptions = Array.from({ length: 12 }, (_, index) => ({ value: index + 1, label: new Date(2000, index, 1).toLocaleString('en', { month: 'long' }) }));
  readonly yearOptions = Array.from({ length: 3 }, (_, index) => this.currentYear() + index - 1);

  readonly selectedDaysCount = computed(() => this.days().filter(day => day.selected).length);
  readonly selectedShiftCount = computed(() => this.days().filter(day => day.selected && day.shiftId != null).length);

  ngOnInit(): void {
    this.buildDays();
    this.loadLookups();
    this.loadGrid();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator ?? null;
  }

  onMonthOrYearChanged(): void {
    this.buildDays();
    this.loadGrid();
  }

  onGroupChanged(groupId: number | null): void {
    this.form.update(state => ({ ...state, groupId }));
    this.loadGrid();
  }

  onTargetChanged(empId: string | null): void {
    this.form.update(state => ({ ...state, targetEmpId: empId }));
    if (!empId) {
      return;
    }

    const matched = this.lookups().targetStaff.find(item => item.empId === empId)
      ?? this.lookups().sourceStaff.find(item => item.empId === empId);

    if (matched) {
      this.form.update(state => ({ ...state, targetEmpId: matched.empId }));
    }
  }

  onSourceChanged(empId: string | null): void {
    this.form.update(state => ({ ...state, sourceEmpId: empId }));
  }

  async loadLookups(): Promise<void> {
    this.loading.set(true);
    this.alertService.startLoadingMessage('Loading roster lookups...', 'Roster');

    this.rosterEndpoint.getLookupsEndpoint<RosterLookups>('').subscribe({
      next: result => {
        this.lookups.set(result);
        const firstGroup = result.groups[0] ?? null;
        if (firstGroup && !this.form().groupId) {
          this.form.update(state => ({ ...state, groupId: firstGroup.groupId }));
        }

        this.alertService.stopLoadingMessage();
        this.loading.set(false);
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loading.set(false);
        this.alertService.showStickyMessage(
          'Load Error',
          this.getErrorMessage(error),
          MessageSeverity.error
        );
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
      groupId: this.form().groupId,
      fromDate,
      toDate,
      latestOnly: true
    }).subscribe({
      next: rows => {
        this.rows.set(rows);
        this.dataSource.data = rows;
        this.selectedGridRow.set(null);
      },
      error: error => {
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  buildDays(): void {
    const month = this.currentMonth();
    const year = this.currentYear();
    const totalDays = new Date(year, month, 0).getDate();

    this.days.set(Array.from({ length: totalDays }, (_, index) => {
      const date = new Date(year, month - 1, index + 1);
      return {
        date: this.formatDate(date),
        label: date.toLocaleDateString('en-US', { day: '2-digit', month: 'short', year: 'numeric' }),
        dayName: date.toLocaleDateString('en-US', { weekday: 'short' }),
        selected: false,
        shiftId: null,
        shiftName: '',
        shiftAbbrv: ''
      } satisfies RosterDayVm;
    }));
  }

  selectAllDays(): void {
    this.days.update(items => items.map(item => ({ ...item, selected: true })));
  }

  deselectAllDays(): void {
    this.days.update(items => items.map(item => ({ ...item, selected: false })));
  }

  applyShiftToDay(index: number, shiftId: number | null): void {
    const shift = this.lookups().shifts.find(item => item.sno === shiftId) ?? null;
    this.days.update(items => items.map((item, itemIndex) => itemIndex === index
      ? { ...item, shiftId, shiftName: shift?.shiftName ?? '', shiftAbbrv: shift?.evalTo ?? '' }
      : item));
  }

  toggleDay(index: number, selected: boolean): void {
    this.days.update(items => items.map((item, itemIndex) => itemIndex === index ? { ...item, selected } : item));
  }

  clearSelections(): void {
    this.deselectAllDays();
    this.selectedGridRow.set(null);
  }

  loadSourceRoster(): void {
    const sourceEmpId = this.form().sourceEmpId;
    if (!sourceEmpId) {
      this.alertService.showMessage('Validation', 'Specify a source staff first.', MessageSeverity.warn);
      return;
    }

    const fromDate = this.formatDate(new Date(this.currentYear(), this.currentMonth() - 1, 1));
    const toDate = this.formatDate(new Date(this.currentYear(), this.currentMonth(), 0));

    this.loading.set(true);
    this.rosterEndpoint.getExistingEndpoint<RosterGridItem[]>({ empId: sourceEmpId, fromDate, toDate }).subscribe({
      next: rows => {
        this.applyRosterRowsToDays(rows);
        this.loading.set(false);
        this.alertService.showMessage('Loaded', 'Source roster copied into the selection grid.', MessageSeverity.success);
      },
      error: error => {
        this.loading.set(false);
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  editRow(row: RosterGridItem): void {
    this.selectedGridRow.set(row);
    const empId = row.empID ?? null;
    this.form.update(state => ({
      ...state,
      groupId: row.groupID ? Number(row.groupID) : state.groupId,
      targetEmpId: empId,
      selectedDate: row.date
    }));

    if (empId) {
      const sourceDate = new Date(row.date);
      this.currentMonth.set(sourceDate.getMonth() + 1);
      this.currentYear.set(sourceDate.getFullYear());
      this.buildDays();
      this.loadRosterForEmployee(empId);
    }
  }

  loadRosterForEmployee(empId: string): void {
    const fromDate = this.formatDate(new Date(this.currentYear(), this.currentMonth() - 1, 1));
    const toDate = this.formatDate(new Date(this.currentYear(), this.currentMonth(), 0));

    this.loading.set(true);
    this.rosterEndpoint.getExistingEndpoint<RosterGridItem[]>({ empId, fromDate, toDate }).subscribe({
      next: rows => {
        this.applyRosterRowsToDays(rows);
        this.loading.set(false);
      },
      error: error => {
        this.loading.set(false);
        this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  applyRosterRowsToDays(rows: RosterGridItem[]): void {
    const shifts = this.lookups().shifts;

    this.days.update(items => items.map(day => {
      const match = rows.find(row => this.formatDate(new Date(row.date)) === day.date);
      if (!match) {
        return { ...day, selected: false, shiftId: null, shiftName: '', shiftAbbrv: '' };
      }

      const shift = shifts.find(item => item.shiftName?.trim().toLowerCase() === (match.shiftName ?? '').trim().toLowerCase())
        ?? shifts.find(item => item.evalTo?.trim().toLowerCase() === (match.shiftAbbrv ?? '').trim().toLowerCase());

      return {
        ...day,
        selected: true,
        shiftId: shift?.sno ?? null,
        shiftName: match.shiftName ?? shift?.shiftName ?? '',
        shiftAbbrv: match.shiftAbbrv ?? shift?.evalTo ?? ''
      };
    }));
  }

  saveRoster(): void {
    const state = this.form();
    if (!state.groupId) {
      this.alertService.showMessage('Validation', 'Roster group is required.', MessageSeverity.warn);
      return;
    }

    if (!state.targetEmpId) {
      this.alertService.showMessage('Validation', 'Target staff is required.', MessageSeverity.warn);
      return;
    }

    const selectedDays = this.days()
      .filter(day => day.selected)
      .map(day => ({
        date: day.date,
        shiftId: day.shiftId ?? 0,
        shiftAbbrv: day.shiftAbbrv,
        shiftName: day.shiftName
      }))
      .filter(day => day.shiftId > 0);

    if (selectedDays.length === 0) {
      this.alertService.showMessage('Validation', 'Select at least one day and shift.', MessageSeverity.warn);
      return;
    }

    const group = this.lookups().groups.find(item => item.groupId === state.groupId);
    const payload: RosterSaveRequest = {
      deptId: state.deptId,
      groupId: state.groupId,
      sourceEmpId: state.sourceEmpId,
      targetEmpId: state.targetEmpId,
      groupName: group?.groupName ?? '',
      selectedDays
    };

    this.saving.set(true);
    this.rosterEndpoint.saveRosterEndpoint(payload).subscribe({
      next: () => {
        this.saving.set(false);
        this.alertService.showMessage('Saved', 'Roster saved successfully.', MessageSeverity.success);
        this.loadGrid();
        if (state.targetEmpId) {
          this.loadRosterForEmployee(state.targetEmpId);
        }
      },
      error: error => {
        this.saving.set(false);
        this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error);
      }
    });
  }

  deleteRow(row: RosterGridItem): void {
    const sno = row.sno;
    if (!window.confirm(`Delete roster row ${sno}?`)) {
      return;
    }

    this.deletingSno.set(sno);
    this.rosterEndpoint.deleteRosterEntryEndpoint<void>(sno).subscribe({
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
    this.buildDays();
    this.loadGrid();
  }

  resetForm(): void {
    this.form.update(state => ({
      ...state,
      sourceEmpId: null,
      targetEmpId: null,
      selectedDate: null
    }));
    this.clearSelections();
    this.buildDays();
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
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

  get groups(): RosterGroupLookup[] {
    return this.lookups().groups;
  }

  get sourceStaff(): RosterStaffLookup[] {
    return this.lookups().sourceStaff;
  }

  get targetStaff(): RosterStaffLookup[] {
    return this.lookups().targetStaff;
  }

  get shifts(): RosterShiftLookup[] {
    return this.lookups().shifts;
  }
}
