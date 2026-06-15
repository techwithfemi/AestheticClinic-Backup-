import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Billing } from '../models/legacy/billing.model';

export interface SaveReceiptRequest {
  payType: string;
  amountToPay?: number;   // if omitted, backend pays the full balance
  accountNo?: string;
  chequeNo?: string;
  bankCode?: string;
  valueDate?: string;   // ISO date string
  remarks?: string;
  receivedBy?: string;
}

export interface UpdateReceiptRequest {
  payType: string;
  accountNo?: string;
  chequeNo?: string;
  bankCode?: string;
  valueDate?: string;   // ISO date string
  remarks?: string;
  receivedBy?: string;
}

export interface ReceiptSaved {
  receiptNo: string;
  receiptDate: string;
  amountPaid: number;
  payType: string;
}

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

  getInvoicePrintDataEndpoint<T>(billNo: string): Observable<T> {
    return this.http.get<T>(`${this.billingsUrl}/${encodeURIComponent(billNo)}/print-data`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getInvoicePrintDataEndpoint<T>(billNo)))
    );
  }

  getUpdateDiscountEndpoint<T>(billNo: string, discount: number): Observable<T> {
    return this.http.patch<T>(
      `${this.billingsUrl}/${encodeURIComponent(billNo)}/discount`,
      JSON.stringify({ discount }),
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateDiscountEndpoint<T>(billNo, discount)))
    );
  }

  getSaveReceiptEndpoint<T>(billNo: string, payload: SaveReceiptRequest): Observable<T> {
    return this.http.post<T>(
      `${this.billingsUrl}/${encodeURIComponent(billNo)}/receipt`,
      JSON.stringify(payload),
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.getSaveReceiptEndpoint<T>(billNo, payload)))
    );
  }

  getReceiptsEndpoint<T>(includeAll = false, retainId?: string): Observable<T> {
    const retainFilter = retainId ? `&retainId=${encodeURIComponent(retainId)}` : '';
    return this.http.get<T>(`${this.billingsUrl}/receipts?includeAll=${includeAll}${retainFilter}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getReceiptsEndpoint<T>(includeAll, retainId)))
    );
  }

  getDeleteReceiptEndpoint<T>(receiptNo: string): Observable<T> {
    return this.http.delete<T>(
      `${this.billingsUrl}/receipts/${encodeURIComponent(receiptNo)}`,
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteReceiptEndpoint<T>(receiptNo)))
    );
  }

  getUpdateReceiptEndpoint<T>(receiptNo: string, payload: UpdateReceiptRequest): Observable<T> {
    return this.http.put<T>(
      `${this.billingsUrl}/receipts/${encodeURIComponent(receiptNo)}`,
      JSON.stringify(payload),
      this.requestHeaders
    ).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateReceiptEndpoint<T>(receiptNo, payload)))
    );
  }

  getVwhRecordSummaryEndpoint<T>(consultId: string): Observable<T> {
    return this.http.get<T>(`${this.billingsUrl}/vwh-record/${encodeURIComponent(consultId)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getVwhRecordSummaryEndpoint<T>(consultId)))
    );
  }

  getBankAccountsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.billingsUrl}/bank-accounts`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getBankAccountsEndpoint<T>()))
    );
  }

  getPrivateCreditAccountEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.billingsUrl}/private-credit-account`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPrivateCreditAccountEndpoint<T>()))
    );
  }
}
