import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

interface AuditLog {
  id: number;
  consultationId?: number;
  patientId?: number;
  eventType: string;
  procedureType?: string;
  summary: string;
  details?: string;
  severity: string;
  entityType?: string;
  entityId?: number;
  fieldName?: string;
  oldValue?: string;
  newValue?: string;
  performedBy?: string;
  eventDateTime: Date | string;
  sourceIp?: string;
  tags?: string;
  status: string;
  reviewedBy?: string;
  reviewedDate?: Date;
  resolutionNotes?: string;
}

@Component({
  selector: 'app-audit-trail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatChipsModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatTooltipModule,
    MatDialogModule
  ],
  template: `
    <div class="audit-trail-page">
      <div class="page-header">
        <h2>Audit Trail & Incident Management</h2>
        <p class="subtitle">Track all changes, complications, and safety events</p>
      </div>

      <mat-tab-group>
        <!-- Open Incidents Tab -->
        <mat-tab label="Open Incidents">
          <div class="tab-content">
            <button mat-raised-button color="warn" (click)="loadOpenIncidents()" class="action-button">
              <mat-icon>refresh</mat-icon>
              Refresh
            </button>

            @if (loadingIndicator()) {
              <mat-spinner diameter="40"></mat-spinner>
            } @else if (openIncidents().length === 0) {
              <p class="no-data">No open incidents</p>
            } @else {
              <div class="table-container">
                <table mat-table [dataSource]="openIncidents()" class="audit-table">
                  <ng-container matColumnDef="eventType">
                    <th mat-header-cell>Event Type</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="severity">
                    <th mat-header-cell>Severity</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="summary">
                    <th mat-header-cell>Summary</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell>Date/Time</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="performedBy">
                    <th mat-header-cell>Reported By</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell>Actions</th>
                    <td mat-cell></td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                  <tr mat-row *matRowDef="let item; columns: displayedColumns;"></tr>
                </table>
              </div>
            }
          </div>
        </mat-tab>

        <!-- All Incidents Tab -->
        <mat-tab label="All Incidents">
          <div class="tab-content">
            <div class="filter-row">
              <mat-form-field appearance="outline">
                <mat-label>Severity</mat-label>
                <mat-select [(ngModel)]="filterSeverity">
                  <mat-option value="">All</mat-option>
                  <mat-option value="Info">Info</mat-option>
                  <mat-option value="Warning">Warning</mat-option>
                  <mat-option value="Error">Error</mat-option>
                  <mat-option value="Critical">Critical</mat-option>
                </mat-select>
              </mat-form-field>

              <button mat-raised-button color="primary" (click)="applyFilters()">
                <mat-icon>search</mat-icon>
                Search
              </button>
            </div>

            @if (loadingIndicator()) {
              <mat-spinner diameter="40"></mat-spinner>
            } @else if (allIncidents().length === 0) {
              <p class="no-data">No incidents found</p>
            } @else {
              <div class="table-container">
                <table mat-table [dataSource]="allIncidents()" class="audit-table">
                  <ng-container matColumnDef="eventType">
                    <th mat-header-cell>Event Type</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="severity">
                    <th mat-header-cell>Severity</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="summary">
                    <th mat-header-cell>Summary</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="status">
                    <th mat-header-cell>Status</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell>Date/Time</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell>Actions</th>
                    <td mat-cell></td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="displayedColumnsAll"></tr>
                  <tr mat-row *matRowDef="let item; columns: displayedColumnsAll;"></tr>
                </table>
              </div>
            }
          </div>
        </mat-tab>

        <!-- Consultation Trail Tab -->
        <mat-tab label="Consultation Trail">
          <div class="tab-content">
            <div class="filter-row">
              <mat-form-field appearance="outline">
                <mat-label>Consultation ID</mat-label>
                <input matInput type="number" [(ngModel)]="consultationIdFilter">
              </mat-form-field>

              <button mat-raised-button color="primary" (click)="loadConsultationTrail()">
                <mat-icon>search</mat-icon>
                Load
              </button>
            </div>

            @if (loadingIndicator()) {
              <mat-spinner diameter="40"></mat-spinner>
            } @else if (consultationTrail().length === 0) {
              <p class="no-data">No audit trail</p>
            } @else {
              <div class="table-container">
                <table mat-table [dataSource]="consultationTrail()" class="audit-table">
                  <ng-container matColumnDef="eventType">
                    <th mat-header-cell>Event Type</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="fieldName">
                    <th mat-header-cell>Field</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="oldValue">
                    <th mat-header-cell>Old Value</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="newValue">
                    <th mat-header-cell>New Value</th>
                    <td mat-cell></td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell>Date/Time</th>
                    <td mat-cell></td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="displayedColumnsConsultation"></tr>
                  <tr mat-row *matRowDef="let item; columns: displayedColumnsConsultation;"></tr>
                </table>
              </div>
            }
          </div>
        </mat-tab>
      </mat-tab-group>
    </div>
  `,
  styles: [`
    .audit-trail-page { padding: 20px; }
    .page-header { margin-bottom: 20px; }
    .subtitle { color: #666; margin-top: 4px; }
    .tab-content { padding: 16px; }
    .action-button { margin-bottom: 16px; }
    .filter-row { display: flex; gap: 12px; align-items: flex-end; margin-bottom: 16px; }
    .table-container { overflow-x: auto; margin: 16px 0; }
    .audit-table { width: 100%; }
    .no-data { text-align: center; padding: 40px; color: #999; }
  `]
})
export class AuditTrailComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly alertService = inject(AlertService);

  readonly loadingIndicator = signal(false);
  readonly openIncidents = signal<AuditLog[]>([]);
  readonly allIncidents = signal<AuditLog[]>([]);
  readonly consultationTrail = signal<AuditLog[]>([]);

  readonly filterSeverity = signal('');
  readonly consultationIdFilter = signal(0);

  readonly displayedColumns = ['eventType', 'severity', 'summary', 'eventDateTime', 'performedBy', 'actions'];
  readonly displayedColumnsAll = ['eventType', 'severity', 'summary', 'status', 'eventDateTime', 'actions'];
  readonly displayedColumnsConsultation = ['eventType', 'fieldName', 'oldValue', 'newValue', 'eventDateTime'];

  ngOnInit(): void {
    this.loadOpenIncidents();
  }

  loadOpenIncidents(): void {
    this.loadingIndicator.set(true);
    this.http.get<AuditLog[]>('api/audit/incidents/open').subscribe({
      next: incidents => {
        this.openIncidents.set(incidents || []);
        this.loadingIndicator.set(false);
      },
      error: () => {
        this.loadingIndicator.set(false);
        this.alertService.showStickyMessage('Error', 'Unable to load incidents', MessageSeverity.error);
      }
    });
  }

  applyFilters(): void {
    if (!this.filterSeverity()) {
      this.alertService.showStickyMessage('Validation', 'Please select a severity level', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator.set(true);
    const params = { severity: this.filterSeverity() };

    this.http.get<AuditLog[]>('api/audit/incidents', { params }).subscribe({
      next: incidents => {
        this.allIncidents.set(incidents || []);
        this.loadingIndicator.set(false);
      },
      error: () => {
        this.loadingIndicator.set(false);
        this.alertService.showStickyMessage('Error', 'Unable to load incidents', MessageSeverity.error);
      }
    });
  }

  loadConsultationTrail(): void {
    const id = this.consultationIdFilter();
    if (id <= 0) {
      this.alertService.showStickyMessage('Validation', 'Please enter a valid consultation ID', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator.set(true);
    this.http.get<AuditLog[]>(`api/audit/consultation/${id}`).subscribe({
      next: trail => {
        this.consultationTrail.set(trail || []);
        this.loadingIndicator.set(false);
      },
      error: () => {
        this.loadingIndicator.set(false);
        this.alertService.showStickyMessage('Error', 'Unable to load audit trail', MessageSeverity.error);
      }
    });
  }

  reviewIncident(incident: AuditLog): void {
    const notes = prompt('Enter resolution notes:');
    if (notes) {
      this.http.put(`api/audit/${incident.id}/review`, { resolutionNotes: notes }).subscribe({
        next: () => {
          this.alertService.showMessage('Success', 'Incident marked as reviewed', MessageSeverity.success);
          this.loadOpenIncidents();
        },
        error: () => {
          this.alertService.showStickyMessage('Error', 'Unable to update incident', MessageSeverity.error);
        }
      });
    }
  }

  viewDetails(incident: AuditLog): void {
    const details = `
      Event Type: ${incident.eventType}
      Severity: ${incident.severity}
      Summary: ${incident.summary}
      Details: ${incident.details || 'N/A'}
      Performed By: ${incident.performedBy || 'N/A'}
      Date: ${incident.eventDateTime}
    `;
    alert(details);
  }
}
