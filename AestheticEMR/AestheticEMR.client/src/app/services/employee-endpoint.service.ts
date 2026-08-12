import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Employee, EmployeeReportRow, Designation, EmpDepartment } from '../models/employee.model';

@Injectable({ providedIn: 'root' })
export class EmployeeEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/employee`; }

  generateIdEndpoint(): Observable<string> {
    return this.http.get(`${this.baseUrl}/generate-id`, { ...this.requestHeaders, responseType: 'text' }).pipe(
      catchError(error => this.handleError(error, () => this.generateIdEndpoint()))
    );
  }

  getEmployeesEndpoint<T = Employee[]>(): Observable<T> {
    return this.http.get<T>(this.baseUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getEmployeesEndpoint<T>()))
    );
  }

  getEmployeeReportRowsEndpoint<T = EmployeeReportRow[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/report`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getEmployeeReportRowsEndpoint<T>()))
    );
  }

  getEmployeeByIdEndpoint<T = Employee>(id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getEmployeeByIdEndpoint<T>(id)))
    );
  }

  createEmployeeEndpoint<T = Employee>(employee: Employee): Observable<T> {
    return this.http.post<T>(this.baseUrl, JSON.stringify(employee), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createEmployeeEndpoint<T>(employee)))
    );
  }

  updateEmployeeEndpoint<T = Employee>(id: string, employee: Employee): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${id}`, JSON.stringify(employee), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateEmployeeEndpoint<T>(id, employee)))
    );
  }

  deleteEmployeeEndpoint<T>(id: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteEmployeeEndpoint<T>(id)))
    );
  }

  getDesignationsEndpoint<T = Designation[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/designations`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDesignationsEndpoint<T>()))
    );
  }

  getDepartmentsEndpoint<T = EmpDepartment[]>(): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/departments`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDepartmentsEndpoint<T>()))
    );
  }
}
