import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AttendanceEndpoint } from '../../../services/attendance-endpoint.service';
import { AestheticConsultation, AestheticPatient, AestheticPhoto } from '../../../models/aesthetic.model';
import { Attendance } from '../../../models/legacy/attendance.model';
import { PhotosDialogComponent } from './photos-dialog.component';

interface PhotoDialogResult {
  id: number;
  consultationId: number;
  fileName: string;
  type: string;
  file?: File;
}

@Component({
  selector: 'app-photos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ],
  template: `
    <div class="photos-page">
      <div class="page-header">
        <div>
          <h2>Before & After Photos</h2>
          <p class="subtitle">Upload, manage, and view before and after treatment photos for patient consultations.</p>
        </div>
        <button mat-raised-button color="primary" (click)="openAddDialog()">
          <mat-icon>add</mat-icon>
          Add Photo
        </button>
      </div>

      <div class="search-section">
        <input
          type="text"
          class="search-input"
          [(ngModel)]="searchText"
          (input)="onSearch()"
          placeholder="Search by patient name, PNO, or consultation..." />
      </div>

      <mat-card>
        @if (filteredPhotos().length === 0 && !loadingIndicator) {
          <p class="empty-state">No photos uploaded yet.</p>
        }

        @if (filteredPhotos().length > 0) {
          <table mat-table [dataSource]="filteredPhotos()" class="data-table">

            <ng-container matColumnDef="thumbnail">
              <th mat-header-cell *matHeaderCellDef>Preview</th>
              <td mat-cell *matCellDef="let row">
                <button class="thumb-btn" type="button" (click)="openFullImage(row)" [attr.aria-label]="'View full image'">
                  <img class="thumb" [src]="row.thumbnailUrl || row.url" [alt]="row.fileName || 'Photo'" />
                </button>
              </td>
            </ng-container>

            <ng-container matColumnDef="consultation">
              <th mat-header-cell *matHeaderCellDef>Patient (PNO)</th>
              <td mat-cell *matCellDef="let row">{{ resolvePatientLabel(row.consultationId) }}</td>
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
              <td mat-cell *matCellDef="let row">{{ row.createdDate | date:'short' }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let row">
                <button mat-icon-button type="button" (click)="openFullImage(row)" title="View full image">
                  <mat-icon>visibility</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button mat-icon-button type="button" (click)="delete(row.id)" title="Delete" [disabled]="true">
                  <mat-icon>delete</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
          </table>
        }
      </mat-card>

      <!-- Full Image Overlay -->
      @if (isFullImageOpen()) {
        <div class="full-image-overlay" (click)="closeFullImage()" tabindex="0" (keydown.escape)="closeFullImage()">
          <div class="full-image-dialog" (click)="$event.stopPropagation()" (keydown)="$event.stopPropagation()" role="dialog" tabindex="0">
            <div class="full-image-header">
              <span class="full-image-title">{{ fullImageName() || 'Photo Preview' }}</span>
              <button mat-icon-button type="button" (click)="closeFullImage()" aria-label="Close">
                <mat-icon>close</mat-icon>
              </button>
            </div>
            <img class="full-image" [src]="fullImageUrl()!" [alt]="fullImageName() || 'Photo'" />
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .photos-page { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 20px; }
    .subtitle { color: #666; margin: 4px 0 0; font-size: 0.9rem; }
    .search-section { margin-bottom: 16px; }
    .search-input { width: 100%; padding: 10px 12px; border: 1px solid #ddd; border-radius: 6px; font-size: 0.95rem; }
    .data-table { width: 100%; }
    .thumb { width: 54px; height: 54px; object-fit: cover; border-radius: 6px; border: 1px solid #e2e2e2; }
    .thumb-btn { padding: 0; background: none; border: none; cursor: pointer; display: inline-flex; border-radius: 6px; }
    .thumb-btn:focus-visible { outline: 2px solid #1976d2; outline-offset: 2px; }
    .full-image-overlay { position: fixed; inset: 0; background: rgba(0,0,0,0.75); display: flex; align-items: center; justify-content: center; z-index: 1000; }
    .full-image-dialog { width: min(92vw, 1100px); max-height: 92vh; background: #fff; border-radius: 10px; overflow: hidden; display: flex; flex-direction: column; }
    .full-image-header { display: flex; align-items: center; justify-content: space-between; padding: 6px 10px; border-bottom: 1px solid #eee; }
    .full-image-title { font-weight: 600; font-size: 0.95rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .full-image { width: 100%; height: auto; max-height: calc(92vh - 56px); object-fit: contain; background: #111; }
    .empty-state { color: #888; padding: 32px; text-align: center; }
    @media (max-width: 992px) { .photos-page { padding: 12px; } }
  `]
})
export class PhotosComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly dialog = inject(MatDialog);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly consultations = signal<AestheticConsultation[]>([]);
  readonly photos = signal<AestheticPhoto[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly searchText = signal<string>('');
  readonly displayedColumns = ['thumbnail', 'consultation', 'type', 'name', 'created', 'actions'];

  readonly consultationOptions = computed(() => {
    return this.patients()
      .flatMap(patient =>
        (patient.consultations || []).map((consultation: AestheticConsultation) => ({
          id: consultation.id,
          label: `${patient.firstName} ${patient.lastName} [${String(consultation.id).padStart(4, '0')}]`
        }))
      )
      .sort((a, b) => a.label.localeCompare(b.label));
  });

  readonly filteredPhotos = computed(() => {
    const search = this.searchText().trim().toLowerCase();

    const base = search
      ? this.photos()
      : this.photos().filter(photo => this.isToday(photo.createdDate));

    if (!search) {
      return base;
    }

    return base.filter(photo => {
      const consultation = this.resolvePatientLabel(photo.consultationId).toLowerCase();
      const type = (photo.type || '').toLowerCase();
      const name = (photo.fileName || '').toLowerCase();

      return consultation.includes(search) || type.includes(search) || name.includes(search);
    });
  });

  readonly fullImageUrl = signal<string | null>(null);
  readonly fullImageName = signal<string>('');
  readonly isFullImageOpen = computed(() => !!this.fullImageUrl());

  readonly todayAttendancePatients = computed(() => {
    const today = new Date().toISOString().split('T')[0];
    return this.attendance()
      .filter(a => a.recDate?.startsWith(today) && a.clinicType?.toLowerCase() === 'aesthetics')
      .map(a => a.pNo);
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading photos and consultations...');

    Promise.all([
      this.endpoint.getPatientsEndpoint<AestheticPatient[]>().toPromise(),
      this.endpoint.getPhotosEndpoint<AestheticPhoto[]>().toPromise(),
      this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise()
    ]).then(([patients, photos, attendance]) => {
      this.patients.set(patients || []);
      this.photos.set(photos || []);
      this.attendance.set(attendance || []);
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load error', 'Unable to load consultations.', MessageSeverity.error, error);
    });
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(PhotosDialogComponent, {
      data: { isEdit: false, consultationOptions: this.consultationOptions(), replaceMode: false },
      width: '420px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: PhotoDialogResult | undefined) => {
      if (!result || !result.file) return;

      const formData = new FormData();
      formData.append('consultationId', String(result.consultationId));
      formData.append('type', result.type ?? 'Before');
      formData.append('file', result.file, result.fileName);

      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Uploading photo...');

      this.endpoint.uploadPhotoEndpoint<AestheticPhoto>(formData).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.load();
          this.alertService.showMessage('Success', 'Photo uploaded.', MessageSeverity.success);
        },
        error: (error: unknown) => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Upload error', 'Unable to upload photo.', MessageSeverity.error, error);
        }
      });
    });
  }

  openEditDialog(photo: AestheticPhoto): void {
    const dialogRef = this.dialog.open(PhotosDialogComponent, {
      data: { isEdit: true, photo, consultationOptions: this.consultationOptions(), replaceMode: false },
      width: '420px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: PhotoDialogResult | undefined) => {
      if (!result) return;

      if (result.file) {
        const formData = new FormData();
        formData.append('consultationId', String(result.consultationId));
        formData.append('type', result.type ?? 'Before');
        formData.append('file', result.file, result.fileName);

        this.loadingIndicator = true;
        this.alertService.startLoadingMessage('Updating photo...');

        this.endpoint.updatePhotoUploadEndpoint<AestheticPhoto>(photo.id, formData).subscribe({
          next: () => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.load();
            this.alertService.showMessage('Success', 'Photo updated.', MessageSeverity.success);
          },
          error: (error: unknown) => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.alertService.showStickyMessage('Update error', 'Unable to update photo.', MessageSeverity.error, error);
          }
        });
      } else {
        const selectedConsultation = this.findConsultation(result.consultationId);
        const selectedPatient = this.findPatientByConsultation(result.consultationId);

        const payload: AestheticPhoto = {
          id: photo.id,
          consultationId: result.consultationId,
          consultId: selectedConsultation ? String(selectedConsultation.id) : photo.consultId,
          pNo: selectedPatient?.pno ?? photo.pNo,
          fileName: result.fileName,
          type: result.type,
          url: photo.url,
          thumbnailUrl: photo.thumbnailUrl,
          createdDate: photo.createdDate
        };

        this.loadingIndicator = true;
        this.alertService.startLoadingMessage('Updating photo...');

        this.endpoint.updatePhotoEndpoint<AestheticPhoto>(photo.id, payload).subscribe({
          next: () => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.load();
            this.alertService.showMessage('Success', 'Photo updated.', MessageSeverity.success);
          },
          error: (error: unknown) => {
            this.alertService.stopLoadingMessage();
            this.loadingIndicator = false;
            this.alertService.showStickyMessage('Update error', 'Unable to update photo.', MessageSeverity.error, error);
          }
        });
      }
    });
  }

  delete(id: number): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Deleting photo...');

    this.endpoint.deletePhotoEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.load();
        this.alertService.showMessage('Success', 'Photo deleted.', MessageSeverity.success);
      },
      error: (error: unknown) => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Delete error', 'Unable to delete photo.', MessageSeverity.error, error);
      }
    });
  }

  openFullImage(row: AestheticPhoto): void {
    this.fullImageUrl.set(row.url);
    this.fullImageName.set(row.fileName || 'Photo');
  }

  closeFullImage(): void {
    this.fullImageUrl.set(null);
    this.fullImageName.set('');
  }

  onSearch(): void {
    // Search is handled by computed filteredPhotos
  }

  resolvePatientLabel(consultationId: number): string {
    return this.consultationOptions().find(x => x.id === consultationId)?.label ?? `Consultation #${consultationId}`;
  }

  private formatDate(value?: string): string {
    if (!value) return 'No date';
    return value.slice(0, 10);
  }

  private findConsultation(consultationId: number): AestheticConsultation | undefined {
    for (const patient of this.patients()) {
      const match = (patient.consultations || []).find(c => c.id === consultationId);
      if (match) {
        return match;
      }
    }

    return undefined;
  }

  private findPatientByConsultation(consultationId: number): AestheticPatient | undefined {
    return this.patients().find(patient => (patient.consultations || []).some(c => c.id === consultationId));
  }

  private isToday(value?: string): boolean {
    if (!value) {
      return false;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }
}
