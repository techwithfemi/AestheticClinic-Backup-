## Debt Carry-Forward Logic - Summary

### Files Created

I've created comprehensive T-SQL scripts to understand, validate, and fix the debt carry-forward logic:

#### 1. **RecalculatePatientDebt.sql**
- **Purpose:** Recalculate debt for a SINGLE patient
- **Usage:** `EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;`
- **Features:**
  - Validates patient exists
  - Checks if patient is private (RetainCode = '0001')
  - Recalculates correct DebtBF in chronological order
  - Shows what will be corrected
  - Can apply corrections with `@dryRun = 0`

#### 2. **RecalculateAllPatientDebt.sql**
- **Purpose:** Find and fix ALL private patients with debt issues
- **Usage:** `EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;`
- **Features:**
  - Scans ALL private patients
  - Identifies those with incorrect DebtBF
  - Generates detailed report
  - Can batch-fix all issues with `@applyChanges = 1`

#### 3. **ValidateDebtCarryForward.sql**
- **Purpose:** Quick validation queries
- **Contains:**
  - Query 1: Check specific patient's debt history
  - Query 2: Find private patients with outstanding debt
  - Query 3: Find potential debt carry-forward errors
  - Query 4: Summary statistics

#### 4. **QuickDebtAudit.sql**
- **Purpose:** Fast audits for debt issues
- **Contains 6 audits:**
  - Audit 1: Patients with mismatched debt values
  - Audit 2: Corporate patients with non-zero debt (ERROR)
  - Audit 3: Orphaned retainership references
  - Audit 4: First invoices with non-zero DebtBF (ERROR)
  - Audit 5: Recent billing activity
  - Audit 6: Summary statistics

#### 5. **DEBT_CARRY_FORWARD_README.md**
- Comprehensive documentation of debt logic
- Database schema explanation
- Code implementation details
- Testing checklist
- Troubleshooting guide

---

### Debt Carry-Forward Logic (Quick Reference)

#### Formula
```
Balance Due = ((AmountBilled - Discount) + DebtBF + Tax) - AmountPaid
```

#### Key Rules
1. **Only Private Patients** (RetainCode = "0001") get debt carry-forward
2. **First Invoice:** DebtBF = 0
3. **Subsequent Invoices:** DebtBF = Previous Invoice's Balance
4. **Corporate/HMO:** DebtBF is always 0 (no carry-forward)

#### Example
| Invoice | Billed | DebtBF | Paid | Balance |
|---------|--------|--------|------|---------|
| INV1 | 100 | 0 | 0 | **100** |
| INV2 | 50 | **100** | 30 | **120** |
| INV3 | 75 | **120** | 50 | **145** |

---

### How to Use the Scripts

#### Step 1: Check a Specific Patient
```sql
-- This will show you what's wrong (safe - no changes)
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 1;
```

#### Step 2: Fix That Patient
```sql
-- This applies the corrections
EXEC sp_RecalculatePatientDebt @pNo = 'P001', @dryRun = 0;
```

#### Step 3: Check All Patients
```sql
-- Report of all patients with issues (safe - no changes)
EXEC sp_RecalculateAllPatientDebt @applyChanges = 0;
```

#### Step 4: Fix All Patients
```sql
-- Batch fix all private patients with debt issues
EXEC sp_RecalculateAllPatientDebt @applyChanges = 1;
```

#### Step 5: Run Audit
```sql
-- Quick audit to verify everything is correct
-- (this is just queries, no changes)
EXEC sp_QuickDebtAudit;  -- if procedure is created
-- OR run the script directly
```

---

### Database Relationships

```
HPatients
├── Pno (PK)
├── CoyName ──────┐
├── DebtBf        │
└── Debt          │
                  │
HRetainerships    │
├── RetainCode ◄──┘
├── RetainName
└── ClientType ("PRIVATE" if RetainCode = "0001")

                  │
Billings          │
├── ID (PK)       │
├── billNO        │
├── pNo ──────────┘
├── DebtBF (carries previous balance)
├── AmountBilled
├── AmountPaid
└── Balance (calculated)
```

---

### Common Issues & Fixes

| Issue | Symptom | Fix |
|-------|---------|-----|
| Wrong RetainCode Check | Using `CoyType` instead of retainership lookup | Fixed in `SaveDebtAsync` - now does proper lookup |
| Existing Billing Not Updated | Re-taking attendance doesn't update DebtBF | Fixed in `SaveBillAsync` - now updates if billing exists |
| Corporate with Debt | Non-private patients showing debt | Use `QuickDebtAudit.sql` Audit 2 to find and fix |
| First Invoice Wrong | First invoice has non-zero DebtBF | Use `QuickDebtAudit.sql` Audit 4 to find and fix |

---

### Files Location

All scripts are in:
```
AestheticEMR\sql_scripts\
├── RecalculatePatientDebt.sql
├── RecalculateAllPatientDebt.sql
├── ValidateDebtCarryForward.sql
├── QuickDebtAudit.sql
└── DEBT_CARRY_FORWARD_README.md
```

---

### Backend Code Changes

**File:** `AestheticEMR\AestheticEMR.Core\Services\Legacy\AttendanceService.cs`

**Changes Made:**
1. **SaveDebtAsync** - Now correctly identifies private patients via HRetainership lookup (not CoyType)
2. **SaveBillAsync** - Now updates existing billing records with latest DebtBF value

---

### Testing Before Running Scripts

1. Backup your database first!
2. Always run with `@dryRun = 1` or `@applyChanges = 0` first
3. Review the report carefully
4. Only then apply with `@dryRun = 0` or `@applyChanges = 1`

---

### Next Steps

1. ✅ Run `ValidateDebtCarryForward.sql` to check current state
2. ✅ Run `QuickDebtAudit.sql` to identify issues
3. ✅ Run `sp_RecalculatePatientDebt` for specific patients with issues
4. ✅ Verify with `ValidateDebtCarryForward.sql` query results

---

**Questions?** Refer to `DEBT_CARRY_FORWARD_README.md` for detailed documentation.
