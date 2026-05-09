// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { APP_INITIALIZER, ApplicationConfig, ErrorHandler, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { PreloadAllModules, provideRouter, TitleStrategy, UrlSerializer, withPreloading } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MAT_DATE_FORMATS, provideNativeDateAdapter } from '@angular/material/core';

import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { routes } from './app.routes';
import { AppErrorHandler } from './app-error.handler';
import { AppTitleService } from './services/app-title.service';
import { LowerCaseUrlSerializer } from './services/lowercase-url-serializer.service';
import { TranslateLanguageLoader } from './services/app-translation.service';
import { AppConfigService } from './services/app-config.service';
import { GlobalLoadingInterceptor } from './services/global-loading.interceptor';

const APP_DATE_FORMATS = {
  parse: {
    dateInput: 'input'
  },
  display: {
    dateInput: { day: '2-digit', month: 'short', year: 'numeric' },
    monthYearLabel: { month: 'short', year: 'numeric' },
    dateA11yLabel: { day: '2-digit', month: 'long', year: 'numeric' },
    monthYearA11yLabel: { month: 'long', year: 'numeric' }
  }
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(withInterceptorsFromDi()),
    provideCharts(withDefaultRegisterables()),
    provideAnimations(),
    provideNativeDateAdapter(),
    { provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS },
    importProvidersFrom(
      TranslateModule.forRoot({
        loader: { provide: TranslateLoader, useClass: TranslateLanguageLoader }
      })
    ),
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: TitleStrategy, useClass: AppTitleService },
    { provide: UrlSerializer, useClass: LowerCaseUrlSerializer },
    {
      provide: APP_INITIALIZER,
      useFactory: (appConfig: AppConfigService) => () => appConfig.init(),
      deps: [AppConfigService],
      multi: true
    },
    { provide: HTTP_INTERCEPTORS, useClass: GlobalLoadingInterceptor, multi: true }
  ]
};
