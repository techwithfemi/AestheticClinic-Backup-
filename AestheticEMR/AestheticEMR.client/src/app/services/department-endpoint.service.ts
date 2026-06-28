import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Department } from '../models/employee.model';

@Injectable({ providedIn: 'root' })
export class DepartmentEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/department`; }

  generateIdEndpoint(): Observable<string> {
    return this.http.get(`${this.baseUrl}/generate-id`, { ...this.requestHeaders, responseType: 'text' }).pipe(
      catchError(error => this.handleError(error, () => this.generateIdEndpoint()))
    );
  }

  getDepartmentsEndpoint<T = Department[]>(includeUsage = true): Observable<T> {
    // Always request the list with in-use counts so the UI can show / disable delete.
    // The flag is kept for future use (e.g. a slimmer "for dropdown" endpoint) but
    // currently both flows hit GET /api/department.
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDepartmentsEndpoint<T>(includeUsage)))
    );
  }

  getDepartmentByIdEndpoint<T = Department>(id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDepartmentByIdEndpoint<T>(id)))
    );
  }

  createDepartmentEndpoint<T = Department>(department: Department): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(department), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createDepartmentEndpoint<T>(department)))
    );
  }

  updateDepartmentEndpoint<T = Department>(id: string, department: Department): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, JSON.stringify(department), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateDepartmentEndpoint<T>(id, department)))
    );
  }

  deleteDepartmentEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteDepartmentEndpoint<T>(id)))
    );
  }
}
