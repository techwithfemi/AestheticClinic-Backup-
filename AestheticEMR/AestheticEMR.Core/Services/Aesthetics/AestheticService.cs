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
            dbContext.AestheticConsultations.Add(consultation);
            dbContext.SaveChanges();

            SyncLegacyConsultingOnCreate(consultation, consultId, pNo, services);
            return consultation;
        }

        public AestheticConsultation UpdateConsultation(AestheticConsultation consultation, string? consultId = null, string? pNo = null, string? services = null)
        {
            var existing = dbContext.AestheticConsultations.Find(consultation.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Consultation not found: {consultation.Id}");

            existing.PatientId = consultation.PatientId;
            existing.ConsultationDate = consultation.ConsultationDate;
            existing.ProcedureType = consultation.ProcedureType;
            existing.Provider = consultation.Provider;
            existing.ConsentGiven = consultation.ConsentGiven;
            existing.InformationAccepted = consultation.InformationAccepted;
            existing.ConsentDate = consultation.ConsentDate;
            existing.ConsentNotes = consultation.ConsentNotes;
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

            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();

            SyncLegacyConsultingOnUpdate(existing, consultId, pNo, services);
            return existing;
        }

        public void DeleteConsultation(int consultationId)
        {
            var consultation = dbContext.AestheticConsultations
                .Include(c => c.Photos)
                .Include(c => c.Patient)
                .FirstOrDefault(c => c.Id == consultationId);

            if (consultation == null)
                throw new KeyNotFoundException($"Consultation not found: {consultationId}");

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

        public AestheticPhoto UpdatePhoto(AestheticPhoto photo)
        {
            var existing = dbContext.AestheticPhotos.Find(photo.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Photo not found: {photo.Id}");

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

        public void DeletePhoto(int photoId)
        {
            var photo = dbContext.AestheticPhotos.Find(photoId);
            if (photo == null)
                throw new KeyNotFoundException($"Photo not found: {photoId}");

            dbContext.AestheticPhotos.Remove(photo);
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
    }
}
