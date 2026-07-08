# Visual Architecture Diagram

## High-Level Component Tree (After Phase 1-2)

```
┌─────────────────────────────────────────────────────────────────┐
│  ACCOUNTING MODULE (accounting.routes.ts)                       │
│                                                                 │
│  /accounting/expenses      → ExpensesComponent (wrapper)        │
│  /accounting/journal       → JournalComponent (wrapper)         │
│  /accounting/incomes       → IncomesComponent (wrapper)         │
│  /accounting/chart-of-...  → ChartOfAccountsComponent           │
└──────────────────┬──────────────────────────────────────────────┘
                   │
       ┌───────────┼───────────┐
       │           │           │
       ▼           ▼           ▼
   ┌─────────┐ ┌─────────┐ ┌─────────┐
   │Expenses │ │ Journal │ │ Incomes │
   │Wrapper  │ │Wrapper  │ │Wrapper  │
   └────┬────┘ └────┬────┘ └────┬────┘
        │           │           │
        └───────────┼───────────┘
                    │
         ┌──────────▼──────────┐
         │ TransactionList     │  ◄─── REUSABLE
         │ Component           │
         │                     │
         │ @Input config       │
         │ - loadData()        │
         │ - openDialog()      │
         │ - deleteTransaction │
         └────────────┬────────┘
                      │
                      │ (Opens via MatDialog)
                      │
         ┌────────────▼──────────────┐
         │ TransactionDialog         │  ◄─── REUSABLE
         │ Component                 │
         │                           │
         │ MAT_DIALOG_DATA = config  │
         │ - loadLookups()           │
         │ - addOrUpdateGrid()       │
         │ - save()                  │
         └───────────────────────────┘
```

---

## Configuration Flow Diagram

```
┌──────────────────────────────────────┐
│  Wrapper Component (e.g., Expenses)  │
│  ┌──────────────────────────────────┐│
│  │ Creates TransactionConfig object ││
│  │                                  ││
│  │ config = {                       ││
│  │   pageTitle: 'Expenses',         ││
│  │   translateKeyPrefix: 'expenses',││
│  │   debitAccountLabel: '...',      ││
│  │   creditAccountLabel: '...',     ││
│  │   debitAccounts$: endpoint(),    ││
│  │   creditAccounts$: endpoint(),   ││
│  │   list$: endpoint(query),        ││
│  │   ... more endpoints             ││
│  │ }                                ││
│  └──────────┬───────────────────────┘│
└─────────────┼────────────────────────┘
              │
              │ [config]
              ▼
┌──────────────────────────────────────┐
│  TransactionListComponent            │
│  @Input config                       │
│  ┌──────────────────────────────────┐│
│  │ Uses config to:                  ││
│  │ 1. Load data from config.list$() ││
│  │ 2. Display title from config     ││
│  │ 3. Use translation prefix        ││
│  │ 4. Opens dialog with config      ││
│  └──────────┬───────────────────────┘│
└─────────────┼────────────────────────┘
              │
              │ dialog.open(Dialog, {data: {config}})
              ▼
┌──────────────────────────────────────┐
│  TransactionDialogComponent          │
│  MAT_DIALOG_DATA = {config, ...}     │
│  ┌──────────────────────────────────┐│
│  │ Uses config to:                  ││
│  │ 1. Load debit from config        ││
│  │ 2. Load credit from config       ││
│  │ 3. Use debit/credit labels       ││
│  │ 4. Save via config endpoint      ││
│  └──────────────────────────────────┘│
└──────────────────────────────────────┘
```

---

## Data Flow Example: New Expense Entry

```
USER INTERACTION                    COMPONENT STATE                 DATA FLOW
─────────────────────────────────────────────────────────────────────────────

1. User clicks "Add"
                        ┌─ List loads next TranID ──── config.nextTranIdEndpoint()
                        │
2. List opens dialog    ├─ Dialog receives config
                        │
                        ├─ Dialog loads dropdowns ──── config.debitAccountsEndpoint()
                        │                            config.creditAccountsEndpoint()
3. User selects 
   accounts
                        └─ Dialog is ready

4. User enters fields
   (date, amount, desc)

5. User clicks "Add" 
   in form              ┌─ Dialog validates form
                        │
6. Form gets added
   to grid              ├─ Grid updates (client-side)
                        │
7. User clicks "Save"   │
                        └─ Dialog calls save endpoint ─ config.saveBatchEndpoint()

8. Save succeeds        ┌─ Dialog closes
                        │
                        ├─ List refreshes ─────────── config.listEndpoint(query)
                        │
9. User sees new
   entry in list        └─ New entry appears in table
```

---

## File Organization Diagram

```
AestheticEMR.client/src/app/features/accounting/
│
├── shared/                          ◄─── NEW SHARED FOLDER
│   ├── components/
│   │   ├── transaction-list/        ◄─── REUSABLE COMPONENT
│   │   │   ├── transaction-list.component.ts
│   │   │   ├── transaction-list.component.html
│   │   │   └── transaction-list.component.scss
│   │   │
│   │   └── transaction-dialog/      ◄─── REUSABLE COMPONENT
│   │       ├── transaction-dialog.component.ts
│   │       ├── transaction-dialog.component.html
│   │       └── transaction-dialog.component.scss
│   │
│   └── models/
│       └── transaction-config.interface.ts  ◄─── CONFIGURATION INTERFACE
│
├── expenses/                        ◄─── WILL USE REUSABLE COMPONENTS
│   ├── expenses.component.ts        ┌─ Wrapper (20 lines)
│   └── expense-dialog.component.ts  └─ Deprecated in Phase 4
│
├── journal/                         ◄─── NEW (Phase 4)
│   └── journal.component.ts         ┌─ Wrapper (20 lines)
│
├── chart-of-accounts/               ◄─── UNCHANGED
│   ├── chart-of-account-dialog.component.ts
│   └── chart-of-accounts.component.ts
│
├── incomes/                         ◄─── WILL USE REUSABLE COMPONENTS
│   └── incomes.component.ts         ┌─ Wrapper (20 lines, replaces stub)
│
└── accounting.routes.ts             ◄─── TO UPDATE (Phase 4)
```

---

## Dependency Injection Diagram

```
┌─────────────────────────────────────┐
│  ExpensesComponent (Wrapper)        │
│                                     │
│  inject(ExpenseEndpoint) ───────┐   │
└─────────────────────────────────┼───┘
                                  │
                    ┌─────────────▼──────┐
                    │ Creates config    │
                    │ with endpoints    │
                    └─────────────┬──────┘
                                  │
           ┌──────────────────────▼────────────────────┐
           │ TransactionListComponent                 │
           │                                          │
           │ @Input config: TransactionConfig         │
           │ inject(AlertService)                     │
           │ inject(AccountService)                   │
           │ inject(MatDialog)                        │
           │ inject(AppConfigService)                 │
           │                                          │
           └──────────────────────┬───────────────────┘
                                  │
                    ┌─────────────▼──────┐
                    │  Dialog opens      │
                    │  with config       │
                    └─────────────┬──────┘
                                  │
                ┌─────────────────▼──────────────────┐
                │ TransactionDialogComponent         │
                │                                   │
                │ MAT_DIALOG_DATA = config          │
                │ inject(AlertService)              │
                │ inject(MatDialogRef)              │
                │                                   │
                │ Uses config.debitAccountsEndpoint │
                │ Uses config.creditAccountsEndpoint│
                │ Uses config.saveBatchEndpoint     │
                │ Uses config.updateByTranIdEndpoint│
                │ Uses config.deleteTranIdEndpoint  │
                │                                   │
                └───────────────────────────────────┘
```

**Key Point:** Services are injected in wrapper, endpoints passed via config to reusables.

---

## Translation Key Pattern Diagram

```
TransactionConfig.translateKeyPrefix = "expenses"
                       │
        ┌──────────────┼──────────────┐
        │              │              │
        ▼              ▼              ▼

   expenses.        expenses.        expenses.
   PageTitle        TranDate         Description

   ◄─ Resolved via translate pipe

   {{ (config.translateKeyPrefix + '.PageTitle') | translate }}

   ◄─ Dynamic key generation allows same component for all 3 transaction types


Translation Files:
  src/locale/en.json
    └─ expenses: { PageTitle, TranDate, ... }
    └─ journal: { PageTitle, TranDate, ... }
    └─ income: { PageTitle, TranDate, ... }
```

---

## Phase 1-2 vs Phase 4 Comparison

```
BEFORE (Phase 0)                    AFTER PHASE 1-2              AFTER PHASE 4
────────────────────────────────────────────────────────────────────────────

ExpensesComponent                   TransactionListComponent     ExpensesComponent
  (full logic)                        (reusable)                   (wrapper 20 lines)
                                                                    └─ uses config
ExpenseDialogComponent              TransactionDialogComponent   
  (full logic)                        (reusable)                   (no longer needed)

JournalEntriesInfoComponent         TransactionConfig interface  JournalComponent
  (similar logic)                     (enables reuse)              (wrapper 20 lines)
                                                                    └─ uses config

IncomesComponent                    [READY FOR PHASE 4]          IncomesComponent
  (stub - Coming Soon)                                            (wrapper 20 lines)
                                                                    └─ uses config


CODE STATS:
  Before: 2,700 lines (3 × 900)
  Phase 1-2: 900 lines (reusable) + 165 lines (config)
  Phase 4: + 60 lines (3 × 20 wrapper lines)
  Final: ~1,100 lines total (59% reduction)
```

---

## Error Handling Flow

```
User Action (e.g., Save)
         │
         ▼
Component calls config.saveBatchEndpoint()
         │
    ┌────┴────┐
    │          │
    ▼          ▼
 Success      Error
    │          │
    └──────┬───┘
           │
    ┌──────▼──────┐
    │ AlertService│
    │             │
    ├─ Success: showMessage()
    ├─ Error: showStickyMessage()
    └─ Loading: startLoadingMessage() / stopLoadingMessage()


Error Message:
  1. Extract from error.error.errors[] (validation)
  2. Fall back to error.error.title
  3. Fall back to error.message
  4. Final fallback: 'Unknown error'
```

---

## Responsive Design Breakpoints

```
DESKTOP (≥ 992px)              TABLET (768-991px)         MOBILE (< 768px)
┌─────────────────────────┐   ┌──────────────────┐       ┌─────────────┐
│  Full Width             │   │ Reduced Padding  │       │ Full Screen │
│  2 Column Form Grid     │   │ Single Column    │       │ Stack All   │
│  Sticky Header          │   │ 1x Paginator     │       │ No Sticky   │
│  Normal Font Sizes      │   │ Smaller Icons    │       │ Tiny Fonts  │
│  Padding: 20px          │   │ Padding: 16px    │       │ Padding:12px│
└─────────────────────────┘   └──────────────────┘       └─────────────┘


Dialog Sizes:
  Desktop: 1100px width (max 95vw)
  Tablet: Scales down via max-width
  Mobile: Full width with scrolling
```

---

## Phase Progression Visual

```
                        PHASE 1-2 ✅ COMPLETE
                              │
                              │
              ┌────────────────┤────────────────┐
              │                │                │
        Created             Created          Created
       Components          Interface         Docs
              │                │                │
     ┌─ transaction-list  ┌─ config       ┌─ 4 docs
     ├─ transaction-      │  interface    │  (guides &
     └─ dialog            └─ (flexible)   │  reference)
                                          │
                                          ▼
                                    Ready for
                                     PHASE 4
                                          │
                    ┌─────────────────────┤─────────────────────┐
                    │                     │                     │
              Create Wrapper         Update Routes         Add Translations
              Components             & Navigation          & Test
                    │                     │                     │
              3 × 20 lines          routing updates       All locale files
              (expenses/             (new paths)          (en, fr, de, es...)
              journal/income)                             
                                         ▼
                                    PHASE 4 COMPLETE

                              ┌──────────────────┐
                              │  3 Transaction   │
                              │  Types Working   │
                              │  w/ Reusable     │
                              │  Components ✅   │
                              └──────────────────┘
```

---

## Success Indicators

```
✅ PHASE 1-2 SUCCESS
   • Build passes
   • No TypeScript errors
   • Components import correctly
   • All tests pass

✅ PHASE 4 SUCCESS
   • Expenses page loads
   • Journal page loads
   • Income page loads
   • Data shows correctly
   • Add/Edit/Delete work
   • Translations resolve
   • No console errors
   • Responsive on all devices
```

---

# Architecture Complete!

**Next:** Proceed to Phase 4 Implementation
