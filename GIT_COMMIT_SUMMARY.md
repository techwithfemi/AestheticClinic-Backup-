# Git Commit Summary

## Subject
feat: Extract unselected roster days and send to backend following VB6 logic

## Description

### Problem
Previously, unselected roster days were calculated by the backend and inserted with a hardcoded "PLS_ENTER_SHIFT" placeholder. This meant:
- Frontend had no control over unselected day data
- Unselected shift information was lost
- Backend logic was coupled to the roster generation algorithm

### Solution
Frontend now extracts shift data from **all checkboxes** (selected and unselected) using identical logic and sends both arrays to the backend. The backend processes both independently.

This matches the VB6 `InsertBlankShifts` approach exactly:
- Extract shift data for each list item
- Separate into selected and unselected arrays  
- Send to backend for processing

### Changes Made

#### Frontend (Angular)
- **`roster-endpoint.service.ts`**
  - Added `unselectedDays?: RosterDaySelection[]` to `RosterSaveRequest` interface

- **`create-roster-dialog.component.ts`**
  - Extract unselected items: `const unselectedItems = this.listItems().filter(i => !i.selected)`
  - Map to day selections: Same extraction as selected items
  - Include in payload: Pass `unselectedDays` to `commitSave()`

#### Backend (C#)
- **`RosterVMs.cs`**
  - Added `UnselectedDays` property to `RosterSaveVM`

- **`RosterModels.cs`**
  - Added `UnselectedDays` property to `RosterSaveRequest`

- **`RosterService.cs`**
  - Replaced automatic blank-day calculation with frontend-provided `UnselectedDays` array
  - Process each unselected day with same insert logic as selected (but `isOffDuty = 1`)
  - Removed month-wide date loop that generated placeholders

### Data Flow

```
Frontend:
  - Checkbox checked → selectedDays[]
  - Checkbox unchecked → unselectedDays[]
  - Both extracted identically

Backend:
  - Insert selectedDays with isOffDuty=0
  - Insert unselectedDays with isOffDuty=1
```

### Database Result

| Date | Shift | isOffDuty | Source |
|------|-------|-----------|--------|
| 14-Jul | Morning | 0 | Selected |
| 14-Jul | Afternoon | 1 | Unselected |
| 15-Jul | Morning | 0 | Selected |
| 16-Jul | Morning | 1 | Unselected |

### VB6 Alignment

The implementation now matches VB6 pattern:
1. Iterate all list items
2. Extract shift data for each
3. Separate by selection state
4. Send both to backend
5. Backend inserts independently

Previous version only completed steps 1-3 for selected items.

### Testing

✅ Build successful (C# projects)
✅ No compilation errors
✅ Ready for functional testing

Test scenarios:
- Select some days, leave others blank → Verify both sent
- Save with OFF_DUTY shifts → Verify handled correctly
- Modify selections and save again → Verify data replaced
- Check roster grid → Verify all days populated

### Breaking Changes
None - Addition of optional `unselectedDays` parameter in payload.

---

## Files Changed
- `AestheticEMR/AestheticEMR.client/src/app/services/roster-endpoint.service.ts`
- `AestheticEMR/AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts`
- `AestheticEMR/AestheticEMR.Server/ViewModels/Legacy/RosterVMs.cs`
- `AestheticEMR/AestheticEMR.Core/Services/Legacy/Models/RosterModels.cs`
- `AestheticEMR/AestheticEMR.Core/Services/Legacy/RosterService.cs`

## Commit Type
- ✨ Feature (new functionality)
- 🐛 Bug fix (fixing the extraction issue)
- ♻️ Refactor (moving logic from backend to frontend)

---

## Related Issues
- Issue: Days not selected were showing "PLS_ENTER_SHIFT" instead of extracted shift values
- Root cause: Backend was calculating unselected days instead of receiving them from frontend
- Solution: Frontend now extracts and sends all data following VB6 pattern

---

## Notes for Reviewers

1. **Data Extraction**: Notice that selected and unselected items use identical extraction logic
   - Only difference is the filter condition (`i.selected` vs `!i.selected`)
   - This ensures consistency and reduces bugs

2. **Backend Logic**: The `InsertBlankShifts` equivalent is now data-driven instead of calculated
   - Lines 251-287 in RosterService.cs show the new approach
   - Much simpler and more maintainable

3. **VB6 Compliance**: The new approach matches VB6 code exactly
   - Reference: `SaveButton_Click()` and `InsertBlankShifts()` procedures

4. **Type Safety**: All data is extracted to typed objects early
   - No string parsing at save time
   - Reduces errors and improves maintainability

