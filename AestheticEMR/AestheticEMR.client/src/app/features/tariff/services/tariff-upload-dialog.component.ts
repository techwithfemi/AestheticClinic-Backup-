import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { TariffCompany } from '../../../models/legacy/tariff-company.model';
import * as XLSX from 'xlsx';

export interface TariffUploadDialogResult {
  file?: File | null;
  sourceCoyId?: string | null;
  sheetName?: string | null;
}

interface TariffUploadDialogData {
  sourceCompanies: TariffCompany[];
  category: string;
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
        <h2 mat-dialog-title>Upload Tariff — {{ data.category }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <div class="category-badge">
          <mat-icon class="badge-icon">label</mat-icon>
          <span>Category: <strong>{{ data.category }}</strong></span>
        </div>

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

        @if (sheetNames.length > 1) {
          <mat-form-field appearance="outline" class="full-width sheet-field">
            <mat-label>Select Worksheet</mat-label>
            <mat-select [(ngModel)]="selectedSheet">
              @for (sheet of sheetNames; track sheet) {
                <mat-option [value]="sheet">{{ sheet }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        }

        <p class="hint">If no file is selected, the tariff from the selected company above will be used.</p>
        <p class="hint">Re-upload will replace any existing <strong>{{ data.category }}</strong> tariff items for the selected company.</p>
        <p class="hint">Supported formats: .xls, .xlsx, .csv</p>
        <p class="hint">First row = header. Column 1 = service item, Column 2 = price. Blank rows are ignored.</p>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" type="button"
          [disabled]="!canContinue()"
          (click)="submit()">Continue</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 500px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .full-width { width: 100%; margin-bottom: 12px; }
    .sheet-field { margin-top: 4px; }
    .file-row { display: flex; align-items: center; gap: 10px; margin-bottom: 12px; flex-wrap: wrap; }
    .file-name { color: #666; font-size: 0.9rem; word-break: break-word; }
    .hint { margin: 12px 0 0; color: #777; font-size: 0.85rem; }
    .category-badge { display: flex; align-items: center; gap: 6px; background: rgba(25,118,210,0.08); border-radius: 6px; padding: 8px 12px; margin-bottom: 16px; font-size: 0.95rem; color: #1976d2; }
    .badge-icon { font-size: 18px; width: 18px; height: 18px; }
  `]
})
export class TariffUploadDialogComponent {
  private _dialogRef = inject(MatDialogRef<TariffUploadDialogComponent>);
  private _data = inject<TariffUploadDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  selectedFile: File | null = null;
  selectedSourceCoyId = '';
  sheetNames: string[] = [];
  selectedSheet = '';

  onFilePicked(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    input.value = '';

    this.selectedFile = file;
    this.sheetNames = [];
    this.selectedSheet = '';

    if (!file) return;

    const ext = file.name.split('.').pop()?.toLowerCase();
    if (ext !== 'xls' && ext !== 'xlsx') return;   // CSV has no sheets

    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const data = new Uint8Array(e.target!.result as ArrayBuffer);
        const wb = XLSX.read(data, { type: 'array', bookSheets: true });
        this.sheetNames = wb.SheetNames ?? [];
        this.selectedSheet = this.sheetNames[0] ?? '';
      } catch {
        this.sheetNames = [];
        this.selectedSheet = '';
      }
    };
    reader.readAsArrayBuffer(file);
  }

  canContinue(): boolean {
    if (!this.selectedFile && !this.selectedSourceCoyId) return false;
    if (this.selectedFile && this.sheetNames.length > 1 && !this.selectedSheet) return false;
    return true;
  }

  submit(): void {
    if (!this.canContinue()) return;

    this.dialogRef.close({
      file: this.selectedFile,
      sourceCoyId: this.selectedSourceCoyId || null,
      sheetName: this.selectedFile ? (this.selectedSheet || null) : null
    } as TariffUploadDialogResult);
  }
}
