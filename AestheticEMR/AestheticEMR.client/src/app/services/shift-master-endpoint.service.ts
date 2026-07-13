import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

export interface DepartmentLookup {
  deptId: string;
  deptName: string;
  location?: string | null;
}

export interface ShiftMasterItem {
  shiftId: number;
  shiftName: string;
  departmentCount: number;
  departments: string;
}

export interface ShiftMasterDetail {
  shiftId: number;
  shiftName: string;
  deptIds: string[];
}

@Injectable({ providedIn: 'root' })
export class ShiftMasterEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/roster/shift-master`; }

  getAllEndpoint<T = ShiftMasterItem[]>(): Observable<T> {
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAllEndpoint<T>()))
    );
  }

  getDepartmentsEndpoint<T = DepartmentLookup[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/departments`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDepartmentsEndpoint<T>()))
    );
  }

  getByIdEndpoint<T = ShiftMasterDetail>(shiftId: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${shiftId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getByIdEndpoint<T>(shiftId)))
    );
  }

  createEndpoint<T = ShiftMasterDetail>(payload: ShiftMasterDetail): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createEndpoint<T>(payload)))
    );
  }

  updateEndpoint<T = ShiftMasterDetail>(shiftId: number, payload: ShiftMasterDetail): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${shiftId}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateEndpoint<T>(shiftId, payload)))
    );
  }

  deleteEndpoint<T>(shiftId: number): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${shiftId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteEndpoint<T>(shiftId)))
    );
  }
}
