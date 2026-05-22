import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ModuleSettingsService {
  private readonly http = inject(HttpClient);
  private readonly cache = new Map<string, unknown>();

  getModuleSettings<T>(moduleName: string, defaults: T): Promise<T> {
    const cached = this.cache.get(moduleName) as T | undefined;
    if (cached) {
      return Promise.resolve(cached);
    }

    return firstValueFrom(this.http.get<T>(`/assets/module-settings/${moduleName}.json`))
      .then(settings => {
        this.cache.set(moduleName, settings as unknown);
        return settings;
      })
      .catch(() => defaults);
  }
}
