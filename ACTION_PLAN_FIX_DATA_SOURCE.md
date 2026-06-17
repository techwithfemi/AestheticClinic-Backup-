# Action Plan: Fix Dental Page to Use Correct Data Source

## Current Issue

The dental page table currently displays **DentalImaging** (imaging/x-ray records) as the primary data source.

**This is incorrect.** The table should display **HDentalTreat/DentalChart** (treatment records) as primary.

---

## ✅ Solution

### **Step 1: Understand the TypeScript Model**

The frontend already has the correct model:
```typescript
// In: src/app/models/dental.model.ts
export interface DentalChart {
  id: number;
  pno: string;
  consultId: string;
  dtype?: string;              // Treatment type
  tDate: string;               // Treatment date (PRIMARY)
  tTime?: string;              // Treatment time (PRIMARY)
  teethStatus?: Record<string, ToothStatus>;
  oralExam?: OralExam;
  orthodontics?: OrthodonticForm;
  // ... 32 tooth status fields
  patientName?: string;
}
```

**Key Point**: `DentalChart` maps to backend `HDentalTreat` ✅

---

### **Step 2: Verify Backend Has Charts Endpoint**

✅ **Confirmed** - Backend has:

```csharp
// In DentalController
[HttpGet("charts")]                    // GET /api/dental/charts
[HttpGet("charts/{id}")]              // GET /api/dental/charts/{id}
[HttpPost("charts")]                  // POST /api/dental/charts
[HttpPut("charts/{id}")]              // PUT /api/dental/charts/{id}
[HttpDelete("charts/{id}")]           // DELETE /api/dental/charts/{id}
```

And in `DentalEndpoint` service:
```typescript
getChartsEndpoint<T>(): Observable<T>
createChartEndpoint<T>(chart: object): Observable<T>
updateChartEndpoint<T>(id: number, chart: object): Observable<T>
deleteChartEndpoint<T>(id: number): Observable<T>
```

---

### **Step 3: Required Changes to Component**

| Item | Current | Change To |
|------|---------|-----------|
| **Data Signal** | `imagingRecords: DentalImaging[]` | `dentalCharts: DentalChart[]` |
| **Table DataSource** | `MatTableDataSource<DentalImaging>` | `MatTableDataSource<DentalChart>` |
| **Load Endpoint** | `getImagingEndpoint()` | `getChartsEndpoint()` |
| **Table Column** | patient | patient |
| **Table Column** | consultId | consultId |
| **Table Column** | imagingDate → | tDate (Treatment Date) |
| **Table Column** | imagingType → | dtype (Treatment Type) |
| **Table Column** | findings → | (remove, use reports instead) |
| **Table Column** | tTime | tTime (Treatment Time) |
| **Search** | Search across 3 fields | Same + treatment type |

---

### **Step 4: Implementation Details**

#### **4.1 Import DentalChart Model**
```typescript
import { DentalChart, DentalEncounter, DentalImaging } from '../../models/dental.model';
```

#### **4.2 Update Signals**
```typescript
// OLD
readonly imagingRecords = signal<DentalImaging[]>([]);

// NEW
readonly dentalCharts = signal<DentalChart[]>([]);
readonly imagingRecords = signal<DentalImaging[]>([]);  // Still keep for secondary
```

#### **4.3 Update Table DataSource**
```typescript
// OLD
tableDataSource = new MatTableDataSource<DentalImaging>([]);

// NEW
tableDataSource = new MatTableDataSource<DentalChart>([]);
```

#### **4.4 Update Columns Array**
```typescript
// OLD
readonly columns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'findings', 'actions'];

// NEW
readonly columns = ['patient', 'consultId', 'treatmentDate', 'treatmentTime', 'treatmentType', 'actions'];
```

#### **4.5 Update Load Method**
```typescript
// OLD
this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().toPromise()

// NEW
this.dentalEndpoint.getChartsEndpoint<DentalChart[]>().toPromise()
```

#### **4.6 Update Column Definitions in Template**
```html
<!-- OLD -->
<ng-container matColumnDef="imagingDate">
  <th mat-header-cell *matHeaderCellDef>Imaging Date</th>
  <td mat-cell *matCellDef="let row">{{ row.imagingDate | date:'dd-MMM-yyyy' }}</td>
</ng-container>

<!-- NEW -->
<ng-container matColumnDef="treatmentDate">
  <th mat-header-cell *matHeaderCellDef>Treatment Date</th>
  <td mat-cell *matCellDef="let row">{{ row.tDate | date:'dd-MMM-yyyy' }}</td>
</ng-container>

<ng-container matColumnDef="treatmentTime">
  <th mat-header-cell *matHeaderCellDef>Treatment Time</th>
  <td mat-cell *matCellDef="let row">{{ row.tTime | date:'HH:mm' }}</td>
</ng-container>

<ng-container matColumnDef="treatmentType">
  <th mat-header-cell *matHeaderCellDef>Treatment Type</th>
  <td mat-cell *matCellDef="let row">{{ row.dtype || '—' }}</td>
</ng-container>
```

#### **4.7 Update Filter Logic**
```typescript
private filterData(): void {
  const s = this.searchText().trim().toLowerCase();

  let filtered = this.dentalCharts();  // Use charts, not imaging

  if (!s) {
    // Empty search: show only today's treatment records
    filtered = filtered.filter(r => this.isToday(r.tDate));
  } else {
    // Search across chart fields
    filtered = filtered.filter(r =>
      (r.pno || '').toLowerCase().includes(s)
      || (r.consultId || '').toLowerCase().includes(s)
      || this.resolvePatientLabel(r.pno).toLowerCase().includes(s)
      || (r.dtype || '').toLowerCase().includes(s));  // Added treatment type
  }

  this.tableDataSource.data = filtered;
  this.totalRecords.set(filtered.length);
}
```

#### **4.8 Update Delete Method**
```typescript
// OLD
deleteImaging(id: number): void

// NEW
deleteChart(id: number): void {
  this.alertService.showDialog('Delete this dental treatment record?', DialogType.confirm, () => {
    this.alertService.startLoadingMessage('Deleting...');
    this.dentalEndpoint.deleteChartEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.load();
        this.alertService.showMessage('Success', 'Dental treatment deleted.', MessageSeverity.success);
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Delete error', 'Unable to delete record.', MessageSeverity.error, error);
      }
    });
  });
}
```

#### **4.9 Update Edit Method**
```typescript
openEditDialog(row: DentalChart): void {  // Changed from DentalImaging
  const initialTabIndex = 0;

  this.dentalEndpoint.getEncounterEndpoint<DentalEncounter>(row.consultId, row.pno).subscribe({
    next: encounter => {
      const ref = this.dialog.open(DentalEncounterDialogComponent, {
        width: '98vw',
        maxWidth: '980px',
        disableClose: true,
        data: {
          initialTabIndex,
          patientOptions: this.patientOptions(),
          encounter
        }
      });

      ref.afterClosed().subscribe((result: DentalEncounter | undefined) => {
        if (!result) return;
        this.saveEncounter(result);
      });
    },
    error: error => {
      this.alertService.showStickyMessage('Load error', 'Unable to open dental encounter.', MessageSeverity.error, error);
    }
  });
}
```

---

## 🗂️ Data Flow After Fix

```
1. Component loads (ngOnInit)
   ↓
2. Load data from API
   dentalEndpoint.getChartsEndpoint<DentalChart[]>()  // PRIMARY
   ↓
3. Store in signal
   dentalCharts.set(data)
   ↓
4. Apply filter
   filterData()
   ↓
5. Set table datasource
   tableDataSource.data = filtered
   ↓
6. Display table
   Show: Patient | ConsultID | TreatmentDate | TreatmentTime | TreatmentType | Actions
   ↓
7. User clicks edit
   getEncounterEndpoint()  // Gets full encounter (Chart + Imaging + Consulting)
   ↓
8. Dialog opens with ALL data
   - Chart: Odontogram, teeth status
   - Imaging: X-rays (secondary)
   - Consulting: Clinical notes (secondary)
```

---

## 📊 Comparison: Before vs After

### **Before (Incorrect)**
```
Table shows:        DentalImaging records
Primary data:       X-ray imaging records
Purpose:            Show imaging/x-ray dates and types
Problem:            Treatment records not visible!
Reports use:        DentalImaging (secondary)
```

### **After (Correct)**
```
Table shows:        DentalChart (HDentalTreat) records
Primary data:       Treatment records with dates, times, types
Purpose:            Show clinical treatment sessions
Dialog shows:       All related data (treatment + imaging + notes)
Reports use:        DentalImaging & HConsulting (secondary)
```

---

## ✅ Verification Checklist

After making changes:

- [ ] Component compiles without errors
- [ ] Table loads treatment records (not imaging)
- [ ] Treatment date column shows correctly
- [ ] Treatment time column shows correctly
- [ ] Treatment type column shows correctly
- [ ] Search filters by treatment date, type, patient
- [ ] Empty search shows today's treatments only
- [ ] Edit button opens dialog with full encounter data
- [ ] Delete button removes treatment record
- [ ] Page size is still 10
- [ ] Pagination works correctly
- [ ] Tooltips still appear on buttons
- [ ] Action buttons have correct colors

---

## 📝 Summary

**Change from:**
- Table data = DentalImaging (X-rays)

**Change to:**
- Table data = DentalChart (Treatment Records)
- Keep: DentalImaging for reports and secondary data
- Keep: Dialog loads full encounter (all 3 models together)

