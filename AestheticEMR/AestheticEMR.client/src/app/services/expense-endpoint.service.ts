import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ExpenseBatchSaveRequest, ExpenseEntry, ExpenseListQuery } from '../models/accounting/expense.model';

@Injectable({ providedIn: 'root' })
export class ExpenseEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get expensesUrl() {
    return `${this.configurations.baseUrl}/api/accounting/expenses`;
  }

  getExpensesEndpoint<T>(query: ExpenseListQuery): Observable<T> {
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

    return this.http.get<T>(this.expensesUrl, { ...this.requestHeaders, params }).pipe(
      catchError(error => this.handleError(error, () => this.getExpensesEndpoint<T>(query)))
    );
  }

  getExpenseByIdEndpoint<T>(sNo: number): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/${sNo}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getExpenseByIdEndpoint<T>(sNo)))
    );
  }

  getExpenseEntriesByTranIdEndpoint<T>(tranId: string): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/tran-id/${encodeURIComponent(tranId)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getExpenseEntriesByTranIdEndpoint<T>(tranId)))
    );
  }

  getNextTranIdEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/next-tran-id`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNextTranIdEndpoint<T>()))
    );
  }

  getExpenseAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/expense-accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getExpenseAccountsEndpoint<T>()))
    );
  }

  getPayingAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/paying-accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPayingAccountsEndpoint<T>()))
    );
  }

  getTransactionLinesByTranId<T>(tranId: string): Observable<T> {
    return this.http.get<T>(`${this.expensesUrl}/transaction-lines/${encodeURIComponent(tranId)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getTransactionLinesByTranId<T>(tranId)))
    );
  }

  getNewExpenseEndpoint<T>(model: ExpenseEntry): Observable<T> {
    return this.http.post<T>(this.expensesUrl, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewExpenseEndpoint<T>(model)))
    );
  }

  getNewExpensesBatchEndpoint<T>(entries: ExpenseEntry[], tranId?: string | null): Observable<T> {
    const model: ExpenseBatchSaveRequest = { tranId, entries };
    return this.http.post<T>(`${this.expensesUrl}/batch`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewExpensesBatchEndpoint<T>(entries, tranId)))
    );
  }

  getUpdateExpenseByTranIdEndpoint<T>(tranId: string, entries: ExpenseEntry[]): Observable<T> {
    const model: ExpenseBatchSaveRequest = { tranId, entries };
    return this.http.put<T>(`${this.expensesUrl}/tran-id/${encodeURIComponent(tranId)}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateExpenseByTranIdEndpoint<T>(tranId, entries)))
    );
  }

  getUpdateExpenseEndpoint<T>(sNo: number, model: ExpenseEntry): Observable<T> {
    return this.http.put<T>(`${this.expensesUrl}/${sNo}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateExpenseEndpoint<T>(sNo, model)))
    );
  }

  getDeleteExpenseEndpoint<T>(sNo: number): Observable<T> {
    return this.http.delete<T>(`${this.expensesUrl}/${sNo}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteExpenseEndpoint<T>(sNo)))
    );
  }

  getDeleteExpenseByTranIdEndpoint<T>(tranId: string): Observable<T> {
    return this.http.delete<T>(`${this.expensesUrl}/tran-id/${encodeURIComponent(tranId)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteExpenseByTranIdEndpoint<T>(tranId)))
    );
  }
}
