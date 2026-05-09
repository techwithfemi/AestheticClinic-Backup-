import { Routes } from '@angular/router';

export const tariffRoutes: Routes = [
  {
    path: '',
    redirectTo: 'services',
    pathMatch: 'full'
  },
  {
    path: 'stock',
    redirectTo: 'services',
    pathMatch: 'full'
  },
  {
    path: 'investigations',
    redirectTo: 'services',
    pathMatch: 'full'
  },
  {
    path: 'services',
    loadComponent: () => import('./services/services.component')
      .then(m => m.TariffServicesComponent),
    title: 'Tariff - Services'
  },
  {
    path: 'products',
    redirectTo: '/inventory/products',
    pathMatch: 'full'
  }
];
