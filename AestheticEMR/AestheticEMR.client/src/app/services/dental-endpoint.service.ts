import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class DentalEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/dental`; }
  private get chartsUrl() { return `${this.baseUrl}/charts`; }
  private get imagingUrl() { return `${this.baseUrl}/imaging`; }
  private get encounterUrl() { return `${this.baseUrl}/encounter`; }

  // Combined encounter
  getEncounterEndpoint<T>(consultId: string, pno: string): Observable<T> {
    return this.http.get<T>(`${this.encounterUrl}?consultId=${encodeURIComponent(consultId)}&pno=${encodeURIComponent(pno)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getEncounterEndpoint<T>(consultId, pno))));
  }

  saveEncounterEndpoint<T>(payload: object): Observable<T> {
    return this.http.post<T>(this.encounterUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.saveEncounterEndpoint<T>(payload))));
  }

  // Charts
  getChartsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.chartsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getChartsEndpoint<T>())));
  }

  createChartEndpoint<T>(chart: object): Observable<T> {
    return this.http.post<T>(this.chartsUrl, JSON.stringify(chart), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createChartEndpoint<T>(chart))));
  }

  updateChartEndpoint<T>(id: number, chart: object): Observable<T> {
    return this.http.put<T>(`${this.chartsUrl}/${id}`, JSON.stringify(chart), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateChartEndpoint<T>(id, chart))));
  }

  deleteChartEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.chartsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteChartEndpoint<T>(id))));
  }

  // Imaging
  getImagingEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.imagingUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getImagingEndpoint<T>())));
  }

  createImagingEndpoint<T>(imaging: object): Observable<T> {
    return this.http.post<T>(this.imagingUrl, JSON.stringify(imaging), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createImagingEndpoint<T>(imaging))));
  }

  updateImagingEndpoint<T>(id: number, imaging: object): Observable<T> {
    return this.http.put<T>(`${this.imagingUrl}/${id}`, JSON.stringify(imaging), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateImagingEndpoint<T>(id, imaging))));
  }

  deleteImagingEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.imagingUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteImagingEndpoint<T>(id))));
  }
}
