import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EndpointBase } from './endpoint-base.service';
import { Appointment } from '../models/legacy/appointment.model';

@Injectable({ providedIn: 'root' })
export class AppointmentEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private readonly appointmentsUrl = '/api/appointment';

  getAppointmentsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.appointmentsUrl, this.requestHeaders);
  }

  getAppointmentByIdEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/${id}`, this.requestHeaders);
  }

  getAppointmentClinicTypesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/clinic-types`, this.requestHeaders);
  }

  getAppointmentEmployeesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/employees`, this.requestHeaders);
  }

  getNewAppointmentEndpoint<T>(appointment: Appointment): Observable<T> {
    return this.http.post<T>(this.appointmentsUrl, JSON.stringify(appointment), this.requestHeaders);
  }

  getUpdateAppointmentEndpoint<T>(id: number, appointment: Appointment): Observable<T> {
    return this.http.put<T>(`${this.appointmentsUrl}/${id}`, JSON.stringify(appointment), this.requestHeaders);
  }

  getDeleteAppointmentEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.appointmentsUrl}/${id}`, this.requestHeaders);
  }
}
