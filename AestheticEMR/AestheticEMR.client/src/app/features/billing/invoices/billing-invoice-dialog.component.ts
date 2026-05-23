import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { firstValueFrom } from 'rxjs';
import { NgSelectComponent, NgSelectModule } from '@ng-select/ng-select';

import { Billing, BillingDetail } from '../../../models/legacy/billing.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { HPatient } from '../../../models/legacy/h-patient.model';
import { BillingEndpoint } from '../../../services/billing-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../../services/h-patient-endpoint.service';
import { ProductEndpoint } from '../../../services/product-endpoint.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { Product, ProductCategory } from '../../../models/shop/product.model';
import { hRevenueType } from '../../../models/legacy/h-revenue-type.model';
import { HRevenueTypeEndpoint } from '../../../services/h-revenue-type-endpoint.service';
import { ServiceTariffEndpoint } from '../../../services/service-tariff-endpoint.service';
import { DrugNHISEndpoint } from '../../../services/drug-nhis-endpoint.service';
import { LabServiceNHIEndpoint } from '../../../services/lab-service-nhi-endpoint.service';
import { ServiceTariff } from '../../../models/legacy/service-tariff.model';
import { DrugNhi } from '../../../models/legacy/drug-nhi.model';
import { LabServiceNhi } from '../../../models/legacy/lab-service-nhi.model';
import { ProductTariff } from '../../../models/legacy/product-tariff.model';
import { ProductTariffEndpoint } from '../../../services/product-tariff-endpoint.service';
import { PatientSummaryComponent } from '../../../components/patient-summary/patient-summary.component';

export interface BillingInvoiceDialogData {
  mode: 'create' | 'edit';
  billNo?: string;
  consultId?: string;
  pNo?: string;
  coyID?: string;
  company?: string;
  clientID?: string;
}

interface AttendanceOption {
  consultId: string;
  pNo: string;
  patientName: string;
  coyID?: string;
  label: string;
  fullName?: string;
  photo?: string;
  dateOfBirth?: string;
  companyName?: string;
  clinic?: string;
}

@Component({
  selector: 'app-billing-invoice-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    NgSelectModule,
    PatientSummaryComponent
  ],
  templateUrl: './billing-invoice-dialog.component.html',
  styleUrl: './billing-invoice-dialog.component.scss'
})
export class BillingInvoiceDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private billingEndpoint = inject(BillingEndpoint);
  private attendanceEndpoint = inject(AttendanceEndpoint);
  private patientEndpoint = inject(HPatientEndpoint);
  private productEndpoint = inject(ProductEndpoint);
  private alertService = inject(AlertService);
  private hRevenueTypeEndpoint = inject(HRevenueTypeEndpoint);
  private serviceTariffEndpoint = inject(ServiceTariffEndpoint);
  private drugNHISEndpoint = inject(DrugNHISEndpoint);
  private labServiceNHIEndpoint = inject(LabServiceNHIEndpoint);
  private productTariffEndpoint = inject(ProductTariffEndpoint);

  private dialogRef = inject(MatDialogRef<BillingInvoiceDialogComponent>);
  data = inject<BillingInvoiceDialogData>(MAT_DIALOG_DATA);

  readonly detailsColumns = ['drgName', 'price', 'qty', 'lineTotal', 'actions'];
  loadingIndicator = false;
  hasChanges = false;

  attendanceOptions: AttendanceOption[] = [];
  selectedAttendanceKey = '';
  persistedDetails: BillingDetail[] = [];

  productCategories: ProductCategory[] = [];
  products: Product[] = [];
  revenueTypes: hRevenueType[] = [];

  itemCategories: string[] = ["Service", "Product", "Drug", "Investigation"];
  itemTariffs: ((ServiceTariff | DrugNhi | LabServiceNhi | ProductTariff) & { drgName: string; price?: number })[] = [];

  invoiceForm: FormGroup = this.fb.group({
    bDate: [this.today(), Validators.required],
    discount: [0, [Validators.min(0)]],
    amountPaid: [0, [Validators.min(0)]],
    billType: ['', Validators.maxLength(50)],
    // Remove detailsArray and use a single FormGroup for line items
  });

  // Remove detailsArray, lineItemGroup, and createDetailGroup

  // Use a single FormGroup for line items
  lineItemForm: FormGroup = this.fb.group({
    itemCategory: [null, Validators.required],
    drgName: ['', [Validators.required, Validators.maxLength(200)]],
    price: [0, [Validators.required, Validators.min(0)]],
    qty: [1, [Validators.required, Validators.min(1)]],
    billType: ['', [Validators.maxLength(50)]],
    conID: ['', [Validators.maxLength(50)]],
    revenueType: [null, Validators.required] // UI only, not persisted
  });

  headerInfo = {
    consultId: '',
    billNo: '',
    pNo: '',
    patientName: '',
    coyID: ''
  };

  get selectedPatientInfo(): AttendanceOption | undefined {
    return this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
  }

  get selectedPatientAge(): number | null {
    const dob = this.selectedPatientInfo?.dateOfBirth;
    if (!dob) {
      return null;
    }

    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) {
      return null;
    }

    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age >= 0 ? age : null;
  }

  getPatientPhotoSource(photo?: string): string {
    if (!photo) {
      return '';
    }

    return photo.startsWith('data:') ? photo : `data:image/jpeg;base64,${photo}`;
  }

  private hasShownSessionError = false;

  get isEditing(): boolean {
    return this.data.mode === 'edit';
  }

  ngOnInit(): void {
    this.loadAttendanceOptions();
    this.loadItemCategories();
    this.loadProductCategories();
    this.loadProducts();
    this.loadRevenueTypes();
    this.lineItemForm.get('itemCategory')?.valueChanges.subscribe((category: string) => {
      this.lineItemForm.get('drgName')?.setValue(null, { emitEvent: false });
      this.lineItemForm.get('price')?.setValue(0, { emitEvent: false });
      this.loadTariffsForCategory(category);
    });
    this.lineItemForm.get('drgName')?.valueChanges.subscribe((drgName: string) => {
      const selected = this.itemTariffs.find(item => item.drgName === drgName);
      if (selected && typeof selected.price !== 'undefined') {
        this.lineItemForm.get('price')?.setValue(selected.price);
      }
    });
    if (this.isEditing && this.data.billNo) {
      void this.loadInvoice(this.data.billNo);
      return;
    }

    this.applyContextDefaults();
  }

  async addToGrid(): Promise<void> {
    if (this.lineItemForm.invalid || !this.headerInfo.billNo || !this.headerInfo.pNo) {
      this.alertService.showStickyMessage('Validation Error', 'Please select Patient [ConsultID] and complete all required fields.', MessageSeverity.error);
      return;
    }
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Adding bill item...');
    try {
      const newDetail = this.mapCurrentLineItem();
      this.persistedDetails = [...this.persistedDetails, newDetail];

      const selectedCategory = this.lineItemForm.get('itemCategory')?.value ?? null;
      this.lineItemForm.reset({
        itemCategory: selectedCategory,
        drgName: null,
        price: 0,
        qty: 1,
        billType: '',
        conID: '',
        revenueType: null
      }, { emitEvent: false });

      this.hasChanges = true;
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showMessage('Success', 'Bill item added to grid.', MessageSeverity.success);
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Add Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  async deleteDetail(index: number): Promise<void> {
    if (!this.headerInfo.billNo) {
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting bill item...');

    try {
      const existing = await this.getInvoiceByBillNo(this.headerInfo.billNo);
      if (!existing) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        return;
      }

      const remaining = (existing.details ?? []).filter((_, i) => i !== index);
      if (remaining.length === 0) {
        await firstValueFrom(this.billingEndpoint.getDeleteInvoiceEndpoint<void>(existing.billNo));
      } else {
        const updatePayload = this.buildPayload(remaining);
        await firstValueFrom(this.billingEndpoint.getUpdateInvoiceEndpoint<Billing>(existing.billNo, updatePayload));
      }

      await this.refreshPersistedDetails();
      this.hasChanges = true;

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showMessage('Success', 'Bill item deleted.', MessageSeverity.success);
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Delete Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  onAttendanceSelectionChanged(): void {
    const selected = this.attendanceOptions.find(x => this.optionKey(x) === this.selectedAttendanceKey);
    if (!selected) {
      return;
    }

    this.headerInfo.consultId = selected.consultId;
    this.headerInfo.billNo = selected.consultId;
    this.headerInfo.pNo = selected.pNo;
    this.headerInfo.patientName = selected.patientName;
    this.headerInfo.coyID = selected.coyID ?? '';
    void this.refreshPersistedDetails();
  }

  save(): void {
    // Save all rows in the grid to the db (billingDetail table) at once
    // Update AmountBilled col in billing table (add to existing value if any)
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Saving invoice...');
    try {
      const payload = this.buildPayload(this.persistedDetails);
      // Call endpoint to save all details and update AmountBilled
      this.billingEndpoint.getUpdateInvoiceEndpoint<Billing>(payload.billNo!, payload).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.dialogRef.close(true);
        },
        error: (error) => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
        }
      });
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Save Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  computeLineTotal(): number {
    const price = Number(this.lineItemForm.get('price')?.value ?? 0);
    const qty = Number(this.lineItemForm.get('qty')?.value ?? 0);
    return price * qty;
  }

  computePersistedLineTotal(detail: BillingDetail): number {
    return Number(detail.price ?? 0) * Number(detail.qty ?? 0);
  }

  computeGrandTotal(): number {
    return this.persistedDetails.reduce((sum, item) => sum + this.computePersistedLineTotal(item), 0);
  }

  optionKey(option: AttendanceOption): string {
    return `${option.consultId}|${option.pNo}`;
  }

  cancel(): void {
    this.dialogRef.close(this.hasChanges);
  }

  private applyContextDefaults(): void {
    this.headerInfo.consultId = this.data.consultId ?? '';
    this.headerInfo.billNo = this.data.billNo ?? this.data.consultId ?? '';
    this.headerInfo.pNo = this.data.pNo ?? '';
    this.headerInfo.coyID = this.data.coyID ?? this.data.clientID ?? '';
    void this.refreshPersistedDetails();
  }

  private async loadInvoice(billNo: string): Promise<void> {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    try {
      const invoice = await firstValueFrom(this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo));

      this.headerInfo.billNo = invoice.billNo;
      this.headerInfo.consultId = invoice.consultId ?? invoice.billNo;
      this.headerInfo.pNo = invoice.pNo;
      this.headerInfo.coyID = invoice.clientID ?? '';

      this.invoiceForm.patchValue({
        bDate: this.normalizeDateInput(invoice.bDate),
        discount: invoice.discount ?? 0,
        amountPaid: invoice.amountPaid ?? 0,
        billType: invoice.billType ?? ''
      });

      this.persistedDetails = [...((invoice.details ?? []) as BillingDetail[])];
      this.resetLineItemForm();

      if (this.headerInfo.pNo) {
        const option = this.attendanceOptions.find(x => x.pNo === this.headerInfo.pNo && x.consultId === this.headerInfo.consultId);
        if (option) {
          this.selectedAttendanceKey = this.optionKey(option);
          this.headerInfo.patientName = option.patientName;
          this.headerInfo.coyID = option.coyID ?? this.headerInfo.coyID;
        }
      }

      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
    } catch (error) {
      this.alertService.stopLoadingMessage();
      this.loadingIndicator = false;
      this.alertService.showStickyMessage('Load Error', this.getErrorMessage(error), MessageSeverity.error, error);
    }
  }

  private loadAttendanceOptions(): void {
    Promise.all([
      firstValueFrom(this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>()),
      firstValueFrom(this.patientEndpoint.getHPatientsEndpoint<HPatient[]>())
    ]).then(([attendance, patients]) => {
      const patientMap = new Map<string, HPatient>((patients ?? []).map(p => [p.pno ?? '', p]));
      const todays = (attendance ?? []).filter(a => this.isToday(a.recDate));

      const unique = new Map<string, AttendanceOption>();
      for (const item of todays) {
        const consultId = item.consultId ?? '';
        const pNo = item.pNo ?? '';
        if (!consultId || !pNo) {
          continue;
        }

        const patient = patientMap.get(pNo);
        const patientName = `${patient?.pSurName ?? 'Unknown'} ${patient?.pFirstname ?? ''}`.trim();
        const company = item.coyname ?? patient?.coyName;
        const option: AttendanceOption = {
          consultId,
          pNo,
          patientName,
          coyID: company,
          label: `${patientName} [${consultId}]`,
          fullName: patientName,
          photo: patient?.patPixBase64,
          dateOfBirth: patient?.dob,
          companyName: company,
          clinic: item.clinicType
        };

        const key = this.optionKey(option);
        if (!unique.has(key)) {
          unique.set(key, option);
        }
      }

      this.attendanceOptions = Array.from(unique.values()).sort((a, b) => a.label.localeCompare(b.label));

      if (this.headerInfo.consultId && this.headerInfo.pNo) {
        const preselected = this.attendanceOptions.find(x => x.consultId === this.headerInfo.consultId && x.pNo === this.headerInfo.pNo);
        if (preselected) {
          this.selectedAttendanceKey = this.optionKey(preselected);
          this.headerInfo.patientName = preselected.patientName;
          this.headerInfo.coyID = preselected.coyID ?? this.headerInfo.coyID;
          return;
        }
      }

      if (!this.isEditing && this.attendanceOptions.length > 0 && !this.headerInfo.consultId) {
        const first = this.attendanceOptions[0];
        this.selectedAttendanceKey = this.optionKey(first);
        this.onAttendanceSelectionChanged();
      }
    }).catch(error => {
      this.handleLoadError('Attendance Load Error', error);
      this.attendanceOptions = [];
    });
  }

  private loadProductCategories(): void {
    if (!this.headerInfo.coyID) {
      this.productCategories = [];
      return;
    }
    this.productEndpoint.getProductCategoriesEndpoint<ProductCategory[]>().subscribe({
      next: data => { this.productCategories = data || []; },
      error: err => { this.handleLoadError('Product Categories Error', err); }
    });
  }

  private loadProducts(): void {
    if (!this.headerInfo.coyID) {
      this.products = [];
      return;
    }
    this.productEndpoint.getProductsEndpoint<Product[]>().subscribe({
      next: data => { this.products = data || []; },
      error: err => { this.handleLoadError('Products Error', err); }
    });
  }

  private loadRevenueTypes(): void {
    this.hRevenueTypeEndpoint.getRevenueTypesEndpoint<unknown[]>().subscribe({
      next: data => {
        const mapped = (data ?? [])
          .map((item, index) => {
            const source = item as Record<string, unknown>;
            const revType = String(source['revType'] ?? source['RevType'] ?? source['revenueType'] ?? '').trim();
            const snoRaw = source['sno'] ?? source['SNO'] ?? source['id'] ?? source['Id'] ?? index + 1;
            const sno = Number(snoRaw);

            return {
              sno: Number.isNaN(sno) ? index + 1 : sno,
              revType,
              catRemarks: String(source['catRemarks'] ?? source['CatRemarks'] ?? '')
            } as hRevenueType;
          })
          .filter(x => !!x.revType);

        this.revenueTypes = mapped;
      },
      error: err => {
        this.handleLoadError('Revenue Types Error', err);
      }
    });
  }

  // Remove tariffCache and always fetch from backend
  private loadTariffsForCategory(category: string): void {
    const coyID = (this.headerInfo.coyID || '').trim();
    const normalized = (category || '').toLowerCase();
    if (!category) {
      this.itemTariffs = [];
      return;
    }
    console.log(`[BillingInvoice] loadTariffsForCategory: category="${category}", coyID="${coyID}"`);

    if (normalized === 'service') {
      // Uses ServiceTariffController (/api/servicetariff) which filters by VwServiceNhi.CoyId
      this.serviceTariffEndpoint.getServiceTariffsEndpoint<ServiceTariff[]>(coyID || undefined).subscribe({
        next: data => {
          console.log(`[BillingInvoice] Service tariffs raw count=${data?.length ?? 0}`, data?.[0]);
          this.itemTariffs = this.normalizeTariffItems(data ?? []);
          console.log(`[BillingInvoice] Service tariffs normalized count=${this.itemTariffs.length}`);
        },
        error: err => { this.handleLoadError('Service Tariffs Error', err); }
      });
    } else if (normalized === 'drug') {
      this.drugNHISEndpoint.getDrugTariffsEndpoint<DrugNhi[]>(coyID || undefined).subscribe({
        next: data => {
          console.log(`[BillingInvoice] Drug tariffs raw count=${data?.length ?? 0}`, data?.[0]);
          this.itemTariffs = this.normalizeTariffItems(data ?? []);
          console.log(`[BillingInvoice] Drug tariffs normalized count=${this.itemTariffs.length}`);
        },
        error: err => { this.handleLoadError('Drug Tariffs Error', err); }
      });
    } else if (normalized === 'investigation') {
      this.labServiceNHIEndpoint.getLabServiceTariffsEndpoint<LabServiceNhi[]>(coyID || undefined).subscribe({
        next: data => {
          console.log(`[BillingInvoice] Lab tariffs raw count=${data?.length ?? 0}`, data?.[0]);
          this.itemTariffs = this.normalizeTariffItems(data ?? []);
          console.log(`[BillingInvoice] Lab tariffs normalized count=${this.itemTariffs.length}`);
        },
        error: err => { this.handleLoadError('Lab Service Tariffs Error', err); }
      });
    } else if (normalized === 'product') {
      this.productTariffEndpoint.getProductTariffsEndpoint<ProductTariff[]>(coyID || '').subscribe({
        next: data => {
          console.log(`[BillingInvoice] Product tariffs raw count=${data?.length ?? 0}`, data?.[0]);
          this.itemTariffs = this.normalizeTariffItems(data ?? []);
          console.log(`[BillingInvoice] Product tariffs normalized count=${this.itemTariffs.length}`);
        },
        error: err => { this.handleLoadError('Product Tariffs Error', err); }
      });
    } else {
      this.itemTariffs = [];
    }
  }

  private normalizeTariffItems(items: (ServiceTariff | DrugNhi | LabServiceNhi | ProductTariff)[]): ((ServiceTariff | DrugNhi | LabServiceNhi | ProductTariff) & { drgName: string; price?: number })[] {
    return items
      .map(item => {
        const source = item as unknown as Record<string, unknown>;
        // Handle both camelCase and PascalCase (ASP.NET Core System.Text.Json sends PascalCase)
        const drgName = String(
          (source['drgName'] ?? source['DrgName']) ||
          (source['service'] ?? source['Service']) ||
          (source['pdtName'] ?? source['PdtName']) ||
          ''
        ).trim();
        const priceRaw = source['price'] ?? source['Price'];
        const price = typeof priceRaw === 'number' ? priceRaw : Number(priceRaw ?? 0);
        return {
          ...(item as object),
          drgName,
          price: Number.isNaN(price) ? 0 : price
        } as (ServiceTariff | DrugNhi | LabServiceNhi | ProductTariff) & { drgName: string; price?: number };
      })
      .filter(x => !!x.drgName)
      .sort((a, b) => a.drgName.localeCompare(b.drgName));
  }

  private async refreshPersistedDetails(): Promise<void> {
    if (!this.headerInfo.billNo) {
      this.persistedDetails = [];
      return;
    }

    const existing = await this.getInvoiceByBillNo(this.headerInfo.billNo);
    this.persistedDetails = [...(existing?.details ?? [])];
  }

  private async getInvoiceByBillNo(billNo: string): Promise<Billing | null> {
    try {
      return await firstValueFrom(this.billingEndpoint.getInvoiceByBillNoEndpoint<Billing>(billNo));
    } catch {
      return null;
    }
  }

  private resetLineItemForm(): void {
    this.lineItemForm.reset({
      productCategoryId: null,
      productId: null,
      revenueTypeId: null,
      drgName: '',
      price: 0,
      qty: 1,
      billType: '',
      conID: this.headerInfo.consultId ?? ''
    });
  }

  private mapCurrentLineItem(): BillingDetail {
    const value = this.lineItemForm.getRawValue();
    const selectedRevenueType = this.revenueTypes.find(x => Number(x.sno) === Number(value.revenueType));
    const revenueTypeName = selectedRevenueType?.revType?.trim() || undefined;
    const fallbackRevenueType = String(value.revenueType ?? '').trim();

    return {
      drgName: (value.drgName ?? '').trim(),
      price: Number(value.price ?? 0),
      qty: Number(value.qty ?? 1),
      billType: (value.billType ?? '').trim() || undefined,
      conID: (value.conID ?? '').trim() || this.headerInfo.consultId || undefined,
      revenueType: revenueTypeName ?? (fallbackRevenueType || undefined),
      revenueTypeName
    } as BillingDetail;
  }

  private buildPayload(details: BillingDetail[]): Billing {
    return {
      billNo: this.headerInfo.billNo,
      consultId: this.headerInfo.consultId,
      pNo: this.headerInfo.pNo,
      clientID: this.headerInfo.coyID,
      bDate: this.normalizeDateOutput(this.invoiceForm.get('bDate')?.value),
      discount: this.invoiceForm.get('discount')?.value ?? 0,
      amountPaid: this.invoiceForm.get('amountPaid')?.value ?? 0,
      billType: this.invoiceForm.get('billType')?.value ?? '',
      details
    };
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof Error) {
      return error.message;
    }

    if (typeof error === 'string') {
      return error;
    }

    return 'An unknown error occurred.';
  }

  private today(): string {
    return new Date().toISOString().split('T')[0];
  }

  private isToday(dateString: string): boolean {
    const today = new Date();
    const date = new Date(dateString);
    return date.getFullYear() === today.getFullYear() && date.getMonth() === today.getMonth() && date.getDate() === today.getDate();
  }

  private normalizeDateInput(dateString: string): string {
    const [datePart] = dateString.split('T');
    return datePart;
  }

  private normalizeDateOutput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString();
  }

  private loadItemCategories(): void {
    fetch('/assets/module-settings/billing.json')
      .then(response => {
        if (!response.ok) throw new Error('Failed to load billing.json: ' + response.statusText);
        return response.json();
      })
      .then(json => {
        if (json.itemCategories && Array.isArray(json.itemCategories)) {
          this.itemCategories = json.itemCategories;
          console.log('Item categories loaded:', this.itemCategories);
        } else {
          throw new Error('itemCategories missing or not an array in billing.json');
        }
      })
      .catch((err) => {
        this.itemCategories = ["Service", "Product", "Drug", "Investigation"];
        this.handleLoadError('Tariff Category Error', err);
      });
  }

  private handleLoadError(title: string, error: unknown): void {
    if (this.isUnauthorized(error)) {
      if (!this.hasShownSessionError) {
        this.hasShownSessionError = true;
        this.alertService.showStickyMessage(
          'Session Expired',
          'Your session has expired. Please sign in again, then reopen Add Invoice.',
          MessageSeverity.warn,
          error
        );
      }
      return;
    }

    this.alertService.showStickyMessage(title, this.getErrorMessage(error), MessageSeverity.error, error);
  }

  private isUnauthorized(error: unknown): boolean {
    if (!error || typeof error !== 'object') {
      return false;
    }

    const source = error as { status?: unknown; error?: { status?: unknown } };
    return Number(source.status) === 401 || Number(source.error?.status) === 401;
  }

  onNgSelectMouseDown(event: MouseEvent, select: NgSelectComponent): void {
    const target = event.target as HTMLElement | null;
    if (!target || this.shouldIgnoreSelectMouseDown(target)) {
      return;
    }

    const host = target.closest('.ng-select');
    const isOpened = !!host?.classList.contains('ng-select-opened');

    if (!isOpened) {
      return;
    }

    event.preventDefault();
    event.stopPropagation();
    select.close();
  }

  private shouldIgnoreSelectMouseDown(target: HTMLElement): boolean {
    return !!target.closest('.ng-clear-wrapper, .ng-option, .ng-dropdown-panel');
  }
}
