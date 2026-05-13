// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Aesthetic;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Aesthetics
{
    public class AestheticService(ApplicationDbContext dbContext) : IAestheticService
    {
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

            dbContext.AestheticConsultations.Add(consultation);
            dbContext.SaveChanges();

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
