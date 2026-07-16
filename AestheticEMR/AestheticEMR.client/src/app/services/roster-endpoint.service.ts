import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

export interface RosterGroupLookup {
  groupId: number;
  groupName: string;
  deptId?: string;
  deptName?: string;
}

export interface RosterStaffLookup {
  empId: string;
  empName: string;
}

export interface RosterShiftLookup {
  sno: number;
  shiftName: string;
  evalTo: string;
  deptId?: string;
}

export interface RosterDaySelection {
  date: string;
  shiftId: number;
  shiftAbbrv: string;
  shiftName: string;
}

export interface RosterGridItem {
  sno: number;
  date: string;
  staffName?: string;
  clockIn?: string;
  clockOut?: string;
  status?: string;
  fine?: number;
  shiftName?: string;
  groupName?: string;
  startDate?: string;
  endDate?: string;
  deptName?: string;
  exempted?: string;
  groupID?: string;
  rosterGrpShiftID?: number;
  empID?: string;
  shiftAbbrv?: string;
}

export interface RosterLookups {
  groups: RosterGroupLookup[];
  sourceStaff: RosterStaffLookup[];
  targetStaff: RosterStaffLookup[];
  shifts: RosterShiftLookup[];
}

export interface RosterSaveRequest {
  deptId?: string;
  deptName?: string;
  groupId?: number | null;
  sourceEmpId?: string | null;
  targetEmpId?: string | null;
  groupName: string;
  selectedDays: RosterDaySelection[];
  unselectedDays?: RosterDaySelection[];
}

@Injectable({ providedIn: 'root' })
export class RosterEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/roster`; }

  getLookupsEndpoint<T = RosterLookups>(deptId: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/lookups`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getLookupsEndpoint<T>(deptId)))
    );
  }

  getGridEndpoint<T = RosterGridItem[]>(query: { deptId: string; groupId?: number | null; fromDate?: string; toDate?: string; latestOnly?: boolean; }): Observable<T> {
    const params = new URLSearchParams();
    if (query.groupId != null) params.set('groupId', String(query.groupId));
    if (query.fromDate) params.set('fromDate', query.fromDate);
    if (query.toDate) params.set('toDate', query.toDate);
    if (query.latestOnly != null) params.set('latestOnly', String(query.latestOnly));
    return this.http.get<T>(`${this.baseUrl}/grid?${params.toString()}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getGridEndpoint<T>(query)))
    );
  }

  getExistingEndpoint<T = RosterGridItem[]>(query: { empId: string; deptId?: string; fromDate?: string; toDate?: string; }): Observable<T> {
    const params = new URLSearchParams();
    params.set('empId', query.empId);
    if (query.deptId) params.set('deptId', query.deptId);
    if (query.fromDate) params.set('fromDate', query.fromDate);
    if (query.toDate) params.set('toDate', query.toDate);
    return this.http.get<T>(`${this.baseUrl}/existing?${params.toString()}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getExistingEndpoint<T>(query)))
    );
  }

  saveRosterEndpoint<T = { createdCount: number; items: RosterGridItem[] }>(payload: RosterSaveRequest): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.saveRosterEndpoint<T>(payload)))
    );
  }

  deleteRosterEntryEndpoint<T>(sNo: number): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${sNo}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteRosterEntryEndpoint<T>(sNo)))
    );
  }
}
