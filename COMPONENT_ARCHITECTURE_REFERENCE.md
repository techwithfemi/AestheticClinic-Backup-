# Quick Reference: Reusable Components Architecture

## Component Hierarchy

```
┌─────────────────────────────────────────────────────────────────┐
│  Wrapper Page Component (Expenses, Journal, Income)             │
│  - Defines TransactionConfig with page-specific endpoints       │
│  - Passes config to TransactionListComponent                    │
└────────────────────────────┬────────────────────────────────────┘
                             │
                ┌────────────▼─────────────┐
                │ TransactionListComponent │
                │ (Reusable)              │
                │                         │
                │ @Input config           │
                │ - loadData()            │
                │ - openDialog()          │
                │ - deleteTransaction()   │
                └────────────┬────────────┘
                             │
                             │ (Opens via MatDialog)
                             │
                ┌────────────▼──────────────────┐
                │TransactionDialogComponent     │
                │ (Reusable)                   │
                │                              │
                │ MAT_DIALOG_DATA = config     │
                │ - loadLookups()              │
                │ - addOrUpdateGrid()          │
                │ - save()                     │
                └──────────────────────────────┘
```

## Configuration Flow

```typescript
// 1. Wrapper Component Creates Config
const config: TransactionConfig = {
  pageTitle: 'Expenses',
  translateKeyPrefix: 'expenses',
  debitAccountLabel: 'Expense Account',
  creditAccountLabel: 'Paying Account',
  debitAccountsEndpoint: () => this.expenseEndpoint.getExpenseAccounts(),
  creditAccountsEndpoint: () => this.expenseEndpoint.getPayingAccounts(),
  // ... other endpoints
};

// 2. List Component Uses Config
<app-transaction-list [config]="config"></app-transaction-list>

// 3. List Component Passes Config to Dialog
const dialogRef = this.dialog.open(TransactionDialogComponent, {
  data: { entry: null, tranId: '...', isEdit: false, config }
});

// 4. Dialog Component Uses Config
@Component({
  selector: 'app-transaction-dialog'
})
export class TransactionDialogComponent {
  data = inject<TransactionDialogData>(MAT_DIALOG_DATA);
  config = this.data.config;

  ngOnInit() {
    this.config.debitAccountsEndpoint().subscribe(/* ... */);
  }
}
```

## Key Files Location

```
AestheticEMR.client/src/app/features/accounting/
│
├── shared/                          🆕 NEW SHARED FOLDER
│   ├── components/
│   │   ├── transaction-list/        🆕 REUSABLE LIST COMPONENT
│   │   │   ├── transaction-list.component.ts
│   │   │   ├── transaction-list.component.html
│   │   │   └── transaction-list.component.scss
│   │   │
│   │   └── transaction-dialog/      🆕 REUSABLE DIALOG COMPONENT
│   │       ├── transaction-dialog.component.ts
│   │       ├── transaction-dialog.component.html
│   │       └── transaction-dialog.component.scss
│   │
│   └── models/
│       └── transaction-config.interface.ts  🆕 CONFIGURATION INTERFACE
│
├── expenses/                        ⚠️  TO BE UPDATED (Phase 4)
│   ├── expenses.component.ts        (WILL BECOME WRAPPER)
│   └── expense-dialog.component.ts  (CAN BE DEPRECATED)
│
├── journal-entries-info/           ⚠️  WILL BE WRAPPED (Phase 4)
│   └── journal-entries-info.component.ts
│
└── incomes/                        ⚠️  TO BE REPLACED (Phase 4)
    └── incomes.component.ts        (STUB - WILL BECOME WRAPPER)
```

## TransactionConfig Interface

```typescript
export interface TransactionConfig {
  // UI Configuration
  pageTitle: string;                    // "Expenses", "Journal", "Income"
  translateKeyPrefix: string;           // "expenses", "journal", "income"
  debitAccountLabel: string;            // "Expense Account", "Debit Account", etc.
  creditAccountLabel: string;           // "Paying Account", "Credit Account", etc.

  // Dropdown Data Sources
  debitAccountsEndpoint: () => Observable<AccountLookup[]>;
  creditAccountsEndpoint: () => Observable<AccountLookup[]>;

  // List Data Sources
  listEndpoint: (query: TransactionListQuery) => Observable<PagedTransactionResult>;
  nextTranIdEndpoint: () => Observable<TranIdResponse>;
  entriesByTranIdEndpoint: (tranId: string) => Observable<TransactionEntry[]>;

  // Save/Update/Delete Operations
  saveBatchEndpoint: (entries: TransactionEntry[], tranId: string) => Observable<BatchSaveResult>;
  updateByTranIdEndpoint: (tranId: string, entries: TransactionEntry[]) => Observable<BatchSaveResult>;
  deleteTranIdEndpoint: (tranId: string, period: string, coyID: string) => Observable<void>;
}
```

## Translation Key Patterns

All keys use the `config.translateKeyPrefix` to support multiple transaction types:

```
Expenses:
  expenses.PageTitle
  expenses.Subtitle
  expenses.Search
  expenses.Clear
  expenses.SNo
  expenses.TranDate
  expenses.AccountName
  expenses.Debit
  expenses.Credit
  expenses.Description
  expenses.Period
  expenses.Actions
  expenses.AddTransaction
  expenses.NoData
  expenses.EditTransaction
  expenses.NewTransaction
  expenses.EntryHeader
  expenses.TranId
  expenses.Amount
  expenses.SelectDebitAccount
  expenses.SelectCreditAccount
  expenses.UpdateLine
  expenses.AddToGrid
  expenses.ClearLine
  expenses.GridEntries
  expenses.NoGridEntries
  expenses.Cancel
  expenses.Save
  expenses.Saving
  expenses.Required
  expenses.Delete
  expenses.EditLine

Journal (same structure):
  journal.PageTitle
  journal.Subtitle
  journal.Search
  ... etc.

Income (same structure):
  income.PageTitle
  income.Subtitle
  ... etc.
```

## How Components Work Together

### 1. List Component Flow

```
TransactionListComponent
  ├── ngOnInit()
  │   └── loadData()                    # Load transactions via config.listEndpoint()
  ├── openNewDialog()
  │   ├── config.nextTranIdEndpoint()   # Get next transaction ID
  │   └── dialog.open(TransactionDialogComponent, { config, ... })
  ├── openEditDialog(row)
  │   ├── config.entriesByTranIdEndpoint(tranId)  # Load entries for editing
  │   └── dialog.open(TransactionDialogComponent, { config, ... })
  ├── deleteTransaction(row)
  │   └── config.deleteTranIdEndpoint()          # Delete transaction
  └── onPageChange(), onSearch(), etc.
```

### 2. Dialog Component Flow

```
TransactionDialogComponent
  ├── ngOnInit()
  │   ├── config.debitAccountsEndpoint()   # Load debit accounts for dropdown
  │   └── config.creditAccountsEndpoint()  # Load credit accounts for dropdown
  ├── addOrUpdateGrid()                    # Add/edit line in grid
  ├── save()
  │   ├── config.saveBatchEndpoint()       # For new transactions
  │   └── config.updateByTranIdEndpoint()  # For existing transactions
  └── cancel()
```

## Testing Checklist (Phase 4)

- [ ] Expenses page loads with correct title "Expenses"
- [ ] Journal page loads with correct title "Journal"
- [ ] Income page loads with correct title "Income"
- [ ] List shows transactions from correct endpoint
- [ ] Debit accounts dropdown populated correctly
- [ ] Credit accounts dropdown populated correctly
- [ ] Search/filter works
- [ ] Add transaction dialog opens
- [ ] Edit transaction dialog opens with pre-filled data
- [ ] Delete transaction works
- [ ] Save transaction works
- [ ] All translation keys resolve (no missing translations)
- [ ] Responsive design on mobile/tablet/desktop
- [ ] No console errors

## Benefits Summary

| Before (3 separate components) | After (1 reusable + config) |
|--------|---------|
| ~2,700 lines of code (3 × 900) | ~900 lines (reusable) + ~50 lines per config |
| Bug fix in 3 places | Bug fix in 1 place |
| 3 separate dialogs | 1 reusable dialog |
| Copy-paste to add 4th type | Just create config + wrapper |
| Hard-coded endpoint names | Configurable endpoints |
| Fixed dropdown labels | Customizable labels |

---

✅ **Phase 1 & 2 Complete!** Ready for Phase 4: Wrapper Pages & Routing
