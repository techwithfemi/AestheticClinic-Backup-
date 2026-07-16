# Visual Flow Diagrams

## Complete Data Flow

```
╔═══════════════════════════════════════════════════════════════════╗
║                    ROSTER DIALOG COMPONENT                        ║
║                                                                   ║
║  buildListItems()                                                 ║
║  ┌─────────────────────────────────────────────────────────────┐ ║
║  │ For each date in month × shifts for dept:                 │ ║
║  │                                                             │ ║
║  │ listItems.push({                                            │ ║
║  │   date: "2026-07-14"                                        │ ║
║  │   shiftId: 123                                              │ ║
║  │   shiftName: "Morning"                                      │ ║
║  │   shiftAbbrv: "AM"                                          │ ║
║  │   label: "14 Jul 2026  Morning [AM]  Monday"               │ ║
║  │   selected: false         ← Unchecked initially            │ ║
║  │ })                                                          │ ║
║  │                                                             │ ║
║  │ Repeat for all shifts:                                     │ ║
║  │ - Morning [AM]                                             │ ║
║  │ - Afternoon [PM]                                           │ ║
║  │ - Evening [EV]                                             │ ║
║  │ - Night [NT]                                               │ ║
║  └─────────────────────────────────────────────────────────────┘ ║
║                            ↓                                      ║
║  User checks/unchecks boxes                                       ║
║  ┌─────────────────────────────────────────────────────────────┐ ║
║  │ "14 Jul 2026  Morning [AM]  Monday"    ✓ checked          │ ║
║  │ "14 Jul 2026  Afternoon [PM]  Monday"  ☐ unchecked        │ ║
║  │ "14 Jul 2026  Evening [EV]  Monday"    ☐ unchecked        │ ║
║  │ "14 Jul 2026  Night [NT]  Monday"      ✓ checked          │ ║
║  └─────────────────────────────────────────────────────────────┘ ║
║                            ↓                                      ║
║  save() Method                                                    ║
║  ┌──────────────────────────┬─────────────────────────────────┐ ║
║  │   SELECTED ITEMS         │    UNSELECTED ITEMS             │ ║
║  │  (selected === true)     │   (selected === false)          │ ║
║  ├──────────────────────────┼─────────────────────────────────┤ ║
║  │ Items:                   │ Items:                          │ ║
║  │ - Morning [AM]  ✓        │ - Afternoon [PM]  ☐             │ ║
║  │ - Night [NT]    ✓        │ - Evening [EV]   ☐             │ ║
║  │                          │                                 │ ║
║  │ Map to:                  │ Map to:                         │ ║
║  │ selectedDays = [         │ unselectedDays = [              │ ║
║  │   {                      │   {                             │ ║
║  │     date: "2026-07-14"  │     date: "2026-07-14"         │ ║
║  │     shiftId: 123        │     shiftId: 124               │ ║
║  │     shiftAbbrv: "AM"    │     shiftAbbrv: "PM"           │ ║
║  │     shiftName: "Morning"│     shiftName: "Afternoon"     │ ║
║  │   },                    │   },                            │ ║
║  │   {                     │   {                             │ ║
║  │     date: "2026-07-14" │     date: "2026-07-14"         │ ║
║  │     shiftId: 128       │     shiftId: 126               │ ║
║  │     shiftAbbrv: "NT"   │     shiftAbbrv: "EV"           │ ║
║  │     shiftName: "Night" │     shiftName: "Evening"       │ ║
║  │   }                    │   }                             │ ║
║  │ ]                      │ ]                               │ ║
║  └──────────────────────────┴─────────────────────────────────┘ ║
║                            ↓                                      ║
║  commitSave({                                                     ║
║    deptId: "DEPT001"                                              ║
║    deptName: "Dental Clinic"                                     ║
║    groupId: 5                                                     ║
║    groupName: "Morning Shift Group"                              ║
║    selectedDays: [...]                                            ║
║    unselectedDays: [...]    ← NEW                                ║
║  })                                                               ║
║                            ↓                                      ║
║  POST /api/roster                                                ║
╚═══════════════════════════════════════════════════════════════════╝
                             ↓
╔═══════════════════════════════════════════════════════════════════╗
║              ASP.NET CORE BACKEND                                ║
║                                                                   ║
║  RosterController.Save()                                          ║
║  ↓                                                                ║
║  AutoMapper: RosterSaveVM → RosterSaveRequest                    ║
║  ↓                                                                ║
║  RosterService.SaveAsync()                                        ║
║  ┌─────────────────────────────────────────────────────────────┐ ║
║  │ Step 1: Delete existing month                              │ ║
║  │ DELETE FROM Roster                                         │ ║
║  │ WHERE RosterDate BETWEEN @Start AND @End                   │ ║
║  │   AND GroupID = 5                                          │ ║
║  └─────────────────────────────────────────────────────────────┘ ║
║                            ↓                                      ║
║  ┌──────────────────────────┬─────────────────────────────────┐ ║
║  │ Step 2: Insert Selected  │ Step 3: Insert Unselected      │ ║
║  ├──────────────────────────┼─────────────────────────────────┤ ║
║  │ ForEach selectedDay:     │ ForEach unselectedDay:         │ ║
║  │                          │                                 │ ║
║  │ isOffDuty = Check if:    │ isOffDuty = Check if:         │ ║
║  │   - OFF_DUTY ShiftId?    │   - OFF_DUTY ShiftId?         │ ║
║  │   - LEAVE ShiftId?       │   - LEAVE ShiftId?            │ ║
║  │   → isOffDuty = 1        │   - ShiftId == 0?             │ ║
║  │   → Otherwise: 0         │   → isOffDuty = 1             │ ║
║  │                          │                                 │ ║
║  │ INSERT INTO Roster (     │ INSERT INTO Roster (           │ ║
║  │   ShiftID = 123          │   ShiftID = 124               │ ║
║  │   ShiftName = "Morning"  │   ShiftName = "Afternoon"    │ ║
║  │   ShiftAbbrv = "AM"      │   ShiftAbbrv = "PM"          │ ║
║  │   isOffDuty = 0          │   isOffDuty = 1              │ ║
║  │   RosterDate = date      │   RosterDate = date           │ ║
║  │ )                        │ )                             │ ║
║  └──────────────────────────┴─────────────────────────────────┘ ║
║                            ↓                                      ║
║  ┌─────────────────────────────────────────────────────────────┐ ║
║  │ Step 4: Return saved items                                 │ ║
║  │ Query: SELECT * FROM Roster WHERE ... AND EmpID = @EmpId  │ ║
║  │ Return: RosterSaveResult { CreatedCount, Items }          │ ║
║  └─────────────────────────────────────────────────────────────┘ ║
╚═══════════════════════════════════════════════════════════════════╝
                             ↓
╔═══════════════════════════════════════════════════════════════════╗
║                    DATABASE (SQL SERVER)                         ║
║                         Roster Table                             ║
║  ┌───────────────────────────────────────────────────────────┐  ║
║  │ SNo│Date│EmpID│ShiftID│ShiftName│ShiftAbbrv│isOffDuty     │  ║
║  ├───┼────┼─────┼───────┼─────────┼──────────┼──────────────┤  ║
║  │1  │14-Jul│EMP1│  123 │Morning  │AM        │0 (confirmed) │  ║
║  │2  │14-Jul│EMP1│  124 │Afternoon│PM        │1 (pending)   │  ║
║  │3  │14-Jul│EMP1│  128 │Night    │NT        │0 (confirmed) │  ║
║  │4  │14-Jul│EMP1│  126 │Evening  │EV        │1 (pending)   │  ║
║  │5  │15-Jul│EMP1│  123 │Morning  │AM        │0 (confirmed) │  ║
║  │6  │15-Jul│EMP1│  124 │Afternoon│PM        │1 (pending)   │  ║
║  │... ... ... ... ... ... ... ...                           │  ║
║  └───────────────────────────────────────────────────────────┘  ║
║                                                                   ║
║  Result:                                                          ║
║  - All shifts for each day are recorded                          ║
║  - Checked shifts (isOffDuty=0): User confirmed these           ║
║  - Unchecked shifts (isOffDuty=1): User needs to fill these     ║
║  - User can see what needs to be completed                      ║
╚═══════════════════════════════════════════════════════════════════╝
```

---

## Comparison: Before vs After

### BEFORE
```
┌─────────────────────────────────────────────────────────┐
│ Frontend Sends:                                         │
│ {                                                       │
│   selectedDays: [Morning, Night]  ← Only selected     │
│ }                                                       │
└─────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────┐
│ Backend Logic:                                          │
│ FOR date = monthStart TO monthEnd                       │
│   IF date NOT IN selectedDays THEN                      │
│     INSERT "PLS_ENTER_SHIFT"  ← Hardcoded!            │
│   END IF                                                │
│ END FOR                                                 │
│                                                         │
│ Problem: Backend calculates, hardcodes placeholder     │
└─────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────┐
│ Result: Grid shows                                      │
│ - Morning (confirmed)                                   │
│ - Night (confirmed)                                     │
│ - Afternoon (PLS_ENTER_SHIFT) ← Hardcoded!            │
│ - Evening (PLS_ENTER_SHIFT)   ← Hardcoded!            │
│                                                         │
│ Issue: No control, no real data                        │
└─────────────────────────────────────────────────────────┘
```

### AFTER
```
┌─────────────────────────────────────────────────────────┐
│ Frontend Sends:                                         │
│ {                                                       │
│   selectedDays: [                                       │
│     {date: 14-Jul, shiftId: 123, name: "Morning"}      │
│     {date: 14-Jul, shiftId: 128, name: "Night"}        │
│   ],                                                    │
│   unselectedDays: [  ← NEW!                            │
│     {date: 14-Jul, shiftId: 124, name: "Afternoon"}   │
│     {date: 14-Jul, shiftId: 126, name: "Evening"}     │
│   ]                                                     │
│ }                                                       │
└─────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────┐
│ Backend Logic:                                          │
│ FOR EACH selectedDay                                    │
│   INSERT day WITH isOffDuty = 0                         │
│ END FOR                                                 │
│                                                         │
│ FOR EACH unselectedDay  ← NEW!                         │
│   INSERT day WITH isOffDuty = 1                        │
│ END FOR                                                 │
│                                                         │
│ Benefit: Simple, data-driven, no calculation          │
└─────────────────────────────────────────────────────────┘
                         ↓
┌─────────────────────────────────────────────────────────┐
│ Result: Grid shows                                      │
│ - Morning (confirmed, isOffDuty=0)                      │
│ - Night (confirmed, isOffDuty=0)                        │
│ - Afternoon (pending, isOffDuty=1) ← Real data!       │
│ - Evening (pending, isOffDuty=1)   ← Real data!       │
│                                                         │
│ Benefit: Control, real data, user can see what's needed│
└─────────────────────────────────────────────────────────┘
```

---

## Extraction Logic (Same for Both)

```
┌──────────────────────────────────────────────────────────────┐
│             DayShiftItem Object (Created in buildListItems)  │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ key: "2026-07-14|123"                                 │ │
│  │ date: "2026-07-14"           ← Pre-extracted         │ │
│  │ label: "14 Jul 2026 Morning [AM] Monday"              │ │
│  │ shiftId: 123                 ← Pre-extracted         │ │
│  │ shiftName: "Morning"         ← Pre-extracted         │ │
│  │ shiftAbbrv: "AM"             ← Pre-extracted         │ │
│  │ dayName: "Monday"                                     │ │
│  │ selected: boolean            ← Toggled by user       │ │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────┘
                         ↓
┌──────────────────────────────────────────────────────────────┐
│                  Save Method Extraction                      │
│  ┌────────────────────────┬──────────────────────────────┐  │
│  │   FILTER: selected     │   FILTER: !selected         │  │
│  ├────────────────────────┼──────────────────────────────┤  │
│  │   .map(i => ({         │   .map(i => ({              │  │
│  │     date: i.date      │     date: i.date          │  │
│  │     shiftId: i.shift  │     shiftId: i.shift      │  │
│  │     shiftAbbrv: ...   │     shiftAbbrv: ...       │  │
│  │     shiftName: ...    │     shiftName: ...        │  │
│  │   }))                 │   }))                     │  │
│  │                       │                            │  │
│  │   → selectedDays[]    │   → unselectedDays[]      │  │
│  └────────────────────────┴──────────────────────────────┘ │
│   ✓ IDENTICAL EXTRACTION LOGIC FOR BOTH!                   │
└──────────────────────────────────────────────────────────────┘
```

---

## isOffDuty Flag Decision

```
┌─────────────────────────────────────────────────┐
│ Selected Items Processing                       │
├─────────────────────────────────────────────────┤
│ isOffDuty = Check if:                          │
│  ├─ ShiftId === OFF_DUTY_ShiftID → 1          │
│  ├─ ShiftId === LEAVE_ShiftID    → 1          │
│  └─ Otherwise                    → 0          │
│                                                 │
│ Result: Explicit shift assignment              │
└─────────────────────────────────────────────────┘
              VS
┌─────────────────────────────────────────────────┐
│ Unselected Items Processing                    │
├─────────────────────────────────────────────────┤
│ isOffDuty = Check if:                          │
│  ├─ ShiftId === OFF_DUTY_ShiftID → 1          │
│  ├─ ShiftId === LEAVE_ShiftID    → 1          │
│  ├─ ShiftId === 0 (blank)        → 1          │
│  └─ Otherwise                    → 1 (NEW)   │
│                                                 │
│ Result: Placeholder, needs user review         │
└─────────────────────────────────────────────────┘

Key Difference:
- Selected: Only OFF_DUTY/LEAVE marked as 1
- Unselected: All marked as 1 (pending review)
```

