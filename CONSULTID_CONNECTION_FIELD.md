# ConsultID: Connection Field Architecture

## 🎯 Key Understanding

**`consultID`** = **Master Connection Field** that links all three dental models together across the backend tables

```
┌──────────────────────────────────────────────────────────┐
│                    DATABASE TABLES                       │
├──────────────────────────────────────────────────────────┤
│                                                          │
│  HDentalTreat          DentalImaging        HConsulting │
│  ├─ Id                 ├─ Id                ├─ Id       │
│  ├─ Pno                ├─ Pno               ├─ Pno      │
│  ├─ ConsultID ◄────────┼─ ConsultID ◄──────┼─ ConsultID│
│  ├─ TDate              ├─ ImagingDate       ├─ Diagnosis│
│  ├─ TTime              ├─ ImagingType       ├─ Notes    │
│  ├─ Dtype              ├─ Findings          ├─ Plan     │
│  └─ [32 teeth]         └─ FilePath          └─ Services │
│                                                          │
│  PRIMARY              SECONDARY            SECONDARY   │
│  (Treatment)         (X-rays)             (Clinical)   │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## 🔗 How ConsultID Connects Everything

### **In the Frontend Component**

**Step 1: Load Table (Primary Data)**
```typescript
// Load all HDentalTreat records
this.dentalEndpoint.getChartsEndpoint<DentalChart[]>()
// Each record has: id, pno, consultId, tDate, tTime, dtype, teethStatus, etc.
```

**Step 2: User Clicks Edit**
```typescript
openEditDialog(row: DentalChart): void {
  // row.consultId is the CONNECTION FIELD
  this.dentalEndpoint.getEncounterEndpoint<DentalEncounter>(
    row.consultId,  // ◄─── Uses consultID to fetch all related data
    row.pno         // ◄─── And patient number as secondary filter
  ).subscribe({
    next: (encounter) => {
      // encounter contains:
      // • encounter.chart        (HDentalTreat record)
      // • encounter.imaging      (DentalImaging record with same consultID)
      // • encounter.consulting   (HConsulting record with same consultID)
    }
  });
}
```

### **In the Backend Service**

```csharp
public (HDentalTreat Chart, DentalImaging Imaging, HConsulting Consulting)? 
  GetEncounter(string consultId, string pno)
{
  // Query using consultId to find related records
  var chart = dbContext.HDentalTreats
    .FirstOrDefault(x => x.ConsultId == consultId && x.Pno == pno);

  if (chart == null) return null;

  // Use SAME consultId to fetch secondary records
  var imaging = dbContext.DentalImagings
    .FirstOrDefault(x => x.ConsultId == consultId && x.Pno == pno);

  var consulting = dbContext.HConsultings
    .FirstOrDefault(x => x.ConsultId == consultId && x.Pno == pno);

  return (chart, imaging, consulting);
}
```

---

## 📊 Data Flow Using ConsultID

```
┌─────────────────────────────────────────────────────────────┐
│ SCENARIO: User Opens Dental Treatment for Consultation C001 │
└─────────────────────────────────────────────────────────────┘

1. TABLE DISPLAYS
   ┌───────────────────────────────────────────┐
   │ Patient | ConsultID | TDate | TTime | Type│
   ├───────────────────────────────────────────┤
   │ John    │ C001      │ 15-Jan│ 10:30 │ Fill│ ◄── User clicks Edit
   └───────────────────────────────────────────┘

2. CLICK EDIT → Fetch by ConsultID
   getEncounterEndpoint("C001", "P001")

3. BACKEND QUERIES USING ConsultID
   ┌─────────────────────────────────────────┐
   │ Query 1: HDentalTreat                   │
   │ WHERE ConsultId = "C001" AND Pno = "P001"
   │ Result: Treatment record (filling)      │
   └─────────────────────────────────────────┘

   ┌─────────────────────────────────────────┐
   │ Query 2: DentalImaging                  │
   │ WHERE ConsultId = "C001" AND Pno = "P001"
   │ Result: X-ray images for this consult   │
   └─────────────────────────────────────────┘

   ┌─────────────────────────────────────────┐
   │ Query 3: HConsulting                    │
   │ WHERE ConsultId = "C001" AND Pno = "P001"
   │ Result: Clinical notes for this consult │
   └─────────────────────────────────────────┘

4. DIALOG DISPLAYS ALL THREE
   ┌─────────────────────────────────────────┐
   │ TAB 1: TREATMENT (HDentalTreat)         │
   │ ├─ Date: 15-Jan-2025                    │
   │ ├─ Time: 10:30                          │
   │ ├─ Type: Filling                        │
   │ └─ Odontogram: [teeth status]           │
   │                                          │
   │ TAB 2: IMAGING (DentalImaging)          │
   │ ├─ X-ray images                         │
   │ ├─ Findings: "Cavity on tooth 36"       │
   │ └─ Recommendations: "Fill"              │
   │                                          │
   │ TAB 3: NOTES (HConsulting)              │
   │ ├─ Diagnosis: "Caries on 36"            │
   │ ├─ Plan: "Composite filling"            │
   │ └─ Services: "Filling, 1 hour"          │
   └─────────────────────────────────────────┘
```

---

## ✅ Component Verification

The component **correctly** uses `consultID` as the connection field:

### **Loading Primary Data**
```typescript
// Line 319: Load HDentalTreat records
this.dentalEndpoint.getChartsEndpoint<DentalChart[]>()
// Each chart has: id, pno, consultId, tDate, tTime, dtype, etc.
```

### **Using ConsultID to Fetch Related Data**
```typescript
// Line 289: When editing, use consultId + pno to get all related data
this.dentalEndpoint.getEncounterEndpoint<DentalEncounter>(
  row.consultId,  // ◄─── CONNECTION FIELD
  row.pno
)
```

### **Backend Call**
```typescript
// DentalEndpoint service
getEncounterEndpoint<T>(consultId: string, pno: string): Observable<T> {
  return this.http.get<T>(
    `${this.encounterUrl}?consultId=${encodeURIComponent(consultId)}&pno=${encodeURIComponent(pno)}`,
    this.requestHeaders
  );
}
```

---

## 🗂️ Data Model Relationships

### **C# Backend Models**
```csharp
public class HDentalTreat
{
  public long Id { get; set; }
  public string Pno { get; set; }
  public string ConsultId { get; set; }          // ◄── CONNECTION FIELD
  public DateTime TDate { get; set; }
  public DateTime TTime { get; set; }
  public string? Dtype { get; set; }
  // ... 32 tooth properties
}

public class DentalImaging
{
  public int Id { get; set; }
  public string Pno { get; set; }
  public string ConsultId { get; set; }          // ◄── CONNECTION FIELD
  public string ImagingDate { get; set; }
  public string? ImagingType { get; set; }
  public string? Findings { get; set; }
  // ...
}

public class HConsulting
{
  public long Id { get; set; }
  public string Pno { get; set; }
  public string ConsultId { get; set; }          // ◄── CONNECTION FIELD
  public string? Diagnosis { get; set; }
  public string? Notes { get; set; }
  public string? Plan { get; set; }
  // ...
}
```

### **TypeScript Frontend Models**
```typescript
export interface DentalChart {
  id: number;
  pno: string;
  consultId: string;                            // ◄── CONNECTION FIELD
  dtype?: string;
  tDate: string;
  tTime?: string;
  teethStatus?: Record<string, ToothStatus>;
  // ...
}

export interface DentalImaging {
  id: number;
  pno: string;
  consultId: string;                            // ◄── CONNECTION FIELD
  imagingDate: string;
  imagingType?: string;
  findings?: string;
  // ...
}

export interface DentalEncounter {
  chart: DentalChart;                           // PRIMARY
  imaging: DentalImaging;                       // SECONDARY (linked by consultId)
  consulting: DentalConsulting;                 // SECONDARY (linked by consultId)
}
```

---

## 🎯 Summary

**ConsultID Connection Architecture:**

```
┌─────────────────┐
│   consultID     │
│ (Connection ID) │
└────────┬────────┘
         │
    ┌────┴─────┬──────────┐
    ↓          ↓          ↓
┌────────┐ ┌────────┐ ┌────────┐
│Treatment│ │Imaging │ │Clinical│
│ (Main)  │ │(Support)│ │(Support)
└────────┘ └────────┘ └────────┘
```

**How Component Uses It:**

1. **Load**: Get all HDentalTreat records (primary table)
2. **Click Edit**: Use `consultId` from row to fetch full encounter
3. **Backend**: Queries all three tables using `consultId` as filter
4. **Display**: Dialog shows all three models connected by `consultId`

---

## ✅ Current Implementation Status

The component **correctly implements** this architecture:

- ✅ Loads HDentalTreat as primary table data
- ✅ Uses `consultId` as connection field
- ✅ Fetches all related records by `consultId`
- ✅ Displays all three models in dialog
- ✅ Searches based on HDentalTreat data
- ✅ DentalImaging and HConsulting available as secondary data

**Everything is wired correctly!**

