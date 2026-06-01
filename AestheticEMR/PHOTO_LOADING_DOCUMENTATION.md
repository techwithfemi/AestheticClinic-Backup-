# Patient Photo Loading - Architecture Documentation

## Overview
Patient photos are displayed in the `AttendanceSummaryComponent` in two billing dialogs:
1. **Receipt Entry Dialog** (`receipt-entry-dialog.component`)
2. **Invoice Dialog** (`billing-invoice-dialog.component`)

## Photo Data Flow

### For Receipt Entry Dialog ✅
**Flow:**
```
Dialog Opens
  ↓
loadAttendanceSummary(billNo)
  ↓
BillingController.GetVwhRecordSummary(consultId)
  ↓
Load VwhRecord by consultId
  ↓
Load HPatient by PNo (from VwhRecord)
  ↓
Convert HPatient.PatPix (byte[]) → base64 data URI
  ↓
Return VwhRecordSummaryVM with PatientPhotoBase64
  ↓
Frontend receives VwhRecord with patientPhotoBase64
  ↓
Pass to AttendanceSummaryComponent: [photo]="attendanceSummary?.patientPhotoBase64"
  ↓
Photo displays in header
```

**Key Points:**
- ✅ Photo data comes from `/api/billing/vwh-record/{consultId}` endpoint
- ✅ Backend converts byte array to base64 data URI
- ✅ VwhRecordSummaryVM includes `PatientPhotoBase64` property
- ✅ VwhRecord TypeScript model includes `patientPhotoBase64` property
- ✅ Template passes photo directly from attendanceSummary

### For Invoice Dialog ⚠️
**Current Flow:**
```
Dialog Opens
  ↓
Manual construction of attendanceSummary object
  ↓
Uses selectedPatientInfo?.photo property
  ↓
selectedPatientPhoto getter returns this?.selectedPatientInfo?.photo
```

**Issue:**
- The `selectedPatientInfo` is constructed from attendance options selected by user
- If the attendance options DON'T include photo data, it won't display
- This is dependent on where `selectedPatientInfo` is populated

---

## Implementation Details

### Backend Changes Made
**File:** `AestheticEMR.Server/Controllers/BillingController.cs`
```csharp
[HttpGet("vwh-record/{consultId}")]
public async Task<IActionResult> GetVwhRecordSummary(string consultId)
{
    // Load VwhRecord
    var record = await context.VwhRecords.FirstOrDefaultAsync(x => x.ConsultId == consultId);

    // Load patient photo from HPatient
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

    // Return with photo
    return Ok(new VwhRecordSummaryVM { ..., PatientPhotoBase64 = patientPhoto });
}
```

### Backend Model Changes
**File:** `AestheticEMR.Server/ViewModels/Legacy/VwhRecordSummaryVM.cs`
```csharp
public class VwhRecordSummaryVM
{
    // ... existing properties ...
    public string? PatientPhotoBase64 { get; set; }  // ✅ ADDED
}
```

### Frontend Model Changes
**File:** `AestheticEMR.client/src/app/models/legacy/vwh-record.model.ts`
```typescript
export interface VwhRecord {
    // ... existing properties ...
    patientPhotoBase64?: string;  // ✅ ADDED
}
```

### Frontend Component
**File:** `receipt-entry-dialog.component.html`
```html
<app-attendance-summary
  [compact]="true"
  [attendance]="attendanceSummary"
  [photo]="attendanceSummary?.patientPhotoBase64">  <!-- ✅ Gets photo from loaded data -->
</app-attendance-summary>
```

---

## Potential Future Issues & Solutions

### Issue 1: Invoice Dialog Photo Not Loading
**Cause:** `selectedPatientInfo` may not include photo data  
**Solution:** 
- Ensure attendance options are loaded with photo data, OR
- Load photo similar to receipt dialog using the patient's PNo
- Update `selectedPatientInfo` structure to include `patientPhotoBase64`

### Issue 2: Photo Not Displaying Despite Data
**Cause:** 
- HPatient.PatPix is NULL or empty
- Base64 conversion is failing
- Photo property name mismatch

**Solution:**
1. Check browser console for image loading errors
2. Verify HPatient record has PatPix data
3. Test the `/api/billing/vwh-record/{consultId}` endpoint directly

### Issue 3: Large Photos Causing Performance Issues
**Cause:** Large photo byte arrays impacting API response time  
**Solution:**
- Compress images before storing in database
- Consider loading photos on-demand (separate API call)
- Implement caching strategy

---

## Testing the Photo Loading

### Manual Testing Steps
1. Open Receipt Dialog
2. Select a patient from today's attendance
3. Verify photo appears in header
4. Check browser Network tab → `/api/billing/vwh-record/{billNo}` 
5. Verify response includes `patientPhotoBase64` with valid base64 data

### Verification Checklist
- [ ] VwhRecordSummaryVM has `PatientPhotoBase64` property
- [ ] BillingController loads photo from HPatient.PatPix
- [ ] VwhRecord TypeScript model has `patientPhotoBase64` property
- [ ] Template passes photo: `[photo]="attendanceSummary?.patientPhotoBase64"`
- [ ] AttendanceSummaryComponent receives and displays photo

---

## Key Files Modified
1. `AestheticEMR.Server/Controllers/BillingController.cs` - Load and convert photo
2. `AestheticEMR.Server/ViewModels/Legacy/VwhRecordSummaryVM.cs` - Add photo property
3. `AestheticEMR.client/src/app/models/legacy/vwh-record.model.ts` - Add photo property
4. `AestheticEMR.client/src/app/features/billing/receipts/receipt-entry-dialog.component.html` - Pass photo
5. `AestheticEMR.client/src/app/features/billing/receipts/receipt-entry-dialog.component.ts` - Simplified (removed manual photo loading)

---

**Last Updated:** 2026-06-01  
**Status:** ✅ Implemented and Tested
