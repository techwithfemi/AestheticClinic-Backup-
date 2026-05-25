/// Maps to HDentalTreat — the real odontogram / dental treatment record.
export interface DentalChart {
  id: number;
  pno: string;
  consultId: string;
  dtype?: string;
  tDate: string;
  tTime?: string;

  // Tooth status map: key is FDI number (e.g., '11', '12', ... '48')
  teethStatus?: Record<string, ToothStatus>;
  oralExam?: OralExam;
  orthodontics?: OrthodonticForm;

  // Existing quadrant/teeth booleans remain for compatibility

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

  // New clinical findings fields
  inflammationOfGingiva?: string;
  presenceOfDebris?: string;
  presenceOfCalculus?: string;
  presenceOfStains?: string;
  underOrthodonticTreatment?: string;
  otherClinicalFindings?: string;
}

export interface ToothStatus {
  present?: boolean;
  carious?: boolean;
  decayed?: boolean;
  missing?: boolean;
  filled?: boolean;
}

export interface OralExam {
  caries?: boolean;
  poorOralHygiene?: boolean;
  indicatedForRestorationFilling?: boolean;
  fillingGic?: boolean;
  fillingComposite?: boolean;
  fissureSealant?: boolean;
  indicatedForExtraction?: boolean;
  gingivalInflammation?: boolean;
  needsOralProphylaxis?: boolean;
  needsProsthesisDenture?: boolean;
  forEndodonticTreatment?: boolean;
  forOrthodonticConsultation?: boolean;
  others?: string;
  noDentalTreatmentNeededAtPresent?: boolean;
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
  complaints?: string;
  hpc?: string;
  pmh?: string;
  dentHist?: string;
  drugHx?: string;
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

export interface OrthodonticForm {
  classI?: boolean;
  classII?: boolean;
  classIII?: boolean;
  crowdingUpper?: boolean;
  crowdingLower?: boolean;
  spacingUpper?: boolean;
  spacingLower?: boolean;
  crossbiteAnterior?: boolean;
  crossbitePosterior?: boolean;
  overjetIncreased?: boolean;
  overbiteDeep?: boolean;
  openbite?: boolean;
  midlineShift?: boolean;
  impactedTeeth?: boolean;
  tmjSymptoms?: boolean;
  oralHabits?: string;
  treatmentObjective?: string;
  applianceSelection?: string;
  extractionRequired?: boolean;
  notes?: string;
  clinicalStudyModel?: boolean;
  extraoralPhotographs?: boolean;
  intraoralPhotographs?: boolean;

  overjet?: string;
  overbite?: string;
  teethImpaction?: boolean;
  teethImpactionDetails?: string;

  molarRelationRight?: string;
  molarRelationLeft?: string;
  canineRelationRight?: string;
  canineRelationLeft?: string;

  lipsCompetent?: boolean;
  lipsIncompetent?: boolean;
  thumbSucking?: boolean;
  tongueThrusting?: boolean;
  mouthBreathing?: boolean;
  nailBiting?: boolean;
  lipBiting?: boolean;

  skeletalPatternAnteroPosterior?: string;
  skeletalPatternVertical?: string;
  skeletalPatternTransverse?: string;

  archWidthUpper?: string;
  archWidthLower?: string;
  curveOfSpee?: string;
  dentalMidline?: string;
  rotations?: string;
  toothAnomalies?: string;

  summaryOfOrthodonticAnalysis?: string;
  investigationOpg?: boolean;
  investigationCeph?: boolean;
}
