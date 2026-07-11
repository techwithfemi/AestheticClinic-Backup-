import { Routes } from '@angular/router';

export const staffRosterRoutes: Routes = [
  {
    path: '',
    redirectTo: 'create-roster',
    pathMatch: 'full'
  },
  {
    path: 'create-roster',
    loadComponent: () => import('./create-roster/create-roster.component')
      .then(m => m.CreateRosterComponent)
  },
  {
    path: 'staff-group',
    loadComponent: () => import('./staff-group/staff-group.component')
      .then(m => m.StaffGroupComponent)
  },
  {
    path: 'shifts-master',
    loadComponent: () => import('./shifts-master/shifts-master.component')
      .then(m => m.ShiftsMasterComponent)
  },
  {
    path: 'shift-details',
    loadComponent: () => import('./shifts/shifts.component')
      .then(m => m.ShiftsComponent)
  },
  {
    path: 'shifts',
    redirectTo: 'shift-details',
    pathMatch: 'full'
  }
];
