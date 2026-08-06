# Audit Trail Before/After Implementation

## Overview
Implemented a two-column audit trail system in the `Auditrail` table to capture both original (before) and updated (after) values for compliance and audit analysis.

---

## Database Schema Changes

### New Column Added to `Auditrail` Table
```sql
ALTER TABLE Auditrail ADD OriginalAction VARCHAR(MAX) NULL;
```

**Column Details:**
- **Column Name**: `OriginalAction`
- **Data Type**: `VARCHAR(MAX)` (JSON)
- **Purpose**: Stores the original/previous values before an update
- **Used For**: Updates only (Create/Delete set this to NULL)

---

## Code Changes

### 1. **IHospitalAuditWriter.cs** (Interface)

**Changed Signature:**
```csharp
// BEFORE:
Task WriteAsync(string tranCode, string eventType, string src, string auditCat,
    IReadOnlyDictionary<string, object?> payload);

// AFTER:
Task WriteAsync(string tranCode, string eventType, string src, string auditCat,
    IReadOnlyDictionary<string, object?> payload,
    IReadOnlyDictionary<string, object?>? originalPayload = null);
```

**New Parameter**: `originalPayload` - Optional dictionary containing original values before update

---

### 2. **HospitalAuditWriter.cs** (Implementation)

**Changes:**
- Updated INSERT SQL to include `OriginalAction` column
- Added logic to serialize `originalPayload` to JSON when provided
- Maintains backward compatibility (originalPayload is optional)

**Key Logic:**
```csharp
// OriginalAction: JSON object with old values (for Update operations)
var originalAction = originalPayload != null 
    ? Truncate(JsonSerializer.Serialize(originalPayload), 5000) 
    : null;
```

---

### 3. **AuditedSqlDataAccess.cs** (Dapper Decorator)

**Changes:**
- Updated INSERT SQL to include `OriginalAction` column
- Sets `OriginalAction = null` for all audits via this path

**Rationale:**
- `AuditedSqlDataAccess` audits AFTER the write completes
- Cannot capture before-values without additional queries
- Services with explicit before/after data should use `IHospitalAuditWriter` instead

**Insert SQL Updated:**
```csharp
private const string InsertAuditSql =
    "INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat) " +
    "VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";
```

---

### 4. **DepartmentService.cs** (Example Implementation)

**Changed UpdateAsync Method:**
- Captures original values BEFORE executing the update
- Passes both original and new payloads to audit writer

**Before:**
```csharp
public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
{
    // ... update logic ...

    await auditWriter.WriteAsync(normalizedId, "Update", AuditSrc, AuditCat,
        new Dictionary<string, object?>
        {
            ["deptId"] = normalizedId,
            ["deptName"] = department.DeptName,
            ["deptAddress"] = department.DeptAddress,
            ["location"] = department.Location
        });
}
```

**After:**
```csharp
public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
{
    var normalizedId = NormalizeText(department.DeptId)
        ?? throw new KeyNotFoundException("Department id is required.");

    await using var connection = await OpenConnectionAsync();

    // Capture ORIGINAL values BEFORE update ← NEW
    var originalDepartment = await GetByIdAsync(normalizedId)
        ?? throw new KeyNotFoundException($"Department {normalizedId} not found.");

    const string updateSql = @"
UPDATE EmpDepartments
SET DeptName = @DeptName,
    DeptAddress = @DeptAddress,
    Location = @Location
WHERE LTRIM(RTRIM(DeptID)) = @DeptId;";

    var affected = await connection.ExecuteAsync(updateSql, new
    {
        DeptId = normalizedId,
        DeptName = NormalizeText(department.DeptName) ?? string.Empty,
        DeptAddress = NormalizeText(department.DeptAddress),
        Location = NormalizeText(department.Location)
    });

    if (affected == 0)
        throw new KeyNotFoundException($"Department {normalizedId} not found.");

    var refreshed = await GetByIdAsync(normalizedId);
    logger.LogInformation("Updated department {DeptId}", normalizedId);

    // Write audit with BOTH new and original values ← NEW
    await auditWriter.WriteAsync(normalizedId, "Update", AuditSrc, AuditCat,
        payload: new Dictionary<string, object?>
        {
            ["deptId"] = normalizedId,
            ["deptName"] = department.DeptName,
            ["deptAddress"] = department.DeptAddress,
            ["location"] = department.Location
        },
        originalPayload: new Dictionary<string, object?>
        {
            ["deptId"] = originalDepartment.DeptId,
            ["deptName"] = originalDepartment.DeptName,
            ["deptAddress"] = originalDepartment.DeptAddress,
            ["location"] = originalDepartment.Location
        });

    return refreshed ?? department;
}
```

---

## Usage Examples

### Update Scenario
When updating a Department from "Old Name" to "New Name":

**Auditrail Record:**
```json
{
  "TranCode": "01",
  "UserName": "admin",
  "UserAction": {
    "deptId": "01",
    "deptName": "New Name",
    "deptAddress": "123 New Street",
    "location": "New Location"
  },
  "OriginalAction": {
    "deptId": "01",
    "deptName": "Old Name",
    "deptAddress": "456 Old Street",
    "location": "Old Location"
  },
  "ActionDate": "2024-12-15",
  "ActionTime": "2024-12-15 14:30:45.123",
  "Remarks": "updated record with priKey: 01",
  "Src": "EmpDepartments",
  "AuditCat": "employees"
}
```

### Reporting Query Example
Find all departments that changed their name:
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    ActionTime,
    JSON_VALUE(OriginalAction, '$.deptName') AS OldName,
    JSON_VALUE(UserAction, '$.deptName') AS NewName
FROM Auditrail
WHERE 
    AuditCat = 'employees' 
    AND EventType = 'Update'
    AND JSON_VALUE(OriginalAction, '$.deptName') != JSON_VALUE(UserAction, '$.deptName');
```

---

## Migration Path for Other Services

To implement before/after auditing in other services:

1. **Capture original values before write:**
   ```csharp
   var originalRecord = await GetByIdAsync(id);
   ```

2. **Perform the update operation**

3. **Call auditWriter with both payloads:**
   ```csharp
   await auditWriter.WriteAsync(id, "Update", "EntityName", "moduleName",
       payload: new Dictionary<string, object?> { /* new values */ },
       originalPayload: new Dictionary<string, object?> { /* old values */ });
   ```

---

## Benefits

✅ **Compliance**: Full before/after audit trail for regulatory requirements  
✅ **Audit Analysis**: Easy to identify what actually changed  
✅ **Query Performance**: Separate JSON columns for efficient filtering  
✅ **Backward Compatible**: `originalPayload` parameter is optional  
✅ **Clean JSON**: Property names match ViewModel definitions (no spaces)  
✅ **Enterprise Standard**: Follows industry best practices for audit trails

---

## Notes

- **Create & Delete Operations**: `OriginalAction` remains NULL (no before-values needed)
- **AuditedSqlDataAccess Path**: Always sets `OriginalAction = null` (decorator approach doesn't capture before-values)
- **IHospitalAuditWriter Path**: Can capture before/after when originalPayload is provided
- **JSON Size Limit**: Both UserAction and OriginalAction truncated at 5000 characters

---

## Files Modified

1. ✅ `AestheticEMR.Core/Infrastructure/IHospitalAuditWriter.cs`
2. ✅ `AestheticEMR.Core/Infrastructure/HospitalAuditWriter.cs`
3. ✅ `AestheticEMR.Core/DataAccess/DbAccess/AuditedSqlDataAccess.cs`
4. ✅ `AestheticEMR.Core/Services/Employees/DepartmentService.cs`

---

## Next Steps

- Apply same pattern to other services that need before/after audit trails
- Create reporting views/queries for audit analysis
- Test the implementation with Department updates
- Monitor performance impact (should be minimal)
