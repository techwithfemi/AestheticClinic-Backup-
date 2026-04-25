import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticConsultation, AestheticPatient, AestheticPhoto } from '../../../models/aesthetic.model';

interface ConsultationOption {
  id: number;
  label: string;
}

@Component({
  selector: 'app-photos',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule,
    MatIconModule
  ],
  template: `
    <div class="photos-page">
      <mat-card class="form-card">
        <h2>Before & After Photo Management</h2>

        <form [formGroup]="form" class="form-grid">
          <mat-form-field appearance="outline">
            <mat-label>Consultation</mat-label>
            <mat-select formControlName="consultationId">
              @for (option of consultationOptions(); track option.id) {
                <mat-option [value]="option.id">{{ option.label }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Type</mat-label>
            <mat-select formControlName="type">
              <mat-option value="Before">Before</mat-option>
              <mat-option value="After">After</mat-option>
              <mat-option value="Progress">Progress</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>File Name</mat-label>
            <input matInput formControlName="fileName" />
          </mat-form-field>

          <div class="file-control">
            <button mat-stroked-button type="button" (click)="fileInput.click()">
              {{ editing() ? 'Replace Image' : 'Choose Image' }}
            </button>
            <input #fileInput type="file" accept="image/*" (change)="onFilePicked($event)" hidden />
            <span class="file-name">{{ selectedFileName() || 'No file selected' }}</span>
          </div>

          @if (previewSource()) {
            <div class="preview-pane">
              <img [src]="previewSource()!" alt="Selected preview" />
            </div>
          }
        </form>

        <div class="actions">
          <button mat-raised-button color="primary" (click)="save()" [disabled]="loadingIndicator">
            {{ editing() ? 'Update' : 'Add' }} Photo
          </button>
          <button mat-stroked-button type="button" (click)="resetForm()">Clear</button>
        </div>
      </mat-card>

      <mat-card>
        <h3>Photo Library</h3>
        <table mat-table [dataSource]="photos()" class="data-table">
          <ng-container matColumnDef="thumbnail">
            <th mat-header-cell *matHeaderCellDef>Preview</th>
            <td mat-cell *matCellDef="let row">
              <button class="thumb-btn" type="button" (click)="openFullImage(row)" [attr.aria-label]="'Open full image ' + (row.fileName || '')">
                <img class="thumb" [src]="row.thumbnailUrl || row.url" [alt]="row.fileName || 'Photo'" />
              </button>
            </td>
          </ng-container>

          <ng-container matColumnDef="consultation">
            <th mat-header-cell *matHeaderCellDef>Consultation</th>
            <td mat-cell *matCellDef="let row">{{ resolveConsultationLabel(row.consultationId) }}</td>
          </ng-container>

          <ng-container matColumnDef="type">
            <th mat-header-cell *matHeaderCellDef>Type</th>
            <td mat-cell *matCellDef="let row">{{ row.type || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>File Name</th>
            <td mat-cell *matCellDef="let row">{{ row.fileName || '—' }}</td>
          </ng-container>

          <ng-container matColumnDef="created">
            <th mat-header-cell *matHeaderCellDef>Uploaded</th>
            <td mat-cell *matCellDef="let row">{{ row.createdDate | date:'medium' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let row">
              <button mat-icon-button type="button" (click)="openFullImage(row)" aria-label="View full image">
                <mat-icon>visibility</mat-icon>
              </button>
              <button mat-icon-button type="button" (click)="edit(row)" aria-label="Edit">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button type="button" (click)="remove(row.id)" aria-label="Delete">
                <mat-icon>delete</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
        </table>
      </mat-card>

      @if (isFullImageOpen()) {
        <div class="full-image-overlay" (click)="closeFullImage()" (keydown.escape)="closeFullImage()" tabindex="0">
          <div class="full-image-dialog" (click)="$event.stopPropagation()">
            <div class="full-image-header">
              <span class="full-image-title">{{ fullImageName() || 'Photo Preview' }}</span>
              <button mat-icon-button type="button" (click)="closeFullImage()" aria-label="Close full image preview">
                <mat-icon>close</mat-icon>
              </button>
            </div>
            <img class="full-image" [src]="fullImageUrl()!" [alt]="fullImageName() || 'Photo preview'" />
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .photos-page { padding: 20px; display: grid; gap: 16px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    .file-control { display: flex; align-items: center; gap: 10px; min-height: 56px; }
    .file-name { font-size: 0.85rem; color: #555; }
    .preview-pane { grid-column: 1 / -1; display: flex; justify-content: flex-start; }
    .preview-pane img { width: 180px; height: 180px; object-fit: cover; border-radius: 8px; border: 1px solid #e5e5e5; }
    .actions { display: flex; gap: 10px; margin-top: 10px; }
    .data-table { width: 100%; }
    .thumb { width: 54px; height: 54px; object-fit: cover; border-radius: 6px; border: 1px solid #e2e2e2; }
    .thumb-btn { padding: 0; background: none; border: none; cursor: pointer; display: inline-flex; border-radius: 6px; }
    .thumb-btn:focus-visible { outline: 2px solid #1976d2; outline-offset: 2px; }
    .full-image-overlay { position: fixed; inset: 0; background: rgba(0,0,0,0.75); display: flex; align-items: center; justify-content: center; z-index: 1000; }
    .full-image-dialog { width: min(92vw, 1100px); max-height: 92vh; background: #fff; border-radius: 10px; overflow: hidden; display: flex; flex-direction: column; }
    .full-image-header { display: flex; align-items: center; justify-content: space-between; padding: 6px 10px; border-bottom: 1px solid #eee; }
    .full-image-title { font-weight: 600; font-size: 0.95rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .full-image { width: 100%; height: auto; max-height: calc(92vh - 56px); object-fit: contain; background: #111; }
    @media (max-width: 992px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class PhotosComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  loadingIndicator = false;
  readonly photos = signal<AestheticPhoto[]>([]);
  readonly consultationOptions = signal<ConsultationOption[]>([]);
  readonly editingId = signal<number | null>(null);
  readonly editing = computed(() => this.editingId() !== null);
  readonly displayedColumns = ['thumbnail', 'consultation', 'type', 'name', 'created', 'actions'];

  readonly selectedFile = signal<File | null>(null);
  readonly selectedFileName = signal<string>('');
  readonly selectedPreviewUrl = signal<string | null>(null);
  readonly currentPhotoUrl = signal<string | null>(null);
  readonly previewSource = computed(() => this.selectedPreviewUrl() || this.currentPhotoUrl());
  readonly fullImageUrl = signal<string | null>(null);
  readonly fullImageName = signal<string>('');
  readonly isFullImageOpen = computed(() => !!this.fullImageUrl());

  readonly form = this.fb.nonNullable.group({
    id: [0],
    consultationId: [0, Validators.min(1)],
    fileName: ['', Validators.required],
    type: ['Before']
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading photos...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.consultationOptions.set(this.buildConsultationOptions(patients));

        this.endpoint.getPhotosEndpoint<AestheticPhoto[]>().subscribe({
          next: photos => {
            this.photos.set(photos);
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
          },
          error: error => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage('Load error', 'Unable to load photo library.', MessageSeverity.error, error);
          }
        });
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load error', 'Unable to load consultation options.', MessageSeverity.error, error);
      }
    });
  }

  onFilePicked(event: Event): void {
    const element = event.target as HTMLInputElement;
    const file = element.files?.[0] ?? null;

    this.clearObjectPreview();
    this.selectedFile.set(file);
    this.selectedFileName.set(file?.name ?? '');

    if (file) {
      if (!this.form.controls.fileName.value?.trim()) {
        this.form.controls.fileName.setValue(file.name);
      }

      this.selectedPreviewUrl.set(URL.createObjectURL(file));
    }

    element.value = '';
  }

  save(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation error', 'Consultation and file name are required.', MessageSeverity.warn);
      return;
    }

    if (!this.editing() && !this.selectedFile()) {
      this.alertService.showStickyMessage('Validation error', 'Please choose an image file before saving.', MessageSeverity.warn);
      return;
    }

    const value = this.form.getRawValue();
    const existing = this.photos().find(x => x.id === value.id);

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.editing() ? 'Updating photo...' : 'Saving photo...');

    if (this.selectedFile()) {
      const formData = new FormData();
      formData.append('consultationId', String(value.consultationId));
      formData.append('type', value.type ?? 'Before');
      formData.append('file', this.selectedFile() as Blob, this.form.controls.fileName.value || this.selectedFile()!.name);

      const request = this.editing()
        ? this.endpoint.updatePhotoUploadEndpoint<AestheticPhoto>(value.id, formData)
        : this.endpoint.uploadPhotoEndpoint<AestheticPhoto>(formData);

      request.subscribe({
        next: () => this.onSaveSuccess(),
        error: error => this.onSaveError(error)
      });

      return;
    }

    if (!existing) {
      this.onSaveError(new Error('Photo not found for update.'));
      return;
    }

    const payload: AestheticPhoto = {
      id: value.id,
      consultationId: value.consultationId,
      fileName: value.fileName,
      type: value.type,
      url: existing.url,
      thumbnailUrl: existing.thumbnailUrl,
      createdDate: existing.createdDate
    };

    this.endpoint.updatePhotoEndpoint<AestheticPhoto>(value.id, payload).subscribe({
      next: () => this.onSaveSuccess(),
      error: error => this.onSaveError(error)
    });
  }

  edit(row: AestheticPhoto): void {
    this.editingId.set(row.id);
    this.currentPhotoUrl.set(row.url);
    this.clearObjectPreview();
    this.selectedFile.set(null);
    this.selectedFileName.set('');

    this.form.patchValue({
      id: row.id,
      consultationId: row.consultationId,
      fileName: row.fileName ?? '',
      type: row.type ?? 'Before'
    });
  }

  remove(id: number): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting photo...');

    this.endpoint.deletePhotoEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.resetForm();
        this.load();
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete error', 'Unable to delete photo.', MessageSeverity.error, error);
      }
    });
  }

  resetForm(): void {
    this.editingId.set(null);
    this.selectedFile.set(null);
    this.selectedFileName.set('');
    this.currentPhotoUrl.set(null);
    this.clearObjectPreview();

    this.form.reset({
      id: 0,
      consultationId: 0,
      fileName: '',
      type: 'Before'
    });
  }

  resolveConsultationLabel(consultationId: number): string {
    return this.consultationOptions().find(x => x.id === consultationId)?.label ?? `Consultation #${consultationId}`;
  }

  openFullImage(row: AestheticPhoto): void {
    this.fullImageUrl.set(row.url);
    this.fullImageName.set(row.fileName || 'Photo Preview');
  }

  closeFullImage(): void {
    this.fullImageUrl.set(null);
    this.fullImageName.set('');
  }

  private buildConsultationOptions(patients: AestheticPatient[]): ConsultationOption[] {
    return patients
      .flatMap(patient =>
        (patient.consultations || []).map((consultation: AestheticConsultation) => ({
          id: consultation.id,
          label: `${patient.firstName} ${patient.lastName} - ${consultation.procedureType || 'Aesthetics'} (${this.formatDate(consultation.consultationDate)})`
        }))
      )
      .sort((a, b) => a.label.localeCompare(b.label));
  }

  private formatDate(value?: string): string {
    if (!value) {
      return 'No date';
    }

    return value.slice(0, 10);
  }

  private onSaveSuccess(): void {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;
    this.resetForm();
    this.load();
  }

  private onSaveError(error: unknown): void {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;
    this.alertService.showStickyMessage('Save error', 'Unable to save photo.', MessageSeverity.error, error);
  }

  private clearObjectPreview(): void {
    const current = this.selectedPreviewUrl();
    if (current) {
      URL.revokeObjectURL(current);
    }

    this.selectedPreviewUrl.set(null);
  }
}
