import { Component, OnDestroy, OnInit, TemplateRef, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../../services/h-retainership-endpoint.service';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { HRetainership } from '../../../models/legacy/h-retainership.model';
import { DialogHeaderComponent } from '../../../components/controls/dialog-header/dialog-header.component';

interface FrontdeskSettings {
  clientType?: Record<string, string[]>;
}

type PatientSortColumn = 'pno' | 'name' | 'client' | 'sex' | 'debt' | 'dob' | 'regDate' | 'phone' | 'email';
type SortDirection = 'asc' | 'desc';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.scss',
  animations: [fadeInOut],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    DialogHeaderComponent
  ]
})
export class PatientsComponent implements OnInit, OnDestroy {
  private readonly fb = inject(FormBuilder);
  private readonly alertService = inject(AlertService);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);
  private readonly router = inject(Router);
  private readonly modalService = inject(NgbModal);
  private readonly http = inject(HttpClient);

  patients = signal<HPatient[]>([]);
  companies: HRetainership[] = [];
  frontdeskSettings: FrontdeskSettings | null = null;
  patCatOptions = signal<string[]>([]);
  searchText = signal('');
  debouncedSearchText = signal('');
  loadingIndicator = signal(false);
  isEditing = signal(false);
  currentPatient = signal<HPatient | null>(null);
  sortColumn = signal<PatientSortColumn>('name');
  sortDirection = signal<SortDirection>('asc');
  modalRef: NgbModalRef | null = null;
  formSubmitted = false;
  private readonly searchDebounceMs = 300;
  private searchDebounceTimer: ReturnType<typeof setTimeout> | null = null;
  private loadRequestSequence = 0;

  // Photo state
  photoPreview = signal<string | null>(null);
  photoBase64 = signal<string | null>(null);
  viewingPhotoPatient = signal<HPatient | null>(null);
  private readonly photoLoadInProgress = new Set<string>();

  readonly patientCategoryOptions = ['HMO', 'PRIVATE', 'MTHLY', 'NHIS'];
  readonly maritalStatuses = ['SINGLE', 'MARRIED'];
  readonly sexOptions = ['MALE', 'FEMALE'];
  readonly pageSize = 10;
  currentPage = signal(1);

  readonly filteredPatients = computed(() => {
    const term = this.debouncedSearchText().trim().toLowerCase();
    const patients = this.patients();

    if (term === '***') {
      return [...patients];
    }

    if (term === 'debt') {
      return patients.filter(x => (x.debtBf ?? 0) > 0);
    }

    if (!term) {
      return patients;
    }

    return patients.filter(x =>
      (x.pno ?? '').toLowerCase().includes(term)
      || (x.pSurName ?? '').toLowerCase().includes(term)
      || (x.pFirstname ?? '').toLowerCase().includes(term)
      || (x.clientName ?? '').toLowerCase().includes(term)
      || (x.coyName ?? '').toLowerCase().includes(term)
      || (x.pPhoneNo ?? '').toLowerCase().includes(term)
      || (x.email ?? '').toLowerCase().includes(term)
    );
  });

  readonly sortedPatients = computed(() => {
    const column = this.sortColumn();
    const direction = this.sortDirection();
    const multiplier = direction === 'asc' ? 1 : -1;

    return [...this.filteredPatients()].sort((a, b) => {
      const aValue = this.getSortValue(a, column);
      const bValue = this.getSortValue(b, column);

      if (aValue === bValue) {
        return 0;
      }

      return aValue > bValue ? multiplier : -multiplier;
    });
  });

  readonly totalPages = computed(() => Math.max(1, Math.ceil(this.sortedPatients().length / this.pageSize)));

  readonly pagedPatients = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    return this.sortedPatients().slice(start, start + this.pageSize);
  });

  patientForm = this.fb.group({
    // pno is auto-generated by the server (getIDNo stored proc) — hidden in UI
    pno: [{ value: '', disabled: true }],
    oldPno: ['', Validators.maxLength(100)],
    pSurName: ['', [Validators.required, Validators.maxLength(255)]],
    pFirstname: ['', [Validators.required, Validators.maxLength(150)]],
    title: ['', Validators.maxLength(150)],
    sex: ['', [Validators.required, Validators.maxLength(50)]],
    mstatus: ['', Validators.maxLength(100)],
    dob: ['', Validators.required],
    occupation: ['', Validators.maxLength(100)],
    homeAddress: ['', Validators.maxLength(1100)],
    officeAddress: ['', Validators.maxLength(1000)],
    pPhoneNo: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', Validators.maxLength(500)],
    empNo: ['', Validators.maxLength(100)],
    nextofKin: ['', [Validators.required, Validators.maxLength(500)]],
    kinAddress: ['', Validators.maxLength(1000)],
    relationToKin: ['', Validators.maxLength(100)],
    pCatId: ['', Validators.maxLength(100)],
    coyName: ['', [Validators.required, Validators.maxLength(7)]],
    clientCatId: ['', [Validators.required, Validators.maxLength(100)]],
    policyType: ['', Validators.maxLength(100)],
    nokphone: ['', [Validators.required, Validators.maxLength(100)]],
    regDate: [this.getTodayInputValue()],
    userName: ['', Validators.maxLength(100)]
  });

  ngOnInit(): void {
    this.loadFrontdeskSettings();
    this.loadTodayRegistrations();
    this.loadCompanies();
    // Listen for company selection changes
    this.patientForm.get('coyName')?.valueChanges.subscribe(val => {
      this.updatePatCatOptions(val ?? '');
    });
  }

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
      this.searchDebounceTimer = null;
    }
  }

  loadFrontdeskSettings(): void {
    this.http.get<FrontdeskSettings>('/assets/module-settings/frontdesk.json').subscribe({
      next: settings => {
        this.frontdeskSettings = settings;
        // Set default options on load
        this.updatePatCatOptions((this.patientForm.get('coyName')?.value ?? ''));
      },
      error: () => {
        this.frontdeskSettings = null;
        this.patCatOptions.set(['PRIVATE', 'MTHLY', 'HMO', 'NHIS']);
      }
    });
  }

  updatePatCatOptions(selectedCompanyId: string): void {
    if (!this.frontdeskSettings || !this.frontdeskSettings.clientType) {
      this.patCatOptions.set(['PRIVATE', 'MTHLY', 'HMO', 'NHIS']);
      return;
    }
    let clientType = '';
    if (selectedCompanyId) {
      const company = this.companies.find(c => c.retainId === selectedCompanyId);
      clientType = company?.clientType || '';
    }
    let options: string[] = [];
    if (clientType && this.frontdeskSettings.clientType[clientType]) {
      options = this.frontdeskSettings.clientType[clientType];
    } else if (this.frontdeskSettings.clientType['default']) {
      options = this.frontdeskSettings.clientType['default'];
    } else {
      options = ['PRIVATE', 'MTHLY', 'HMO', 'NHIS'];
    }
    this.patCatOptions.set(options);
    // Optionally reset patient category if not in new list
    const currentCat = this.patientForm.get('clientCatId')?.value ?? '';
    if (!options.includes(currentCat)) {
      this.patientForm.get('clientCatId')?.setValue('');
    }
  }

  get filteredCompanies(): HRetainership[] {
    return this.companies;
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }

    this.currentPage.set(page);
    this.hydratePagedPatientPhotos();
  }

  onSearchChanged(value: string): void {
    const text = value ?? '';
    this.searchText.set(text);

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
      this.searchDebounceTimer = null;
    }

    if (!text.trim()) {
      this.loadTodayRegistrations();
      return;
    }

    this.searchDebounceTimer = setTimeout(() => {
      if (this.debouncedSearchText() === text) {
        return;
      }

      this.debouncedSearchText.set(text);
      this.currentPage.set(1);
      this.loadData(text);
    }, this.searchDebounceMs);
  }

  setSort(column: PatientSortColumn): void {
    if (this.sortColumn() === column) {
      this.sortDirection.set(this.sortDirection() === 'asc' ? 'desc' : 'asc');
    } else {
      this.sortColumn.set(column);
      this.sortDirection.set('asc');
    }

    this.currentPage.set(1);
    this.hydratePagedPatientPhotos();
  }

  getSortIcon(column: PatientSortColumn): string {
    if (this.sortColumn() !== column) {
      return 'unfold_more';
    }

    return this.sortDirection() === 'asc' ? 'arrow_upward' : 'arrow_downward';
  }

  loadCompanies(): void {
    this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[] | { $values?: unknown[]; items?: unknown[]; data?: unknown[] }>()
      .subscribe({
        next: response => {
          this.companies = this.extractCompanies(response)
            .sort((a, b) => (a.retainName ?? '').localeCompare(b.retainName ?? ''));
        },
        error: () => {
          this.companies = [];
        }
      });
  }

  loadData(searchTerm: string = this.debouncedSearchText().trim(), resetPage = true): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator.set(true);

    const term = searchTerm.trim().toLowerCase();
    const request = term
      ? this.patientEndpoint.getHPatientsEndpoint<HPatient[]>()
      : this.patientEndpoint.getHPatientsByRegDateEndpoint<HPatient[]>(`${this.getTodayInputValue()}T00:00:00`);

    const requestSequence = ++this.loadRequestSequence;

    request.subscribe({
      next: patients => {
        if (requestSequence !== this.loadRequestSequence) {
          return;
        }

        this.alertService.stopLoadingMessage();
        this.loadingIndicator.set(false);
        this.patients.set(patients);

        if (resetPage) {
          this.currentPage.set(1);
        } else if (this.currentPage() > this.totalPages()) {
          this.currentPage.set(this.totalPages());
        }

        this.hydratePagedPatientPhotos();
      },
      error: error => {
        if (requestSequence !== this.loadRequestSequence) {
          return;
        }

        this.alertService.stopLoadingMessage();
        this.loadingIndicator.set(false);
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to retrieve patients.\r\nError: "${this.getErrorMessage(error)}"`,   MessageSeverity.error,
          error
        );
      }
    });
  }

  openCreate(content: TemplateRef<unknown>): void {
    this.isEditing.set(false);
    this.currentPatient.set(null);
    this.photoPreview.set(null);
    this.photoBase64.set(null);
    this.formSubmitted = false;
    this.patientForm.reset({
      regDate: this.getTodayInputValue()
    });

    if (!this.companies.length) {
      this.loadCompanies();
    }

    this.modalRef = this.modalService.open(content, { size: 'xl', scrollable: true, backdrop: 'static', keyboard: false });
  }

  openEdit(content: TemplateRef<unknown>, patient: HPatient): void {
    this.isEditing.set(true);
    this.currentPatient.set(patient);
    this.photoBase64.set(null);
    this.photoPreview.set(this.toPhotoDataUrl(patient.patPixBase64));
    this.formSubmitted = false;

    this.patientForm.patchValue({
      ...patient,
      dob: this.toDateInputValue(patient.dob),
      regDate: this.toDateInputValue(patient.regDate) || this.getTodayInputValue()
    });

    this.ensurePatientPhoto(patient, resolved => {
      if (this.currentPatient()?.pno === resolved.pno && !this.photoBase64()) {
        this.photoPreview.set(this.toPhotoDataUrl(resolved.patPixBase64));
      }
    });

    if (!this.companies.length) {
      this.loadCompanies();
    }

    this.modalRef = this.modalService.open(content, { size: 'xl', scrollable: true, backdrop: 'static', keyboard: false });
  }

  cancelForm(): void {
    this.currentPatient.set(null);
    this.photoPreview.set(null);
    this.photoBase64.set(null);
    this.formSubmitted = false;
    this.patientForm.reset({
      regDate: this.getTodayInputValue()
    });
    this.modalRef?.close();
    this.modalRef = null;
  }

  hasControlError(controlName: string, errorName: string): boolean {
    const control = this.patientForm.get(controlName);
    if (!control) {
      return false;
    }

    return control.hasError(errorName) && (control.touched || control.dirty || this.formSubmitted);
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      const result = reader.result as string;
      this.photoPreview.set(result);
      this.photoBase64.set(result); // full data URI e.g. "data:image/jpeg;base64,..."
    };
    reader.readAsDataURL(file);
  }

  removePhoto(): void {
    this.photoPreview.set(null);
    this.photoBase64.set(null);
  }

  savePatient(): void {
    this.formSubmitted = true;

    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      this.alertService.showStickyMessage('Validation Error', 'Please correct the form errors.', MessageSeverity.error);
      return;
    }

    const raw = this.patientForm.getRawValue();

    this.alertService.startLoadingMessage();

    if (this.isEditing() && this.currentPatient()) {
      const current = this.currentPatient();
      const payload: HPatient = {
        ...current,
        ...raw,
        pno: current?.pno,
        dob: this.toDateInputValue(raw.dob),
        regDate: this.toDateInputValue(raw.regDate),
        patPixBase64: this.photoBase64() ?? current?.patPixBase64 ?? undefined
      } as HPatient;
      this.patientEndpoint.getUpdateHPatientEndpoint<HPatient>(current!.pno!, payload)
        .subscribe({
          next: () => {
            this.alertService.stopLoadingMessage();
            this.cancelForm();
            this.refreshCurrentGrid();
            this.alertService.showMessage('Success', 'Patient updated successfully.', MessageSeverity.success);
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage(
              'Update Error',
              `Unable to update patient.\r\nError: "${this.getErrorMessage(error)}"`,    MessageSeverity.error,
              error
            );
          }
        });
      return;
    }

    const { pno, ...createPayload } = raw as HPatient;
    void pno;
    const newPatient: HPatient = {
      ...createPayload,
      patPixBase64: this.photoBase64() ?? undefined
    } as HPatient;
    this.patientEndpoint.getNewHPatientEndpoint<HPatient>(newPatient)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.cancelForm();
          this.refreshCurrentGrid();
          this.alertService.showMessage('Success', 'Patient created successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Create Error',
            `Unable to create patient.\r\nError: "${this.getErrorMessage(error)}"`,    MessageSeverity.error,
            error
          );
        }
      });
  }

  deletePatient(patient: HPatient): void {
    this.alertService.showDialog('Are you sure you want to delete this patient?', DialogType.confirm,
      () => {
        this.alertService.startLoadingMessage();
        this.patientEndpoint.getDeleteHPatientEndpoint<void>(patient.pno!)
          .subscribe({
            next: () => {
              this.alertService.stopLoadingMessage();
              this.refreshCurrentGrid();
              this.alertService.showMessage('Success', 'Patient deleted successfully.', MessageSeverity.success);
            },
            error: error => {
              this.alertService.stopLoadingMessage();
              const message = this.getErrorMessage(error);
              this.alertService.showStickyMessage(
                'Delete Error',
                message.includes('attendance records')
                  ? 'Cannot delete this patient because related attendance records exist.'
                  : `Unable to delete patient.\r\nError: "${message}"`,
                MessageSeverity.error,
                error
              );
            }
          });
      });
  }


  private loadTodayRegistrations(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
      this.searchDebounceTimer = null;
    }

    this.searchText.set('');
    this.debouncedSearchText.set('');
    this.currentPage.set(1);
    this.loadData('', true);
  }

  private refreshCurrentGrid(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
      this.searchDebounceTimer = null;
    }

    this.loadData(this.debouncedSearchText().trim(), false);
  }

  openAttendanceForPatient(patient: HPatient): void {
    if (!patient.pno) {
      return;
    }

    void this.router.navigate(['/frontdesk/attendance'], {
      queryParams: { action: 'create', pNo: patient.pno }
    });
  }

  openAppointmentForPatient(patient: HPatient): void {
    if (!patient.pno) {
      return;
    }

    void this.router.navigate(['/frontdesk/appointments'], {
      queryParams: { action: 'create', pNo: patient.pno }
    });
  }

  viewPhoto(content: TemplateRef<unknown>, patient: HPatient): void {
    this.ensurePatientPhoto(patient, resolved => {
      if (!this.hasPhotoData(resolved.patPixBase64)) {
        this.alertService.showMessage('No Photo', 'No photo available for this patient.', MessageSeverity.warn);
        return;
      }

      this.viewingPhotoPatient.set(resolved);
      this.modalService.open(content, { size: 'sm', centered: true, backdrop: 'static', keyboard: false });
    });
  }

  hasPatientPhoto(patient: HPatient): boolean {
    return this.hasPhotoData(patient.patPixBase64);
  }

  getPhotoSrc(patient: HPatient): string {
    const b64 = (patient.patPixBase64 ?? '').trim();
    if (!b64) {
      return '';
    }

    return b64.startsWith('data:') ? b64 : `data:image/jpeg;base64,${b64}`;
  }

  private hydratePagedPatientPhotos(): void {
    for (const patient of this.pagedPatients()) {
      this.ensurePatientPhoto(patient, () => undefined);
    }
  }

  private hasPhotoData(value?: string | null): boolean {
    return !!value && value.trim().length > 0;
  }

  private toPhotoDataUrl(value?: string | null): string | null {
    if (!this.hasPhotoData(value)) {
      return null;
    }

    return value!.startsWith('data:') ? value! : `data:image/jpeg;base64,${value}`;
  }

  private ensurePatientPhoto(patient: HPatient, callback: (resolved: HPatient) => void): void {
    if (!patient.pno) {
      callback(patient);
      return;
    }

    if (this.hasPhotoData(patient.patPixBase64)) {
      callback(patient);
      return;
    }

    if (this.photoLoadInProgress.has(patient.pno)) {
      callback(patient);
      return;
    }

    this.photoLoadInProgress.add(patient.pno);

    this.patientEndpoint.getHPatientByIdEndpoint<HPatient>(patient.pno)
      .subscribe({
        next: fullPatient => {
          const base64 = fullPatient.patPixBase64?.trim();
          if (base64) {
            this.applyPatientPhoto(patient.pno!, base64);
            const refreshed = this.findPatientByPno(patient.pno!) ?? { ...patient, patPixBase64: base64 };
            callback(refreshed);
          } else {
            callback(patient);
          }

          this.photoLoadInProgress.delete(patient.pno!);
        },
        error: () => {
          callback(patient);
          this.photoLoadInProgress.delete(patient.pno!);
        }
      });
  }

  private applyPatientPhoto(pno: string, base64: string): void {
    const withPhoto = (rows: HPatient[]) => rows.map(x => x.pno === pno ? { ...x, patPixBase64: base64 } : x);

    this.patients.set(withPhoto(this.patients()));

    const current = this.currentPatient();
    if (current?.pno === pno) {
      this.currentPatient.set({ ...current, patPixBase64: base64 });
    }

    const viewing = this.viewingPhotoPatient();
    if (viewing?.pno === pno) {
      this.viewingPhotoPatient.set({ ...viewing, patPixBase64: base64 });
    }
  }

  private findPatientByPno(pno: string): HPatient | undefined {
    return this.patients().find(x => x.pno === pno);
  }

  private extractCompanies(
    response: HRetainership[] | { $values?: unknown[]; items?: unknown[]; data?: unknown[] } | null | undefined
  ): HRetainership[] {
    const items = Array.isArray(response)
      ? response
      : Array.isArray(response?.$values)
        ? response.$values
        : Array.isArray(response?.items)
          ? response.items
          : Array.isArray(response?.data)
            ? response.data
            : [];

    return items.map(item => this.normalizeRetainership(item));
  }

  private normalizeRetainership(item: unknown): HRetainership {
    const source = item as Partial<HRetainership> & Record<string, unknown>;

    const getString = (camelKey: keyof HRetainership, pascalKey: string): string | undefined => {
      const camelValue = source[camelKey as string];
      if (typeof camelValue === 'string') {
        return camelValue;
      }

      const pascalValue = source[pascalKey];
      if (typeof pascalValue === 'string') {
        return pascalValue;
      }

      return undefined;
    };

    return {
      retainId: getString('retainId', 'RetainId') ?? '',
      retainCode: getString('retainCode', 'RetainCode') ?? '',
      retainName: getString('retainName', 'RetainName') ?? '',
      clientCatId: getString('clientCatId', 'ClientCatId'),
      clientType: getString('clientType', 'ClientType'),
      address: getString('address', 'Address'),
      phoneNo: getString('phoneNo', 'PhoneNo'),
      email: getString('email', 'Email'),
      contact: getString('contact', 'Contact'),
      active: getString('active', 'Active')
    };
  }

  private getSortValue(patient: HPatient, column: PatientSortColumn): string | number {
    switch (column) {
      case 'pno':
        return (patient.pno ?? '').toLowerCase();
      case 'name':
        return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim().toLowerCase();
      case 'client':
        return (patient.clientName ?? '').toLowerCase();
      case 'sex':
        return (patient.sex ?? '').toLowerCase();
      case 'debt':
        return patient.debtBf ?? 0;
      case 'dob':
        return patient.dob ? new Date(patient.dob).getTime() : 0;
      case 'regDate':
        return patient.regDate ? new Date(patient.regDate).getTime() : 0;
      case 'phone':
        return (patient.pPhoneNo ?? '').toLowerCase();
      case 'email':
        return (patient.email ?? '').toLowerCase();
      default:
        return '';
    }
  }

  private toDateInputValue(value?: string | null): string {
    if (!value) {
      return '';
    }

    const dateOnly = this.extractDateOnly(value);
    if (dateOnly) {
      return dateOnly;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return '';
    }

    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(date.getUTCDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      const payload = error.error as { message?: string; [key: string]: unknown } | undefined;
      const directMessage = payload?.message;
      if (directMessage) {
        return directMessage;
      }

      if (payload && typeof payload === 'object') {
        for (const value of Object.values(payload)) {
          if (Array.isArray(value) && value.length && typeof value[0] === 'string') {
            return value[0];
          }
        }
      }

      return error.message;
    }

    return (error as Error)?.message || 'Unknown error';
  }

  private getTodayInputValue(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = `${today.getMonth() + 1}`.padStart(2, '0');
    const day = `${today.getDate()}`.padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  calculateAge(dob?: string | null): string {
    if (!dob) {
      return '—';
    }

    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) {
      return '—';
    }

    const today = new Date();
    let years = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      years--;
    }

    if (years < 0) {
      return '—';
    }

    if (years === 0) {
      const totalMonths = (today.getFullYear() - birthDate.getFullYear()) * 12 + monthDiff;
      return totalMonths <= 1 ? '<1mo' : `${totalMonths}mo`;
    }

    return `${years}y`;
  }

  private isToday(value?: string | null): boolean {
    if (!value) {
      return false;
    }

    return this.toDateInputValue(value) === this.getTodayInputValue();
  }

  private extractDateOnly(value: string): string {
    const match = value.trim().match(/^(\d{4}-\d{2}-\d{2})/);
    return match?.[1] ?? '';
  }
}
