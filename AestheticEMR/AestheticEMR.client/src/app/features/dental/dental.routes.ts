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
      .then(m => m.OdontogramComponent)
  },
  {
    path: 'xray',
    loadComponent: () => import('./imaging/imaging.component')
      .then(m => m.ImagingComponent)
  }
];
