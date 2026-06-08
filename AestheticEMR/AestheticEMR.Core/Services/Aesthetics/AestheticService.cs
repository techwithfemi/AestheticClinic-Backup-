// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Aesthetic;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace AestheticEMR.Core.Services.Aesthetics
{
    public class AestheticService(ApplicationDbContext dbContext) : IAestheticService
    {
        private const string PatientSatisfactionTokenPurpose = "AESTHETIC_PATIENT_SATISFACTION";

        public IEnumerable<AestheticPatient> GetPatients() => dbContext.AestheticPatients
            .Include(p => p.Consultations)
            .ThenInclude(c => c.Photos)
            .AsSingleQuery()
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToList();

        public AestheticPatient? GetPatientById(int id) => dbContext.AestheticPatients
            .Include(p => p.Consultations)
            .ThenInclude(c => c.Photos)
            .AsSingleQuery()
            .FirstOrDefault(p => p.Id == id);

        public AestheticPatient AddPatient(AestheticPatient patient)
        {
            patient.CreatedDate = DateTime.UtcNow;
            patient.UpdatedDate = DateTime.UtcNow;
            dbContext.AestheticPatients.Add(patient);
            dbContext.SaveChanges();
            return patient;
        }

        public AestheticSignedConsent UpdateSignedConsent(int consentId, int? patientId, int? consentTemplateId, string signatureName, string? witnessedBy, string? notes, byte[]? signatureImage, string? signatureImagePath, string currentUserId)
        {
            var existing = dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .FirstOrDefault(x => x.Id == consentId);

            if (existing == null)
                throw new KeyNotFoundException($"Signed consent not found: {consentId}");

            // Only creator can update
            if (!string.Equals(existing.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this consent can update it.");

            if (consentTemplateId.HasValue)
            {
                var template = dbContext.AestheticConsentTemplates.FirstOrDefault(x => x.Id == consentTemplateId.Value && x.IsActive);
                if (template == null)
                    throw new KeyNotFoundException($"Consent template not found: {consentTemplateId}");

                existing.ConsentTemplateId = template.Id;
                existing.ConsentTemplate = template;
                existing.ConsentContent = template.Content;
            }

            existing.PatientId = patientId ?? existing.PatientId;
            existing.SignatureName = NormalizeRequired(signatureName, "Signature name");
            existing.WitnessedBy = NormalizeOptional(witnessedBy);
            existing.Notes = NormalizeOptional(notes);
            existing.SignatureImage = signatureImage ?? existing.SignatureImage;
            existing.SignatureImagePath = NormalizeOptional(signatureImagePath) ?? existing.SignatureImagePath;
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();

            return dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .First(x => x.Id == existing.Id);
        }

        public AestheticPatient UpdatePatient(AestheticPatient patient)
        {
            var existing = dbContext.AestheticPatients.Find(patient.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Patient not found: {patient.Id}");

            existing.FirstName = patient.FirstName;
            existing.LastName = patient.LastName;
            existing.Email = patient.Email;
            existing.PhoneNumber = patient.PhoneNumber;
            existing.DateOfBirth = patient.DateOfBirth;
            existing.Gender = patient.Gender;
            existing.SkinType = patient.SkinType;
            existing.Allergies = patient.Allergies;
            existing.MedicalHistory = patient.MedicalHistory;
            existing.CurrentMedications = patient.CurrentMedications;
            existing.Notes = patient.Notes;
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            return existing;
        }

        public IEnumerable<AestheticConsultation> GetConsultationsForPatient(int patientId) => dbContext.AestheticConsultations
            .Include(c => c.Photos)
            .Include(c => c.Patient)
            .AsSingleQuery()
            .Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.ConsultationDate)
            .ToList();

        public IEnumerable<AestheticConsultation> GetConsultationsByProcedure(string procedureType) => dbContext.AestheticConsultations
            .Include(c => c.Photos)
            .Include(c => c.Patient)
            .AsSingleQuery()
            .Where(c => c.ProcedureType.ToLower() == procedureType.ToLower())
            .OrderByDescending(c => c.ConsultationDate)
            .ToList();

        public IEnumerable<AestheticConsultation> GetLaserSessions() => dbContext.AestheticConsultations
            .Include(c => c.Patient)
            .Include(c => c.Photos)
            .AsSingleQuery()
            .Where(c => c.ProcedureType == "Laser")
            .OrderByDescending(c => c.ConsultationDate)
            .ToList();

        public AestheticConsultation AddConsultation(AestheticConsultation consultation, string? consultId = null, string? pNo = null, string? services = null)
        {
            consultation.CreatedDate = DateTime.UtcNow;
            consultation.UpdatedDate = DateTime.UtcNow;

            ApplyConsentStatus(consultation, consultId, pNo);
            ValidateConsultationSafetyRequirements(consultation, consultId, pNo);

            dbContext.AestheticConsultations.Add(consultation);
            dbContext.SaveChanges();

            AutoScheduleDefaultFollowUp(consultation.Id);
            SyncLegacyConsultingOnCreate(consultation, consultId, pNo, services);
            return consultation;
        }

        public AestheticConsultation UpdateConsultation(AestheticConsultation consultation, string currentUserId, string? consultId = null, string? pNo = null, string? services = null)
        {
            var existing = dbContext.AestheticConsultations.Find(consultation.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Consultation not found: {consultation.Id}");

            if (!string.Equals(existing.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

            existing.PatientId = consultation.PatientId;
            existing.ConsultationDate = consultation.ConsultationDate;
            existing.ProcedureType = consultation.ProcedureType;
            existing.Provider = consultation.Provider;
            existing.ProcedureDescription = consultation.ProcedureDescription;
            existing.RisksAndComplications = consultation.RisksAndComplications;
            existing.PostTreatmentInstructions = consultation.PostTreatmentInstructions;
            existing.SkinAssessment = consultation.SkinAssessment;
            existing.TreatmentPlan = consultation.TreatmentPlan;
            existing.CurrentMedications = consultation.CurrentMedications;
            existing.Allergies = consultation.Allergies;
            existing.DeviceSettings = consultation.DeviceSettings;
            existing.AreaTreated = consultation.AreaTreated;
            existing.DeviceUsed = consultation.DeviceUsed;
            existing.Wavelength = consultation.Wavelength;
            existing.SpotSize = consultation.SpotSize;
            existing.Fluence = consultation.Fluence;
            existing.PulseDuration = consultation.PulseDuration;
            existing.CoolingMethod = consultation.CoolingMethod;
            existing.NumberOfShots = consultation.NumberOfShots;
            existing.SkinReaction = consultation.SkinReaction;
            existing.NextSessionDate = consultation.NextSessionDate;
            existing.Indication = consultation.Indication;
            existing.BrandUsed = consultation.BrandUsed;
            existing.Dilution = consultation.Dilution;
            existing.UnitsUsed = consultation.UnitsUsed;
            existing.InjectionMapping = consultation.InjectionMapping;
            existing.LotNumber = consultation.LotNumber;
            existing.FollowUpReview = consultation.FollowUpReview;

            ApplyConsentStatus(existing, consultId, pNo);
            ValidateConsultationSafetyRequirements(existing, consultId, pNo);
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();

            SyncLegacyConsultingOnUpdate(existing, consultId, pNo, services);
            return existing;
        }

        public void DeleteConsultation(int consultationId, string currentUserId)
        {
            var consultation = dbContext.AestheticConsultations
                .Include(c => c.Photos)
                .Include(c => c.Patient)
                .FirstOrDefault(c => c.Id == consultationId);

            if (consultation == null)
                throw new KeyNotFoundException($"Consultation not found: {consultationId}");

            if (!string.Equals(consultation.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can delete it.");

            var consultId = ResolveLegacyConsultId(consultation);
            if (!string.IsNullOrWhiteSpace(consultId) && IsConsultIdReferenced(consultId))
            {
                throw new InvalidOperationException("Cannot delete this Botox record because its consult ID is referenced by operational/billing records.");
            }

            if (consultation.Photos.Count > 0)
                dbContext.AestheticPhotos.RemoveRange(consultation.Photos);

            dbContext.AestheticConsultations.Remove(consultation);
            dbContext.SaveChanges();
        }

        public AestheticConsultation? GetConsultationById(int consultationId) => dbContext.AestheticConsultations
            .Include(c => c.Photos)
            .Include(c => c.Patient)
            .AsSingleQuery()
            .FirstOrDefault(c => c.Id == consultationId);

        public IEnumerable<AestheticPhoto> GetPhotos() => dbContext.AestheticPhotos
            .Include(p => p.Consultation)
            .ThenInclude(c => c.Patient)
            .AsSingleQuery()
            .OrderByDescending(photo => photo.CreatedDate)
            .ToList();

        public AestheticPhoto? GetPhotoById(int photoId) => dbContext.AestheticPhotos
            .Include(p => p.Consultation)
            .ThenInclude(c => c.Patient)
            .AsSingleQuery()
            .FirstOrDefault(p => p.Id == photoId);

        public IEnumerable<AestheticPhoto> GetPhotosForConsultation(int consultationId) => dbContext.AestheticPhotos
            .Where(photo => photo.ConsultationId == consultationId)
            .OrderByDescending(photo => photo.CreatedDate)
            .ToList();

        public AestheticPhoto AddPhoto(AestheticPhoto photo)
        {
            var consultation = dbContext.AestheticConsultations
                .Include(c => c.Patient)
                .FirstOrDefault(c => c.Id == photo.ConsultationId);

            photo.ConsultId = string.IsNullOrWhiteSpace(photo.ConsultId)
                ? ResolveLegacyConsultId(consultation)
                : photo.ConsultId;
            photo.PNo = string.IsNullOrWhiteSpace(photo.PNo)
                ? consultation?.Patient?.Pno
                : photo.PNo;

            photo.CreatedDate = DateTime.UtcNow;
            photo.UpdatedDate = DateTime.UtcNow;
            dbContext.AestheticPhotos.Add(photo);
            dbContext.SaveChanges();
            return photo;
        }

        public AestheticPhoto UpdatePhoto(AestheticPhoto photo, string currentUserId)
        {
            var existing = dbContext.AestheticPhotos.Find(photo.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Photo not found: {photo.Id}");

            if (!string.Equals(existing.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can update it.");

            var consultation = dbContext.AestheticConsultations
                .Include(c => c.Patient)
                .FirstOrDefault(c => c.Id == photo.ConsultationId);

            existing.ConsultationId = photo.ConsultationId;
            existing.ConsultId = string.IsNullOrWhiteSpace(photo.ConsultId)
                ? ResolveLegacyConsultId(consultation)
                : photo.ConsultId;
            existing.PNo = string.IsNullOrWhiteSpace(photo.PNo)
                ? consultation?.Patient?.Pno
                : photo.PNo;
            existing.FileName = photo.FileName;
            existing.FilePath = photo.FilePath;
            existing.Type = photo.Type;
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            return existing;
        }

        public void DeletePhoto(int photoId, string currentUserId)
        {
            var photo = dbContext.AestheticPhotos.Find(photoId);
            if (photo == null)
                throw new KeyNotFoundException($"Photo not found: {photoId}");

            if (!string.Equals(photo.CreatedBy, currentUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Only the author that created this clinical record can delete it.");

            dbContext.AestheticPhotos.Remove(photo);
            dbContext.SaveChanges();
        }

        public IEnumerable<AestheticConsentTemplate> GetConsentTemplates(string? procedureType = null, bool includeInactive = false)
        {
            var query = dbContext.AestheticConsentTemplates.AsNoTracking().AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(x => x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(procedureType))
            {
                var normalizedProcedureType = NormalizeRequired(procedureType, "Procedure type");
                query = query.Where(x => x.ProcedureType == null || x.ProcedureType == normalizedProcedureType);
            }

            return query
                .OrderBy(x => x.ProcedureType)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Title)
                .ToList();
        }

        public AestheticConsentTemplate? GetConsentTemplateById(int id) => dbContext.AestheticConsentTemplates
            .FirstOrDefault(x => x.Id == id);

        public AestheticConsentTemplate AddConsentTemplate(AestheticConsentTemplate template)
        {
            template.Name = NormalizeRequired(template.Name, "Template name");
            template.Title = NormalizeRequired(template.Title, "Template title");
            template.Content = NormalizeRequired(template.Content, "Template content");
            template.ProcedureType = NormalizeOptional(template.ProcedureType);
            template.CreatedDate = DateTime.UtcNow;
            template.UpdatedDate = DateTime.UtcNow;

            dbContext.AestheticConsentTemplates.Add(template);
            dbContext.SaveChanges();
            return template;
        }

        public AestheticConsentTemplate UpdateConsentTemplate(AestheticConsentTemplate template)
        {
            var existing = dbContext.AestheticConsentTemplates.FirstOrDefault(x => x.Id == template.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Consent template not found: {template.Id}");
            }

            existing.Name = NormalizeRequired(template.Name, "Template name");
            existing.Title = NormalizeRequired(template.Title, "Template title");
            existing.Content = NormalizeRequired(template.Content, "Template content");
            existing.ProcedureType = NormalizeOptional(template.ProcedureType);
            existing.IsActive = template.IsActive;
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            return existing;
        }

        public void DeleteConsentTemplate(int id)
        {
            var existing = dbContext.AestheticConsentTemplates
                .Include(x => x.SignedConsents)
                .FirstOrDefault(x => x.Id == id);

            if (existing == null)
            {
                throw new KeyNotFoundException($"Consent template not found: {id}");
            }

            if (existing.SignedConsents.Count > 0)
            {
                throw new InvalidOperationException("Consent template cannot be deleted because it has signed consent records.");
            }

            dbContext.AestheticConsentTemplates.Remove(existing);
            dbContext.SaveChanges();
        }

        public IEnumerable<AestheticSignedConsent> GetSignedConsents(string? consultId = null, string? pNo = null, string? procedureType = null, bool includeVoided = false)
        {
            var query = dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .AsQueryable();

            if (!includeVoided)
            {
                query = query.Where(x => !x.IsVoided);
            }

            if (!string.IsNullOrWhiteSpace(consultId))
            {
                var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
                query = query.Where(x => x.ConsultId == normalizedConsultId);
            }

            if (!string.IsNullOrWhiteSpace(pNo))
            {
                var normalizedPNo = NormalizeRequired(pNo, "PNo");
                query = query.Where(x => x.PNo == normalizedPNo);
            }

            if (!string.IsNullOrWhiteSpace(procedureType))
            {
                var normalizedProcedureType = NormalizeRequired(procedureType, "Procedure type");
                query = query.Where(x => x.ProcedureType == normalizedProcedureType);
            }

            return query
                .OrderByDescending(x => x.SignedDate)
                .ThenByDescending(x => x.Id)
                .ToList();
        }

        public AestheticSignedConsent SignConsent(int? patientId, string consultId, string pNo, string procedureType, int consentTemplateId, string signatureName, string? witnessedBy, string? signedBy, string? notes, byte[]? signatureImage, string? signatureImagePath)
        {
            var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
            var normalizedPNo = NormalizeRequired(pNo, "PNo");
            var normalizedProcedureType = NormalizeRequired(procedureType, "Procedure type");
            var normalizedSignatureName = NormalizeRequired(signatureName, "Signature name");
            var normalizedSignatureImagePath = NormalizeOptional(signatureImagePath);

            var attendance = dbContext.HRecords.FirstOrDefault(x => x.ConsultId == normalizedConsultId && x.PNo == normalizedPNo);
            if (attendance == null)
            {
                throw new InvalidOperationException("Patient consent can only be signed after attendance is taken.");
            }

            var template = dbContext.AestheticConsentTemplates.FirstOrDefault(x => x.Id == consentTemplateId && x.IsActive);
            if (template == null)
            {
                throw new KeyNotFoundException($"Consent template not found: {consentTemplateId}");
            }

            if (!string.IsNullOrWhiteSpace(template.ProcedureType)
                && !string.Equals(template.ProcedureType, normalizedProcedureType, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Selected consent template does not match the procedure type.");
            }

            var existing = GetLatestSignedConsent(normalizedConsultId, normalizedPNo, normalizedProcedureType);
            if (existing != null)
            {
                return existing;
            }

            var resolvedPatientId = patientId ?? dbContext.AestheticPatients
                .Where(x => x.Pno == normalizedPNo)
                .Select(x => (int?)x.Id)
                .FirstOrDefault();

            var signedConsent = new AestheticSignedConsent
            {
                PatientId = resolvedPatientId,
                ConsentTemplateId = consentTemplateId,
                ConsentTemplate = template,
                ConsultId = normalizedConsultId,
                PNo = normalizedPNo,
                ProcedureType = normalizedProcedureType,
                SignedDate = DateTime.UtcNow,
                SignedBy = NormalizeOptional(signedBy),
                WitnessedBy = NormalizeOptional(witnessedBy),
                SignatureName = normalizedSignatureName,
                Notes = NormalizeOptional(notes),
                ConsentContent = template.Content,
                SignatureImage = signatureImage,
                SignatureImagePath = normalizedSignatureImagePath,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsVoided = false
            };

            dbContext.AestheticSignedConsents.Add(signedConsent);
            dbContext.SaveChanges();

            UpdateConsultationConsentFields(normalizedConsultId, normalizedPNo, normalizedProcedureType, signedConsent);
            return dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .First(x => x.Id == signedConsent.Id);
        }

        public AestheticSignedConsent VoidConsent(int consentId, string voidReason, string voidedBy)
        {
            var normalizedVoidReason = NormalizeRequired(voidReason, "Void reason");
            var normalizedVoidedBy = NormalizeRequired(voidedBy, "Voided by");
            var consent = dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == consentId);

            if (consent == null)
            {
                throw new KeyNotFoundException($"Signed consent not found: {consentId}");
            }

            consent.IsVoided = true;
            consent.VoidReason = $"{normalizedVoidReason} (by {normalizedVoidedBy})";
            consent.UpdatedDate = DateTime.UtcNow;
            dbContext.SaveChanges();

            ResetConsultationConsentFields(consent.ConsultId, consent.PNo, consent.ProcedureType);
            return consent;
        }

        public AestheticConsentStatus GetConsentStatus(string consultId, string pNo, string procedureType)
        {
            var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
            var normalizedPNo = NormalizeRequired(pNo, "PNo");
            var normalizedProcedureType = NormalizeRequired(procedureType, "Procedure type");

            var attendanceTaken = dbContext.HRecords.Any(x => x.ConsultId == normalizedConsultId && x.PNo == normalizedPNo);
            var latestSignedConsent = GetLatestSignedConsent(normalizedConsultId, normalizedPNo, normalizedProcedureType);
            var activeTemplate = GetConsentTemplates(normalizedProcedureType).FirstOrDefault();

            return new AestheticConsentStatus
            {
                ConsultId = normalizedConsultId,
                PNo = normalizedPNo,
                ProcedureType = normalizedProcedureType,
                AttendanceTaken = attendanceTaken,
                HasValidConsent = latestSignedConsent != null,
                ActiveTemplate = activeTemplate,
                LatestSignedConsent = latestSignedConsent
            };
        }

        public AestheticSignedConsent? GetLatestSignedConsent(string consultId, string pNo, string procedureType)
        {
            var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
            var normalizedPNo = NormalizeRequired(pNo, "PNo");
            var normalizedProcedureType = NormalizeRequired(procedureType, "Procedure type");

            return dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .Where(x => x.ConsultId == normalizedConsultId
                         && x.PNo == normalizedPNo
                         && x.ProcedureType == normalizedProcedureType
                         && !x.IsVoided)
                .OrderByDescending(x => x.SignedDate)
                .ThenByDescending(x => x.Id)
                .FirstOrDefault();
        }

        public AestheticSignedConsent MarkConsentViewed(int consentId, string doctorViewedBy)
        {
            var normalizedDoctorViewedBy = NormalizeRequired(doctorViewedBy, "Doctor");
            var consent = dbContext.AestheticSignedConsents
                .Include(x => x.ConsentTemplate)
                .Include(x => x.Patient)
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == consentId);

            if (consent == null)
            {
                throw new KeyNotFoundException($"Signed consent not found: {consentId}");
            }

            consent.DoctorViewedBy = normalizedDoctorViewedBy;
            consent.DoctorViewedDate = DateTime.UtcNow;
            consent.UpdatedDate = DateTime.UtcNow;
            dbContext.SaveChanges();
            return consent;
        }

        public IEnumerable<AestheticFollowUp> GetFollowUps(int? patientId = null, int? consultationId = null, bool? isCompleted = null)
        {
            var query = dbContext.Set<AestheticFollowUp>()
                .Include(x => x.Consultation)
                .ThenInclude(c => c.Patient)
                .AsSingleQuery()
                .AsQueryable();

            if (patientId.HasValue)
            {
                query = query.Where(x => x.Consultation.PatientId == patientId.Value);
            }

            if (consultationId.HasValue)
            {
                query = query.Where(x => x.ConsultationId == consultationId.Value);
            }

            if (isCompleted.HasValue)
            {
                query = query.Where(x => x.IsCompleted == isCompleted.Value);
            }

            return query
                .OrderBy(x => x.IsCompleted)
                .ThenBy(x => x.ScheduledDate)
                .ToList();
        }

        public AestheticFollowUp? GetFollowUpById(int followUpId)
        {
            return dbContext.Set<AestheticFollowUp>()
                .Include(x => x.Consultation)
                .ThenInclude(c => c.Patient)
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == followUpId);
        }

        public AestheticFollowUp ScheduleFollowUp(int consultationId, int daysAhead, bool isAutoScheduled = false, string? notes = null)
        {
            if (daysAhead < 1)
            {
                throw new InvalidOperationException("Follow-up schedule days must be at least 1 day.");
            }

            var consultation = dbContext.AestheticConsultations.FirstOrDefault(x => x.Id == consultationId)
                ?? throw new KeyNotFoundException($"Consultation not found: {consultationId}");

            var followUp = new AestheticFollowUp
            {
                ConsultationId = consultationId,
                Consultation = consultation,
                ScheduledDate = DateTime.UtcNow.Date.AddDays(daysAhead),
                IsAutoScheduled = isAutoScheduled,
                IsCompleted = false,
                Notes = NormalizeOptional(notes),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            dbContext.Set<AestheticFollowUp>().Add(followUp);
            dbContext.SaveChanges();
            return followUp;
        }

        public AestheticFollowUp CompleteFollowUp(int followUpId, string? outcome, int? patientSatisfactionScore, bool repeatPhotosTaken, string? nextTreatmentRecommendation, string? notes)
        {
            var followUp = dbContext.Set<AestheticFollowUp>().FirstOrDefault(x => x.Id == followUpId)
                ?? throw new KeyNotFoundException($"Follow-up not found: {followUpId}");

            if (string.IsNullOrWhiteSpace(outcome))
            {
                throw new InvalidOperationException("Follow-up outcome is required.");
            }

            if (!patientSatisfactionScore.HasValue)
            {
                throw new InvalidOperationException("Patient satisfaction score (1-10) is required.");
            }

            if (patientSatisfactionScore.Value < 1 || patientSatisfactionScore.Value > 10)
            {
                throw new InvalidOperationException("Patient satisfaction score must be between 1 and 10.");
            }

            if (string.IsNullOrWhiteSpace(nextTreatmentRecommendation))
            {
                throw new InvalidOperationException("Next treatment recommendation is required.");
            }

            followUp.IsCompleted = true;
            followUp.CompletedDate = DateTime.UtcNow;
            followUp.Outcome = NormalizeOptional(outcome);
            followUp.PatientSatisfactionScore = patientSatisfactionScore;
            followUp.RepeatPhotosTaken = repeatPhotosTaken;
            followUp.NextTreatmentRecommendation = NormalizeOptional(nextTreatmentRecommendation);
            followUp.Notes = NormalizeOptional(notes);
            followUp.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            return followUp;
        }

        public (int followUpId, int consultationId, string? consultId, string? pNo, string? patientName, DateTime? scheduledDate) GetFollowUpSubmissionContext(int followUpId)
        {
            var followUp = dbContext.Set<AestheticFollowUp>()
                .Include(x => x.Consultation)
                .ThenInclude(c => c.Patient)
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == followUpId)
                ?? throw new KeyNotFoundException($"Follow-up not found: {followUpId}");

            var pNo = ResolveLegacyPNo(followUp.Consultation.PatientId, followUp.Consultation.Patient?.Pno);
            var consultId = ResolveLegacyConsultId(followUp.Consultation.ConsultationDate, pNo, null);
            var patientName = followUp.Consultation.Patient != null
                ? $"{followUp.Consultation.Patient.FirstName} {followUp.Consultation.Patient.LastName}".Trim()
                : null;

            return (followUp.Id, followUp.ConsultationId, consultId, pNo, patientName, followUp.ScheduledDate);
        }

        public string CreatePatientSatisfactionToken(int followUpId, string consultId, string pNo, DateTime expiresOnUtc)
        {
            var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
            var normalizedPNo = NormalizeRequired(pNo, "PNo");
            var expiry = expiresOnUtc.ToUniversalTime();

            var payload = $"{PatientSatisfactionTokenPurpose}|{followUpId}|{normalizedConsultId}|{normalizedPNo}|{expiry:O}";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var signatureBytes = SHA256.HashData(payloadBytes);

            return $"{Convert.ToBase64String(payloadBytes)}.{Convert.ToBase64String(signatureBytes)}";
        }

        public (int followUpId, string consultId, string pNo)? ValidatePatientSatisfactionToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var parts = token.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return null;
            }

            try
            {
                var payloadBytes = Convert.FromBase64String(parts[0]);
                var signatureBytes = Convert.FromBase64String(parts[1]);
                var expectedSignature = SHA256.HashData(payloadBytes);

                if (!CryptographicOperations.FixedTimeEquals(signatureBytes, expectedSignature))
                {
                    return null;
                }

                var payload = Encoding.UTF8.GetString(payloadBytes);
                var segments = payload.Split('|', StringSplitOptions.None);
                if (segments.Length != 5 || !string.Equals(segments[0], PatientSatisfactionTokenPurpose, StringComparison.Ordinal))
                {
                    return null;
                }

                if (!int.TryParse(segments[1], out var followUpId) || followUpId <= 0)
                {
                    return null;
                }

                var consultId = NormalizeOptional(segments[2]);
                var pNo = NormalizeOptional(segments[3]);

                if (string.IsNullOrWhiteSpace(consultId) || string.IsNullOrWhiteSpace(pNo))
                {
                    return null;
                }

                if (!DateTime.TryParse(segments[4], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresOnUtc))
                {
                    return null;
                }

                if (DateTime.UtcNow > expiresOnUtc)
                {
                    return null;
                }

                return (followUpId, consultId, pNo);
            }
            catch
            {
                return null;
            }
        }

        public AestheticFollowUp SubmitPatientSatisfaction(int followUpId, string consultId, string pNo, int patientSatisfactionScore, string? outcome)
        {
            if (patientSatisfactionScore < 1 || patientSatisfactionScore > 10)
            {
                throw new InvalidOperationException("Patient satisfaction score must be between 1 and 10.");
            }

            var normalizedConsultId = NormalizeRequired(consultId, "ConsultId");
            var normalizedPNo = NormalizeRequired(pNo, "PNo");
            var normalizedOutcome = NormalizeOptional(outcome);

            var followUp = dbContext.Set<AestheticFollowUp>()
                .Include(x => x.Consultation)
                .ThenInclude(c => c.Patient)
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == followUpId)
                ?? throw new KeyNotFoundException($"Follow-up not found: {followUpId}");

            var resolvedPNo = ResolveLegacyPNo(followUp.Consultation.PatientId, followUp.Consultation?.Patient?.Pno);
            var resolvedConsultId = ResolveLegacyConsultId(followUp.Consultation.ConsultationDate, resolvedPNo, null);

            if (string.IsNullOrWhiteSpace(resolvedConsultId) || string.IsNullOrWhiteSpace(resolvedPNo))
            {
                throw new InvalidOperationException("ConsultId and PNo are required for patient satisfaction submission.");
            }

            if (!string.Equals(resolvedConsultId, normalizedConsultId, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(resolvedPNo, normalizedPNo, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Invalid satisfaction submission context for this follow-up.");
            }

            followUp.PatientSatisfactionScore = patientSatisfactionScore;
            followUp.PatientSatisfactionConsultId = normalizedConsultId;
            followUp.PatientSatisfactionPNo = normalizedPNo;
            followUp.PatientSatisfactionSubmittedOn = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(normalizedOutcome))
            {
                followUp.Outcome = normalizedOutcome;
            }

            if (!followUp.IsCompleted)
            {
                followUp.IsCompleted = true;
                followUp.CompletedDate = DateTime.UtcNow;
            }

            followUp.UpdatedDate = DateTime.UtcNow;
            dbContext.SaveChanges();
            return followUp;
        }

        private void ValidateConsultationSafetyRequirements(AestheticConsultation consultation, string? consultId, string? pNo)
        {
            if (consultation.PatientId <= 0)
            {
                throw new InvalidOperationException("Patient is required.");
            }

            if (consultation.ConsultationDate == default)
            {
                throw new InvalidOperationException("Consultation date is required.");
            }

            if (consultation.ConsultationDate > DateTime.UtcNow.AddMinutes(5))
            {
                throw new InvalidOperationException("Consultation date cannot be in the future.");
            }

            if (string.IsNullOrWhiteSpace(consultation.ProcedureType))
            {
                throw new InvalidOperationException("Procedure type is required.");
            }

            if (string.IsNullOrWhiteSpace(consultation.Provider))
            {
                throw new InvalidOperationException("Provider is required.");
            }

            if (string.IsNullOrWhiteSpace(consultation.Allergies))
            {
                throw new InvalidOperationException("Allergy information is required for safety.");
            }

            if (string.IsNullOrWhiteSpace(consultation.CurrentMedications))
            {
                throw new InvalidOperationException("Current medications are required for safety.");
            }

            if (string.IsNullOrWhiteSpace(consultation.RisksAndComplications))
            {
                throw new InvalidOperationException("Risks and complications assessment is required.");
            }

            if (string.IsNullOrWhiteSpace(consultation.PostTreatmentInstructions))
            {
                throw new InvalidOperationException("Post-treatment instructions are required.");
            }

            var requiresSignedConsent = IsConsentRequiredProcedure(consultation.ProcedureType);
            if (!requiresSignedConsent)
            {
                return;
            }

            var resolvedPNo = ResolveLegacyPNo(consultation.PatientId, pNo);
            var resolvedConsultId = ResolveLegacyConsultId(consultation.ConsultationDate, resolvedPNo, consultId);

            if (string.IsNullOrWhiteSpace(resolvedConsultId) || string.IsNullOrWhiteSpace(resolvedPNo))
            {
                throw new InvalidOperationException("ConsultId and PNo are required to validate signed consent for this procedure.");
            }

            var hasValidConsent = GetLatestSignedConsent(resolvedConsultId, resolvedPNo, consultation.ProcedureType) != null;
            if (!hasValidConsent)
            {
                throw new InvalidOperationException("Signed consent is required before saving this procedure record.");
            }
        }

        private static bool IsConsentRequiredProcedure(string procedureType)
        {
            return procedureType.Equals("Botox", StringComparison.OrdinalIgnoreCase)
                   || procedureType.Equals("Laser", StringComparison.OrdinalIgnoreCase)
                   || procedureType.Equals("Spa", StringComparison.OrdinalIgnoreCase)
                   || procedureType.Equals("Procedures", StringComparison.OrdinalIgnoreCase);
        }

        private void AutoScheduleDefaultFollowUp(int consultationId)
        {
            var alreadyScheduled = dbContext.Set<AestheticFollowUp>().Any(x => x.ConsultationId == consultationId && !x.IsCompleted);
            if (alreadyScheduled)
            {
                return;
            }

            ScheduleFollowUp(consultationId, 14, true, "Auto-scheduled default follow-up.");
        }

        private void ApplyConsentStatus(AestheticConsultation consultation, string? consultId, string? pNo)
        {
            var resolvedPNo = ResolveLegacyPNo(consultation.PatientId, pNo);
            var resolvedConsultId = ResolveLegacyConsultId(consultation.ConsultationDate, resolvedPNo, consultId);

            if (string.IsNullOrWhiteSpace(resolvedConsultId) || string.IsNullOrWhiteSpace(resolvedPNo) || string.IsNullOrWhiteSpace(consultation.ProcedureType))
            {
                consultation.ConsentGiven = false;
                consultation.InformationAccepted = false;
                consultation.ConsentDate = null;
                return;
            }

            var signedConsent = GetLatestSignedConsent(resolvedConsultId, resolvedPNo, consultation.ProcedureType);
            consultation.ConsentGiven = signedConsent != null;
            consultation.InformationAccepted = signedConsent != null;
            consultation.ConsentDate = signedConsent?.SignedDate;
            if (signedConsent != null && string.IsNullOrWhiteSpace(consultation.ConsentNotes))
            {
                consultation.ConsentNotes = signedConsent.Notes;
            }
        }

        private void UpdateConsultationConsentFields(string consultId, string pNo, string procedureType, AestheticSignedConsent signedConsent)
        {
            var consultation = dbContext.AestheticConsultations
                .FirstOrDefault(x => x.PatientId == signedConsent.PatientId
                    && x.ProcedureType == procedureType
                    && x.ConsultationDate.Date == signedConsent.SignedDate.Date);

            if (consultation == null)
            {
                consultation = dbContext.AestheticConsultations
                    .Include(x => x.Patient)
                    .AsEnumerable()
                    .FirstOrDefault(x => string.Equals(x.Patient?.Pno, pNo, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(x.ProcedureType, procedureType, StringComparison.OrdinalIgnoreCase)
                        && x.ConsultationDate.Date == signedConsent.SignedDate.Date);
            }

            if (consultation == null)
            {
                return;
            }

            consultation.ConsentGiven = true;
            consultation.InformationAccepted = true;
            consultation.ConsentDate = signedConsent.SignedDate;
            consultation.ConsentNotes = signedConsent.Notes;
            consultation.UpdatedDate = DateTime.UtcNow;
            dbContext.SaveChanges();
        }

        private void ResetConsultationConsentFields(string consultId, string pNo, string procedureType)
        {
            var remainingConsent = GetLatestSignedConsent(consultId, pNo, procedureType);
            if (remainingConsent != null)
            {
                UpdateConsultationConsentFields(consultId, pNo, procedureType, remainingConsent);
                return;
            }

            var consultations = dbContext.AestheticConsultations
                .Include(x => x.Patient)
                .AsEnumerable()
                .Where(x => string.Equals(x.Patient?.Pno, pNo, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(x.ProcedureType, procedureType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var consultation in consultations)
            {
                consultation.ConsentGiven = false;
                consultation.InformationAccepted = false;
                consultation.ConsentDate = null;
                consultation.ConsentNotes = null;
                consultation.UpdatedDate = DateTime.UtcNow;
            }

            dbContext.SaveChanges();
        }

        private string? ResolveLegacyConsultId(AestheticConsultation? consultation)
        {
            if (consultation == null)
            {
                return null;
            }

            var pNo = consultation.Patient?.Pno;

            if (string.IsNullOrWhiteSpace(pNo))
            {
                pNo = dbContext.HPatients
                    .Where(x => x.PSurName == consultation.Patient.LastName && x.PFirstname == consultation.Patient.FirstName)
                    .Select(x => x.Pno)
                    .FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(pNo))
            {
                return null;
            }

            var consultId = dbContext.HRecords
                .Where(x => x.PNo == pNo && x.RecDate.Date == consultation.ConsultationDate.Date)
                .OrderByDescending(x => x.EntryTime)
                .Select(x => x.ConsultId)
                .FirstOrDefault();

            return string.IsNullOrWhiteSpace(consultId) ? null : consultId;
        }

        private bool IsConsultIdReferenced(string consultId)
        {
            return dbContext.HConsultings.Any(x => x.ConsultId == consultId)
                   || dbContext.HDentals.Any(x => x.ConsultId == consultId)
                   || dbContext.HDentalTreats.Any(x => x.ConsultId == consultId)
                   || dbContext.Billings.Any(x => x.billNO == consultId)
                   || dbContext.BillAccums.Any(x => x.consultID == consultId);
        }

        private void SyncLegacyConsultingOnCreate(AestheticConsultation consultation, string? consultId, string? pNo, string? services)
        {
            var resolvedPNo = ResolveLegacyPNo(consultation.PatientId, pNo);
            var resolvedConsultId = ResolveLegacyConsultId(consultation.ConsultationDate, resolvedPNo, consultId);

            if (string.IsNullOrWhiteSpace(resolvedConsultId) || string.IsNullOrWhiteSpace(resolvedPNo))
                return;

            var existing = dbContext.HConsultings.FirstOrDefault(x => x.ConsultId == resolvedConsultId);
            if (existing != null)
            {
                existing.Services = MergeServices(existing.Services, services, consultation.ProcedureType);
                existing.EditDate = DateTime.UtcNow;
                existing.EditTime = DateTime.UtcNow;
                dbContext.SaveChanges();
                return;
            }

            var now = DateTime.UtcNow;
            var consulting = new Models.Legacy.HConsulting
            {
                ConsultId = resolvedConsultId,
                PNo = resolvedPNo,
                CDate = consultation.ConsultationDate == default ? now : consultation.ConsultationDate,
                CTime = now,
                Services = services,
                TreatedBy = string.IsNullOrWhiteSpace(consultation.Provider) ? "SYSTEM" : consultation.Provider,
                ClientCat = "PRIVATE",
                IsLatest = true
            };

            dbContext.HConsultings.Add(consulting);
            dbContext.SaveChanges();
        }

        private void SyncLegacyConsultingOnUpdate(AestheticConsultation consultation, string? consultId, string? pNo, string? services)
        {
            var resolvedPNo = ResolveLegacyPNo(consultation.PatientId, pNo);
            var resolvedConsultId = ResolveLegacyConsultId(consultation.ConsultationDate, resolvedPNo, consultId);

            if (string.IsNullOrWhiteSpace(resolvedConsultId))
                return;

            var existing = dbContext.HConsultings.FirstOrDefault(x => x.ConsultId == resolvedConsultId);
            if (existing == null)
                return;

            existing.Services = MergeServices(existing.Services, services, consultation.ProcedureType);
            existing.EditDate = DateTime.UtcNow;
            existing.EditTime = DateTime.UtcNow;
            dbContext.SaveChanges();
        }

        private static string? MergeServices(string? existingServices, string? incomingServices, string? procedureType)
        {
            if (string.IsNullOrWhiteSpace(incomingServices))
                return existingServices;

            var isLaserOrBotox = string.Equals(procedureType, "Laser", StringComparison.OrdinalIgnoreCase)
                                 || string.Equals(procedureType, "Botox", StringComparison.OrdinalIgnoreCase);

            if (!isLaserOrBotox || string.IsNullOrWhiteSpace(existingServices))
                return incomingServices;

            var incomingTrimmed = incomingServices.Trim();
            if (existingServices.Contains(incomingTrimmed, StringComparison.OrdinalIgnoreCase))
                return existingServices;

            return $"{existingServices.TrimEnd()}\n{incomingTrimmed}";
        }

        private string? ResolveLegacyPNo(int patientId, string? providedPNo)
        {
            if (!string.IsNullOrWhiteSpace(providedPNo))
                return providedPNo;

            return dbContext.AestheticPatients
                .Where(x => x.Id == patientId)
                .Select(x => x.Pno)
                .FirstOrDefault();
        }

        private string? ResolveLegacyConsultId(DateTime consultationDate, string? pNo, string? providedConsultId)
        {
            if (!string.IsNullOrWhiteSpace(providedConsultId))
                return providedConsultId;

            if (string.IsNullOrWhiteSpace(pNo))
                return null;

            var consultId = dbContext.HRecords
                .Where(x => x.PNo == pNo && x.RecDate.Date == consultationDate.Date)
                .OrderByDescending(x => x.EntryTime)
                .Select(x => x.ConsultId)
                .FirstOrDefault();

            return string.IsNullOrWhiteSpace(consultId) ? null : consultId;
        }

        public IEnumerable<ProcedureRevenueMetric> GetRevenuePerProcedure(DateTime? from = null, DateTime? to = null)
        {
            var query = dbContext.AestheticConsultations
                .Include(x => x.Patient)
                .AsSingleQuery()
                .AsQueryable();

            if (from.HasValue)
            {
                var fromDate = from.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date >= fromDate);
            }

            if (to.HasValue)
            {
                var toDate = to.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date <= toDate);
            }

            var consultIds = query
                .Select(x => x.ConsultationDate.Date)
                .Distinct()
                .ToList();

            var billingQuery = dbContext.Billings.AsQueryable();
            if (from.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(from.Value.Date);
                billingQuery = billingQuery.Where(x => x.bDate >= fromDateOnly);
            }

            if (to.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(to.Value.Date);
                billingQuery = billingQuery.Where(x => x.bDate <= toDateOnly);
            }

            var billingMap = billingQuery
                .GroupBy(x => x.billNO)
                .ToDictionary(x => x.Key, x => x.Sum(i => i.AmountBilled ?? 0));

            return dbContext.AestheticConsultations
                .Include(x => x.Patient)
                .AsSingleQuery()
                .Where(x => !from.HasValue || x.ConsultationDate.Date >= from.Value.Date)
                .Where(x => !to.HasValue || x.ConsultationDate.Date <= to.Value.Date)
                .AsEnumerable()
                .GroupBy(x => string.IsNullOrWhiteSpace(x.ProcedureType) ? "Unknown" : x.ProcedureType)
                .Select(group => new ProcedureRevenueMetric
                {
                    ProcedureType = group.Key,
                    ConsultationCount = group.Count(),
                    Revenue = group.Sum(c =>
                    {
                        var consultId = ResolveLegacyConsultId(c.ConsultationDate, c.Patient?.Pno, null);
                        return !string.IsNullOrWhiteSpace(consultId) && billingMap.TryGetValue(consultId, out var amount)
                            ? amount
                            : 0m;
                    })
                })
                .OrderByDescending(x => x.Revenue)
                .ThenBy(x => x.ProcedureType)
                .ToList();
        }

        public IEnumerable<ProductUsageMetric> GetMostUsedProducts(int top = 10, DateTime? from = null, DateTime? to = null)
        {
            var query = dbContext.Set<Models.Shop.ProcedureProductUsage>()
                .Include(x => x.Product)
                .AsSingleQuery()
                .AsQueryable();

            if (from.HasValue)
            {
                var fromDate = from.Value.Date;
                query = query.Where(x => x.UsedOn.Date >= fromDate);
            }

            if (to.HasValue)
            {
                var toDate = to.Value.Date;
                query = query.Where(x => x.UsedOn.Date <= toDate);
            }

            return query
                .AsEnumerable()
                .GroupBy(x => new { x.ProductId, ProductName = x.Product.Name })
                .Select(g => new ProductUsageMetric
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantityUsed = g.Sum(x => x.QuantityUsed)
                })
                .OrderByDescending(x => x.TotalQuantityUsed)
                .ThenBy(x => x.ProductName)
                .Take(top)
                .ToList();
        }

        public ComplicationRateMetric GetComplicationRate(DateTime? from = null, DateTime? to = null)
        {
            var query = dbContext.Set<AestheticFollowUp>()
                .AsQueryable()
                .Where(x => x.IsCompleted);

            if (from.HasValue)
            {
                var fromDate = from.Value.Date;
                query = query.Where(x => x.CompletedDate.HasValue && x.CompletedDate.Value.Date >= fromDate);
            }

            if (to.HasValue)
            {
                var toDate = to.Value.Date;
                query = query.Where(x => x.CompletedDate.HasValue && x.CompletedDate.Value.Date <= toDate);
            }

            var completed = query.ToList();
            var total = completed.Count;
            var complications = completed.Count(x =>
                (!string.IsNullOrWhiteSpace(x.Outcome) && x.Outcome.Contains("complication", StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrWhiteSpace(x.Notes) && x.Notes.Contains("complication", StringComparison.OrdinalIgnoreCase)));

            var rate = total == 0 ? 0 : Math.Round((decimal)complications * 100m / total, 2);

            return new ComplicationRateMetric
            {
                TotalCompletedFollowUps = total,
                ComplicationCases = complications,
                ComplicationRatePercent = rate
            };
        }

        public PatientRetentionMetric GetPatientRetention(DateTime? from = null, DateTime? to = null)
        {
            var query = dbContext.AestheticConsultations.AsQueryable();

            if (from.HasValue)
            {
                var fromDate = from.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date >= fromDate);
            }

            if (to.HasValue)
            {
                var toDate = to.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date <= toDate);
            }

            var grouped = query
                .AsEnumerable()
                .GroupBy(x => x.PatientId)
                .Select(g => g.Count())
                .ToList();

            var totalPatients = grouped.Count;
            var returning = grouped.Count(x => x > 1);
            var rate = totalPatients == 0 ? 0 : Math.Round((decimal)returning * 100m / totalPatients, 2);

            return new PatientRetentionMetric
            {
                TotalPatients = totalPatients,
                ReturningPatients = returning,
                RetentionRatePercent = rate
            };
        }

        public BeforeAfterOutcomeMetric GetBeforeAfterOutcomeTracking(DateTime? from = null, DateTime? to = null)
        {
            var query = dbContext.AestheticConsultations
                .Include(x => x.Photos)
                .AsSingleQuery()
                .AsQueryable();

            if (from.HasValue)
            {
                var fromDate = from.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date >= fromDate);
            }

            if (to.HasValue)
            {
                var toDate = to.Value.Date;
                query = query.Where(x => x.ConsultationDate.Date <= toDate);
            }

            var consultations = query.ToList();
            var withPhotos = consultations.Count(x => x.Photos.Count > 0);
            var withBeforeAfter = consultations.Count(x =>
                x.Photos.Any(p => !string.IsNullOrWhiteSpace(p.Type) && p.Type.Equals("Before", StringComparison.OrdinalIgnoreCase))
                && x.Photos.Any(p => !string.IsNullOrWhiteSpace(p.Type) && p.Type.Equals("After", StringComparison.OrdinalIgnoreCase)));

            var rate = withPhotos == 0 ? 0 : Math.Round((decimal)withBeforeAfter * 100m / withPhotos, 2);

            return new BeforeAfterOutcomeMetric
            {
                TotalConsultationsWithPhotos = withPhotos,
                ConsultationsWithBeforeAfter = withBeforeAfter,
                BeforeAfterRatePercent = rate
            };
        }

        private static string NormalizeRequired(string? value, string fieldName)
        {
            var normalized = NormalizeOptional(value);
            return !string.IsNullOrWhiteSpace(normalized)
                ? normalized
                : throw new InvalidOperationException($"{fieldName} is required.");
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
