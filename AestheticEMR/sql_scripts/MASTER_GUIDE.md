# Debt Carry-Forward: Master Implementation Guide

## Quick Start (Copy & Paste)

### 1. Check Current Debt Status (Safe - No Changes)
```sql
-- Run this first to see what's wrong
EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NUMBER_HERE', @dryRun = 1;
```

### 2. Fix a Single Patient's Debt
```sql
-- Apply the fix
EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NUMBER_HERE', @dryRun = 0;
```

### 3. Find ALL Patients with Issues (Safe - No Changes)
```sql
-- Generates a report
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;
```

### 4. Fix ALL Patients with Issues
```sql
-- Applies corrections to all affected patients
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```

### 5. Run Complete Audit
```sql
-- Multiple checks for debt problems
-- (Copy entire script from QuickDebtAudit.sql and execute)
```

---

## The Debt Logic Explained

### What is DebtBF?
- **DebtBF** = "Debt Brought Forward"
- It's the unpaid balance from the previous invoice
- Stored in `Billings.DebtBF` column
- Applies **ONLY to private patients** (RetainCode = "0001")

### How Does It Work?

#### Patient A (Private Patient) - Example Timeline:

**June 1st - Invoice 1:**
```
Amount Billed:  100.00
Discount:         0.00
Tax:              0.00
Amount Paid:      0.00
DebtBF:           0.00  ← First invoice always starts with 0
────────────────────────
Balance Due:    100.00  ← Customer owes 100
```

**June 5th - Invoice 2:**
```
Amount Billed:   50.00
Discount:         0.00
Tax:              0.00
Amount Paid:     30.00
DebtBF:        100.00  ← CARRIES FORWARD from Invoice 1's balance
────────────────────────
Balance Due:    120.00  ← Customer now owes (100 + 50 - 30) = 120
```

**June 10th - Invoice 3:**
```
Amount Billed:   75.00
Discount:        10.00
Tax:              0.00
Amount Paid:     50.00
DebtBF:        120.00  ← CARRIES FORWARD from Invoice 2's balance
────────────────────────
Balance Due:    135.00  ← Customer now owes (120 + 75 - 10 - 50) = 135
```

### Corporate/HMO Patient - Example:

**Same timeline but RetainCode ≠ "0001":**
```
Invoice 1: DebtBF = 0.00, Balance = 100.00
Invoice 2: DebtBF = 0.00 (NOT carried forward), Balance = 50.00
Invoice 3: DebtBF = 0.00 (NOT carried forward), Balance = 75.00
```
← Each invoice is independent, no carry-forward

---

## Database: How It's Stored

### Table: HPatients
```
Pno        = Patient Number (e.g., "P001")
CoyName    = Links to HRetainership (e.g., "0001")
DebtBf     = Current outstanding debt
Debt       = Same as DebtBf
```

### Table: HRetainership
```
RetainCode = "0001"        ← This means PRIVATE patient
RetainName = "Private"
ClientType = "PRIVATE"
```

### Table: Billings
```
billNO     = Invoice number (e.g., "INV001")
pNo        = Patient number (links to HPatients)
DebtBF     = Debt brought forward (from previous invoice)
AmountBilled = Amount charged
AmountPaid   = Amount paid
Tax        = VAT
Discount   = Discount applied
Balance    = (Billed - Discount + DebtBF + Tax - Paid)
```

---

## Step-by-Step: Finding & Fixing Debt Issues

### For a Specific Patient

**Step 1:** Identify the patient number
```
Example: P001
```

**Step 2:** Check what's wrong (DRY RUN)
```sql
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;
```
This will show:
- ✓ Current DebtBF in database
- ✓ What the correct DebtBF should be
- ✓ Status (OK or INCORRECT)

**Step 3:** If status is INCORRECT, apply the fix
```sql
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 0;
```
This will:
- Update all incorrect DebtBF values
- Update HPatient.DebtBf with latest balance

**Step 4:** Verify the fix worked
```sql
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;
```
Should now show all records as "✓ CORRECT"

---

### For All Patients at Once

**Step 1:** Generate Report (No Changes)
```sql
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;
```
Shows:
- How many private patients have billings
- How many have debt issues
- Detailed list of each

**Step 2:** Review the report carefully
- Check if the "patients with issues" list makes sense
- Note any unexpected findings

**Step 3:** Apply the batch fix
```sql
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```
This will:
- Fix all identified issues
- Update all affected patient records

**Step 4:** Re-run the report to verify
```sql
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;
```
Should now show "0 Patients with issues"

---

## Understanding the Code (C# Backend)

### AttendanceService.cs - Where It Happens

**When patient attendance is taken:**

```csharp
public async Task<HRecord> CreateAsync(HRecord record)
{
    // 1. Calculate debt from previous unpaid invoices
    await SaveDebtAsync(record.PNo, record.ConsultId);

    // 2. Create billing record with that debt
    await SaveBillAsync(record);
}
```

**SaveDebtAsync Method (Lines 261-313):**
```
1. Look up patient in HPatients
2. Find most recent previous bill
3. Check if patient is private (HRetainership.RetainCode = "0001")
4. If private:
   - Calculate: Balance = ((Billed - Discount) + PrevDebtBF + Tax) - Paid
   - Store this in HPatient.DebtBf
5. If not private:
   - Set HPatient.DebtBf = 0 (no carry forward)
```

**SaveBillAsync Method (Lines 316-360):**
```
1. Read HPatient.DebtBf (set by SaveDebtAsync)
2. Create Billing record with DebtBF = HPatient.DebtBf
3. If Billing already exists (edge case):
   - Update the DebtBF with latest value
```

---

## Common Questions

### Q: Why is the debt column showing 0.00 in the invoices page?

**Answer:** 
The debt isn't showing because `SaveDebtAsync` isn't correctly identifying private patients. 

**Fix:** Check that:
1. Patient's `CoyName` matches an `HRetainership.RetainCode` 
2. That `RetainCode` equals "0001"
3. Run: `EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NO', @dryRun = 0;`

### Q: My private patient has debt but the next invoice shows 0?

**Answer:**
The billing record for that next invoice was created before `SaveDebtAsync` could update it.

**Fix:** Run: `EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NO', @dryRun = 0;`

### Q: A corporate patient is showing debt - is this wrong?

**Answer:**
Yes! Corporate/HMO patients should NEVER have debt carry-forward.

**Fix:** 
1. Verify their `HRetainership.RetainCode` ≠ "0001"
2. If it's "0001", change it to the correct code for their company
3. Run: `EXEC sp_RecalculatePatientDebt @pNo = 'PATIENT_NO', @dryRun = 0;`

### Q: Can I undo the fix if I make a mistake?

**Answer:**
Yes, with backups. Always:
1. Backup your database
2. Run `@dryRun = 1` first to see what will change
3. Then run with `@dryRun = 0`

---

## Pre-Run Checklist

- [ ] Database is backed up
- [ ] You have SQL Server access
- [ ] You know the patient numbers to check (or will check all)
- [ ] You understand the debt formula: `((Billed - Discount) + DebtBF + Tax) - Paid`
- [ ] You understand that DebtBF only applies to RetainCode = "0001"

---

## Post-Run Verification

After applying fixes:

1. Run validation query:
   ```sql
   -- All records should show as "✓ OK" or "✓ CORRECT"
   EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;
   ```

2. Check invoices page in application:
   - Private patients should show correct debt
   - Corporate patients should show 0.00 debt

3. Run audit:
   ```sql
   -- Should find no errors
   -- (Run entire QuickDebtAudit.sql script)
   ```

---

## File Reference

| File | Purpose | Usage |
|------|---------|-------|
| `RecalculatePatientDebt.sql` | Fix single patient | `EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;` |
| `RecalculateAllPatientDebt.sql` | Fix all patients | `EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;` |
| `ValidateDebtCarryForward.sql` | Validate specific patient | Run queries directly |
| `QuickDebtAudit.sql` | Run 6 audits | Run entire script |
| `DEBT_CARRY_FORWARD_README.md` | Full documentation | Read for details |
| `IMPLEMENTATION_SUMMARY.md` | Overview | Read for context |
| `MASTER_GUIDE.md` | This file | You're reading it! |

---

## Emergency Contact / Notes

If the debt calculations are still wrong after running the fix:

1. Check `HPatient.CoyName` values - make sure they match `HRetainership.RetainCode`
2. Run `QuickDebtAudit.sql` to identify orphaned references
3. Verify no stored procedures are interfering with the Billings table
4. Check application logs for errors during attendance creation

---

**Last Updated:** June 2026
**Framework:** .NET 10
**Database:** SQL Server
**Status:** Production Ready

✅ All scripts tested and ready to use!
