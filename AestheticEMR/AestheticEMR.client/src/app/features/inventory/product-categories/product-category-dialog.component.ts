import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { ProductCategoryEdit } from '../../../models/shop/product.model';

interface ProductCategoryDialogData {
  isEdit: boolean;
  item?: ProductCategoryEdit;
}

@Component({
  selector: 'app-product-category-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Product Category' : 'Add Product Category' }}</h2>
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
            <mat-label>Description</mat-label>
            <textarea matInput rows="2" formControlName="description"></textarea>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Icon</mat-label>
            <input matInput formControlName="icon" />
          </mat-form-field>
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" type="button" [disabled]="form.invalid" (click)="save()">{{ data.isEdit ? 'Update' : 'Save' }}</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 460px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .full-width { width: 100%; margin-bottom: 12px; }
  `]
})
export class ProductCategoryDialogComponent {
  private fb = inject(FormBuilder);
  private _dialogRef = inject(MatDialogRef<ProductCategoryDialogComponent>);
  private _data = inject<ProductCategoryDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  form = this.fb.nonNullable.group({
    id: [this.data.item?.id ?? 0],
    name: [this.data.item?.name ?? '', [Validators.required, Validators.maxLength(100)]],
    description: [this.data.item?.description ?? '', [Validators.maxLength(500)]],
    icon: [this.data.item?.icon ?? '', [Validators.maxLength(256)]]
  });

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.getRawValue() as ProductCategoryEdit);
  }
}
