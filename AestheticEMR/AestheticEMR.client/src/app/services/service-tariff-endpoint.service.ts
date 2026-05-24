import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ServiceTariff } from '../models/legacy/service-tariff.model';

@Injectable({ providedIn: 'root' })
export class ServiceTariffEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get serviceTariffUrl() { return `${this.configurations.baseUrl}/api/servicetariff`; }

  getTariffCompaniesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.serviceTariffUrl}/companies`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getTariffCompaniesEndpoint<T>()))
    );
  }

  getTariffSourceCompaniesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.serviceTariffUrl}/source-companies`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getTariffSourceCompaniesEndpoint<T>()))
    );
  }

  getServiceTariffsEndpoint<T>(coyId?: string, search?: string): Observable<T> {
    const params = new URLSearchParams();

    if (coyId && coyId.trim()) {
      params.set('coyId', coyId.trim());
    }

    if (search && search.trim()) {
      params.set('search', search.trim());
    }

    const query = params.toString();
    const url = query ? `${this.serviceTariffUrl}?${query}` : this.serviceTariffUrl;

    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getServiceTariffsEndpoint<T>(coyId, search)))
    );
  }

  getServiceTariffByIdEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.serviceTariffUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getServiceTariffByIdEndpoint<T>(id)))
    );
  }

  getNewServiceTariffEndpoint<T>(model: ServiceTariff): Observable<T> {
    return this.http.post<T>(this.serviceTariffUrl, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewServiceTariffEndpoint<T>(model)))
    );
  }

  getUpdateServiceTariffEndpoint<T>(id: number, model: ServiceTariff): Observable<T> {
    return this.http.put<T>(`${this.serviceTariffUrl}/${id}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateServiceTariffEndpoint<T>(id, model)))
    );
  }

  getDeleteServiceTariffEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.serviceTariffUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteServiceTariffEndpoint<T>(id)))
    );
  }

  uploadServiceTariffEndpoint<T>(coyId: string, file: File, deleteExisting: boolean, category?: string): Observable<T> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('coyId', coyId);
    formData.append('deleteExisting', String(deleteExisting));
    if (category) {
      formData.append('category', category);
    }

    return this.http.post<T>(`${this.serviceTariffUrl}/upload`, formData, this.uploadHeaders).pipe(
      catchError(error => this.handleError(error, () => this.uploadServiceTariffEndpoint<T>(coyId, file, deleteExisting, category)))
    );
  }

  copyServiceTariffEndpoint<T>(targetCoyId: string, sourceCoyId: string, deleteExisting: boolean, category?: string): Observable<T> {
    const payload = {
      targetCoyId,
      sourceCoyId,
      deleteExisting,
      category: category ?? null
    };

    return this.http.post<T>(`${this.serviceTariffUrl}/copy`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.copyServiceTariffEndpoint<T>(targetCoyId, sourceCoyId, deleteExisting, category)))
    );
  }
}
