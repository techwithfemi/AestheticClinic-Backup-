# Debt Carry-Forward: Visual Reference

## 1. Database Relationship Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     HPatients (Patient Master)              │
├─────────────────────────────────────────────────────────────┤
│ Pno (PK)           = "P001"                                 │
│ PSurName           = "Babafemi"                             │
│ PFirstname         = "Omage"                                │
│ CoyName            = "0001" ────────────────┐              │
│ DebtBf             = 120.00 (current debt)  │              │
│ Debt               = 120.00 (same as DebtBf)              │
│ IsRev              = true (reversed flag)   │              │
└─────────────────────────────────────────────┼──────────────┘
                                              │
                                              │ Foreign Key
                                              ▼
┌─────────────────────────────────────────────────────────────┐
│              HRetainership (Patient Classification)         │
├─────────────────────────────────────────────────────────────┤
│ RetainCode (PK)    = "0001" ◄─ PRIVATE PATIENT             │
│ RetainName         = "Private"                             │
│ RetainId           = "1"                                   │
│ ClientType         = "PRIVATE"                             │
│ Active             = "Y"                                   │
└─────────────────────────────────────────────────────────────┘

                                              │
                                              │ Patient Number (pNo)
                                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 Billings (Invoice Records)                  │
├─────────────────────────────────────────────────────────────┤
│ Invoice 1:                                                  │
│  billNO = "INV001", pNo = "P001"                           │
│  bDate = 2024-06-01, AmountBilled = 100.00                │
│  DebtBF = 0.00 (First invoice always 0)                   │
│  AmountPaid = 0.00                                         │
│  Balance = ((100 - 0) + 0 + 0) - 0 = 100.00              │
├─────────────────────────────────────────────────────────────┤
│ Invoice 2:                                                  │
│  billNO = "INV002", pNo = "P001"                           │
│  bDate = 2024-06-05, AmountBilled = 50.00                 │
│  DebtBF = 100.00 ◄─ Carried forward from Invoice 1        │
│  AmountPaid = 30.00                                        │
│  Balance = ((50 - 0) + 100 + 0) - 30 = 120.00            │
├─────────────────────────────────────────────────────────────┤
│ Invoice 3:                                                  │
│  billNO = "INV003", pNo = "P001"                           │
│  bDate = 2024-06-10, AmountBilled = 75.00                 │
│  DebtBF = 120.00 ◄─ Carried forward from Invoice 2        │
│  AmountPaid = 50.00, Discount = 10.00                     │
│  Balance = ((75 - 10) + 120 + 0) - 50 = 135.00           │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Debt Calculation Flow

### When Attendance is Taken:

```
┌─────────────────────────────────┐
│ Attendance Recording            │
│ (Patient comes for service)     │
└──────────────┬──────────────────┘
               │
               ▼
┌──────────────────────────────────────────────────┐
│ Step 1: SaveDebtAsync()                          │
│                                                   │
│ 1. Get Patient Details                           │
│    └─ Query: HPatients WHERE Pno = @pNo         │
│                                                   │
│ 2. Find Previous Bill                            │
│    └─ Query: Billings WHERE pNo = @pNo          │
│       (Most recent, excluding current)           │
│                                                   │
│ 3. Check if Patient is PRIVATE                   │
│    └─ Query: HRetainerships                      │
│       WHERE RetainCode = Patient.CoyName         │
│       AND RetainCode = "0001"                    │
│                                                   │
│ 4. If PRIVATE:                                   │
│    └─ Calculate: openBal = 
│       ((Billed - Discount) + PrevDebtBF + Tax)
│       - Paid
│                                                   │
│ 5. If NOT PRIVATE:                               │
│    └─ openBal = 0 (No carry-forward)             │
│                                                   │
│ 6. Update Patient Record                         │
│    └─ HPatient.DebtBf = openBal                  │
│    └─ HPatient.Debt = openBal                    │
│    └─ Save to database                           │
└──────────────┬───────────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────────┐
│ Step 2: SaveBillAsync()                          │
│                                                   │
│ 1. Read Updated Patient Data                     │
│    └─ Query: HPatients WHERE Pno = @pNo         │
│       (Gets DebtBf set by SaveDebtAsync)        │
│                                                   │
│ 2. Check if Billing Already Exists               │
│    └─ Query: Billings                            │
│       WHERE billNO = record.ConsultId            │
│                                                   │
│ 3. If BILLING EXISTS:                            │
│    └─ Update existing Billing                    │
│    └─ Set DebtBF = patient.DebtBf               │
│    └─ Save changes                               │
│                                                   │
│ 4. If BILLING NOT EXISTS:                        │
│    └─ Create new Billing record                  │
│    └─ Set DebtBF = patient.DebtBf               │
│    └─ Insert into database                       │
└──────────────┬───────────────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────────────┐
│ Result: Billing Record Created/Updated           │
│                                                   │
│ New Billing with correct DebtBF                  │
│ Ready for display in Invoices page               │
└──────────────────────────────────────────────────┘
```

---

## 3. Private vs Non-Private Patient Comparison

### PRIVATE Patient (RetainCode = "0001")

```
┌─────────────────────────────────────┐
│ Invoice 1 (June 1)                  │
├─────────────────────────────────────┤
│ Billed:     100.00                  │
│ Discount:     0.00                  │
│ DebtBF:       0.00  ◄─ First invoice
│ Tax:          0.00                  │
│ Paid:         0.00                  │
│ ─────────────────────────────────── │
│ BALANCE:    100.00                  │
└─────────────────────────────────────┘
           │
           │ Debt carries forward ✓
           ▼
┌─────────────────────────────────────┐
│ Invoice 2 (June 5)                  │
├─────────────────────────────────────┤
│ Billed:      50.00                  │
│ Discount:     0.00                  │
│ DebtBF:    100.00  ◄─ CARRIED FWD   │
│ Tax:          0.00                  │
│ Paid:        30.00                  │
│ ─────────────────────────────────── │
│ BALANCE:    120.00                  │
└─────────────────────────────────────┘
           │
           │ Debt carries forward ✓
           ▼
┌─────────────────────────────────────┐
│ Invoice 3 (June 10)                 │
├─────────────────────────────────────┤
│ Billed:      75.00                  │
│ Discount:    10.00                  │
│ DebtBF:    120.00  ◄─ CARRIED FWD   │
│ Tax:          0.00                  │
│ Paid:        50.00                  │
│ ─────────────────────────────────── │
│ BALANCE:    135.00                  │
└─────────────────────────────────────┘
```

### NON-PRIVATE Patient (RetainCode ≠ "0001")

```
┌─────────────────────────────────────┐
│ Invoice 1                           │
├─────────────────────────────────────┤
│ Billed:     100.00                  │
│ Discount:     0.00                  │
│ DebtBF:       0.00                  │
│ Tax:          0.00                  │
│ Paid:         0.00                  │
│ ─────────────────────────────────── │
│ BALANCE:    100.00                  │
└─────────────────────────────────────┘
           │
           │ Debt DOES NOT carry forward ✗
           ▼
┌─────────────────────────────────────┐
│ Invoice 2                           │
├─────────────────────────────────────┤
│ Billed:      50.00                  │
│ Discount:     0.00                  │
│ DebtBF:       0.00  ◄─ ZERO ALWAYS  │
│ Tax:          0.00                  │
│ Paid:        30.00                  │
│ ─────────────────────────────────── │
│ BALANCE:     20.00  ◄─ Independent  │
└─────────────────────────────────────┘
           │
           │ Debt DOES NOT carry forward ✗
           ▼
┌─────────────────────────────────────┐
│ Invoice 3                           │
├─────────────────────────────────────┤
│ Billed:      75.00                  │
│ Discount:    10.00                  │
│ DebtBF:       0.00  ◄─ ZERO ALWAYS  │
│ Tax:          0.00                  │
│ Paid:        50.00                  │
│ ─────────────────────────────────── │
│ BALANCE:     15.00  ◄─ Independent  │
└─────────────────────────────────────┘
```

---

## 4. Troubleshooting Decision Tree

```
START: Invoice Debt Column Shows 0.00 But Should Show Value

    │
    ├─ Patient is PRIVATE?
    │  │
    │  ├─ YES ──────────────────────────────┐
    │  │                                     │
    │  └─ NO (Corporate/HMO) ──────────────┤
    │     │                                  │
    │     └─ This is CORRECT! ◄─────────────┼─ END (No fix needed)
    │        (Non-private patients always    │
    │         have DebtBF = 0.00)           │
    │                                       │
    ├─ Previous Unpaid Invoices Exist?     │
    │  │                                     │
    │  ├─ NO ──────────────────────────────┤
    │  │   (First invoice or all paid)      │
    │  │   This is CORRECT! ◄──────────────┼─ END (No fix needed)
    │  │                                     │
    │  └─ YES (Unpaid from before) ────────┤
    │      │                                 │
    │      ├─ HPatient.CoyName             │
    │      │  matches HRetainership?       │
    │      │                                 │
    │      ├─ NO ──────────────────────────┤
    │      │   (Orphaned reference)         │
    │      │   FIX: Correct CoyName value   │
    │      │   RUN: sp_RecalculatePatient..│
    │      │                                 │
    │      └─ YES ──────────────────────────┤
    │         │                              │
    │         ├─ RetainCode = "0001"?       │
    │         │                              │
    │         ├─ NO ─────────────────────┤
    │         │   (Patient type wrong)       │
    │         │   FIX: Update RetainCode     │
    │         │   RUN: sp_RecalculatePatient..
    │         │                              │
    │         └─ YES ────────────────────┤
    │            (Everything correct)       │
    │            Problem: DebtBF not set    │
    │            │                          │
    │            └─ FIX THIS! ─────────────┤
    │               RUN: 
    │               sp_RecalculatePatientDebt
    │                 @pNo = 'PATIENT_NO',
    │                 @dryRun = 0;
    │
    └──────────────────────────────────────────────────────────► END
```

---

## 5. Script Execution Flowchart

```
START HERE

    │
    ├─ Do you need to check ONE patient?
    │  │
    │  └─ YES ──────────────────────────────────┐
    │                                             │
    │     Step 1: EXEC sp_RecalculatePatientDebt│
    │             @pNo = 'PATIENT_NO',          │
    │             @dryRun = 1;                  │
    │     (Review: Is status CORRECT or BAD?)   │
    │             │                              │
    │             ├─ All CORRECT ──► END ✓     │
    │             │                              │
    │             └─ Some BAD ──┐              │
    │                           │               │
    │     Step 2: EXEC sp_RecalculatePatientDebt
    │             @pNo = 'PATIENT_NO',         │
    │             @dryRun = 0;                 │
    │     (Apply the fix)                       │
    │             │                             │
    │             ├─ Error? ──► Check Logs     │
    │             │                             │
    │             └─ Success ─► Verify ✓       │
    │                                          │
    └─────────────────────────────────────────────────────┐
                                                          │
    ├─ Do you need to check ALL patients?               │
    │  │                                                 │
    │  └─ YES ──────────────────────────────────┐        │
    │                                             │        │
    │     Step 1: EXEC sp_RecalculateAllPatientDebt
    │             @applyChanges = 0;            │        │
    │     (Review: How many patients have issues?)      │
    │             │                              │        │
    │             ├─ 0 Issues ──► END ✓        │        │
    │             │                              │        │
    │             └─ Many Issues ─┐            │        │
    │                              │             │        │
    │     Step 2: EXEC sp_RecalculateAllPatientDebt
    │             @applyChanges = 1;           │        │
    │     (Batch fix all patients)              │        │
    │             │                             │        │
    │             ├─ Error? ──► Rollback       │        │
    │             │                             │        │
    │             └─ Success ─► Verify ✓       │        │
    │                                          │        │
    └──────────────────────────────────────────────────────┐
                                                          │
    ├─ Do you want a detailed audit?                     │
    │  │                                                 │
    │  └─ YES ──────────────────────────────────┐        │
    │                                             │        │
    │     Run: QuickDebtAudit.sql               │        │
    │     (6 audits will run automatically)     │        │
    │             │                              │        │
    │             ├─ Issues Found? ──► See results
    │             │                              │        │
    │             └─ All Clear ──► END ✓        │        │
    │                                          │        │
    └──────────────────────────────────────────────────────┘
                                      │
                                      ▼
                            ✓ PROCESS COMPLETE
```

---

## Quick Reference Card

### Formulas

```
Balance Due = ((AmountBilled - Discount) + DebtBF + Tax) - AmountPaid

For First Invoice (Seq = 1):
  DebtBF = 0

For Later Invoices (Seq > 1):
  DebtBF = Previous Invoice's Balance

For Non-Private (RetainCode ≠ "0001"):
  DebtBF = Always 0
```

### Queries to Remember

```sql
-- Check single patient
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;

-- Fix single patient
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 0;

-- Check all patients
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;

-- Fix all patients
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```

### Critical Rules

1. ✓ DebtBF only applies to Private patients (RetainCode = "0001")
2. ✓ First invoice always has DebtBF = 0
3. ✓ Subsequent invoices carry forward previous balance
4. ✓ Corporate/HMO always have DebtBF = 0
5. ✓ Patient's CoyName must match HRetainership.RetainCode

---

End of Visual Reference
