import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ChartOfAccountEntry, ChartOfAccountListQuery } from '../models/accounting/chart-of-account.model';

@Injectable({ providedIn: 'root' })
export class ChartOfAccountEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get chartOfAccountsUrl() {
    return `${this.configurations.baseUrl}/api/accounting/chart-of-accounts`;
  }

  getChartOfAccountsEndpoint<T>(query: ChartOfAccountListQuery): Observable<T> {
    let params = new HttpParams()
      .set('Page', String(query.page ?? 1))
      .set('PageSize', String(query.pageSize ?? 10));

    if (query.search) {
      params = params.set('Search', query.search);
    }

    if (query.sortBy) {
      params = params.set('SortBy', query.sortBy);
    }

    if (query.sortDirection) {
      params = params.set('SortDirection', query.sortDirection);
    }

    return this.http.get<T>(this.chartOfAccountsUrl, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getChartOfAccountsEndpoint<T>(query)))
    );
  }

  getChartOfAccountByIdEndpoint<T>(sNo: number): Observable<T> {
    return this.http.get<T>(`${this.chartOfAccountsUrl}/${sNo}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getChartOfAccountByIdEndpoint<T>(sNo)))
    );
  }

  getChartOfAccountDefaultsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.chartOfAccountsUrl}/defaults`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getChartOfAccountDefaultsEndpoint<T>()))
    );
  }

  getChartOfAccountGroupsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.chartOfAccountsUrl}/groups`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getChartOfAccountGroupsEndpoint<T>()))
    );
  }

  getNextChartOfAccountNoEndpoint<T>(groupId: string): Observable<T> {
    const params = new HttpParams().set('groupId', groupId);
    return this.http.get<T>(`${this.chartOfAccountsUrl}/next-account-no`, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getNextChartOfAccountNoEndpoint<T>(groupId)))
    );
  }

  getNewChartOfAccountEndpoint<T>(model: ChartOfAccountEntry): Observable<T> {
    return this.http.post<T>(this.chartOfAccountsUrl, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewChartOfAccountEndpoint<T>(model)))
    );
  }

  getUpdateChartOfAccountEndpoint<T>(sNo: number, model: ChartOfAccountEntry): Observable<T> {
    return this.http.put<T>(`${this.chartOfAccountsUrl}/${sNo}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateChartOfAccountEndpoint<T>(sNo, model)))
    );
  }

  getDeleteChartOfAccountEndpoint<T>(sNo: number): Observable<T> {
    return this.http.delete<T>(`${this.chartOfAccountsUrl}/${sNo}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteChartOfAccountEndpoint<T>(sNo)))
    );
  }
}
