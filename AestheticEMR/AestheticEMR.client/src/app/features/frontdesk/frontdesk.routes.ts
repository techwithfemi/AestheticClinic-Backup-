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
    path: 'attendance',
    loadComponent: () => import('./attendance/attendance.component')
      .then(m => m.AttendanceComponent),
    title: 'Patient Attendance'
  },
  {
    path: 'companies',
    loadComponent: () => import('../../components/retainerships/retainerships.component')
      .then(m => m.RetainershipsComponent),
    title: 'Company Information'
  }
];
