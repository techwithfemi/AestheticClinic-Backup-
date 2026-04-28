// ---------------------------------------
// Aesthetic EMR frontend models
// ---------------------------------------

export interface AestheticPhoto {
  id: number;
  consultationId: number;
  consultId?: string;
  pNo?: string;
  fileName?: string;
  url: string;
  thumbnailUrl?: string;
  type?: string;
  caption?: string;
  createdDate?: string;
  updatedDate?: string;
}

export interface AestheticConsultation {
  id: number;
  patientId: number;
  patientName?: string;
  consultationDate?: string;
  procedureType?: string;
  provider?: string;
  consentGiven?: boolean;
  informationAccepted?: boolean;
  consentDate?: string;
  consentNotes?: string;
  procedureDescription?: string;
  risksAndComplications?: string;
  postTreatmentInstructions?: string;
  skinAssessment?: string;
  treatmentPlan?: string;
  currentMedications?: string;
  allergies?: string;
  deviceSettings?: string;

  areaTreated?: string;

  deviceUsed?: string;
  wavelength?: string;
  spotSize?: string;
  fluence?: string;
  pulseDuration?: string;
  coolingMethod?: string;
  numberOfShots?: number;
  skinReaction?: string;
  nextSessionDate?: string;

  indication?: string;
  brandUsed?: string;
  dilution?: string;
  unitsUsed?: number;
  injectionMapping?: string;
  lotNumber?: string;
  followUpReview?: string;

  photos?: AestheticPhoto[];
}

export interface AestheticPatient {
  id: number;
  pno?: string; // Patient number from legacy system
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  gender?: string;
  skinType?: string;
  allergies?: string;
  medicalHistory?: string;
  currentMedications?: string;
  notes?: string;
  consultations?: AestheticConsultation[];
}
