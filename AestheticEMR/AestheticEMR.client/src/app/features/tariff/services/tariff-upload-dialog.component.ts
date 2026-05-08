import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { TariffCompany } from '../../../models/legacy/tariff-company.model';

export interface TariffUploadDialogResult {
  file?: File | null;
  sourceCoyId?: string | null;
}

interface TariffUploadDialogData {
  sourceCompanies: TariffCompany[];
}

@Component({
  selector: 'app-tariff-upload-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>Upload Services Tariff from Excel</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Use tariff from existing company</mat-label>
          <mat-select [(ngModel)]="selectedSourceCoyId">
            <mat-option value="">-- None --</mat-option>
            @for (company of data.sourceCompanies; track company.coyId) {
              <mat-option [value]="company.coyId">{{ company.company }} [{{ company.coyId }}]</mat-option>
            }
          </mat-select>
        </mat-form-field>

        <div class="file-row">
          <button mat-stroked-button type="button" (click)="fileInput.click()">
            <mat-icon>upload_file</mat-icon>
            Select File...
          </button>
          <input #fileInput type="file" accept=".xls,.xlsx,.csv" (change)="onFilePicked($event)" hidden />
          <span class="file-name">{{ selectedFile?.name || 'No file selected' }}</span>
        </div>

        <p class="hint">If no file is selected, the tariff from the selected company above will be used.</p>
        <p class="hint">Re-upload will replace any existing tariff upload for the selected company.</p>
        <p class="hint">Supported formats: .xls, .xlsx, .csv</p>
        <p class="hint">The first row should be a header row. Column 1 = service item, Column 2 = price. Blank rows are ignored.</p>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" type="button" [disabled]="!selectedFile && !selectedSourceCoyId" (click)="submit()">Continue</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 500px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .full-width { width: 100%; margin-bottom: 12px; }
    .file-row { display: flex; align-items: center; gap: 10px; margin-bottom: 12px; flex-wrap: wrap; }
    .file-name { color: #666; font-size: 0.9rem; word-break: break-word; }
    .hint { margin: 12px 0 0; color: #777; font-size: 0.85rem; }
  `]
})
export class TariffUploadDialogComponent {
  private _dialogRef = inject(MatDialogRef<TariffUploadDialogComponent>);
  private _data = inject<TariffUploadDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  selectedFile: File | null = null;
  selectedSourceCoyId = '';

  onFilePicked(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] ?? null;
    input.value = '';
  }

  submit(): void {
    if (!this.selectedFile && !this.selectedSourceCoyId) {
      return;
    }

    this.dialogRef.close({
      file: this.selectedFile,
      sourceCoyId: this.selectedSourceCoyId || null
    } as TariffUploadDialogResult);
  }
}
