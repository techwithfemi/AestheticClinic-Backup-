import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EndpointBase } from './endpoint-base.service';

@Injectable({ providedIn: 'root' })
export class DrugNHISEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private readonly drugNhisUrl = '/api/drugnhis';

  getDrugTariffsEndpoint<T>(coyID?: string): Observable<T> {
    let url = this.drugNhisUrl;
    if (coyID) url += `?coyID=${encodeURIComponent(coyID)}`;
    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDrugTariffsEndpoint<T>(coyID)))
    );
  }
}
