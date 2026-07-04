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
    path: 'chart-of-accounts',
    loadComponent: () => import('./chart-of-accounts/chart-of-accounts.component')
      .then(m => m.ChartOfAccountsComponent),
    title: 'Chart of Accounts'
  },
  {
    path: 'expenses',
    loadComponent: () => import('./expenses/expenses.component')
      .then(m => m.ExpensesComponent),
    title: 'Expenses'
  },
  {
    path: 'incomes',
    loadComponent: () => import('./incomes/incomes.component')
      .then(m => m.IncomesComponent)
  }
];
