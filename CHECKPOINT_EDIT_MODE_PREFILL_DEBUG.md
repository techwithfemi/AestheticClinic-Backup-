# Edit-Mode Prefill - Debug Checkpoint

## Summary
After the user's critical correction about VB6 checkbox behavior, the edit-mode prefill logic has been updated with comprehensive debug logging to diagnose why checkboxes may not be preselecting as expected.

## Current Status: ✅ Build Successful (with pre-existing module resolution warnings)

The dialog component now includes debug logging that will help us understand:
1. Which list items are being built and their date/shiftAbbrv values
2. Which records are returned from the backend
3. Whether the matching logic correctly identifies existing records
4. How many items are being marked as selected

## Key Changes Made

### 1. Enhanced Debug Logging in `loadExistingRosterData()`
```typescript
console.debug(`[prefill] Loading existing roster: empId=${empId}, fromDate=${fromDate}, toDate=${toDate}, groupId=${groupId}`);
console.debug(`[prefill] Loaded ${existingRecords.length} existing records from backend:`, existingRecords);
```

This will show:
- The exact parameters being sent to the backend
- The full data structure returned for each record

### 2. Enhanced Debug Logging in `buildListItems()`
```typescript
console.debug(`[prefill] Built list item: date=${dateStr}, shiftAbbrv=${shift.evalTo}, key=${item.key}`);
```

This will show:
- Each list item being created
- The exact date format (should be `yyyy-MM-dd`)
- The shift abbreviation value

### 3. Enhanced Debug Logging in `markExistingItemsAsSelected()`
```typescript
console.debug(`[prefill] DB record: date=${record.date}, shiftAbbrv=${record.shiftAbbrv}, key=${key}`);
console.debug(`[prefill] MATCH: item date=${item.date}, shiftAbbrv=${item.shiftAbbrv}`);
console.debug(`[prefill] Matched ${matchCount} list items out of ${this.listItems().length}`);
```

This will show:
- The keys being created from DB records
- Which list items successfully match DB records
- Total match count

## Next Steps for Diagnosis

When you test the edit-mode flow:

1. **Click edit icon** on a roster grid row
2. **Open browser DevTools** (F12)
3. **Go to Console tab**
4. **Observe the `[prefill]` prefixed debug messages**

### Expected Debug Output Example
```
[prefill] Loading existing roster: empId=EMP001, fromDate=2026-07-01, toDate=2026-07-31, groupId=5
[prefill] Built list item: date=2026-07-01, shiftAbbrv=DAY, key=2026-07-01|4
[prefill] Built list item: date=2026-07-01, shiftAbbrv=EVE, key=2026-07-01|5
...
[prefill] Loaded 3 existing records from backend: [
  { date: "2026-07-01", shiftAbbrv: "DAY", ... },
  { date: "2026-07-05", shiftAbbrv: "DAY", ... },
  { date: "2026-10-15", shiftAbbrv: "EVE", ... }
]
[prefill] DB record: date=2026-07-01, shiftAbbrv=DAY, key=2026-07-01|DAY
[prefill] MATCH: item date=2026-07-01, shiftAbbrv=DAY
[prefill] Matched 3 list items out of 62
```

## Potential Issues to Look For

### Issue 1: Date Format Mismatch
- List items use: `yyyy-MM-dd` (e.g., `2026-07-01`)
- Backend might return: different format
- **Look for**: Date strings that don't match exactly in the console output

### Issue 2: ShiftAbbrv vs ShiftId
- Currently matching on: `date|shiftAbbrv` (e.g., `2026-07-01|DAY`)
- VB6 original logic uses: `date|shiftId` (numeric shift ID)
- **Look for**: Console showing DB records have `shiftAbbrv` populated

### Issue 3: No DB Records Returned
- **Look for**: `Loaded 0 existing records from backend` message
- This would mean the query isn't finding anything (possibly wrong empId or date range)

### Issue 4: Empty List Built
- **Look for**: `Matched 0 list items out of 0`
- This would mean no list items were created (possibly group selection issue)

## Code Location
File: `AestheticEMR/AestheticEMR.client/src/app/features/staff-roster/create-roster/create-roster-dialog.component.ts`

Methods with debug logging:
- `buildListItems()` lines ~414-444
- `loadExistingRosterData()` lines ~542-563
- `markExistingItemsAsSelected()` lines ~565-600

## VB6 Reference Reminders
From the user's VB6 code:
- `loadListBoxAll()`: Builds the list with all day×shift combinations
- `loadListBoxWithValues()`: Queries DB and marks matching items selected
- Matching is by: **RosterDate** (date) AND **ShiftID** (numeric ID)
- Angular currently uses: **date** (string) AND **shiftAbbrv** (string abbreviation)

The core question: Should Angular match by `shiftId` (numeric) instead of `shiftAbbrv` (string)?

## Build Status
- ✅ TypeScript compilation: SUCCESS
- ⚠️ Module resolution warnings: Pre-existing (environment issue)
- ✅ No new linting errors introduced
- ✅ Ready for runtime testing
