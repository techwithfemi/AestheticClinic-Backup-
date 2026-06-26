import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { EmployeeEndpoint } from '../../../services/employee-endpoint.service';
import { Employee, Designation, EmpDepartment } from '../../../models/employee.model';

// ─── Dialog Component ────────────────────────────────────────────────────────

export interface EmployeeDialogData {
  employee: Employee | null;
  designations: Designation[];
  departments: EmpDepartment[];
}

@Component({
  selector: 'app-employee-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgSelectModule,
  ],
  template: `
    <div class="emp-dialog">
      <div class="dialog-header">
        <h2 class="dialog-title mat-dialog-title">
          <mat-icon>{{ isNew ? 'person_add' : 'edit' }}</mat-icon>
          {{ isNew ? 'New Employee' : 'Edit Employee' }}
        </h2>
        <button mat-icon-button (click)="close()" class="close-btn"><mat-icon>close</mat-icon></button>
      </div>

      <div mat-dialog-content class="dialog-content">
        <!-- EmpID (read-only) -->
        <div class="form-row">
          <mat-form-field appearance="outline" class="field-full">
            <mat-label>Employee ID</mat-label>
            <input matInput [(ngModel)]="form.empId" readonly placeholder="Auto-generated" />
          </mat-form-field>
        </div>

        <!-- Name row -->
        <div class="form-row two-col">
          <mat-form-field appearance="outline">
            <mat-label>Last Name *</mat-label>
            <input matInput [(ngModel)]="form.lastName" name="lastName" required maxlength="100" />
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>First Name *</mat-label>
            <input matInput [(ngModel)]="form.firstName" name="firstName" required maxlength="100" />
          </mat-form-field>
        </div>

        <!-- Designation & Department -->
        <div class="form-row two-col">
          <div class="ng-select-wrapper">
            <span class="ng-select-label">Designation</span>
            <ng-select
              [items]="data.designations"
              bindLabel="designationName"
              bindValue="designationId"
              placeholder="Select Designation"
              [(ngModel)]="form.designationId"
              [clearable]="true">
            </ng-select>
          </div>
          <div class="ng-select-wrapper">
            <span class="ng-select-label">Department</span>
            <ng-select
              [items]="data.departments"
              bindLabel="deptName"
              bindValue="deptId"
              placeholder="Select Department"
              [(ngModel)]="form.deptId"
              [clearable]="true">
            </ng-select>
          </div>
        </div>

        <!-- DOB & Sex -->
        <div class="form-row two-col">
          <mat-form-field appearance="outline">
            <mat-label>Date of Birth</mat-label>
            <input matInput [matDatepicker]="dobPicker" [(ngModel)]="dobDate" (ngModelChange)="onDobChange($event)" placeholder="DD/MM/YYYY" />
            <mat-datepicker-toggle matIconSuffix [for]="dobPicker"></mat-datepicker-toggle>
            <mat-datepicker #dobPicker></mat-datepicker>
          </mat-form-field>
          <div class="ng-select-wrapper">
            <span class="ng-select-label">Sex</span>
            <ng-select
              [items]="sexOptions"
              placeholder="Select Sex"
              [(ngModel)]="form.sex"
              [clearable]="true">
            </ng-select>
          </div>
        </div>

        <!-- Active -->
        <div class="form-row">
          <mat-checkbox [(ngModel)]="form.active" color="primary">Active</mat-checkbox>
        </div>
      </div>

      <div mat-dialog-actions class="dialog-actions">
        <button mat-stroked-button (click)="close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()" [disabled]="saving || !form.lastName || !form.firstName">
          <mat-icon>save</mat-icon>
          {{ saving ? 'Saving...' : 'Save' }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .emp-dialog { min-width: 340px; }
    .dialog-header { display: flex; align-items: center; justify-content: space-between; padding: 16px 16px 0; }
    .dialog-title { margin: 0; font-size: 1.1rem; display: flex; align-items: center; gap: 8px; }
    .close-btn { margin-left: auto; }
    .dialog-content { padding: 16px; }
    .form-row { margin-bottom: 12px; }
    .form-row.two-col { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .field-full { width: 100%; }
    mat-form-field { width: 100%; }
    .ng-select-wrapper { display: flex; flex-direction: column; }
    .ng-select-label { font-size: 12px; color: rgba(0,0,0,.6); margin-bottom: 4px; }
    .dialog-actions { display: flex; justify-content: flex-end; gap: 8px; padding: 8px 16px 16px; }
    @media (max-width: 575.98px) {
      .form-row.two-col { grid-template-columns: 1fr; }
      .emp-dialog { min-width: unset; }
    }
  `]
})
export class EmployeeDialogComponent {
  data: EmployeeDialogData = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<EmployeeDialogComponent>);
  private alertService = inject(AlertService);
  private endpoint = inject(EmployeeEndpoint);

  isNew = !this.data.employee?.empId;
  saving = false;
  dobDate: Date | null = null;

  sexOptions = ['Male', 'Female'];

  form: Employee = {
    empId: this.data.employee?.empId ?? '',
    lastName: this.data.employee?.lastName ?? '',
    firstName: this.data.employee?.firstName ?? '',
    designationId: this.data.employee?.designationId,
    deptId: this.data.employee?.deptId,
    active: this.data.employee?.active ?? true,
    sex: this.data.employee?.sex,
    dob: this.data.employee?.dob,
    empStatusCode: this.data.employee?.empStatusCode,
  };

  constructor() {
    if (this.form.dob) {
      this.dobDate = new Date(this.form.dob);
    }
    if (this.isNew) {
      this.endpoint.generateIdEndpoint().subscribe({
        next: id => { this.form.empId = id; },
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        error: () => { }
      });
    }
  }

  onDobChange(date: Date | null) {
    this.form.dob = date ? date.toISOString() : null;
  }

  save() {
    if (!this.form.lastName?.trim() || !this.form.firstName?.trim()) {
      this.alertService.showMessage('Validation', 'Last name and first name are required.', MessageSeverity.warn);
      return;
    }
    this.saving = true;
    const obs = this.isNew
      ? this.endpoint.createEmployeeEndpoint(this.form)
      : this.endpoint.updateEmployeeEndpoint(this.form.empId!, this.form);

    obs.subscribe({
      next: (saved: Employee) => {
        this.saving = false;
        this.dialogRef.close(saved);
      },
      error: (err: unknown) => {
        this.saving = false;
        const msg = (err as { message?: string })?.message ?? 'An error occurred.';
        this.alertService.showStickyMessage('Save Error', msg, MessageSeverity.error);
      }
    });
  }

  close() { this.dialogRef.close(); }
}

// ─── List Page Component ──────────────────────────────────────────────────────

@Component({
  selector: 'app-employee-info',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
    MatTooltipModule,
    TranslateModule,
  ],
  animations: [fadeInOut],
  template: `
    <div @fadeInOut class="emp-page">
      <div class="page-header">
        <div>
          <h2>Employees</h2>
          <p class="subtitle">Manage employee records.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>person_add</mat-icon>
          Add Employee
        </button>
      </div>

      <div class="search-row">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          (ngModelChange)="onSearchChange($event)"
          placeholder="Search by ID, name, department or designation..." />
      </div>

      <mat-card>
        @if (loadingIndicator) {
          <p class="empty">Loading...</p>
        } @else if (dataSource.data.length === 0) {
          <p class="empty">No employee records found.</p>
        } @else {
          <div class="table-container">
            <table mat-table [dataSource]="dataSource" class="emp-table">
              <ng-container matColumnDef="empId">
                <th mat-header-cell *matHeaderCellDef>Emp ID</th>
                <td mat-cell *matCellDef="let row">{{ row.empId }}</td>
              </ng-container>
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.lastName }}, {{ row.firstName }}</td>
              </ng-container>
              <ng-container matColumnDef="designation">
                <th mat-header-cell *matHeaderCellDef>Designation</th>
                <td mat-cell *matCellDef="let row">{{ resolveDesignation(row.designationId) }}</td>
              </ng-container>
              <ng-container matColumnDef="department">
                <th mat-header-cell *matHeaderCellDef>Department</th>
                <td mat-cell *matCellDef="let row">{{ resolveDepartment(row.deptId) }}</td>
              </ng-container>
              <ng-container matColumnDef="sex">
                <th mat-header-cell *matHeaderCellDef>Sex</th>
                <td mat-cell *matCellDef="let row">{{ row.sex }}</td>
              </ng-container>
              <ng-container matColumnDef="active">
                <th mat-header-cell *matHeaderCellDef>Active</th>
                <td mat-cell *matCellDef="let row">
                  <mat-icon [style.color]="row.active ? '#4caf50' : '#f44336'">
                    {{ row.active ? 'check_circle' : 'cancel' }}
                  </mat-icon>
                </td>
              </ng-container>
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let row" class="actions-cell">
                  <button mat-icon-button color="primary" (click)="openEditDialog(row)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button color="warn" (click)="deleteEmployee(row)" matTooltip="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>
          </div>
          <mat-paginator [pageSize]="10" [pageSizeOptions]="[10, 25, 50]" showFirstLastButtons></mat-paginator>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .emp-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; flex-wrap: wrap; gap: 12px; }
    .page-header h2 { margin: 0; }
    .subtitle { margin: 4px 0 0; color: #666; font-size: 0.9rem; }
    .search-row { margin-bottom: 16px; }
    .search-input { width: 100%; max-width: 500px; padding: 8px 12px; border: 1px solid #ccc; border-radius: 4px; font-size: 14px; box-sizing: border-box; }
    .table-container { overflow-x: auto; }
    .emp-table { width: 100%; }
    .actions-cell { white-space: nowrap; }
    .empty { padding: 24px; text-align: center; color: #888; }
    @media (max-width: 992px) { .emp-page { padding: 16px; } }
    @media (max-width: 575.98px) { .emp-page { padding: 12px; } .page-header { flex-direction: column; } }
  `]
})
export class EmployeeInfoComponent implements OnInit {
  private dialog = inject(MatDialog);
  private alertService = inject(AlertService);
  private endpoint = inject(EmployeeEndpoint);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedColumns = ['empId', 'name', 'designation', 'department', 'sex', 'active', 'actions'];
  dataSource = new MatTableDataSource<Employee>([]);
  rowsCache: Employee[] = [];
  designations: Designation[] = [];
  departments: EmpDepartment[] = [];
  searchText = '';
  loadingIndicator = false;

  ngOnInit() {
    this.loadLookups();
    this.loadData();
  }

  loadLookups() {
    this.endpoint.getDesignationsEndpoint().subscribe({ next: d => (this.designations = d) });
    this.endpoint.getDepartmentsEndpoint().subscribe({ next: d => (this.departments = d) });
  }

  loadData() {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;
    this.endpoint.getEmployeesEndpoint().subscribe({
      next: employees => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.rowsCache = employees;
        this.dataSource.data = employees;
        if (this.paginator) this.dataSource.paginator = this.paginator;
      },
      error: err => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error', `Unable to load employees.\r\nError: "${this.getErrorMessage(err)}"`, MessageSeverity.error);
      }
    });
  }

  onSearchChange(text: string) {
    const q = text.toLowerCase();
    this.dataSource.data = this.rowsCache.filter(e =>
      (e.empId ?? '').toLowerCase().includes(q) ||
      (e.lastName ?? '').toLowerCase().includes(q) ||
      (e.firstName ?? '').toLowerCase().includes(q) ||
      (e.deptId ?? '').toLowerCase().includes(q) ||
      this.resolveDepartment(e.deptId).toLowerCase().includes(q) ||
      this.resolveDesignation(e.designationId).toLowerCase().includes(q)
    );
  }

  resolveDesignation(id?: string) {
    return this.designations.find(d => d.designationId === id)?.designationName ?? id ?? '';
  }

  resolveDepartment(id?: string) {
    return this.departments.find(d => d.deptId === id)?.deptName ?? id ?? '';
  }

  openAddDialog() {
    const ref = this.dialog.open(EmployeeDialogComponent, {
      width: '600px',
      maxWidth: '98vw',
      disableClose: true,
      data: { employee: null, designations: this.designations, departments: this.departments } as EmployeeDialogData
    });
    ref.afterClosed().subscribe((result: Employee | undefined) => {
      if (result) {
        this.rowsCache = [result, ...this.rowsCache];
        this.dataSource.data = this.rowsCache;
        this.alertService.showMessage('Success', `Employee ${result.empId} created.`, MessageSeverity.success);
      }
    });
  }

  openEditDialog(employee: Employee) {
    const ref = this.dialog.open(EmployeeDialogComponent, {
      width: '600px',
      maxWidth: '98vw',
      disableClose: true,
      data: { employee: { ...employee }, designations: this.designations, departments: this.departments } as EmployeeDialogData
    });
    ref.afterClosed().subscribe((result: Employee | undefined) => {
      if (result) {
        const idx = this.rowsCache.findIndex(e => e.empId === result.empId);
        if (idx !== -1) this.rowsCache[idx] = result;
        this.dataSource.data = [...this.rowsCache];
        this.alertService.showMessage('Success', `Employee ${result.empId} updated.`, MessageSeverity.success);
      }
    });
  }

  deleteEmployee(employee: Employee) {
    this.alertService.showDialog(
      `Delete employee "${employee.firstName} ${employee.lastName}"?`,
      DialogType.confirm,
      () => {
        this.endpoint.deleteEmployeeEndpoint(employee.empId!).subscribe({
          next: () => {
            this.rowsCache = this.rowsCache.filter(e => e.empId !== employee.empId);
            this.dataSource.data = [...this.rowsCache];
            this.alertService.showMessage('Deleted', `Employee ${employee.empId} removed.`, MessageSeverity.success);
          },
          error: err => {
            this.alertService.showStickyMessage('Delete Error', `Could not delete employee.\r\nError: "${this.getErrorMessage(err)}"`, MessageSeverity.error);
          }
        });
      }
    );
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') return error;
    if (error instanceof Error) return error.message;
    return JSON.stringify(error);
  }
}
