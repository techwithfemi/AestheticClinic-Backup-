import { Routes } from '@angular/router';

export const adminRoutes: Routes = [
  {
    path: '',
    redirectTo: 'users',
    pathMatch: 'full'
  },
  {
    path: 'users',
    loadComponent: () => import('../../components/controls/users-management.component')
      .then(m => m.UsersManagementComponent)
  },
  {
    path: 'roles',
    loadComponent: () => import('../../components/controls/roles-management.component')
      .then(m => m.RolesManagementComponent)
  },
  {
    path: 'consent-templates',
    loadComponent: () => import('./consent-templates/consent-templates.component')
      .then(m => m.ConsentTemplatesComponent),
    title: 'Consent Templates'
  }
];
