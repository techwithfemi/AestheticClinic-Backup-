# Phase 4 Implementation Guide: Wrapper Pages & Routing

This guide shows exactly how to implement Phase 4 using the reusable components.

## Step 1: Update `expenses.component.ts` (Convert to Wrapper)

Replace the entire `expenses.component.ts` with this wrapper:

```typescript
import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import { TransactionConfig } from '../shared/models/transaction-config.interface';
import { ExpenseEndpoint } from '../../../services/expense-endpoint.service';

@Component({
  selector: 'app-expenses',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class ExpensesComponent {
  private expenseEndpoint = inject(ExpenseEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Expenses',
    translateKeyPrefix: 'expenses',
    debitAccountLabel: 'Expense Account',
    creditAccountLabel: 'Paying Account',

    // Dropdown endpoints
    debitAccountsEndpoint: () => this.expenseEndpoint.getExpenseAccountsEndpoint(),
    creditAccountsEndpoint: () => this.expenseEndpoint.getPayingAccountsEndpoint(),

    // List endpoints
    listEndpoint: (query) => this.expenseEndpoint.getExpensesEndpoint(query),
    nextTranIdEndpoint: () => this.expenseEndpoint.getNextTranIdEndpoint(),
    entriesByTranIdEndpoint: (tranId) => this.expenseEndpoint.getExpenseEntriesByTranIdEndpoint(tranId),

    // Save/Update/Delete endpoints
    saveBatchEndpoint: (entries, tranId) => this.expenseEndpoint.getNewExpensesBatchEndpoint(entries, tranId),
    updateByTranIdEndpoint: (tranId, entries) => this.expenseEndpoint.getUpdateExpenseByTranIdEndpoint(tranId, entries),
    deleteTranIdEndpoint: (tranId, period, coyID) => this.expenseEndpoint.getDeleteExpenseByTranIdEndpoint(tranId, period, coyID),
  };
}
```

**Note:** Delete `expense-dialog.component.ts` - no longer needed!

---

## Step 2: Create `journal.component.ts` (New Wrapper)

Create a NEW file to replace the current `JournalEntriesInfoComponent`:

**Path:** `AestheticEMR.client/src/app/features/accounting/journal/journal.component.ts`

```typescript
import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import { TransactionConfig } from '../shared/models/transaction-config.interface';
import { JournalEndpoint } from '../../../services/journal-endpoint.service';

@Component({
  selector: 'app-journal',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class JournalComponent {
  private journalEndpoint = inject(JournalEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Journal Entries',
    translateKeyPrefix: 'journal',
    debitAccountLabel: 'Debit Account',
    creditAccountLabel: 'Credit Account',

    // Dropdown endpoints (replace with actual Journal endpoints)
    debitAccountsEndpoint: () => this.journalEndpoint.getDebitAccountsEndpoint(),
    creditAccountsEndpoint: () => this.journalEndpoint.getCreditAccountsEndpoint(),

    // List endpoints
    listEndpoint: (query) => this.journalEndpoint.getJournalEntriesEndpoint(query),
    nextTranIdEndpoint: () => this.journalEndpoint.getNextTranIdEndpoint(),
    entriesByTranIdEndpoint: (tranId) => this.journalEndpoint.getEntriesByTranIdEndpoint(tranId),

    // Save/Update/Delete endpoints
    saveBatchEndpoint: (entries, tranId) => this.journalEndpoint.getSaveJournalBatchEndpoint(entries, tranId),
    updateByTranIdEndpoint: (tranId, entries) => this.journalEndpoint.getUpdateJournalByTranIdEndpoint(tranId, entries),
    deleteTranIdEndpoint: (tranId, period, coyID) => this.journalEndpoint.getDeleteJournalByTranIdEndpoint(tranId, period, coyID),
  };
}
```

**Note:** Rename current `journal-entries-info` folder to `journal` or keep both during transition.

---

## Step 3: Update `incomes.component.ts` (Convert Stub to Wrapper)

Replace the stub with:

```typescript
import { Component, inject } from '@angular/core';
import { TransactionListComponent } from '../shared/components/transaction-list/transaction-list.component';
import { TransactionConfig } from '../shared/models/transaction-config.interface';
import { IncomeEndpoint } from '../../../services/income-endpoint.service';

@Component({
  selector: 'app-incomes',
  standalone: true,
  imports: [TransactionListComponent],
  template: `<app-transaction-list [config]="config"></app-transaction-list>`
})
export class IncomesComponent {
  private incomeEndpoint = inject(IncomeEndpoint);

  config: TransactionConfig = {
    pageTitle: 'Income',
    translateKeyPrefix: 'income',
    debitAccountLabel: 'Income Bank Account',
    creditAccountLabel: 'Income Account',

    // Dropdown endpoints
    debitAccountsEndpoint: () => this.incomeEndpoint.getIncomeBankAccountsEndpoint(),
    creditAccountsEndpoint: () => this.incomeEndpoint.getIncomeAccountsEndpoint(),

    // List endpoints
    listEndpoint: (query) => this.incomeEndpoint.getIncomeEndpoint(query),
    nextTranIdEndpoint: () => this.incomeEndpoint.getNextTranIdEndpoint(),
    entriesByTranIdEndpoint: (tranId) => this.incomeEndpoint.getIncomeEntriesByTranIdEndpoint(tranId),

    // Save/Update/Delete endpoints
    saveBatchEndpoint: (entries, tranId) => this.incomeEndpoint.getNewIncomeBatchEndpoint(entries, tranId),
    updateByTranIdEndpoint: (tranId, entries) => this.incomeEndpoint.getUpdateIncomeByTranIdEndpoint(tranId, entries),
    deleteTranIdEndpoint: (tranId, period, coyID) => this.incomeEndpoint.getDeleteIncomeByTranIdEndpoint(tranId, period, coyID),
  };
}
```

---

## Step 4: Update `accounting.routes.ts`

```typescript
import { Routes } from '@angular/router';

export const accountingRoutes: Routes = [
  {
    path: '',
    redirectTo: 'journal',
    pathMatch: 'full'
  },
  {
    path: 'journal',
    loadComponent: () => import('./journal/journal.component')
      .then(m => m.JournalComponent),
    title: 'Journal Entries'
  },
  {
    path: 'chart-of-accounts',
    loadComponent: () => import('./chart-of-accounts/chart-of-accounts.component')
      .then(m => m.ChartOfAccountsComponent),
    title: 'Chart of Accounts'
  },
  {
    path: 'expenses',
    loadComponent: () => import('./expenses/expenses.component')
      .then(m => m.ExpensesComponent),
    title: 'Expenses'
  },
  {
    path: 'incomes',
    loadComponent: () => import('./incomes/incomes.component')
      .then(m => m.IncomesComponent),
    title: 'Income'
  }
];
```

---

## Step 5: Add Translation Keys

Add these keys to all locale files:

### `en.json`
```json
{
  "journal": {
    "PageTitle": "Journal Entries",
    "Subtitle": "Record and manage journal entries",
    "Search": "Search by Transaction ID or Account",
    "Clear": "Clear",
    "SNo": "S.No",
    "TranDate": "Date",
    "AccountName": "Account Name",
    "Debit": "Debit",
    "Credit": "Credit",
    "Description": "Description",
    "Period": "Period",
    "Actions": "Actions",
    "AddTransaction": "New Journal Entry",
    "NoData": "No journal entries found",
    "EditTransaction": "Edit Journal Entry",
    "NewTransaction": "New Journal Entry",
    "EntryHeader": "Entry Details",
    "TranId": "Transaction ID",
    "Amount": "Amount",
    "SelectDebitAccount": "Select debit account",
    "SelectCreditAccount": "Select credit account",
    "UpdateLine": "Update Line",
    "AddToGrid": "Add to Grid",
    "ClearLine": "Clear Line",
    "GridEntries": "Entry Lines",
    "NoGridEntries": "No entries added yet",
    "Cancel": "Cancel",
    "Save": "Save",
    "Saving": "Saving...",
    "Required": "Required",
    "Delete": "Delete",
    "EditLine": "Edit"
  },
  "income": {
    "PageTitle": "Income",
    "Subtitle": "Record and manage income transactions",
    "Search": "Search by Transaction ID or Account",
    "Clear": "Clear",
    "SNo": "S.No",
    "TranDate": "Date",
    "AccountName": "Account Name",
    "Debit": "Debit",
    "Credit": "Credit",
    "Description": "Description",
    "Period": "Period",
    "Actions": "Actions",
    "AddTransaction": "New Income Entry",
    "NoData": "No income entries found",
    "EditTransaction": "Edit Income Entry",
    "NewTransaction": "New Income Entry",
    "EntryHeader": "Entry Details",
    "TranId": "Transaction ID",
    "Amount": "Amount",
    "SelectDebitAccount": "Select income bank account",
    "SelectCreditAccount": "Select income account",
    "UpdateLine": "Update Line",
    "AddToGrid": "Add to Grid",
    "ClearLine": "Clear Line",
    "GridEntries": "Entry Lines",
    "NoGridEntries": "No entries added yet",
    "Cancel": "Cancel",
    "Save": "Save",
    "Saving": "Saving...",
    "Required": "Required",
    "Delete": "Delete",
    "EditLine": "Edit"
  },
  "expenses": {
    "PageTitle": "Expenses",
    "Subtitle": "Record and manage expense transactions",
    "Search": "Search by Transaction ID or Account",
    "Clear": "Clear",
    "SNo": "S.No",
    "TranDate": "Date",
    "AccountName": "Account Name",
    "Debit": "Debit",
    "Credit": "Credit",
    "Description": "Description",
    "Period": "Period",
    "Actions": "Actions",
    "AddTransaction": "New Expense Entry",
    "NoData": "No expense entries found",
    "EditTransaction": "Edit Expense Entry",
    "NewTransaction": "New Expense Entry",
    "EntryHeader": "Entry Details",
    "TranId": "Transaction ID",
    "Amount": "Amount",
    "SelectDebitAccount": "Select expense account",
    "SelectCreditAccount": "Select paying account",
    "UpdateLine": "Update Line",
    "AddToGrid": "Add to Grid",
    "ClearLine": "Clear Line",
    "GridEntries": "Entry Lines",
    "NoGridEntries": "No entries added yet",
    "Cancel": "Cancel",
    "Save": "Save",
    "Saving": "Saving...",
    "Required": "Required",
    "Delete": "Delete",
    "EditLine": "Edit"
  }
}
```

---

## Step 6: Verify Endpoints

Make sure these services exist or create them:

- `ExpenseEndpoint` - ✅ ALREADY EXISTS
- `IncomeEndpoint` - ⚠️ NEEDS TO BE CREATED OR UPDATED
- `JournalEndpoint` - ⚠️ NEEDS TO BE UPDATED (currently has different method names)

---

## Step 7: Clean Up (Optional but Recommended)

After all tests pass:

1. **Delete old components:**
   - `expense-dialog.component.ts`
   - `journal-entries-info/` folder (if replaced with journal wrapper)

2. **Keep for reference (Phase 5 if needed):**
   - Old `ExpensesComponent` code (in case rollback needed)

---

## Testing Checklist for Phase 4

Run this after implementing Phase 4:

- [ ] **Build succeeds** - `npm run build` (or equivalent)
- [ ] **Expenses page loads** - Navigate to `/accounting/expenses`
- [ ] **Journal page loads** - Navigate to `/accounting/journal`
- [ ] **Income page loads** - Navigate to `/accounting/incomes`
- [ ] **List shows data** - Each page displays transactions
- [ ] **Search works** - Can search by transaction ID
- [ ] **Add dialog opens** - Click "New" button
- [ ] **Dialog dropdowns populated** - Debit and Credit accounts visible
- [ ] **Edit works** - Click edit icon, dialog opens with data
- [ ] **Delete works** - Click delete, confirmation dialog shows
- [ ] **Save works** - Can save new transaction
- [ ] **Translation keys resolve** - No "Missing translation" messages
- [ ] **Responsive** - Page works on mobile/tablet/desktop
- [ ] **No console errors** - Browser console clean

---

## Commands to Run

```bash
# After making changes:
npm run build                    # Build Angular app
npm run start                    # Start dev server
npm run lint                     # Lint checks

# Navigate to pages:
# http://localhost:4200/accounting/journal
# http://localhost:4200/accounting/expenses
# http://localhost:4200/accounting/incomes
```

---

## What Each Wrapper Does

| Component | Page Title | Debit Label | Credit Label | Endpoints Source |
|-----------|-----------|------------|-------------|------------------|
| `ExpensesComponent` | Expenses | Expense Account | Paying Account | `ExpenseEndpoint` |
| `JournalComponent` | Journal Entries | Debit Account | Credit Account | `JournalEndpoint` |
| `IncomesComponent` | Income | Income Bank Account | Income Account | `IncomeEndpoint` |

All wrappers use the same reusable:
- `TransactionListComponent` - for the list/table
- `TransactionDialogComponent` - for the add/edit dialog

✅ **Ready to implement Phase 4!**
