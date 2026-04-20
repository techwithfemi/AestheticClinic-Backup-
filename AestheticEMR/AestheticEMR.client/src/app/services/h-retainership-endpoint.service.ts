import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EndpointBase } from './endpoint-base.service';
import { HRetainership } from '../models/legacy/h-retainership.model';

@Injectable({ providedIn: 'root' })
export class HRetainershipEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private readonly retainershipsUrl = '/api/hretainership';

  getHRetainershipsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.retainershipsUrl, this.requestHeaders);
  }

  getHRetainershipByIdEndpoint<T>(id: string): Observable<T> {
    return this.http.get<T>(`${this.retainershipsUrl}/${id}`, this.requestHeaders);
  }

  getNewHRetainershipEndpoint<T>(retainership: HRetainership): Observable<T> {
    return this.http.post<T>(this.retainershipsUrl, JSON.stringify(retainership), this.requestHeaders);
  }

  getUpdateHRetainershipEndpoint<T>(id: string, retainership: HRetainership): Observable<T> {
    return this.http.put<T>(`${this.retainershipsUrl}/${id}`, JSON.stringify(retainership), this.requestHeaders);
  }

  getDeleteHRetainershipEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.retainershipsUrl}/${id}`, this.requestHeaders);
  }
}
