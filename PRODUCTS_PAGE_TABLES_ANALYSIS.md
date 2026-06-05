# Inventory Module - Products Page CRUD Operations

## Overview
The Products page in the Inventory module manages tariff products with their pricing, categories, and stock information.

---

## Tables Involved in CRUD Operations

### **Primary Tables**

#### 1. **Product** (Main Entity)
- **Location**: `AestheticEMR.Core/Models/Shop/Product.cs`
- **Database Table**: `AppProducts`
- **Purpose**: Stores product/item information
- **Key Fields**:
  - `Id` (Primary Key, inherited from BaseEntity)
  - `Name` - Product name
  - `Description` - Product description
  - `Icon` - Product icon/image
  - `BuyingPrice` - Cost price (Buying column in UI)
  - `SellingPrice` - Sale price (Selling column in UI)
  - `UnitsInStock` - Stock quantity (Stock column in UI)
  - `IsActive` - Product status
  - `IsDiscontinued` - Discontinued flag
  - `ProductCategoryId` - Foreign key to ProductCategory
  - `ParentId` - For hierarchical product structure (nullable)
  - `CreatedBy`, `UpdatedBy`, `CreatedDate`, `UpdatedDate` (Audit fields from BaseEntity)

**CRUD Operations**:
- ✅ **Create**: `POST /api/product` → Creates new product
- ✅ **Read**: `GET /api/product` → Lists all products
- ✅ **Read**: `GET /api/product/{id}` → Gets single product
- ✅ **Update**: `PUT /api/product/{id}` → Updates product details
- ✅ **Delete**: `DELETE /api/product/{id}` → Deletes product

---

#### 2. **ProductCategory** (Related Entity)
- **Location**: `AestheticEMR.Core/Models/Shop/ProductCategory.cs`
- **Database Table**: `AppProductCategories`
- **Purpose**: Categories for grouping products
- **Key Fields**:
  - `Id` (Primary Key)
  - `Name` - Category name
  - `Description` - Category description
  - `Icon` - Category icon
  - Navigation property: `Products` collection

**CRUD Operations**:
- ✅ **Create**: `POST /api/product/categories` → Creates new category
- ✅ **Read**: `GET /api/product/categories` → Lists all categories
- ✅ **Read**: `GET /api/product/categories/{id}` → Gets single category
- ✅ **Update**: `PUT /api/product/categories/{id}` → Updates category
- ✅ **Delete**: `DELETE /api/product/categories/{id}` → Deletes category

**Relationship**: `Product.ProductCategoryId` → `ProductCategory.Id` (Foreign Key)

---

### **Secondary/Related Tables**

#### 3. **ProductTariff** (Legacy Table)
- **Location**: `AestheticEMR.Core/Models/Legacy/ProductTariff.cs`
- **Database Table**: `AppProductTariffs`
- **Purpose**: Legacy tariff pricing information
- **Key Fields**:
  - `SNO` - Auto-increment ID
  - `PdtName` - Product name
  - `Category` - Product category
  - `Company` (CoyID) - Company/Organization ID
  - `Price` - Tariff price
  - `CoyName` - Company name
  - `Capitated` - Capitation flag
  - `TariffStatus` - Status (FIXED/VARIABLE)
  - `RevType` - Revenue type
  - `UsersCat` - User category

**Usage**: Referenced in API endpoint but not directly manipulated in standard product CRUD

---

#### 4. **ProductStockReport** (Reporting)
- **Database Table**: `AppProductStockReports`
- **Purpose**: Stock level tracking and reporting
- **Related Endpoint**: `GET /api/product/stock-report`
- **Relationship**: `ProductStockReport.ProductId` → `Product.Id`

**CRUD Operations**:
- ✅ **Read**: `GET /api/product/stock-report` → Gets stock report data

---

#### 5. **ProductBatch** (Batch Management)
- **Database Table**: `AppProductBatches`
- **Purpose**: Track product batches with expiry dates, batch numbers
- **Related Endpoints**:
  - `GET /api/product/batches` → Gets batches (optional filter by productId)
  - `GET /api/product/batches/{id}` → Gets single batch
  - `POST /api/product/batches` → Create batch
  - `PUT /api/product/batches/{id}` → Update batch
  - `DELETE /api/product/batches/{id}` → Delete batch

**Relationship**: `ProductBatch.ProductId` → `Product.Id`

---

#### 6. **OrderDetail** (Related Through Product)
- **Purpose**: Contains products ordered via products table
- **Relationship**: `OrderDetail.ProductId` → `Product.Id`

---

#### 7. **ProcedureProductUsage** (Consumption Tracking)
- **Purpose**: Tracks product usage in procedures
- **Relationship**: `ProcedureProductUsage.ProductId` → `Product.Id`

---

## Database Schema Relationships

```
ProductCategory
    ↑
    │ (ProductCategoryId - FK)
    │
Product ←→ ProductBatch
    ↓         ↓
OrderDetail  StockReport
    ↓
ProcedureProductUsage
```

---

## API Endpoints Summary

### Product Endpoints
| Method | Endpoint | Table(s) | Operation |
|--------|----------|----------|-----------|
| GET | `/api/product` | Product | Read All |
| GET | `/api/product/{id}` | Product | Read One |
| POST | `/api/product` | Product | Create |
| PUT | `/api/product/{id}` | Product | Update |
| DELETE | `/api/product/{id}` | Product | Delete |
| GET | `/api/product/stock-report` | ProductStockReport | Read Report |

### Category Endpoints
| Method | Endpoint | Table(s) | Operation |
|--------|----------|----------|-----------|
| GET | `/api/product/categories` | ProductCategory | Read All |
| GET | `/api/product/categories/{id}` | ProductCategory | Read One |
| POST | `/api/product/categories` | ProductCategory | Create |
| PUT | `/api/product/categories/{id}` | ProductCategory | Update |
| DELETE | `/api/product/categories/{id}` | ProductCategory | Delete |

### Batch Endpoints
| Method | Endpoint | Table(s) | Operation |
|--------|----------|----------|-----------|
| GET | `/api/product/batches` | ProductBatch | Read All |
| GET | `/api/product/batches/{id}` | ProductBatch | Read One |
| POST | `/api/product/batches` | ProductBatch | Create |
| PUT | `/api/product/batches/{id}` | ProductBatch | Update |
| DELETE | `/api/product/batches/{id}` | ProductBatch | Delete |

---

## UI Display Fields (From Screenshot)
The Products page displays:
- **Name** - From Product.Name
- **Category** - From Product.ProductCategory.Name
- **Buying** - From Product.BuyingPrice
- **Selling** - From Product.SellingPrice
- **Stock** - From Product.UnitsInStock
- **Actions** - Edit/Delete buttons

---

## Key Points
1. **Product** is the core table managing inventory items
2. **ProductCategory** provides categorization (referenced in foreign key)
3. **ProductStockReport** provides aggregate stock data
4. **ProductBatch** manages batches with expiry tracking
5. **ProductTariff** is legacy data not directly involved in current CRUD
6. All entities inherit from **BaseEntity** providing `Id`, `CreatedBy`, `UpdatedBy`, `CreatedDate`, `UpdatedDate`

