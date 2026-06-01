# AttendanceSummaryComponent Usage - Future-Proof Guide

## Component Overview
- **Location:** `AestheticEMR/src/app/components/attendance-summary/attendance-summary.component.ts`
- **Purpose:** Display patient attendance/header information with optional photo
- **Requires:** `VwhRecord` model with `patientPhotoBase64` property

---

## Current Usage Locations

### ✅ 1. Receipt Entry Dialog (IMPLEMENTED)
**File:** `features/billing/receipts/receipt-entry-dialog.component.ts|html`
**Status:** ✅ FULLY IMPLEMENTED WITH PHOTO LOADING

**Data Source:** API endpoint `/api/billing/vwh-record/{billNo}`

---

### ⚠️ 2. Invoice Dialog (NEEDS REVIEW)
**File:** `features/billing/invoices/billing-invoice-dialog.component.ts|html`
**Status:** ⚠️ PARTIALLY IMPLEMENTED - Uses manual object construction

---

## Future Usage Locations (TO BE IMPLEMENTED)

### 🔮 3. Dental Clinic Header
**File:** `features/dental/dental-page.component.ts`
**Status:** 🔮 NOT YET IMPLEMENTED
**Planned Use:** Patient summary header when viewing dental records

### 🔮 4. Spa Services Header
**File:** `features/spa/services/spa-dialog.component.ts`
**Status:** 🔮 NOT YET IMPLEMENTED (Custom implementation exists)
**Planned Use:** Patient summary in spa session dialog

### 🔮 5. Aesthetics Procedures Header
**File:** `features/aesthetics/procedures/procedures.component.ts`
**Status:** 🔮 NOT YET IMPLEMENTED (Custom implementation exists)
**Planned Use:** Patient summary in aesthetics consultation

### 🔮 6. Frontdesk Consent Form
**File:** `features/frontdesk/consent-form/consent-form.component.ts`
**Status:** 🔮 NOT YET IMPLEMENTED
**Planned Use:** Patient header in consent form

---

## Key Requirement for All Uses

### ✅ The Photo MUST Come from the Backend

Whenever `AttendanceSummaryComponent` is used, the component needs:

```typescript
[attendance]="attendanceSummary"  // Must have patientPhotoBase64 property
[photo]="attendanceSummary?.patientPhotoBase64"
```

**Critical Points:**
1. ✅ `attendanceSummary` must have `patientPhotoBase64` property
2. ✅ Photo MUST be loaded from backend (not constructed in component)
3. ✅ Backend must convert byte array to base64 data URI
4. ✅ Always use safe navigation: `attendanceSummary?.patientPhotoBase64`

---

## Common Mistakes to Avoid

### ❌ Mistake 1: Manually Constructing Attendance Object Without Photo
```typescript
// WRONG - Photo data missing
get attendanceSummary(): VwhRecord {
  return {
    consultId: this.data.consultId,
    fullname: this.selectedPatient.name,
    // ... NO PHOTO LOADED
  };
}

// CORRECT - Load from API or ensure photo is included
this.endpoint.getVwhRecordSummaryEndpoint(billNo).subscribe(summary => {
  this.attendanceSummary = summary;  // Includes patientPhotoBase64
});
```

### ❌ Mistake 2: Wrong Property Name
```typescript
// WRONG
[photo]="attendanceSummary.photo"
[photo]="attendanceSummary.patientPhoto"
[photo]="attendanceSummary.photoBase64"

// CORRECT
[photo]="attendanceSummary?.patientPhotoBase64"
```

### ❌ Mistake 3: Forgetting Backend Conversion
```csharp
// WRONG - Returns byte array, won't work in browser
return new VwhRecordSummaryVM {
    PatientPhotoBase64 = patient.PatPix  // byte[] - WRONG!
};

// CORRECT - Convert to base64 data URI
string base64String = Convert.ToBase64String(patient.PatPix);
return new VwhRecordSummaryVM {
    PatientPhotoBase64 = $"data:image/jpeg;base64,{base64String}"
};
```

### ❌ Mistake 4: Missing Model Property
```typescript
// WRONG - Model doesn't include photo
export interface VwhRecord {
  consultId: string;
  fullname: string;
  // ... no patientPhotoBase64
}

// CORRECT - Add to model
export interface VwhRecord {
  consultId: string;
  fullname: string;
  patientPhotoBase64?: string;  // ✅ ADDED
}
```

---

## Implementation Checklist for New Clinical Pages

When adding `AttendanceSummaryComponent` to a new clinical page, ensure:

### Backend Changes
- [ ] ViewModel has `PatientPhotoBase64` property
- [ ] Controller loads photo from HPatient using patient PNo
- [ ] Photo is converted to base64 data URI
- [ ] Response includes photo in the attendance summary object

### Frontend Model Changes
- [ ] TypeScript interface includes `patientPhotoBase64?: string`
- [ ] Property is properly typed as optional string

### Frontend Component Changes
- [ ] Inject endpoint service to load attendance data
- [ ] Load attendance summary from API (not constructed manually)
- [ ] Ensure component property has type `VwhRecord`

### Frontend Template Changes
- [ ] Import `AttendanceSummaryComponent`
- [ ] Bind `[attendance]="attendanceSummary"`
- [ ] Bind `[photo]="attendanceSummary?.patientPhotoBase64"`
- [ ] Add null checks if using safe navigation

---

## Photo Data Pipeline (Reference)

### Backend Flow
```
HPatient.PatPix (byte[])
    ↓
Convert.ToBase64String(patPix)
    ↓
Create data URI: `data:image/jpeg;base64,{base64}`
    ↓
Set: VwhRecordSummaryVM.PatientPhotoBase64
    ↓
JSON Response
```

### Frontend Flow
```
API Response received
    ↓
Mapped to VwhRecord.patientPhotoBase64
    ↓
Passed to AttendanceSummaryComponent: [photo]="attendanceSummary?.patientPhotoBase64"
    ↓
Component receives via @Input() photo?: string
    ↓
Component renders: <img [src]="photoSource">
```

---

## Testing the Implementation

### For Each New Use of AttendanceSummaryComponent

1. **Open the page/dialog**
2. **Select a patient with photo**
3. **Verify photo appears in header**
4. **Check browser Network tab:**
   - Call relevant API endpoint
   - Verify response includes `patientPhotoBase64`
   - Confirm it's a valid data URI: `data:image/jpeg;base64,...`
5. **Check browser Console:**
   - No image loading errors
   - No undefined property errors

---

## Verification SQL

```sql
-- Verify which patients have photos
SELECT TOP 20 Pno, PSurName, PFirstname, DATALENGTH(PatPix) as PhotoSizeBytes
FROM HPatient
WHERE PatPix IS NOT NULL
ORDER BY Pno;

-- Should return: PNo, Name, and PhotoSizeBytes > 0
```

---

## Reference Files

| Item | Location |
|------|----------|
| Attendance Summary Component | `src/app/components/attendance-summary/` |
| Receipt Dialog (Reference) | `features/billing/receipts/receipt-entry-dialog.component` |
| Invoice Dialog (Reference) | `features/billing/invoices/billing-invoice-dialog.component` |
| Backend Endpoint | `/api/billing/vwh-record/{consultId}` |
| ViewModel (Backend) | `ViewModels/Legacy/VwhRecordSummaryVM.cs` |
| Model (Frontend) | `models/legacy/vwh-record.model.ts` |
| Photo Property Name | `patientPhotoBase64` |
| Source DB Field | `HPatient.PatPix` (byte[]) |

---

## Related Files That Implement Custom Photo Logic

These pages currently implement their own photo handling. Consider refactoring to use `AttendanceSummaryComponent`:

1. `features/spa/services/spa-dialog.component.ts` - Has custom photo display
2. `features/aesthetics/procedures/procedures.component.ts` - Has custom patient info display
3. `features/dental/dental-encounter-dialog.component.ts` - May need patient header

---

**Last Updated:** 2026-06-01  
**Status:** Documentation Updated for Future-Proof Implementation  
**Critical Note:** The photo data architecture must be consistent across ALL uses of AttendanceSummaryComponent
