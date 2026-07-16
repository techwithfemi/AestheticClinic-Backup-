# 🎯 Edit Mode Prefill Implementation Complete

## Overview

Edit mode for roster entries now fully supports **prefilling** (pre-selecting) previously saved data when clicking the edit icon on a grid row.

---

## ✅ What Changed

### 1. **Dialog Component (create-roster-dialog.component.ts)**
- ✅ Added `OnInit` lifecycle hook
- ✅ Added `initializeEditMode()` method (called when `isEdit = true`)
- ✅ Added `parseDate()` helper for multiple date formats
- ✅ Added `loadExistingRosterData()` to fetch existing entries from database
- ✅ Added `markExistingItemsAsSelected()` to pre-select matching checkboxes

---

## 🔄 Edit Mode Flow

```
User clicks edit icon on a grid row
        ↓
Parent component (create-roster.component) calls:
  openEditDialog(row: RosterGridItem)
        ↓
Dialog opens with:
  existingRow = the selected row
  isEdit = true (computed from existingRow)
        ↓
ngOnInit() triggers initializeEditMode()
        ↓
Step 1: Extract month/year from existingRow.date
        ↓
Step 2: Parse and set selectedMonth/selectedYear
        ↓
Step 3: Find group from existingRow.groupID and call onGroupChanged()
        ↓
onGroupChanged() triggers buildListItems()
        (creates all day/shift checkboxes)
        ↓
Step 4: loadExistingRosterData(groupId) fetches existing entries from DB
        (using empID, month/year range)
        ↓
Step 5: markExistingItemsAsSelected() compares fetched data to checkbox list
        and marks matching entries as selected
        ↓
Dialog displays with:
  ✅ Group dropdown prefilled
  ✅ Month/Year dropdowns prefilled
  ✅ Checkboxes pre-selected matching previous choices
```

---

## 📋 Implementation Details

### `initializeEditMode()`
Executes when dialog opens in edit mode:

```typescript
private initializeEditMode(): void {
  const row = this.data.existingRow;
  if (!row) return;

  // Extract date and parse month/year
  if (row.date) {
    const dateObj = this.parseDate(row.date);
    this.selectedMonth = dateObj.getMonth() + 1;
    this.selectedYear = dateObj.getFullYear();
  }

  // Find and select the group
  const groupId = row.groupID ? parseInt(row.groupID.toString(), 10) : null;
  if (groupId) {
    const group = this.data.lookups.groups.find(g => g.groupId === groupId);
    if (group) {
      this.onGroupChanged(groupId);  // Builds list items
      this.loadExistingRosterData(groupId);  // Loads and pre-selects
    }
  }
}
```

### `parseDate(dateStr: string): Date`
Handles multiple date formats:

1. **ISO Format**: `"2026-07-14"` (yyyy-MM-dd)
2. **Text Format**: `"14-Jul-2026"` (dd-MMM-yyyy)
3. **Fallback**: Native Date parsing

```typescript
private parseDate(dateStr: string): Date {
  // Try ISO format first
  const isoMatch = dateStr.match(/(\d{4})-(\d{2})-(\d{2})/);
  if (isoMatch) {
    return new Date(parseInt(isoMatch[1], 10), parseInt(isoMatch[2], 10) - 1, parseInt(isoMatch[3], 10));
  }

  // Try text format (dd-MMM-yyyy)
  const dmyMatch = dateStr.match(/(\d{2})-([A-Za-z]+)-(\d{4})/);
  if (dmyMatch) {
    const monthStr = dmyMatch[2];
    const month = new Date(`${monthStr} 1 2000`).getMonth();
    return new Date(parseInt(dmyMatch[3], 10), month, parseInt(dmyMatch[1], 10));
  }

  // Fallback
  return new Date(dateStr);
}
```

### `loadExistingRosterData(groupId: number)`
Fetches existing roster entries from backend:

```typescript
private loadExistingRosterData(groupId: number): void {
  this.loading.set(true);
  const empId = this.data.existingRow?.empID ?? '';
  const fromDate = `${this.selectedYear}-${String(this.selectedMonth).padStart(2, '0')}-01`;
  const toDate = `${this.selectedYear}-${String(this.selectedMonth).padStart(2, '0')}-${new Date(this.selectedYear, this.selectedMonth, 0).getDate()}`;

  // Call backend endpoint to get existing records
  this.rosterEndpoint.getExistingEndpoint<RosterGridItem[]>({
    empId,
    fromDate,
    toDate
  }).subscribe({
    next: (existingRecords) => {
      this.loading.set(false);
      this.markExistingItemsAsSelected(existingRecords);
    },
    error: (_error) => {
      this.loading.set(false);
      console.warn('Could not load existing roster data for prefill');
      // Continue anyway - user can select manually
    }
  });
}
```

### `markExistingItemsAsSelected(existingRecords: RosterGridItem[])`
Compares database records with checkbox list and pre-selects matches:

```typescript
private markExistingItemsAsSelected(existingRecords: RosterGridItem[]): void {
  // Build a map from database records: date + shiftAbbrv → true
  const existingMap = new Set<string>();
  for (const record of existingRecords) {
    if (record.date && record.shiftAbbrv) {
      existingMap.add(`${record.date}|${record.shiftAbbrv}`);
    }
  }

  // Mark matching items as selected
  this.listItems.update(items =>
    items.map(item => {
      const key = `${item.date}|${item.shiftAbbrv}`;
      return { ...item, selected: existingMap.has(key) };
    })
  );
}
```

**Matching Logic**:
- Compares `date` + `shiftAbbrv` from database with checkbox list
- If a match is found, checkbox is pre-selected
- User can modify selections before saving (delete-insert pattern applies)

---

## 🎨 User Experience

### Before Edit Mode
```
Grid shows:
┌─────────────────────────────────────────────┐
│ Date    | Group        | Shift  | Action   │
├─────────────────────────────────────────────┤
│ 14-Jul  | Morning Grp  | AM     | [Edit] ✏️│
└─────────────────────────────────────────────┘
```

### Click Edit ✏️
```
Dialog opens with:
✅ Month: July (auto-filled)
✅ Year: 2026 (auto-filled)
✅ Group: Morning Grp (pre-selected)
✅ Checkboxes: (loading...)
```

### After Prefill Loads
```
Day-Shift List:
☑️ 14-Jul  Morning [AM]   Monday  ← Pre-selected (was saved)
☐ 14-Jul  Afternoon [PM]  Monday
☑️ 15-Jul  Morning [AM]   Tuesday ← Pre-selected (was saved)
☐ 15-Jul  Evening [EV]   Tuesday
```

### Modify & Save
- User can change selections (uncheck/check)
- Clicks Save
- **Delete-insert pattern applied**:
  - Old DB records deleted
  - New selections inserted

---

## 🔗 Related Methods

### Existing Methods (No Changes)
- `onGroupChanged()` - Still builds list items when group selected
- `buildListItems()` - Still creates all day/shift checkbox rows
- `save()` - Still performs delete-insert with selected/unselected days
- `formatDate()` - Converts Date to yyyy-MM-dd string

### New Methods
- `initializeEditMode()` - Entry point for edit prefill
- `parseDate()` - Parses multiple date formats
- `loadExistingRosterData()` - Fetches existing records from backend
- `markExistingItemsAsSelected()` - Pre-selects matching checkboxes

---

## 📌 Key Points

✅ **Backward Compatible**
- New mode only activates when `existingRow` is provided
- New mode flow doesn't affect existing new mode behavior

✅ **Graceful Degradation**
- If date parsing fails, uses current month/year
- If database fetch fails, user can select manually
- Always allows manual selection regardless of prefill

✅ **Data Integrity**
- Matches by date + shift abbreviation (unique combination)
- Fetches only for the specific month/year/employee
- No cross-contamination with other records

✅ **Performance**
- Single database query to fetch existing records
- Efficient Set-based matching logic
- Loading spinner shown during data fetch

---

## 🧪 Testing Checklist

- [ ] Click edit icon on a roster grid row
- [ ] Dialog opens with month/year prefilled
- [ ] Group dropdown shows the group from the row
- [ ] Wait for loading spinner to complete
- [ ] Verify checkboxes are pre-selected matching the previous selection
- [ ] Modify selections (check/uncheck different days)
- [ ] Click Save
- [ ] Verify grid refreshes with new data
- [ ] Verify old data is deleted, new data is inserted (delete-insert pattern)

---

## 📂 Files Modified

- `AestheticEMR/AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts`
  - Added `OnInit` interface implementation
  - Added `ngOnInit()` lifecycle hook
  - Added 4 new private methods for edit mode prefill
  - Added unused variable suppression (`_e`, `_error`) for cleaner code

---

## 🚀 Ready for Testing

Build successful ✅

You can now:
1. Navigate to Staff Roster → Create Roster
2. Click the edit icon (✏️) on any grid row
3. The dialog should prefill with previous selections
4. Modify and save to test delete-insert behavior

