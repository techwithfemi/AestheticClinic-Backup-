import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { AestheticConsentTemplate } from '../../../models/aesthetic.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';

@Component({
  selector: 'app-consent-template-manager',
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
    MatSelectModule
  ],
  template: `
    <mat-card class="template-card">
      <div class="template-header">
        <div>
          <h3>Consent Template Management</h3>
          <p class="subtitle">Dynamic template CRUD for frontdesk and clinic staff. Dental templates can be added, edited, or removed here.</p>
        </div>
        <div class="template-header-actions">
          <button mat-stroked-button type="button" (click)="refreshRequested.emit()" [disabled]="loadingIndicator">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
          <button mat-raised-button color="primary" type="button" (click)="newTemplate()">
            <mat-icon>add</mat-icon>
            New Template
          </button>
        </div>
      </div>

      <div class="template-grid">
        <section>
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Filter Templates</mat-label>
            <input matInput [value]="templateSearchText()" (input)="templateSearchText.set(($any($event.target).value || '').trim())" placeholder="Search by name, title, or procedure" />
          </mat-form-field>

          <div class="template-list">
            @for (template of filteredTemplates(); track template.id) {
              <button type="button" class="template-item" [class.active]="editingTemplateId() === template.id" (click)="editTemplate(template)">
                <div class="template-item-title">{{ template.title || template.name }}</div>
                <div class="template-item-meta">
                  <span>{{ template.procedureType || 'General' }}</span>
                  <span>{{ template.isActive ? 'Active' : 'Inactive' }}</span>
                </div>
              </button>
            }

            @if (!filteredTemplates().length) {
              <div class="empty-state">No templates match the current filter.</div>
            }
          </div>
        </section>

        <section>
          <form [formGroup]="templateForm" class="editor-panel">
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
              <textarea matInput rows="16" formControlName="content"></textarea>
            </mat-form-field>

            <mat-checkbox formControlName="isActive">Active template</mat-checkbox>

            <div class="actions-row template-actions">
              <button mat-stroked-button type="button" (click)="deleteTemplate()" [disabled]="loadingIndicator || !editingTemplateId()">
                <mat-icon>delete</mat-icon>
                Delete
              </button>
              <button mat-stroked-button type="button" (click)="newTemplate()">Clear</button>
              <button mat-raised-button color="primary" type="button" (click)="saveTemplate()" [disabled]="loadingIndicator || templateForm.invalid">
                {{ editingTemplateId() ? 'Update' : 'Create' }}
              </button>
            </div>

            <mat-card class="preview-card" appearance="outlined">
              <strong>Live Preview</strong>
              <p class="subtitle">{{ previewSubtitle }}</p>
              <div class="consent-box preview-box">{{ templateForm.controls.content.value || 'Template content preview will appear here.' }}</div>
            </mat-card>
          </form>
        </section>
      </div>
    </mat-card>
  `,
  styles: [`
    .template-card { margin-top: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .template-grid { display: grid; grid-template-columns: minmax(0, 0.8fr) minmax(0, 1.2fr); gap: 16px; }
    .full-width { width: 100%; }
    .form-stack, .editor-panel { display: flex; flex-direction: column; gap: 12px; }
    .actions-row { display: flex; justify-content: flex-end; gap: 12px; flex-wrap: wrap; }
    .template-header { display: flex; justify-content: space-between; align-items: flex-start; gap: 12px; margin-bottom: 16px; }
    .template-header-actions { display: flex; gap: 10px; flex-wrap: wrap; }
    .template-list { display: flex; flex-direction: column; gap: 10px; max-height: 520px; overflow: auto; padding-right: 4px; }
    .template-item {
      display: block;
      width: 100%;
      text-align: left;
      border: 1px solid #d7dce3;
      border-radius: 10px;
      padding: 12px;
      background: #fff;
      cursor: pointer;
    }
    .template-item.active { border-color: #1976d2; background: rgba(25, 118, 210, 0.06); }
    .template-item-title { font-weight: 600; margin-bottom: 4px; }
    .template-item-meta { display: flex; justify-content: space-between; gap: 8px; color: #68707f; font-size: .85rem; }
    .empty-state { color: #666; font-style: italic; padding: 12px 0; }
    .preview-card { margin-top: 4px; }
    .consent-box { white-space: pre-wrap; border: 1px solid #ddd; border-radius: 8px; padding: 12px; min-height: 140px; background: #fafafa; margin-top: 10px; }
    @media (max-width: 992px) {
      .template-grid { grid-template-columns: 1fr; }
      .template-header { flex-direction: column; }
    }
    @media (max-width: 575.98px) {
      .template-header-actions { width: 100%; }
      .template-header-actions button { width: 100%; min-height: 44px; }
      .actions-row { flex-direction: column; }
      .actions-row button { width: 100%; min-height: 44px; }
    }
  `]
})
export class ConsentTemplateManagerComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  @Input() templates: AestheticConsentTemplate[] = [];
  @Input() selectedProcedureType = '';
  @Output() refreshRequested = new EventEmitter<void>();

  loadingIndicator = false;
  readonly editingTemplateId = signal<number | null>(null);
  readonly templateSearchText = signal<string>('');
  readonly selectedTemplateId = signal<number | null>(null);

  readonly templateForm = this.fb.nonNullable.group({
    id: [0],
    name: ['', Validators.required],
    title: ['', Validators.required],
    procedureType: [''],
    content: ['', Validators.required],
    isActive: [true]
  });

  get previewSubtitle(): string {
    const procedureType = (this.templateForm.controls.procedureType.value || '').trim();
    return procedureType ? `Previewing ${procedureType} consent text.` : 'Previewing a general consent template.';
  }

  filteredTemplates(): AestheticConsentTemplate[] {
    const term = this.templateSearchText().toLowerCase();
    const procedure = this.selectedProcedureType.toLowerCase();

    return (this.templates || [])
      .filter(template => !term || `${template.name} ${template.title} ${template.procedureType}`.toLowerCase().includes(term))
      .filter(template => !procedure || !template.procedureType || template.procedureType.toLowerCase() === procedure || procedure === 'procedures')
      .sort((a, b) => `${a.procedureType || ''} ${a.title || a.name || ''}`.localeCompare(`${b.procedureType || ''} ${b.title || b.name || ''}`));
  }

  newTemplate(): void {
    this.editingTemplateId.set(null);
    this.selectedTemplateId.set(null);
    this.templateForm.reset({
      id: 0,
      name: '',
      title: '',
      procedureType: this.selectedProcedureType,
      content: '',
      isActive: true
    });
  }

  editTemplate(template: AestheticConsentTemplate): void {
    this.editingTemplateId.set(template.id);
    this.selectedTemplateId.set(template.id);
    this.templateForm.reset({
      id: template.id,
      name: template.name || '',
      title: template.title || '',
      procedureType: template.procedureType || '',
      content: template.content || '',
      isActive: template.isActive ?? true
    });
  }

  saveTemplate(): void {
    if (this.templateForm.invalid) {
      return;
    }

    const payload = this.templateForm.getRawValue();
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.editingTemplateId() ? 'Updating consent template...' : 'Creating consent template...');

    const request = this.editingTemplateId()
      ? this.endpoint.updateConsentTemplateEndpoint<AestheticConsentTemplate>(this.editingTemplateId()!, payload)
      : this.endpoint.createConsentTemplateEndpoint<AestheticConsentTemplate>(payload);

    request.subscribe({
      next: createdOrUpdated => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage('Success', 'Consent template saved successfully.', MessageSeverity.success);
        this.editingTemplateId.set(createdOrUpdated.id);
        this.selectedTemplateId.set(createdOrUpdated.id);
        this.templateForm.patchValue(createdOrUpdated);
        this.refreshRequested.emit();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save Error', 'Unable to save consent template.', MessageSeverity.error, error);
      }
    });
  }

  deleteTemplate(): void {
    const templateId = this.editingTemplateId();
    if (!templateId) {
      return;
    }

    const template = this.templates.find(x => x.id === templateId);
    const templateName = template?.title || template?.name || 'this template';

    this.alertService.showDialog(`Delete consent template "${templateName}"?`, DialogType.confirm, () => {
      this.loadingIndicator = true;
      this.alertService.startLoadingMessage('Deleting consent template...');
      this.endpoint.deleteConsentTemplateEndpoint<void>(templateId)
        .subscribe({
          next: () => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showMessage('Deleted', 'Consent template deleted successfully.', MessageSeverity.success);
            this.newTemplate();
            this.refreshRequested.emit();
          },
          error: error => {
            this.loadingIndicator = false;
            this.alertService.stopLoadingMessage();
            this.alertService.showStickyMessage('Delete Error', 'Unable to delete consent template.', MessageSeverity.error, error);
          }
        });
    });
  }

  private syncAfterRefresh(): void {
    if (!this.editingTemplateId()) {
      return;
    }

    const existing = this.templates.find(x => x.id === this.editingTemplateId());
    if (existing) {
      this.templateForm.patchValue(existing);
    }
  }
}
