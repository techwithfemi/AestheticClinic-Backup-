export interface BillingDetail {
  sno?: number;
  drgName: string;
  price: number;
  qty: number;
  billType?: string;
  conID?: string;
  revenueType?: string;
  revenueTypeName?: string;
  billTo?: string;
  coyName?: string;
}

export interface Billing {
  billNo: string;
  bDate: string;
  pNo: string;
  clientID?: string;
  debtBF?: number;
  amountBilled?: number;
  discount?: number;
  amountPaid?: number;
  billType?: string;
  isPaid?: boolean;
  details: BillingDetail[];
  consultId?: string;
  company?: string;
  patientName?: string;
}
