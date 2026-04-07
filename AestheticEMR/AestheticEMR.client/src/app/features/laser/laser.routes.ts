import { Routes } from '@angular/router';

export const laserRoutes: Routes = [
  {
    path: '',
    redirectTo: 'logs',
    pathMatch: 'full'
  },
  {
    path: 'logs',
    loadComponent: () => import('./logs/logs.component')
      .then(m => m.LogsComponent)
  },
  {
    path: 'safety',
    loadComponent: () => import('./safety/safety.component')
      .then(m => m.SafetyComponent)
  }
];
