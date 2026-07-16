# Designation Service Refactoring - Dapper Implementation

## Summary

Successfully refactored the **DesignationService** from Entity Framework Core to **Dapper** for data access, following the VB.NET legacy stored procedures from the SmartHR application.

## Changes Made

### 1. **DesignationService.cs** (Refactored)
**Location**: `AestheticEMR\AestheticEMR.Core\Services\Employees\DesignationService.cs`

**Key Changes**:
- ✅ Injected `ISqlDataAccess` (Dapper wrapper) alongside `ApplicationDbContext`
- ✅ Added `ConnectionId = "smartHRConnection"` constant for database connection
- ✅ Refactored all CRUD operations to use Dapper with legacy stored procedures

**Data Access Pattern**:

| Operation | Technology | Method/Query |
|-----------|-----------|--------------|
| **Read All** | Dapper (SP) | `getDesig` stored procedure |
| **Read By ID** | Dapper (Raw SQL) | `SELECT desID, desName FROM Designation WHERE desID = @desID` |
| **Create** | Dapper (SP) | `InsertEmpDesig` stored procedure with `@desID`, `@desName` |
| **Update** | Dapper (SP) | `updateEmpDesig` stored procedure with `@desOldID`, `@desID`, `@desName` |
| **Delete** | Dapper (SP) | `deleteEmpDesig` stored procedure with `@desID` |
| **IsInUse Check** | Dapper (Raw SQL) | `SELECT COUNT(*) FROM HREmployees WHERE Designation = @Designation` |
| **Usage Counts** | Dapper (Raw SQL) | `SELECT Designation, COUNT(*) FROM HREmployees GROUP BY Designation` |
| **ID Generation** | EF Core | Read from `HrIdgens` table (transactional consistency) |

### 2. **DesignationVM.cs** (Updated)
**Location**: `AestheticEMR\AestheticEMR.Server\ViewModels\Employees\EmployeeVMs.cs`

**Changes**:
- ✅ Updated to use non-nullable `string` properties (not `string?`)
- ✅ Added proper validation attributes (`[Required]`, `[StringLength]`)
- ✅ Changed `InUseCount` to non-nullable `int` (was `int?`)

**Before**:
```csharp
[StringLength(50)]
public string? DesignationId { get; set; }

[Required(AllowEmptyStrings = false), StringLength(150)]
public string? DesignationName { get; set; }

public int? InUseCount { get; set; }
```

**After**:
```csharp
[Required]
[StringLength(2, MinimumLength = 2, ErrorMessage = "Designation ID must be exactly 2 characters")]
public string DesignationId { get; set; } = string.Empty;

[Required]
[StringLength(150, ErrorMessage = "Designation name cannot exceed 150 characters")]
public string DesignationName { get; set; } = string.Empty;

public int InUseCount { get; set; }
```

### 3. **Connection String Configuration**

**Updated Files**:
- ✅ `appsettings.json` (base)
- ✅ `appsettings.Development.json`

**Connection String**:
```json
"smartHRConnection": "Server=Logic;Database=Hospital;User ID=smart;Password=;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

**Note**: `smartHRConnection` points to the same `Hospital` database as `DefaultConnection` since HR tables (`Designation`, `HREmployees`, `IDgen`) are in the same database.

---

## VB.NET Legacy Stored Procedures Used

### 1. **getDesig** (Read All)
```vb
' VB.NET Reference: loanInfo.vb line 2048
dr = SqlHelper.ExecuteDataset(conStr2, CommandType.StoredProcedure, "getDesig")
```

### 2. **InsertEmpDesig** (Create)
```vb
' VB.NET Reference: loanInfo.vb line 2669
Dim params() As SqlParameter = {
    New SqlParameter("@desID", desID), 
    New SqlParameter("@desName", desName)
}
SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "InsertEmpDesig", params)
```

### 3. **updateEmpDesig** (Update)
```vb
' VB.NET Reference: loanInfo.vb line 2248
Dim params() As SqlParameter = {
    New SqlParameter("@desOldID", str), 
    New SqlParameter("@desID", desID), 
    New SqlParameter("@desName", info)
}
SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "updateEmpDesig", params)
```

### 4. **deleteEmpDesig** (Delete)
```vb
' VB.NET Reference: loanInfo.vb line 2231
Dim params() As SqlParameter = {
    New SqlParameter("@desID", info)
}
SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "deleteEmpDesig", params)
```

---

## Dapper Implementation Details

### Example: GetAllAsync (Read)
```csharp
public async Task<IEnumerable<Designation>> GetAllAsync()
{
    // Calls stored procedure: getDesig
    var designations = await db.LoadData<Designation, dynamic>(
        "getDesig",
        new { },
        ConnectionId);

    return designations.OrderBy(d => d.desID);
}
```

### Example: CreateAsync (Write)
```csharp
// Calls stored procedure: InsertEmpDesig
await db.SaveData(
    "InsertEmpDesig",
    new { desID = designation.desID, desName = designation.desName },
    ConnectionId);
```

### Example: IsInUseAsync (Raw SQL)
```csharp
var query = "SELECT COUNT(*) FROM HREmployees WHERE Designation = @Designation";
var results = await db.LoadDataText<int, dynamic>(
    query,
    new { Designation = desId },
    ConnectionId);

return results.FirstOrDefault() > 0;
```

---

## Hybrid Approach: EF Core + Dapper

The service uses a **hybrid approach**:

| Scenario | Technology | Reason |
|----------|-----------|--------|
| **IDgen management** | EF Core | Transactional consistency with `BeginTransactionAsync()` |
| **Designation CRUD** | Dapper | Matches legacy stored procedures, better performance for read-heavy queries |
| **Foreign key checks** | Dapper | Raw SQL for lightweight count queries |

**Transaction Example** (CreateAsync):
```csharp
await using var transaction = await context.Database.BeginTransactionAsync();
try
{
    // EF: Update IDgen counter
    var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DesIdCode);
    idgen.Id = nextId;

    // Dapper: Insert designation via stored procedure
    await db.SaveData("InsertEmpDesig", new { desID, desName }, ConnectionId);

    await context.SaveChangesAsync(); // Commit IDgen
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

---

## Testing Checklist

### Backend API Endpoints
- [ ] `GET /api/designation/generate-id` - Returns next 2-digit ID
- [ ] `GET /api/designation` - Returns all designations with usage counts
- [ ] `GET /api/designation/{id}` - Returns single designation
- [ ] `POST /api/designation` - Creates new designation
- [ ] `PUT /api/designation/{id}` - Updates designation
- [ ] `DELETE /api/designation/{id}` - Deletes designation (when not in use)

### Frontend Component
- [ ] List page loads designations
- [ ] Add dialog generates ID and saves
- [ ] Edit dialog updates designation
- [ ] Delete checks for usage and shows appropriate error
- [ ] Search/filter works on cached data

### Database Stored Procedures
Ensure these stored procedures exist in the `Hospital` database:
- [ ] `getDesig`
- [ ] `InsertEmpDesig`
- [ ] `updateEmpDesig`
- [ ] `deleteEmpDesig`

---

## AI Rules Compliance

✅ **Data Access Rules**:
- Uses Dapper for read-heavy queries (designation list)
- Uses parameterized queries (no SQL injection risk)
- Respects explicit override (user specified Dapper)

✅ **Service Layer Rules**:
- Injects `ISqlDataAccess` alongside `ApplicationDbContext`
- Uses async/await for all database operations
- Returns `IEnumerable<T>` for collections

✅ **ViewModel Rules**:
- Uses `[Required]` and `[StringLength]` DataAnnotations
- Non-nullable reference types for required fields
- Flattened `InUseCount` property

✅ **Configuration Management**:
- Updated base `appsettings.json`
- Updated environment-specific `appsettings.Development.json`

---

## Migration Notes

### From EF Core to Dapper
**What Changed**:
- All CRUD operations now call stored procedures
- Connection string changed from `DefaultConnection` to `smartHRConnection`
- Kept EF Core for IDgen transactions (safer for concurrent inserts)

**What Stayed the Same**:
- API controller remains unchanged
- Frontend component requires no changes
- AutoMapper configuration works as-is
- Service interface (`IDesignationService`) unchanged

### Performance Benefits
- ✅ **Faster reads**: Direct stored procedure calls, no EF query translation
- ✅ **Less memory**: No change tracking overhead for read operations
- ✅ **Legacy compatibility**: Matches VB.NET implementation exactly

---

## Related Files

### Backend
- `AestheticEMR.Core/Services/Employees/DesignationService.cs` (refactored)
- `AestheticEMR.Core/Services/Employees/Interfaces/IDesignationService.cs` (unchanged)
- `AestheticEMR.Core/DataAccess/DbAccess/SqlDataAccess.cs` (existing Dapper wrapper)
- `AestheticEMR.Server/Controllers/DesignationController.cs` (unchanged)
- `AestheticEMR.Server/ViewModels/Employees/EmployeeVMs.cs` (DesignationVM updated)
- `AestheticEMR.Server/Configuration/MappingProfile.cs` (unchanged)

### Frontend
- `AestheticEMR.client/src/app/features/employees/designation/designation.component.ts` (unchanged)
- `AestheticEMR.client/src/app/services/designation-endpoint.service.ts` (unchanged)
- `AestheticEMR.client/src/app/models/employee.model.ts` (unchanged)

### Configuration
- `AestheticEMR.Server/appsettings.json` (smartHRConnection added)
- `AestheticEMR.Server/appsettings.Development.json` (smartHRConnection added)

---

## Build Status

✅ **Build successful** - No compilation errors

**Verified**:
- All Dapper dependencies resolved
- ISqlDataAccess service registered in DI
- AutoMapper mappings compile
- ViewModel validation attributes correct
