import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface ReportPdfDialogData {
  title: string;
  blobUrl: string;
}

@Component({
  selector: 'app-report-pdf-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <div class="pdf-dialog-shell">
      <div class="pdf-dialog-header">
        <div class="pdf-dialog-title">{{ data.title }}</div>
        <button mat-icon-button type="button" aria-label="Close" [mat-dialog-close]="true">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <div class="pdf-dialog-body">
        <iframe [src]="trustedUrl" title="PDF Report" class="pdf-frame"></iframe>
      </div>
    </div>
  `,
  styles: [
    `
      .pdf-dialog-shell { width: min(100vw - 24px, 1200px); height: min(100vh - 24px, 900px); display: flex; flex-direction: column; }
      .pdf-dialog-header { display: flex; align-items: center; justify-content: space-between; gap: 12px; padding: 12px 16px; border-bottom: 1px solid rgba(0,0,0,.12); }
      .pdf-dialog-title { font-size: 16px; font-weight: 600; }
      .pdf-dialog-body { flex: 1; min-height: 0; background: #f5f5f5; }
      .pdf-frame { width: 100%; height: 100%; border: 0; }
    `
  ]
})
export class ReportPdfDialogComponent {
  readonly data = inject<ReportPdfDialogData>(MAT_DIALOG_DATA);
  get trustedUrl(): string { return this.data.blobUrl; }
}
