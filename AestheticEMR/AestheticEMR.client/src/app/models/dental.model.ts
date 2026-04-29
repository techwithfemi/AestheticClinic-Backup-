/// Maps to HDentalTreat — the real odontogram / dental treatment record.
export interface DentalChart {
  id: number;
  pno: string;
  consultId: string;
  dtype?: string;
  tDate: string;
  tTime?: string;

  // Adult Upper Left
  auli1?: boolean; auli2?: boolean; aulc?: boolean;
  aulpm1?: boolean; aulpm2?: boolean;
  aulm1?: boolean; aulm2?: boolean; aulm3?: boolean;

  // Adult Upper Right
  auri1?: boolean; auri2?: boolean; aurc?: boolean;
  aurpm1?: boolean; aurpm2?: boolean;
  aurm1?: boolean; aurm2?: boolean; aurm3?: boolean;

  // Adult Lower Left
  alli1?: boolean; alli2?: boolean; allc?: boolean;
  allpm1?: boolean; allpm2?: boolean;
  allm1?: boolean; allm2?: boolean; allm3?: boolean;

  // Adult Lower Right
  alri1?: boolean; alri2?: boolean; alrc?: boolean;
  alrpm1?: boolean; alrpm2?: boolean;
  alrm1?: boolean; alrm2?: boolean; alrm3?: boolean;

  // Child Upper Left
  culi1?: boolean; culi2?: boolean; culc?: boolean;
  culpm1?: boolean; culpm2?: boolean;

  // Child Upper Right
  curi1?: boolean; curi2?: boolean; curc?: boolean;
  curpm1?: boolean; curpm2?: boolean;

  // Child Lower Left
  clli1?: boolean; clli2?: boolean; cllc?: boolean;
  cllpm1?: boolean; cllpm2?: boolean;

  // Child Lower Right
  clri1?: boolean; clri2?: boolean; clrc?: boolean;
  clrpm1?: boolean; clrpm2?: boolean;

  aRem?: string;   // Adult remarks
  cRem?: string;   // Child remarks
  conId?: string;

  patientName?: string;
}

export interface DentalImaging {
  id: number;
  pno: string;
  consultId: string;
  imagingDate: string;
  imagingType?: string;
  toothRegion?: string;
  findings?: string;
  impression?: string;
  recommendations?: string;
  filePath?: string;
  fileName?: string;
  notes?: string;
  patientName?: string;
  createdBy?: string;
  createdDate?: string;
}

export interface DentalConsulting {
  id: number;
  consultId: string;
  pNo: string;
  clientCat: string;
  diagnosis?: string;
  prescription?: string;
  services?: string;
  investigate?: string;
  treatPlan?: string; // hidden in UI; populated on backend
}

export interface DentalEncounter {
  chart: DentalChart;
  imaging: DentalImaging;
  consulting: DentalConsulting;
}

export interface DentalEncounterSave {
  chart: DentalChart;
  imaging: DentalImaging;
  consulting: DentalConsulting;
}
