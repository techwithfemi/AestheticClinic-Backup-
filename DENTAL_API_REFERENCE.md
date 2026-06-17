# Dental Imaging API Reference

## Backend Endpoint Details

### GET /api/dental/imaging
**Purpose**: Retrieve all dental imaging records

**Request**
```
GET /api/dental/imaging HTTP/1.1
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

**Response Status**: 200 OK

**Response Schema**
```json
[
  {
    "id": 1,
    "pno": "P000001",
    "consultId": "C001",
    "imagingDate": "2025-01-15T10:30:00Z",
    "imagingType": "Panoramic X-ray",
    "toothRegion": "Full mouth",
    "findings": "No cavities detected",
    "impression": "Normal dentition",
    "recommendations": "Continue regular dental hygiene",
    "filePath": "/uploads/dental/2025/01/imaging_001.jpg",
    "fileName": "imaging_001.jpg",
    "notes": "Patient had good compliance",
    "patientName": "John Doe",
    "createdBy": "Dr. Smith",
    "createdDate": "2025-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "pno": "P000002",
    "consultId": "C002",
    "imagingDate": "2025-01-15T11:00:00Z",
    "imagingType": "Intraoral X-ray",
    "toothRegion": "Upper right",
    "findings": "Caries on tooth 16",
    "impression": "Mild periodontal disease",
    "recommendations": "Schedule restoration",
    "filePath": "/uploads/dental/2025/01/imaging_002.jpg",
    "fileName": "imaging_002.jpg",
    "notes": "Follow-up after 2 weeks",
    "patientName": "Jane Smith",
    "createdBy": "Dr. Johnson",
    "createdDate": "2025-01-15T11:00:00Z"
  }
]
```

---

## Component Usage Flow

### 1. Load Data (on ngOnInit)
```typescript
// Calls this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>()
private load(): void {
  Promise.all([
    this.dentalEndpoint.getImagingEndpoint<DentalImaging[]>().toPromise(),
    // ... other endpoint calls
  ]).then(([imaging, ...]) => {
    this.imagingRecords.set(imaging || []);  // Store raw data
    this.filterData();                        // Apply filters
  });
}
```

### 2. Display in Table
```typescript
// Filter data and set to DataSource
private filterData(): void {
  let filtered = this.imagingRecords();
  // Apply search filters...
  this.tableDataSource.data = filtered;  // Update table
  this.totalRecords.set(filtered.length);
}

// Table displays via MatTableDataSource
<table mat-table [dataSource]="tableDataSource">
  <!-- Columns rendered from DentalImaging properties -->
</table>
```

### 3. Paginate Results
```typescript
// MatPaginator automatically handles:
// - Page size (default: 10)
// - Page navigation
// - Record count (totalRecords)

<mat-paginator
  [length]="totalRecords"           // Total records after filtering
  [pageSize]="10"                   // Show 10 per page
  [pageSizeOptions]="[5, 10, 25, 50]"
  showFirstLastButtons>
</mat-paginator>
```

---

## Table Column Mapping

| Table Column | Source Property | Format | Description |
|--------------|-----------------|--------|-------------|
| Patient (PNO) | `pno` + patient lookup | `"Name [PNO]"` | Patient identifier with name |
| Consult ID | `consultId` | text | Consultation reference |
| Imaging Date | `imagingDate` | `dd-MMM-yyyy` | Date formatted as 15-Jan-2025 |
| Imaging Type | `imagingType` | text | Type of imaging (X-ray, etc.) |
| Findings | `findings` | truncated @ 320px | Clinical findings (max 320px width) |
| Actions | N/A | buttons | Bill, Edit, Delete operations |

---

## Search Implementation

### Search Query Processing
```typescript
onSearchChange(query: string): void {
  this.searchText.set(query);
  this.filterData();
}

private filterData(): void {
  const s = this.searchText().trim().toLowerCase();
  let filtered = this.imagingRecords();

  if (!s) {
    // Empty search: show today's records only
    filtered = filtered.filter(r => this.isToday(r.imagingDate));
  } else {
    // Search across multiple fields
    filtered = filtered.filter(r =>
      (r.pno || '').toLowerCase().includes(s)           // Search by PNO
      || (r.consultId || '').toLowerCase().includes(s)  // Search by Consult ID
      || this.resolvePatientLabel(r.pno).toLowerCase().includes(s) // Search by Patient Name
    );
  }

  this.tableDataSource.data = filtered;
  this.totalRecords.set(filtered.length);
}
```

### Search Examples
| Query | Result |
|-------|--------|
| `""` (empty) | Shows only today's imaging records |
| `"P000001"` | Records for patient P000001 |
| `"John"` | Records where patient name contains "John" |
| `"C001"` | Records for consultation C001 |
| `"panoramic"` | Records where imaging type contains "panoramic" |

---

## Related Endpoints Called

### During Load
1. **GET /api/dental/imaging** - Dental imaging records
2. **GET /api/attendance** - Patient attendance/consultation history
3. **GET /api/attendance/today-visits** - Today's patient visits
4. **GET /api/patients** - Patient master data (names, DOB, etc.)
5. **GET /api/retainership** - Company/retainer information

### On Save
- **POST /api/dental/encounter** - Create new or update existing encounter

### On Delete
- **DELETE /api/dental/imaging/{id}** - Remove imaging record

---

## Data Enrichment

The component enriches raw API data by joining with other datasets:

```typescript
resolvePatientLabel(pno: string): string {
  // Lookup patient by PNO from loaded patients
  const p = this.patients().find(x => x.pno === pno);
  // Return formatted label: "LastName FirstName [PNO]"
  return p ? `${p.pSurName} ${p.pFirstname ?? ''} [${pno}]`.trim() : `[${pno}]`;
}
```

### Enrichment Examples
```
Raw Data:  pno="P000001"
With Patient Lookup: "John Doe [P000001]"

Raw Data:  consultId="C001", pNo="P000001"
With Attendance Lookup: coyname="ABC Clinic"
Result in Billing Dialog: Uses matched attendance record
```

---

## Error Handling

### Load Errors
```typescript
.catch(error => {
  this.alertService.stopLoadingMessage();
  this.alertService.showStickyMessage(
    'Load error',
    'Unable to load dental records.',
    MessageSeverity.error,
    error
  );
});
```

### Save Errors
```typescript
.subscribe({
  next: () => { /* success */ },
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
```

### Delete Errors
```typescript
.subscribe({
  next: () => { /* success */ },
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
```

---

## Service Architecture

```
┌─────────────────────────────────────────────┐
│     DentalPageComponent                     │
│  (Display and User Interactions)            │
└────────────────┬────────────────────────────┘
                 │
     ┌───────────┼───────────┐
     │           │           │
┌────▼─────┐  ┌─▼─────────────┐  ┌──────────────────┐
│DentalEnd │  │Attendance     │  │HPatient         │
│point     │  │Endpoint       │  │Endpoint         │
└────┬─────┘  └─┬─────────────┘  └──────┬───────────┘
     │         │                         │
     │  ┌──────┴────────────────────────┤
     │  │                               │
┌────▼──▼───────────────────────────────▼─┐
│         EndpointBase                    │
│  (Auto token refresh, error handling)   │
└────────┬──────────────────────────────┬──┘
         │                              │
    ┌────▼──────────────────────────────▼────┐
    │     HttpClient                         │
    │  (Angular HTTP communication)          │
    └────────────────────────────────────────┘
```

---

## Performance Considerations

### Data Caching
- Data is cached in signals (`imagingRecords`, `patients`, etc.)
- Filtered/paginated views don't require new API calls
- Search is performed in-memory on client side

### Pagination
- MatTableDataSource handles pagination efficiently
- Only visible rows are rendered (10 per page default)
- Large datasets (1000+ records) remain responsive

### Search Optimization
- Search is debounced by user input (not automatic)
- Case-insensitive matching for better UX
- Multiple field support (PNO, ConsultID, Patient Name)

