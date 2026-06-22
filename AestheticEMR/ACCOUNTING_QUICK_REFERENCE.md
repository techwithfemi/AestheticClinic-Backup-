# Quick Reference: Inventory Accounting Implementation

## Files Modified/Created

### New Files:
1. `AestheticEMR.Core/Services/Legacy/Interfaces/IInventoryAccountingService.cs`
2. `AestheticEMR.Core/Services/Legacy/InventoryAccountingService.cs`
3. `AestheticEMR/ACCOUNTING_INVENTORY_IMPLEMENTATION.md` (this documentation)

### Modified Files:
1. `AestheticEMR.Core/Services/Legacy/BillingService.cs`
   - Added `IInventoryAccountingService` injection
   - Updated `UpdateProductInventoryAsync()` to post accounting
   - Updated `ReverseProductInventoryAsync()` to post reversals

2. `AestheticEMR.Core/Services/Shop/ProductService.cs`
   - Fixed syntax error in lambda expression (line 420)

3. `AestheticEMR.Core/Services/Shop/Interfaces/IProductService.cs`
   - Added `GetByNameAsync(string name)` method

4. `AestheticEMR.Server/Program.cs`
   - Registered `IInventoryAccountingService` in dependency injection

## Key Features

✅ **Automatic COGS/Inventory Posting**
- Debits COGS account when products used in billing
- Credits Inventory account with corresponding amount
- Uses product buying price for cost calculation

✅ **Reversals on Bill Updates**
- Automatically reverses accounting when bill items removed
- Credits COGS and debits Inventory for reversal
- Maintains balanced accounting entries

✅ **Configuration-Driven**
- Respects `AcctPostOn` flag in emrAppDefaults.json
- Requires `AcctPostType_Inventory_Purchase = AUTO`
- Uses configured account numbers for COGS and Inventory

✅ **Transaction Safety**
- SQL transactions with rollback on failure
- Balance verification before commit (Dr = Cr)
- Graceful degradation if posting fails

✅ **Full Audit Trail**
- Logs all posting attempts with amounts
- Includes bill number and product details
- Tracks reversals separately

## Configuration Required

### emrAppDefaults.json (Values section):

```json
{
  "AcctPostOn": "true",
  "AcctPostType_Inventory_Purchase": "AUTO",
  "AcctNo_COGS": "5110060",
  "AcctNo_Inventory_Pharmacy": "1260020",
  "AcctCostCenter": "0001"
}
```

## How It Works

### Bill Save with Product Items:
1. User saves invoice with items (Category = "Product")
2. `BillingService.CreateAsync()` saves billing details
3. `UpdateProductInventoryAsync()` called
4. For each product item:
   - Product stock deducted: `UnitsInStock -= Qty`
   - Accounting posted: COGS debited, Inventory credited
   - Both entries use `InsertTranxaction` sproc in accounting DB

### Bill Update with Item Removal:
1. User modifies invoice (removes some items)
2. `BillingService.UpdateAsync()` retrieves old details
3. `ReverseProductInventoryAsync()` called for old items
4. For each removed product:
   - Product stock restored: `UnitsInStock += Qty`
   - Accounting reversed: COGS credited, Inventory debited
5. `UpdateProductInventoryAsync()` called for new items
6. Repeat posting for new items

## Error Handling

| Scenario | Behavior |
|----------|----------|
| AcctPostOn = false | Posting skipped, logged as INFO |
| AcctPostType_Inventory_Purchase ≠ AUTO | Posting skipped, logged as INFO |
| Missing COGS account | Warning logged, posting skipped |
| Missing Inventory account | Warning logged, posting skipped |
| No accounting connection | Warning logged, posting skipped |
| Database error | Error logged, exception thrown |

## Logging

All operations logged with:
- **Level**: INFO (success), WARNING (configuration), ERROR (failure)
- **Scope**: BillNo, Product name, Quantities involved
- **Details**: Account numbers, amounts, dates

Example log:
```
Inventory deduction for BillNo INV-2025-001, Product Paracetamol (Qty: 5): 
posted to accounting (debit 5110060, credit 1260020, amount 50).
```

## Testing

### Unit Test Scenario 1: Create Invoice with Products
```
Given: Bill with 2 product items (Qty: 5, 2)
When: CreateAsync() called
Then: 
  - Products updated in Product table
  - 4 accounting entries posted (2 pairs)
  - Dr = Cr balanced
```

### Unit Test Scenario 2: Update Invoice Remove Items
```
Given: Existing bill with 2 items, updating to 1 item
When: UpdateAsync() called
Then:
  - Old item: Reversal posting (reversed Dr/Cr)
  - Old item: Stock restored
  - New items: Deduction posting
  - All accounting balanced
```

## Dependencies

- **IProductService**: Get products by name, update stock
- **IEmrAppDefaultsService**: Read accounting configuration
- **IConfiguration**: Read connection strings
- **ILogger**: Diagnostic logging
- **SqlConnection/SqlTransaction**: Direct accounting DB access

## Notes

- Product lookup is **case-insensitive** (uses product Name field)
- Stock can never go **negative** (Math.Max enforces minimum 0)
- Accounting posts use **product.BuyingPrice** for COGS calculation
- **PreviousUnitsInStock** maintains audit trail of stock changes
- **TranCat = "b"** for all inventory transactions (category code)
