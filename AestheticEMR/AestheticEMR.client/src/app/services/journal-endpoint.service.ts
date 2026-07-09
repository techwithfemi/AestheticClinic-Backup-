import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import {
  JournalEntry,
  JournalListLineQuery,
  JournalListQuery,
} from '../models/accounting/journal-entry.model';

@Injectable({ providedIn: 'root' })
export class JournalEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get journalUrl() {
    return `${this.configurations.baseUrl}/api/accounting/journal`;
  }

  getJournalEntriesEndpoint<T>(query: JournalListQuery): Observable<T> {
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

    return this.http.get<T>(this.journalUrl, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getJournalEntriesEndpoint<T>(query)))
    );
  }

  /**
   * Flat line-level list backed by `vwTranx`. One row per journal line
   * with derived Dr/Cr amounts and a running SN. `tranDate` defaults to
   * the current date on first load; sending a non-empty `search` lets
   * the user find any TranNo across all dates.
   */
  getJournalEntryLinesEndpoint<T>(query: JournalListLineQuery): Observable<T> {
    let params = new HttpParams()
      .set('Page', String(query.page ?? 1))
      .set('PageSize', String(query.pageSize ?? 10));

    if (query.search) {
      params = params.set('Search', query.search);
    }
    if (query.tranDate) {
      params = params.set('TranDate', query.tranDate);
    }
    if (query.fromDate) {
      params = params.set('FromDate', query.fromDate);
    }
    if (query.toDate) {
      params = params.set('ToDate', query.toDate);
    }

    return this.http.get<T>(`${this.journalUrl}/lines`, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getJournalEntryLinesEndpoint<T>(query)))
    );
  }

  getJournalEntryEndpoint<T>(tranNo: string): Observable<T> {
    return this.http.get<T>(`${this.journalUrl}/${encodeURIComponent(tranNo)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getJournalEntryEndpoint<T>(tranNo)))
    );
  }

  getNextJournalTranNoEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.journalUrl}/next-tran-no`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNextJournalTranNoEndpoint<T>()))
    );
  }

  getJournalAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.journalUrl}/accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getJournalAccountsEndpoint<T>()))
    );
  }

  getJournalCostCentersEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.journalUrl}/cost-centers`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getJournalCostCentersEndpoint<T>()))
    );
  }

  createJournalEntryEndpoint<T>(model: JournalEntry): Observable<T> {
    return this.http.post<T>(this.journalUrl, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createJournalEntryEndpoint<T>(model)))
    );
  }

  updateJournalEntryEndpoint<T>(tranNo: string, model: JournalEntry): Observable<T> {
    return this.http.put<T>(
      `${this.journalUrl}/${encodeURIComponent(tranNo)}`,
      JSON.stringify(model),
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.updateJournalEntryEndpoint<T>(tranNo, model)))
    );
  }

  deleteJournalEntryEndpoint<T>(tranNo: string): Observable<T> {
    return this.http.delete<T>(
      `${this.journalUrl}/${encodeURIComponent(tranNo)}`,
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.deleteJournalEntryEndpoint<T>(tranNo)))
    );
  }
}
