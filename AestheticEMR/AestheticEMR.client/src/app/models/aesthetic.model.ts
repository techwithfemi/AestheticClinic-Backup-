// ---------------------------------------
// Aesthetic EMR frontend models
// ---------------------------------------

export interface AestheticPhoto {
  id: number;
  consultationId: number;
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
  procedureDescription?: string;
  risksAndComplications?: string;
  postTreatmentInstructions?: string;
  skinAssessment?: string;
  treatmentPlan?: string;
  currentMedications?: string;
  allergies?: string;
  deviceSettings?: string;
  photos?: AestheticPhoto[];
}

export interface AestheticPatient {
  id: number;
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
