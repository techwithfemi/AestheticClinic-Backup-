export type ExpenseViewMode = 'all';

export interface ExpenseAccountLookup {
  accountNo: string;
  accountName: string;
}

export interface ExpenseEntry {
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
  remarks?: string | null;
}

export interface ExpenseListItem {
  sNo: number;
  tranDate: string;
  accountDebit: string;
  accountCredit: string;
  debitAccountName: string;
  creditAccountName: string;
  amount: number;
  description?: string | null;
  isPost: boolean;
  isClose: boolean;
  userName?: string | null;
  tranId?: string | null;
  remarks?: string | null;
}

export interface ExpenseListQuery {
  search?: string | null;
  fromDate?: string | null;
  toDate?: string | null;
  viewMode?: ExpenseViewMode;
  page?: number;
  pageSize?: number;
}

export interface PagedExpenseResult {
  totalCount: number;
  page: number;
  pageSize: number;
  items: ExpenseListItem[];
}

export interface ExpenseDialogData {
  entry: ExpenseEntry | null;
  entries?: ExpenseEntry[] | null;
  tranId?: string | null;
  isEdit?: boolean;
}

export interface ExpenseDialogResult {
  saved: boolean;
  sNo?: number;
  tranId?: string;
}

export interface ExpenseBatchSaveRequest {
  tranId?: string | null;
  entries: ExpenseEntry[];
}

export interface ExpenseBatchSaveResult {
  entries: ExpenseEntry[];
}

export interface ExpenseTranIdResponse {
  tranId: string;
}

export interface TransactionLine {
  sNo: number;
  tranNo: string;
  accountNo: string;
  accountName: string;
  debit: number;
  credit: number;
  description?: string | null;
}
