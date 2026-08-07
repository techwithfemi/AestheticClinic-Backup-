# EF Core Audit Logging - THE REAL PROBLEM FOUND & FIXED

## The Issue
Audit logs were **NOT saving** because `HPatient` (and other critical business entities) were being **explicitly excluded** from auditing.

## Root Cause
The `HospitalAuditTrailInterceptor` had this exclusion list:

```csharp
private static readonly string[] ExcludedNamespacePrefixes =
[
    "AestheticEMR.Core.Models.Legacy",  // ← BLOCKED ALL LEGACY MODELS!
    "OpenIddict",
    "Microsoft.AspNetCore.Identity",
];
```

Since `HPatient` is in the `AestheticEMR.Core.Models.Legacy` namespace, it was being completely excluded from auditing in the `ShouldAuditEntry()` method:

```csharp
private static bool ShouldAuditEntry(EntityEntry entry)
{
    var entityNamespace = entry.Metadata.ClrType.Namespace ?? string.Empty;

    foreach (var prefix in ExcludedNamespacePrefixes)
    {
        if (entityNamespace.StartsWith(prefix, StringComparison.Ordinal))
            return false;  // ← HPatient returns false here
    }
    // ...
}
```

## Why This Was Wrong
The "Legacy" namespace contains **active, critical business entities** that absolutely MUST be audited:

- `HPatient` - Patient records
- `HConsulting` - Consultation records
- `HDental` - Dental records
- `Employee` - Employee data
- `HConsultingItem` - Consultation items
- `Payment`, `PaymentDetail` - Payment records
- `Billing`, `BillingDetail` - Billing records
- `HRetainership` - Patient retainer/account type
- And many more core entities...

The exclusion was probably added to skip migration/scaffold artifacts, but it inadvertently blocked all business logic auditing.

## The Fix
**Remove the `AestheticEMR.Core.Models.Legacy` exclusion:**

```csharp
private static readonly string[] ExcludedNamespacePrefixes =
[
    "OpenIddict",                          // ← Keep these (auth framework)
    "Microsoft.AspNetCore.Identity",       // ← Keep these (identity framework)
];

private static readonly string[] ExcludedTablePrefixes =
[
    "AspNet", "vw", "qry", "OpenIddict",  // ← These still filter views & identity tables
];
```

**Why this works:**
- Views (table names starting with `vw` or `qry`) are still excluded by the `ExcludedTablePrefixes`
- Identity framework entities are still excluded (by both namespace and table prefix)
- ALL business entities in the Legacy namespace can now be audited
- The interceptor will now fire for `HPatient` updates/creates/deletes

## Result
✅ When you edit a patient record, the interceptor will now:
1. Detect the change to `HPatient`
2. Pass the `ShouldAuditEntry()` check (no longer excluded)
3. Capture `UserAction` (new values) and `OriginalAction` (old values)
4. Insert audit entry into `Auditrail` table after `SaveChangesAsync()`

✅ Build successful with no errors

## Testing
1. Edit a patient record in the UI
2. Query the `Auditrail` table:
   ```sql
   SELECT * FROM Hospital..Auditrail ORDER BY ActionDate DESC, ActionTime DESC
   ```
3. You should see:
   - `Src = 'HPatient'`
   - `UserAction = { "Pno": "...", "PSurName": "...", ... }` (changed fields)
   - `OriginalAction = { "Pno": "...", "PSurName": "...", ... }` (old values)
   - `TranCode = '<patient-id>'`
   - `Remarks = 'updated record with priKey: <id>'`

## Files Modified
- `AestheticEMR\AestheticEMR.Core\Infrastructure\HospitalAuditTrailInterceptor.cs` (removed Legacy namespace exclusion)

## Related Code Flow
1. **UI Controller** updates `HPatient` via EF Core
2. **ApplicationDbContext.SaveChangesAsync()** is called
3. **HospitalAuditTrailInterceptor.SavingChangesAsync()** fires
4. **CaptureEntries()** checks `ShouldAuditEntry()` → now returns `true` for HPatient
5. **BuildUserActionJson()** & **BuildOriginalActionJson()** capture the payload
6. **SavedChangesAsync()** inserts audit row into `Auditrail` table

## Summary
The audit system was fully implemented and working, but the Legacy namespace was being excluded from auditing. Removing this exclusion (while keeping view/identity exclusions) enables full audit trail for all business entities.
