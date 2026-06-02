import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ProductTariff } from '../models/legacy/product-tariff.model';

@Injectable({ providedIn: 'root' })
export class ProductTariffEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get productTariffUrl() { return `${this.configurations.baseUrl}/api/product-tariff`; }

  getProductTariffsEndpoint<T>(coyID: string): Observable<T> {
    return this.http.get<T>(`${this.productTariffUrl}?coyID=${encodeURIComponent(coyID)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getProductTariffsEndpoint<T>(coyID)))
    );
  }

  getUpdateProductTariffEndpoint<T>(id: number, model: ProductTariff): Observable<T> {
    return this.http.put<T>(`${this.productTariffUrl}/${id}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateProductTariffEndpoint<T>(id, model)))
    );
  }
}
