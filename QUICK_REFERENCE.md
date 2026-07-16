# Quick Reference - Unselected Days Extraction

## Question Answered ✅
**"Why is shift col showing default value (PLS_ENTER_SHIFT) instead of explicit value during save?"**

**Answer:** The backend was inserting unselected days automatically with a placeholder. Now the **frontend extracts shift data from unselected checkboxes** and sends them to the backend.

---

## What Changed

### Before
```
User checks: "Morning [AM]" ✓
User leaves: "Afternoon [PM]" ☐

Frontend sends: Only "Morning"
Backend does: Calculates unselected day → Inserts "Afternoon" with "PLS_ENTER_SHIFT"
```

### After
```
User checks: "Morning [AM]" ✓
User leaves: "Afternoon [PM]" ☐

Frontend sends: "Morning" AND "Afternoon" (both with extracted shift data)
Backend does: Insert both exactly as provided
```

---

## Code Changes Summary

### 1️⃣ Frontend Extraction (Component)
```typescript
// Extract UNSELECTED items same way as selected
const unselectedItems = this.listItems().filter(i => !i.selected);
const unselectedDays = unselectedItems.map(i => ({
  date: i.date,
  shiftId: i.shiftId,
  shiftAbbrv: i.shiftAbbrv.trim(),
  shiftName: i.shiftName.trim()
}));
```

### 2️⃣ Payload (Endpoint)
```typescript
{
  selectedDays: [...],      // Checked checkboxes
  unselectedDays: [...]     // Unchecked checkboxes (NEW)
}
```

### 3️⃣ Backend Processing (Service)
```csharp
// OLD: Calculate unselected dates
// NEW: Use unselectedDays from frontend
var unselectedDays = request.UnselectedDays ?? [];
foreach (var day in unselectedDays)
{
    INSERT INTO Roster VALUES (
        ShiftName = day.ShiftName,      // From frontend
        ShiftAbbrv = day.ShiftAbbrv,    // From frontend
        isOffDuty = 1
    )
}
```

---

## Example Payload

```json
{
  "selectedDays": [
    {
      "date": "2026-07-14",
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
    }
  ]
}
```

---

## Result in Grid

| Date | Shift | Status | isOffDuty |
|------|-------|--------|-----------|
| 14-Jul-2026 | Morning | - | 0 |
| 14-Jul-2026 | Afternoon | - | 1 |

- **Morning (isOffDuty=0)**: Explicit assignment (user selected it)
- **Afternoon (isOffDuty=1)**: Needs review (user did NOT select it)

---

## Files Modified

| File | Change |
|------|--------|
| roster-endpoint.service.ts | Added `unselectedDays?` to RosterSaveRequest |
| create-roster-dialog.component.ts | Extract unselected items in save() method |
| RosterVMs.cs | Added `UnselectedDays` property |
| RosterModels.cs | Added `UnselectedDays` property |
| RosterService.cs | Process unselectedDays instead of calculating |

---

## Key Insight

**Extraction method is identical for both:**

```typescript
// Extraction logic (identical for selected and unselected)
const days = items.map(i => ({
  date: i.date,
  shiftId: i.shiftId,
  shiftAbbrv: i.shiftAbbrv.trim(),
  shiftName: i.shiftName.trim()
}));
```

**Only difference:** Which `items` array we filter from
- Selected: `i.selected === true`
- Unselected: `i.selected === false`

---

## Build Status
✅ .NET: No compilation errors  
✅ Ready to test

