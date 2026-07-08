import { Routes } from '@angular/router';

export const accountingRoutes: Routes = [
  {
    path: '',
    redirectTo: 'journal',
    pathMatch: 'full'
  },
  {
    path: 'journal',
    loadComponent: () => import('./journal/journal.component')
      .then(m => m.JournalComponent),
    title: 'Journal Entries'
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
      .then(m => m.IncomesComponent),
    title: 'Income'
  }
];
