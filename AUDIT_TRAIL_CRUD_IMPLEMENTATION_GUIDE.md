# CRUD Audit Trail Implementation Guide

## Overview

This guide explains how to implement proper audit trails for CREATE, UPDATE, and DELETE operations using the `IHospitalAuditWriter` and the two-column `Auditrail` table schema:

- **UserAction**: New values (or minimal info for deletes)
- **OriginalAction**: Old values (for updates/deletes)

---

## Column Handling by Operation Type

### 1. CREATE Operation

**Pattern:**
- `UserAction`: Full new record (all fields)
- `OriginalAction`: NULL

**Rationale:**
- Record didn't exist before creation
- NULL is semantically correct
- Minimal database footprint

**Example:**
```json
{
  "UserAction": {
    "deptId": "01",
    "deptName": "Finance",
    "deptAddress": "100 Main St",
    "location": "Headquarters"
  },
  "OriginalAction": null
}
```

**Query to find creates:**
```sql
WHERE EventType = 'Create' AND OriginalAction IS NULL
```

---

### 2. UPDATE Operation

**Pattern:**
- `UserAction`: New values after update
- `OriginalAction`: Old values before update

**Rationale:**
- Track exactly what changed
- Enable before/after comparison
- Compliance requirement for modifications

**Example:**
```json
{
  "UserAction": {
    "deptId": "01",
    "deptName": "Finance & Accounting",  // ← Changed
    "deptAddress": "100 Main St",
    "location": "Headquarters"
  },
  "OriginalAction": {
    "deptId": "01",
    "deptName": "Finance",  // ← Original value
    "deptAddress": "100 Main St",
    "location": "Headquarters"
  }
}
```

**Query to find what changed:**
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    JSON_VALUE(OriginalAction, '$.deptName') AS OldName,
    JSON_VALUE(UserAction, '$.deptName') AS NewName
FROM Auditrail
WHERE EventType = 'Update'
  AND JSON_VALUE(OriginalAction, '$.deptName') != JSON_VALUE(UserAction, '$.deptName')
ORDER BY ActionDate DESC;
```

---

### 3. DELETE Operation

**Pattern:**
- `UserAction`: Minimal info (just ID)
- `OriginalAction`: Full deleted record

**Rationale:**
- **CRITICAL**: Deleted record can only be recovered from audit trail
- Full data preservation for compliance
- Minimal UserAction keeps record lean

**Example:**
```json
{
  "UserAction": {
    "deptId": "01"  // ← Minimal
  },
  "OriginalAction": {
    "deptId": "01",
    "deptName": "Finance",
    "deptAddress": "100 Main St",
    "location": "Headquarters"
  }
}
```

**Query to recover deleted data:**
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    OriginalAction AS DeletedRecord
FROM Auditrail
WHERE EventType = 'Delete'
  AND TranCode = '01';
```

---

## Implementation Patterns

### Pattern 1: Basic Service (Manual Dictionary)

```csharp
public class DepartmentService
{
    private readonly IHospitalAuditWriter _auditWriter;

    // CREATE
    public async Task<EmpDepartments> CreateAsync(EmpDepartments department)
    {
        // ... save to database ...

        await _auditWriter.WriteAsync(department.DeptId, "Create", "EmpDepartments", "employees",
            payload: new Dictionary<string, object?>
            {
                ["deptId"] = department.DeptId,
                ["deptName"] = department.DeptName,
                ["deptAddress"] = department.DeptAddress,
                ["location"] = department.Location
            });
            // originalPayload defaults to null ✓
    }

    // UPDATE
    public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
    {
        var normalizedId = NormalizeText(department.DeptId);

        // Capture BEFORE values
        var originalDepartment = await GetByIdAsync(normalizedId);

        // ... perform update ...

        await _auditWriter.WriteAsync(normalizedId, "Update", "EmpDepartments", "employees",
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
    }

    // DELETE
    public async Task<bool> DeleteAsync(string deptId)
    {
        var normalizedId = NormalizeText(deptId);

        // Capture FULL record BEFORE deletion
        var deletedDepartment = await GetByIdAsync(normalizedId);

        // ... perform delete ...

        await _auditWriter.WriteAsync(normalizedId, "Delete", "EmpDepartments", "employees",
            payload: new Dictionary<string, object?>
            {
                ["deptId"] = normalizedId
            },
            originalPayload: new Dictionary<string, object?>
            {
                ["deptId"] = deletedDepartment.DeptId,
                ["deptName"] = deletedDepartment.DeptName,
                ["deptAddress"] = deletedDepartment.DeptAddress,
                ["location"] = deletedDepartment.Location
            });
    }
}
```

### Pattern 2: Using Helper Class (Recommended)

```csharp
using AestheticEMR.Core.Services.Audit;

public class DepartmentService
{
    private readonly IHospitalAuditWriter _auditWriter;

    // CREATE
    public async Task<EmpDepartments> CreateAsync(EmpDepartments department)
    {
        // ... save to database ...

        var (payload, originalPayload) = AuditPayloadHelper.BuildCreatePayload(
            new Dictionary<string, object?>
            {
                ["deptId"] = department.DeptId,
                ["deptName"] = department.DeptName,
                ["deptAddress"] = department.DeptAddress,
                ["location"] = department.Location
            });

        await _auditWriter.WriteAsync(department.DeptId, "Create", "EmpDepartments", "employees",
            payload, originalPayload);
    }

    // UPDATE
    public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
    {
        var originalDepartment = await GetByIdAsync(NormalizeText(department.DeptId));

        // ... perform update ...

        var (payload, originalPayload) = AuditPayloadHelper.BuildUpdatePayload(
            new Dictionary<string, object?>
            {
                ["deptId"] = department.DeptId,
                ["deptName"] = department.DeptName,
                ["deptAddress"] = department.DeptAddress,
                ["location"] = department.Location
            },
            new Dictionary<string, object?>
            {
                ["deptId"] = originalDepartment.DeptId,
                ["deptName"] = originalDepartment.DeptName,
                ["deptAddress"] = originalDepartment.DeptAddress,
                ["location"] = originalDepartment.Location
            });

        await _auditWriter.WriteAsync(department.DeptId, "Update", "EmpDepartments", "employees",
            payload, originalPayload);
    }

    // DELETE
    public async Task<bool> DeleteAsync(string deptId)
    {
        var deletedDepartment = await GetByIdAsync(NormalizeText(deptId));

        // ... perform delete ...

        var (payload, originalPayload) = AuditPayloadHelper.BuildDeletePayload(
            new Dictionary<string, object?>
            {
                ["deptId"] = deletedDepartment.DeptId,
                ["deptName"] = deletedDepartment.DeptName,
                ["deptAddress"] = deletedDepartment.DeptAddress,
                ["location"] = deletedDepartment.Location
            },
            idKey: "deptId");

        await _auditWriter.WriteAsync(deptId, "Delete", "EmpDepartments", "employees",
            payload, originalPayload);
    }
}
```

---

## Quick Reference Table

| Operation | UserAction | OriginalAction | Purpose |
|-----------|-----------|-----------------|---------|
| **CREATE** | Full new record | NULL | Track creation, minimal footprint |
| **UPDATE** | New values | Old values | Track all changes for compliance |
| **DELETE** | Just ID | Full record | **CRITICAL** - preserve deleted data |

---

## Compliance & Audit Queries

### Find all changes to a specific field
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    JSON_VALUE(OriginalAction, '$.fieldName') AS OldValue,
    JSON_VALUE(UserAction, '$.fieldName') AS NewValue
FROM Auditrail
WHERE EventType = 'Update'
  AND JSON_VALUE(OriginalAction, '$.fieldName') IS NOT NULL
ORDER BY ActionDate DESC;
```

### Find who deleted what
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    ActionTime,
    OriginalAction AS DeletedData
FROM Auditrail
WHERE EventType = 'Delete'
ORDER BY ActionDate DESC;
```

### Find record creation history
```sql
SELECT 
    TranCode,
    UserName,
    ActionDate,
    UserAction AS CreatedData
FROM Auditrail
WHERE EventType = 'Create'
  AND AuditCat = 'employees'
ORDER BY ActionDate DESC;
```

### Audit trail for single entity
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
WHERE TranCode = @EntityId
ORDER BY ActionDate DESC, ActionTime DESC;
```

---

## Implementation Checklist

When adding audit trails to a new service:

- [ ] **Inject `IHospitalAuditWriter`** into service constructor
- [ ] **CREATE method**: Pass full new record to `WriteAsync()` with `payload`, leave `originalPayload = null`
- [ ] **UPDATE method**: Capture old record BEFORE update, pass both `payload` (new) and `originalPayload` (old)
- [ ] **DELETE method**: Capture full record BEFORE delete, pass minimal `payload` (just ID) and full `originalPayload`
- [ ] **Error Handling**: Audit failures should never fail the business operation (already handled by `IHospitalAuditWriter`)
- [ ] **Testing**: Verify all three CRUD operations create correct audit trail entries
- [ ] **Query Testing**: Test sample compliance/recovery queries

---

## Best Practices

✅ **DO:**
- Capture before-values BEFORE performing the write operation
- Use consistent property naming across all services (camelCase/PascalCase)
- Store full data for delete operations (recovery requirement)
- Log all write operations (CREATE, UPDATE, DELETE)
- Document your audit column mapping in code

❌ **DON'T:**
- Forget to capture original values before UPDATE
- Skip DELETE audits (compliance violation)
- Store sensitive data in audit trail (use encryption if needed)
- Modify audit trail records after creation
- Rely on database change tracking alone (audit trail is source of truth)

---

## File References

- **Interface**: `AestheticEMR.Core/Infrastructure/IHospitalAuditWriter.cs`
- **Implementation**: `AestheticEMR.Core/Infrastructure/HospitalAuditWriter.cs`
- **Helper**: `AestheticEMR.Core/Services/Audit/AuditPayloadHelper.cs`
- **Example**: `AestheticEMR.Core/Services/Employees/DepartmentService.cs`

---

## Troubleshooting

**Issue**: "OriginalAction is NULL for all updates"
- **Solution**: Ensure you're capturing the original record BEFORE performing the update

**Issue**: "Delete audit only has deptId, need full data"
- **Solution**: Capture full record BEFORE delete and pass as `originalPayload`

**Issue**: "Audit trail is storing all nulls"
- **Solution**: Check that values are being serialized correctly to `Dictionary<string, object?>`

**Issue**: "Audit entries are being created with 5000+ char JSON"
- **Solution**: JSON is being truncated to 5000 chars by design (configurable in `HospitalAuditWriter`)

---

## Future Enhancements

- Add encryption for sensitive audit data
- Implement audit trail retention policies
- Create dashboard for audit trail visualization
- Add email alerts for critical operations
- Build compliance reports (SOX, HIPAA, etc.)
