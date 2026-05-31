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
    path: 'receipts',
    loadComponent: () => import('./receipts/receipts.component')
      .then(m => m.ReceiptsComponent)
  }
];
