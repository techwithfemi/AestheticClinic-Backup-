import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

@Injectable({ providedIn: 'root' })
export class HRevenueTypeEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get revenueTypesUrl() { return `${this.configurations.baseUrl}/api/hrevenue`; }

  getRevenueTypesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.revenueTypesUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getRevenueTypesEndpoint<T>()))
    );
  }
}
