export interface Attendance {
  consultId?: string;
  recId?: number;
  recDate: string;
  pNo: string;
  clientCat?: string;
  clinicType: string;
  htime?: string;
  patVal?: number;
  suppres?: boolean;
  exitDate?: string;
  exitDateComment?: string;
  coyname?: string;
  billDate?: string;
  attndStatus?: string;
  attendedToByImmume?: boolean;
  hmoRef?: string;
  entryDate?: string;
  entryTime?: string;
}
