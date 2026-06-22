# Bill Update Accounting Flow - Complete Reconciliation

## Overview
When a bill is updated for a `billNo`, the system performs a **complete inventory & accounting reconciliation**. Here's exactly how it's handled:

---

## The Update Flow (Line-by-Line from BillingService.UpdateAsync)

### **Phase 1: Data Retrieval** (Lines 112-114)
```csharp
var oldDetails = await context.BillingDetails
    .Where(x => x.billNO == normalizedBillNo)
    .ToListAsync();
```
- Fetch **ALL existing bill items** for this `billNo`
- These will be reversed (inventory restored, accounting deleted)

---

### **Phase 2: Delete Old Bill Details from DB** (Lines 119-125)
```csharp
context.BillingDetails.RemoveRange(oldDetails);      // Line 119: Remove from DB
if (normalizedDetails.Count > 0)
{
    context.BillingDetails.AddRange(normalizedDetails); // Line 122: Add new items
}
await context.SaveChangesAsync();                     // Line 125: Persist to DB
```

**Key Point:** At this stage:
- ✅ Old `BillingDetail` records are **deleted from database**
- ✅ New `BillingDetail` records are **inserted into database**
- ✅ **NO accounting changes yet** - we're still in the main DB transaction

---

### **Phase 3: Reverse OLD Product Inventory & Accounting** (Lines 127)
```csharp
await ReverseProductInventoryAsync(oldDetails, currentUserId);
```

**What Happens Inside `ReverseProductInventoryAsync`:**

```
For each OLD bill item:
  1. Check if Category == "Product"
  2. Get product by item name (drgName)
  3. Restore the quantity:
     - currentStock = 10 (current in DB)
     - qtyToRestore = 5 (original deduction)
     - newStock = 10 + 5 = 15

  4. Update Product table:
     - PreviousUnitsInStock = 10
     - UnitsInStock = 15
     - LastInventoryTranID = null (will be set to new tranID after delete)

  5. Call PostInventoryReversalAsync():
     └─ Inside InventoryAccountingService:

        ✅ STEP A: Retrieve the OLD TranID from Product.LastInventoryTranID
           └─ Example: "uuid-old-transaction-123"

        ✅ STEP B: Delete OLD accounting entry from Accounting DB
           └─ Call DeleteTranxaction SPROC
           └─ Removes: Debit COGS, Credit Inventory (from original deduction)
           └─ Result: Old transaction COMPLETELY REMOVED

        ✅ STEP C: Clear the TranID from product
           └─ Product.LastInventoryTranID = null
```

**Accounting State After Reversal:**
```
Product Table:
  - UnitsInStock: 15 (restored)
  - LastInventoryTranID: null (no active transaction)

Accounting DB:
  - Old transaction: DELETED ❌
  - Status: Clean slate, ready for new entry
```

---

### **Phase 4: Apply NEW Product Inventory & Accounting** (Lines 128)
```csharp
await UpdateProductInventoryAsync(normalizedDetails, currentUserId);
```

**What Happens Inside `UpdateProductInventoryAsync`:**

```
For each NEW bill item:
  1. Check if Category == "Product"
  2. Get product by item name (drgName)
  3. Deduct the new quantity:
     - currentStock = 15 (from previous reversal)
     - qtyToDeduct = 6 (NEW qty, may be different from old)
     - newStock = 15 - 6 = 9

  4. Update Product table:
     - PreviousUnitsInStock = 15
     - UnitsInStock = 9
     - LastInventoryTranID = "new-uuid-456" (will be set by accounting service)

  5. Call PostInventoryDeductionAsync():
     └─ Inside InventoryAccountingService:

        ✅ STEP A: Generate NEW TranID
           └─ Example: "uuid-new-transaction-456"

        ✅ STEP B: Insert NEW accounting entries to Accounting DB
           └─ Call InsertTranxaction SPROC (twice for debit/credit)
           └─ Debit: COGS account (AcctNo_COGS) = 6 * BuyingPrice
           └─ Credit: Inventory account (AcctNo_Inventory_Pharmacy) = -6 * BuyingPrice

        ✅ STEP C: Store new TranID in product
           └─ Product.LastInventoryTranID = "uuid-new-transaction-456"
```

**Accounting State After New Deduction:**
```
Product Table:
  - UnitsInStock: 9 (new stock level)
  - LastInventoryTranID: "uuid-new-transaction-456" (tracks current transaction)

Accounting DB:
  - Old transaction: GONE (deleted in Phase 3)
  - New transaction: INSERTED (fresh debit/credit entries)
  - Status: One clean entry representing current state ✅
```

---

## Complete Bill Update Example

### **Scenario: Update Bill #INV-2024-001**

**Original Bill (Before Update):**
```
BillNo: INV-2024-001
Item 1: Paracetamol (Product) × 5 units
  → Accounting: TranID "txn-old-001"
  → Stock: 20 → 15

Product State:
  UnitsInStock: 15
  LastInventoryTranID: "txn-old-001"
```

**User Updates Bill To:**
```
BillNo: INV-2024-001 (same)
Item 1: Paracetamol (Product) × 8 units (increased from 5)
```

**Update Process Execution:**

```
STEP 1: Fetch old bill details
        ↓ Found: Paracetamol × 5, TranID "txn-old-001"

STEP 2: Delete old BillingDetail, insert new BillingDetail
        ✓ DB updated

STEP 3: ReverseProductInventoryAsync(old details)
        ├─ Get Paracetamol from DB (currently UnitsInStock=15)
        ├─ Restore qty: 15 + 5 = 20
        ├─ Update Product: UnitsInStock=20, LastInventoryTranID=null
        └─ PostInventoryReversalAsync():
           ├─ Retrieve old TranID: "txn-old-001"
           ├─ Call DeleteTranxaction("txn-old-001")
           ├─ Accounting DB: Old entry DELETED ❌
           └─ Product: LastInventoryTranID=null

STEP 4: UpdateProductInventoryAsync(new details)
        ├─ Get Paracetamol from DB (now UnitsInStock=20)
        ├─ Deduct qty: 20 - 8 = 12
        ├─ Update Product: UnitsInStock=12
        └─ PostInventoryDeductionAsync():
           ├─ Generate new TranID: "txn-new-002"
           ├─ Call InsertTranxaction (debit COGS × 8 units)
           ├─ Call InsertTranxaction (credit Inventory × 8 units)
           ├─ Accounting DB: New entries INSERTED ✅
           └─ Product: LastInventoryTranID="txn-new-002"

STEP 5: Commit transaction
        ✓ All changes saved atomically
```

**Final State After Update:**
```
BillingDetail:
  billNO: INV-2024-001
  Item: Paracetamol × 8 units
  TranID: "txn-new-002" (new)

Product:
  UnitsInStock: 12 (new level)
  LastInventoryTranID: "txn-new-002" (tracks current transaction)

Accounting DB:
  Transactions:
    - "txn-old-001": DELETED
    - "txn-new-002": Debit COGS (8 × BuyingPrice)
    - "txn-new-002": Credit Inventory (8 × BuyingPrice)

  Result: Clean, accurate, no reversals or clutter ✅
```

---

## Key Design Decisions Explained

### **1. Why Delete Old Accounting First?**
- **Clean Reconciliation:** If you changed qty from 5 → 8, you don't want entries for both quantities cluttering the accounting DB
- **Atomicity:** SQL transaction wraps both operations; if deletion fails, no new entries are added
- **Traceability:** `Product.LastInventoryTranID` always points to ONE active transaction

### **2. How Accounting Stays Correct?**
| Operation | Product Stock | Accounting Entry | TranID Status |
|-----------|---------------|------------------|---------------|
| Initial Deduction (Create) | 20 → 15 | INSERT (qty=5) | Stored |
| Update to Qty=8 | 15 → 20 → 12 | DELETE old + INSERT new (qty=8) | Updated |
| Update to Qty=3 | 12 → 20 → 17 | DELETE old + INSERT new (qty=3) | Updated |
| Delete Bill | 17 → 20 | DELETE | Cleared |

**Result:** Accounting DB **always reflects the current reality**, not historical edits.

### **3. SQL Transaction Safety**
```csharp
await using var transaction = await context.Database.BeginTransactionAsync();
try
{
    // Phase 2: Update DB
    await context.SaveChangesAsync();

    // Phase 3: Reverse old inventory/accounting
    await ReverseProductInventoryAsync(oldDetails, currentUserId);

    // Phase 4: Update new inventory/accounting
    await UpdateProductInventoryAsync(normalizedDetails, currentUserId);

    // Phase 5: Cross-database sync
    await billingCrossDatabaseSyncService.SyncCreateOrUpdateAsync(...);

    // All-or-nothing commit
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();  // If ANY step fails, entire operation reverts
    throw;
}
```

**Guarantees:**
- ✅ If accounting posting fails, entire bill update is rolled back
- ✅ No orphaned BillingDetail records
- ✅ No mismatched product stock + accounting entries
- ✅ Atomicity across both databases (EMR + Accounting)

---

## Summary: The Accounting End During Bill Update

| Step | Action | Database | Result |
|------|--------|----------|--------|
| 1 | Fetch old bill items | EMR | `oldDetails` loaded |
| 2 | Delete old, insert new | EMR | BillingDetail updated |
| 3 | Reverse inventory | EMR Product | UnitsInStock restored |
| 4 | Delete old accounting | Accounting | Old TranID removed |
| 5 | Restore inventory | EMR Product | LastInventoryTranID cleared |
| 6 | Update inventory | EMR Product | New UnitsInStock set |
| 7 | Insert new accounting | Accounting | New TranID inserted |
| 8 | Store new TranID | EMR Product | LastInventoryTranID updated |
| 9 | Cross-DB sync | Both DBs | BillingDetail synced |
| 10 | Commit | Both DBs | Transaction finalized |

**End Result:** 
- 🎯 One accounting entry per bill state
- 🎯 Product stock always accurate
- 🎯 No clutter from historical changes
- 🎯 Full audit trail (TranID tracks the entry)
- 🎯 Atomic across both databases
