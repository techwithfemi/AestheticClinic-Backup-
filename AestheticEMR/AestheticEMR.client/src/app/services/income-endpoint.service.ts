import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { IncomeBatchSaveRequest, IncomeEntry, IncomeListQuery } from '../models/accounting/income.model';

@Injectable({ providedIn: 'root' })
export class IncomeEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get incomesUrl() {
    return `${this.configurations.baseUrl}/api/accounting/incomes`;
  }

  getIncomesEndpoint<T>(query: IncomeListQuery): Observable<T> {
    let params = new HttpParams()
      .set('Page', String(query.page ?? 1))
      .set('PageSize', String(query.pageSize ?? 10));

    if (query.search) {
      params = params.set('Search', query.search);
    }
    if (query.fromDate) {
      params = params.set('FromDate', query.fromDate);
    }
    if (query.toDate) {
      params = params.set('ToDate', query.toDate);
    }

    return this.http.get<T>(this.incomesUrl, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getIncomesEndpoint<T>(query)))
    );
  }

  getIncomeEntriesByTranIdEndpoint<T>(tranId: string): Observable<T> {
    return this.http.get<T>(`${this.incomesUrl}/tran-id/${encodeURIComponent(tranId)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getIncomeEntriesByTranIdEndpoint<T>(tranId)))
    );
  }

  getNextTranIdEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.incomesUrl}/next-tran-id`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNextTranIdEndpoint<T>()))
    );
  }

  getIncomeAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.incomesUrl}/income-accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getIncomeAccountsEndpoint<T>()))
    );
  }

  getReceivingAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.incomesUrl}/receiving-accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getReceivingAccountsEndpoint<T>()))
    );
  }

  getNewIncomesBatchEndpoint<T>(entries: IncomeEntry[], tranId?: string | null): Observable<T> {
    const model: IncomeBatchSaveRequest = { tranId, entries };
    return this.http.post<T>(`${this.incomesUrl}/batch`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewIncomesBatchEndpoint<T>(entries, tranId)))
    );
  }

  getUpdateIncomeByTranIdEndpoint<T>(tranId: string, entries: IncomeEntry[]): Observable<T> {
    const model: IncomeBatchSaveRequest = { tranId, entries };
    return this.http.put<T>(`${this.incomesUrl}/tran-id/${encodeURIComponent(tranId)}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateIncomeByTranIdEndpoint<T>(tranId, entries)))
    );
  }

  getDeleteIncomeByTranIdEndpoint<T>(tranId: string, period: string, coyID: string): Observable<T> {
    const params = new HttpParams()
      .set('period', period)
      .set('coyID', coyID);

    return this.http.delete<T>(`${this.incomesUrl}/tran-id/${encodeURIComponent(tranId)}`, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteIncomeByTranIdEndpoint<T>(tranId, period, coyID)))
    );
  }
}
