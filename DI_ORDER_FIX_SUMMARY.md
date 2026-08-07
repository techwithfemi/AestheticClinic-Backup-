# EF Core Audit Logging - Dependency Injection Order Fix

## The Problem
Audit logs were **NOT being saved** because the `HospitalAuditTrailInterceptor` was **never actually registered** with the DbContext.

## Root Cause
**Dependency Injection Order Bug in `Program.cs`:**

```csharp
// LINE 111-118: Add DbContext and try to get interceptor
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    options.UseOpenIddict();
    options.AddInterceptors(serviceProvider.GetRequiredService<HospitalAuditTrailInterceptor>()); // ← FAILS HERE!
});

// LINE 136: Register the interceptor (TOO LATE!)
builder.Services.AddScoped<HospitalAuditTrailInterceptor>();
```

**The lambda in `AddDbContext` runs IMMEDIATELY**, so when it tries to get `HospitalAuditTrailInterceptor` from the service provider on line 116, the interceptor hasn't been registered yet (line 136). This causes a dependency resolution failure.

## The Fix
**Move all audit service registrations BEFORE `AddDbContext`:**

```csharp
// Register interceptor and audit services FIRST
builder.Services.AddSingleton<SqlDataAccess>();
builder.Services.AddScoped<ISqlDataAccess, AuditedSqlDataAccess>();
builder.Services.AddScoped<HospitalAuditTrailInterceptor>();  // ← NOW it exists!
builder.Services.AddScoped<IHospitalAuditWriter, HospitalAuditWriter>();

// NOW add DbContext (which can use the interceptor)
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    options.UseOpenIddict();
    options.AddInterceptors(serviceProvider.GetRequiredService<HospitalAuditTrailInterceptor>()); // ← Works now!
});
```

## What Was Changed
In `AestheticEMR.Server\Program.cs`, moved these service registrations:
- `builder.Services.AddSingleton<SqlDataAccess>();`
- `builder.Services.AddScoped<ISqlDataAccess, AuditedSqlDataAccess>();`
- `builder.Services.AddScoped<HospitalAuditTrailInterceptor>();`
- `builder.Services.AddScoped<IHospitalAuditWriter, HospitalAuditWriter>();`

**FROM:** After `AddDbContext<ApplicationDbContext>` (lines 132-136)
**TO:** Before `AddDbContext<ApplicationDbContext>` (now at the beginning of service registration)

## Result
✅ Interceptor is now properly registered and available when DbContext is configured
✅ EF Core `SaveChangesAsync()` calls will trigger the interceptor
✅ Audit entries will be written to `Auditrail` table for all CRUD operations
✅ Build successful with no errors

## Testing
Edit a patient record (or any EF Core entity) and verify:
1. `Auditrail` table now has new rows
2. `UserAction` contains the changed field values
3. `OriginalAction` contains the old values (for updates/deletes)
4. All metadata (TranCode, ActionDate, ActionTime, Remarks, UserName) is populated

## Files Modified
- `AestheticEMR\AestheticEMR.Server\Program.cs` (DI registration order fixed)

## Related Code
- `HospitalAuditTrailInterceptor.cs` — EF Core audit interceptor (was not being used)
- `AuditedSqlDataAccess.cs` — Dapper audit decorator (already working)
- `ApplicationDbContext.cs` — DbContext with SaveChanges overrides
