export interface IncomeAccountLookup {
  accountNo: string;
  accountName: string;
}

export interface IncomeEntry {
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

export interface IncomeListItem {
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

export interface IncomeListQuery {
  search?: string | null;
  fromDate?: string | null;
  toDate?: string | null;
  page?: number;
  pageSize?: number;
}

export interface PagedIncomeResult {
  totalCount: number;
  page: number;
  pageSize: number;
  items: IncomeListItem[];
}

export interface IncomeBatchSaveRequest {
  tranId?: string | null;
  entries: IncomeEntry[];
}
