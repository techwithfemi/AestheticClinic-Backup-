import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { Attendance } from '../models/legacy/attendance.model';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class AttendanceEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get attendanceUrl() { return `${this.configurations.baseUrl}/api/attendance`; }

  getAttendancesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.attendanceUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAttendancesEndpoint<T>()))
    );
  }

  getAttendanceByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.attendanceUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAttendanceByIdEndpoint<T>(id)))
    );
  }

  getAttendanceClinicTypesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.attendanceUrl}/clinic-types`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAttendanceClinicTypesEndpoint<T>()))
    );
  }

  getNewAttendanceEndpoint<T>(attendance: Attendance): Observable<T> {
    return this.http.post<T>(this.attendanceUrl, JSON.stringify(attendance), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewAttendanceEndpoint<T>(attendance)))
    );
  }

  getUpdateAttendanceEndpoint<T>(id: string, attendance: Attendance): Observable<T> {
    return this.http.put<T>(`${this.attendanceUrl}/${id}`, JSON.stringify(attendance), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateAttendanceEndpoint<T>(id, attendance)))
    );
  }

  getDeleteAttendanceEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.attendanceUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteAttendanceEndpoint<T>(id)))
    );
  }
}
