# How EF Core CRUD Audit Works Now

## The Complete Flow

### 1. EF Core Operation Detected
Any EF Core `dbContext.SaveChangesAsync()` call triggers the interceptor:

```csharp
public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
{
    await PersistEntriesAsync(eventData.Context, cancellationToken);
    return result;
}
```

### 2. Changes Are Captured (SavingChanges Phase)
```csharp
public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
{
    CaptureEntries(eventData.Context);  // ← Before actual save
    return result;
}
```

For each EF-tracked entity with State = Added/Modified/Deleted:
- Get entity metadata
- Determine event type (Create/Update/Delete)
- Extract transaction code (consultID, BillNo, etc.)
- **Capture UserAction** (current values)
- **Capture OriginalAction** (original values or null)
- Store in `PendingEntries` dictionary

### 3. After EF SaveChanges Succeeds
The `SavedChangesAsync()` method is called and inserts audit rows:

```csharp
const string insertSql = @"
INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat)
VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";

foreach (var entry in entries)
{
    await sqlDataAccess.SaveDataText(insertSql, new
    {
        entry.TranCode,
        entry.UserName,
        entry.UserAction,      // ← New values
        entry.OriginalAction,  // ← Old values (or null for Create)
        entry.ActionDate,
        entry.ActionTime,
        entry.Remarks,
        entry.Src,
        entry.AuditCat
    }, DefaultConnectionId);
}
```

## By Operation Type

### CREATE
```
UserAction:     {"firstName":"Ahmed","lastName":"Ali","email":"ahmed.ali@clinic.com","departmentId":2}
OriginalAction: null
Remarks:        "created record"
```

### UPDATE
```
UserAction:     {"firstName":"Ahmed Updated","email":"new@clinic.com"}
OriginalAction: {"firstName":"Ahmed","email":"ahmed.ali@clinic.com"}
Remarks:        "updated record with priKey: 5"
```
Only changed fields are included in both payloads.

### DELETE
```
UserAction:     {}
OriginalAction: {"id":5,"firstName":"Ahmed","lastName":"Ali","email":"ahmed.ali@clinic.com","departmentId":2}
Remarks:        "deleted record with priKey: 5"
```
Full deleted record is preserved in `OriginalAction`.

## Key Features

✅ **Automatic Capture** — No manual auditing code needed, interceptor handles it
✅ **Before/After Values** — `OriginalAction` vs `UserAction` for compliance
✅ **Change Detection** — Updates only log changed fields
✅ **Full Recovery** — Deletes preserve complete record for rollback/audit
✅ **Clean JSON** — Property names used as keys (e.g., `"desName"` not `"des name"`)
✅ **Excluded Entities** — Identity, OpenIddict, and Legacy models are not audited
✅ **Transaction Context** — Automatically extracts TranCode from entity properties
✅ **User Identity** — Captures current user via `IUserIdAccessor`

## Comparison: Dapper vs EF Core

| Aspect | Dapper (`AuditedSqlDataAccess`) | EF Core (`HospitalAuditTrailInterceptor`) |
|--------|----------------------------------|------------------------------------------|
| **Trigger** | After `SaveData`/`SaveDataText` call | EF Core change tracking (`SaveChanges`) |
| **Original Values** | Must be captured by service (see `DepartmentService`) | Auto-captured from EF change tracking |
| **UserAction** | Built from input parameters | Built from `CurrentValue` |
| **OriginalAction** | Service must pass if needed | Built from `OriginalValue` for updates/deletes |
| **Use Case** | Raw SQL, stored procs, legacy flows | Modern EF Core entity operations |

Both paths now produce the same audit trail structure in `Auditrail` table.
