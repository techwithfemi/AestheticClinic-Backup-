import { Routes } from '@angular/router';

export const dentalRoutes: Routes = [
  {
    path: '',
    redirectTo: 'chart',
    pathMatch: 'full'
  },
  {
    path: 'chart',
    loadComponent: () => import('./odontogram/odontogram.component')
      .then(m => m.OdontogramComponent),
    title: 'Odontogram'
  },
  {
    path: 'xray',
    loadComponent: () => import('./imaging/imaging.component')
      .then(m => m.ImagingComponent),
    title: 'Dental Imaging'
  }
];
