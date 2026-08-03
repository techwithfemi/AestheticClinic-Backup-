import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { Appointment } from '../../../models/legacy/appointment.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { Employee } from '../../../models/employee.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { fadeInOut } from '../../../services/animations';
import { AppointmentEndpoint } from '../../../services/appointment-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { EmployeeEndpoint } from '../../../services/employee-endpoint.service';
import { UtcDisplayPipe } from '../../../pipes/utc-display.pipe';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrl: './appointment.component.scss',
  animations: [fadeInOut],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, TranslateModule, UtcDisplayPipe]
})
export class AppointmentComponent implements OnInit, AfterViewInit {
  private readonly fb = inject(FormBuilder);
  private readonly alertService = inject(AlertService);
  private readonly appointmentEndpoint = inject(AppointmentEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly employeeEndpoint = inject(EmployeeEndpoint);
  private readonly modalService = inject(NgbModal);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  @ViewChild('appointmentDialog') private appointmentDialog?: TemplateRef<unknown>;

  appointments: Appointment[] = [];
  appointmentsCache: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  patients: HPatient[] = [];
  employees: Employee[] = [];
  clinicTypes: string[] = [];
  loadingIndicator = false;
  isEditing = false;
  currentAppointment: Appointment | null = null;
  modalRef: NgbModalRef | null = null;
  searchText = '';
  patientSearchText = '';
  readonly pageSize = 10;
  currentPage = 1;
  private pendingCreatePatientNo: string | null = null;

  appointmentForm = this.fb.group({
    id: [0],
    pno: ['', [Validators.required, Validators.maxLength(50)]],
    apptDate: [this.getTodayInputValue(), Validators.required],
    apptTime: [this.getNowTimeValue(), Validators.required],
    clinicType: ['', [Validators.required, Validators.maxLength(100)]],
    remarks: ['', Validators.maxLength(1000)],
    empID: ['', Validators.maxLength(50)]
  });

  ngOnInit(): void {
    this.loadPatients();
    this.loadEmployees();
    this.loadClinicTypes();
    this.loadData();

    this.appointmentForm.controls.pno.valueChanges.subscribe(() => {
      this.onPatientChanged();
    });

    const action = this.route.snapshot.queryParamMap.get('action');
    if (action === 'create') {
      this.pendingCreatePatientNo = this.route.snapshot.queryParamMap.get('pNo');
    }
  }

  ngAfterViewInit(): void {
    if (this.pendingCreatePatientNo === null || !this.appointmentDialog) {
      return;
    }

    setTimeout(() => {
      this.openCreate(this.appointmentDialog as TemplateRef<unknown>);
      this.applyPreselectedPatient(this.pendingCreatePatientNo);

      this.pendingCreatePatientNo = null;
      void this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { action: null, pNo: null },
        queryParamsHandling: 'merge',
        replaceUrl: true
      });
    });
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredAppointments.length / this.pageSize));
  }

  get pagedAppointments(): Appointment[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredAppointments.slice(start, start + this.pageSize);
  }

  get filteredPatients(): HPatient[] {
    const term = this.patientSearchText.trim().toLowerCase();
    const selectedPno = this.appointmentForm.controls.pno.value ?? '';

    const matches = !term
      ? [...this.patients]
      : this.patients.filter(patient =>
        (patient.pno ?? '').toLowerCase().includes(term)
        || (patient.pSurName ?? '').toLowerCase().includes(term)
        || (patient.pFirstname ?? '').toLowerCase().includes(term)
        || (patient.coyName ?? '').toLowerCase().includes(term)
        || (patient.pPhoneNo ?? '').toLowerCase().includes(term)
      );

    if (!selectedPno) {
      return matches;
    }

    const selected = this.patients.find(patient => patient.pno === selectedPno);
    if (!selected || matches.some(patient => patient.pno === selectedPno)) {
      return matches;
    }

    return [selected, ...matches];
  }

  get shouldAutoExpandPatientList(): boolean {
    return this.patientSearchText.trim().length > 0 && this.filteredPatients.length > 0;
  }

  get patientSelectSize(): number | null {
    if (!this.shouldAutoExpandPatientList) {
      return null;
    }

    return Math.min(6, this.filteredPatients.length + 1);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.currentPage = page;
  }

  loadPatients(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>()
      .subscribe({
        next: patients => {
          this.patients = [...patients].sort((a, b) => this.getPatientName(a).localeCompare(this.getPatientName(b)));
          const selectedNo = this.appointmentForm.controls.pno.value ?? '';
          if (selectedNo) {
            this.applyPreselectedPatient(selectedNo);
          }
        },
        error: () => {
          this.patients = [];
        }
      });
  }

  loadClinicTypes(): void {
    this.appointmentEndpoint.getAppointmentClinicTypesEndpoint<string[]>()
      .subscribe({
        next: clinicTypes => {
          this.clinicTypes = clinicTypes;
        },
        error: () => {
          this.clinicTypes = [];
        }
      });
  }

  loadEmployees(): void {
    this.employeeEndpoint.getEmployeesEndpoint<Employee[]>()
      .subscribe({
        next: employees => {
          this.employees = [...employees].sort((a, b) => this.getEmployeeName(a).localeCompare(this.getEmployeeName(b)));
        },
        error: () => {
          this.employees = [];
        }
      });
  }

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.appointmentEndpoint.getAppointmentsEndpoint<Appointment[]>()
      .subscribe({
        next: appointments => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.appointments = appointments;
          this.appointmentsCache = [...appointments];
          this.applyFilters();
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to retrieve appointments.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  refresh(): void {
    this.loadData();
  }

  openCreate(content: TemplateRef<unknown>): void {
    this.isEditing = false;
    this.currentAppointment = null;
    this.patientSearchText = '';
    this.appointmentForm.reset({
      id: 0,
      pno: '',
      apptDate: this.getTodayInputValue(),
      apptTime: this.getNowTimeValue(),
      clinicType: '',
      remarks: '',
      empID: ''
    });

    this.modalRef = this.modalService.open(content, { size: 'md', scrollable: true, backdrop: 'static', keyboard: false });
  }

  openEdit(content: TemplateRef<unknown>, appointment: Appointment): void {
    this.isEditing = true;
    this.currentAppointment = appointment;
    this.patientSearchText = '';

    this.appointmentForm.reset({
      id: appointment.id ?? 0,
      pno: appointment.pno ?? '',
      apptDate: this.toDateInputValue(appointment.apptDate) || this.getTodayInputValue(),
      apptTime: this.toTimeInputValue(appointment.apptTime) || this.getNowTimeValue(),
      clinicType: appointment.clinicType ?? '',
      remarks: appointment.remarks ?? '',
      empID: appointment.empID ?? ''
    });

    this.modalRef = this.modalService.open(content, { size: 'md', scrollable: true, backdrop: 'static', keyboard: false });
  }

  cancelForm(): void {
    this.modalRef?.close();
    this.modalRef = null;
    this.currentAppointment = null;
    this.patientSearchText = '';
  }

  onSearch(): void {
    this.applyFilters();
  }

  onPatientChanged(): void {
    this.patientSearchText = '';
  }

  saveAppointment(): void {
    if (this.appointmentForm.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Please correct the form errors.', MessageSeverity.error);
      return;
    }

    const payload = this.mapFormToAppointment(this.appointmentForm.getRawValue());

    this.alertService.startLoadingMessage();

    if (this.isEditing && payload.id) {
      this.appointmentEndpoint.getUpdateAppointmentEndpoint<Appointment>(payload.id, payload)
        .subscribe({
          next: updated => {
            this.alertService.stopLoadingMessage();
            const index = this.appointments.findIndex(item => item.id === updated.id);
            if (index > -1) {
              this.appointments[index] = updated;
            }
            this.appointmentsCache = [...this.appointments];
            this.applyFilters();
            this.cancelForm();
            this.alertService.showMessage('Success', 'Appointment updated successfully.', MessageSeverity.success);
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage(
              'Update Error',
              `Unable to update appointment.\r\nError: "${this.getErrorMessage(error)}"`,
              MessageSeverity.error,
              error
            );
          }
        });
      return;
    }

    const { id, ...createPayload } = payload;
    void id;

    this.appointmentEndpoint.getNewAppointmentEndpoint<Appointment>(createPayload as Appointment)
      .subscribe({
        next: created => {
          this.alertService.stopLoadingMessage();
          this.appointments.unshift(created);
          this.appointmentsCache = [...this.appointments];
          this.applyFilters();
          this.cancelForm();
          this.alertService.showMessage('Success', 'Appointment created successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Create Error',
            `Unable to create appointment.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  deleteAppointment(appointment: Appointment): void {
    if (!appointment.id) {
      return;
    }

    const appointmentId = appointment.id;

    this.alertService.showDialog('Are you sure you want to delete this appointment?', DialogType.confirm,
      () => {
        this.alertService.startLoadingMessage();
        this.appointmentEndpoint.getDeleteAppointmentEndpoint<void>(appointmentId)
          .subscribe({
            next: () => {
              this.alertService.stopLoadingMessage();
              this.appointments = this.appointments.filter(item => item.id !== appointmentId);
              this.appointmentsCache = [...this.appointments];
              this.applyFilters();
              this.alertService.showMessage('Success', 'Appointment deleted successfully.', MessageSeverity.success);
            },
            error: error => {
              this.alertService.stopLoadingMessage();
              this.alertService.showStickyMessage(
                'Delete Error',
                `Unable to delete appointment.\r\nError: "${this.getErrorMessage(error)}"`,
                MessageSeverity.error,
                error
              );
            }
          });
      });
  }

  trackByAppointmentId(index: number, appointment: Appointment): number | string {
    return appointment.id ?? `row-${index}`;
  }

  getPatientLabel(patient: HPatient): string {
    const name = this.getPatientName(patient);
    return `${name} [${patient.pno}]`;
  }

  getPatientNameByNo(pno?: string): string {
    if (!pno) {
      return '';
    }

    const patient = this.patients.find(item => item.pno === pno);
    return patient ? this.getPatientName(patient) : pno;
  }

  getEmployeeNameByEmpID(empID?: string): string {
    if (!empID) {
      return '';
    }

    const employee = this.employees.find(item => item.empId === empID);
    return employee ? this.getEmployeeName(employee) : empID;
  }

  private applyFilters(): void {
    const term = this.searchText.trim().toLowerCase();
    let records = [...this.appointmentsCache];

    if (!term) {
      records = records.filter(record => this.isToday(record.entryDate));
    }

    if (term) {
      records = records.filter(record =>
        (record.pno ?? '').toLowerCase().includes(term)
        || this.getPatientNameByNo(record.pno).toLowerCase().includes(term)
        || (record.clinicType ?? '').toLowerCase().includes(term)
        || (record.remarks ?? '').toLowerCase().includes(term)
      );
    }

    this.filteredAppointments = records;
    this.currentPage = 1;
  }

  private applyPreselectedPatient(pNo?: string | null): void {
    const patientNo = (pNo ?? '').trim();
    if (!patientNo) {
      return;
    }

    this.appointmentForm.patchValue({ pno: patientNo });

    const patient = this.patients.find(item => item.pno === patientNo);
    if (patient) {
      this.patientSearchText = this.getPatientLabel(patient);
    } else {
      this.patientSearchText = patientNo;
    }

    queueMicrotask(() => this.onPatientChanged());
  }

  private mapFormToAppointment(raw: Record<string, unknown>): Appointment {
    const apptDate = (raw['apptDate'] as string) || this.getTodayInputValue();
    const apptTime = (raw['apptTime'] as string) || this.getNowTimeValue();

    return {
      id: Number(raw['id'] ?? 0) || undefined,
      pno: ((raw['pno'] as string) ?? '').trim(),
      apptDate: `${apptDate}T00:00:00`,
      apptTime: `${apptDate}T${apptTime}:00`,
      clinicType: ((raw['clinicType'] as string) ?? '').trim(),
      remarks: this.normalizeText(raw['remarks']),
      empID: this.normalizeText(raw['empID'])
    };
  }

  private getPatientName(patient: HPatient): string {
    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() || (patient.pno ?? '');
  }

  private getEmployeeName(employee: Employee): string {
    return `${employee.lastName ?? ''} ${employee.firstName ?? ''}`.trim() || (employee.empId ?? '');
  }

  private getTodayInputValue(): string {
    return this.toLocalDateInputValue(new Date());
  }

  private getNowTimeValue(): string {
    const now = new Date();
    return `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}`;
  }

  private toDateInputValue(value?: string | null): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return this.toLocalDateInputValue(date);
  }

  private toLocalDateInputValue(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private toTimeInputValue(value?: string | null): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private normalizeText(value: unknown): string | undefined {
    const text = (value ?? '').toString().trim();
    return text ? text : undefined;
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message;
    }

    if (error instanceof Error) {
      return error.message;
    }

    return 'Unknown error';
  }
}
