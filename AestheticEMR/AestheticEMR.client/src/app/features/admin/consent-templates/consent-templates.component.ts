import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';

import { AestheticConsentTemplate } from '../../../models/aesthetic.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';

const DEFAULT_DENTAL_TEMPLATE: AestheticConsentTemplate = {
  id: 0,
  name: 'Dental Consent',
  title: 'Dental Treatment Consent',
  procedureType: 'Dental',
  content: 'DENTAL TREATMENT CONSENT FORM\r\n\r\nTREATMENT DETAILS\r\nPROPOSED TREATMENT: _________________________________________________\r\nDENTIST PERFORMING THE TREATMENT: ____________________________________\r\nDATE: ____________________\r\nDESCRIPTION OF PROPOSED TREATMENTS\r\nTHE DENTAL PROCEDURE(S) MAY INCLUDE BUT NOT LIMITED TO:\r\n\r\nI understand that the nature of the treatment, expected benefits, potential risks, and alternatives to the procedure(s) have been explained to me.\r\nI understand that during the course of treatment, unforeseen conditions may require different procedures or additional treatments.\r\nI acknowledge that the following risks are associated with the proposed treatment(s):\r\n• Pain, discomfort, or swelling\r\n• Prolonged numbness or altered sensation\r\n• Need for further treatments, adjustments, or procedures\r\n• Others: _________________________________________________\r\n\r\nNAME & SIGNATURE: _________________________________________________',
  isActive: true
};

@Component({
  selector: 'app-consent-templates',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule
  ],
  template: `
    <div class="page-shell">
      <div class="page-header">
        <div>
          <h2>Consent Templates</h2>
          <p class="subtitle">Manage per-procedure consent templates used by frontdesk and aesthetics workflows.</p>
        </div>
        <button mat-raised-button color="primary" type="button" (click)="newTemplate()">
          <mat-icon>add</mat-icon>
          New Template
        </button>
      </div>

      <mat-card>
        <div class="content-grid">
          <div class="table-panel">
            <table mat-table [dataSource]="templates()" class="data-table">
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.name }}</td>
              </ng-container>
              <ng-container matColumnDef="procedureType">
                <th mat-header-cell *matHeaderCellDef>Procedure</th>
                <td mat-cell *matCellDef="let row">{{ row.procedureType || 'General' }}</td>
              </ng-container>
              <ng-container matColumnDef="active">
                <th mat-header-cell *matHeaderCellDef>Active</th>
                <td mat-cell *matCellDef="let row">{{ row.isActive ? 'Yes' : 'No' }}</td>
              </ng-container>
              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Actions</th>
                <td mat-cell *matCellDef="let row">
                  <button mat-icon-button type="button" (click)="editTemplate(row)" title="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button type="button" color="warn" (click)="deleteTemplate(row)" title="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns" [class.selected-row]="selectedTemplateId() === row.id"></tr>
            </table>
          </div>

          <div class="editor-panel">
            @if (!isEditorVisible()) {
              <div class="empty-state editor-empty">
                No template selected. Click the edit icon to view or modify a record, or click New Template to start a new one.
              </div>
            }

            <form [formGroup]="form" class="editor-form" [class.hidden-editor]="!isEditorVisible()">
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Name</mat-label>
                <input matInput formControlName="name" />
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Title</mat-label>
                <input matInput formControlName="title" />
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Procedure Type</mat-label>
                <mat-select formControlName="procedureType">
                  <mat-option value="">General</mat-option>
                  <mat-option value="Botox">Botox</mat-option>
                  <mat-option value="Dental">Dental</mat-option>
                  <mat-option value="Laser">Laser</mat-option>
                  <mat-option value="Spa">Spa</mat-option>
                  <mat-option value="Procedures">Procedures</mat-option>
                </mat-select>
              </mat-form-field>

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Consent Content</mat-label>
                <textarea matInput rows="18" formControlName="content"></textarea>
              </mat-form-field>

              <mat-checkbox formControlName="isActive">Active template</mat-checkbox>

              <div class="actions-row">
                <button mat-stroked-button type="button" (click)="newTemplate()">Clear</button>
                <button mat-raised-button color="primary" type="button" (click)="saveTemplate()" [disabled]="loadingIndicator || form.invalid">
                  {{ selectedTemplateId() ? 'Update' : 'Create' }}
                </button>
              </div>
            </form>
          </div>
         </div>
       </mat-card>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; gap: 12px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .content-grid { display: grid; grid-template-columns: minmax(0, 1.2fr) minmax(320px, 0.8fr); gap: 16px; }
    .table-panel { overflow-x: auto; -webkit-overflow-scrolling: touch; }
    .data-table { width: 100%; min-width: 100%; table-layout: fixed; }
    .data-table .mat-column-name,
    .data-table .mat-column-procedureType,
    .data-table .mat-column-active,
    .data-table .mat-column-actions { width: auto; }
    .editor-panel { display: flex; flex-direction: column; gap: 12px; }
    .editor-form { display: flex; flex-direction: column; gap: 12px; }
    .hidden-editor { display: none; }
    .full-width { width: 100%; }
    .actions-row { display: flex; justify-content: flex-end; gap: 12px; }
    .selected-row { background: rgba(25, 118, 210, 0.08); }
    .editor-empty { min-height: 220px; display: flex; align-items: center; justify-content: center; text-align: center; padding: 20px; }

    @media (max-width: 992px) {
      .page-shell { padding: 16px; }
      .page-header { flex-direction: column; align-items: stretch; }
      .page-header button { width: 100%; min-height: 44px; }
      .content-grid { grid-template-columns: 1fr; }
    }

    @media (max-width: 575.98px) {
      .page-shell { padding: 12px; }
      .actions-row { flex-direction: column; }
      .actions-row button { width: 100%; min-height: 44px; }
    }
  `]
})
export class ConsentTemplatesComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  loadingIndicator = false;
  readonly templates = signal<AestheticConsentTemplate[]>([]);
  readonly selectedTemplateId = signal<number | null>(null);
  readonly editorMode = signal<'none' | 'create' | 'edit'>('none');
  readonly displayedColumns = ['name', 'procedureType', 'active', 'actions'];

  readonly form = this.fb.nonNullable.group({
    id: [0],
    name: ['', Validators.required],
    title: ['', Validators.required],
    procedureType: [''],
    content: ['', Validators.required],
    isActive: [true]
  });

  ngOnInit(): void {
    this.loadTemplates();
  }

  loadTemplates(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading consent templates...');
    this.endpoint.getConsentTemplatesEndpoint<AestheticConsentTemplate[]>('', true)
      .subscribe({
        next: templates => {
          const list = templates || [];
          this.templates.set(list);
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();

          if (!list.some(x => (x.procedureType || '').trim().toLowerCase() === 'dental')) {
            void this.ensureDentalTemplateExists();
          }
        },
        error: error => {
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Load Error', 'Unable to retrieve consent templates.', MessageSeverity.error, error);
        }
      });
  }

  private ensureDentalTemplateExists(): void {
    this.endpoint.createConsentTemplateEndpoint<AestheticConsentTemplate>(DEFAULT_DENTAL_TEMPLATE)
      .subscribe({
        next: () => this.loadTemplates(),
        error: error => {
          this.alertService.showStickyMessage('Seed Error', 'Unable to add the default dental consent template.', MessageSeverity.warn, error);
        }
      });
  }

  newTemplate(): void {
    this.selectedTemplateId.set(null);
    this.editorMode.set('create');
    this.form.reset({ id: 0, name: '', title: '', procedureType: '', content: '', isActive: true });
  }

  clearSelection(): void {
    this.selectedTemplateId.set(null);
    this.editorMode.set('none');
    this.form.reset({ id: 0, name: '', title: '', procedureType: '', content: '', isActive: true });
  }

  refreshSelectedTemplate(): void {
    const selected = this.templates().find(x => x.id === this.selectedTemplateId());
    if (selected) {
      this.editTemplate(selected);
    }
  }

  editTemplate(template: AestheticConsentTemplate): void {
    this.selectedTemplateId.set(template.id);
    this.editorMode.set('edit');
    this.form.reset({
      id: template.id,
      name: template.name || '',
      title: template.title || '',
      procedureType: template.procedureType || '',
      content: template.content || '',
      isActive: template.isActive ?? true
    });
  }

  saveTemplate(): void {
    if (this.form.invalid) {
      return;
    }

    const payload = this.form.getRawValue();
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.selectedTemplateId() ? 'Updating consent template...' : 'Creating consent template...');

    const request = this.selectedTemplateId()
      ? this.endpoint.updateConsentTemplateEndpoint<AestheticConsentTemplate>(this.selectedTemplateId()!, payload)
      : this.endpoint.createConsentTemplateEndpoint<AestheticConsentTemplate>(payload);

    request.subscribe({
      next: () => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage('Success', 'Consent template saved successfully.', MessageSeverity.success);
        this.clearSelection();
        this.loadTemplates();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save Error', 'Unable to save consent template.', MessageSeverity.error, error);
      }
    });
  }

  deleteTemplate(template: AestheticConsentTemplate): void {
    this.alertService.showDialog(`Delete consent template "${template.name}"?`, DialogType.confirm, () => {
      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Deleting consent template...');
      this.endpoint.deleteConsentTemplateEndpoint<void>(template.id)
        .subscribe({
          next: () => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showMessage('Deleted', 'Consent template deleted successfully.', MessageSeverity.success);
            if (this.selectedTemplateId() === template.id) {
              this.clearSelection();
            }
            this.loadTemplates();
          },
          error: error => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage('Delete Error', 'Unable to delete consent template.', MessageSeverity.error, error);
          }
        });
    });
  }

  hasSelection(): boolean {
    return this.editorMode() === 'edit';
  }

  isEditorVisible(): boolean {
    return this.editorMode() !== 'none';
  }
}
