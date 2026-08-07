import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class RequestMetadataInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (req.headers.has('X-Skip-Request-Metadata')) {
      return next.handle(req);
    }

    const metadataHeaders: Record<string, string> = {
      'X-Device-Name': this.getDeviceName(),
      'X-Client-Device': this.getClientDevice(),
      'X-City': this.getStoredValue(['audit.city', 'city']),
      'X-Country': this.getStoredValue(['audit.country', 'country']),
      'X-Coordinates': this.getStoredValue(['audit.coordinates', 'coordinates'])
    };

    const filtered = Object.entries(metadataHeaders)
      .filter(([, value]) => value.trim().length > 0)
      .reduce((acc, [key, value]) => {
        acc[key] = value;
        return acc;
      }, {} as Record<string, string>);

    return next.handle(req.clone({ setHeaders: filtered }));
  }

  private getDeviceName(): string {
    const savedName = this.getStoredValue(['audit.deviceName', 'deviceName']);
    if (savedName) {
      return savedName;
    }

    if (typeof window === 'undefined' || !window.navigator) {
      return '';
    }

    const platform = window.navigator.platform?.trim();
    const host = window.location?.hostname?.trim();

    if (platform && host) {
      return `${platform}@${host}`;
    }

    return platform || host || '';
  }

  private getClientDevice(): string {
    if (typeof window === 'undefined' || !window.navigator) {
      return '';
    }

    return window.navigator.userAgent?.trim() ?? '';
  }

  private getStoredValue(keys: string[]): string {
    if (typeof window === 'undefined') {
      return '';
    }

    for (const key of keys) {
      const value = window.localStorage.getItem(key)?.trim()
        || window.sessionStorage.getItem(key)?.trim();

      if (value) {
        return value;
      }
    }

    return '';
  }
}
