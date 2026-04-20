import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EndpointBase } from './endpoint-base.service';
import { HPatient } from '../models/legacy/h-patient.model';

@Injectable({ providedIn: 'root' })
export class HPatientEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private readonly patientsUrl = '/api/hpatient';

  getHPatientsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.patientsUrl, this.requestHeaders);
  }

  getHPatientByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.patientsUrl}/${id}`, this.requestHeaders);
  }

  getNewHPatientEndpoint<T>(patient: HPatient): Observable<T> {
    return this.http.post<T>(this.patientsUrl, JSON.stringify(patient), this.requestHeaders);
  }

  getUpdateHPatientEndpoint<T>(id: string, patient: HPatient): Observable<T> {
    return this.http.put<T>(`${this.patientsUrl}/${id}`, JSON.stringify(patient), this.requestHeaders);
  }

  getDeleteHPatientEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.patientsUrl}/${id}`, this.requestHeaders);
  }
}
