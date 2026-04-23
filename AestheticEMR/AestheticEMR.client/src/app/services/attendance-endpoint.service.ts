import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EndpointBase } from './endpoint-base.service';
import { Attendance } from '../models/legacy/attendance.model';

@Injectable({ providedIn: 'root' })
export class AttendanceEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private readonly attendanceUrl = '/api/attendance';

  getAttendancesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.attendanceUrl, this.requestHeaders);
  }

  getAttendanceByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.attendanceUrl}/${id}`, this.requestHeaders);
  }

  getAttendanceClinicTypesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.attendanceUrl}/clinic-types`, this.requestHeaders);
  }

  getNewAttendanceEndpoint<T>(attendance: Attendance): Observable<T> {
    return this.http.post<T>(this.attendanceUrl, JSON.stringify(attendance), this.requestHeaders);
  }

  getUpdateAttendanceEndpoint<T>(id: string, attendance: Attendance): Observable<T> {
    return this.http.put<T>(`${this.attendanceUrl}/${id}`, JSON.stringify(attendance), this.requestHeaders);
  }

  getDeleteAttendanceEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.attendanceUrl}/${id}`, this.requestHeaders);
  }
}
