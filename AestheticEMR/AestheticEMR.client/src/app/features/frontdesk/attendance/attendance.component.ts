import { Component, OnInit, TemplateRef, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HRetainership } from '../../../models/legacy/h-retainership.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { fadeInOut } from '../../../services/animations';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../../services/h-retainership-endpoint.service';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrl: './attendance.component.scss',
  animations: [fadeInOut],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, TranslateModule]
})
export class AttendanceComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly alertService = inject(AlertService);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);
  private readonly modalService = inject(NgbModal);

  attendances: Attendance[] = [];
  attendancesCache: Attendance[] = [];
  filteredAttendances: Attendance[] = [];
  patients: HPatient[] = [];
  retainerships: HRetainership[] = [];
  clinicTypes: string[] = [];
  loadingIndicator = false;
  isEditing = false;
  showForm = false;
  currentAttendance: Attendance | null = null;
  modalRef: NgbModalRef | null = null;
  searchText = '';
  patientSearchText = '';
  showAll = false;
  readonly pageSize = 10;
  currentPage = 1;
  readonly attendanceStatusOptions = ['NORMAL', 'FOLLOW UP', 'EMERGENCY'];
  readonly billingCategoryOptions = ['HMO', 'PRIVATE', 'MTHLY', 'NHIS'];

  private readonly selectedPatientNo = signal('');
  private readonly patientSearch = signal('');

  readonly selectedPatient = computed(() => {
    const pNo = this.selectedPatientNo();
    return this.patients.find(patient => patient.pno === pNo) ?? null;
  });

  readonly filteredPatients = computed(() => {
    const term = this.patientSearch().trim().toLowerCase();

    if (!term) {
      return this.patients;
    }

    return this.patients.filter(patient =>
      (patient.pno ?? '').toLowerCase().includes(term)
      || (patient.pSurName ?? '').toLowerCase().includes(term)
      || (patient.pFirstname ?? '').toLowerCase().includes(term)
      || (patient.coyName ?? '').toLowerCase().includes(term)
      || (patient.pPhoneNo ?? '').toLowerCase().includes(term)
    );
  });

  attendanceForm = this.fb.group({
    consultId: [{ value: '', disabled: true }],
    recDate: [this.getTodayInputValue(), Validators.required],
    pNo: ['', [Validators.required, Validators.maxLength(50)]],
    clinicType: ['', [Validators.required, Validators.maxLength(50)]],
    clientCat: ['', Validators.maxLength(50)],
    coyname: ['', Validators.maxLength(50)],
    attndStatus: ['NORMAL', Validators.maxLength(50)]
  });

  ngOnInit(): void {
    this.loadPatients();
    this.loadRetainerships();
    this.loadClinicTypes();
    this.loadData();
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredAttendances.length / this.pageSize));
  }

  get pagedAttendances(): Attendance[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredAttendances.slice(start, start + this.pageSize);
  }

  get clinicTypeOptions(): string[] {
    const currentValue = this.attendanceForm.controls.clinicType.value?.trim();
    return [...new Set([...this.clinicTypes, ...(currentValue ? [currentValue] : [])])];
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
          this.syncSelectedPatient();
        },
        error: () => {
          this.patients = [];
        }
      });
  }

  loadRetainerships(): void {
    this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>()
      .subscribe({
        next: retainerships => {
          this.retainerships = [...retainerships].sort((a, b) => a.retainName.localeCompare(b.retainName));
        },
        error: () => {
          this.retainerships = [];
        }
      });
  }

  loadClinicTypes(): void {
    this.attendanceEndpoint.getAttendanceClinicTypesEndpoint<string[]>()
      .subscribe({
        next: clinicTypes => {
          this.clinicTypes = clinicTypes;
        },
        error: () => {
          this.clinicTypes = [];
        }
      });
  }

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>()
      .subscribe({
        next: attendances => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.attendances = attendances;
          this.attendancesCache = [...attendances];
          this.applyFilters();
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to retrieve attendance records.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  openCreate(content: TemplateRef<unknown>): void {
    this.isEditing = false;
    this.currentAttendance = null;
    this.showForm = true;
    this.patientSearchText = '';
    this.patientSearch.set('');
    this.selectedPatientNo.set('');
    this.attendanceForm.reset({
      consultId: '',
      recDate: this.getTodayInputValue(),
      pNo: '',
      clinicType: '',
      clientCat: '',
      coyname: '',
      attndStatus: 'NORMAL'
    });

    this.modalRef = this.modalService.open(content, { size: 'xl', scrollable: true });
  }

  openEdit(content: TemplateRef<unknown>, attendance: Attendance): void {
    this.isEditing = true;
    this.currentAttendance = attendance;
    this.showForm = true;
    this.patientSearchText = '';
    this.patientSearch.set('');

    this.attendanceForm.reset({
      consultId: attendance.consultId ?? '',
      recDate: this.toDateInputValue(attendance.recDate) || this.getTodayInputValue(),
      pNo: attendance.pNo ?? '',
      clinicType: attendance.clinicType ?? '',
      clientCat: attendance.clientCat ?? '',
      coyname: attendance.coyname ?? '',
      attndStatus: attendance.attndStatus ?? 'NORMAL'
    });

    this.syncSelectedPatient();
    this.modalRef = this.modalService.open(content, { size: 'xl', scrollable: true });
  }

  cancelForm(): void {
    this.showForm = false;
    this.currentAttendance = null;
    this.patientSearchText = '';
    this.patientSearch.set('');
    this.selectedPatientNo.set('');
    this.attendanceForm.reset({
      consultId: '',
      recDate: this.getTodayInputValue(),
      pNo: '',
      clinicType: '',
      clientCat: '',
      coyname: '',
      attndStatus: 'NORMAL'
    });

    this.modalRef?.close();
    this.modalRef = null;
  }

  onPatientSearchChange(): void {
    this.patientSearch.set(this.patientSearchText);
  }

  onPatientChanged(): void {
    this.syncSelectedPatient();
  }

  onCoynameChanged(): void {
  }

  onSearch(): void {
    this.applyFilters();
  }

  onShowAllChanged(): void {
    this.applyFilters();
  }

  refresh(): void {
    this.loadData();
  }

  saveAttendance(): void {
    if (this.attendanceForm.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Please correct the form errors.', MessageSeverity.error);
      return;
    }

    const raw = this.attendanceForm.getRawValue();
    const payload = this.mapFormToAttendance(raw);

    this.alertService.startLoadingMessage();

    if (this.isEditing && this.currentAttendance?.consultId) {
      this.attendanceEndpoint.getUpdateAttendanceEndpoint<Attendance>(this.currentAttendance.consultId, payload)
        .subscribe({
          next: updated => {
            this.alertService.stopLoadingMessage();
            const index = this.attendances.findIndex(item => item.consultId === updated.consultId);
            if (index > -1) {
              this.attendances[index] = updated;
              this.attendancesCache = [...this.attendances];
              this.applyFilters();
            }
            this.cancelForm();
            this.alertService.showMessage('Success', 'Attendance updated successfully.', MessageSeverity.success);
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage(
              'Update Error',
              `Unable to update attendance.\r\nError: "${this.getErrorMessage(error)}"`,
              MessageSeverity.error,
              error
            );
          }
        });
      return;
    }

    const { consultId, ...createPayload } = payload;
    void consultId;

    this.attendanceEndpoint.getNewAttendanceEndpoint<Attendance>(createPayload as Attendance)
      .subscribe({
        next: created => {
          this.alertService.stopLoadingMessage();
          this.attendances.unshift(created);
          this.attendancesCache = [...this.attendances];
          this.applyFilters();
          this.cancelForm();
          this.alertService.showMessage('Success', 'Attendance created successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Create Error',
            `Unable to create attendance.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  deleteAttendance(attendance: Attendance): void {
    if (!attendance.consultId) {
      return;
    }

    const consultId = attendance.consultId;

    this.alertService.showDialog('Are you sure you want to delete this attendance record?', DialogType.confirm,
      () => {
        this.alertService.startLoadingMessage();
        this.attendanceEndpoint.getDeleteAttendanceEndpoint<void>(consultId)
          .subscribe({
            next: () => {
              this.alertService.stopLoadingMessage();
              this.attendances = this.attendances.filter(item => item.consultId !== consultId);
              this.attendancesCache = [...this.attendances];
              this.applyFilters();
              this.alertService.showMessage('Success', 'Attendance deleted successfully.', MessageSeverity.success);
            },
            error: error => {
              this.alertService.stopLoadingMessage();
              this.alertService.showStickyMessage(
                'Delete Error',
                `Unable to delete attendance.\r\nError: "${this.getErrorMessage(error)}"`,
                MessageSeverity.error,
                error
              );
            }
          });
      });
  }

  getPatientLabel(patient: HPatient): string {
    const name = this.getPatientName(patient);
    return `${name} [${patient.pno}]`;
  }

  getPatientNameByNo(pNo?: string): string {
    const patient = this.patients.find(item => item.pno === pNo);
    if (!patient) {
      return pNo ?? '';
    }

    return this.getPatientName(patient);
  }

  getCompanyNameByNo(pNo?: string): string {
    const patient = this.patients.find(item => item.pno === pNo);
    return patient?.coyName ?? '';
  }

  getRetainershipLabel(retainership: HRetainership): string {
    return `${retainership.retainName} [${retainership.retainId}]`;
  }

  getRetainershipNameById(retainId?: string): string {
    if (!retainId) {
      return '';
    }

    return this.retainerships.find(x => x.retainId === retainId)?.retainName ?? retainId;
  }

  private applyFilters(): void {
    const term = this.searchText.trim().toLowerCase();
    let records = [...this.attendancesCache];

    if (!this.showAll) {
      records = records.filter(record => this.isToday(record.recDate));
    }

    if (term) {
      records = records.filter(record =>
        (record.pNo ?? '').toLowerCase().includes(term)
        || this.getPatientNameByNo(record.pNo).toLowerCase().includes(term)
        || this.getRetainershipNameById(record.coyname).toLowerCase().includes(term)
        || (record.coyname ?? '').toLowerCase().includes(term)
        || (record.clinicType ?? '').toLowerCase().includes(term)
      );
    }

    this.filteredAttendances = records;
    this.currentPage = 1;
  }

  private syncSelectedPatient(): void {
    const pNo = this.attendanceForm.controls.pNo.value ?? '';
    this.selectedPatientNo.set(pNo);

    const patient = this.selectedPatient();
    if (!patient) {
      return;
    }

    this.attendanceForm.patchValue({
      clientCat: patient.clientCatId ?? this.attendanceForm.controls.clientCat.value,
      coyname: patient.coyName ?? this.attendanceForm.controls.coyname.value
    }, { emitEvent: false });
  }

  private mapFormToAttendance(raw: Record<string, unknown>): Attendance {
    return {
      consultId: this.normalizeText(raw['consultId']),
      recDate: (raw['recDate'] as string) || this.getTodayInputValue(),
      pNo: (raw['pNo'] as string) ?? '',
      clientCat: this.normalizeText(raw['clientCat']),
      clinicType: (raw['clinicType'] as string) ?? '',
      coyname: this.normalizeText(raw['coyname']),
      attndStatus: this.normalizeText(raw['attndStatus']) ?? 'NORMAL'
    };
  }

  private getPatientName(patient: HPatient): string {
    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() || (patient.pno ?? '');
  }

  private getTodayInputValue(): string {
    return new Date().toISOString().split('T')[0];
  }

  private toDateInputValue(value?: string | null): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    return date.toISOString().split('T')[0];
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
