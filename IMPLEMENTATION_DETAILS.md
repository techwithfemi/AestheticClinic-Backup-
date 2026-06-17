# Implementation Details: Dental Page Component

## File Information

**Path**: `AestheticEMR/AestheticEMR.client/src/app/features/dental/dental-page.component.ts`

**Component Name**: `DentalPageComponent`

**Type**: Standalone Angular Component

**Route**: `dental/clinical-session`

---

## Class Definition

```typescript
export class DentalPageComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private readonly route = inject(ActivatedRoute);
  private readonly dialog = inject(MatDialog);
  private readonly alertService = inject(AlertService);
  private readonly dentalEndpoint = inject(DentalEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);

  // Signals for reactive data
  readonly imagingRecords = signal<DentalImaging[]>([]);
  readonly attendance = signal<Attendance[]>([]);
  readonly todayVisits = signal<QryhvisitsForToday[]>([]);
  readonly patients = signal<HPatient[]>([]);
  readonly retainerships = signal<HRetainership[]>([]);
  readonly patientOptions = signal<DentalPatientOption[]>([]);

  // Configuration
  readonly columns = ['patient', 'consultId', 'imagingDate', 'imagingType', 'findings', 'actions'];
  readonly searchText = signal('');
  readonly totalRecords = signal(0);

  // Material DataSource for table
  tableDataSource = new MatTableDataSource<DentalImaging>([]);
}
```

---

## Lifecycle Hooks

### ngOnInit()
Called when component initializes. Loads all data from APIs.

```typescript
ngOnInit(): void {
  this.load();
}
```

**What happens**:
1. Starts loading message
2. Calls 5 API endpoints in parallel
3. Stores results in signals
4. Applies filters
5. Updates table
6. Stops loading message

### ngAfterViewInit()
Called after view is fully initialized. Connects paginator to DataSource.

```typescript
ngAfterViewInit(): void {
  this.tableDataSource.paginator = this.paginator;
}
```

**What happens**:
- Enables automatic pagination in Material table
- Allows MatPaginator to control page navigation

---

## Public Methods

### openAddDialog()
Opens dialog to create new dental record.

```typescript
openAddDialog(): void {
  const initialTabIndex = 0;
  const ref = this.dialog.open(DentalEncounterDialogComponent, {
    width: '98vw',
    maxWidth: '980px',
    disableClose: true,
    data: { initialTabIndex, patientOptions: this.patientOptions() }
  });
  ref.afterClosed().subscribe((result: DentalEncounter | undefined) => {
    if (!result) return;
    this.saveEncounter(result);
  });
}
```

**Parameters**: None

**Returns**: void

**Side effects**: 
- Opens dialog
- On save: calls `saveEncounter()`
- Refreshes table

---

### openEditDialog(row: DentalImaging)
Opens dialog to edit existing dental record.

```typescript
openEditDialog(row: DentalImaging): void {
  const initialTabIndex = 0;
  this.dentalEndpoint.getEncounterEndpoint<DentalEncounter>(row.consultId, row.pno).subscribe({
    next: encounter => {
      const ref = this.dialog.open(DentalEncounterDialogComponent, {
        width: '98vw',
        maxWidth: '980px',
        disableClose: true,
        data: { initialTabIndex, patientOptions: this.patientOptions(), encounter }
      });
      ref.afterClosed().subscribe((result: DentalEncounter | undefined) => {
        if (!result) return;
        this.saveEncounter(result);
      });
    },
    error: error => {
      this.alertService.showStickyMessage(
        'Load error',
        'Unable to open dental encounter.',
        MessageSeverity.error,
        error
      );
    }
  });
}
```

**Parameters**: `row` - The table row data

**Returns**: void

**Side effects**:
- Fetches full encounter from API
- Opens dialog
- On save: calls `saveEncounter()`

---

### openBilling(row: DentalImaging)
Opens billing/invoice dialog for the consultation.

```typescript
openBilling(row: DentalImaging): void {
  const attendance = this.attendance().find(a => a.consultId === row.consultId && a.pNo === row.pno);
  const ref = this.dialog.open(BillingInvoiceDialogComponent, {
    width: '57vw',
    maxWidth: '780px',
    disableClose: true,
    data: {
      mode: 'create',
      consultId: row.consultId,
      billNo: row.consultId,
      coyID: attendance?.coyname ?? '',
      pNo: row.pno,
      clientID: attendance?.coyname ?? ''
    }
  });
}
```

**Parameters**: `row` - The table row data

**Returns**: void

**Side effects**: Opens billing dialog

---

### deleteImaging(id: number)
Deletes a dental imaging record.

```typescript
deleteImaging(id: number): void {
  this.alertService.showDialog('Delete this dental record?', DialogType.confirm, () => {
    this.alertService.startLoadingMessage('Deleting...');
    this.dentalEndpoint.deleteImagingEndpoint<void>(id).subscribe({
      next: () => {
        this.alertService.stopLoadingMessage();
        this.load();
        this.alertService.showMessage(
          'Success',
          'Dental record deleted.',
          MessageSeverity.success
        );
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(
          'Delete error',
          'Unable to delete record.',
          MessageSeverity.error,
          error
        );
      }
    });
  });
}
```

**Parameters**: `id` - Record ID to delete

**Returns**: void

**Side effects**:
- Shows confirmation dialog
- If confirmed: DELETE from API
- Refreshes table

---

### onSearchChange(query: string)
Called when user types in search box.

```typescript
onSearchChange(query: string): void {
  this.searchText.set(query);
  this.filterData();
}
```

**Parameters**: `query` - Search string entered by user

**Returns**: void

**Side effects**: Updates table with filtered results

---

### onPageChange(_event: PageEvent)
Called when user changes page or page size.

```typescript
onPageChange(_event: PageEvent): void {
  // MatTableDataSource handles pagination automatically
}
```

**Parameters**: `_event` - Page change event (unused)

**Returns**: void

**Note**: MatTableDataSource handles pagination automatically

---

### resolvePatientLabel(pno: string): string
Resolves patient name from PNO.

```typescript
resolvePatientLabel(pno: string): string {
  const p = this.patients().find(x => x.pno === pno);
  return p ? `${p.pSurName} ${p.pFirstname ?? ''} [${pno}]`.trim() : `[${pno}]`;
}
```

**Parameters**: `pno` - Patient number

**Returns**: Formatted string like "John Doe [P000001]"

---

## Private Methods

### load()
Loads all data from backend APIs.

```typescript
private load(): void {
  this.alertService.startLoadingMessage('Loading dental records...');
  Promise.all([
    this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().toPromise(),
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>().toPromise(),
    this.attendanceEndpoint.getTodayVisitsEndpoint<QryhvisitsForToday[]>().toPromise(),
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>().toPromise(),
    this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>().toPromise()
  ]).then(([imaging, attendance, todayVisits, patients, retainerships]) => {
    this.imagingRecords.set(imaging || []);
    this.attendance.set(attendance || []);
    this.todayVisits.set(todayVisits || []);
    this.patients.set(patients || []);
    this.retainerships.set(retainerships || []);
    this.patientOptions.set(this.buildPatientOptions());
    this.filterData();
    this.alertService.stopLoadingMessage();
  }).catch(error => {
    this.alertService.stopLoadingMessage();
    this.alertService.showStickyMessage(
      'Load error',
      'Unable to load dental records.',
      MessageSeverity.error,
      error
    );
  });
}
```

**Parallel API calls**:
1. `getImagingEndpoint()` - Dental records
2. `getAttendancesEndpoint()` - Attendance data
3. `getTodayVisitsEndpoint()` - Today's visits
4. `getHPatientsEndpoint()` - Patient info
5. `getHRetainershipsEndpoint()` - Company info

---

### filterData()
Applies search filter and updates table.

```typescript
private filterData(): void {
  const s = this.searchText().trim().toLowerCase();

  let filtered = this.imagingRecords();

  if (!s) {
    // Empty search: show only today's records
    filtered = filtered.filter(r => this.isToday(r.imagingDate));
  } else {
    // Search across multiple fields
    filtered = filtered.filter(r =>
      (r.pno || '').toLowerCase().includes(s)
      || (r.consultId || '').toLowerCase().includes(s)
      || this.resolvePatientLabel(r.pno).toLowerCase().includes(s)
    );
  }

  this.tableDataSource.data = filtered;
  this.totalRecords.set(filtered.length);
}
```

**Logic**:
- Empty search: Today's records only
- With search: All records matching any field

---

### saveEncounter(payload: DentalEncounter)
Saves new or updated dental encounter.

```typescript
private saveEncounter(payload: DentalEncounter): void {
  this.alertService.startLoadingMessage('Saving dental info...');
  this.dentalEndpoint.saveEncounterEndpoint<DentalEncounter>(payload).subscribe({
    next: () => {
      this.alertService.stopLoadingMessage();
      this.load();
      this.alertService.showMessage(
        'Success',
        'Dental encounter saved.',
        MessageSeverity.success
      );
    },
    error: error => {
      this.alertService.stopLoadingMessage();
      this.alertService.showStickyMessage(
        'Save error',
        'Unable to save dental encounter.',
        MessageSeverity.error,
        error
      );
    }
  });
}
```

**Parameters**: `payload` - DentalEncounter object to save

**Side effects**: 
- POST to API
- Refreshes table on success

---

### buildPatientOptions(): DentalPatientOption[]
Builds patient options for dialogs.

```typescript
private buildPatientOptions(): DentalPatientOption[] {
  const unique = new Map<string, QryhvisitsForToday>();
  for (const item of this.todayVisits()) {
    if (!item.consultId || !item.pNo) continue;
    const key = `${item.consultId}|${item.pNo}`;
    if (!unique.has(key)) unique.set(key, item);
  }

  return Array.from(unique.values()).map(item => {
    const p = this.patients().find(x => x.pno === item.pNo);
    const fullName = (item.fullname || `${p?.pSurName ?? 'Unknown'} ${p?.pFirstname ?? ''}`).trim();
    const attendDate = this.formatAttendDate(item.recDate);
    const retainership = this.retainerships().find(x => x.retainId === item.coyName);
    const companyName = item.retainName || retainership?.retainName || p?.coyName || item.coyName;
    return {
      pNo: item.pNo,
      consultId: item.consultId,
      clientCat: item.clientCat,
      label: `${fullName} ${attendDate} [${item.consultId}]`,
      fullName,
      attendDate,
      photo: p?.patPixBase64,
      dateOfBirth: p?.dob,
      companyName,
      coyId: item.coyName,
      clinic: item.clinicType
    } as DentalPatientOption;
  }).sort((a, b) => a.label.localeCompare(b.label));
}
```

**Returns**: Sorted array of DentalPatientOption

---

### formatAttendDate(value?: string): string
Formats date string to dd-MMM-yyyy format.

```typescript
private formatAttendDate(value?: string): string {
  if (!value) return '';
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return '';
  return date.toLocaleDateString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  }).replace(/ /g, '-');
}
```

**Parameters**: `value` - Date string

**Returns**: Formatted date like "15-Jan-2025"

---

### isToday(value?: string): boolean
Checks if date is today.

```typescript
private isToday(value?: string): boolean {
  if (!value) return false;
  const recordDate = new Date(value);
  const today = new Date();
  return recordDate.toDateString() === today.toDateString();
}
```

**Parameters**: `value` - Date string to check

**Returns**: true if date is today

---

## Template Structure

```html
<div class="dental-page">
  <!-- Page Header -->
  <div class="page-header">
    <div>
      <h2>Dental Clinic</h2>
      <p class="subtitle">...</p>
    </div>
    <button mat-raised-button color="primary" (click)="openAddDialog()">
      <mat-icon>add</mat-icon>
      Add Dental Info
    </button>
  </div>

  <!-- Search Box -->
  <div class="search-row">
    <input type="text" class="search-input" 
           [(ngModel)]="searchText" 
           (ngModelChange)="onSearchChange($event)"
           placeholder="Search..." />
  </div>

  <!-- Material Card with Table -->
  <mat-card>
    @if (tableDataSource.data.length === 0) {
      <p class="empty">No dental records found.</p>
    } @else {
      <!-- Table -->
      <div class="table-container">
        <table mat-table [dataSource]="tableDataSource" class="dental-table">
          <!-- Column Definitions -->
          <!-- Patient Column -->
          <!-- Consult ID Column -->
          <!-- Imaging Date Column -->
          <!-- Imaging Type Column -->
          <!-- Findings Column -->
          <!-- Actions Column -->

          <tr mat-header-row *matHeaderRowDef="columns"></tr>
          <tr mat-row *matRowDef="let row; columns: columns"></tr>
        </table>
      </div>

      <!-- Paginator -->
      <mat-paginator
        #paginator
        [length]="totalRecords"
        [pageSize]="10"
        [pageSizeOptions]="[5, 10, 25, 50]"
        (page)="onPageChange($event)"
        showFirstLastButtons>
      </mat-paginator>
    }
  </mat-card>
</div>
```

---

## Material Components Used

| Module | Components | Purpose |
|--------|-----------|---------|
| MatTableModule | mat-table | Display records in table |
| MatPaginatorModule | mat-paginator | Page navigation |
| MatButtonModule | mat-button | Action buttons |
| MatIconModule | mat-icon | Icons (add, edit, delete) |
| MatCardModule | mat-card | Card container |
| MatTooltipModule | matTooltip | Button tooltips |

---

## CSS Classes

```css
.dental-page { padding: 20px; }
.page-header { display: flex; justify-content: space-between; }
.subtitle { color: #666; font-size: 0.9rem; }
.search-row { margin-bottom: 12px; }
.search-input { width: 100%; padding: 10px; border: 1px solid #ddd; }
.table-container { overflow-x: auto; }
.dental-table { width: 100%; border-collapse: collapse; }
.dental-table thead th { background-color: #f5f5f5; border-bottom: 2px solid #e0e0e0; }
.dental-table tbody tr:hover { background-color: #fafafa; }
.truncate { max-width: 320px; overflow: hidden; text-overflow: ellipsis; }
.empty { color: #888; text-align: center; padding: 20px; }
```

---

## Dependency Injection

```typescript
private readonly route = inject(ActivatedRoute);
private readonly dialog = inject(MatDialog);
private readonly alertService = inject(AlertService);
private readonly dentalEndpoint = inject(DentalEndpoint);
private readonly attendanceEndpoint = inject(AttendanceEndpoint);
private readonly patientEndpoint = inject(HPatientEndpoint);
private readonly retainershipEndpoint = inject(HRetainershipEndpoint);
```

All services are injected using Angular's `inject()` function.

