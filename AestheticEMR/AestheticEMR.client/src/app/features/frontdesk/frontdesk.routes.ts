import { Routes } from '@angular/router';

export const frontdeskRoutes: Routes = [
  {
    path: '',
    redirectTo: 'patients',
    pathMatch: 'full'
  },
  {
    path: 'patients',
    loadComponent: () => import('./patients/patients.component')
      .then(m => m.PatientsComponent),
    title: 'Patients Information'
  },
  {
    path: 'companies',
    loadComponent: () => import('../../components/retainerships/retainerships.component')
      .then(m => m.RetainershipsComponent),
    title: 'Company Information'
  }
];
