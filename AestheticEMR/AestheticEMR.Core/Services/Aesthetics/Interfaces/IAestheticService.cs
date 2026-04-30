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
        IEnumerable<AestheticConsultation> GetConsultationsByProcedure(string procedureType);
        IEnumerable<AestheticConsultation> GetLaserSessions();
        AestheticConsultation AddConsultation(AestheticConsultation consultation, string? consultId = null, string? pNo = null, string? services = null);
        AestheticConsultation UpdateConsultation(AestheticConsultation consultation, string? consultId = null, string? pNo = null, string? services = null);
        void DeleteConsultation(int consultationId);
        AestheticConsultation? GetConsultationById(int consultationId);
        IEnumerable<AestheticPhoto> GetPhotos();
        AestheticPhoto? GetPhotoById(int photoId);
        IEnumerable<AestheticPhoto> GetPhotosForConsultation(int consultationId);
        AestheticPhoto AddPhoto(AestheticPhoto photo);
        AestheticPhoto UpdatePhoto(AestheticPhoto photo);
        void DeletePhoto(int photoId);
    }
}
