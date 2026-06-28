import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Designation } from '../models/employee.model';

@Injectable({ providedIn: 'root' })
export class DesignationEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/designation`; }

  generateIdEndpoint(): Observable<string> {
    return this.http.get(`${this.baseUrl}/generate-id`, { ...this.requestHeaders, responseType: 'text' }).pipe(
      catchError(error => this.handleError(error, () => this.generateIdEndpoint()))
    );
  }

  getDesignationsEndpoint<T = Designation[]>(): Observable<T> {
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDesignationsEndpoint<T>()))
    );
  }

  getDesignationByIdEndpoint<T = Designation>(id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDesignationByIdEndpoint<T>(id)))
    );
  }

  createDesignationEndpoint<T = Designation>(designation: Designation): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(designation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createDesignationEndpoint<T>(designation)))
    );
  }

  updateDesignationEndpoint<T = Designation>(id: string, designation: Designation): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, JSON.stringify(designation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateDesignationEndpoint<T>(id, designation)))
    );
  }

  deleteDesignationEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteDesignationEndpoint<T>(id)))
    );
  }
}