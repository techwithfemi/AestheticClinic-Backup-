import { Routes } from '@angular/router';

export const aestheticsRoutes: Routes = [
  {
    path: '',
    redirectTo: 'consults',
    pathMatch: 'full'
  },
  {
    path: 'consults',
    loadComponent: () => import('./consults/consults.component')
      .then(m => m.ConsultsComponent)
  }
];
