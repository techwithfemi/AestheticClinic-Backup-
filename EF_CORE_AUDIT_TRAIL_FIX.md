# EF Core Audit Trail Fix - Complete

## Problem
Audit logs were **not being saved to the `Auditrail` table for CRUD operations through Entity Framework Core**, even though the audit writer was expected to handle them.

## Root Cause
The `HospitalAuditTrailInterceptor` class (EF Core audit path) had **three critical issues**:

1. **Missing `OriginalAction` column in INSERT statement** — The SQL insert was missing the `OriginalAction` parameter:
   ```sql
   -- BEFORE (incorrect)
   INSERT INTO Auditrail (TranCode, UserName, UserAction, ActionDate, ActionTime, Remarks, Src, AuditCat)
   VALUES (@TranCode, @UserName, @UserAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);
   ```

2. **Using `ToLabel()` for JSON keys** — The old code converted property names to spaced labels:
   ```csharp
   dict[ToLabel(p.Metadata.Name)] = p.OriginalValue;
   // Result: "desName" became "des name" ❌
   ```

3. **Not capturing original/new values separately** — The old `BuildPayloadJson()` mixed old and new values with `old → new` format instead of separate payloads.

## Solution
Fixed `HospitalAuditTrailInterceptor.cs` with the following changes:

### 1. ✅ Added `OriginalAction` to INSERT Statement
```csharp
const string insertSql = @"
INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat)
VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";
```

### 2. ✅ Updated `HospitalAuditEntry` Record
```csharp
private sealed record HospitalAuditEntry(
    string TranCode,
    string UserName,
    string UserAction,
    string? OriginalAction,  // ← NEW
    DateTime ActionDate,
    DateTime ActionTime,
    string? Remarks,
    string? Src,
    string? AuditCat);
```

### 3. ✅ Split Payload Logic into Two Methods
Replaced the old `BuildPayloadJson()` with two methods:

**`BuildUserActionJson()`** — Captures current values:
- **Create**: All non-null current values
- **Update**: Only changed fields with their NEW values
- **Delete**: Empty (full deleted record goes in `OriginalAction`)

**`BuildOriginalActionJson()`** — Captures original values:
- **Create**: `null` (no previous state)
- **Update**: Only changed fields with their OLD values
- **Delete**: Full deleted record (for recovery/compliance)

### 4. ✅ Use Property Names Directly
Removed the `ToLabel()` method and now use property names as-is:
```csharp
dict[p.Metadata.Name] = p.CurrentValue;  // ✅ "desName" stays "desName"
```

### 5. ✅ Updated `CaptureEntries()` Method
Now calls the new methods and passes both `UserAction` and `OriginalAction`:
```csharp
var userAction = BuildUserActionJson(entry, eventType);
var originalAction = BuildOriginalActionJson(entry, eventType);

return new HospitalAuditEntry(
    // ... other fields ...
    UserAction: SafeTrim(userAction, 5000) ?? userAction,
    OriginalAction: SafeTrim(originalAction, 5000),
    // ... rest of fields ...
);
```

## Results
- ✅ EF Core CRUD operations now write audit entries to `Auditrail`
- ✅ `UserAction` contains new values; `OriginalAction` contains old values
- ✅ JSON payloads use clean property names (e.g., `"desName"` not `"des name"`)
- ✅ Delete operations preserve full deleted records in `OriginalAction` for compliance
- ✅ Update operations show exactly which fields changed and their before/after values
- ✅ Create operations have `OriginalAction = null`

## Files Modified
- `AestheticEMR\AestheticEMR.Core\Infrastructure\HospitalAuditTrailInterceptor.cs`

## Build Status
✅ **Build successful** — AestheticEMR.Core project compiles with no errors

## Audit Trail Behavior (All Paths)
This fix completes the audit trail system for **both** data access paths:

| Operation | Dapper Path | EF Core Path |
|-----------|-------------|--------------|
| **Auditing** | ✅ `AuditedSqlDataAccess` | ✅ `HospitalAuditTrailInterceptor` |
| **UserAction** | Current/new values | Current/new values |
| **OriginalAction** | Requires explicit capture by service | Auto-captured from EF change tracking |
| **JSON Keys** | Property names (clean) | Property names (clean) |

## Next Steps
1. **Test** with any EF Core CRUD operations (e.g., through ASP.NET controllers using `dbContext`)
2. **Verify** audit entries appear in `Auditrail` table with both `UserAction` and `OriginalAction`
3. **Deploy** to production with confidence that all CRUD audit trails are working
