import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

export interface RosterGroupGridItem {
  groupName: string;
  staffName: string;
  deptName: string;
  assigned: string;
  groupID: number;
  empID: string;
}

export interface RosterGroupItem {
  rosterGrpId: number;
  rosterGrpName: string;
  deptId?: string;
  deptName?: string;
  exempted?: string;
  employeeCount?: number;
}

export interface RosterGroupDepartmentItem {
  deptId: string;
  deptName: string;
}

export interface RosterGroupAvailableStaffItem {
  empId: string;
  fullName: string;
  deptId?: string;
  rosterGrpId?: number | null;
  rosterGrpName?: string | null;
}

export interface RosterGroupSaveRequest {
  deptId: string;
  rosterGrpName: string;
  exempted?: string;
  empIds: string[];
}

@Injectable({ providedIn: 'root' })
export class RosterGroupEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/roster/groups`; }

  getAllEndpoint<T = RosterGroupGridItem[]>(): Observable<T> {
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAllEndpoint<T>()))
    );
  }

  getDepartmentsEndpoint<T = RosterGroupDepartmentItem[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/departments`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDepartmentsEndpoint<T>()))
    );
  }

  getAvailableStaffEndpoint<T = RosterGroupAvailableStaffItem[]>(deptId?: string): Observable<T> {
    const query = deptId ? `?deptId=${encodeURIComponent(deptId)}` : '';
    return this.http.get<T>(`${this.baseUrl}/staff${query}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAvailableStaffEndpoint<T>(deptId)))
    );
  }

  getCurrentDepartmentNameEndpoint<T = string>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/current-department`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getCurrentDepartmentNameEndpoint<T>()))
    );
  }

  getByIdEndpoint<T = RosterGroupItem>(id: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getByIdEndpoint<T>(id)))
    );
  }

  createEndpoint<T = RosterGroupItem>(payload: RosterGroupSaveRequest): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createEndpoint<T>(payload)))
    );
  }

  updateEndpoint<T = RosterGroupItem>(id: number, payload: RosterGroupSaveRequest): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateEndpoint<T>(id, payload)))
    );
  }

  deleteEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteEndpoint<T>(id)))
    );
  }
}
