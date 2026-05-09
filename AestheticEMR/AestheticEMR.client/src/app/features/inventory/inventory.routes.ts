import { Routes } from '@angular/router';

export const inventoryRoutes: Routes = [
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  },
  {
    path: 'products',
    loadComponent: () => import('../tariff/products/products.component')
      .then(m => m.TariffProductsComponent),
    title: 'Inventory - Products'
  },
  {
    path: 'product-categories',
    loadComponent: () => import('./product-categories/product-categories.component')
      .then(m => m.ProductCategoriesComponent),
    title: 'Inventory - Product Category'
  }
];
