import { Observable } from 'rxjs';

/**
 * Configuration interface for reusable transaction list and dialog components.
 * Used by Expenses, Income, and Journal pages to customize behavior while sharing code.
 */
export interface TransactionConfig {
  // UI Labels & Titles
  pageTitle: string;                    // e.g., 'Expenses', 'Income', 'Journal'
  translateKeyPrefix: string;           // e.g., 'expenses', 'income', 'journal'

  // Dropdown Labels
  debitAccountLabel: string;            // e.g., 'Expense Account', 'Income Account', 'Debit Account'
  creditAccountLabel: string;           // e.g., 'Paying Account', 'Income Account', 'Credit Account'

  // Data Loading Endpoints
  // If debitAccountsEndpoint is not provided, uses allAccountsEndpoint instead
  debitAccountsEndpoint?: () => Observable<AccountLookup[]>;
  // If creditAccountsEndpoint is not provided, uses allAccountsEndpoint instead
  creditAccountsEndpoint?: () => Observable<AccountLookup[]>;
  // Fallback endpoint when specific debit/credit endpoints are not provided
  // Loads entire chart of Accounts from vwAccountsInfoCombo
  allAccountsEndpoint?: () => Observable<AccountLookup[]>;

  listEndpoint: (query: TransactionListQuery) => Observable<PagedTransactionResult>;
  nextTranIdEndpoint: () => Observable<TranIdResponse>;
  entriesByTranIdEndpoint: (tranId: string) => Observable<TransactionEntry[]>;

  saveBatchEndpoint: (entries: TransactionEntry[], tranId: string) => Observable<BatchSaveResult>;
  updateByTranIdEndpoint: (tranId: string, entries: TransactionEntry[]) => Observable<BatchSaveResult>;
  deleteTranIdEndpoint: (tranId: string, period: string, coyID: string) => Observable<void>;
}

/**
 * Account lookup for dropdowns
 */
export interface AccountLookup {
  accountNo: string;
  accountName: string;
}

/**
 * Single transaction entry (represents a row in the dialog grid)
 */
export interface TransactionEntry {
  sNo?: number | null;
  tranDate: string | Date;
  accountDebit: string;
  accountCredit: string;
  debitAccountName?: string | null;
  creditAccountName?: string | null;
  amount: number;
  description: string;
  isPost: boolean;
  isClose: boolean;
  userName?: string | null;
  tranId?: string | null;
  period?: string | null;
  coyID?: string | null;
  remarks?: string | null;
}

/**
 * List view item (represents a row in the main list/table)
 */
export interface TransactionListItem {
  sn: number;
  tranDate: string;
  accountName: string;
  accountNo: string;
  debit: number;
  credit: number;
  description?: string | null;
  tranNo: string;
  tranCat?: string | null;
  billNo?: string | null;
  costCenter?: string | null;
  entryDate: string;
  period: string;
  userName?: string | null;
  sNo: number;
  remarks?: string | null;
  coyID?: string | null;
  isClose: boolean;
}

/**
 * Query parameters for list endpoint
 */
export interface TransactionListQuery {
  search?: string | null;
  fromDate?: string | null;
  toDate?: string | null;
  viewMode?: string;
  page?: number;
  pageSize?: number;
}

/**
 * Paged result from list endpoint
 */
export interface PagedTransactionResult {
  totalCount: number;
  page: number;
  pageSize: number;
  items: TransactionListItem[];
}

/**
 * Dialog data passed when opening new/edit dialog
 */
export interface TransactionDialogData {
  entry: TransactionEntry | null;
  entries?: TransactionEntry[] | null;
  tranId?: string | null;
  isEdit?: boolean;
  config: TransactionConfig;
}

/**
 * Dialog result returned when dialog closes
 */
export interface TransactionDialogResult {
  saved: boolean;
  sNo?: number;
  tranId?: string;
}

/**
 * Batch save result from endpoints
 */
export interface BatchSaveResult {
  entries: TransactionEntry[];
}

/**
 * Next transaction ID response
 */
export interface TranIdResponse {
  tranId: string;
}
