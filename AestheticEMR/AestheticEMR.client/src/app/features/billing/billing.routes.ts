import { Routes } from '@angular/router';

export const billingRoutes: Routes = [
  {
    path: '',
    redirectTo: 'invoices',
    pathMatch: 'full'
  },
  {
    path: 'invoices',
    loadComponent: () => import('./invoices/invoices.component')
      .then(m => m.InvoicesComponent)
  },
  {
    path: 'claims',
    loadComponent: () => import('./claims/claims.component')
      .then(m => m.ClaimsComponent)
  }
];
