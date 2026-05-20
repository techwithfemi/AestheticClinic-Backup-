import { Routes } from '@angular/router';

export const dentalRoutes: Routes = [
  {
    path: '',
    redirectTo: 'clinical-session',
    pathMatch: 'full'
  },
  {
    path: 'clinical-session',
    loadComponent: () => import('./dental-page.component')
      .then(m => m.DentalPageComponent),
    title: 'Dental - Clinical Session'
  },
  {
    path: 'chart',
    redirectTo: 'clinical-session',
    pathMatch: 'full'
  },
  {
    path: 'xray',
    redirectTo: 'clinical-session',
    pathMatch: 'full'
  }
];
