import { Routes } from '@angular/router';

export const dentalRoutes: Routes = [
  {
    path: '',
    redirectTo: 'chart',
    pathMatch: 'full'
  },
  {
    path: 'chart',
    loadComponent: () => import('./dental-page.component')
      .then(m => m.DentalPageComponent),
    title: 'Dental - Odontogram + Imaging'
  },
  {
    path: 'xray',
    loadComponent: () => import('./dental-page.component')
      .then(m => m.DentalPageComponent),
    title: 'Dental - Imaging'
  }
];
