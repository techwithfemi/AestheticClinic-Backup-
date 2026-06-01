# AttendanceSummaryComponent - Future Implementation Guide

## Summary for Clinical Pages (Dental, Spa, Aesthetics)

The `AttendanceSummaryComponent` is a **reusable patient header component** that displays:
- Patient name, age, company/company
- Bill number, clinic type, client category
- **Patient photo** (base64 data from backend)

It will be used across multiple clinical modules to provide a consistent patient context header.

---

## Why This Matters

### Before (Current Custom Implementation)
- Each clinical page implements its own patient info display
- Photo loading logic scattered across multiple components
- Inconsistent data structures and photo handling
- Duplicated code

### After (With AttendanceSummaryComponent)
- ✅ Single reusable component
- ✅ Consistent patient context display
- ✅ Centralized photo handling
- ✅ DRY principle applied
- ✅ Easier to maintain and update

---

## Quick Start for Clinical Pages

### Step 1: Ensure Backend Data Includes Photo

**If your API endpoint returns patient data, ensure it includes:**

```csharp
// Your endpoint should return an object with:
public class YourPatientSummaryVM
{
    public string ConsultId { get; set; }
    public string PNo { get; set; }
    public string Fullname { get; set; }
    public int? Age { get; set; }
    public string? RetainName { get; set; }
    public string? ClinicType { get; set; }

    // ✅ MUST INCLUDE THIS:
    public string? PatientPhotoBase64 { get; set; }  // data:image/jpeg;base64,...
}
```

### Step 2: Load Photo in Backend

```csharp
// In your controller endpoint:
var patient = await context.HPatients.FirstOrDefaultAsync(p => p.Pno == pNo);

string? patientPhoto = null;
if (patient?.PatPix != null && patient.PatPix.Length > 0)
{
    string base64String = Convert.ToBase64String(patient.PatPix);
    patientPhoto = $"data:image/jpeg;base64,{base64String}";
}

// Include in response
return Ok(new YourPatientSummaryVM 
{ 
    // ... other properties ...
    PatientPhotoBase64 = patientPhoto
});
```

### Step 3: Update Frontend Model

```typescript
// In your models file
export interface YourPatientContext {
  consultId: string;
  pNo: string;
  fullname: string;
  age?: number;
  retainName?: string;
  clinicType?: string;

  // ✅ MUST ADD THIS:
  patientPhotoBase64?: string;
}
```

### Step 4: Use in Component

```typescript
import { AttendanceSummaryComponent } from '../../components/attendance-summary/attendance-summary.component';

@Component({
  imports: [
    // ... other imports ...
    AttendanceSummaryComponent
  ],
  template: `
    <!-- Patient header with photo -->
    <app-attendance-summary
      [attendance]="patientContext"
      [photo]="patientContext?.patientPhotoBase64"
      [compact]="false">
    </app-attendance-summary>

    <!-- Rest of your page -->
  `
})
export class YourClinicalPageComponent {
  patientContext?: YourPatientContext;

  ngOnInit() {
    this.loadPatientContext();
  }

  private loadPatientContext(): void {
    // Load from your API endpoint
    this.yourEndpoint.getPatientContext(consultId).subscribe(context => {
      this.patientContext = context;  // Already has photo!
    });
  }
}
```

---

## Common Data Models to Update

### 1. Dental Pages
**Files to Update:**
- `models/dental.model.ts` - DentalPatient/DentalContext interface
- `services/dental-endpoint.service.ts` - Load photo endpoint
- `features/dental/dental-page.component.ts` - Use component

**Backend Files:**
- `ViewModels/Dental/DentalVMs.cs` - Add PatientPhotoBase64
- `Controllers/DentalController.cs` - Load photo from HPatient

### 2. Spa Pages
**Files to Update:**
- `models/aesthetic.model.ts` - SpaPatientOption or similar
- `services/aesthetic-endpoint.service.ts` - Load photo endpoint
- `features/spa/services/spa-dialog.component.ts` - Use component

**Backend Files:**
- `ViewModels/Aesthetic/SpaVMs.cs` - Add PatientPhotoBase64
- `Controllers/AestheticController.cs` - Load photo from HPatient

### 3. Aesthetics Pages
**Files to Update:**
- `models/aesthetic.model.ts` - AestheticPatient interface
- `services/aesthetic-endpoint.service.ts` - Load photo endpoint
- `features/aesthetics/procedures/procedures.component.ts` - Use component

**Backend Files:**
- `ViewModels/Aesthetic/AestheticVMs.cs` - Add PatientPhotoBase64
- `Controllers/AestheticController.cs` - Load photo from HPatient

---

## Photo Loading Pattern (for all clinical pages)

### Pattern to Follow
```
Clinical Page Opens
  ↓
Load patient context via API
  ↓
Backend: Get HPatient by PNo
  ↓
Backend: Convert PatPix (byte[]) → base64 data URI
  ↓
Backend: Return with PatientPhotoBase64 property
  ↓
Frontend: Receive patient context with photo data
  ↓
Frontend: Pass to AttendanceSummaryComponent
  ↓
Photo displays in header
```

### Code Template for Each Clinical Page

```typescript
// Step 1: Create interface with photo
export interface ClinicalPatientContext {
  consultId: string;
  pNo: string;
  fullname: string;
  patientPhotoBase64?: string;  // ✅ MUST HAVE
}

// Step 2: Load in component
private loadPatientContext(consultId: string): void {
  this.endpoint.getClinicalPatientContext<ClinicalPatientContext>(consultId)
    .subscribe({
      next: context => {
        this.patientContext = context;  // Photo already loaded!
      },
      error: () => { this.patientContext = undefined; }
    });
}

// Step 3: Use in template
<app-attendance-summary
  [attendance]="patientContext"
  [photo]="patientContext?.patientPhotoBase64">
</app-attendance-summary>
```

---

## Backend Implementation Template

### For Each Clinical Module

```csharp
// In ViewModel
public class ClinicalPatientContextVM
{
    public string ConsultId { get; set; }
    public string PNo { get; set; }
    public string Fullname { get; set; }
    public string? PatientPhotoBase64 { get; set; }  // ✅ Add this
}

// In Controller
[HttpGet("{consultId}")]
public async Task<IActionResult> GetPatientContext(string consultId)
{
    try
    {
        // Get main clinical record
        var record = await context.YourClinicalTable
            .FirstOrDefaultAsync(x => x.ConsultId == consultId);

        if (record is null)
            return NotFound(consultId);

        // Load patient photo
        string? patientPhoto = null;
        if (!string.IsNullOrEmpty(record.PNo))
        {
            var patient = await context.HPatients
                .Where(p => p.Pno == record.PNo)
                .FirstOrDefaultAsync();

            if (patient?.PatPix != null && patient.PatPix.Length > 0)
            {
                string base64String = Convert.ToBase64String(patient.PatPix);
                patientPhoto = $"data:image/jpeg;base64,{base64String}";
            }
        }

        return Ok(new ClinicalPatientContextVM
        {
            // ... other properties ...
            PatientPhotoBase64 = patientPhoto  // ✅ Include photo
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving patient context");
        AddModelError("Unable to retrieve patient context");
        return BadRequest(ModelState);
    }
}
```

---

## Standardized Property Names (IMPORTANT!)

To ensure consistency across all clinical pages:

| Concept | Property Name | Type | Example |
|---------|---|---|---|
| Patient photo (data URI) | `patientPhotoBase64` | `string?` | `"data:image/jpeg;base64,/9j/4AAQSkZJRg..."` |
| Patient ID | `pNo` | `string` | `"P123456"` |
| Consultation ID | `consultId` | `string` | `"C202606010001"` |
| Patient name | `fullname` | `string` | `"John Doe"` |
| Patient age | `age` | `int?` | `35` |
| Company/Retainer | `retainName` | `string?` | `"ABC Company"` |
| Clinic type | `clinicType` | `string?` | `"DENTAL"`, `"SPA"`, `"AESTHETIC"` |

---

## Testing Checklist for New Clinical Page

- [ ] Backend endpoint returns patient context with photo
- [ ] Photo is base64 data URI: `data:image/jpeg;base64,...`
- [ ] Frontend model includes `patientPhotoBase64` property
- [ ] Component loads patient context via API
- [ ] Component binds: `[photo]="patientContext?.patientPhotoBase64"`
- [ ] AttendanceSummaryComponent is imported and declared
- [ ] Photo displays in header when page loads
- [ ] Placeholder icon shows if photo is missing
- [ ] No console errors about undefined properties
- [ ] Works with both patients that have and don't have photos

---

## Database Check

Before implementing on a clinical page, verify photos exist in your database:

```sql
-- Find patients with photos
SELECT TOP 20 
    Pno,
    PSurName,
    PFirstname,
    DATALENGTH(PatPix) as PhotoSizeBytes
FROM HPatient
WHERE PatPix IS NOT NULL
    AND DATALENGTH(PatPix) > 0
ORDER BY Pno;
```

If no results, photos may need to be populated first.

---

## Summary: The 5-Minute Checklist

When adding AttendanceSummaryComponent to a new clinical page:

1. **Backend:** Add `PatientPhotoBase64` property to ViewModel
2. **Backend:** Load photo from HPatient and convert to base64 in controller
3. **Frontend:** Add `patientPhotoBase64?: string` to model interface
4. **Frontend:** Load via API into component property of type VwhRecord (or compatible)
5. **Template:** `<app-attendance-summary [attendance]="patientContext" [photo]="patientContext?.patientPhotoBase64">`

That's it! Photo will display automatically.

---

**Last Updated:** 2026-06-01  
**Status:** Ready for Clinical Page Implementation  
**Next Steps:** Apply to Dental, Spa, and Aesthetics pages as needed
