# SPA Dialog - Final Required Fields Configuration

## Summary of All Changes

### ✅ What's Required Now

**Frontend Form Validators:**
```typescript
patientKey: ['', Validators.required]              // Select Patient dropdown
indication: ['', Validators.required]               // Service Type dropdown  
consentGiven: [true, Validators.requiredTrue]      // Must be CHECKED/TRUE
informationAccepted: [true, Validators.requiredTrue] // Must be CHECKED/TRUE
services: ['', Validators.required]                 // Services textarea ← NEW
```

**Backend Validators:**
```csharp
RuleFor(x => x.PatientId).GreaterThan(0)           // From dropdown selection
RuleFor(x => x.ProcedureType).NotEmpty()           // Hardcoded "Spa"
RuleFor(x => x.Services).NotEmpty()                // From textarea ← NEW
RuleFor(x => x.ConsultationDate).LessThanOrEqualTo(DateTime.UtcNow) // If provided
```

---

## Field-by-Field Breakdown

| # | Field Name | UI Component | Frontend Required | Backend Required | Default | Notes |
|---|---|---|---|---|---|---|
| 1 | patientKey | Dropdown | ✅ Yes | ✅ Yes (PatientId > 0) | Empty | "Select Patient" placeholder |
| 2 | indication | Dropdown | ✅ Yes | ✅ Yes (ProcedureType) | Empty | Service Type list from spa.json |
| 3 | services | Textarea | ✅ Yes | ✅ Yes | Empty | List of services rendered |
| 4 | consentGiven | Toggle | ✅ Yes (must be true) | — | Checked | "Consent Obtained" |
| 5 | informationAccepted | Toggle | ✅ Yes (must be true) | — | Checked | "Information Accepted" |
| 6 | consultationDate | Datepicker | ❌ No | Validated if provided | Today | Must not be in future if set |
| 7 | brandUsed | Text | ❌ No | ❌ No | Empty | Optional |
| 8 | areaTreated | Textarea | ❌ No | ❌ No | Empty | Optional |
| 9 | skinAssessment | Text | ❌ No | ❌ No | Empty | Optional |
| 10 | allergies | Textarea | ❌ No | ❌ No | Empty | Optional |
| 11 | risksAndComplications | Textarea | ❌ No | ❌ No | Empty | Optional |
| 12 | treatmentPlan | Textarea | ❌ No | ❌ No | Empty | Optional |
| 13 | deviceSettings | Textarea | ❌ No | ❌ No | Empty | Optional |
| 14 | procedureDescription | Textarea | ❌ No | ❌ No | Empty | Optional |
| 15 | consentNotes | Textarea | ❌ No | ❌ No | Empty | Optional |
| 16 | postTreatmentInstructions | Textarea | ❌ No | ❌ No | Empty | Optional |
| 17 | currentMedications | Textarea | ❌ No | ❌ No | Empty | Optional |
| N/A | provider | (Server-set) | — | ❌ No (auto-set) | — | Set by controller from GetCurrentUserId() |

---

## Save Button State

**Save Button is ENABLED when:**
- ✅ Patient is selected (not empty string)
- ✅ Service Type is selected (not empty string)
- ✅ Services textarea has text (not empty)
- ✅ Consent Obtained toggle is checked/true
- ✅ Information Accepted toggle is checked/true

**Save Button is DISABLED when:**
- ❌ Any required field is empty
- ❌ Consent checkboxes are unchecked

**Button HTML:**
```html
<button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">
  Save
</button>
```

---

## Validation Flow

### Frontend (Angular)
```
User fills form
    ↓
Angular Form Validation (FormGroup)
    ├─ Check patientKey: not empty? 
    ├─ Check indication: not empty?
    ├─ Check services: not empty?
    ├─ Check consentGiven: is true?
    ├─ Check informationAccepted: is true?
    │
    └─ Form Valid? → Enable Save button
      └─ Form Invalid? → Disable Save button
```

### Backend (Asp.NET)
```
Client sends JSON
    ↓
ModelState validation (DataAnnotations)
    ├─ PatientId [Range(1, max)]? ✓
    ├─ ConsultationDate [Required]? ✓
    ├─ ProcedureType [Required]? ✓
    ├─ Services [Required]? ✓
    │
    ├─ Pass? → Continue to next step ✓
    └─ Fail? → Return 400 BadRequest ❌

    ↓
FluentValidator
    ├─ PatientId > 0?
    ├─ ProcedureType not empty?
    ├─ Services not empty?
    ├─ ConsultationDate <= UtcNow?
    │
    ├─ Pass? → Continue to service layer ✓
    └─ Fail? → Return 400 BadRequest with errors ❌

    ↓
Controller sets Provider
    └─ consultation.Provider = GetCurrentUserId();

    ↓
Service saves
    └─ return 201 Created ✓
```

---

## Error Messages (if validation fails)

**Backend will return 400 with ModelState:**
```json
{
  "errors": {
    "PatientId": ["Consultation must be linked to a patient."],
    "ProcedureType": ["Procedure type is required."],
    "Services": ["Services are required."],
    "ConsultationDate": ["Consultation date cannot be in the future."]
  }
}
```

---

## Build Status
✅ **Successful** - Ready to test

## Files Changed
1. `AestheticEMR\AestheticEMR.client\src\app\features\spa\services\spa-dialog.component.ts`
   - Made `services` field required with validator
   - Added asterisk (*) to Services label

2. `AestheticEMR\AestheticEMR.Server\ViewModels\Aesthetic\AestheticConsultationVM.cs`
   - Removed `[Required]` from Provider (set by controller)
   - Added `[Required]` to Services field
   - Updated validator to check Services
   - Removed Provider from validator

---

## Quick Reference

### To Make a Field Required (Frontend)
```typescript
fieldName: ['', Validators.required]  // Add Validators.required
```

### To Make a Field Required (Backend)
```csharp
[Required]
[StringLength(maxLength)]
public string? FieldName { get; set; }

// AND in validator:
RuleFor(x => x.FieldName).NotEmpty().WithMessage("Field is required.");
```

### To Make a Field Optional
```typescript
// Frontend
fieldName: ['']  // No validators

// Backend
public string? FieldName { get; set; }  // No [Required] attribute
// And don't add it to the validator
```

