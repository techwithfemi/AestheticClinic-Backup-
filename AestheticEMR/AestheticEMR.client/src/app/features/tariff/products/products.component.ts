import { Component, inject, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ProductEndpoint } from '../../../services/product-endpoint.service';
import { Product, ProductCategory, ProductEdit } from '../../../models/shop/product.model';
import { TariffProductDialogComponent } from './tariff-product-dialog.component';

@Component({
  selector: 'app-tariff-products',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatDialogModule
  ],
  animations: [fadeInOut],
  template: `
    <div [@fadeInOut] class="tariff-products-page">
      <mat-card>
        <mat-card-header>
          <mat-card-title>Inventory Products</mat-card-title>
          <mat-card-subtitle>Manage inventory product items and prices.</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <div class="toolbar-row">
            <mat-form-field appearance="outline">
              <mat-label>Quick Search</mat-label>
              <input matInput placeholder="Name or category" [(ngModel)]="searchText" (input)="onSearchChanged()" />
            </mat-form-field>

            <div class="toolbar-actions">
              <button mat-raised-button color="primary" type="button" (click)="openCreateDialog()">
                <mat-icon>add</mat-icon>
                Add Product
              </button>
              <button mat-stroked-button type="button" (click)="loadAll()">
                <mat-icon>refresh</mat-icon>
                Refresh
              </button>
            </div>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card>
        <mat-card-content>
          <div class="table-wrap">
            <table mat-table [dataSource]="dataSource" class="products-table">
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.name }}</td>
              </ng-container>

              <ng-container matColumnDef="category">
                <th mat-header-cell *matHeaderCellDef>Category</th>
                <td mat-cell *matCellDef="let row">{{ row.productCategoryName || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="buyingPrice">
                <th mat-header-cell *matHeaderCellDef class="text-end">Buying</th>
                <td mat-cell *matCellDef="let row" class="text-end">{{ row.buyingPrice | number:'1.2-2' }}</td>
              </ng-container>

              <ng-container matColumnDef="unitsInStock">
                <th mat-header-cell *matHeaderCellDef class="text-end">Stock</th>
                <td mat-cell *matCellDef="let row" class="text-end">{{ row.unitsInStock }}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef class="text-end">Actions</th>
                <td mat-cell *matCellDef="let row" class="text-end">
                  <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button type="button" (click)="deleteProduct(row)" title="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
            </table>

            <mat-paginator
              #paginator
              [length]="dataSource.data.length"
              [pageSize]="10"
              [pageSizeOptions]="[5, 10, 25]"
              [showFirstLastButtons]="true"
              aria-label="Select page">
            </mat-paginator>
          </div>

          @if (filteredProducts.length === 0 && !loadingIndicator) {
            <p class="empty-text">No products found</p>
          }

          @if (loadingIndicator) {
            <div class="spinner-wrap">
              <mat-spinner diameter="32"></mat-spinner>
            </div>
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .tariff-products-page { padding: 20px; display: grid; gap: 12px; }
    .toolbar-row { display: grid; grid-template-columns: 1fr auto; gap: 12px; align-items: start; }
    .toolbar-actions { display: flex; gap: 8px; flex-wrap: wrap; }
    .table-wrap { overflow: auto; -webkit-overflow-scrolling: touch; }
    .products-table { width: 100%; min-width: 680px; }
    .text-end { text-align: right; }
    .empty-text { margin: 12px 0 0; color: #777; }
    .spinner-wrap { display: flex; justify-content: center; padding: 12px 0 4px; }

    @media (max-width: 992px) {
      .tariff-products-page { padding: 16px; }
      .toolbar-row { grid-template-columns: 1fr; }
      .toolbar-actions button { flex: 1 1 160px; min-height: 44px; }
    }

    @media (max-width: 575.98px) {
      .tariff-products-page { padding: 12px; }
      .toolbar-actions button { width: 100%; }
    }
  `]
})
export class TariffProductsComponent implements AfterViewInit {
  private alertService = inject(AlertService);
  private productEndpoint = inject(ProductEndpoint);
  private dialog = inject(MatDialog);

  products: Product[] = [];
  filteredProducts: Product[] = [];
  categories: ProductCategory[] = [];
  searchText = '';
  loadingIndicator = false;
  displayedColumns = ['name', 'category', 'buyingPrice', 'unitsInStock', 'actions'];
  dataSource = new MatTableDataSource<Product>(this.filteredProducts);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor() {
    this.loadAll();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  loadAll(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading products...');

    Promise.all([
      this.productEndpoint.getProductsEndpoint<Product[]>().toPromise(),
      this.productEndpoint.getProductCategoriesEndpoint<ProductCategory[]>().toPromise()
    ]).then(([products, categories]) => {
      this.products = products ?? [];
      this.categories = categories ?? [];
      this.applyFilter();
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
    }).catch(error => {
      this.loadingIndicator = false;
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage('Load Error', `Unable to retrieve products.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
    });
  }

  onSearchChanged(): void {
    this.applyFilter();
  }

  openCreateDialog(): void {
    if (this.categories.length === 0) {
      this.alertService.showMessage('Validation', 'No product categories available. Please create a category first.', MessageSeverity.warn);
      return;
    }

    const dialogRef = this.dialog.open(TariffProductDialogComponent, {
      data: { isEdit: false, categories: this.categories },
      width: '580px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ProductEdit | undefined) => {
      if (!result) {
        return;
      }

      const payload: ProductEdit = {
        ...result,
        id: 0
      };

      this.alertService.startLoadingMessage();
      this.productEndpoint.getNewProductEndpoint<Product>(payload).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadAll();
          this.alertService.showMessage('Success', 'Product created successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Create Error', `Unable to create product.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  openEditDialog(item: Product): void {
    const category = this.categories.find(x => x.name === item.productCategoryName);

    const dialogRef = this.dialog.open(TariffProductDialogComponent, {
      data: {
        isEdit: true,
        categories: this.categories,
        item: {
          id: item.id,
          name: item.name ?? '',
          description: item.description,
          icon: item.icon,
          buyingPrice: item.buyingPrice,
          unitsInStock: item.unitsInStock,
          isActive: item.isActive,
          isDiscontinued: item.isDiscontinued,
          productCategoryId: category?.id ?? this.categories[0]?.id ?? 0
        } as ProductEdit
      },
      width: '580px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ProductEdit | undefined) => {
      if (!result) {
        return;
      }

      this.alertService.startLoadingMessage();
      this.productEndpoint.getUpdateProductEndpoint<Product>(item.id, { ...result, id: item.id }).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadAll();
          this.alertService.showMessage('Success', 'Product updated successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Update Error', `Unable to update product.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  deleteProduct(item: Product): void {
    this.alertService.showDialog('Are you sure you want to delete this product?', DialogType.confirm, () => {
      this.alertService.startLoadingMessage();
      this.productEndpoint.getDeleteProductEndpoint<void>(item.id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadAll();
          this.alertService.showMessage('Success', 'Product deleted successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Delete Error', `Unable to delete product.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  private applyFilter(): void {
    const term = this.searchText.trim().toLowerCase();
    if (!term) {
      this.filteredProducts = [...this.products];
      this.dataSource.data = this.filteredProducts;
      return;
    }

    this.filteredProducts = this.products.filter(x =>
      (x.name ?? '').toLowerCase().includes(term)
      || (x.productCategoryName ?? '').toLowerCase().includes(term)
      || (x.description ?? '').toLowerCase().includes(term)
    );
    this.dataSource.data = this.filteredProducts;
    if (this.paginator) {
      this.paginator.firstPage();
    }
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (!error || typeof error !== 'object') {
      return 'Unknown error';
    }

    const source = error as { error?: unknown; message?: unknown };

    if (typeof source.message === 'string' && source.message) {
      return source.message;
    }

    if (source.error && typeof source.error === 'object') {
      const errorBody = source.error as { errors?: Record<string, string[]>; title?: string; message?: string };
      if (typeof errorBody.message === 'string' && errorBody.message) {
        return errorBody.message;
      }

      if (typeof errorBody.title === 'string' && errorBody.title) {
        return errorBody.title;
      }

      if (errorBody.errors) {
        const firstErrorGroup = Object.values(errorBody.errors)[0];
        if (Array.isArray(firstErrorGroup) && firstErrorGroup.length > 0) {
          return firstErrorGroup[0];
        }
      }
    }

    return 'Unable to process request';
  }
}
