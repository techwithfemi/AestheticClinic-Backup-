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
        IEnumerable<AestheticConsentTemplate> GetConsentTemplates(string? procedureType = null, bool includeInactive = false);
        AestheticConsentTemplate? GetConsentTemplateById(int id);
        AestheticConsentTemplate AddConsentTemplate(AestheticConsentTemplate template);
        AestheticConsentTemplate UpdateConsentTemplate(AestheticConsentTemplate template);
        void DeleteConsentTemplate(int id);
        AestheticConsentStatus GetConsentStatus(string consultId, string pNo, string procedureType);
        IEnumerable<AestheticSignedConsent> GetSignedConsents(string? consultId = null, string? pNo = null, string? procedureType = null, bool includeVoided = false);
        AestheticSignedConsent? GetLatestSignedConsent(string consultId, string pNo, string procedureType);
        AestheticSignedConsent SignConsent(int? patientId, string consultId, string pNo, string procedureType, int consentTemplateId, string signatureName, string? witnessedBy, string? signedBy, string? notes, byte[]? signatureImage, string? signatureImagePath);
        AestheticSignedConsent MarkConsentViewed(int consentId, string doctorViewedBy);
        AestheticSignedConsent VoidConsent(int consentId, string voidReason, string voidedBy);
    }
}
