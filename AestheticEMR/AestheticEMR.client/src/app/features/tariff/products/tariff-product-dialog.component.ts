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
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Product Tariff' : 'Add Product Tariff' }}</h2>
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

          <div class="row-2">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Buying Price</mat-label>
              <input matInput type="number" min="0" step="0.01" formControlName="buyingPrice" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Selling Price</mat-label>
              <input matInput type="number" min="0" step="0.01" formControlName="sellingPrice" />
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Units In Stock</mat-label>
            <input matInput type="number" min="0" step="1" formControlName="unitsInStock" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Description</mat-label>
            <textarea matInput rows="2" formControlName="description"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Icon</mat-label>
            <input matInput formControlName="icon" />
          </mat-form-field>

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
    .row-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .check-row { display: flex; gap: 16px; margin-top: 4px; }
    @media (max-width: 768px) {
      .row-2 { grid-template-columns: 1fr; }
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

  private defaultCategoryId = this.data.item?.productCategoryId || this.data.categories[0]?.id || 0;

  form = this.fb.nonNullable.group({
    id: [this.data.item?.id ?? 0],
    name: [this.data.item?.name ?? '', [Validators.required, Validators.maxLength(100)]],
    description: [this.data.item?.description ?? '', [Validators.maxLength(500)]],
    icon: [this.data.item?.icon ?? '', [Validators.maxLength(256)]],
    buyingPrice: [this.data.item?.buyingPrice ?? 0, [Validators.min(0)]],
    sellingPrice: [this.data.item?.sellingPrice ?? 0, [Validators.min(0)]],
    unitsInStock: [this.data.item?.unitsInStock ?? 0, [Validators.min(0)]],
    isActive: [this.data.item?.isActive ?? true],
    isDiscontinued: [this.data.item?.isDiscontinued ?? false],
    productCategoryId: [this.defaultCategoryId, [Validators.min(1)]]
  });

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.getRawValue() as ProductEdit);
  }
}
