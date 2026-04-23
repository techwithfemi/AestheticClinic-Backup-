// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Component, inject, OnInit } from '@angular/core';
import { TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { TranslateModule } from '@ngx-translate/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { fadeInOut } from '../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { HRetainershipEndpoint } from '../../services/h-retainership-endpoint.service';
import { HRetainership } from '../../models/legacy/h-retainership.model';

@Component({
    selector: 'app-retainerships',
    templateUrl: './retainerships.component.html',
    styleUrl: './retainerships.component.scss',
    animations: [fadeInOut],
    imports: [ReactiveFormsModule, FormsModule, CommonModule, TranslateModule]
})
export class RetainershipsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private alertService = inject(AlertService);
  private retainershipEndpoint = inject(HRetainershipEndpoint);
  private modalService = inject(NgbModal);

  retainerships: HRetainership[] = [];
  retainershipsCache: HRetainership[] = [];
  filteredRetainerships: HRetainership[] = [];
  readonly pageSize = 10;
  currentPage = 1;
  searchText = '';
  loadingIndicator = false;
  retainershipForm!: FormGroup;
  isEditing = false;
  currentRetainership: HRetainership | null = null;
  modalRef: NgbModalRef | null = null;

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredRetainerships.length / this.pageSize));
  }

  get pagedRetainerships(): HRetainership[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredRetainerships.slice(start, start + this.pageSize);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.currentPage = page;
  }

  ngOnInit() {
    this.initializeForm();
    this.loadData();
  }

  initializeForm() {
    this.retainershipForm = this.fb.group({
      retainId: [''], // Not required for creation - auto-generated
      retainCode: [''], // Not required for creation - auto-generated
      retainName: ['', [Validators.required, Validators.maxLength(100)]],
      clientCatId: ['', Validators.maxLength(50)],
      clientType: ['', Validators.maxLength(50)],
      address: ['', Validators.maxLength(200)],
      phoneNo: ['', [Validators.maxLength(30)]],
      email: ['', [Validators.email, Validators.maxLength(100)]],
      contact: ['', Validators.maxLength(100)],
      profFee: [0, [Validators.min(0)]],
      debt: [0, [Validators.min(0)]],
      acctId: ['', Validators.maxLength(50)],
      debtType: ['', Validators.maxLength(50)],
      active: ['Y'],
      useTariff: ['N'],
      pcent: [0, [Validators.min(0), Validators.max(100)]],
      billEndDate: [31, [Validators.min(1), Validators.max(31)]],
      regAmount: [0, [Validators.min(0)]],
      conAmount: [0, [Validators.min(0)]],
      cardRenewAmount: [0, [Validators.min(0)]],
      retainDate: [new Date().toISOString().split('T')[0]],
      clientName: ['', Validators.maxLength(100)],
      appName: ['', Validators.maxLength(100)]
    });
  }

  loadData(): void {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;

    this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>()
      .subscribe({
        next: retainerships => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.retainerships = retainerships;
          this.retainershipsCache = [...retainerships];
          this.onSearch();
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage(
            'Load Error',
            `Unable to retrieve retainerships.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
  }

  openCreateModal(content: TemplateRef<unknown>): void {
    this.isEditing = false;
    this.currentRetainership = null;
    this.retainershipForm.reset({
      active: 'Y',
      useTariff: 'N',
      pcent: 0,
      billEndDate: 31,
      regAmount: 0,
      conAmount: 0,
      cardRenewAmount: 0,
      retainDate: new Date().toISOString().split('T')[0]
    });
    this.modalRef = this.modalService.open(content, { size: 'lg' });
  }

  openEditModal(content: TemplateRef<unknown>, retainership: HRetainership): void {
    this.isEditing = true;
    this.currentRetainership = retainership;
    this.retainershipForm.patchValue(retainership);
    this.modalRef = this.modalService.open(content, { size: 'lg' });
  }

  saveRetainership(): void {
    if (this.retainershipForm.invalid) {
      this.alertService.showStickyMessage('Validation Error', 'Please correct the form errors.', MessageSeverity.error);
      return;
    }

    const formValue = this.retainershipForm.value;
    this.alertService.startLoadingMessage();

    if (this.isEditing && this.currentRetainership) {
      this.retainershipEndpoint.getUpdateHRetainershipEndpoint<HRetainership>(
        this.currentRetainership.retainId, formValue
      ).subscribe({
        next: updated => {
          this.alertService.stopLoadingMessage();
          const index = this.retainerships.findIndex(r => r.retainId === updated.retainId);
          if (index !== -1) {
            this.retainerships[index] = updated;
            this.retainershipsCache = [...this.retainerships];
            this.onSearch();
          }
          this.modalRef?.close();
          this.alertService.showMessage('Success', 'Retainership updated successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage(
            'Update Error',
            `Unable to update retainership.\r\nError: "${this.getErrorMessage(error)}"`,
            MessageSeverity.error,
            error
          );
        }
      });
    } else {
      this.retainershipEndpoint.getNewHRetainershipEndpoint<HRetainership>(formValue)
        .subscribe({
          next: created => {
            this.alertService.stopLoadingMessage();
            this.retainerships.push(created);
            this.retainershipsCache = [...this.retainerships];
            this.onSearch();
            this.modalRef?.close();
            this.alertService.showMessage('Success', 'Retainership created successfully.', MessageSeverity.success);
          },
          error: error => {
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage(
              'Create Error',
              `Unable to create retainership.\r\nError: "${this.getErrorMessage(error)}"`,
              MessageSeverity.error,
              error
            );
          }
        });
    }
  }

  deleteRetainership(retainership: HRetainership): void {
    this.alertService.showDialog('Are you sure you want to delete this retainership?', DialogType.confirm,
      () => {
        this.alertService.startLoadingMessage();
        this.retainershipEndpoint.getDeleteHRetainershipEndpoint<void>(retainership.retainId)
          .subscribe({
            next: () => {
              this.alertService.stopLoadingMessage();
              this.retainerships = this.retainerships.filter(r => r.retainId !== retainership.retainId);
              this.retainershipsCache = [...this.retainerships];
              this.onSearch();
              this.alertService.showMessage('Success', 'Retainership deleted successfully.', MessageSeverity.success);
            },
            error: error => {
              this.alertService.stopLoadingMessage();
              this.alertService.showStickyMessage(
                'Delete Error',
                `Unable to delete retainership.\r\nError: "${this.getErrorMessage(error)}"`,
                MessageSeverity.error,
                error
              );
            }
          });
      });
  }

  onSearch(): void {
    const term = this.searchText.trim().toLowerCase();

    if (!term) {
      this.filteredRetainerships = [...this.retainershipsCache];
      this.currentPage = 1;
      return;
    }

    this.filteredRetainerships = this.retainershipsCache.filter(r =>
      (r.retainName ?? '').toLowerCase().includes(term) ||
      (r.retainCode ?? '').toLowerCase().includes(term) ||
      (r.contact ?? '').toLowerCase().includes(term) ||
      (r.phoneNo ?? '').toLowerCase().includes(term) ||
      (r.email ?? '').toLowerCase().includes(term)
    );

    this.currentPage = 1;
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
