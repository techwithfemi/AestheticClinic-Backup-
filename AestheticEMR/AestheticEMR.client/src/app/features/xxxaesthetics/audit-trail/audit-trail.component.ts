import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';

interface AuditLog {
  id: number;
  tranCode: string;
  eventType: string;
  summary: string;
  details?: string;
  severity: string;
  entityType?: string;
  entityId?: number;
  fieldName?: string;
  oldValue?: string;
  newValue?: string;
  userId?: string;
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
                    <th mat-header-cell *matHeaderCellDef>Event Type</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventType }}</td>
                  </ng-container>

                  <ng-container matColumnDef="severity">
                    <th mat-header-cell *matHeaderCellDef>Severity</th>
                    <td mat-cell *matCellDef="let item">{{ item.severity }}</td>
                  </ng-container>

                  <ng-container matColumnDef="summary">
                    <th mat-header-cell *matHeaderCellDef>Summary</th>
                    <td mat-cell *matCellDef="let item">{{ item.summary }}</td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell *matHeaderCellDef>Date/Time</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventDateTime | date:'dd-MMM-yyyy HH:mm' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="performedBy">
                    <th mat-header-cell *matHeaderCellDef>Reported By</th>
                    <td mat-cell *matCellDef="let item">{{ item.performedBy || 'System' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="userId">
                    <th mat-header-cell *matHeaderCellDef>User ID</th>
                    <td mat-cell *matCellDef="let item">{{ item.userId || item.performedBy || '-' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef>Actions</th>
                    <td mat-cell *matCellDef="let item">
                      <button mat-icon-button color="primary" (click)="viewDetails(item)" matTooltip="View details">
                        <mat-icon>visibility</mat-icon>
                      </button>
                      @if (item.status === 'Open') {
                        <button mat-icon-button color="accent" (click)="reviewIncident(item)" matTooltip="Mark reviewed">
                          <mat-icon>done</mat-icon>
                        </button>
                      }
                    </td>
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
                    <th mat-header-cell *matHeaderCellDef>Event Type</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventType }}</td>
                  </ng-container>

                  <ng-container matColumnDef="severity">
                    <th mat-header-cell *matHeaderCellDef>Severity</th>
                    <td mat-cell *matCellDef="let item">{{ item.severity }}</td>
                  </ng-container>

                  <ng-container matColumnDef="summary">
                    <th mat-header-cell *matHeaderCellDef>Summary</th>
                    <td mat-cell *matCellDef="let item">{{ item.summary }}</td>
                  </ng-container>

                  <ng-container matColumnDef="status">
                    <th mat-header-cell *matHeaderCellDef>Status</th>
                    <td mat-cell *matCellDef="let item">{{ item.status }}</td>
                  </ng-container>

                  <ng-container matColumnDef="userId">
                    <th mat-header-cell *matHeaderCellDef>User ID</th>
                    <td mat-cell *matCellDef="let item">{{ item.userId || item.performedBy || '-' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell *matHeaderCellDef>Date/Time</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventDateTime | date:'dd-MMM-yyyy HH:mm' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef>Actions</th>
                    <td mat-cell *matCellDef="let item">
                      <button mat-icon-button color="primary" (click)="viewDetails(item)" matTooltip="View details">
                        <mat-icon>visibility</mat-icon>
                      </button>
                    </td>
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
                    <th mat-header-cell *matHeaderCellDef>Event Type</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventType }}</td>
                  </ng-container>

                  <ng-container matColumnDef="fieldName">
                    <th mat-header-cell *matHeaderCellDef>Field</th>
                    <td mat-cell *matCellDef="let item">{{ item.fieldName || '-' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="oldValue">
                    <th mat-header-cell *matHeaderCellDef>Old Value</th>
                    <td mat-cell *matCellDef="let item">{{ item.oldValue || '(empty)' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="newValue">
                    <th mat-header-cell *matHeaderCellDef>New Value</th>
                    <td mat-cell *matCellDef="let item">{{ item.newValue || '(empty)' }}</td>
                  </ng-container>

                  <ng-container matColumnDef="eventDateTime">
                    <th mat-header-cell *matHeaderCellDef>Date/Time</th>
                    <td mat-cell *matCellDef="let item">{{ item.eventDateTime | date:'dd-MMM-yyyy HH:mm' }}</td>
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
    .action-button { margin-bottom: 16px; min-height: 44px; }
    .filter-row { display: flex; gap: 12px; align-items: flex-end; margin-bottom: 16px; flex-wrap: wrap; }
    .table-container { overflow-x: auto; margin: 16px 0; -webkit-overflow-scrolling: touch; }
    .audit-table { width: 100%; min-width: 680px; }
    .no-data { text-align: center; padding: 40px; color: #999; }

    @media (max-width: 992px) {
      .audit-trail-page { padding: 16px; }
      .tab-content { padding: 12px; }
      .filter-row { align-items: stretch; }
    }

    @media (max-width: 575.98px) {
      .audit-trail-page { padding: 12px; }
      .action-button,
      .filter-row button,
      .filter-row .mat-mdc-form-field {
        width: 100%;
      }
    }
  `]
})
export class AuditTrailComponent implements OnInit {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);

  readonly loadingIndicator = signal(false);
  readonly openIncidents = signal<AuditLog[]>([]);
  readonly allIncidents = signal<AuditLog[]>([]);
  readonly consultationTrail = signal<AuditLog[]>([]);

  readonly filterSeverity = signal('');
  readonly consultationIdFilter = signal(0);

  readonly displayedColumns = ['eventType', 'severity', 'summary', 'eventDateTime', 'performedBy', 'userId', 'actions'];
  readonly displayedColumnsAll = ['eventType', 'severity', 'summary', 'status', 'userId', 'eventDateTime', 'actions'];
  readonly displayedColumnsConsultation = ['eventType', 'fieldName', 'oldValue', 'newValue', 'eventDateTime'];

  ngOnInit(): void {
    this.loadOpenIncidents();
  }

  loadOpenIncidents(): void {
    this.loadingIndicator.set(true);
    this.endpoint.getOpenAuditIncidentsEndpoint<AuditLog[]>().subscribe({
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

    const now = new Date();
    const from = new Date(now);
    from.setMonth(from.getMonth() - 1);

    const fromDate = from.toISOString();
    const toDate = now.toISOString();

    this.endpoint.getAuditIncidentsEndpoint<AuditLog[]>(this.filterSeverity(), fromDate, toDate).subscribe({
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
    this.endpoint.getConsultationAuditTrailEndpoint<AuditLog[]>(id).subscribe({
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
      this.endpoint.reviewAuditIncidentEndpoint(incident.id, { resolutionNotes: notes }).subscribe({
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
      User ID: ${incident.userId || 'N/A'}
      Performed By: ${incident.performedBy || 'N/A'}
      Date: ${incident.eventDateTime}
    `;
    alert(details);
  }
}
