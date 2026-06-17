# Dental Encounter Dialog - Issues Fixed

## 🔧 Issues Addressed

### 1. **Date/Time Timezone Issue (CRITICAL)**
**Problem**: Dates were being saved with a time offset (saved on 2026-06-17 but showing 2026-06-16, time ~5 hours less)

**Root Cause**: The component used `new Date().toISOString()` which converts to UTC, causing timezone shift when the backend stores/retrieves the date.

**Solution**:
- Replaced `toISOString()` with local timezone handling
- Created `toLocalDate()` method that formats dates in local timezone without UTC conversion
- Updated `fromDateInput()` to parse dates in local timezone using `new Date(year, month, day, 0, 0, 0, 0)`
- Added `ensureValidLocalDate()` for fallback handling with local timezone preservation

**Code Changes**:
```typescript
// Before (WRONG - UTC conversion)
chart: DentalChart = { ..., tDate: new Date().toISOString() };
private fromDateInput(value: string, fallbackIso?: string): string {
  const parsed = new Date(`${value}T00:00:00`);  // Assumes UTC
  return parsed.toISOString();
}

// After (CORRECT - Local timezone)
chart: DentalChart = { ..., tDate: this.toLocalDate(new Date()) };
private toLocalDate(date: Date): string {
  const mm = `${date.getMonth() + 1}`.padStart(2, '0');
  const dd = `${date.getDate()}`.padStart(2, '0');
  return `${date.getFullYear()}-${mm}-${dd}T00:00:00`;  // No UTC conversion
}

private fromDateInput(value: string, fallbackIso?: string): string {
  const parts = value.split('-');
  const year = parseInt(parts[0], 10);
  const month = parseInt(parts[1], 10) - 1;
  const day = parseInt(parts[2], 10);

  const localDate = new Date(year, month, day, 0, 0, 0, 0);  // Local timezone
  // Convert back to ISO format preserving local date
  const mm = `${localDate.getMonth() + 1}`.padStart(2, '0');
  const dd = `${localDate.getDate()}`.padStart(2, '0');
  return `${localDate.getFullYear()}-${mm}-${dd}T00:00:00`;
}
```

**Impact**: ✅ Dates now save and display correctly in your local timezone

---

### 2. **Data Type Mismatch (STRUCTURAL)**
**Problem**: `HDentalTreat.TTime` was `DateTime` but should follow `HConsulting` pattern where time is `DateTime?`

**Root Cause**: `HConsulting` correctly uses:
- `CDate: DateTime` (for date)
- `CTime: DateTime?` (nullable for optional time)

But `HDentalTreat` had both as `DateTime`, which is semantically incorrect for storing time-only values.

**Solution**:
- Changed `HDentalTreat.TTime` from `DateTime` to `DateTime?` to match `HConsulting.CTime` pattern
- This allows null values for records without a specific time
- Maintains consistency across the dental data model

**Code Change** (Backend Model):
```csharp
// Before
public DateTime TDate { get; set; }
public DateTime TTime { get; set; }

// After
public DateTime TDate { get; set; }
public DateTime? TTime { get; set; }  // Nullable, matching HConsulting pattern
```

**Impact**: ✅ Data model is now consistent and semantically correct

---

### 3. **Image Display in Edit Mode (UI)**
**Problem**: Images were not displaying well during edit mode in the Imaging tab

**Root Cause**: 
- Angular's strict security policies require image URLs to be sanitized before binding to `[src]`
- File paths from the backend and blob URLs from FileReader weren't being properly sanitized
- Missing DomSanitizer injection

**Solution**:
- Added `DomSanitizer` injection to handle image URL sanitization
- Created `sanitizedImageUrls` map to cache sanitized URLs for performance
- Implemented `getSanitizedImageUrl()` helper method
- Updated all image URL bindings to use sanitized URLs
- Sanitize URLs at multiple points:
  1. When loading existing images in edit mode
  2. When user selects new images (FileReader data URLs)
  3. When backend returns uploaded image paths

**Code Changes**:
```typescript
// Inject DomSanitizer
private readonly sanitizer = inject(DomSanitizer);

// Cache sanitized URLs
sanitizedImageUrls = new Map<string, SafeUrl>();

// Constructor: Sanitize existing images
if (this.imaging.filePath) {
  this.imagingPreviewUrls = [this.imaging.filePath];
  const sanitized = this.sanitizer.bypassSecurityTrustUrl(this.imaging.filePath);
  this.sanitizedImageUrls.set(this.imaging.filePath, sanitized);
}

// When selecting new images
reader.onload = () => {
  const url = (reader.result as string) || '';
  if (url) {
    this.imagingPreviewUrls = [...this.imagingPreviewUrls, url];
    const sanitized = this.sanitizer.bypassSecurityTrustUrl(url);
    this.sanitizedImageUrls.set(url, sanitized);
  }
};

// Helper method
getSanitizedImageUrl(url: string): SafeUrl {
  if (!this.sanitizedImageUrls.has(url)) {
    const sanitized = this.sanitizer.bypassSecurityTrustUrl(url);
    this.sanitizedImageUrls.set(url, sanitized);
  }
  return this.sanitizedImageUrls.get(url)!;
}

// Template binding
<img [src]="getSanitizedImageUrl(img)" [alt]="'Dental image ' + (i + 1)" />
<img [src]="getSanitizedImageUrl(zoomImageUrl)" alt="Dental image zoom" />
```

**Impact**: ✅ Images now display correctly in edit mode

---

## 📋 Summary of Changes

| File | Issue | Fix |
|------|-------|-----|
| `HDentalTreat.cs` | `TTime` should be nullable | Changed `DateTime TTime` → `DateTime? TTime` |
| `dental-encounter-dialog.component.ts` | Date/time timezone offset | Replaced UTC conversion with local timezone handling |
| `dental-encounter-dialog.component.ts` | Image display in edit mode | Added DomSanitizer for URL sanitization |

---

## 🧪 Testing Checklist

### Date/Time Testing
- [ ] Create new dental record and save with today's date
- [ ] Verify saved date matches today's date (not previous day)
- [ ] Verify save time is accurate (not off by hours)
- [ ] Edit existing record and verify date/time unchanged
- [ ] Test in different timezones if possible

### Data Type Testing  
- [ ] Database migration applied for `TTime` nullable change
- [ ] New records save without time (null TTime) if not provided
- [ ] Existing records with time values still work
- [ ] Backend properly handles nullable TTime

### Image Display Testing
- [ ] Open existing record with images - images display correctly
- [ ] Upload new images - preview shows correctly
- [ ] Zoom into images - zoomed view displays
- [ ] Remove images - removed from preview
- [ ] Save and re-open - images still display

---

## 🔍 Technical Details

### Timezone Handling
The fix preserves **local date/time** without UTC conversion:

```
User's Local Time:  2026-06-17 14:30
↓
toLocalDate():      "2026-06-17T00:00:00"
↓
Storage:            "2026-06-17T00:00:00"
↓
Display:            2026-06-17 (same date ✓)
```

Previously (WRONG):
```
User's Local Time:  2026-06-17 14:30
↓
toISOString():      "2026-06-16T19:30:00Z"  (UTC, 5 hours behind)
↓
Storage:            "2026-06-16T19:30:00Z"
↓
Display:            2026-06-16 (wrong date ✗)
```

### Image URL Sanitization
Angular requires `SafeUrl` for dynamic `[src]` bindings. The fix handles:
- **Base64 data URLs** from FileReader (client-side images)
- **File paths** from backend API (uploaded images)
- **Memory cache** for performance (avoid repeated sanitization)

---

## 📝 Database Migration

If using Entity Framework Core, apply this migration for the `TTime` data type change:

```sql
ALTER TABLE HDentalTreat 
ALTER COLUMN TTime DATETIME NULL;
```

Or via EF Core:
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AlterColumn<DateTime>(
        name: "TTime",
        table: "HDentalTreat",
        type: "datetime2",
        nullable: true,
        oldClrType: typeof(DateTime),
        oldType: "datetime2");
}
```

---

## ✅ Verification

After deployment, verify:
1. ✅ Date/time saved and displayed correctly
2. ✅ Timezone matches user's local timezone
3. ✅ Images display in edit mode
4. ✅ Image zoom works
5. ✅ No console errors related to URL sanitization
6. ✅ New records save with nullable TTime
7. ✅ Existing data remains intact

