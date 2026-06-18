export interface Receipt {
  receiptDate: string;
  rTime?: string;
  receiptNo: string;
  pNo: string;
  paymentFor: string;
  amountBilled: number;
  tax: number;
  amountPaid: number;
  balance?: number;
  payType: string;
  clinicId?: string;
  fullname: string;
  patNo: string;
  receivedBy?: string;
  billNo: string;
  coyName?: string;
  isPost?: boolean;
  remarks?: string;
  suppres?: boolean;
  canDelete?: boolean;
}
