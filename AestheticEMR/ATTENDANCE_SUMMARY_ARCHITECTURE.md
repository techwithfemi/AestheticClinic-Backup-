# AttendanceSummaryComponent Architecture - Complete Reference

## Overview

`AttendanceSummaryComponent` is a **reusable patient context header component** designed to display patient information consistently across the entire application. It will be used in:

- ✅ **Billing Receipts Dialog** (IMPLEMENTED)
- ⚠️ **Billing Invoices Dialog** (NEEDS PHOTO FIX)
- 🔮 **Dental Clinic** (FUTURE)
- 🔮 **Spa Services** (FUTURE)
- 🔮 **Aesthetics Procedures** (FUTURE)
- 🔮 **Frontdesk Consent Forms** (FUTURE)

---

## Component Architecture

### Input Properties
```typescript
@Input() attendance?: VwhRecord;      // Patient context data
@Input() photo?: string;              // Base64 photo data URI
@Input() compact = false;             // Compact vs. full display
```

### Required Data Structure
```typescript
interface VwhRecord {
  consultId: string;
  pNo: string;
  fullname: string;
  age?: number;
  retainName?: string;  // Company/Retainer name
  clinicType?: string;

  // ✅ CRITICAL: Must include for photo display
  patientPhotoBase64?: string;  // e.g., "data:image/jpeg;base64,/9j/4AAQSk..."
}
```

---

## The Photo Data Pipeline

### Complete End-to-End Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                     BACKEND (C# / .NET)                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. Controller receives request with patient PNo                │
│  2. Queries HPatient table by PNo                              │
│  3. Gets PatPix column (byte[])                                │
│  4. Checks if PatPix has data:                                 │
│     if (patient?.PatPix != null && patient.PatPix.Length > 0) │
│  5. Converts byte[] to Base64 string:                          │
│     string base64 = Convert.ToBase64String(patient.PatPix)    │
│  6. Creates data URI:                                          │
│     string dataUri = $"data:image/jpeg;base64,{base64}"      │
│  7. Returns in ViewModel with property PatientPhotoBase64      │
│                                                                 │
│  Example Response:                                             │
│  {                                                              │
│    "consultId": "C202606010001",                              │
│    "fullname": "John Doe",                                    │
│    "patientPhotoBase64": "data:image/jpeg;base64,/9j/4AA..." │
│  }                                                              │
│                                                                 │
└────────────────────────┬──────────────────────────────────────┘
                        │ HTTP Response
                        ▼
┌─────────────────────────────────────────────────────────────────┐
│                    FRONTEND (TypeScript)                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  1. API service receives JSON response                         │
│  2. Maps to VwhRecord interface (includes patientPhotoBase64)  │
│  3. Component stores in property: attendanceSummary            │
│  4. Template passes photo to component:                        │
│     [photo]="attendanceSummary?.patientPhotoBase64"          │
│  5. AttendanceSummaryComponent receives via @Input             │
│  6. Component renders <img [src]="photoSource">               │
│  7. Browser displays photo                                     │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Critical Implementation Points

### ✅ MUST DO

1. **Backend: Load Photo from HPatient**
   ```csharp
   var patient = await context.HPatients
       .FirstOrDefaultAsync(p => p.Pno == record.PNo);

   if (patient?.PatPix != null && patient.PatPix.Length > 0)
   {
       patientPhoto = $"data:image/jpeg;base64,{Convert.ToBase64String(patient.PatPix)}";
   }
   ```

2. **Backend: Include in ViewModel**
   ```csharp
   public class YourSummaryVM
   {
       public string PatientPhotoBase64 { get; set; }  // ✅ REQUIRED
   }
   ```

3. **Frontend: Add to Model**
   ```typescript
   export interface YourContext {
       patientPhotoBase64?: string;  // ✅ REQUIRED
   }
   ```

4. **Frontend: Use Safe Navigation**
   ```html
   [photo]="attendanceSummary?.patientPhotoBase64"  <!-- Safe! -->
   ```

---

### ❌ DO NOT

1. ❌ Skip photo loading: `// Photo loading removed`
2. ❌ Use wrong property name: `[photo]="attendanceSummary.photo"`
3. ❌ Pass raw byte array: `PatientPhotoBase64 = patient.PatPix`
4. ❌ Construct manually: `return { photo: null }`
5. ❌ Forget model property: Interface without patientPhotoBase64
6. ❌ Unsafe navigation: `[photo]="attendanceSummary.patientPhotoBase64"` (without `?`)

---

## Real Example: Receipt Entry Dialog

### Backend (C#)
```csharp
[HttpGet("vwh-record/{consultId}")]
public async Task<IActionResult> GetVwhRecordSummary(string consultId)
{
    var record = await context.VwhRecords
        .FirstOrDefaultAsync(x => x.ConsultId == consultId);

    // ✅ Load photo
    string? patientPhoto = null;
    if (!string.IsNullOrEmpty(record.PNo))
    {
        var patient = await context.HPatients
            .FirstOrDefaultAsync(p => p.Pno == record.PNo);

        if (patient?.PatPix != null && patient.PatPix.Length > 0)
        {
            string base64String = Convert.ToBase64String(patient.PatPix);
            patientPhoto = $"data:image/jpeg;base64,{base64String}";
        }
    }

    // ✅ Include in response
    return Ok(new VwhRecordSummaryVM
    {
        ConsultId = record.ConsultId,
        PNo = record.PNo,
        Fullname = record.Fullname,
        PatientPhotoBase64 = patientPhoto  // ✅ INCLUDE THIS
    });
}
```

### Frontend TypeScript Model
```typescript
export interface VwhRecord {
  consultId: string;
  pNo: string;
  fullname: string;
  age?: number;

  patientPhotoBase64?: string;  // ✅ ADD THIS
}
```

### Frontend Component
```typescript
export class ReceiptEntryDialogComponent {
  attendanceSummary?: VwhRecord;

  private loadAttendanceSummary(billNo: string): void {
    this.endpoint.getVwhRecordSummaryEndpoint<VwhRecord>(billNo)
      .subscribe({
        next: summary => {
          this.attendanceSummary = summary;  // ✅ Has photo!
        }
      });
  }
}
```

### Frontend Template
```html
<app-attendance-summary
  [attendance]="attendanceSummary"
  [photo]="attendanceSummary?.patientPhotoBase64">  <!-- ✅ CORRECT -->
</app-attendance-summary>
```

---

## Property Name Reference

### Naming Convention
Always use **`patientPhotoBase64`** across all modules:

- ✅ ViewModel property: `public string? PatientPhotoBase64 { get; set; }`
- ✅ TypeScript property: `patientPhotoBase64?: string`
- ✅ Template binding: `[photo]="something?.patientPhotoBase64"`
- ✅ Database field: `HPatient.PatPix` (byte array - backend converts)

### Why This Name?
- Clear it's a photo
- Clear it's a patient
- Clear it's base64 encoded
- Clear it's a data URI for the browser

---

## File Checklist for New Clinical Pages

When implementing on a new page, update these files:

### Backend
- [ ] `Models/Legacy/YourModel.cs` - Data model
- [ ] `ViewModels/YourVM.cs` - Add `PatientPhotoBase64` property
- [ ] `Controllers/YourController.cs` - Load photo from HPatient
- [ ] `Services/IYourService.cs` - If needed
- [ ] `Services/YourService.cs` - If needed

### Frontend
- [ ] `models/your.model.ts` - Add `patientPhotoBase64?: string`
- [ ] `services/your-endpoint.service.ts` - Already has photo (from API)
- [ ] `features/your-page/your-page.component.ts` - Import AttendanceSummaryComponent
- [ ] `features/your-page/your-page.component.html` - Add component with photo binding

---

## Testing Strategy

### For Receipt Dialog (Already Implemented)
```bash
1. Open Receipt Dialog
2. Select patient from attendance
3. Verify photo appears
4. Check Network → /api/billing/vwh-record/{billNo}
5. Confirm response includes patientPhotoBase64 with valid data URI
6. Check Console for no errors
```

### For New Clinical Pages
Apply same testing when implemented.

---

## Troubleshooting

### Photo Not Displaying
**Check:**
1. Network tab → Is patientPhotoBase64 in response? YES → Go to 2
2. Browser console → Image loading error? YES → Photo is corrupted base64
3. Template → Using correct property name? Check: `patientPhotoBase64`
4. Model → Does interface have property? Add if missing
5. Backend → Is PatPix data in HPatient? Run SQL check

### Broken Photo
**Cause:** PatPix data is NULL or corrupted
```sql
-- Check
SELECT Pno, DATALENGTH(PatPix) FROM HPatient WHERE Pno = 'P123456'
```

### TypeScript Error: "not assignable to type 'string'"
**Cause:** Missing property or wrong type in interface
```typescript
// Fix: Add to interface
patientPhotoBase64?: string;  // Must be string, not byte[]
```

---

## Performance Notes

### Photo Size
- Average patient photo: 50-200 KB
- Base64 encoded adds ~33% overhead
- Data URI in response: 65-270 KB additional per request

### Optimization Options (Future)
1. Compress images before storing
2. Load photos separately if large
3. Implement caching strategy
4. Thumbnail first, full photo on demand

---

## Summary Table

| Aspect | Location | Format | Required |
|--------|----------|--------|----------|
| Database | `HPatient.PatPix` | byte[] | ✅ |
| Backend Conversion | Controller | base64 string | ✅ |
| Data URI Format | ViewModel Property | `data:image/jpeg;base64,...` | ✅ |
| API Response | JSON | `"patientPhotoBase64": "data:..."` | ✅ |
| Frontend Model | TypeScript Interface | `patientPhotoBase64?: string` | ✅ |
| Component Input | `@Input()` | Base64 string | ✅ |
| Component Display | Template | `<img [src]="photoSource">` | ✅ |

---

## Next Steps

1. **Receipt Dialog** → ✅ Already implemented
2. **Invoice Dialog** → Review and apply same photo loading
3. **Dental Pages** → Apply this pattern when header is added
4. **Spa Services** → Apply this pattern when header is added
5. **Aesthetics Procedures** → Apply this pattern when header is added

---

**Last Updated:** 2026-06-01  
**Author:** AI Assistant  
**Status:** Complete Reference Documentation  
**Confidence:** High - Implemented and tested in Receipt Dialog
