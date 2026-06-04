# Debt Carry-Forward Logic Documentation

## Overview

This document explains the debt carry-forward mechanism in AestheticClinic and provides tools to verify, validate, and fix debt calculations.

## Key Concepts

### 1. What is Debt Carry-Forward?

Debt carry-forward (DebtBF) is the mechanism that tracks unpaid balances from previous invoices and carries them forward to subsequent invoices for **private patients only**.

**Formula for Balance Due:**
```
Balance Due = ((AmountBilled - Discount) + DebtBF + Tax) - AmountPaid
```

### 2. Who Gets Debt Carry-Forward?

**Private Patients Only** (identified by `HRetainership.RetainCode = "0001"`)

- Patient lookup: `HPatient.CoyName` → `HRetainership.RetainCode`
- If `RetainCode = "0001"` → Patient IS Private
- Corporate and HMO patients do NOT have debt carry-forward

### 3. Debt Calculation Flow

#### First Invoice (SeqNum = 1)
```
DebtBF = 0  (Always zero for first invoice)
Balance = ((AmountBilled - Discount) + 0 + Tax) - AmountPaid
```

#### Second+ Invoice (SeqNum > 1)
```
DebtBF = Previous Invoice's Balance
Balance = ((AmountBilled - Discount) + DebtBF + Tax) - AmountPaid
```

### 4. Example Scenario

**Patient:** P001 (Private, RetainCode = 0001)

| Invoice | Date | Billed | Discount | Tax | Paid | DebtBF | Balance |
|---------|------|--------|----------|-----|------|--------|---------|
| INV001 | 2024-01-15 | 100.00 | 0.00 | 0.00 | 0.00 | 0.00 | **100.00** |
| INV002 | 2024-01-20 | 50.00 | 0.00 | 0.00 | 30.00 | **100.00** | **120.00** |
| INV003 | 2024-01-25 | 75.00 | 10.00 | 0.00 | 50.00 | **120.00** | **135.00** |

- INV001: Starts with 0 debt, customer owes 100
- INV002: Carries forward 100 debt from INV001 + 50 billed - 30 paid = 120 owed
- INV003: Carries forward 120 debt from INV002 + 75 billed - 10 discount - 50 paid = 135 owed

## Database Tables

### HPatients
```
Pno              (Patient Number) - PK
CoyName          (Links to HRetainership.RetainCode)
DebtBf           (Current debt brought forward)
Debt             (Current debt - same as DebtBf)
IsRev            (Reversal flag)
```

### HRetainership
```
RetainCode       (Unique code - "0001" = Private)
RetainName       (Retainership name)
RetainId         (ID)
ClientType       (e.g., "PRIVATE", "CORPORATE")
```

### Billings
```
ID               (PK - Auto increment)
billNO           (Invoice Number - matches HRecord.ConsultId)
pNo              (Patient Number - FK to HPatients)
bDate            (Billing Date)
AmountBilled     (Amount charged)
Discount         (Discount applied)
Tax              (VAT/Tax)
AmountPaid       (Amount paid)
DebtBF           (Debt Brought Forward from previous invoice)
isPaid           (Whether invoice is fully paid)
```

## Code Implementation

### Backend: AttendanceService.cs

**Method: `SaveDebtAsync(pNo, currentBillNo)`**

This method is called when attendance is recorded:

1. Finds the most recent previous bill for the patient
2. Checks if patient is private (via retainership lookup)
3. If private, calculates: `openBal = ((billed - discount) + debtBf + tax) - paid`
4. Updates `HPatient.DebtBf = openBal`
5. This value is then used when creating the billing record

**Method: `SaveBillAsync(record)`**

This method creates the billing record:

1. Reads `HPatient.DebtBf` (set by SaveDebtAsync)
2. Creates `Billing` record with `DebtBF = patient.DebtBf`
3. If billing already exists (edge case), updates the `DebtBF`

## SQL Scripts for Validation & Repair

### 1. RecalculatePatientDebt.sql

**Purpose:** Recalculate and fix debt for a single patient

**Usage:**
```sql
-- Check debt history (dry run - safe)
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;

-- Apply corrections
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 0;
```

**What it does:**
- Validates patient exists
- Checks if patient is private
- Recalculates correct DebtBF for each invoice in chronological order
- Shows what needs to be corrected
- Applies corrections to Billings table and HPatients table

### 2. RecalculateAllPatientDebt.sql

**Purpose:** Identify and fix debt for ALL private patients

**Usage:**
```sql
-- Generate report of patients with issues (safe)
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;

-- Apply corrections to all patients with issues
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```

**What it does:**
- Scans all private patients with billings
- Identifies those with incorrect DebtBF values
- Generates a detailed report
- Optionally applies corrections to all patients

### 3. ValidateDebtCarryForward.sql

**Purpose:** Quick validation queries

**Contains:**
- Query 1: Check specific patient's debt history
- Query 2: Find all private patients with outstanding debt
- Query 3: Find potential debt carry-forward errors
- Query 4: Summary statistics

## Common Debt Issues & Fixes

### Issue 1: DebtBF Shows 0 for Second Invoice But Should Show Previous Balance

**Cause:** `SaveDebtAsync` not executed or private patient check failed

**Fix:**
```sql
EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NO', @dryRun = 0;
```

### Issue 2: Multiple Patients Have Incorrect Debt

**Cause:** Systemic issue with debt calculation logic

**Fix:**
```sql
-- First, check the scope
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;

-- Then apply fixes
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```

### Issue 3: Corporate Patient Has Debt (Should Be 0)

**Cause:** Patient classification issue or wrong retainership

**Fix:**
1. Verify patient's `HPatient.CoyName` value
2. Check that `HRetainership` has `RetainCode != '0001'`
3. If needed, correct patient's CoyName to appropriate corporate code

## Testing Checklist

- [ ] New private patient with first invoice → DebtBF should be 0.00
- [ ] Same patient with second invoice → DebtBF should equal first invoice's balance
- [ ] Corporate patient with unpaid invoice → DebtBF should be 0.00 in all subsequent invoices
- [ ] Run `sp_RecalculatePatientDebt` on known-good patient → Report should show all records correct
- [ ] Run `sp_RecalculateAllPatientDebt` → Should report no issues for healthy database

## Related Code Files

- **Backend Service:** `/AestheticEMR.Core/Services/Legacy/AttendanceService.cs`
  - Method: `CreateAsync(HRecord)`
  - Method: `SaveDebtAsync(pNo, currentBillNo)`
  - Method: `SaveBillAsync(record)`

- **Frontend Component:** `/AestheticEMR.client/src/app/features/billing/invoices/invoices.component.ts`
  - Method: `getBalance(invoice)` - Calculates displayed balance

- **Database Models:**
  - `/AestheticEMR.Core/Models/Legacy/HPatient.cs`
  - `/AestheticEMR.Core/Models/Legacy/HRetainership.cs`
  - `/AestheticEMR.Core/Models/Legacy/Billing.cs`

## Troubleshooting

### Q: How do I know if a patient's debt is wrong?

A: Run the validation queries in `ValidateDebtCarryForward.sql` - it will highlight errors.

### Q: Can I test the fix without applying it?

A: Yes! All procedures support `@dryRun` or `@applyChanges` parameters. Always run with these flags set to 0 or false first to see what changes will be made.

### Q: What if I accidentally applied corrections?

A: Check your backup. The script logs all changes. If needed, you can manually revert by providing the correct DebtBF values from your records.

### Q: Why is the debt calculation showing in the invoices page?

A: The frontend `getBalance()` method calculates: `debtBF + amountBilled + tax - discount - amountPaid`. Ensure the backend is correctly populating `DebtBF` in the Billing record.

## Performance Considerations

- For large patient databases, run `sp_RecalculateAllPatientDebt` during low-usage periods
- The procedures use indexed queries on `billNO`, `pNo`, and `bDate`
- Consider running annually or after major system changes

## Related Issues & PRs

- Issue: "Invoices page debt column showing 0 when patient had unpaid transaction"
- Fix: Corrected `SaveDebtAsync` to properly identify private patients via retainership lookup
- Fix: Enhanced `SaveBillAsync` to update existing billing records with latest debt

---

**Last Updated:** 2024
**Database:** SQL Server
**Target Framework:** .NET 10
