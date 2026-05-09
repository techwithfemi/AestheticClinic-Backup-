export interface Product {
  id: number;
  name?: string;
  description?: string;
  icon?: string;
  buyingPrice: number;
  sellingPrice: number;
  unitsInStock: number;
  isActive: boolean;
  isDiscontinued: boolean;
  productCategoryName?: string;
}

export interface ProductEdit {
  id: number;
  name: string;
  description?: string;
  icon?: string;
  buyingPrice: number;
  sellingPrice: number;
  unitsInStock: number;
  isActive: boolean;
  isDiscontinued: boolean;
  productCategoryId: number;
}

export interface ProductCategory {
  id: number;
  name: string;
  description?: string;
  icon?: string;
}

export interface ProductCategoryEdit {
  id: number;
  name: string;
  description?: string;
  icon?: string;
}
