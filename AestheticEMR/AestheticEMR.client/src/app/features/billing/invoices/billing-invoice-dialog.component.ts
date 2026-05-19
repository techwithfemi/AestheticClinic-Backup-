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
import { NgSelectModule } from '@ng-select/ng-select';

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
import { HServiceNHIEndpoint } from '../../../services/h-service-nhi-endpoint.service';
import { DrugNHISEndpoint } from '../../../services/drug-nhis-endpoint.service';
import { LabServiceNHIEndpoint } from '../../../services/lab-service-nhi-endpoint.service';
import { hServiceNHI } from '../../../models/legacy/h-service-nhi.model';
import { LabService } from '../../../models/legacy/lab-service.model';
import { DrugNhi } from '../../../models/legacy/drug-nhi.model';
import { LabServiceNhi } from '../../../models/legacy/lab-service-nhi.model';
import { ProductTariff } from '../../../models/legacy/product-tariff.model';
import { ProductTariffEndpoint } from '../../../services/product-tariff-endpoint.service';

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
    NgSelectModule
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
  private hServiceNHIEndpoint = inject(HServiceNHIEndpoint);
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

  itemCategories: string[] = ["Service", "Product", "DRUG", "INVESTIGATIONS"];
  itemTariffs: (hServiceNHI | DrugNhi | LabServiceNhi | ProductTariff)[] = [];

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
      this.loadTariffsForCategory(category);
    });
    this.lineItemForm.get('drgName')?.valueChanges.subscribe((drgName: string) => {
      const category = (this.lineItemForm?.get('itemCategory')?.value || '').toLowerCase();
      let selected;
      if (category === 'service') {
        selected = this.itemTariffs.find(item => (item as hServiceNHI).service === drgName);
        if (selected && typeof (selected as hServiceNHI).price !== 'undefined') {
          this.lineItemForm.get('price')?.setValue((selected as hServiceNHI).price);
        }
      } else if (category === 'drug') {
        selected = this.itemTariffs.find(item => (item as DrugNhi).drgName === drgName);
        if (selected && typeof (selected as DrugNhi).price !== 'undefined') {
          this.lineItemForm.get('price')?.setValue((selected as DrugNhi).price);
        }
      } else if (category === 'investigation') {
        selected = this.itemTariffs.find(item => (item as LabServiceNhi).drgName === drgName);
        if (selected && typeof (selected as LabServiceNhi).price !== 'undefined') {
          this.lineItemForm.get('price')?.setValue((selected as LabServiceNhi).price);
        }
      } else if (category === 'product') {
        selected = this.itemTariffs.find(item => (item as ProductTariff).pdtName === drgName);
        if (selected && typeof (selected as ProductTariff).price !== 'undefined') {
          this.lineItemForm.get('price')?.setValue((selected as ProductTariff).price);
        }
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
      this.lineItemForm.reset({ qty: 1, price: 0 });
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

      this.persistedDetails = [...(invoice.details ?? [])];
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
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
      this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise()
    ]).then(([attendance, patients]) => {
      const patientMap = new Map((patients ?? []).map(p => [p.pno ?? '', p]));
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
          label: `${patientName} [${consultId}]`
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
    });
  }

  private loadProductCategories(): void {
    if (!this.headerInfo.coyID) {
      this.productCategories = [];
      return;
    }
    this.productEndpoint.getProductCategoriesEndpoint<ProductCategory[]>().subscribe({
      next: data => { this.productCategories = data || []; },
      error: err => { this.alertService.showStickyMessage('Product Categories Error', this.getErrorMessage(err), MessageSeverity.error, err); }
    });
  }

  private loadProducts(): void {
    if (!this.headerInfo.coyID) {
      this.products = [];
      return;
    }
    this.productEndpoint.getProductsEndpoint<Product[]>().subscribe({
      next: data => { this.products = data || []; },
      error: err => { this.alertService.showStickyMessage('Products Error', this.getErrorMessage(err), MessageSeverity.error, err); }
    });
  }

  private loadRevenueTypes(): void {
    this.hRevenueTypeEndpoint.getRevenueTypesEndpoint<hRevenueType[]>().subscribe({
      next: data => {
        this.revenueTypes = [{ sno: 0, revType: 'Select Revenue Type', catRemarks: '' }, ...(data || [])];
        console.log('Revenue types loaded:', this.revenueTypes);
      },
      error: err => {
        this.alertService.showStickyMessage('Revenue Types Error', this.getErrorMessage(err), MessageSeverity.error, err);
        console.error('Revenue types load error:', err);
      }
    });
  }

  // Remove tariffCache and always fetch from backend
  private loadTariffsForCategory(category: string): void {
    const coyID = this.headerInfo.coyID;
    const normalized = (category || '').toLowerCase();
    if (!coyID || !category) {
      this.itemTariffs = [];
      return;
    }
    if (normalized === 'service') {
      this.hServiceNHIEndpoint.getServiceTariffsEndpoint<hServiceNHI[]>(coyID).subscribe({
        next: data => {
          this.itemTariffs = data || [{ drgName: 'No items found' } as any];
        },
        error: err => { this.alertService.showStickyMessage('Service Tariffs Error', this.getErrorMessage(err), MessageSeverity.error, err); }
      });
    } else if (normalized === 'drug') {
      this.drugNHISEndpoint.getDrugTariffsEndpoint<DrugNhi[]>(coyID).subscribe({
        next: data => {
          this.itemTariffs = data || [{ drgName: 'No items found' } as any];
        },
        error: err => { this.alertService.showStickyMessage('Drug Tariffs Error', this.getErrorMessage(err), MessageSeverity.error, err); }
      });
    } else if (normalized === 'investigation') {
      this.labServiceNHIEndpoint.getLabServiceTariffsEndpoint<LabServiceNhi[]>(coyID).subscribe({
        next: data => {
          this.itemTariffs = data || [{ drgName: 'No items found' } as any];
        },
        error: err => { this.alertService.showStickyMessage('Lab Service Tariffs Error', this.getErrorMessage(err), MessageSeverity.error, err); }
      });
    } else if (normalized === 'product') {
      this.productTariffEndpoint.getProductTariffsEndpoint<ProductTariff[]>(coyID).subscribe({
        next: data => {
          this.itemTariffs = data || [{ pdtName: 'No items found' } as any];
        },
        error: err => { this.alertService.showStickyMessage('Product Tariffs Error', this.getErrorMessage(err), MessageSeverity.error, err); }
      });
    } else {
      this.itemTariffs = [{ drgName: 'No items found' } as any];
    }
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
    return {
      drgName: (value.drgName ?? '').trim(),
      price: Number(value.price ?? 0),
      qty: Number(value.qty ?? 1),
      billType: (value.billType ?? '').trim() || undefined,
      conID: (value.conID ?? '').trim() || this.headerInfo.consultId || undefined
    } as BillingDetail;
  }

  private buildPayload(details: BillingDetail[]): Billing {
    const totalAmountBilled = details.reduce((sum, item) => sum + this.computePersistedLineTotal(item), 0);
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
    fetch('public/assets/module-settings/billing.json')
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
        this.itemCategories = ["Service", "Product", "DRUG", "INVESTIGATIONS"];
        this.alertService.showStickyMessage('Tariff Category Error', err.message, MessageSeverity.error, err);
        console.error('Tariff category load error:', err);
      });
  }
}
