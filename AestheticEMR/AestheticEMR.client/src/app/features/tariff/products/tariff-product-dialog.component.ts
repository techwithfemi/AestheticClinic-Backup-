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
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Product' : 'Add Product' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">
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
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Buying Price</mat-label>
            <input matInput type="number" min="0" step="0.01" formControlName="buyingPrice" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Units In Stock</mat-label>
            <input matInput type="number" min="0" step="1" formControlName="unitsInStock" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput rows="2" formControlName="description"></textarea>
          </mat-form-field>

          <div class="icon-upload-section">
            <div class="icon-label">Product Icon/Photo</div>
            <div class="icon-preview-container">
              @if (iconPreview) {
                <img [src]="iconPreview" alt="Icon preview" class="icon-preview" />
              } @else {
                <div class="icon-placeholder">
                  <mat-icon>image</mat-icon>
                  <span>No image selected</span>
                </div>
              }
            </div>
            <input 
              type="file" 
              #fileInput 
              (change)="onFileSelected($event)" 
              accept="image/*"
              class="hidden-input"
            />
            <button 
              mat-stroked-button 
              type="button" 
              (click)="fileInput.click()"
              class="upload-btn">
              <mat-icon>upload</mat-icon>
              Choose Photo
            </button>
            @if (form.get('icon')?.value) {
              <button 
                mat-icon-button 
                type="button" 
                (click)="clearIcon()"
                class="clear-btn"
                title="Clear image">
                <mat-icon>clear</mat-icon>
              </button>
            }
          </div>

          <div class="check-row">
            <mat-checkbox formControlName="isActive">Active</mat-checkbox>
            <mat-checkbox formControlName="isDiscontinued">Discontinued</mat-checkbox>
          </div>
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" type="button" [disabled]="form.invalid" (click)="save()">{{ data.isEdit ? 'Update' : 'Save' }}</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 540px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .full-width { width: 100%; margin-bottom: 12px; }
    .check-row { display: flex; gap: 16px; margin-top: 4px; }
    
    .icon-upload-section { 
      margin-bottom: 16px; 
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
    }
    .icon-preview-container { 
      display: flex; 
      align-items: center; 
      justify-content: center;
      width: 100%; 
      height: 120px; 
      border: 2px dashed #ccc; 
      border-radius: 4px;
      margin-bottom: 12px;
      background-color: #fff;
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
      font-size: 32px; 
      width: 32px; 
      height: 32px;
      color: #ccc;
    }
    .hidden-input { 
      display: none; 
    }
    .upload-btn { 
      width: 100%; 
      margin-bottom: 8px;
    }
    .clear-btn { 
      position: absolute;
      right: 12px;
    }
    
    @media (max-width: 768px) {
      .dialog-content { width: 420px; }
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
  private defaultCategoryId = this.data.item?.productCategoryId || this.data.categories[0]?.id || 0;

  form = this.fb.nonNullable.group({
    id: [this.data.item?.id ?? 0],
    name: [this.data.item?.name ?? '', [Validators.required, Validators.maxLength(100)]],
    description: [this.data.item?.description ?? '', [Validators.maxLength(500)]],
    icon: [this.data.item?.icon ?? '', [Validators.maxLength(256)]],
    buyingPrice: [this.data.item?.buyingPrice ?? 0, [Validators.min(0)]],
    unitsInStock: [this.data.item?.unitsInStock ?? 0, [Validators.min(0)]],
    isActive: [this.data.item?.isActive ?? true],
    isDiscontinued: [this.data.item?.isDiscontinued ?? false],
    productCategoryId: [this.defaultCategoryId, [Validators.min(1)]]
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

    // Read file as base64
    const reader = new FileReader();
    reader.onload = (e) => {
      const base64String = e.target?.result as string;
      this.iconPreview = base64String;
      this.form.patchValue({ icon: base64String });
    };
    reader.readAsDataURL(file);
  }

  clearIcon(): void {
    this.iconPreview = null;
    this.form.patchValue({ icon: '' });
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.getRawValue() as ProductEdit);
  }
}
