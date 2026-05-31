export interface Receipt {
  receiptNo: string;
  receiptDate: string;
  billNo: string;
  patientNo: string;
  patientName?: string;
  amountBilled: number;
  amountPaid: number;
  payType: string;
  receivedBy?: string;
  remarks?: string;
  chequeNo?: string;
  bankCode?: string;
  valueDate?: string;
}
