# Visual Guide: Dental Page Component

## 📱 User Interface Layout

```
┌────────────────────────────────────────────────────────────────────┐
│                         DENTAL CLINIC PAGE                         │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  Dental Clinic                                [+ Add Dental Info] │
│  Imaging and odontogram captured in one encounter                 │
│                                                                    │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  🔍 Search by patient name, PNO or ConsultID...                  │
│                                                                    │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  ┌──────────────────────────────────────────────────────────────┐ │
│  │ Patient (PNO)          │ Consult │ Imaging  │ Imaging │ Act │ │
│  │                        │   ID    │ Date     │ Type    │ ions│ │
│  ├──────────────────────────────────────────────────────────────┤ │
│  │ John Doe [P000001]     │ C001    │ 15-Jan   │ Panoram │ 🧾 │ │
│  │                        │         │ 2025     │ ic X    │ ✏️  │ │
│  │                        │         │          │ ray     │ 🗑️  │ │
│  ├──────────────────────────────────────────────────────────────┤ │
│  │ Jane Smith [P000002]   │ C002    │ 15-Jan   │ Intraor │ 🧾 │ │
│  │                        │         │ 2025     │ al X    │ ✏️  │ │
│  │                        │         │          │ ray     │ 🗑️  │ │
│  ├──────────────────────────────────────────────────────────────┤ │
│  │ Bob Wilson [P000003]   │ C003    │ 15-Jan   │ Bitewi  │ 🧾 │ │
│  │                        │         │ 2025     │ ng X    │ ✏️  │ │
│  │                        │         │          │ ray     │ 🗑️  │ │
│  └──────────────────────────────────────────────────────────────┘ │
│                                                                    │
│  ◀◀  ◀  [1] 2 3 4 5  ▶  ▶▶   Rows per page: [10▼]              │
│                                                                    │
└────────────────────────────────────────────────────────────────────┘
```

---

## 🔘 Action Buttons Detail

```
┌─────────────────────────────────────────────────────────────┐
│                    ACTION BUTTONS                           │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  🧾 (Blue/Accent)                                          │
│     Tooltip: "Create Bill"                                │
│     Action: Opens billing/invoice dialog                 │
│     Use: When ready to bill the patient                  │
│                                                             │
│  ✏️  (Default/Gray)                                        │
│     Tooltip: "Edit Dental Info"                          │
│     Action: Opens edit dialog with existing data         │
│     Use: To modify dental record                         │
│                                                             │
│  🗑️  (Red/Warn)                                            │
│     Tooltip: "Delete Record"                             │
│     Action: Shows confirmation, then deletes            │
│     Use: To remove record permanently                    │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔄 User Workflows

### Flow 1: Add New Record
```
User clicks [+ Add Dental Info]
         ↓
Dialog opens: DentalEncounterDialog
         ↓
User selects patient from dropdown
         ↓
User enters imaging data:
  - Imaging Date
  - Imaging Type
  - Findings
  - Recommendations
  - etc.
         ↓
User clicks "Save"
         ↓
POST /api/dental/encounter
         ↓
Table refreshes
         ↓
"Dental encounter saved" message appears
```

### Flow 2: Search Records
```
User types in search box
         ↓
Input detected: onSearchChange()
         ↓
filterData() called
         ↓
Searches across 3 fields:
  - Patient name: "John" ✓
  - Patient PNO: "P000001" ✓
  - Consult ID: "C001" ✓
         ↓
Table updates with matching rows
         ↓
Pagination resets to page 1
```

### Flow 3: Delete Record
```
User clicks 🗑️ delete button
         ↓
Confirmation dialog appears:
"Delete this dental record?"
         ↓
User clicks "Yes"
         ↓
DELETE /api/dental/imaging/{id}
         ↓
Table refreshes
         ↓
"Dental record deleted" message appears
```

### Flow 4: Create Invoice
```
User clicks 🧾 bill button
         ↓
Lookup attendance data for consultation
         ↓
Open: BillingInvoiceDialog
         ↓
Pass data:
  - Consultation ID
  - Patient Number
  - Company Name
  - Attendance Info
         ↓
Billing dialog handles invoice creation
```

---

## 📊 Table Data Flow

```
┌─────────────────────┐
│  Backend API        │
│ GET /api/dental/    │
│    imaging          │
└────────────┬────────┘
             │
             │ [DentalImaging[], ...]
             ↓
┌─────────────────────────────────────┐
│  Component Loads (ngOnInit)         │
│                                     │
│  5 Parallel API calls:              │
│  • getImagingEndpoint()            │
│  • getAttendancesEndpoint()        │
│  • getTodayVisitsEndpoint()        │
│  • getHPatientsEndpoint()          │
│  • getHRetainershipsEndpoint()    │
└────────────┬────────────────────────┘
             │
             │ Data stored in Signals
             ↓
┌─────────────────────────────────────┐
│  filterData()                       │
│                                     │
│  Apply search filter                │
│  (if no search → today only)        │
│  (if search → all matching)         │
└────────────┬────────────────────────┘
             │
             │ Set tableDataSource.data
             ↓
┌─────────────────────────────────────┐
│  MatTableDataSource                │
│                                     │
│  Renders rows in table              │
│  Manages pagination                 │
│  Handles page changes               │
└────────────┬────────────────────────┘
             │
             │ Display in <table>
             ↓
┌─────────────────────────────────────┐
│  User Sees Table                    │
│                                     │
│  ✓ Rows displayed (10 per page)    │
│  ✓ Paginator shown                 │
│  ✓ Action buttons ready            │
└─────────────────────────────────────┘
```

---

## 🎨 Component Hierarchy

```
DentalPageComponent
├── Page Header
│   ├── Title & Subtitle
│   └── Add Button
│
├── Search Box
│   └── Input field with ngModel binding
│
└── Material Card
    ├── Empty State
    │   └── "No dental records found"
    │
    └── Table Section
        ├── Mat Table
        │   ├── Header Row
        │   │   ├── Patient (PNO)
        │   │   ├── Consult ID
        │   │   ├── Imaging Date
        │   │   ├── Imaging Type
        │   │   ├── Findings
        │   │   └── Actions
        │   │
        │   └── Data Rows (10 per page)
        │       └── [DentalImaging row]
        │           ├── 🧾 Bill Button
        │           ├── ✏️ Edit Button
        │           └── 🗑️ Delete Button
        │
        └── Mat Paginator
            ├── First/Previous/Next/Last Buttons
            ├── Page Number Display
            └── Page Size Selector [5, 10, 25, 50]
```

---

## 🎬 Timeline: Component Lifecycle

```
Time    Event                           Action
────────────────────────────────────────────────────────
T0      Component Created               Constructor runs

T1      ngOnInit()                      load() called

T2      API Calls Made (parallel)       5 endpoints queried

T3      Data Received                   Signals updated

T4      filterData() Called             Data filtered

T5      tableDataSource Updated         Table renders

T6      ngAfterViewInit()              Paginator connected

T7      Page Ready                      User can interact
```

---

## 🔍 Search Examples

```
┌──────────────────────────────────────────────────────┐
│ Search Box                                           │
├──────────────────────────────────────────────────────┤
│                                                      │
│ Example 1: [Search by patient name]                 │
│ Input: "John"                                        │
│ Results: All records for patients named "John"       │
│                                                      │
│ Example 2: [Search by PNO]                          │
│ Input: "P000001"                                     │
│ Results: All records for patient P000001            │
│                                                      │
│ Example 3: [Search by Consult ID]                   │
│ Input: "C001"                                        │
│ Results: All records for consultation C001          │
│                                                      │
│ Example 4: [Empty search]                           │
│ Input: "" (or cleared)                               │
│ Results: Only today's imaging records               │
│                                                      │
│ Example 5: [No matches]                             │
│ Input: "xyz12345"                                    │
│ Results: "No dental records found"                   │
│                                                      │
└──────────────────────────────────────────────────────┘
```

---

## 📊 Pagination Example

```
Scenario: 45 total records, 10 per page

┌──────────────────────────────────────────────────────────┐
│ Page 1: Records 1-10   (Rows 1-10 visible)             │
│ Page 2: Records 11-20  (Rows 11-20 visible)            │
│ Page 3: Records 21-30  (Rows 21-30 visible)            │
│ Page 4: Records 31-40  (Rows 31-40 visible)            │
│ Page 5: Records 41-45  (Rows 41-45 visible, only 5)    │
└──────────────────────────────────────────────────────────┘

Paginator shows:
◀◀  ◀  [1] 2 3 4 5  ▶  ▶▶

Clicking [2] → Shows records 11-20
Clicking [▶] → Shows records 11-20
Clicking [▶▶] → Shows records 41-45
Changing size to [25] → 2 pages total
```

---

## 🎯 State Management

```
┌───────────────────────────────────────────┐
│        Component State (Signals)          │
├───────────────────────────────────────────┤
│                                           │
│ imagingRecords: DentalImaging[]           │
│  → Raw data from API                      │
│                                           │
│ attendance: Attendance[]                  │
│  → Consultation attendance info           │
│                                           │
│ todayVisits: QryhvisitsForToday[]        │
│  → Patients visiting today                │
│                                           │
│ patients: HPatient[]                      │
│  → Patient master data (names, etc.)      │
│                                           │
│ retainerships: HRetainership[]           │
│  → Company/clinic info                    │
│                                           │
│ patientOptions: DentalPatientOption[]    │
│  → Dropdown options for dialogs           │
│                                           │
│ searchText: string                        │
│  → Current search query                   │
│                                           │
│ totalRecords: number                      │
│  → Count of filtered records              │
│                                           │
└───────────────────────────────────────────┘
        ↓
┌───────────────────────────────────────────┐
│    MatTableDataSource<DentalImaging>      │
├───────────────────────────────────────────┤
│                                           │
│ data: DentalImaging[]                    │
│  → Filtered records to display            │
│                                           │
│ paginator: MatPaginator                   │
│  → Handles pagination                     │
│                                           │
│ sort: (not used yet)                      │
│  → Future sorting capability              │
│                                           │
└───────────────────────────────────────────┘
        ↓
┌───────────────────────────────────────────┐
│         <table mat-table>                 │
├───────────────────────────────────────────┤
│ Renders rows based on data                │
└───────────────────────────────────────────┘
```

---

## ✨ Key Features Visualization

```
┌─────────────────────────────────────────────────────────┐
│            DENTAL PAGE FEATURES                        │
├─────────────────────────────────────────────────────────┤
│                                                         │
│ ✅ SEARCH          ✅ PAGINATION     ✅ CRUD          │
│    • By name        • First page       • Create      │
│    • By PNO         • Previous         • Read        │
│    • By ConsultID   • Next             • Update      │
│    • Today only     • Last             • Delete      │
│                     • Size selector                   │
│                       [5,10,25,50]                   │
│                                                         │
│ ✅ MATERIAL         ✅ TOOLTIPS       ✅ RESPONSIVE   │
│    • Table          • All buttons      • Mobile ok  │
│    • Paginator      • Hover hints      • Tablet ok  │
│    • Icons          • Context help     • Desktop ok│
│    • Colors                                         │
│    • Styling                                        │
│                                                         │
│ ✅ ERROR HANDLING   ✅ USER FEEDBACK  ✅ DATA JOIN  │
│    • API errors     • Loading msgs     • Patient   │
│    • Validation     • Success msgs     • Attendance│
│    • Dialogs        • Delete confirm   • Company   │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

