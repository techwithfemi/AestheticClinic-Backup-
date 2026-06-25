import { Routes } from '@angular/router';

export const employeesRoutes: Routes = [
  {
    path: '',
    redirectTo: 'employee-info',
    pathMatch: 'full'
  },
  {
    path: 'employee-info',
    loadComponent: () => import('./employee-info/employee-info.component')
      .then(m => m.EmployeeInfoComponent)
  },
  {
    path: 'department',
    loadComponent: () => import('./department/department.component')
      .then(m => m.DepartmentComponent)
  }
];
