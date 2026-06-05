import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { ProductCategory, ProductEdit } from '../../../models/shop/product.model';

interface TariffProductDialogData {
  isEdit: boolean;
  item?: ProductEdit;
  categories: ProductCategory[];
}

@Component({
  selector: 'app-tariff-product-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    MatCheckboxModule
  ],
  template: `
    <div class="dialog-container">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Product' : 'Add Product' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog" class="close-btn">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-body">
        <form [formGroup]="form">
          <!-- Form Fields First -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Name</mat-label>
            <input matInput formControlName="name" />
            @if (form.get('name')?.invalid && form.get('name')?.touched) {
              <mat-error>Name is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Category</mat-label>
            <mat-select formControlName="productCategoryId">
              @for (category of data.categories; track category.id) {
                <mat-option [value]="category.id">{{ category.name }}</mat-option>
              }
            </mat-select>
            @if (form.get('productCategoryId')?.invalid && form.get('productCategoryId')?.touched) {
              <mat-error>Category is required</mat-error>
            }
          </mat-form-field>

          <div class="form-row">
            <mat-form-field appearance="outline" class="form-col">
              <mat-label>Buying Price</mat-label>
              <input matInput type="number" min="0" step="0.01" formControlName="buyingPrice" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="form-col">
              <mat-label>Units In Stock</mat-label>
              <input matInput type="number" min="0" step="1" formControlName="unitsInStock" />
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput rows="3" formControlName="description"></textarea>
          </mat-form-field>

          <div class="check-row">
            <mat-checkbox formControlName="isActive">Active</mat-checkbox>
            <mat-checkbox formControlName="isDiscontinued">Discontinued</mat-checkbox>
          </div>

          <!-- Icon/Photo Section Below Form Fields -->
          <div class="icon-upload-section">
            <div class="icon-label">Product Icon/Photo</div>
            <div class="icon-preview-wrapper">
              <div class="icon-preview-container">
                @if (iconPreview) {
                  <img [src]="iconPreview" alt="Product icon preview" class="icon-preview" />
                  <div class="icon-overlay">
                    <button 
                      mat-icon-button 
                      type="button"
                      (click)="zoomPhoto()"
                      class="icon-action-btn zoom-btn"
                      title="Zoom image">
                      <mat-icon>zoom_in</mat-icon>
                    </button>
                    <button 
                      mat-icon-button 
                      type="button" 
                      (click)="clearIcon()"
                      class="icon-action-btn clear-btn"
                      title="Remove image">
                      <mat-icon>close</mat-icon>
                    </button>
                  </div>
                } @else {
                  <div class="icon-placeholder">
                    <mat-icon>image</mat-icon>
                    <span>No image selected</span>
                  </div>
                }
              </div>
            </div>
            <button 
              mat-stroked-button 
              type="button" 
              (click)="fileInput.click()"
              class="upload-btn">
              <mat-icon>upload</mat-icon>
              Choose Photo
            </button>
            <input 
              type="file" 
              #fileInput 
              (change)="onFileSelected($event)" 
              accept="image/*"
              class="hidden-input"
            />
          </div>
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end" class="dialog-actions">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" type="button" [disabled]="!form.valid" (click)="save()">
          {{ data.isEdit ? 'Update' : 'Save' }}
        </button>
      </mat-dialog-actions>
    </div>

    <!-- Zoom Modal -->
    @if (showZoomModal) {
      <div class="zoom-modal" 
           role="dialog" 
           aria-modal="true"
           tabindex="0"
           (click)="closeZoomModal()"
           (keydown.escape)="closeZoomModal()">
        <div class="zoom-modal-content" 
             tabindex="-1"
             (click)="$event.stopPropagation()"
             (keydown)="$event.stopPropagation()">
          <button mat-icon-button class="zoom-close" (click)="closeZoomModal()" aria-label="Close zoom">
            <mat-icon>close</mat-icon>
          </button>
          <img [src]="iconPreview" alt="Zoomed product icon" class="zoom-image" />
        </div>
      </div>
    }
  `,
  styles: [`
    .dialog-container {
      display: flex;
      flex-direction: column;
      max-width: 540px;
      height: auto;
      max-height: 90vh;
    }

    .dialog-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px;
      border-bottom: 1px solid #e0e0e0;
      flex-shrink: 0;
    }

    .dialog-header h2 {
      margin: 0;
      flex: 1;
      font-size: 1.2rem;
      font-weight: 500;
    }

    .close-btn {
      margin-left: auto;
    }

    .dialog-body {
      flex: 1;
      overflow-y: auto;
      padding: 16px;
    }

    .dialog-actions {
      padding: 12px 16px;
      border-top: 1px solid #e0e0e0;
      flex-shrink: 0;
      gap: 8px;
    }

    .full-width {
      width: 100%;
      margin-bottom: 12px;
    }

    .form-row {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 12px;
      margin-bottom: 12px;
    }

    .form-col {
      width: 100%;
    }

    .check-row {
      display: flex;
      gap: 16px;
      margin-top: 8px;
    }

    /* Icon Upload Section */
    .icon-upload-section {
      margin-bottom: 20px;
      padding: 12px;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      background-color: #fafafa;
    }

    .icon-label {
      display: block;
      font-weight: 500;
      margin-bottom: 8px;
      color: #333;
      font-size: 0.95rem;
    }

    .icon-preview-wrapper {
      position: relative;
      margin-bottom: 12px;
    }

    .icon-preview-container {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 100%;
      height: 140px;
      border: 2px dashed #ccc;
      border-radius: 4px;
      margin-bottom: 12px;
      background-color: #fff;
      position: relative;
      overflow: hidden;
    }

    .icon-preview {
      max-width: 100%;
      max-height: 100%;
      object-fit: contain;
    }

    .icon-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      gap: 8px;
      color: #999;
      font-size: 0.875rem;
    }

    .icon-placeholder mat-icon {
      font-size: 40px;
      width: 40px;
      height: 40px;
      color: #ccc;
    }

    /* Icon Action Buttons (Overlay) */
    .icon-overlay {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
      background-color: rgba(0, 0, 0, 0.5);
      opacity: 0;
      transition: opacity 0.2s ease;
    }

    .icon-preview-container:hover .icon-overlay {
      opacity: 1;
    }

    .icon-action-btn {
      background-color: rgba(255, 255, 255, 0.9) !important;
      color: #333 !important;
      border-radius: 50%;
      transition: background-color 0.2s ease;
    }

    .icon-action-btn:hover {
      background-color: rgba(255, 255, 255, 1) !important;
    }

    .zoom-btn::before {
      content: '';
    }

    .clear-btn::before {
      content: '';
    }

    .upload-btn {
      width: 100%;
    }

    .hidden-input {
      display: none;
    }

    /* Zoom Modal */
    .zoom-modal {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background-color: rgba(0, 0, 0, 0.8);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 9999;
    }

    .zoom-modal-content {
      position: relative;
      max-width: 90vw;
      max-height: 90vh;
      background-color: #fff;
      border-radius: 4px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .zoom-close {
      position: absolute;
      top: 8px;
      right: 8px;
      z-index: 10000;
      background-color: rgba(255, 255, 255, 0.9) !important;
    }

    .zoom-image {
      max-width: 100%;
      max-height: 85vh;
      object-fit: contain;
      padding: 16px;
    }

    /* Responsive */
    @media (max-width: 768px) {
      .dialog-container {
        max-width: 100%;
      }

      .form-row {
        grid-template-columns: 1fr;
      }

      .icon-preview-container {
        height: 120px;
      }

      .check-row {
        flex-direction: column;
        gap: 8px;
      }
    }

    @media (max-width: 480px) {
      .dialog-header {
        padding: 12px;
      }

      .dialog-body {
        padding: 12px;
      }

      .dialog-actions {
        padding: 8px 12px;
        flex-direction: column-reverse;
      }

      .dialog-actions button {
        width: 100%;
      }
    }
  `]
})
export class TariffProductDialogComponent {
  private fb = inject(FormBuilder);
  private _dialogRef = inject(MatDialogRef<TariffProductDialogComponent>);
  private _data = inject<TariffProductDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  iconPreview: string | null = null;
  showZoomModal = false;

  form = this.fb.group({
    id: [this.data.item?.id ?? 0],
    name: [this.data.item?.name ?? '', [Validators.required, Validators.maxLength(100)]],
    description: [this.data.item?.description ?? '', [Validators.maxLength(500)]],
    icon: [this.data.item?.icon ?? '', [Validators.maxLength(5000000)]],
    buyingPrice: [this.data.item?.buyingPrice ?? 0, [Validators.required, Validators.min(0)]],
    unitsInStock: [this.data.item?.unitsInStock ?? 0, [Validators.required, Validators.min(0)]],
    isActive: [this.data.item?.isActive ?? true],
    isDiscontinued: [this.data.item?.isDiscontinued ?? false],
    productCategoryId: [this.data.item?.productCategoryId ?? null, [Validators.required]]
  });

  constructor() {
    // If editing and icon exists, show preview
    if (this.data.isEdit && this.data.item?.icon) {
      this.iconPreview = this.data.item.icon;
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    // Validate file size (max 2MB)
    const maxSize = 2 * 1024 * 1024;
    if (file.size > maxSize) {
      alert('File size must be less than 2MB');
      input.value = '';
      return;
    }

    // Validate file type
    if (!file.type.startsWith('image/')) {
      alert('Please select a valid image file');
      input.value = '';
      return;
    }

    // Read file as base64
    const reader = new FileReader();
    reader.onload = (e) => {
      const base64String = e.target?.result as string;
      this.iconPreview = base64String;
      this.form.patchValue({ icon: base64String });
      this.form.markAsTouched();
    };
    reader.readAsDataURL(file);
  }

  clearIcon(): void {
    this.iconPreview = null;
    this.form.patchValue({ icon: '' });
    this.form.markAsTouched();
  }

  zoomPhoto(): void {
    this.showZoomModal = true;
  }

  closeZoomModal(): void {
    this.showZoomModal = false;
  }

  save(): void {
    if (!this.form.valid) {
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }
}
