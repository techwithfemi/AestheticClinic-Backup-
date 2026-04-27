import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { HPatient } from '../models/legacy/h-patient.model';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class HPatientEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get patientsUrl() { return `${this.configurations.baseUrl}/api/hpatient`; }

  getHPatientsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.patientsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getHPatientsEndpoint<T>()))
    );
  }

  getHPatientByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.patientsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getHPatientByIdEndpoint<T>(id)))
    );
  }

  getNewHPatientEndpoint<T>(patient: HPatient): Observable<T> {
    return this.http.post<T>(this.patientsUrl, JSON.stringify(patient), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewHPatientEndpoint<T>(patient)))
    );
  }

  getUpdateHPatientEndpoint<T>(id: string, patient: HPatient): Observable<T> {
    return this.http.put<T>(`${this.patientsUrl}/${id}`, JSON.stringify(patient), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateHPatientEndpoint<T>(id, patient)))
    );
  }

  getDeleteHPatientEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.patientsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteHPatientEndpoint<T>(id)))
    );
  }
}
