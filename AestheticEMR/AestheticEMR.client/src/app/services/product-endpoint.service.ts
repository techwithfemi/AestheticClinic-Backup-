import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ProductCategoryEdit, ProductEdit } from '../models/shop/product.model';

@Injectable({ providedIn: 'root' })
export class ProductEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get productsUrl() { return `${this.configurations.baseUrl}/api/product`; }

  getProductsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.productsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getProductsEndpoint<T>()))
    );
  }

  getProductCategoriesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.productsUrl}/categories`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getProductCategoriesEndpoint<T>()))
    );
  }

  getNewProductEndpoint<T>(model: ProductEdit): Observable<T> {
    return this.http.post<T>(this.productsUrl, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewProductEndpoint<T>(model)))
    );
  }

  getUpdateProductEndpoint<T>(id: number, model: ProductEdit): Observable<T> {
    return this.http.put<T>(`${this.productsUrl}/${id}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateProductEndpoint<T>(id, model)))
    );
  }

  getDeleteProductEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.productsUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteProductEndpoint<T>(id)))
    );
  }

  getProductCategoryByIdEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.productsUrl}/categories/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getProductCategoryByIdEndpoint<T>(id)))
    );
  }

  getNewProductCategoryEndpoint<T>(model: ProductCategoryEdit): Observable<T> {
    return this.http.post<T>(`${this.productsUrl}/categories`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getNewProductCategoryEndpoint<T>(model)))
    );
  }

  getUpdateProductCategoryEndpoint<T>(id: number, model: ProductCategoryEdit): Observable<T> {
    return this.http.put<T>(`${this.productsUrl}/categories/${id}`, JSON.stringify(model), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateProductCategoryEndpoint<T>(id, model)))
    );
  }

  getDeleteProductCategoryEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.productsUrl}/categories/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteProductCategoryEndpoint<T>(id)))
    );
  }
}
