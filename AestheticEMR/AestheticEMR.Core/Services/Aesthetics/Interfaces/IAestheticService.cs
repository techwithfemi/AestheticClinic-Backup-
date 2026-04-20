// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Aesthetic;

namespace AestheticEMR.Core.Services.Aesthetics
{
    public interface IAestheticService
    {
        IEnumerable<AestheticPatient> GetPatients();
        AestheticPatient? GetPatientById(int id);
        AestheticPatient AddPatient(AestheticPatient patient);
        AestheticPatient UpdatePatient(AestheticPatient patient);
        IEnumerable<AestheticConsultation> GetConsultationsForPatient(int patientId);
        IEnumerable<AestheticConsultation> GetLaserSessions();
        AestheticConsultation AddConsultation(AestheticConsultation consultation);
        AestheticConsultation? GetConsultationById(int consultationId);
        IEnumerable<AestheticPhoto> GetPhotosForConsultation(int consultationId);
        AestheticPhoto AddPhoto(AestheticPhoto photo);
    }
}
