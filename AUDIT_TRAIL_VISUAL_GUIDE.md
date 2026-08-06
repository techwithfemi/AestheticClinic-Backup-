# Visual Guide: CRUD Audit Trail Implementation

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                  DepartmentService                       │
├─────────────────────────────────────────────────────────┤
│                                                           │
│  ┌─────────────┐  ┌─────────────┐  ┌──────────────┐    │
│  │   CREATE    │  │   UPDATE    │  │    DELETE    │    │
│  │             │  │             │  │              │    │
│  └──────┬──────┘  └──────┬──────┘  └──────┬───────┘    │
│         │                │                │             │
│         └────────────────┼────────────────┘             │
│                          │                               │
│                          ▼                               │
│         ┌────────────────────────────────┐              │
│         │  IHospitalAuditWriter          │              │
│         │  .WriteAsync()                 │              │
│         └────────────────┬───────────────┘              │
│                          │                               │
│                          ▼                               │
│         ┌────────────────────────────────┐              │
│         │  Auditrail Table               │              │
│         │  (UserAction + OriginalAction) │              │
│         └────────────────────────────────┘              │
│                                                           │
└─────────────────────────────────────────────────────────┘
```

---

## Operation-by-Operation Breakdown

### 1️⃣ CREATE OPERATION

**Flow:**
```
Input: New Department
   ↓
Save to EmpDepartments
   ↓
Audit: Full record → UserAction
        OriginalAction = null
   ↓
Result: Record created + audited
```

**Database Result:**
```
Auditrail Row:
┌─────────────────────────────────────────────────────┐
│ TranCode | UserName | ActionDate | ActionTime      │
│ "01"     | "admin"  | "2024-12-15"| "14:30:45"     │
├─────────────────────────────────────────────────────┤
│ UserAction (NEW):                                   │
│ {                                                   │
│   "deptId": "01",                                   │
│   "deptName": "Finance",                           │
│   "deptAddress": "100 Main St",                    │
│   "location": "Headquarters"                        │
│ }                                                   │
├─────────────────────────────────────────────────────┤
│ OriginalAction: null  ← No previous values          │
├─────────────────────────────────────────────────────┤
│ Remarks: "created record"                           │
│ Src: "EmpDepartments"                              │
│ AuditCat: "employees"                               │
└─────────────────────────────────────────────────────┘
```

**Code Pattern:**
```csharp
public async Task<EmpDepartments> CreateAsync(EmpDepartments department)
{
    // ① Save to database
    await connection.ExecuteAsync(insertSql, new
    {
        DeptId = department.DeptId,
        DeptName = department.DeptName,
        DeptAddress = department.DeptAddress,
        Location = department.Location
    }, transaction);

    // ② Audit with NO original values
    await auditWriter.WriteAsync(department.DeptId, "Create", AuditSrc, AuditCat,
        payload: new Dictionary<string, object?>
        {
            ["deptId"] = department.DeptId,
            ["deptName"] = department.DeptName,
            ["deptAddress"] = department.DeptAddress,
            ["location"] = department.Location
        }
        // originalPayload OMITTED → defaults to null ✓
    );
}
```

---

### 2️⃣ UPDATE OPERATION

**Flow:**
```
Input: Updated Department
   ↓
① Query BEFORE-VALUES from database
   ↓
② Update record
   ↓
③ Audit:
   UserAction ← new values
   OriginalAction ← old values
   ↓
Result: Record updated + changes tracked
```

**Database Result:**
```
Auditrail Row:
┌──────────────────────────────────────────────────────┐
│ TranCode | UserName | ActionDate | ActionTime       │
│ "01"     | "admin"  | "2024-12-15"| "14:35:20"      │
├──────────────────────────────────────────────────────┤
│ UserAction (NEW VALUES):                             │
│ {                                                    │
│   "deptId": "01",                                    │
│   "deptName": "Finance & Accounting",  ← CHANGED     │
│   "deptAddress": "100 Main St",                     │
│   "location": "Headquarters"                         │
│ }                                                    │
├──────────────────────────────────────────────────────┤
│ OriginalAction (OLD VALUES):                         │
│ {                                                    │
│   "deptId": "01",                                    │
│   "deptName": "Finance",  ← ORIGINAL                │
│   "deptAddress": "100 Main St",                     │
│   "location": "Headquarters"                         │
│ }                                                    │
├──────────────────────────────────────────────────────┤
│ Remarks: "updated record with priKey: 01"            │
│ Src: "EmpDepartments"                               │
│ AuditCat: "employees"                                │
└──────────────────────────────────────────────────────┘
```

**Code Pattern:**
```csharp
public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
{
    // ① BEFORE: Capture original values FIRST
    var originalDepartment = await GetByIdAsync(normalizedId);

    // ② UPDATE: Perform the update
    await connection.ExecuteAsync(updateSql, new
    {
        DeptId = normalizedId,
        DeptName = NormalizeText(department.DeptName) ?? string.Empty,
        DeptAddress = NormalizeText(department.DeptAddress),
        Location = NormalizeText(department.Location)
    });

    // ③ AUDIT: Record both old and new
    await auditWriter.WriteAsync(normalizedId, "Update", AuditSrc, AuditCat,
        payload: new Dictionary<string, object?>  // NEW VALUES
        {
            ["deptId"] = normalizedId,
            ["deptName"] = department.DeptName,
            ["deptAddress"] = department.DeptAddress,
            ["location"] = department.Location
        },
        originalPayload: new Dictionary<string, object?>  // OLD VALUES
        {
            ["deptId"] = originalDepartment.DeptId,
            ["deptName"] = originalDepartment.DeptName,
            ["deptAddress"] = originalDepartment.DeptAddress,
            ["location"] = originalDepartment.Location
        }
    );
}
```

---

### 3️⃣ DELETE OPERATION

**Flow:**
```
Input: Department ID
   ↓
① Query FULL RECORD from database
   ↓
② Delete from database
   ↓
③ Audit:
   UserAction ← just ID (minimal)
   OriginalAction ← full deleted record (CRITICAL!)
   ↓
Result: Record deleted + full data preserved
```

**Database Result:**
```
Auditrail Row:
┌──────────────────────────────────────────────────────┐
│ TranCode | UserName | ActionDate | ActionTime       │
│ "01"     | "admin"  | "2024-12-15"| "14:40:10"      │
├──────────────────────────────────────────────────────┤
│ UserAction (MINIMAL):                                │
│ {                                                    │
│   "deptId": "01"  ← Just ID                         │
│ }                                                    │
├──────────────────────────────────────────────────────┤
│ OriginalAction (FULL RECORD):                        │
│ {                                                    │
│   "deptId": "01",                                    │
│   "deptName": "Finance & Accounting",  ← PRESERVED   │
│   "deptAddress": "100 Main St",        ← PRESERVED   │
│   "location": "Headquarters"           ← PRESERVED   │
│ }                                                    │
├──────────────────────────────────────────────────────┤
│ Remarks: "deleted record with priKey: 01"            │
│ Src: "EmpDepartments"                               │
│ AuditCat: "employees"                                │
└──────────────────────────────────────────────────────┘

✓ Full department data recovered from audit trail!
```

**Code Pattern:**
```csharp
public async Task<bool> DeleteAsync(string deptId)
{
    var normalizedId = NormalizeText(deptId);

    // ① BEFORE: Capture FULL record BEFORE deletion
    var deletedDepartment = await GetByIdAsync(normalizedId);

    // ② DELETE: Remove from database
    await connection.ExecuteAsync(sql, new { DeptId = normalizedId });

    // ③ AUDIT: Store full deleted record for recovery
    await auditWriter.WriteAsync(normalizedId, "Delete", AuditSrc, AuditCat,
        payload: new Dictionary<string, object?>  // MINIMAL (just ID)
        {
            ["deptId"] = normalizedId
        },
        originalPayload: new Dictionary<string, object?>  // FULL RECORD (CRITICAL!)
        {
            ["deptId"] = deletedDepartment.DeptId,
            ["deptName"] = deletedDepartment.DeptName,
            ["deptAddress"] = deletedDepartment.DeptAddress,
            ["location"] = deletedDepartment.Location
        }
    );
}
```

---

## Comparison Table

```
┌────────────┬──────────────────┬──────────────────┬────────────────────┐
│ Operation  │ UserAction       │ OriginalAction   │ Business Logic     │
├────────────┼──────────────────┼──────────────────┼────────────────────┤
│ CREATE     │ Full new record  │ NULL             │ Track new entries  │
│            │ ✓All fields      │ (nothing before) │ Minimal footprint  │
├────────────┼──────────────────┼──────────────────┼────────────────────┤
│ UPDATE     │ New values       │ Old values       │ Compliance: track  │
│            │ ✓All fields      │ ✓All fields      │ what changed & who  │
├────────────┼──────────────────┼──────────────────┼────────────────────┤
│ DELETE     │ Just ID (min)    │ Full record      │ CRITICAL: only     │
│            │ ✓Minimal space   │ ✓Full recovery   │ place data survives │
└────────────┴──────────────────┴──────────────────┴────────────────────┘
```

---

## Data Lifecycle Example

### Department "Finance" - Complete Lifecycle

```
STEP 1: CREATE
─────────────────────────────────────────────────────
Event: Admin creates department
  UserAction: { deptId: "01", deptName: "Finance", ... }
  OriginalAction: null
  Remarks: "created record"

STEP 2: UPDATE #1
─────────────────────────────────────────────────────
Event: Admin updates name
  UserAction: { deptId: "01", deptName: "Finance & Accounting", ... }
  OriginalAction: { deptId: "01", deptName: "Finance", ... }
  Remarks: "updated record with priKey: 01"

STEP 3: UPDATE #2
─────────────────────────────────────────────────────
Event: Admin updates address
  UserAction: { deptId: "01", deptName: "Finance & Accounting", deptAddress: "200 New St", ... }
  OriginalAction: { deptId: "01", deptName: "Finance & Accounting", deptAddress: "100 Main St", ... }
  Remarks: "updated record with priKey: 01"

STEP 4: DELETE
─────────────────────────────────────────────────────
Event: Admin deletes department
  UserAction: { deptId: "01" }
  OriginalAction: { deptId: "01", deptName: "Finance & Accounting", deptAddress: "200 New St", ... }
  Remarks: "deleted record with priKey: 01"

RESULT: Complete history + full recovery capability!
```

---

## Audit Trail Query Examples

### Find what changed in the name field:
```sql
SELECT 
    ActionDate, 
    UserName,
    JSON_VALUE(OriginalAction, '$.deptName') AS OldName,
    JSON_VALUE(UserAction, '$.deptName') AS NewName
FROM Auditrail
WHERE EventType = 'Update'
  AND JSON_VALUE(OriginalAction, '$.deptName') != JSON_VALUE(UserAction, '$.deptName')
ORDER BY ActionDate DESC;
```

### Recover deleted department data:
```sql
SELECT 
    UserName,
    ActionDate,
    OriginalAction AS RecoveredData
FROM Auditrail
WHERE EventType = 'Delete' AND TranCode = '01';
```

### Full audit trail for compliance:
```sql
SELECT 
    EventType,
    UserName,
    ActionDate,
    ActionTime,
    UserAction,
    OriginalAction,
    Remarks
FROM Auditrail
WHERE TranCode = '01'
ORDER BY ActionDate DESC, ActionTime DESC;
```

---

## Implementation Status

✅ **DepartmentService.cs** - All CRUD operations updated  
✅ **AuditPayloadHelper.cs** - Utility class created  
✅ **Documentation** - 3 comprehensive guides  
✅ **Build Status** - Compiles without errors  
✅ **Ready for Production** - Fully tested pattern

---

## Key Takeaways

| What | Why | How |
|------|-----|-----|
| **CREATE**: UserAction only | Record didn't exist before | Pass full payload, no originalPayload |
| **UPDATE**: Both columns | Track changes for compliance | Capture before-values, pass both |
| **DELETE**: Full record in Original | Data recovery requirement | Capture full record, pass as originalPayload |

🎯 **This strategy ensures: Compliance ✓ Auditability ✓ Recoverability ✓**
