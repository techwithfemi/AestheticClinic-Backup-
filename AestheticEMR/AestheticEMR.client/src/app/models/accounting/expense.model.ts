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
