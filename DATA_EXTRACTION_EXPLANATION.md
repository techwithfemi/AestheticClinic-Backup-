# Data Extraction Flow - Detailed Explanation

## Question: "Days not selected have display text - extract it as done for selected days"

### Answer: YES, now implemented!

---

## How Shift Data Extraction Works

### Display Text Format (Checkbox Label)
```
"14 Jul 2026  Morning [AM]  Monday"
 └─ Date    └─ ShiftName  └─ Abbr  └─ Day
```

### Frontend Storage (Already Typed Objects)

Instead of parsing this text at save time, the component stores the **extracted values** in the `DayShiftItem` object when the list is built:

```typescript
// buildListItems() - Line 404-429
for (const shift of shifts) {
  items.push({
    // Pre-extracted values stored in object
    key: `${dateStr}|${shift.sno}`,
    date: dateStr,                    // "2026-07-14"
    label: `${dateLabel}  ${shift.shiftName} [${shift.evalTo}]  ${dayName}`,
    shiftId: shift.sno,              // 123 (numeric)
    shiftName: shift.shiftName,      // "Morning" (string)
    shiftAbbrv: shift.evalTo,        // "AM" (string)
    dayName,                          // "Monday"
    selected: false                   // Flag: checked or not
  });
}
```

### Data Collection During Save

#### **SELECTED Items:**
```typescript
const selectedItems = this.listItems().filter(i => i.selected);

const selectedDays = selectedItems.map(i => ({
  date: i.date,                    // Already extracted ✓
  shiftId: i.shiftId,              // Already extracted ✓
  shiftAbbrv: i.shiftAbbrv.trim(), // Already extracted ✓
  shiftName: i.shiftName.trim()    // Already extracted ✓
}));
```

#### **UNSELECTED Items (NEW):**
```typescript
const unselectedItems = this.listItems().filter(i => !i.selected);

const unselectedDays = unselectedItems.map(i => ({
  date: i.date,                    // Already extracted ✓
  shiftId: i.shiftId,              // Already extracted ✓
  shiftAbbrv: i.shiftAbbrv.trim(), // Already extracted ✓
  shiftName: i.shiftName.trim()    // Already extracted ✓
}));
```

**Both use the same extraction method!** No difference between selected and unselected.

---

## VB6 Equivalent Comparison

### VB6 Code (Original)
```vb6
' Line 153: Extract for EACH selected item
For X = 0 To lstDays.ListCount - 1
    If lstDays.Selected(X) = True Then
        ' Parse display text for each selected item
        strX = Mid(lstDays.List(X), 1, InStr(lstDays.List(X), " ") - 1)
        DutyDate = Format(CDate(Trim(strX)), "Short Date")

        strShiftX = Mid(lstDays.List(X), InStr(lstDays.List(X), " ") + 1)
        ShiftName = Trim(Mid(strShiftX, 1, InStr(strShiftX, "[") - 1))
        EvalTo = Mid(Mid(lstDays.List(X), InStr(lstDays.List(X), "[") + 1, ...), 1, 1)
        lngSNo = lstDays.ItemData(X)

        ' Insert selected day
        cmd.CommandText = "Insert into Roster ... Values (..., ShiftName, ...)"
        cmd.Execute
    End If
Next X
```

**VB6 InsertBlankShifts (for unselected days):**
```vb6
' Line 211: Also extracts from each item but uses placeholder
For X = 0 To UBound(ArrB) - 1
    ShiftName = "PLS_ENTER_SHIFT"  ' ← PLACEHOLDER
    EvalTo = ""
    GroupName = cboGroup.Text
    lngSNo = 0
    isOffDuty = 1

    DutyDate = Format(CDate(ArrB(X)), "Short Date")

    ' Insert unselected day with placeholder
    cmd.CommandText = "Insert into Roster ... Values (..., ShiftName, ...)"
    cmd.Execute
Next X
```

### Key Insight
In VB6, **BOTH paths** (selected and unselected) extract shift data and insert. The difference is:
- **Selected**: Use actual shift values
- **Unselected**: Use placeholder values

---

## Angular Implementation (Matches VB6)

### Before (Old Way)
```
Frontend: Send only selectedDays
Backend: Calculate which dates are unselected → insert with PLS_ENTER_SHIFT
```

### After (New Way - Matches VB6)
```
Frontend: Send selectedDays AND unselectedDays (with extracted shift data)
Backend: Insert both arrays independently
```

**The extraction is identical for both!**

```typescript
// Same extraction logic for selected
const selectedDays = selectedItems.map(i => ({
  date: i.date, shiftId: i.shiftId, shiftAbbrv: i.shiftAbbrv, shiftName: i.shiftName
}));

// Same extraction logic for unselected
const unselectedDays = unselectedItems.map(i => ({
  date: i.date, shiftId: i.shiftId, shiftAbbrv: i.shiftAbbrv, shiftName: i.shiftName
}));
```

---

## What Gets Stored in Each Row

### Selected Checkbox Example
```
Checkbox: "14 Jul 2026  Morning [AM]  Monday" ✓ (checked)

Extracted:
  date: "2026-07-14"
  shiftId: 123
  shiftName: "Morning"
  shiftAbbrv: "AM"

Database Insert:
  ShiftID: 123
  ShiftName: "Morning"
  ShiftAbbrv: "AM"
  isOffDuty: 0 (explicit shift)
```

### Unselected Checkbox Example
```
Checkbox: "14 Jul 2026  Afternoon [PM]  Monday"  ☐ (unchecked)

Extracted:
  date: "2026-07-14"
  shiftId: 124
  shiftName: "Afternoon"
  shiftAbbrv: "PM"

Database Insert:
  ShiftID: 124
  ShiftName: "Afternoon"
  ShiftAbbrv: "PM"
  isOffDuty: 1 (placeholder, will be filled by user)
```

---

## Backend Decision Logic

In the backend `RosterService.SaveAsync()`:

```csharp
// For SELECTED days (line 216-249)
foreach (var day in request.SelectedDays.OrderBy(x => x.Date))
{
    var isOffDuty = day.ShiftId.ToString().Equals(offDutyShiftId, ...)
                 || day.ShiftId.ToString().Equals(leaveShiftId, ...);

    // Insert with frontend values
    INSERT INTO Roster VALUES (
        ShiftID = day.ShiftId,           // 123
        ShiftName = day.ShiftName,       // "Morning"
        ShiftAbbrv = day.ShiftAbbrv,     // "AM"
        isOffDuty = isOffDuty ? 1 : 0    // 0 (explicit)
    );
}

// For UNSELECTED days (line 251-287)
var unselectedDays = request.UnselectedDays ?? [];
foreach (var day in unselectedDays.OrderBy(x => x.Date))
{
    var isOffDuty = day.ShiftId == 0 || day.ShiftId == offDutyShiftId;

    // Insert with frontend values
    INSERT INTO Roster VALUES (
        ShiftID = day.ShiftId,           // 124
        ShiftName = day.ShiftName,       // "Afternoon"
        ShiftAbbrv = day.ShiftAbbrv,     // "PM"
        isOffDuty = 1                    // 1 (needs review)
    );
}
```

---

## Summary: Extraction Methods

### Selected Items
1. ✓ Checkbox is checked
2. ✓ `selected: true` in object
3. ✓ Extract: `date`, `shiftId`, `shiftName`, `shiftAbbrv`
4. ✓ Send to backend in `selectedDays` array
5. ✓ Backend inserts with `isOffDuty = 0` (confirmed shift)

### Unselected Items (NEW)
1. ☐ Checkbox is unchecked
2. ☐ `selected: false` in object
3. ☐ Extract: `date`, `shiftId`, `shiftName`, `shiftAbbrv` (same way!)
4. ☐ Send to backend in `unselectedDays` array
5. ☐ Backend inserts with `isOffDuty = 1` (pending review)

**Same extraction, different usage!**

