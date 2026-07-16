# 📝 Edit Mode: Code Changes Summary

## File Modified
`AestheticEMR/AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts`

---

## Change 1: Add OnInit Import

**Line 2 - Before:**
```typescript
import { Component, computed, inject, signal } from '@angular/core';
```

**Line 2 - After:**
```typescript
import { Component, computed, inject, signal, OnInit } from '@angular/core';
```

---

## Change 2: Implement OnInit Interface

**Line 355 - Before:**
```typescript
export class CreateRosterDialogComponent {
```

**Line 355 - After:**
```typescript
export class CreateRosterDialogComponent implements OnInit {
```

---

## Change 3: Add ngOnInit Lifecycle Hook

**After line 381 (readonly groups computed), add:**

```typescript
  ngOnInit(): void {
    // In edit mode, prefill the dialog with existing data
    if (this.isEdit && this.data.existingRow) {
      this.initializeEditMode();
    }
  }
```

---

## Change 4: Add Four Private Helper Methods

**Before the `save()` method (around line 488), add:**

```typescript
  private initializeEditMode(): void {
    const row = this.data.existingRow;
    if (!row) return;

    // Extract date from the grid row and parse month/year
    if (row.date) {
      try {
        const dateObj = this.parseDate(row.date);
        this.selectedMonth = dateObj.getMonth() + 1;
        this.selectedYear = dateObj.getFullYear();
      } catch (_e) {
        console.warn('Could not parse date from existing row:', row.date);
      }
    }

    // Find and select the group
    const groupId = row.groupID ? parseInt(row.groupID.toString(), 10) : null;
    if (groupId) {
      const group = this.data.lookups.groups.find(g => g.groupId === groupId);
      if (group) {
        // Trigger group selection which will build list items
        this.onGroupChanged(groupId);

        // Load existing roster data for this group/month to pre-select checkboxes
        this.loadExistingRosterData(groupId);
      }
    }
  }

  private parseDate(dateStr: string): Date {
    // Try multiple date formats
    // Format 1: "2026-07-14" (yyyy-MM-dd)
    const isoMatch = dateStr.match(/(\d{4})-(\d{2})-(\d{2})/);
    if (isoMatch) {
      return new Date(parseInt(isoMatch[1], 10), parseInt(isoMatch[2], 10) - 1, parseInt(isoMatch[3], 10));
    }

    // Format 2: "14-Jul-2026" (dd-MMM-yyyy)
    const dmyMatch = dateStr.match(/(\d{2})-([A-Za-z]+)-(\d{4})/);
    if (dmyMatch) {
      const monthStr = dmyMatch[2];
      const month = new Date(`${monthStr} 1 2000`).getMonth();
      return new Date(parseInt(dmyMatch[3], 10), month, parseInt(dmyMatch[1], 10));
    }

    // Fallback: try native Date parsing
    return new Date(dateStr);
  }

  private loadExistingRosterData(groupId: number): void {
    this.loading.set(true);
    const empId = this.data.existingRow?.empID ?? '';
    const fromDate = `${this.selectedYear}-${String(this.selectedMonth).padStart(2, '0')}-01`;
    const toDate = `${this.selectedYear}-${String(this.selectedMonth).padStart(2, '0')}-${new Date(this.selectedYear, this.selectedMonth, 0).getDate()}`;

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

  private markExistingItemsAsSelected(existingRecords: RosterGridItem[]): void {
    // Build a map of existing shifts: date + shiftAbbrv → true
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

---

## Summary of Changes

| Item | Count |
|------|-------|
| Lines Added | ~95 |
| Methods Added | 4 private |
| Interfaces Implemented | 1 (OnInit) |
| Breaking Changes | 0 |
| Compilation Errors | 0 |

---

## Logic Flow

```
1. ngOnInit() → Called when component initializes
2. initializeEditMode() → Orchestrates prefill
   ├─ parseDate() → Extract month/year from existing row
   ├─ onGroupChanged() → Select group and build checkboxes
   └─ loadExistingRosterData() → Fetch and pre-select
3. loadExistingRosterData() → Makes HTTP request
   └─ markExistingItemsAsSelected() → Pre-selects matching checkboxes
```

---

## No Changes To

- ✅ Parent component (`create-roster.component.ts`)
- ✅ Router configuration
- ✅ Backend controllers/services
- ✅ Template HTML
- ✅ Styling (CSS)
- ✅ Existing methods (`save()`, `buildListItems()`, etc.)

---

## Backward Compatibility

**New Mode (Add Button)**: Unaffected ✅
- `isEdit = false` (no existingRow)
- `ngOnInit()` skips `initializeEditMode()`
- Dialog works exactly as before

**Edit Mode (Edit Icon)**: Enhanced ✅
- `isEdit = true` (has existingRow)
- `ngOnInit()` calls `initializeEditMode()`
- Dialog prefills with previous selections

---

## Testing the Changes

```typescript
// The dialog component will automatically:

// 1. Check if in edit mode
if (this.isEdit && this.data.existingRow) {
  this.initializeEditMode();  // ← This runs
}

// 2. Extract month/year from row.date
const dateObj = this.parseDate(row.date);  // ← Parses date
this.selectedMonth = dateObj.getMonth() + 1;
this.selectedYear = dateObj.getFullYear();

// 3. Select the group
this.onGroupChanged(groupId);  // ← Builds checkboxes

// 4. Load and pre-select
this.loadExistingRosterData(groupId);  // ← Fetches + marks
```

---

## All Changes in One File

Everything is in:
```
AestheticEMR/AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts
```

**No other files were modified.** ✅

---

## Status

```
✅ Implemented
✅ Compiled Successfully
✅ No Errors
✅ Ready to Test
```

