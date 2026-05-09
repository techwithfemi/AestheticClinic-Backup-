import { Injectable, inject } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { GlobalLoadingService } from './global-loading.service';

@Injectable()
export class GlobalLoadingInterceptor implements HttpInterceptor {
  private readonly globalLoadingService = inject(GlobalLoadingService);

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (req.headers.has('X-Skip-Global-Loading')) {
      return next.handle(req);
    }

    this.globalLoadingService.startRequest();

    return next.handle(req).pipe(
      finalize(() => this.globalLoadingService.endRequest())
    );
  }
}
