# Consent Form Entry UI - Implementation Guide

## Overview

The Consent Form module has been refactored to follow the **Entry Form UI Pattern** as established in the Procedures module. This provides a consistent, user-friendly interface for managing consent forms across the application.

## Architecture

### Components

#### 1. **ConsentFormComponent** (Parent Router Component)
- **Location**: `consent-form.component.ts`
- **Purpose**: Entry point that delegates to the list component
- **Responsibility**: Route-level container

#### 2. **ConsentFormListComponent** (Listing/Worklist Page)
- **Location**: `consent-form-list.component.ts`
- **Purpose**: Display all consent entries in a searchable, paginated table
- **Key Features**:
  - Search by patient name, procedure type, signed by, or witness
  - Pagination (10, 25, 50 items per page)
  - Toggle to show/hide voided entries
  - Refresh button to reload data
  - Add/Edit/Delete action buttons
  - Responsive Material Design (desktop, tablet, mobile)

#### 3. **ConsentFormEntryDialogComponent** (Entry Form Dialog)
- **Location**: `consent-form-entry-dialog.component.ts`
- **Purpose**: Reusable dialog for creating and editing consent forms
- **Key Features**:
  - Patient selection dropdown (shows today's attended patients)
  - Attendance Summary component displays patient info in dialog header
  - Procedure type selection
  - Consent template preview
  - Signature pad with clear button
  - Witnessed by and notes fields
  - Save/Cancel buttons
  - Mobile-responsive signature area
  - Support for both Create and Edit modes

---

## Data Flow

### Create New Consent Form

```
User clicks "Add Consent Form" button
    ↓
ConsentFormListComponent.openAddDialog()
    ↓
ConsentFormEntryDialogComponent opens (modal)
    ↓
User selects patient → Attendance Summary displays
    ↓
User selects procedure type → Template loads
    ↓
User captures signature
    ↓
User clicks "Save"
    ↓
API: signConsentEndpoint()
    ↓
Success: Close dialog, Refresh list
```

### Edit Existing Consent Form

```
User clicks Edit button on table row
    ↓
ConsentFormListComponent.openEditDialog(entry)
    ↓
ConsentFormEntryDialogComponent opens with consentId
    ↓
Dialog loads existing consent data
    ↓
Signature image loads into canvas
    ↓
User modifies fields (optional)
    ↓
User clicks "Update"
    ↓
API: updateSignedConsentEndpoint()
    ↓
Success: Close dialog, Refresh list
```

### Delete (Void) Consent Form

```
User clicks Delete button
    ↓
Confirmation dialog appears
    ↓
User confirms
    ↓
Entry marked "pending void" locally (UI feedback)
    ↓
5-second timer starts
    ↓
API: voidConsentEndpoint() called
    ↓
Success: Entry removed from list

OR

User clicks "Undo" within 5 seconds
    ↓
Timer cancelled, "pending void" flag removed
    ↓
Entry remains in list
```

---

## UI Pattern Features

### ✅ Separation of Concerns

- **List Page**: Manages search, filtering, pagination, and CRUD actions
- **Dialog**: Manages form input, validation, and signature capture
- **No duplicate forms**: Same dialog used for Create and Edit

### ✅ Mobile Responsive

- **Desktop** (>992px): Standard layout with 2-column form sections
- **Tablet** (767px-992px): Single-column layout, full-width controls
- **Mobile** (<767px): Optimized touch targets, full-width buttons, compact spacing
- **Signature area**: Adapts height and maintains aspect ratio on all devices

### ✅ Accessibility

- Material Design icons for visual clarity
- Proper label associations
- Disabled states for form controls
- Clear button labels and tooltips
- ARIA labels on buttons

### ✅ User Feedback

- Loading indicators
- Success/error messages
- Inline validation
- "Pending void" state with undo capability
- Attendance summary shows patient details

---

## Component Usage

### Opening the Consent Form Module

```typescript
// In your routing module
{
  path: 'frontdesk/consent-forms',
  loadComponent: () => import('./features/frontdesk/consent-form/consent-form.component')
    .then(m => m.ConsentFormComponent),
  canActivate: [AuthGuard],
  title: 'Consent Forms'
}
```

### Programmatically Opening a Dialog

```typescript
import { ConsentFormEntryDialogComponent } from './consent-form-entry-dialog.component';

// In your component
constructor(private dialog: MatDialog) {}

// Create new consent
openNewConsent() {
  this.dialog.open(ConsentFormEntryDialogComponent, {
    width: '100%',
    maxWidth: '800px',
    disableClose: true,
    data: {}
  });
}

// Edit existing consent
editConsent(consentId: number) {
  this.dialog.open(ConsentFormEntryDialogComponent, {
    width: '100%',
    maxWidth: '800px',
    disableClose: true,
    data: { consentId }
  });
}
```

---

## Key Technical Details

### Signal-Based State Management

```typescript
// List Component
readonly entries = signal<AestheticSignedConsent[]>([]);
readonly searchText = signal<string>('');
readonly showVoided = signal<boolean>(false);
readonly pageIndex = signal<number>(0);
readonly pageSize = signal<number>(10);

// Computed filtered and paged entries
readonly filteredEntries = computed(() => { /* filtering logic */ });
readonly pagedEntries = computed(() => { /* pagination logic */ });
```

### Reactive Forms

```typescript
// Dialog Component
form = this.fb.nonNullable.group({
  signatureName: ['', Validators.required],
  witnessedBy: [''],
  notes: [''],
  signatureImageBase64: ['', Validators.required]
});

// Validation state
readonly canSave = computed(() => {
  // Check patient, template, signature, form validity
});
```

### Signature Pad Integration

```typescript
// Initialize with device pixel ratio for crisp drawing
// Auto-save on stroke end
// Support loading existing signatures
// Clear button to reset
```

---

## API Integration Points

### Endpoints Used

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `getSignedConsentsEndpoint()` | GET | Fetch consent entries (with optional filters) |
| `signConsentEndpoint()` | POST | Create new signed consent |
| `updateSignedConsentEndpoint()` | PUT | Update existing consent |
| `voidConsentEndpoint()` | DELETE | Void/delete consent |
| `getConsentTemplatesEndpoint()` | GET | Fetch available consent templates |
| `getTodayVisitsEndpoint()` | GET | Get today's patient visits for selection |
| `getHPatientsEndpoint()` | GET | Get patient master data for name resolution |

### Expected Response Models

```typescript
interface AestheticSignedConsent {
  id?: number;
  consultId: string;
  pNo: string;
  procedureType: string;
  consentTemplateId: number;
  signatureName: string;
  witnessedBy?: string;
  notes?: string;
  signatureImageBase64: string;
  signedDate?: string;
  isVoided?: boolean;
  voidReason?: string;
}

interface AestheticConsentTemplate {
  id: number;
  procedureType: string;
  content: string;
  createdDate?: string;
}

interface QryhvisitsForToday {
  consultId: string;
  pNo: string;
  fullname: string;
  recDate: string;
  clinicType: string;
  attndStatus: string;
}
```

---

## Styling & Responsive Behavior

### Breakpoints

- **Desktop**: `> 992px` - Full 2-column layout
- **Tablet**: `768px - 992px` - Single column
- **Mobile**: `< 768px` - Full-width, touch-optimized

### Key CSS Classes

```css
/* Container */
.consent-dialog-container { }
.page-shell { }

/* Forms */
.form-shell { }
.form-section { }
.full-width { }

/* Signature Area */
.signature-pad-wrap { }
.signature-canvas { height: 180px; touch-action: none; }

/* Table */
.table-wrap { overflow-x: auto; }
.badge { }
.badge-signed { background: #e6f4ea; color: #1e7e34; }
.badge-voided { background: #fce8e6; color: #c5221f; }

/* Actions */
.actions-row { display: flex; justify-content: flex-end; gap: 12px; }
```

---

## Error Handling

### User-Facing Errors

All errors are handled via `AlertService`:

```typescript
// Load error
this.alertService.showStickyMessage(
  'Load Error',
  'Unable to load consent entries.',
  MessageSeverity.error,
  error
);

// Validation error
this.alertService.showStickyMessage(
  'Validation Error',
  'Please complete all required fields.',
  MessageSeverity.warn
);

// Success message
this.alertService.showMessage(
  'Success',
  'Consent saved successfully.',
  MessageSeverity.success
);
```

---

## Testing Considerations

### Unit Test Scenarios

1. **List Component**
   - Load entries on init
   - Filter by search text
   - Paginate results
   - Open add/edit dialogs
   - Delete with undo

2. **Dialog Component**
   - Load today's visits
   - Load templates by procedure type
   - Validate form before save
   - Capture and persist signature
   - Edit existing consent

### E2E Test Scenarios

1. Create new consent form from start to finish
2. Edit existing consent form
3. Delete consent with undo
4. Search and filter consent entries
5. Signature capture on touch device

---

## Future Enhancements

- [ ] Bulk actions (export, print, email)
- [ ] Consent template versioning
- [ ] E-signature integration
- [ ] Audit trail / history tracking
- [ ] Template customization UI
- [ ] Signature verification
- [ ] Multi-language support for templates

---

## Migration from Old Component

If you were using the old `consent-form.component.ts`, the new pattern provides:

### Before (Old Pattern)
- Single large component
- Mixed concerns (list + form)
- Limited reusability
- Modal not separated

### After (New Pattern)
- Separated list and dialog components
- Clear responsibilities
- Reusable dialog for Add/Edit
- Better testability
- Consistent with other modules (Procedures, etc.)

---

## Support

For questions or issues with the consent form implementation:

1. Check the component templates for usage examples
2. Review the signal/computed patterns in the code
3. Refer to Material Design documentation for UI components
4. Check API endpoint documentation for data contracts

