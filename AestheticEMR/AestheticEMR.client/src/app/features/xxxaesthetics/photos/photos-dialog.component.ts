import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';

import { AestheticPhoto } from '../../../models/aesthetic.model';

interface ConsultationOption {
  id: number;
  label: string;
}

interface PhotoDialogData {
  isEdit: boolean;
  photo?: AestheticPhoto;
  consultationOptions: ConsultationOption[];
  replaceMode?: boolean;
}

@Component({
  selector: 'app-photos-dialog',
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
        <h2 mat-dialog-title>{{ data.isEdit ? 'Edit Photo' : 'Add Photo' }}</h2>
        <button mat-icon-button type="button" (click)="dialogRef.close()" class="close-btn" aria-label="Close dialog">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content>
        <form [formGroup]="form">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Search Patient / Consult ID</mat-label>
            <input matInput [value]="searchText" (input)="onSearchChange($event)" placeholder="Type patient or consult ID" />
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Patient [ConsultId]</mat-label>
            <mat-select formControlName="consultationId" required>
              @for (option of filteredConsultationOptions; track option.id) {
                <mat-option [value]="option.id">{{ option.label }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Type</mat-label>
            <mat-select formControlName="type">
              <mat-option value="Before">Before</mat-option>
              <mat-option value="After">After</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>File Name</mat-label>
            <input matInput formControlName="fileName" required />
          </mat-form-field>

          @if (!data.isEdit || data.replaceMode) {
            <div class="file-control">
              <button mat-stroked-button type="button" (click)="fileInput.click()">
                {{ data.isEdit ? 'Replace Image' : 'Choose Image' }}
              </button>
              <input #fileInput type="file" accept="image/*" (change)="onFilePicked($event)" hidden />
              <span class="file-name">{{ selectedFileName }}</span>
            </div>
          }

          @if (previewUrl) {
            <div class="preview-pane">
              <img [src]="previewUrl" alt="Preview" />
            </div>
          }
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button (click)="dialogRef.close()">Cancel</button>
        <button mat-raised-button color="primary" (click)="save()">Save</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .dialog-content { width: 420px; box-sizing: border-box; padding: 16px; }
    .dialog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .dialog-header h2 { margin: 0; flex: 1; font-size: 1.2rem; }
    .close-btn { position: relative; right: 0; top: 0; min-width: auto; }
    .full-width { width: 100%; margin-bottom: 12px; box-sizing: border-box; }
    .file-control { display: flex; align-items: center; gap: 8px; margin: 12px 0; flex-wrap: wrap; }
    .file-name { font-size: 0.8rem; color: #555; word-break: break-word; }
    .preview-pane { margin-top: 12px; }
    .preview-pane img { width: 100%; max-height: 250px; object-fit: contain; border-radius: 6px; border: 1px solid #e5e5e5; box-sizing: border-box; }
    mat-dialog-content { max-height: 65vh; overflow-y: auto; padding: 0; margin: 0; }
    mat-form-field { display: block; width: 100%; }
    mat-form-field ::ng-deep .mat-mdc-form-field-infix { padding: 8px 0; }
  `]
})
export class PhotosDialogComponent {
  private fb = inject(FormBuilder);
  private _matDialogRef = inject(MatDialogRef<PhotosDialogComponent>);
  private _data = inject<PhotoDialogData>(MAT_DIALOG_DATA);

  get dialogRef() { return this._matDialogRef; }
  get data() { return this._data; }

  selectedFile: File | null = null;
  selectedFileName = '';
  previewUrl: string | null = null;
  searchText = '';

  get filteredConsultationOptions(): ConsultationOption[] {
    const term = this.searchText.trim().toLowerCase();
    if (!term) {
      return this.data.consultationOptions;
    }

    return this.data.consultationOptions.filter(option => option.label.toLowerCase().includes(term));
  }

  form = this.fb.nonNullable.group({
    id: [0],
    consultationId: [0, Validators.min(1)],
    fileName: ['', Validators.required],
    type: ['Before']
  });

  constructor() {
    if (this.data.isEdit && this.data.photo) {
      this.form.patchValue({
        id: this.data.photo.id,
        consultationId: this.data.photo.consultationId,
        fileName: this.data.photo.fileName ?? '',
        type: this.data.photo.type ?? 'Before'
      });

      if (!this.data.replaceMode && this.data.photo.url) {
        this.previewUrl = this.data.photo.url;
      }
    }
  }

  onFilePicked(event: Event): void {
    const element = event.target as HTMLInputElement;
    const file = element.files?.[0] ?? null;

    this.clearPreview();
    this.selectedFile = file;
    this.selectedFileName = file?.name ?? '';

    if (file) {
      if (!this.form.controls.fileName.value?.trim()) {
        this.form.controls.fileName.setValue(file.name);
      }

      this.previewUrl = URL.createObjectURL(file);
    }

    element.value = '';
  }

  onSearchChange(event: Event): void {
    this.searchText = (event.target as HTMLInputElement).value ?? '';
    const matches = this.filteredConsultationOptions;
    if (matches.length === 1) {
      this.form.controls.consultationId.setValue(matches[0].id);
    }
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    if (!this.data.isEdit && !this.selectedFile) {
      return;
    }

    const value = this.form.getRawValue();

    const result = {
      id: value.id,
      consultationId: value.consultationId,
      fileName: value.fileName,
      type: value.type,
      file: this.selectedFile
    };

    this.dialogRef.close(result);
  }

  private clearPreview(): void {
    if (this.previewUrl && this.previewUrl.startsWith('blob:')) {
      URL.revokeObjectURL(this.previewUrl);
    }

    this.previewUrl = null;
  }
}
