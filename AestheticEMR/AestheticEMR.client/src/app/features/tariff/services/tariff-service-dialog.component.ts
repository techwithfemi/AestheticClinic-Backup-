import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';

import { ServiceTariff } from '../../../models/legacy/service-tariff.model';

interface TariffServiceDialogData {
  isEdit: boolean;
  item?: ServiceTariff;
}

@Component({
  selector: 'app-tariff-service-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Tariff Item' : 'Add Tariff Item' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Service</mat-label>
            <input matInput formControlName="service" />
            @if (form.get('service')?.invalid && form.get('service')?.touched) {
              <mat-error>Service is required</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Price</mat-label>
            <input matInput type="number" min="0" step="0.01" formControlName="price" />
            @if (form.get('price')?.invalid && form.get('price')?.touched) {
              <mat-error>Price must be zero or greater</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Capitated</mat-label>
            <mat-select formControlName="capitated">
              <mat-option value="NO">NO</mat-option>
              <mat-option value="YES">YES</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Tariff Status</mat-label>
            <mat-select formControlName="tariffStatus">
              <mat-option value="FIXED">FIXED</mat-option>
              <mat-option value="VARIABLE">VARIABLE</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Revenue Type</mat-label>
            <input matInput formControlName="revType" />
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
    .dialog-content { width: 420px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .full-width { width: 100%; margin-bottom: 12px; }
    mat-dialog-content { max-height: 65vh; overflow-y: auto; padding: 0; margin: 0; }
  `]
})
export class TariffServiceDialogComponent {
  private fb = inject(FormBuilder);
  private _dialogRef = inject(MatDialogRef<TariffServiceDialogComponent>);
  private _data = inject<TariffServiceDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  form = this.fb.nonNullable.group({
    sno: [this.data.item?.sno ?? 0],
    service: [this.data.item?.service ?? '', [Validators.required, Validators.maxLength(255)]],
    price: [this.data.item?.price ?? 0, [Validators.required, Validators.min(0)]],
    capitated: [this.data.item?.capitated ?? 'NO', [Validators.maxLength(50)]],
    tariffStatus: [this.data.item?.tariffStatus ?? 'FIXED', [Validators.maxLength(50)]],
    revType: [this.data.item?.revType ?? '', [Validators.maxLength(200)]],
    remarks: [this.data.item?.remarks ?? 'HMO', [Validators.maxLength(255)]],
    usersCat: [this.data.item?.usersCat ?? '', [Validators.maxLength(50)]]
  });

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.dialogRef.close(this.form.getRawValue());
  }
}
