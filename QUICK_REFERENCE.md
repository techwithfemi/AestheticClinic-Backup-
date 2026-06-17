# Quick Reference: Dental Page Component

## 🎯 Quick Overview

**What Changed**: The dental page now uses Angular Material's `MatTableDataSource` with proper pagination, tooltips, and improved styling.

**Key Improvements**:
- ✅ Page size default = 10 records
- ✅ Material Design table with proper styling
- ✅ Tooltips on all action buttons
- ✅ Delete button now enabled
- ✅ Semantic button colors (accent, warn)
- ✅ Better search and filter logic

---

## 📊 Data Source

**Backend**: `GET /api/dental/imaging` → Returns `DentalImaging[]`

```typescript
interface DentalImaging {
  id: number;              // Record ID
  pno: string;             // Patient number
  consultId: string;       // Consultation ID
  imagingDate: string;     // When the imaging was done
  imagingType?: string;    // Type: X-ray, Panoramic, etc.
  findings?: string;       // What was found
  filePath?: string;       // Image file location
  // ... and more fields
}
```

---

## 🖥️ Component Structure

```typescript
export class DentalPageComponent implements OnInit, AfterViewInit {
  // Data storage
  imagingRecords: DentalImaging[] = [];

  // Material table
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  tableDataSource = new MatTableDataSource<DentalImaging>([]);

  // Configuration
  columns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'findings', 'actions'];
  pageSize = 10;  // Default

  // Actions
  openAddDialog()
  openEditDialog(row)
  openBilling(row)
  deleteImaging(id)
  onSearchChange(query)
}
```

---

## 📋 Table Columns

| Column | Data Source | Display Format |
|--------|-------------|-----------------|
| Patient (PNO) | `pno` + lookup | "John Doe [P000001]" |
| Consult ID | `consultId` | "C001" |
| Imaging Date | `imagingDate` | "15-Jan-2025" |
| Imaging Type | `imagingType` | "Panoramic X-ray" |
| Findings | `findings` | Truncated @ 320px |
| Actions | buttons | Bill • Edit • Delete |

---

## 🔍 Search Behavior

**Empty search**: Shows today's records only
**With search term**: Searches across:
- Patient name (fuzzy match)
- Patient PNO
- Consultation ID

```typescript
// Example searches:
"" → Today's records
"P000001" → Patient P000001's records
"John" → Records where patient name contains "John"
"C001" → Consultation C001 records
```

---

## 🎨 Button Actions & Colors

```
┌─────────────────┬──────────┬──────────────┬─────────────────┐
│ Button          │ Icon     │ Color        │ Tooltip         │
├─────────────────┼──────────┼──────────────┼─────────────────┤
│ Bill Patient    │ receipt  │ accent       │ Create Bill     │
│ Edit            │ edit     │ default      │ Edit Dental Info│
│ Delete          │ delete   │ warn (red)   │ Delete Record   │
└─────────────────┴──────────┴──────────────┴─────────────────┘
```

---

## 📱 Pagination

```
┌─────────────────────────────────────────────────────┐
│ Records: 45 total, showing 10 per page              │
│                                                     │
│ [◄◄] [<] Page 1 [>] [►►]  [5▼] [10▼] [25▼] [50▼]  │
│                                                     │
│ [First] [Previous] [Next] [Last]                    │
└─────────────────────────────────────────────────────┘

Default: 10 records per page
Options: 5, 10, 25, 50
```

---

## 🔄 User Workflow

```
1. Component loads → Load data from API
                  → Display in Material table
                  → Page size = 10

2. User searches → Filter applied
                → Table updates
                → Paginator resets to page 1

3. User clicks "Bill Patient" → Open billing dialog
                               → Pass consultation ID
                               → Pass patient info

4. User clicks "Edit" → Load full encounter from API
                     → Open edit dialog with data
                     → Allow modifications

5. User clicks "Delete" → Show confirmation dialog
                       → If confirmed: DELETE from API
                       → Refresh table

6. User clicks "Add" → Open dialog for new record
                    → Save to API
                    → Refresh table
```

---

## 💾 Data Flow

```
API Call
   ↓
imagingRecords (signal)
   ↓
filterData() method
   ↓
tableDataSource.data = filtered
   ↓
<table [dataSource]="tableDataSource">
   ↓
MatTableDataSource renders rows
   ↓
MatPaginator shows pages
```

---

## 🛠️ Common Tasks

### Search for a patient
```typescript
// User types in search box
onSearchChange("John")
// Component filters imagingRecords
// Shows only records where patient name contains "John"
```

### Show next page
```typescript
// User clicks next page button
// MatPaginator handles this automatically
// tableDataSource shows rows 11-20 (if exists)
```

### Create new dental record
```typescript
// User clicks "Add Dental Info"
openAddDialog()
// Opens DentalEncounterDialogComponent
// User fills form and clicks Save
saveEncounter(payload)
// POST to /api/dental/encounter
// Refresh table
load()
```

### Delete a record
```typescript
// User clicks Delete on row
deleteImaging(id)
// Show confirmation: "Delete this dental record?"
// If confirmed:
// DELETE /api/dental/imaging/{id}
// Refresh table
```

---

## ⚙️ Configuration

### Default Page Size
```typescript
<mat-paginator [pageSize]="10" ...>
```
Change `10` to any number to adjust default

### Page Size Options
```typescript
<mat-paginator [pageSizeOptions]="[5, 10, 25, 50]" ...>
```
Edit array to show different options

### Search Fields
In `filterData()` method:
```typescript
filtered = filtered.filter(r =>
  (r.pno || '').toLowerCase().includes(s) // PNO
  || (r.consultId || '').toLowerCase().includes(s) // Consult ID
  || this.resolvePatientLabel(r.pno).toLowerCase().includes(s) // Patient name
);
```

### Table Columns
```typescript
readonly columns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'findings', 'actions'];
```
Reorder or add columns here

---

## 🐛 Troubleshooting

| Issue | Cause | Solution |
|-------|-------|----------|
| Table is empty | No records loaded | Check API endpoint returns data |
| Search doesn't work | filterData() not called | Check onSearchChange() binding |
| Pagination missing | Paginator not connected | Check ngAfterViewInit() hook |
| Buttons don't respond | Event handlers not bound | Check (click) directives |
| Tooltips don't appear | MatTooltipModule not imported | Check imports array |

---

## 📞 Related Services

| Service | Purpose |
|---------|---------|
| `DentalEndpoint` | Get/save/delete dental records |
| `AttendanceEndpoint` | Get patient attendance history |
| `HPatientEndpoint` | Get patient info (name, DOB, etc.) |
| `HRetainershipEndpoint` | Get company/clinic info |
| `AlertService` | Show messages and confirmations |

---

## 🎓 Angular Concepts Used

- **Signal**: Store reactive data (`signal()`)
- **Computed**: Derived values (`computed()`)
- **ViewChild**: Reference to child components (`@ViewChild()`)
- **AfterViewInit**: Hook after view is initialized
- **Dependency Injection**: Inject services with `inject()`
- **Standalone Component**: No module declaration needed
- **Material Components**: Table, Paginator, Icon, Button, Tooltip, Dialog

---

## 📦 Imports Required

```typescript
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ViewChild, AfterViewInit } from '@angular/core';
```

---

## ✅ Verification Checklist

- [ ] Component compiles without errors
- [ ] Table displays 10 records per page (default)
- [ ] Search filters by patient name, PNO, Consult ID
- [ ] Pagination works (next, prev, first, last buttons)
- [ ] Page size selector works (5, 10, 25, 50)
- [ ] Add button opens dialog
- [ ] Edit button opens dialog with data
- [ ] Delete button shows confirmation and removes record
- [ ] Bill button opens billing dialog
- [ ] Tooltips appear on button hover
- [ ] Action buttons have correct colors
- [ ] Table rows highlight on hover
- [ ] Empty state shows message when no records

---

## 📚 Documentation

All detailed docs are in these files:
- `DENTAL_PAGE_IMPROVEMENTS.md` - Detailed changes
- `BEFORE_AFTER_COMPARISON.md` - Code comparison
- `DENTAL_API_REFERENCE.md` - API endpoint details
- `SUMMARY_OF_CHANGES.md` - Summary of all changes
- `QUICK_REFERENCE.md` - This file

