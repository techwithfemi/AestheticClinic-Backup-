import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

export interface ShiftLookup {
  shiftId: number;
  shiftJob: string;
}

export interface ShiftDetail {
  shiftId: number;
  shiftJob: string;
  periodOfDay: string;
  resumptionTime: string;
  closingTime: string;
  punctualityRemarks?: string | null;
  lateRemarks?: string | null;
  normalClosingRemarks?: string | null;
  abnormalClosingRemarks?: string | null;
  evalTo?: string | null;
}

@Injectable({ providedIn: 'root' })
export class ShiftDetailsEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/roster/shift-details`; }

  getAllEndpoint<T = ShiftDetail[]>(): Observable<T> {
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAllEndpoint<T>()))
    );
  }

  getLookupsEndpoint<T = ShiftLookup[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/lookups`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getLookupsEndpoint<T>()))
    );
  }

  getByIdEndpoint<T = ShiftDetail>(shiftId: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${shiftId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getByIdEndpoint<T>(shiftId)))
    );
  }

  createEndpoint<T = ShiftDetail>(payload: ShiftDetail): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createEndpoint<T>(payload)))
    );
  }

  updateEndpoint<T = ShiftDetail>(shiftId: number, payload: ShiftDetail): Observable<T> {
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
