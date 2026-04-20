// ---------------------------------------
// Aesthetic EMR frontend endpoint service
// ---------------------------------------

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class AestheticEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/aesthetic`; }

  private get patientsUrl() { return `${this.baseUrl}/patients`; }
  private get consultationsUrl() { return `${this.baseUrl}/consultations`; }

  getPatientsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.patientsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPatientsEndpoint<T>())));
  }

  getPatientConsultationsEndpoint<T>(patientId: number): Observable<T> {
    return this.http.get<T>(`${this.patientsUrl}/${patientId}/consultations`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPatientConsultationsEndpoint<T>(patientId))));
  }

  createPatientEndpoint<T>(patient: object): Observable<T> {
    return this.http.post<T>(this.patientsUrl, JSON.stringify(patient), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createPatientEndpoint<T>(patient))));
  }

  createConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(this.consultationsUrl, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createConsultationEndpoint<T>(consultation))));
  }
}
