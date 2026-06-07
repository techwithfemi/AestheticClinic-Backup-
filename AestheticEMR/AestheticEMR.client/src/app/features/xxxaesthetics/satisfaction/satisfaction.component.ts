import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import {
  AestheticFollowUp,
  PublicPatientSatisfactionSurvey,
  SubmitPatientSatisfaction
} from '../../../models/aesthetic.model';

@Component({
  selector: 'app-aesthetic-satisfaction',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="satisfaction-page">
      <div class="satisfaction-card" *ngIf="!loadingIndicator; else loadingTemplate">
        <h2>Patient Satisfaction</h2>
        <p class="subtitle">Please rate your treatment satisfaction from 1 to 10.</p>

        <div *ngIf="errorMessage" class="error-box">{{ errorMessage }}</div>

        <div *ngIf="survey && !submitted; else submittedTemplate">
          <div class="context-row"><strong>Patient:</strong> {{ survey.patientName || 'Patient' }}</div>
          <div class="context-row"><strong>Consult ID:</strong> {{ survey.consultId || '-' }}</div>
          <div class="context-row"><strong>PNo:</strong> {{ survey.pNo || '-' }}</div>

          <div class="score-section">
            <label for="scoreRange">Score: <strong>{{ score }}</strong>/10</label>
            <input id="scoreRange" type="range" min="1" max="10" [(ngModel)]="score" />
            <div class="score-scale">
              <span>1 (Very dissatisfied)</span>
              <span>10 (Very satisfied)</span>
            </div>
          </div>

          <div class="form-group">
            <label for="outcome">Optional feedback</label>
            <textarea id="outcome" rows="4" [(ngModel)]="outcome" placeholder="Share your treatment feedback..."></textarea>
          </div>

          <div class="actions">
            <button type="button" class="btn-primary" (click)="submit()" [disabled]="submitting">Done</button>
          </div>
        </div>
      </div>
    </div>

    <ng-template #loadingTemplate>
      <div class="satisfaction-page">
        <div class="satisfaction-card">
          <h2>Patient Satisfaction</h2>
          <p>Loading survey...</p>
        </div>
      </div>
    </ng-template>

    <ng-template #submittedTemplate>
      <div *ngIf="submitted" class="success-box">
        <h3>Thank you</h3>
        <p>Your satisfaction score has been submitted successfully.</p>
      </div>
    </ng-template>
  `,
  styles: [`
    .satisfaction-page {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 24px;
      background: #f6f8fb;
    }

    .satisfaction-card {
      width: min(560px, 100%);
      background: #fff;
      border-radius: 12px;
      box-shadow: 0 12px 30px rgba(0, 0, 0, .08);
      padding: 24px;
    }

    h2 { margin: 0; }
    .subtitle { color: #666; margin: 8px 0 16px; }

    .context-row {
      margin-bottom: 8px;
      font-size: 0.95rem;
    }

    .score-section {
      margin: 16px 0;
      display: grid;
      gap: 8px;
    }

    .score-section input[type='range'] {
      width: 100%;
    }

    .score-scale {
      display: flex;
      justify-content: space-between;
      font-size: .85rem;
      color: #555;
    }

    .form-group {
      display: grid;
      gap: 6px;
      margin-top: 12px;
    }

    textarea {
      width: 100%;
      border: 1px solid #d2d7df;
      border-radius: 8px;
      padding: 10px;
      resize: vertical;
      font-family: inherit;
    }

    .actions {
      display: flex;
      justify-content: flex-end;
      margin-top: 16px;
    }

    .btn-primary {
      border: none;
      background: #1976d2;
      color: #fff;
      border-radius: 8px;
      padding: 10px 18px;
      min-height: 44px;
      cursor: pointer;
    }

    .btn-primary:disabled {
      opacity: .65;
      cursor: not-allowed;
    }

    .error-box {
      background: #fdeaea;
      border: 1px solid #f3c5c5;
      color: #a32525;
      border-radius: 8px;
      padding: 10px;
      margin-bottom: 12px;
    }

    .success-box {
      margin-top: 16px;
      padding: 12px;
      border-radius: 8px;
      background: #eaf8ef;
      border: 1px solid #b8e5c8;
      color: #1c6d3e;
    }

    @media (max-width: 575.98px) {
      .satisfaction-page {
        padding: 12px;
      }

      .satisfaction-card {
        padding: 16px;
      }

      .actions {
        justify-content: stretch;
      }

      .btn-primary {
        width: 100%;
      }
    }
  `]
})
export class SatisfactionComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);

  token = '';
  loadingIndicator = true;
  submitting = false;
  submitted = false;
  errorMessage = '';

  survey?: PublicPatientSatisfactionSurvey;
  score = 8;
  outcome = '';

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token') || '';
    if (!this.token) {
      this.errorMessage = 'Missing satisfaction token.';
      this.loadingIndicator = false;
      return;
    }

    this.loadSurvey();
  }

  private loadSurvey(): void {
    this.alertService.startLoadingMessage('Loading patient satisfaction survey...');
    this.endpoint.getPatientSatisfactionSurveyEndpoint<PublicPatientSatisfactionSurvey>(this.token)
      .subscribe({
        next: survey => {
          this.alertService.stopLoadingMessage();
          this.survey = survey;
          this.loadingIndicator = false;
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.errorMessage = 'This satisfaction link is invalid or expired.';
          this.alertService.showStickyMessage('Survey unavailable', this.errorMessage, MessageSeverity.error, error);
        }
      });
  }

  submit(): void {
    if (this.submitting || !this.token) {
      return;
    }

    if (this.score < 1 || this.score > 10) {
      this.alertService.showStickyMessage('Validation error', 'Score must be between 1 and 10.', MessageSeverity.warn);
      return;
    }

    const payload: SubmitPatientSatisfaction = {
      patientSatisfactionScore: this.score,
      outcome: this.outcome?.trim() || undefined
    };

    this.submitting = true;
    this.alertService.startLoadingMessage('Submitting satisfaction...');

    this.endpoint.submitPatientSatisfactionEndpoint<AestheticFollowUp>(this.token, payload)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.submitting = false;
          this.submitted = true;
          this.alertService.showMessage('Thank you', 'Your satisfaction score was submitted.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.submitting = false;
          this.alertService.showStickyMessage('Submit failed', 'Unable to submit your score.', MessageSeverity.error, error);
        }
      });
  }
}
