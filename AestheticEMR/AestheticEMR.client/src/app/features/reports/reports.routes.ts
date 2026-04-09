import { Routes } from '@angular/router';

export const reportsRoutes: Routes = [
  {
    path: '',
    redirectTo: 'frontdesk-daily-report',
    pathMatch: 'full'
  },
  {
    path: 'frontdesk-daily-report',
    loadComponent: () => import('./frontdesk/frontdesk-daily-report.component')
      .then(m => m.FrontdeskDailyReportComponent)
  },
  {
    path: 'frontdesk-appointments-report',
    loadComponent: () => import('./frontdesk/frontdesk-appointments-report.component')
      .then(m => m.FrontdeskAppointmentsReportComponent)
  },
  {
    path: 'frontdesk-registration-report',
    loadComponent: () => import('./frontdesk/frontdesk-registration-report.component')
      .then(m => m.FrontdeskRegistrationReportComponent)
  },
  {
    path: 'laser-sessions-report',
    loadComponent: () => import('./laser/laser-sessions-report.component')
      .then(m => m.LaserSessionsReportComponent)
  },
  {
    path: 'laser-safety-report',
    loadComponent: () => import('./laser/laser-safety-report.component')
      .then(m => m.LaserSafetyReportComponent)
  },
  {
    path: 'laser-utilization-report',
    loadComponent: () => import('./laser/laser-utilization-report.component')
      .then(m => m.LaserUtilizationReportComponent)
  },
  {
    path: 'spa-services-report',
    loadComponent: () => import('./spa/spa-services-report.component')
      .then(m => m.SpaServicesReportComponent)
  },
  {
    path: 'spa-therapists-report',
    loadComponent: () => import('./spa/spa-therapists-report.component')
      .then(m => m.SpaTherapistsReportComponent)
  },
  {
    path: 'spa-packages-report',
    loadComponent: () => import('./spa/spa-packages-report.component')
      .then(m => m.SpaPackagesReportComponent)
  },
  {
    path: 'dental-treatments-report',
    loadComponent: () => import('./dental/dental-treatments-report.component')
      .then(m => m.DentalTreatmentsReportComponent)
  },
  {
    path: 'dental-imaging-report',
    loadComponent: () => import('./dental/dental-imaging-report.component')
      .then(m => m.DentalImagingReportComponent)
  },
  {
    path: 'dental-cases-report',
    loadComponent: () => import('./dental/dental-cases-report.component')
      .then(m => m.DentalCasesReportComponent)
  },
  {
    path: 'aesthetics-consultations-report',
    loadComponent: () => import('./aesthetics/aesthetics-consultations-report.component')
      .then(m => m.AestheticsConsultationsReportComponent)
  },
  {
    path: 'aesthetics-procedures-report',
    loadComponent: () => import('./aesthetics/aesthetics-procedures-report.component')
      .then(m => m.AestheticsProceduresReportComponent)
  },
  {
    path: 'aesthetics-outcomes-report',
    loadComponent: () => import('./aesthetics/aesthetics-outcomes-report.component')
      .then(m => m.AestheticsOutcomesReportComponent)
  },
  {
    path: 'billing-receipt-report',
    loadComponent: () => import('./billing/billing-receipt-report.component')
      .then(m => m.BillingReceiptReportComponent)
  },
  {
    path: 'billing-claims-report',
    loadComponent: () => import('./billing/billing-claims-report.component')
      .then(m => m.BillingClaimsReportComponent)
  },
  {
    path: 'billing-revenue-report',
    loadComponent: () => import('./billing/billing-revenue-report.component')
      .then(m => m.BillingRevenueReportComponent)
  },
  {
    path: 'admin-users-report',
    loadComponent: () => import('./admin/admin-users-report.component')
      .then(m => m.AdminUsersReportComponent)
  },
  {
    path: 'admin-audit-report',
    loadComponent: () => import('./admin/admin-audit-report.component')
      .then(m => m.AdminAuditReportComponent)
  },
  {
    path: 'admin-compliance-report',
    loadComponent: () => import('./admin/admin-compliance-report.component')
      .then(m => m.AdminComplianceReportComponent)
  }
];
