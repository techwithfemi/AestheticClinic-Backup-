import { Routes } from '@angular/router';

export const aestheticsRoutes: Routes = [
  {
    path: '',
    redirectTo: 'procedures',
    pathMatch: 'full'
  },
  {
    path: 'procedures',
    loadComponent: () => import('./procedures/procedures.component')
      .then(m => m.ProceduresComponent),
    title: 'Aesthetic Procedures'
  },
  {
    path: 'view-consent',
    loadComponent: () => import('./view-consent/view-consent.component')
      .then(m => m.ViewConsentComponent),
    title: 'View Consent'
  },
  {
    path: 'botox',
    loadComponent: () => import('./procedures/procedures.component')
      .then(m => m.ProceduresComponent),
    data: { initialTab: 'neuromodulator' },
    title: 'Aesthetic Procedures'
  },
  {
    path: 'laser',
    loadComponent: () => import('./procedures/procedures.component')
      .then(m => m.ProceduresComponent),
    data: { initialTab: 'laser' },
    title: 'Aesthetic Procedures'
  },
  {
    path: 'photos',
    redirectTo: 'procedures',
    pathMatch: 'full'
  },
  {
    path: 'audit-trail',
    loadComponent: () => import('./audit-trail/audit-trail.component')
      .then(m => m.AuditTrailComponent),
    title: 'Audit Trail & Incidents'
  }
];
