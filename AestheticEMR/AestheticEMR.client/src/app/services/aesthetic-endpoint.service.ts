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
  private get photosUrl() { return `${this.baseUrl}/photos`; }
  private get botoxConsultationsUrl() { return `${this.consultationsUrl}/botox`; }

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

  getBotoxConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.botoxConsultationsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getBotoxConsultationsEndpoint<T>())));
  }

  createBotoxConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(this.botoxConsultationsUrl, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createBotoxConsultationEndpoint<T>(consultation))));
  }

  getLaserConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.consultationsUrl}/laser`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getLaserConsultationsEndpoint<T>())));
  }

  createLaserConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(`${this.consultationsUrl}/laser`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createLaserConsultationEndpoint<T>(consultation))));
  }

  getSpaConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.consultationsUrl}/spa`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSpaConsultationsEndpoint<T>())));
  }

  createSpaConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(`${this.consultationsUrl}/spa`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createSpaConsultationEndpoint<T>(consultation))));
  }

  updateConsultationEndpoint<T>(consultationId: number, consultation: object): Observable<T> {
    return this.http.put<T>(`${this.consultationsUrl}/${consultationId}`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateConsultationEndpoint<T>(consultationId, consultation))));
  }

  deleteConsultationEndpoint<T>(consultationId: number): Observable<T> {
    return this.http.delete<T>(`${this.consultationsUrl}/${consultationId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteConsultationEndpoint<T>(consultationId))));
  }

  getConsentTemplatesEndpoint<T>(procedureType: string, includeInactive = false): Observable<T> {
    const params = new URLSearchParams();
    if (procedureType) {
      params.set('procedureType', procedureType);
    }
    if (includeInactive) {
      params.set('includeInactive', 'true');
    }

    return this.http.get<T>(`${this.baseUrl}/consent-templates${params.toString() ? `?${params.toString()}` : ''}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentTemplatesEndpoint<T>(procedureType, includeInactive))));
  }

  getConsentTemplateEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consent-templates/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentTemplateEndpoint<T>(id))));
  }

  createConsentTemplateEndpoint<T>(template: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/consent-templates`, JSON.stringify(template), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createConsentTemplateEndpoint<T>(template))));
  }

  updateConsentTemplateEndpoint<T>(id: number, template: object): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consent-templates/${id}`, JSON.stringify(template), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateConsentTemplateEndpoint<T>(id, template))));
  }

  deleteConsentTemplateEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/consent-templates/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteConsentTemplateEndpoint<T>(id))));
  }

  getConsentStatusEndpoint<T>(consultId: string, pNo: string, procedureType: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consent-status?consultId=${encodeURIComponent(consultId)}&pNo=${encodeURIComponent(pNo)}&procedureType=${encodeURIComponent(procedureType)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentStatusEndpoint<T>(consultId, pNo, procedureType))));
  }

  signConsentEndpoint<T>(payload: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/consents/sign`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.signConsentEndpoint<T>(payload))));
  }

  markConsentViewedEndpoint<T>(consentId: number): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consents/${consentId}/viewed`, JSON.stringify({}), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.markConsentViewedEndpoint<T>(consentId))));
  }

  getSignedConsentsEndpoint<T>(options?: { consultId?: string; pNo?: string; procedureType?: string; includeVoided?: boolean }): Observable<T> {
    const params = new URLSearchParams();
    if (options?.consultId) params.set('consultId', options.consultId);
    if (options?.pNo) params.set('pNo', options.pNo);
    if (options?.procedureType) params.set('procedureType', options.procedureType);
    if (options?.includeVoided !== undefined) params.set('includeVoided', String(options.includeVoided));
    const query = params.toString();
    const url = query ? `${this.baseUrl}/consents?${query}` : `${this.baseUrl}/consents`;

    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSignedConsentsEndpoint<T>(options))));
  }

  getSignedConsentEndpoint<T>(consentId: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consents/${consentId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSignedConsentEndpoint<T>(consentId))));
  }

  voidConsentEndpoint<T>(consentId: number, payload: object): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consents/${consentId}/void`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.voidConsentEndpoint<T>(consentId, payload))));
  }

  getPhotosEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.photosUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPhotosEndpoint<T>())));
  }

  createPhotoEndpoint<T>(photo: object): Observable<T> {
    return this.http.post<T>(this.photosUrl, JSON.stringify(photo), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createPhotoEndpoint<T>(photo))));
  }

  uploadPhotoEndpoint<T>(payload: FormData): Observable<T> {
    return this.http.post<T>(`${this.photosUrl}/upload`, payload, this.uploadHeaders).pipe(
      catchError(error => this.handleError(error, () => this.uploadPhotoEndpoint<T>(payload))));
  }

  updatePhotoEndpoint<T>(photoId: number, photo: object): Observable<T> {
    return this.http.put<T>(`${this.photosUrl}/${photoId}`, JSON.stringify(photo), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updatePhotoEndpoint<T>(photoId, photo))));
  }

  updatePhotoUploadEndpoint<T>(photoId: number, payload: FormData): Observable<T> {
    return this.http.put<T>(`${this.photosUrl}/${photoId}/upload`, payload, this.uploadHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updatePhotoUploadEndpoint<T>(photoId, payload))));
  }

  deletePhotoEndpoint<T>(photoId: number): Observable<T> {
    return this.http.delete<T>(`${this.photosUrl}/${photoId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deletePhotoEndpoint<T>(photoId))));
  }
}
