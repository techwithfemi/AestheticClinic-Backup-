import { Routes } from '@angular/router';

export const aestheticsRoutes: Routes = [
  {
    path: '',
    redirectTo: 'botox',
    pathMatch: 'full'
  },
  {
    path: 'botox',
    loadComponent: () => import('./botox/botox.component')
      .then(m => m.BotoxComponent),
    title: 'Botox Treatments'
  },
  {
    path: 'laser',
    loadChildren: () => import('../laser/laser.routes')
      .then(m => m.laserRoutes),
    title: 'Laser Treatments'
  },
  {
    path: 'photos',
    loadComponent: () => import('./photos/photos.component')
      .then(m => m.PhotosComponent),
    title: 'Before & After Photos'
  }
];
