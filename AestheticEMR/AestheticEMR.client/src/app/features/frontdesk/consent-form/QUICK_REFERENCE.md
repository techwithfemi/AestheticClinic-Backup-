# Consent Form - Quick Reference Guide

## 🚀 Quick Start

### View Consent Forms
Navigate to: `/frontdesk/consent-forms`

The page displays:
- List of all consent entries
- Search by patient, procedure, or signed by
- Add/Edit/Delete buttons

### Add New Consent Form
1. Click **"Add Consent Form"** button
2. Select patient from dropdown (today's visits only)
3. Check **Attendance Summary** in header
4. Select procedure type
5. Review consent template
6. Enter signature name
7. (Optional) Add witness name and notes
8. **Draw signature** on pad
9. Click **"Save"**

### Edit Existing Consent
1. Find entry in table
2. Click **"Edit"** icon button
3. Modify fields as needed
4. Update signature if needed
5. Click **"Update"**

### Delete Consent
1. Click **"Delete"** icon button
2. Confirm in dialog
3. Entry marked "Pending void"
4. **5 seconds to undo** - click "Undo" button
5. After 5 seconds, consent is voided

### Search & Filter
- Use search box: search patient name, procedure, signed by, or witness
- Toggle "Show voided" to include/exclude voided entries
- Change pagination size: 10, 25, or 50 per page

---

## 📁 File Structure

```
AestheticEMR/AestheticEMR.client/src/app/features/frontdesk/consent-form/
├── consent-form.component.ts                   ← Router (entry point)
├── consent-form-list.component.ts              ← List page (worklist)
├── consent-form-entry-dialog.component.ts      ← Add/Edit dialog (form)
├── consent-template-manager.component.ts       ← Template management
├── CONSENT_FORM_PATTERN.md                     ← Architecture docs
├── IMPLEMENTATION_SUMMARY.md                   ← Full summary
└── BEFORE_AFTER_COMPARISON.md                  ← Before/after comparison
```

---

## 🎯 Component Purposes

### ConsentFormComponent
- **Type**: Route entry point
- **Purpose**: Simple router to list component
- **Lines**: ~10
- **Use**: In routing module only

### ConsentFormListComponent
- **Type**: Smart component (listing page)
- **Purpose**: Display, search, paginate consent entries
- **Lines**: ~300
- **Use**: Main page for users

### ConsentFormEntryDialogComponent
- **Type**: Reusable dialog component
- **Purpose**: Form for creating/editing consent
- **Lines**: ~400
- **Use**: Opened by list component via `MatDialog.open()`

---

## 💾 State Management (Signals)

### List Component
```typescript
readonly entries = signal<AestheticSignedConsent[]>([]);
readonly patients = signal<HPatient[]>([]);
readonly searchText = signal<string>('');
readonly showVoided = signal<boolean>(false);
readonly pageIndex = signal<number>(0);
readonly pageSize = signal<number>(10);

readonly filteredEntries = computed(() => { /* filtered by search/voided */ });
readonly pagedEntries = computed(() => { /* sliced for current page */ });
```

### Dialog Component
```typescript
readonly selectedConsultId = signal<string>('');
readonly selectedProcedureType = signal<string>('Procedures');
readonly existingConsent = signal<AestheticSignedConsent | null>(null);
readonly todayVisits = signal<QryhvisitsForToday[]>([]);
readonly templates = signal<AestheticConsentTemplate[]>([]);

readonly selectedAttendance = computed(() => { /* find selected visit */ });
readonly activeTemplate = computed(() => { /* resolve template */ });
readonly canSave = computed(() => { /* validation */ });
```

---

## 📱 Responsive Breakpoints

| Device | Width | Layout | Signature |
|--------|-------|--------|-----------|
| Mobile | <768px | Single column | 160px |
| Tablet | 768-992px | Single column | 180px |
| Desktop | >992px | 2-column form | 180px |

---

## 🔗 API Endpoints Used

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `getSignedConsentsEndpoint()` | GET | Fetch consent entries |
| `signConsentEndpoint()` | POST | Create new consent |
| `updateSignedConsentEndpoint()` | PUT | Update existing |
| `voidConsentEndpoint()` | DELETE | Delete/void consent |
| `getConsentTemplatesEndpoint()` | GET | Get templates |
| `getTodayVisitsEndpoint()` | GET | Get today's patients |
| `getHPatientsEndpoint()` | GET | Get patient master data |

---

## 🎨 Key CSS Classes

### Containers
```css
.page-shell              /* Main page wrapper */
.consent-dialog-container /* Dialog wrapper */
.filters-header          /* Filter bar */
.table-card              /* Table card */
```

### Forms
```css
.form-shell              /* Form wrapper */
.form-section            /* Form section */
.full-width              /* Full width field */
.signature-pad-wrap      /* Signature area */
.signature-canvas        /* Canvas element */
```

### Tables
```css
.table-wrap              /* Table wrapper */
.badge                   /* Status badge */
.badge-signed            /* Green badge */
.badge-voided            /* Red badge */
```

---

## ⚙️ Form Validation

```typescript
form = this.fb.nonNullable.group({
  signatureName: ['', Validators.required],      // REQUIRED
  witnessedBy: [''],                              // Optional
  notes: [''],                                    // Optional
  signatureImageBase64: ['', Validators.required] // REQUIRED
});
```

**Save enabled when:**
- Patient selected ✅
- Template available ✅
- Signature captured ✅
- Form valid ✅
- Not loading ✅

---

## 🔔 User Feedback

### Success Messages
```
"Consent saved successfully."
"Consent updated successfully."
"Consent voided."
```

### Warning Messages
```
"Validation Error - Please complete all required fields."
"Pending void - Consent will be voided in 5 seconds."
```

### Error Messages
```
"Unable to load consent entries."
"Unable to save consent."
"Unable to update consent."
"Unable to void consent."
```

---

## 🚀 Opening Dialog Programmatically

### From Another Component
```typescript
import { ConsentFormEntryDialogComponent } from '.../consent-form-entry-dialog.component';
import { MatDialog } from '@angular/material/dialog';

constructor(private dialog: MatDialog) {}

// Create new
openNewConsent() {
  const dialogRef = this.dialog.open(ConsentFormEntryDialogComponent, {
    width: '100%',
    maxWidth: '800px',
    disableClose: true,
    data: {}
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      // Consent saved, refresh your data
      this.loadConsents();
    }
  });
}

// Edit existing
editConsent(consentId: number) {
  const dialogRef = this.dialog.open(ConsentFormEntryDialogComponent, {
    width: '100%',
    maxWidth: '800px',
    disableClose: true,
    data: { consentId }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.loadConsents();
    }
  });
}
```

---

## 🧪 Testing Common Scenarios

### Test Add Workflow
1. ✅ Click Add button
2. ✅ Dialog opens
3. ✅ Select patient
4. ✅ Attendance summary appears
5. ✅ Select procedure type
6. ✅ Template loads
7. ✅ Draw signature
8. ✅ Click Save
9. ✅ Dialog closes
10. ✅ Table refreshes

### Test Edit Workflow
1. ✅ Click Edit on row
2. ✅ Dialog opens
3. ✅ Data prefilled
4. ✅ Signature image loads
5. ✅ Modify field
6. ✅ Click Update
7. ✅ Dialog closes
8. ✅ Table updates

### Test Delete Workflow
1. ✅ Click Delete on row
2. ✅ Confirm dialog
3. ✅ Entry shows "Pending void"
4. ✅ Wait 5 seconds OR click Undo
5. ✅ Entry removed or stays

### Test Search Workflow
1. ✅ Type in search box
2. ✅ Table filters in real-time
3. ✅ Works for: patient, procedure, signed by, witness
4. ✅ Pagination resets
5. ✅ Clear search shows all

### Test Mobile Workflow
1. ✅ Open on mobile device
2. ✅ Buttons are 44px height
3. ✅ Full-width inputs
4. ✅ Signature pad: 160px
5. ✅ Table scrolls horizontally
6. ✅ Can add/edit/delete

---

## 🔍 Debugging Tips

### Check Current Route
```
URL should be: /frontdesk/consent-forms
```

### Inspect Console for Errors
```javascript
// Check for API errors
// Look for validation messages
// Check loading indicators
```

### Verify Data Loading
```typescript
// In browser console
// Check if patients array populated
// Check if templates array populated
// Check if entries array populated
```

### Test Signature Capture
```
1. Click on signature pad
2. Try to draw
3. Should see stroke appear
4. Check "Clear" button works
5. Save should be enabled
```

---

## 📞 Common Issues & Solutions

### Issue: Dialog won't open
**Solution**: Check MatDialog is imported, disableClose is set to true

### Issue: Signature won't save
**Solution**: Ensure at least one stroke drawn, then try clearing/redrawing

### Issue: Can't find patient
**Solution**: Patients from today's attendance only, check date

### Issue: Template not loading
**Solution**: Check template exists and procedure type matches

### Issue: Mobile layout broken
**Solution**: Check browser zoom is 100%, try different device

### Issue: Table doesn't refresh after save
**Solution**: Dialog should call loadEntries() on close

---

## 🎓 Learning Resources

See documentation files for detailed info:
- `CONSENT_FORM_PATTERN.md` - Full architecture guide
- `IMPLEMENTATION_SUMMARY.md` - Complete feature list
- `BEFORE_AFTER_COMPARISON.md` - Why changes were made

---

## ✨ Version History

### v1.0.0 - Initial Implementation
- ✅ Separated list and dialog components
- ✅ Full CRUD operations
- ✅ Mobile responsive design
- ✅ Material Design UI
- ✅ Attendance integration
- ✅ Signature capture

---

## 🎯 Key Takeaways

1. **List Page** - Search, filter, paginate, CRUD buttons
2. **Dialog Form** - Patient selection, signature capture, save
3. **Reusable** - Same dialog for add and edit
4. **Responsive** - Works on all devices
5. **Material Design** - Modern, accessible UI
6. **Clear Architecture** - Easy to maintain and extend

---

## 🚀 Next Steps

- Deploy to production
- Monitor user feedback
- Add telemetry/analytics if needed
- Consider future enhancements (templates management, audit trail, etc.)

---

**Ready to use! Start managing consent forms with the new UI pattern.** 🎉
