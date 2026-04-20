import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { AuthGuard } from './services/auth-guard';

export const routes: Routes = [
  // 1. PUBLIC ROUTE: Login (No Sidebar/Header)
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login - AestheticEMR'
  },

  // 2. PROTECTED ROUTES: Wrapped in MainLayout
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard], // Uses the boilerplate's existing AuthGuard
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      // Dashboard
      {
        path: 'dashboard',
        loadComponent: () => import('./components/home/home.component')
          .then(m => m.HomeComponent)
      },

      // Feature Routes
      {
        path: 'dental',
        loadChildren: () => import('./features/dental/dental.routes')
          .then(m => m.dentalRoutes)
      },
      {
        path: 'billing',
        loadChildren: () => import('./features/billing/billing.routes')
          .then(m => m.billingRoutes)
      },
      {
        path: 'frontdesk',
        loadChildren: () => import('./features/frontdesk/frontdesk.routes')
          .then(m => m.frontdeskRoutes)
      },
      {
        path: 'laser',
        redirectTo: 'aesthetics/laser',
        pathMatch: 'full'
      },
      {
        path: 'spa',
        loadChildren: () => import('./features/spa/spa.routes')
          .then(m => m.spaRoutes)
      },
      {
        path: 'management',
        loadChildren: () => import('./features/management/management.routes')
          .then(m => m.managementRoutes)
      },
      {
        path: 'aesthetics',
        loadChildren: () => import('./features/aesthetics/aesthetics.routes')
          .then(m => m.aestheticsRoutes)
      },
      {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.routes')
          .then(m => m.adminRoutes)
      },
      {
        path: 'reports',
        loadChildren: () => import('./features/reports/reports.routes')
          .then(m => m.reportsRoutes)
      },

      // Settings & Profile (Replacing the boilerplate settingsTab)
      {
        path: 'settings',
        loadComponent: () => import('./components/settings/settings.component')
          .then(m => m.SettingsComponent)
      }
    ]
  },

  // 3. FALLBACK: Redirect to login
  { path: '**', redirectTo: 'login' }
];
