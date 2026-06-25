import { Routes } from '@angular/router';

export const accountingRoutes: Routes = [
  {
    path: '',
    redirectTo: 'journal-entries-info',
    pathMatch: 'full'
  },
  {
    path: 'journal-entries-info',
    loadComponent: () => import('./journal-entries-info/journal-entries-info.component')
      .then(m => m.JournalEntriesInfoComponent)
  },
  {
    path: 'expenses',
    loadComponent: () => import('./expenses/expenses.component')
      .then(m => m.ExpensesComponent)
  },
  {
    path: 'incomes',
    loadComponent: () => import('./incomes/incomes.component')
      .then(m => m.IncomesComponent)
  }
];
