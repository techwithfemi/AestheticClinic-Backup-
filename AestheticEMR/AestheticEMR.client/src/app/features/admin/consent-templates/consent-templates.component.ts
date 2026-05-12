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

          <form [formGroup]="form" class="editor-panel">
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
                <mat-option value="Laser">Laser</mat-option>
                <mat-option value="Spa">Spa</mat-option>
                <mat-option value="Procedures">Procedures</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Consent Content</mat-label>
              <textarea matInput rows="12" formControlName="content"></textarea>
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
      </mat-card>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .content-grid { display: grid; grid-template-columns: minmax(0, 1.2fr) minmax(320px, 0.8fr); gap: 16px; }
    .data-table { width: 100%; }
    .editor-panel { display: flex; flex-direction: column; gap: 12px; }
    .full-width { width: 100%; }
    .actions-row { display: flex; justify-content: flex-end; gap: 12px; }
    .selected-row { background: rgba(25, 118, 210, 0.08); }
    @media (max-width: 992px) { .content-grid { grid-template-columns: 1fr; } }
  `]
})
export class ConsentTemplatesComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly fb = inject(FormBuilder);

  loadingIndicator = false;
  readonly templates = signal<AestheticConsentTemplate[]>([]);
  readonly selectedTemplateId = signal<number | null>(null);
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
          this.templates.set(templates || []);
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();
        },
        error: error => {
          this.loadingIndicator = false;
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Load Error', 'Unable to retrieve consent templates.', MessageSeverity.error, error);
        }
      });
  }

  newTemplate(): void {
    this.selectedTemplateId.set(null);
    this.form.reset({ id: 0, name: '', title: '', procedureType: '', content: '', isActive: true });
  }

  editTemplate(template: AestheticConsentTemplate): void {
    this.selectedTemplateId.set(template.id);
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
        this.newTemplate();
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
              this.newTemplate();
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
}
