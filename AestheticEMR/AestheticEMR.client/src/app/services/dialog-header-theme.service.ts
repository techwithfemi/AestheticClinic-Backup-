import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

import { ConfigurationService } from './configuration.service';

interface DialogHeaderTheme {
  gradientStart: string;
  gradientMid: string;
  gradientEnd: string;
  accentStart: string;
  accentMid: string;
  accentEnd: string;
  titleColor: string;
  closeBackground: string;
  closeBorder: string;
  closeHoverBackground: string;
  closeHoverBorder: string;
}

@Injectable({ providedIn: 'root' })
export class DialogHeaderThemeService {
  private readonly http = inject(HttpClient);
  private readonly configurations = inject(ConfigurationService);

  private readonly fallbackTheme: DialogHeaderTheme = {
    gradientStart: '#0b1f5e',
    gradientMid: '#12357f',
    gradientEnd: '#1d4ed8',
    accentStart: '#14b8a6',
    accentMid: '#f59e0b',
    accentEnd: '#2dd4bf',
    titleColor: '#f8fafc',
    closeBackground: 'rgba(11, 31, 94, 0.45)',
    closeBorder: 'rgba(255, 255, 255, 0.22)',
    closeHoverBackground: 'rgba(29, 78, 216, 0.55)',
    closeHoverBorder: 'rgba(255, 255, 255, 0.38)'
  };

  init(): Promise<void> {
    this.applyTheme(this.fallbackTheme);

    return firstValueFrom(this.http.get<DialogHeaderTheme>(`${this.configurations.baseUrl}/api/ui-settings/dialog-header-theme`))
      .then(theme => {
        this.applyTheme({ ...this.fallbackTheme, ...theme });
      })
      .catch(() => {
        // Keep fallback theme when endpoint is unavailable.
      });
  }

  private applyTheme(theme: DialogHeaderTheme): void {
    const root = document.documentElement;

    root.style.setProperty('--dialog-header-gradient-start', theme.gradientStart);
    root.style.setProperty('--dialog-header-gradient-mid', theme.gradientMid);
    root.style.setProperty('--dialog-header-gradient-end', theme.gradientEnd);
    root.style.setProperty('--dialog-header-accent-start', theme.accentStart);
    root.style.setProperty('--dialog-header-accent-mid', theme.accentMid);
    root.style.setProperty('--dialog-header-accent-end', theme.accentEnd);
    root.style.setProperty('--dialog-header-title-color', theme.titleColor);
    root.style.setProperty('--dialog-header-close-bg', theme.closeBackground);
    root.style.setProperty('--dialog-header-close-border', theme.closeBorder);
    root.style.setProperty('--dialog-header-close-hover-bg', theme.closeHoverBackground);
    root.style.setProperty('--dialog-header-close-hover-border', theme.closeHoverBorder);
  }
}
