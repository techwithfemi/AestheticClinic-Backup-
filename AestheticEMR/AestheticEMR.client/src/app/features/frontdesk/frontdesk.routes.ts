import { Routes } from '@angular/router';

export const frontdeskRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./frontdesk.component')
      .then(m => m.FrontdeskComponent)
  }
];
