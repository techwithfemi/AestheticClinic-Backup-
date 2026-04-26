import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { HRetainership } from '../models/legacy/h-retainership.model';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class HRetainershipEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get retainershipsUrl() { return `${this.configurations.baseUrl}/api/hretainership`; }

  getHRetainershipsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.retainershipsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getHRetainershipsEndpoint<T>()))
    );
  }

  getHRetainershipByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.retainershipsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getHRetainershipByIdEndpoint<T>(id)))
    );
  }

  getNewHRetainershipEndpoint<T>(retainership: HRetainership): Observable<T> {
    return this.http.post<T>(this.retainershipsUrl, JSON.stringify(retainership), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewHRetainershipEndpoint<T>(retainership)))
    );
  }

  getUpdateHRetainershipEndpoint<T>(id: string, retainership: HRetainership): Observable<T> {
    return this.http.put<T>(`${this.retainershipsUrl}/${id}`, JSON.stringify(retainership), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateHRetainershipEndpoint<T>(id, retainership)))
    );
  }

  getDeleteHRetainershipEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.retainershipsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteHRetainershipEndpoint<T>(id)))
    );
  }
}
