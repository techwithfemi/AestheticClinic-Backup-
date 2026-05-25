import { Routes } from '@angular/router';

export const tariffRoutes: Routes = [
  {
    path: '',
    redirectTo: 'services',
    pathMatch: 'full'
  },
  {
    path: 'services',
    loadComponent: () => import('./services/services.component')
      .then(m => m.TariffServicesComponent),
    title: 'Tariff - Service',
    data: { category: 'Service' }
  },
  {
    path: 'investigations',
    loadComponent: () => import('./services/services.component')
      .then(m => m.TariffServicesComponent),
    title: 'Tariff - Investigation',
    data: { category: 'Investigation' }
  },
  {
    path: 'stock',
    loadComponent: () => import('./services/services.component')
      .then(m => m.TariffServicesComponent),
    title: 'Tariff - Drug',
    data: { category: 'Drug' }
  },
  {
    path: 'products',
    loadComponent: () => import('./services/services.component')
      .then(m => m.TariffServicesComponent),
    title: 'Tariff - Product',
    data: { category: 'Product' }
  }
];
