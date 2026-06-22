# Accounting Aspect for Product Inventory Updates

## Overview
Implemented automated accounting transactions for product inventory updates during billing operations. When bill items with category "Product" are saved, the system now posts accounting entries to debit COGS and credit Inventory accounts.

## Architecture

### New Components

#### 1. **IInventoryAccountingService Interface**
- **Location**: `AestheticEMR.Core/Services/Legacy/Interfaces/IInventoryAccountingService.cs`
- **Purpose**: Defines contract for posting inventory accounting transactions
- **Methods**:
  - `PostInventoryDeductionAsync()`: Posts debit to COGS, credit to Inventory when products are used
  - `PostInventoryReversalAsync()`: Posts credit to COGS, debit to Inventory when bills are updated

#### 2. **InventoryAccountingService Implementation**
- **Location**: `AestheticEMR.Core/Services/Legacy/InventoryAccountingService.cs`
- **Dependencies**:
  - `IConfiguration`: For connection strings
  - `IEmrAppDefaultsService`: For accounting configuration
  - `ILogger<InventoryAccountingService>`: For diagnostic logging

### Accounting Configuration

**Source**: `emrAppDefaults.json` (Values section)

#### Required Configuration Keys:
```json
{
  "Values": {
    "AcctPostOn": "true",                           // Enable accounting posting
    "AcctPostType_Inventory_Purchase": "AUTO",      // Auto-post inventory transactions
    "AcctCostCenter": "0001",                       // Cost center code
    "CoyID": "0001",                                // Company ID
    "AcctNo_COGS": "5110060",                       // Cost of Goods Sold account
    "AcctNo_Inventory_Pharmacy": "1260020",         // Inventory account
    "AcctPostType": "AUTO"                          // General posting type
  }
}
```

### Transaction Flow

#### **Deduction (Bill Creation/Update with New Items)**

1. **BillingService.UpdateProductInventoryAsync()**
   - Processes each billing detail with category "Product"
   - Updates `Product.UnitsInStock` (deducts quantity)
   - Calls `inventoryAccountingService.PostInventoryDeductionAsync()`

2. **InventoryAccountingService.PostInventoryDeductionAsync()**
   - **Validates** configuration gates (`AcctPostOn`, `AcctPostType_Inventory_Purchase`)
   - **Calculates** COGS amount: `Product.BuyingPrice × QuantityDeducted`
   - **Connects** to Accounting database
   - **Posts two entries** via `InsertTranxaction` stored procedure:
     - **Debit**: COGS account (positive amount)
     - **Credit**: Inventory account (negative amount)
   - **Balances** Dr = Cr before commit
   - **Logs** transaction details for audit trail

#### **Reversal (Bill Update with Item Removal)**

1. **BillingService.ReverseProductInventoryAsync()**
   - Processes old billing details before removal
   - Updates `Product.UnitsInStock` (restores quantity)
   - Calls `inventoryAccountingService.PostInventoryReversalAsync()`

2. **InventoryAccountingService.PostInventoryReversalAsync()**
   - **Validates** same configuration gates
   - **Calculates** COGS amount: `Product.BuyingPrice × QuantityRestored`
   - **Connects** to Accounting database
   - **Posts reverse entries** via `InsertTranxaction` stored procedure:
     - **Credit**: COGS account (negative amount - reversal)
     - **Debit**: Inventory account (positive amount - reversal)
   - **Logs** reversal transaction details

### Integration Points

#### **BillingService Changes**

1. **Constructor Injection**:
   ```csharp
   public class BillingService(
       // ... existing dependencies ...
       IInventoryAccountingService inventoryAccountingService) : IBillingService
   ```

2. **CreateAsync() Method**:
   - Calls `UpdateProductInventoryAsync()` after saving details
   - Accounting posts occur within same transaction scope

3. **UpdateAsync() Method**:
   - Calls `ReverseProductInventoryAsync()` for old details (reversals)
   - Calls `UpdateProductInventoryAsync()` for new details (deductions)
   - Ensures inventory properly recalculated when bills modified

#### **Dependency Registration** (Program.cs)
```csharp
builder.Services.AddScoped<IInventoryAccountingService, InventoryAccountingService>();
```

### Stored Procedure Integration

**Stored Procedure**: `InsertTranxaction`

**Parameters Used**:
- `@TranID`: Unique transaction ID (GUID)
- `@AccountNo`: Target account number (COGS or Inventory)
- `@TranNo`: Transaction number (same as TranID)
- `@TranDate`: Transaction date (Today)
- `@CostCenterID`: Cost center from configuration
- `@Amount`: Positive (debit) or negative (credit) amount
- `@Description`: Transaction description with product name and bill number
- `@TranCat`: "b" (inventory transaction category)
- `@EntryDate`: Entry timestamp
- `@Period`: MM/YYYY format period
- `@CoyID2`: Company ID
- `@UserName`: "system"
- `@SNoID`: 0
- `@BillNO`: Bill number reference
- `@Reversed`: false
- `@ReversedPair`: 0

### Error Handling & Resilience

1. **Configuration Validation**:
   - Checks all required accounts are configured
   - Validates posting flags before connecting to database
   - Logs and skips posting if configuration incomplete

2. **Transaction Management**:
   - SQL transaction with automatic rollback on failure
   - Balanced Dr = Cr enforcement
   - Graceful failure mode: errors logged, system continues

3. **Logging**:
   - INFO: Successful posts with account details and amounts
   - WARNING: Configuration issues or missing data
   - ERROR: Database failures with context

### Workflow Example

**Scenario**: User saves billing invoice with 2 Product items

1. **CreateAsync() Called**:
   - Saves Billing header
   - Saves 2 BillingDetails (both category="Product")
   - Calls `UpdateProductInventoryAsync()`

2. **UpdateProductInventoryAsync() Processes**:
   - Item 1: "Paracetamol" (Qty: 5)
     - Updates Product.UnitsInStock: 50 → 45
     - Calls `PostInventoryDeductionAsync()` with Qty=5
   - Item 2: "Antibiotic" (Qty: 2)
     - Updates Product.UnitsInStock: 20 → 18
     - Calls `PostInventoryDeductionAsync()` with Qty=2

3. **PostInventoryDeductionAsync() Posts** (for each item):
   - Debit COGS: 5 × $10 = $50 (Paracetamol)
   - Credit Inventory: -$50
   - Debit COGS: 2 × $25 = $50 (Antibiotic)
   - Credit Inventory: -$50
   - Total accounting entries: 4 (2 pairs of Dr/Cr)

### Considerations

- **Cost Valuation**: Uses `Product.BuyingPrice` as cost basis for COGS calculation
- **Audit Trail**: Each transaction includes bill number and product details
- **Negative Stock**: Prevented at product level (Math.Max(0, newStock))
- **No Cross-Database Conflicts**: Accounting posts occur in dedicated transaction after billing save
- **Frequency**: Executes only when `AcctPostOn=true` and `AcctPostType_Inventory_Purchase=AUTO`

### Testing Checklist

- [ ] Bill creation with Product items posts accounting correctly
- [ ] Bill update with removed items posts reversals
- [ ] Bill update with replaced items posts both reversals and new deductions
- [ ] Configuration validation prevents posting with incomplete settings
- [ ] Accounting period balances (Dr = Cr) verified before commit
- [ ] Audit logs capture all transactions with amounts and references
- [ ] Negative stock prevented at product level
- [ ] Error handling graceful with proper logging
