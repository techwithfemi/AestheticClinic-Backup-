import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { fadeInOut } from '../../../services/animations';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ProductEndpoint } from '../../../services/product-endpoint.service';
import { ProductCategory, ProductCategoryEdit } from '../../../models/shop/product.model';
import { ProductCategoryDialogComponent } from './product-category-dialog.component';

@Component({
  selector: 'app-product-categories',
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
    MatProgressSpinnerModule,
    MatDialogModule
  ],
  animations: [fadeInOut],
  template: `
    <div [@fadeInOut] class="inventory-categories-page">
      <mat-card>
        <mat-card-header>
          <mat-card-title>Product Categories</mat-card-title>
          <mat-card-subtitle>Manage inventory product categories.</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <div class="toolbar-row">
            <mat-form-field appearance="outline">
              <mat-label>Quick Search</mat-label>
              <input matInput placeholder="Category name" [(ngModel)]="searchText" (input)="onSearchChanged()" />
            </mat-form-field>

            <div class="toolbar-actions">
              <button mat-raised-button color="primary" type="button" (click)="openCreateDialog()">
                <mat-icon>add</mat-icon>
                Add Category
              </button>
              <button mat-stroked-button type="button" (click)="loadCategories()">
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
            <table mat-table [dataSource]="filteredCategories" class="categories-table">
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let row">{{ row.name }}</td>
              </ng-container>

              <ng-container matColumnDef="description">
                <th mat-header-cell *matHeaderCellDef>Description</th>
                <td mat-cell *matCellDef="let row">{{ row.description || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="icon">
                <th mat-header-cell *matHeaderCellDef>Icon</th>
                <td mat-cell *matCellDef="let row">{{ row.icon || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef class="text-end">Actions</th>
                <td mat-cell *matCellDef="let row" class="text-end">
                  <button mat-icon-button type="button" (click)="openEditDialog(row)" title="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button type="button" (click)="deleteCategory(row)" title="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
            </table>
          </div>

          @if (filteredCategories.length === 0 && !loadingIndicator) {
            <p class="empty-text">No categories found</p>
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
    .inventory-categories-page { padding: 20px; display: grid; gap: 12px; }
    .toolbar-row { display: grid; grid-template-columns: 1fr auto; gap: 12px; align-items: start; }
    .toolbar-actions { display: flex; gap: 8px; flex-wrap: wrap; }
    .table-wrap { overflow: auto; }
    .categories-table { width: 100%; }
    .text-end { text-align: right; }
    .empty-text { margin: 12px 0 0; color: #777; }
    .spinner-wrap { display: flex; justify-content: center; padding: 12px 0 4px; }
    @media (max-width: 992px) {
      .toolbar-row { grid-template-columns: 1fr; }
    }
  `]
})
export class ProductCategoriesComponent {
  private alertService = inject(AlertService);
  private productEndpoint = inject(ProductEndpoint);
  private dialog = inject(MatDialog);

  categories: ProductCategory[] = [];
  filteredCategories: ProductCategory[] = [];
  searchText = '';
  loadingIndicator = false;
  displayedColumns = ['name', 'description', 'icon', 'actions'];

  constructor() {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading categories...');

    this.productEndpoint.getProductCategoriesEndpoint<ProductCategory[]>().subscribe({
      next: data => {
        this.categories = data ?? [];
        this.applyFilter();
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load Error', `Unable to retrieve product categories.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
      }
    });
  }

  onSearchChanged(): void {
    this.applyFilter();
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(ProductCategoryDialogComponent, {
      data: { isEdit: false },
      width: '500px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ProductCategoryEdit | undefined) => {
      if (!result) {
        return;
      }

      this.alertService.startLoadingMessage();
      this.productEndpoint.getNewProductCategoryEndpoint<ProductCategory>({ ...result, id: 0 }).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadCategories();
          this.alertService.showMessage('Success', 'Product category created successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Create Error', `Unable to create product category.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  openEditDialog(item: ProductCategory): void {
    const dialogRef = this.dialog.open(ProductCategoryDialogComponent, {
      data: {
        isEdit: true,
        item: {
          id: item.id,
          name: item.name,
          description: item.description,
          icon: item.icon
        } as ProductCategoryEdit
      },
      width: '500px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: ProductCategoryEdit | undefined) => {
      if (!result) {
        return;
      }

      this.alertService.startLoadingMessage();
      this.productEndpoint.getUpdateProductCategoryEndpoint<ProductCategory>(item.id, { ...result, id: item.id }).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadCategories();
          this.alertService.showMessage('Success', 'Product category updated successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Update Error', `Unable to update product category.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  deleteCategory(item: ProductCategory): void {
    this.alertService.showDialog('Are you sure you want to delete this product category?', DialogType.confirm, () => {
      this.alertService.startLoadingMessage();
      this.productEndpoint.getDeleteProductCategoryEndpoint<void>(item.id).subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.loadCategories();
          this.alertService.showMessage('Success', 'Product category deleted successfully.', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Delete Error', `Unable to delete product category.\r\nError: "${this.getErrorMessage(error)}"`, MessageSeverity.error, error);
        }
      });
    });
  }

  private applyFilter(): void {
    const term = this.searchText.trim().toLowerCase();
    if (!term) {
      this.filteredCategories = [...this.categories];
      return;
    }

    this.filteredCategories = this.categories.filter(x =>
      (x.name ?? '').toLowerCase().includes(term)
      || (x.description ?? '').toLowerCase().includes(term)
      || (x.icon ?? '').toLowerCase().includes(term)
    );
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
