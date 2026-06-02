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
    path: 'invoices/:billNo/preview',
    loadComponent: () => import('./print/billing-invoice-print.component')
      .then(m => m.BillingInvoicePrintComponent)
  },
  {
    path: 'receipts/:billNo/preview',
    loadComponent: () => import('./print/billing-receipt-print.component')
      .then(m => m.BillingReceiptPrintComponent)
  },
  {
    path: 'receipts',
    loadComponent: () => import('./receipts/receipts.component')
      .then(m => m.ReceiptsComponent)
  }
];
