export interface InvoicePrintDetail {
  sno: number;
  itemName: string;
  price: number;
  qty: number;
  subTotal: number;
  category?: string;
  billType?: string;
}

export interface InvoicePrintData {
  // Header
  billHead: string;
  billHead2: string;
  billHead3: string;
  billHead4: string;
  // Metadata
  billNo: string;
  billDate: string;
  taxName: string;
  tin: string;
  taxPcent: number;
  // Patient / payer
  patientName: string;
  patientNo: string;
  clientCat: string;
  payerName: string;
  payerAddress: string;
  payerPhone: string;
  // Summary
  debtBF: number;
  amountBilled: number;
  discount: number;
  tax: number;
  amountPaid: number;
  balance: number;
  // Receipt fields (populated after SaveReceipt)
  receiptNo?: string;
  receiptDate?: string;
  payType?: string;
  // Line items
  details: InvoicePrintDetail[];
}
