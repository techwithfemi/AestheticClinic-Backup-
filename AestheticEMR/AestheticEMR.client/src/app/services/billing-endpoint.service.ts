import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Billing } from '../models/legacy/billing.model';

@Injectable({ providedIn: 'root' })
export class BillingEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get billingsUrl() { return `${this.configurations.baseUrl}/api/billing`; }

  getInvoicesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.billingsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getInvoicesEndpoint<T>()))
    );
  }

  getInvoiceByBillNoEndpoint<T>(billNo: string): Observable<T> {
    return this.http.get<T>(`${this.billingsUrl}/${encodeURIComponent(billNo)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getInvoiceByBillNoEndpoint<T>(billNo)))
    );
  }

  getNewInvoiceEndpoint<T>(invoice: Billing): Observable<T> {
    return this.http.post<T>(this.billingsUrl, JSON.stringify(invoice), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewInvoiceEndpoint<T>(invoice)))
    );
  }

  getUpdateInvoiceEndpoint<T>(billNo: string, invoice: Billing): Observable<T> {
    return this.http.put<T>(`${this.billingsUrl}/${encodeURIComponent(billNo)}`, JSON.stringify(invoice), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateInvoiceEndpoint<T>(billNo, invoice)))
    );
  }

  getDeleteInvoiceEndpoint<T>(billNo: string): Observable<T> {
    return this.http.delete<T>(`${this.billingsUrl}/${encodeURIComponent(billNo)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteInvoiceEndpoint<T>(billNo)))
    );
  }
}
