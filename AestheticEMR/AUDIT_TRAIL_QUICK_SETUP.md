# Quick Copy-Paste Setup for Audit Trail System

## Step 1: Add to ApplicationDbContext

**File**: `AestheticEMR/AestheticEMR.Core/Infrastructure/ApplicationDbContext.cs`

**Find this line** (around line 104):
```csharp
public DbSet<AestheticSignedConsent> AestheticSignedConsents { get; set; }
```

**Add after it**:
```csharp
public DbSet<AuditLog> AuditLogs { get; set; }
```

**Full context** (lines 100-107 should look like):
```csharp
public DbSet<AestheticPatient> AestheticPatients { get; set; }
public DbSet<AestheticConsultation> AestheticConsultations { get; set; }
public DbSet<AestheticPhoto> AestheticPhotos { get; set; }
public DbSet<AestheticConsentTemplate> AestheticConsentTemplates { get; set; }
public DbSet<AestheticSignedConsent> AestheticSignedConsents { get; set; }
public DbSet<AuditLog> AuditLogs { get; set; }
public DbSet<DentalImaging> DentalImagings { get; set; }
```

---

## Step 2: Register Service in Program.cs

**File**: `AestheticEMR/AestheticEMR.Server/Program.cs`

**Find the section** with other service registrations (search for `builder.Services.AddScoped`):

**Example context** (may be around line 40-60):
```csharp
builder.Services.AddScoped<IAestheticService, AestheticService>();
builder.Services.AddScoped<IProductService, ProductService>();
// Add here:
builder.Services.AddScoped<IAuditService, AuditService>();
```

**Add this import** at the top of the file if not present:
```csharp
using AestheticEMR.Core.Services.Aesthetics;
```

---

## Step 3: Run Database Migration

**Open Terminal/PowerShell** and run:

```powershell
cd C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.Server

dotnet ef database update
```

**Expected output**:
```
Build started...
Build succeeded.
Applying migration '20250101000000_AddAuditLogTable'.
Done.
```

---

## Step 4A: Add Route to aesthetics.routes.ts

**File**: `AestheticEMR/AestheticEMR.client/src/app/features/aesthetics/aesthetics.routes.ts`

**Add this route** (typically in the export const Routes array):

```typescript
{
  path: 'audit-trail',
  loadComponent: () => import('./audit-trail/audit-trail.component').then(m => m.AuditTrailComponent),
  canActivate: [AuthGuard],
  title: 'Audit Trail & Incidents'
}
```

**Example context** (should look similar to):
```typescript
export const AESTHETICS_ROUTES: Routes = [
  { path: '', redirectTo: 'procedures', pathMatch: 'full' },
  { path: 'procedures', loadComponent: () => import('./procedures/procedures.component')... },
  { path: 'botox', loadComponent: () => import('./procedures/procedures.component')... },
  { path: 'laser', loadComponent: () => import('./procedures/procedures.component')... },
  { path: 'photos', redirectTo: 'procedures' },
  // ADD HERE:
  {
    path: 'audit-trail',
    loadComponent: () => import('./audit-trail/audit-trail.component').then(m => m.AuditTrailComponent),
    canActivate: [AuthGuard],
    title: 'Audit Trail & Incidents'
  }
];
```

---

## Step 4B: Update Sidebar Navigation

**File**: `AestheticEMR/AestheticEMR.client/public/assets/navigation.json`

**Find the Aesthetics section** (search for `"name": "Aesthetics"` or `"Aesthetics"`):

**Current structure** looks like:
```json
{
  "name": "Aesthetics",
  "icon": "spa",
  "subItems": [
    {
      "name": "Procedures",
      "routeOrAction": "aesthetics/procedures",
      ...
    }
  ]
}
```

**Add this item** to the `subItems` array:
```json
{
  "name": "Audit Trail",
  "routeOrAction": "aesthetics/audit-trail",
  "icon": "history",
  "resourcePermission": null,
  "subItems": []
}
```

**Updated Aesthetics section** should look like:
```json
{
  "name": "Aesthetics",
  "icon": "spa",
  "subItems": [
    {
      "name": "Procedures",
      "routeOrAction": "aesthetics/procedures",
      "icon": "medical_services",
      "resourcePermission": null,
      "subItems": []
    },
    {
      "name": "Audit Trail",
      "routeOrAction": "aesthetics/audit-trail",
      "icon": "history",
      "resourcePermission": null,
      "subItems": []
    }
  ]
}
```

---

## Verification

After completing all steps, run:

```bash
dotnet build
```

Should complete with:
```
Build succeeded.
```

Then test the app:
```bash
dotnet run
# or in separate terminal
ng serve
```

Navigate to: **Aesthetics → Audit Trail**

Expected: Dashboard with tabs for Open Incidents, All Incidents, and Consultation Trail.

---

## Quick Reference

| Component | Status | Location |
|-----------|--------|----------|
| AuditLog Model | ✅ Created | `Models/Aesthetic/AuditLog.cs` |
| AuditService | ✅ Created | `Services/Aesthetics/AuditService.cs` |
| AuditController | ✅ Created | `Controllers/AuditController.cs` |
| UI Component | ✅ Created | `audit-trail/audit-trail.component.ts` |
| Migration | ✅ Created | `Migrations/20250101000000_*.cs` |
| **DbSet** | ⏳ **Needs manual add** | `ApplicationDbContext.cs` |
| **Service Registration** | ⏳ **Needs manual add** | `Program.cs` |
| **Database Migration** | ⏳ **Needs to run** | `dotnet ef database update` |
| **Route** | ⏳ **Needs manual add** | `aesthetics.routes.ts` |
| **Navigation** | ⏳ **Needs manual add** | `navigation.json` |

---

## Troubleshooting

**Q: Build fails with "DbSet not found"**  
A: You need to add the DbSet line to ApplicationDbContext (Step 1)

**Q: API returns 404**  
A: Service not registered. Add to Program.cs (Step 2)

**Q: Component doesn't load**  
A: Route not added. Add to aesthetics.routes.ts (Step 4A)

**Q: Menu item doesn't appear**  
A: Navigation not updated. Edit navigation.json (Step 4B)

**Q: Database migration fails**  
A: Ensure DbSet is added first (Step 1), then run update again
