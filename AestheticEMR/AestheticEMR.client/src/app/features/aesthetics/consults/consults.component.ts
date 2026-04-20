import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticPatient } from '../../../models/aesthetic.model';

@Component({
  selector: 'app-consults',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="consults-container">
      <section class="section-header">
        <h2>Aesthetics Consultations</h2>
        <p>Capture patient intake, consult history and treatment planning for medical spa workflows.</p>
      </section>

      <section class="grid-layout">
        <div class="panel patient-form">
          <h3>New Patient Intake</h3>
          <div class="form-group">
            <label>First Name</label>
            <input type="text" [(ngModel)]="newPatient.firstName" name="firstName" placeholder="First name" />
          </div>
          <div class="form-group">
            <label>Last Name</label>
            <input type="text" [(ngModel)]="newPatient.lastName" name="lastName" placeholder="Last name" />
          </div>
          <div class="form-group">
            <label>Email</label>
            <input type="email" [(ngModel)]="newPatient.email" name="email" placeholder="Email address" />
          </div>
          <div class="form-group">
            <label>Phone</label>
            <input type="text" [(ngModel)]="newPatient.phoneNumber" name="phoneNumber" placeholder="Phone number" />
          </div>
          <div class="form-group">
            <label>Skin Type</label>
            <input type="text" [(ngModel)]="newPatient.skinType" name="skinType" placeholder="Skin type" />
          </div>
          <div class="form-group">
            <label>Medical History</label>
            <textarea rows="4" [(ngModel)]="newPatient.medicalHistory" name="medicalHistory" placeholder="Medical history..."></textarea>
          </div>
          <button type="button" (click)="createPatient()" [disabled]="loadingIndicator">Save Patient</button>
        </div>

        <div class="panel patient-list">
          <h3>Known Patients</h3>
          <div *ngIf="patients.length === 0" class="empty-state">No patients have been registered yet.</div>
          <ul *ngIf="patients.length > 0">
            <li *ngFor="let patient of patients" (click)="selectPatient(patient)" [class.selected]="selectedPatient?.id === patient.id">
              <div class="patient-name">{{ patient.firstName }} {{ patient.lastName }}</div>
              <div class="patient-meta">{{ patient.email || 'No email' }} · {{ patient.phoneNumber || 'No phone' }}</div>
            </li>
          </ul>
        </div>
      </section>

      <section *ngIf="selectedPatient" class="panel selected-details">
        <h3>Selected Patient</h3>
        <div class="detail-row"><strong>Name:</strong> {{ selectedPatient.firstName }} {{ selectedPatient.lastName }}</div>
        <div class="detail-row"><strong>Email:</strong> {{ selectedPatient.email || '—' }}</div>
        <div class="detail-row"><strong>Phone:</strong> {{ selectedPatient.phoneNumber || '—' }}</div>
        <div class="detail-row"><strong>Skin Type:</strong> {{ selectedPatient.skinType || '—' }}</div>
        <div class="detail-row"><strong>Medical History:</strong> {{ selectedPatient.medicalHistory || 'No medical history recorded' }}</div>
      </section>
    </div>
  `,
  styles: [
    `
      .consults-container {
        padding: 20px;
      }

      .section-header {
        margin-bottom: 18px;
      }

      .grid-layout {
        display: grid;
        grid-template-columns: 1.2fr 1fr;
        gap: 18px;
      }

      .panel {
        background: #ffffff;
        border-radius: 12px;
        box-shadow: 0 8px 24px rgba(0, 0, 0, 0.07);
        padding: 18px;
      }

      .form-group {
        display: grid;
        gap: 6px;
        margin-bottom: 16px;
      }

      input,
      textarea {
        width: 100%;
        padding: 10px 12px;
        border: 1px solid #d8d8d8;
        border-radius: 8px;
        font-size: 0.95rem;
      }

      button {
        background-color: #0069d9;
        border: none;
        color: white;
        padding: 10px 18px;
        border-radius: 8px;
        cursor: pointer;
      }

      button:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }

      .patient-list ul {
        list-style: none;
        margin: 0;
        padding: 0;
      }

      .patient-list li {
        border: 1px solid #ececec;
        border-radius: 10px;
        padding: 14px;
        margin-bottom: 10px;
        cursor: pointer;
        transition: transform 0.16s ease, box-shadow 0.16s ease;
      }

      .patient-list li:hover,
      .patient-list li.selected {
        transform: translateY(-1px);
        box-shadow: 0 12px 24px rgba(0, 0, 0, 0.08);
      }

      .patient-name {
        font-weight: 600;
        margin-bottom: 4px;
      }

      .patient-meta {
        color: #666;
        font-size: 0.9rem;
      }

      .selected-details {
        margin-top: 22px;
      }

      .detail-row {
        margin-bottom: 10px;
      }
    `
  ]
})
export class ConsultsComponent {
  private readonly alertService = inject(AlertService);
  private readonly endpoint = inject(AestheticEndpoint);

  loadingIndicator = false;
  patients: AestheticPatient[] = [];
  selectedPatient?: AestheticPatient;
  newPatient: AestheticPatient = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: '',
    gender: '',
    skinType: '',
    allergies: '',
    medicalHistory: '',
    currentMedications: '',
    notes: ''
  };

  constructor() {
    this.loadPatients();
  }

  private loadPatients(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading aesthetic patients...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients = patients;
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Unable to load patients', 'Please try again or contact support.', MessageSeverity.error, error);
      }
    });
  }

  createPatient(): void {
    if (!this.newPatient.firstName?.trim() || !this.newPatient.lastName?.trim()) {
      this.alertService.showStickyMessage('Validation error', 'Patient first name and last name are required.', MessageSeverity.warn);
      return;
    }

    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Saving patient...');

    this.endpoint.createPatientEndpoint<AestheticPatient>(this.newPatient).subscribe({
      next: patient => {
        this.patients.unshift(patient);
        this.selectedPatient = patient;
        this.newPatient = {
          id: 0,
          firstName: '',
          lastName: '',
          email: '',
          phoneNumber: '',
          dateOfBirth: '',
          gender: '',
          skinType: '',
          allergies: '',
          medicalHistory: '',
          currentMedications: '',
          notes: ''
        };
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showMessage('Patient created', `Patient ${patient.firstName} ${patient.lastName} was added successfully.`, MessageSeverity.success);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Unable to save patient', 'Please review your input and try again.', MessageSeverity.error, error);
      }
    });
  }

  selectPatient(patient: AestheticPatient): void {
    this.selectedPatient = patient;
  }
}
