# SPA Dialog Save Error Fix

## Problem
The spa consultation save was returning HTTP 400 with error: **"Provider: The Provider field is required."**

This occurred because:
1. The ViewModel had `[Required]` attribute on the `Provider` field
2. The client was not sending the Provider (it should be set server-side)
3. The validation failed before the controller code could set the Provider

## Solution

### Backend Changes
**File**: `AestheticEMR\AestheticEMR.Server\ViewModels\Aesthetic\AestheticConsultationVM.cs`

**Changed:**
```csharp
// Before - was marked as Required
[Required]
[StringLength(150)]
public string? Provider { get; set; }

// After - now optional (will be set by controller)
[StringLength(150)]
public string? Provider { get; set; }
```

**Updated Validator:**
```csharp
public class AestheticConsultationViewModelValidator : AbstractValidator<AestheticConsultationVM>
{
    public AestheticConsultationViewModelValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0)
            .WithMessage("Consultation must be linked to a patient.");
        RuleFor(x => x.ProcedureType).NotEmpty()
            .WithMessage("Procedure type is required.");
        RuleFor(x => x.ConsultationDate).LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Consultation date cannot be in the future.");
        // Provider is no longer validated here - it's set by the controller
    }
}
```

### How It Works Now

1. **Client sends**: Patient ID, Procedure Type, Consultation Date, and optional fields
2. **Controller receives** the request and validates with FluentValidator
3. **Controller sets** `consultation.Provider = GetCurrentUserId();` before saving
4. **Save completes** successfully

### Flow in AestheticController.CreateSpaConsultation()
```csharp
[HttpPost("consultations/spa")]
public IActionResult CreateSpaConsultation([FromBody] AestheticConsultationVM consultationVM)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);  // Only PatientId, ProcedureType, ConsultationDate checked

    consultationVM.ProcedureType = "Spa";
    try
    {
        var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
        consultation.Provider = GetCurrentUserId();  // ← Provider is set here!
        var created = _aestheticService.AddConsultation(consultation, ...);
        return CreatedAtAction(...);
    }
    catch (Exception ex) { ... }
}
```

## Required Fields Summary

| Field | Required | Set By | Notes |
|-------|----------|--------|-------|
| PatientId | ✅ Yes | Client | Must be > 0 |
| ProcedureType | ✅ Yes | Client (or Controller) | Automatically set to "Spa" |
| ConsultationDate | ✅ Yes | Client | Must not be in future |
| Provider | ❌ No | Controller | Set to GetCurrentUserId() |
| All other fields | ❌ No | Client | Optional |

## Build Status
✅ **Build succeeded** - No compilation errors

## Testing
The save should now work when you:
1. Select a patient (required)
2. Select a service type (required) 
3. Check both consent checkboxes (required to be true)
4. Fill optional fields as desired
5. Click Save

The error "Provider: The Provider field is required." should no longer appear.

