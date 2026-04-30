import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

interface AppConfig {
  appName: string;
  appVersion: string;
  apiEndpoint: string;
  maxItemsPerPage: number;
  logLevel: string;
  screenDisplay: string;
  clientName: string;
  clientID: string;
  clientLogo: string;
  altClientLogo: string;
  enableAnalytics: boolean;
}

@Injectable({ providedIn: 'root' })
export class AppConfigService {
  private http = inject(HttpClient);
  private config: AppConfig | null = null;

  get appName(): string { return this.config?.appName ?? 'Aesthetic EMR'; }
  get clientName(): string { return this.config?.clientName ?? 'Aesthetic EMR'; }
  get screenDisplay(): string { return this.config?.screenDisplay ?? ''; }
  get clientLogo(): string { return this.normalizeAssetPath(this.config?.clientLogo ?? ''); }
  get altClientLogo(): string { return this.normalizeAssetPath(this.config?.altClientLogo ?? ''); }

  /** config.json stores paths as "public/assets/img/..." — strip the leading "public/" */
  private normalizeAssetPath(path: string): string {
    return path.replace(/^public\//, '');
  }

  init(): Promise<void> {
    return firstValueFrom(this.http.get<AppConfig>('assets/config.json'))
      .then(config => { this.config = config; })
      .catch(() => { /* fall back to defaults silently */ });
  }
}
