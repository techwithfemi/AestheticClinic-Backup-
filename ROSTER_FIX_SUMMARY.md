# Roster Dialog - Unselected Days Extraction Fix

## Overview
Updated the create-roster-dialog component and backend services to extract shift data from **unselected checkbox items** and send them to the backend, following the VB6 `InsertBlankShifts` pattern strictly.

---

## Problem Statement

Previously, the backend was automatically generating placeholder rows (`PLS_ENTER_SHIFT`) for all unselected days in the month. This meant:
- Unselected checkbox data was never sent from the frontend
- The backend calculated which days were unselected and inserted them with hardcoded values
- No ability to control unselected day behavior from the UI

## Solution

Now the frontend **extracts shift data from ALL checkboxes** (both selected and unselected), following the VB6 approach exactly:

```
VB6 Code Flow:
1. lstDays.ListCount — All items in the list (selected or not)
2. For each item:
   - If selected: Extract shift data → insert explicit value
   - If NOT selected: Extract shift data → send to backend as unselected
3. Backend processes BOTH arrays independently
```

---

## Changes Made

### 1. Frontend - TypeScript Component

#### File: `AestheticEMR.client/src/app/services/roster-endpoint.service.ts`

**Added to RosterSaveRequest:**
```typescript
export interface RosterSaveRequest {
  deptId?: string;
  deptName?: string;
  groupId?: number | null;
  sourceEmpId?: string | null;
  targetEmpId?: string | null;
  groupName: string;
  selectedDays: RosterDaySelection[];
  unselectedDays?: RosterDaySelection[];  // ← NEW
}
```

#### File: `AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts`

**Updated save() method:**
```typescript
save(): void {
  // ... validation code ...

  const selectedItems = this.listItems().filter(i => i.selected);
  const unselectedItems = this.listItems().filter(i => !i.selected);  // ← NEW

  // Extract selected days (unchanged)
  const selectedDays = selectedItems
    .map(i => ({
      date: i.date,
      shiftId: i.shiftId,
      shiftAbbrv: i.shiftAbbrv.trim(),
      shiftName: i.shiftName.trim()
    }));

  // Extract unselected days (NEW - same extraction as selected)
  const unselectedDays = unselectedItems
    .map(i => ({
      date: i.date,
      shiftId: i.shiftId,
      shiftAbbrv: i.shiftAbbrv.trim(),
      shiftName: i.shiftName.trim()
    }));

  // Send both arrays to backend
  this.alertService.showDialog(
    `Are you sure to save Record for ${groupName}?`,
    DialogType.confirm,
    () => this.commitSave({
      deptId,
      deptName,
      groupId: group.groupId,
      groupName,
      selectedDays,
      unselectedDays  // ← NEW
    })
  );
}
```

### 2. Backend - ViewModels

#### File: `AestheticEMR.Server/ViewModels/Legacy/RosterVMs.cs`

**Added to RosterSaveVM:**
```csharp
public sealed class RosterSaveVM
{
    public string? DeptId { get; set; }
    [StringLength(200)]
    public string? DeptName { get; set; }
    public long? GroupId { get; set; }
    [StringLength(50)]
    public string? SourceEmpId { get; set; }
    [StringLength(50)]
    public string? TargetEmpId { get; set; }
    [Required, StringLength(200)]
    public string GroupName { get; set; } = string.Empty;
    [MinLength(1)]
    public List<RosterDaySelectionVM> SelectedDays { get; set; } = [];
    public List<RosterDaySelectionVM> UnselectedDays { get; set; } = [];  // ← NEW
}
```

### 3. Backend - Core Models

#### File: `AestheticEMR.Core/Services/Legacy/Models/RosterModels.cs`

**Added to RosterSaveRequest:**
```csharp
public sealed class RosterSaveRequest
{
    public string? DeptId { get; set; } = string.Empty;
    public string? DeptName { get; set; }
    public long? GroupId { get; set; }
    public string? SourceEmpId { get; set; }
    public string? TargetEmpId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public List<RosterDaySelection> SelectedDays { get; set; } = [];
    public List<RosterDaySelection> UnselectedDays { get; set; } = [];  // ← NEW
}
```

### 4. Backend - Service Logic

#### File: `AestheticEMR.Core/Services/Legacy/RosterService.cs`

**SaveAsync() method - Replaced automatic blank insertion with frontend data:**

**Before:**
```csharp
// Old code: Backend calculated unselected dates
var selectedDateSet = request.SelectedDays.Select(x => x.Date).ToHashSet();
for (var date = monthStart; date <= monthEnd; date = date.AddDays(1))
{
    if (selectedDateSet.Contains(date))
        continue;

    // Hardcoded PLS_ENTER_SHIFT for all unselected days
    ShiftName = "PLS_ENTER_SHIFT";
    // ... insert
}
```

**After:**
```csharp
// New code: Use unselected data from frontend
var unselectedDays = request.UnselectedDays ?? [];
foreach (var day in unselectedDays.OrderBy(x => x.Date))
{
    var isOffDuty = day.ShiftId.ToString().Equals(offDutyShiftId, StringComparison.OrdinalIgnoreCase)
        || day.ShiftId.ToString().Equals(leaveShiftId, StringComparison.OrdinalIgnoreCase)
        || day.ShiftId == 0;  // ShiftId 0 = blank/unselected day

    // Delete then insert with frontend-provided shift data
    // (Same logic as selected days, but using unselected data)
    await connection.ExecuteAsync(@"
        INSERT INTO Roster
        (RosterGrpShiftID, EmpID, ShiftID, GroupID, isOffDuty, ShiftAbbrv, ShiftName, GroupName, DeptID, RosterDate)
        VALUES
        (@RosterGrpShiftID, @EmpID, @ShiftID, @GroupID, @IsOffDuty, @ShiftAbbrv, @ShiftName, @GroupName, @DeptID, @RosterDate);",
        new
        {
            RosterGrpShiftID = 0,
            EmpID = targetEmpId,
            ShiftID = day.ShiftId,           // ← Now from frontend
            GroupID = rosterGroupId,
            IsOffDuty = isOffDuty ? 1 : 0,
            ShiftAbbrv = day.ShiftAbbrv,     // ← Now from frontend
            ShiftName = day.ShiftName,       // ← Now from frontend
            GroupName = groupName,
            DeptID = deptId,
            RosterDate = day.Date.ToDateTime(TimeOnly.MinValue)
        }, transaction);
}
```

---

## Data Flow Comparison

### VB6 Behavior (Original)
```
lstDays.List:
  [0] "14 Jul 2026  Morning [AM]  Monday" → Selected=TRUE  → Extract & Insert
  [1] "14 Jul 2026  Afternoon [PM]  Monday" → Selected=FALSE → Extract & Send to Backend
  [2] "15 Jul 2026  Morning [AM]  Tuesday" → Selected=TRUE  → Extract & Insert
  ...
```

### Angular Implementation (New)
```
listItems Signal:
  [0] { date: "2026-07-14", shiftId: 123, shiftName: "Morning", shiftAbbrv: "AM", selected: true }
       → Add to selectedDays array

  [1] { date: "2026-07-14", shiftId: 124, shiftName: "Afternoon", shiftAbbrv: "PM", selected: false }
       → Add to unselectedDays array

  [2] { date: "2026-07-15", shiftId: 123, shiftName: "Morning", shiftAbbrv: "AM", selected: true }
       → Add to selectedDays array
```

**Payload sent to backend:**
```json
{
  "deptId": "DEPT001",
  "deptName": "Dental Clinic",
  "groupId": 5,
  "groupName": "Morning Shift Group",
  "selectedDays": [
    {
      "date": "2026-07-14",
      "shiftId": 123,
      "shiftAbbrv": "AM",
      "shiftName": "Morning"
    },
    {
      "date": "2026-07-15",
      "shiftId": 123,
      "shiftAbbrv": "AM",
      "shiftName": "Morning"
    }
  ],
  "unselectedDays": [
    {
      "date": "2026-07-14",
      "shiftId": 124,
      "shiftAbbrv": "PM",
      "shiftName": "Afternoon"
    },
    {
      "date": "2026-07-15",
      "shiftId": 124,
      "shiftAbbrv": "PM",
      "shiftName": "Afternoon"
    }
  ]
}
```

---

## Backend Processing

### Selected Days Processing
```csharp
foreach (var day in request.SelectedDays.OrderBy(x => x.Date))
{
    // Insert with explicit values from frontend
    // isOffDuty = 0 for explicit shifts
    // ShiftName = actual shift name
    INSERT INTO Roster VALUES (..., ShiftName="Morning", isOffDuty=0, ...)
}
```

### Unselected Days Processing
```csharp
foreach (var day in request.UnselectedDays.OrderBy(x => x.Date))
{
    // Insert with frontend data BUT mark as OFF_DUTY if needed
    // isOffDuty = 1 when ShiftId == 0 (placeholder days)
    INSERT INTO Roster VALUES (..., ShiftName="[placeholder name]", isOffDuty=1, ...)
}
```

---

## Key Differences from Old Implementation

| Aspect | Old | New |
|--------|-----|-----|
| **Unselected Days** | Auto-calculated in backend | Extracted by frontend |
| **Placeholder Logic** | Hardcoded `PLS_ENTER_SHIFT` | Frontend decides what to send |
| **Data Source** | Backend loop through calendar | Frontend checkbox state |
| **Flexibility** | Fixed behavior | Can customize unselected handling |
| **VB6 Alignment** | Partial (only selected days) | Complete (both selected & unselected) |

---

## Testing Checklist

- [ ] Select some checkboxes and save → Verify selected days inserted with explicit shift values
- [ ] Leave some checkboxes unselected → Verify unselected days sent to backend
- [ ] Check roster grid → All days for the month should appear
- [ ] Verify `isOffDuty` flag set correctly for unselected days
- [ ] Test with OFF_DUTY and LEAVE shifts in unselected days
- [ ] Confirm no automatic blank-day insertion happens without frontend data

---

## Build Status

✅ **C# Projects**: No errors  
⚠️ **TypeScript**: Pre-existing module resolution issues (not related to this change)

