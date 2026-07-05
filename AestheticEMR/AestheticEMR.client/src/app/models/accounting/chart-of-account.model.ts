export interface ChartOfAccountGroupLookup {
  groupID: string;
  groupName: string;
}

export interface ChartOfAccountDefaults {
  autoAccountNo: string;
  receiveExtData: string;
  receiveArData: string;
  receiveApData: string;
  receiveExpenseData: string;
  receivePayrollData: string;
}

export interface ChartOfAccountEntry {
  sNo?: number | null;
  accountNo: string;
  accountName: string;
  groupID: string;
  groupName?: string | null;
  accountDesc?: string | null;
  accountOpAmt: number;
  accountClAmt: number;
}

export interface ChartOfAccountListItem {
  sNo: number;
  accountNo: string;
  accountName: string;
  groupID: string;
  groupName: string;
  accountDesc?: string | null;
  accountOpAmt: number;
  accountClAmt: number;
}

export interface ChartOfAccountListQuery {
  search?: string | null;
  page?: number;
  pageSize?: number;
  sortBy?: string | null;
  sortDirection?: 'asc' | 'desc' | null;
}

export interface PagedChartOfAccountResult {
  totalCount: number;
  page: number;
  pageSize: number;
  items: ChartOfAccountListItem[];
}

export interface ChartOfAccountDialogData {
  entry: ChartOfAccountEntry | null;
  groups: ChartOfAccountGroupLookup[];
  defaults: ChartOfAccountDefaults;
}

export interface ChartOfAccountDialogResult {
  saved: boolean;
  sNo?: number;
  operation?: 'create' | 'update';
  entry?: ChartOfAccountEntry;
}
