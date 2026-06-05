import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import * as XLSX from 'xlsx';
import { MatTooltipModule } from '@angular/material/tooltip';

export interface ProductUploadDialogResult {
  file?: File | null;
  sheetName?: string | null;
}

interface ProductUploadDialogData {
  allowedFileFormats?: string[];
}

@Component({
  selector: 'app-product-upload-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTooltipModule
  ],
  template: `
    <div class="dialog-content">
      <div class="dialog-header">
        <h2 mat-dialog-title>Upload Products</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <div class="file-row">
          <button mat-stroked-button type="button" (click)="fileInput.click()">
            <mat-icon>upload_file</mat-icon>
            Select File...
          </button>
          <input #fileInput type="file" [attr.accept]="fileInputAccept" (change)="onFilePicked($event)" hidden />
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

        <p class="hint">Upload will replace all existing inventory products in the database.</p>
        <p class="hint">Supported formats: {{ supportedFormatsText }}</p>
        <p class="hint">First row = header. Product name and quantity columns are read from inventory settings.</p>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <span [style.cursor]="canContinue() ? 'default' : 'not-allowed'" [matTooltip]="canContinue() ? '' : 'Select a file' + (sheetNames.length > 1 ? ', then pick a worksheet' : '')">
          <button mat-raised-button color="primary" type="button"
            [disabled]="!canContinue()"
            [style.pointer-events]="canContinue() ? 'auto' : 'none'"
            (click)="submit()">Continue</button>
        </span>
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
  `]
})
export class ProductUploadDialogComponent {
  private _dialogRef = inject(MatDialogRef<ProductUploadDialogComponent>);
  private _data = inject<ProductUploadDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._dialogRef; }
  get data() { return this._data; }

  selectedFile: File | null = null;
  sheetNames: string[] = [];
  selectedSheet = '';

  get allowedFileFormats(): string[] {
    const configured = this.data.allowedFileFormats?.map(x => x?.trim().toLowerCase()).filter(Boolean) ?? [];
    return configured.length ? configured : ['xls', 'xlsx', 'csv'];
  }

  get fileInputAccept(): string {
    return this.allowedFileFormats.map(x => `.${x}`).join(',');
  }

  get supportedFormatsText(): string {
    return this.allowedFileFormats.map(x => `.${x}`).join(', ');
  }

  onFilePicked(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    input.value = '';

    this.selectedFile = null;
    this.sheetNames = [];
    this.selectedSheet = '';

    if (!file) return;

    const ext = file.name.split('.').pop()?.toLowerCase();
    if (!ext || !this.allowedFileFormats.includes(ext)) return;

    this.selectedFile = file;

    if (ext !== 'xls' && ext !== 'xlsx') return;

    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const data = new Uint8Array(e.target!.result as ArrayBuffer);
        const wb = XLSX.read(data, { type: 'array', bookSheets: true });
        this.sheetNames = wb.SheetNames ?? [];
        this.selectedSheet = '';
      } catch {
        this.sheetNames = [];
        this.selectedSheet = '';
      }
    };
    reader.readAsArrayBuffer(file);
  }

  canContinue(): boolean {
    if (!this.selectedFile) return false;
    if (this.sheetNames.length > 1 && !this.selectedSheet) return false;
    return true;
  }

  submit(): void {
    if (!this.canContinue()) return;

    this.dialogRef.close({
      file: this.selectedFile,
      sheetName: this.selectedSheet || null
    } as ProductUploadDialogResult);
  }
}
