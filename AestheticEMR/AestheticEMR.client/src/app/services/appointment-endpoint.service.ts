import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { Appointment } from '../models/legacy/appointment.model';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class AppointmentEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get appointmentsUrl() { return `${this.configurations.baseUrl}/api/appointment`; }

  getAppointmentsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.appointmentsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAppointmentsEndpoint<T>()))
    );
  }

  getAppointmentByIdEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAppointmentByIdEndpoint<T>(id)))
    );
  }

  getAppointmentClinicTypesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/clinic-types`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAppointmentClinicTypesEndpoint<T>()))
    );
  }

  getAppointmentEmployeesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.appointmentsUrl}/employees`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAppointmentEmployeesEndpoint<T>()))
    );
  }

  getNewAppointmentEndpoint<T>(appointment: Appointment): Observable<T> {
    return this.http.post<T>(this.appointmentsUrl, JSON.stringify(appointment), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewAppointmentEndpoint<T>(appointment)))
    );
  }

  getUpdateAppointmentEndpoint<T>(id: number, appointment: Appointment): Observable<T> {
    return this.http.put<T>(`${this.appointmentsUrl}/${id}`, JSON.stringify(appointment), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateAppointmentEndpoint<T>(id, appointment)))
    );
  }

  getDeleteAppointmentEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.appointmentsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteAppointmentEndpoint<T>(id)))
    );
  }
}
