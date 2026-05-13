# Comprehensive Audit Trail System - Implementation Guide

## Overview
A complete audit trail system has been created for tracking all changes, complications, and safety events in the Aesthetic Procedures module.

## Components Created

### 1. Database Model
**File**: `AestheticEMR/AestheticEMR.Core/Models/Aesthetic/AuditLog.cs`
- Comprehensive audit log entity with 25+ tracking fields
- Tracks event type, severity, user, timestamp, resolution status
- Supports field-level change tracking (before/after values)
- Tags system for categorization (#allergy, #complication, #vascular, #incident, etc.)

### 2. Service Layer
**File**: `AestheticEMR/AestheticEMR.Core/Services/Aesthetics/Interfaces/IAuditService.cs`

#### Key Methods:
- `LogEventAsync()` - Log any audit event
- `LogComplicationAsync()` - Log procedure complications
- `LogAllergyEventAsync()` - Log allergy detections
- `LogSafetyIncidentAsync()` - Log critical safety incidents
- `LogFieldChangeAsync()` - Track field-level changes
- `GetConsultationAuditTrailAsync()` - Retrieve all events for a consultation
- `GetPatientAuditTrailAsync()` - Retrieve all events for a patient
- `GetOpenIncidentsAsync()` - Get unresolved incidents
- `GetIncidentsAsync()` - Filter by severity and date range
- `MarkAsReviewedAsync()` - Mark incidents as reviewed with resolution notes
- `PurgeOldEntriesAsync()` - Retention policy (default 365 days)

### 3. API Controller
**File**: `AestheticEMR/AestheticEMR.Server/Controllers/AuditController.cs`

#### Endpoints:
- `GET /api/audit/incidents/open` - Get all open incidents
- `GET /api/audit/incidents` - Filter incidents by severity and date
- `GET /api/audit/consultation/{consultationId}` - Consultation audit trail
- `GET /api/audit/patient/{patientId}` - Patient audit trail
- `POST /api/audit/complication` - Log complication
- `POST /api/audit/safety-incident` - Log safety incident
- `POST /api/audit/allergy` - Log allergy event
- `PUT /api/audit/{auditLogId}/review` - Mark as reviewed

### 4. Angular Component (Frontend)
**File**: `AestheticEMR/AestheticEMR.client/src/app/features/aesthetics/audit-trail/audit-trail.component.ts`

#### Features:
- **Open Incidents Tab**: Shows unresolved incidents with quick review buttons
- **All Incidents Tab**: Filtered search by severity, date range with pagination
- **Consultation Trail Tab**: View complete change history for a specific consultation
- Severity color coding (Info, Warning, Error, Critical)
- Status tracking (Open, Reviewed, Resolved, Escalated)
- Real-time refresh capability
- Responsive design

### 5. Database Migration
**File**: `AestheticEMR/AestheticEMR.Server/Migrations/20250101000000_AddAuditLogTable.cs`
- Creates `AppAuditLogs` table with proper indexes
- Foreign keys to AestheticConsultations and AestheticPatients
- Indexes on EventDateTime, Severity, Status for query performance

## Implementation Steps

### Step 1: Register Service in Program.cs
```csharp
using AestheticEMR.Core.Services.Aesthetics;

// Add to service registration section:
builder.Services.AddScoped<IAuditService, AuditService>();
```

### Step 2: Apply Database Migration
```powershell
cd AestheticEMR.Server
dotnet ef database update
```

### Step 3: Add Audit Route to Aesthetics Routes
Edit: `AestheticEMR/AestheticEMR.client/src/app/features/aesthetics/aesthetics.routes.ts`

```typescript
{
  path: 'audit-trail',
  loadComponent: () => import('./audit-trail/audit-trail.component').then(m => m.AuditTrailComponent),
  canActivate: [AuthGuard],
  title: 'Audit Trail & Incidents'
}
```

### Step 4: Update Sidebar Navigation
Edit: `AestheticEMR/AestheticEMR.client/public/assets/navigation.json`

Add under Aesthetics section:
```json
{
  "name": "Audit Trail",
  "routeOrAction": "aesthetics/audit-trail",
  "icon": "history",
  "resourcePermission": null,
  "subItems": []
}
```

## Integration with Procedures Component

### In Procedures.component.ts, update safety feature logging:

```typescript
// Inject the audit service
private readonly auditService = inject(IAuditService);

// Log complications when reported
async onComplicationsToggled(tab: string): Promise<void> {
  const hasComplications = this.form.controls[tab].get('complications')?.value ?? false;
  if (hasComplications) {
    await this.auditService.LogComplicationAsync(
      this.currentConsultationId(),
      this.form.controls.patientId.value,
      tab,
      `Complication in ${tab}`,
      `Complication reported in ${tab} tab`,
      'Warning'
    );
    this.reportedComplications.update(c => [...c, { tab, timestamp: new Date() }]);
  }
}

// Log allergy events
async onAllergiesChanged(): Promise<void> {
  const allergies = this.consultationGroup.get('allergySelections')?.value || [];
  for (const allergy of allergies) {
    await this.auditService.LogAllergyEventAsync(
      this.form.controls.patientId.value,
      allergy,
      `Allergy detected: ${allergy}`,
      'Error'
    );
  }
}
```

## Audit Event Types & Severity Levels

### Event Types:
- `Create` - New record created
- `Update` - Field modified
- `Delete` - Record deleted
- `Complication` - Adverse event occurred
- `Allergy` - Allergy detected/documented
- `Safety Incident` - Critical safety concern
- `Review` - Incident reviewed by staff

### Severity Levels:
- `Info` - Routine logging
- `Warning` - Caution needed
- `Error` - Problem occurred
- `Critical` - Immediate action required

### Tags Examples:
- `#allergy` - Allergy-related
- `#complication` - Adverse event
- `#vascular` - Vascular emergency
- `#ptosis` - Botox ptosis
- `#incident` - Safety incident
- `#infection` - Infection risk
- `#safety` - Safety concern

## Query Examples

### Get all critical incidents from past 7 days:
```csharp
var incidents = await auditService.GetIncidentsAsync("Critical", 
    DateTime.UtcNow.AddDays(-7), 
    DateTime.UtcNow);
```

### Get patient's complete audit trail:
```csharp
var trail = await auditService.GetPatientAuditTrailAsync(patientId);
```

### Log a vascular emergency:
```csharp
await auditService.LogSafetyIncidentAsync(
    consultationId,
    patientId,
    "Vascular Occlusion - Suspected",
    "Patient reported blanching and pain. Stopped injection, massaging area.",
    "Critical",
    "#vascular #emergency"
);
```

### Mark incident as reviewed:
```csharp
await auditService.MarkAsReviewedAsync(
    auditLogId,
    currentUserId,
    "Reviewed patient, no permanent harm. Updated post-care protocol."
);
```

## Performance Considerations

1. **Indexes**: EventDateTime, Severity, Status for fast filtering
2. **Pagination**: UI component supports pagination for large datasets
3. **Retention Policy**: 365-day default retention; old entries purged automatically
4. **Async Operations**: All database calls are async/await

## Security Notes

1. All audit endpoints require `[Authorize]` attribute
2. User ID captured via `IUserIdAccessor`
3. Source IP can be logged for additional security tracking
4. ReviewedBy field ensures accountability
5. Immutable creation timestamps prevent tampering

## Testing Checklist

- [ ] Audit table created in database
- [ ] IAuditService registered in DI
- [ ] AuditController endpoints accessible
- [ ] Complications logged when toggled
- [ ] Allergies logged when selected
- [ ] Safety incidents logged and queryable
- [ ] Open incidents dashboard functional
- [ ] Audit trail component renders
- [ ] Filters work correctly
- [ ] Review functionality updates status
- [ ] Old entries purged after retention period

## Next Steps

1. Wire the AuditService into ProceduresComponent
2. Test each endpoint via Postman/API explorer
3. Review sample incidents in dashboard
4. Configure retention policy if needed
5. Train staff on using audit trail viewer
