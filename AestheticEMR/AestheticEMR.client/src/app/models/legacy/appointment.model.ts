export interface Appointment {
  id?: number;
  pno: string;
  apptDate: string;
  apptTime: string;
  clinicType: string;
  remarks?: string;
  empID?: string;
  entryDate?: string;
  entryTime?: string;
}
