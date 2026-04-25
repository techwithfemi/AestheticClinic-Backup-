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

        public AestheticConsultation AddConsultation(AestheticConsultation consultation)
        {
            consultation.CreatedDate = DateTime.UtcNow;
            consultation.UpdatedDate = DateTime.UtcNow;
            dbContext.AestheticConsultations.Add(consultation);
            dbContext.SaveChanges();
            return consultation;
        }

        public AestheticConsultation UpdateConsultation(AestheticConsultation consultation)
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
            existing.ProcedureDescription = consultation.ProcedureDescription;
            existing.RisksAndComplications = consultation.RisksAndComplications;
            existing.PostTreatmentInstructions = consultation.PostTreatmentInstructions;
            existing.SkinAssessment = consultation.SkinAssessment;
            existing.TreatmentPlan = consultation.TreatmentPlan;
            existing.CurrentMedications = consultation.CurrentMedications;
            existing.Allergies = consultation.Allergies;
            existing.DeviceSettings = consultation.DeviceSettings;
            existing.UpdatedDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            return existing;
        }

        public void DeleteConsultation(int consultationId)
        {
            var consultation = dbContext.AestheticConsultations
                .Include(c => c.Photos)
                .FirstOrDefault(c => c.Id == consultationId);

            if (consultation == null)
                throw new KeyNotFoundException($"Consultation not found: {consultationId}");

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

            existing.ConsultationId = photo.ConsultationId;
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
    }
}
