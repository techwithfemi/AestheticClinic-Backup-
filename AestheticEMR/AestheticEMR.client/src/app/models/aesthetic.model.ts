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

export interface AestheticConsentTemplate {
  id: number;
  name?: string;
  title?: string;
  procedureType?: string;
  content?: string;
  isActive?: boolean;
}

export interface AestheticSignedConsent {
  id: number;
  patientId?: number;
  consentTemplateId: number;
  consultId?: string;
  pNo?: string;
  procedureType?: string;
  signedDate?: string;
  signedBy?: string;
  witnessedBy?: string;
  signatureName?: string;
  notes?: string;
  consentContent?: string;
  signatureImageBase64?: string;
  signatureImagePath?: string;
  doctorViewedBy?: string;
  doctorViewedDate?: string;
  isVoided?: boolean;
  voidReason?: string;
}

export interface AestheticConsentStatus {
  consultId?: string;
  pNo?: string;
  procedureType?: string;
  attendanceTaken?: boolean;
  hasValidConsent?: boolean;
  canSign?: boolean;
  activeTemplate?: AestheticConsentTemplate;
  latestSignedConsent?: AestheticSignedConsent;
}

export interface SignAestheticConsent {
  patientId?: number;
  consultId: string;
  pNo: string;
  procedureType: string;
  consentTemplateId: number;
  signatureName: string;
  witnessedBy?: string;
  signedBy?: string;
  notes?: string;
  signatureImageBase64?: string;
}

export interface AestheticConsultation {
  id: number;
  patientId: number;
  patientName?: string;
  consultationDate?: string;
  procedureType?: string;
  provider?: string;
  consultId?: string;
  pNo?: string;
  services?: string;
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

export interface AestheticFollowUp {
  id: number;
  consultationId: number;
  patientId?: number;
  patientName?: string;
  scheduledDate?: string;
  isAutoScheduled?: boolean;
  isCompleted?: boolean;
  completedDate?: string;
  outcome?: string;
  patientSatisfactionScore?: number;
  repeatPhotosTaken?: boolean;
  nextTreatmentRecommendation?: string;
  notes?: string;
  patientSatisfactionConsultId?: string;
  patientSatisfactionPNo?: string;
  patientSatisfactionSubmittedOn?: string;
}

export interface ScheduleAestheticFollowUp {
  consultationId: number;
  daysAhead: number;
  notes?: string;
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

export interface VoidAestheticConsent {
  voidReason: string;
}

export interface SendPatientSatisfactionRequest {
  recipientEmail: string;
  recipientName?: string;
}

export interface PatientSatisfactionEmailResponse {
  followUpId: number;
  consultationId: number;
  consultId?: string;
  pNo?: string;
  expiresOnUtc?: string;
  sentTo?: string;
}

export interface PublicPatientSatisfactionSurvey {
  followUpId: number;
  consultationId: number;
  consultId?: string;
  pNo?: string;
  patientName?: string;
  scheduledDate?: string;
}

export interface SubmitPatientSatisfaction {
  patientSatisfactionScore: number;
  outcome?: string;
}
