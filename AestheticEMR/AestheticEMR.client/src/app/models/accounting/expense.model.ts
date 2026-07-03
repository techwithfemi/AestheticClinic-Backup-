export type ExpenseViewMode = 'all' | 'posted' | 'unposted';

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
  postDirectly?: boolean;
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
}

export interface ExpenseDialogResult {
  saved: boolean;
  sNo?: number;
}
