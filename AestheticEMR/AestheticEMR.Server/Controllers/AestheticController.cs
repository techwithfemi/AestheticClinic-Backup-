// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Server.Configuration;
using AestheticEMR.Server.Services.Email;
using AestheticEMR.Server.ViewModels.Aesthetic;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AestheticController : BaseApiController
    {
        private readonly IAestheticService _aestheticService;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly AppSettings _appSettings;
        private const string UploadFolder = "uploads/aesthetic";
        private const string ConsentUploadFolder = "uploads/aesthetic/consents";
        private const string PatientSatisfactionPath = "/aesthetics/satisfaction";

        public AestheticController(
            ILogger<AestheticController> logger,
            IMapper mapper,
            IAestheticService aestheticService,
            IWebHostEnvironment environment,
            IEmailSender emailSender,
            IOptions<AppSettings> appSettings)
            : base(logger, mapper)
        {
            _aestheticService = aestheticService;
            _environment = environment;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
        }

        [HttpPut("consents/{consentId:int}")]
        [ProducesResponseType(typeof(AestheticSignedConsentVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSignedConsent(int consentId, [FromBody] UpdateAestheticConsentVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _aestheticService.GetSignedConsents(includeVoided: true).FirstOrDefault(x => x.Id == consentId);
            if (existing == null)
                return NotFound(consentId);

            try
            {
                var signatureBytes = ToBytes(model.SignatureImageBase64);
                var signatureImagePath = SaveConsentSignature(signatureBytes, existing.ConsultId ?? string.Empty, existing.PNo ?? string.Empty, existing.ProcedureType ?? string.Empty);
                var updated = _aestheticService.UpdateSignedConsent(consentId, model.PatientId, model.ConsentTemplateId, model.SignatureName, model.WitnessedBy, model.Notes, signatureBytes, signatureImagePath, GetCurrentUserId() ?? "SYSTEM");
                return Ok(ApplyConsentImageUrl(_mapper.Map<AestheticSignedConsentVM>(updated)));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Update blocked for consent {ConsentId}", consentId);
                AddModelError(ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consent {ConsentId}", consentId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("patients")]
        [ProducesResponseType(typeof(IEnumerable<AestheticPatientVM>), StatusCodes.Status200OK)]
        public IActionResult GetPatients()
        {
            var patients = _aestheticService.GetPatients();
            return Ok(_mapper.Map<IEnumerable<AestheticPatientVM>>(patients));
        }

        [HttpGet("vwh-records")]
        [ProducesResponseType(typeof(IEnumerable<VwhRecordSummaryVM>), StatusCodes.Status200OK)]
        public IActionResult GetVwhRecords()
        {
            var records = _aestheticService.GetVwhRecords();
            return Ok(_mapper.Map<IEnumerable<VwhRecordSummaryVM>>(records));
        }

        [HttpGet("consultations")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsultationVM>), StatusCodes.Status200OK)]
        public IActionResult GetConsultations()
        {
            var consultations = _aestheticService.GetConsultations();
            return Ok(_mapper.Map<IEnumerable<AestheticConsultationVM>>(consultations));
        }

        [HttpGet("patients/{id}")]
        [ProducesResponseType(typeof(AestheticPatientVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPatient(int id)
        {
            var patient = _aestheticService.GetPatientById(id);
            if (patient == null) return NotFound(id);
            return Ok(_mapper.Map<AestheticPatientVM>(patient));
        }

        [HttpPost("patients")]
        public IActionResult CreatePatient([FromBody] AestheticPatientVM patientVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = _mapper.Map<Core.Models.Aesthetic.AestheticPatient>(patientVM);
            var created = _aestheticService.AddPatient(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = created.Id }, _mapper.Map<AestheticPatientVM>(created));
        }

        [HttpPut("patients/{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] AestheticPatientVM patientVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != patientVM.Id)
                return BadRequest("Patient id mismatch");

            var patient = _mapper.Map<Core.Models.Aesthetic.AestheticPatient>(patientVM);
            var updated = _aestheticService.UpdatePatient(patient);
            return Ok(_mapper.Map<AestheticPatientVM>(updated));
        }

        [HttpGet("patients/{patientId}/consultations")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsultationVM>), StatusCodes.Status200OK)]
        public IActionResult GetPatientConsultations(int patientId)
        {
            var consultations = _aestheticService.GetConsultationsForPatient(patientId);
            return Ok(_mapper.Map<IEnumerable<AestheticConsultationVM>>(consultations));
        }

        [HttpPost("consultations")]
        public IActionResult CreateConsultation([FromBody] AestheticConsultationVM consultationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
                consultation.Provider = GetCurrentUserId();
                var created = _aestheticService.AddConsultation(consultation, consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return CreatedAtAction(nameof(GetConsultation), new { consultationId = created.Id }, _mapper.Map<AestheticConsultationVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consultation");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("consultations/{consultationId}")]
        public IActionResult GetConsultation(int consultationId)
        {
            var consultation = _aestheticService.GetConsultationById(consultationId);
            if (consultation == null) return NotFound(consultationId);
            return Ok(_mapper.Map<AestheticConsultationVM>(consultation));
        }

        [HttpGet("consultations/{consultationId}/photos")]
        [ProducesResponseType(typeof(IEnumerable<AestheticPhotoVM>), StatusCodes.Status200OK)]
        public IActionResult GetConsultationPhotos(int consultationId)
        {
            var photos = _aestheticService.GetPhotosForConsultation(consultationId);
            return Ok(photos.Select(ToPhotoViewModel));
        }

        [HttpPost("consultations/{consultationId}/photos")]
        public IActionResult AddConsultationPhoto(int consultationId, [FromBody] AestheticPhotoVM photoVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (consultationId != photoVM.ConsultationId)
                return BadRequest("Consultation id mismatch");

            var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
            var created = _aestheticService.AddPhoto(photo);
            return CreatedAtAction(nameof(GetConsultationPhotos), new { consultationId }, ToPhotoViewModel(created));
        }

        [HttpGet("consultations/botox")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsultationVM>), StatusCodes.Status200OK)]
        public IActionResult GetBotoxConsultations()
        {
            var consultations = _aestheticService.GetConsultationsByProcedure("Botox");
            return Ok(_mapper.Map<IEnumerable<AestheticConsultationVM>>(consultations));
        }

        [HttpPost("consultations/botox")]
        [ProducesResponseType(typeof(AestheticConsultationVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateBotoxConsultation([FromBody] AestheticConsultationVM consultationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            consultationVM.ProcedureType = "Botox";
            try
            {
                var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
                consultation.Provider = GetCurrentUserId();
                var created = _aestheticService.AddConsultation(consultation, consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return CreatedAtAction(nameof(GetConsultation), new { consultationId = created.Id }, _mapper.Map<AestheticConsultationVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Botox consultation");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("consultations/laser")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsultationVM>), StatusCodes.Status200OK)]
        public IActionResult GetLaserConsultations()
        {
            var consultations = _aestheticService.GetConsultationsByProcedure("Laser");
            return Ok(_mapper.Map<IEnumerable<AestheticConsultationVM>>(consultations));
        }

        [HttpPost("consultations/laser")]
        [ProducesResponseType(typeof(AestheticConsultationVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateLaserConsultation([FromBody] AestheticConsultationVM consultationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            consultationVM.ProcedureType = "Laser";
            try
            {
                var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
                consultation.Provider = GetCurrentUserId();
                var created = _aestheticService.AddConsultation(consultation, consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return CreatedAtAction(nameof(GetConsultation), new { consultationId = created.Id }, _mapper.Map<AestheticConsultationVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Laser consultation");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("consultations/spa")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsultationVM>), StatusCodes.Status200OK)]
        public IActionResult GetSpaConsultations()
        {
            var consultations = _aestheticService.GetConsultationsByProcedure("Spa");
            return Ok(_mapper.Map<IEnumerable<AestheticConsultationVM>>(consultations));
        }

        [HttpPost("consultations/spa")]
        [ProducesResponseType(typeof(AestheticConsultationVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateSpaConsultation([FromBody] AestheticConsultationVM consultationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            consultationVM.ProcedureType = "Spa";
            try
            {
                var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
                consultation.Provider = GetCurrentUserId();
                var created = _aestheticService.AddConsultation(consultation, consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return CreatedAtAction(nameof(GetConsultation), new { consultationId = created.Id }, _mapper.Map<AestheticConsultationVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Spa consultation");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("consultations/{consultationId}")]
        [ProducesResponseType(typeof(AestheticConsultationVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateConsultation(int consultationId, [FromBody] AestheticConsultationVM consultationVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (consultationId != consultationVM.Id)
                return BadRequest("Consultation id mismatch");

            var existing = _aestheticService.GetConsultationById(consultationId);
            if (existing == null)
                return NotFound(consultationId);

            try
            {
                var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
                consultation.Provider = GetCurrentUserId();
                var updated = _aestheticService.UpdateConsultation(consultation, GetCurrentUserId(), consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return Ok(_mapper.Map<AestheticConsultationVM>(updated));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Update blocked for consultation {ConsultationId}", consultationId);
                AddModelError(ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consultation {ConsultationId}", consultationId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("consultations/{consultationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteConsultation(int consultationId)
        {
            var existing = _aestheticService.GetConsultationById(consultationId);
            if (existing == null)
                return NotFound(consultationId);

            try
            {
                foreach (var photo in existing.Photos)
                {
                    DeletePhysicalFile(photo.FilePath);
                }

                _aestheticService.DeleteConsultation(consultationId, GetCurrentUserId());
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Delete blocked for consultation {ConsultationId}", consultationId);
                AddModelError(ex.Message);
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Delete blocked for consultation {ConsultationId}", consultationId);
                AddModelError(ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consultation {ConsultationId}", consultationId);
                AddModelError("Unable to delete consultation");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("photos")]
        [ProducesResponseType(typeof(IEnumerable<AestheticPhotoVM>), StatusCodes.Status200OK)]
        public IActionResult GetPhotos()
        {
            var photos = _aestheticService.GetPhotos();
            return Ok(photos.Select(ToPhotoViewModel));
        }

        [HttpPost("photos")]
        [ProducesResponseType(typeof(AestheticPhotoVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePhoto([FromBody] AestheticPhotoVM photoVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
            var created = _aestheticService.AddPhoto(photo);
            return CreatedAtAction(nameof(GetPhotos), new { id = created.Id }, ToPhotoViewModel(created));
        }

        [HttpPost("photos/upload")]
        [ProducesResponseType(typeof(AestheticPhotoVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UploadPhoto([FromForm] AestheticPhotoUploadVM uploadVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsSupportedImage(uploadVM.File))
                return BadRequest("Only non-empty image files are allowed.");

            var consultation = _aestheticService.GetConsultationById(uploadVM.ConsultationId);
            if (consultation == null)
                return NotFound(uploadVM.ConsultationId);

            var savedPath = SaveUploadedFile(uploadVM.File);
            var photoVM = new AestheticPhotoVM
            {
                ConsultationId = uploadVM.ConsultationId,
                ConsultId = consultation.Photos.FirstOrDefault()?.ConsultId,
                PNo = consultation.Patient?.Pno,
                FileName = uploadVM.File.FileName,
                Type = uploadVM.Type,
                Url = savedPath
            };

            var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
            var created = _aestheticService.AddPhoto(photo);
            return CreatedAtAction(nameof(GetPhotos), new { id = created.Id }, ToPhotoViewModel(created));
        }

        [HttpPut("photos/{photoId}")]
        [ProducesResponseType(typeof(AestheticPhotoVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePhoto(int photoId, [FromBody] AestheticPhotoVM photoVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (photoId != photoVM.Id)
                return BadRequest("Photo id mismatch");

            var existing = _aestheticService.GetPhotoById(photoId);
            if (existing == null)
                return NotFound(photoId);

            try
            {
                var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
                var updated = _aestheticService.UpdatePhoto(photo, GetCurrentUserId());
                return Ok(ToPhotoViewModel(updated));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Photo update blocked for {PhotoId}", photoId);
                AddModelError(ex.Message);
                return Forbid();
            }
        }

        [HttpPut("photos/{photoId}/upload")]
        [ProducesResponseType(typeof(AestheticPhotoVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePhotoUpload(int photoId, [FromForm] AestheticPhotoUploadVM uploadVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsSupportedImage(uploadVM.File))
                return BadRequest("Only non-empty image files are allowed.");

            var existing = _aestheticService.GetPhotoById(photoId);
            if (existing == null)
                return NotFound(photoId);

            var consultation = _aestheticService.GetConsultationById(uploadVM.ConsultationId);
            if (consultation == null)
                return NotFound(uploadVM.ConsultationId);

            var savedPath = SaveUploadedFile(uploadVM.File);
            DeletePhysicalFile(existing.FilePath);

            var photoVM = new AestheticPhotoVM
            {
                Id = photoId,
                ConsultationId = uploadVM.ConsultationId,
                ConsultId = consultation.Photos.FirstOrDefault()?.ConsultId,
                PNo = consultation.Patient?.Pno,
                FileName = uploadVM.File.FileName,
                Type = uploadVM.Type,
                Url = savedPath
            };

            try
            {
                var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
                var updated = _aestheticService.UpdatePhoto(photo, GetCurrentUserId());
                return Ok(ToPhotoViewModel(updated));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Photo upload update blocked for {PhotoId}", photoId);
                AddModelError(ex.Message);
                return Forbid();
            }
        }

        [HttpGet("consent-templates")]
        [ProducesResponseType(typeof(IEnumerable<AestheticConsentTemplateVM>), StatusCodes.Status200OK)]
        public IActionResult GetConsentTemplates([FromQuery] string? procedureType, [FromQuery] bool includeInactive = false)
        {
            var templates = _aestheticService.GetConsentTemplates(procedureType, includeInactive);
            return Ok(_mapper.Map<IEnumerable<AestheticConsentTemplateVM>>(templates));
        }

        [HttpGet("consent-templates/{id:int}")]
        [ProducesResponseType(typeof(AestheticConsentTemplateVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetConsentTemplate(int id)
        {
            var template = _aestheticService.GetConsentTemplateById(id);
            if (template == null)
                return NotFound(id);

            return Ok(_mapper.Map<AestheticConsentTemplateVM>(template));
        }

        [HttpPost("consent-templates")]
        [ProducesResponseType(typeof(AestheticConsentTemplateVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateConsentTemplate([FromBody] AestheticConsentTemplateVM templateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var template = _mapper.Map<Core.Models.Aesthetic.AestheticConsentTemplate>(templateVM);
                var created = _aestheticService.AddConsentTemplate(template);
                return CreatedAtAction(nameof(GetConsentTemplate), new { id = created.Id }, _mapper.Map<AestheticConsentTemplateVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consent template");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("consent-templates/{id:int}")]
        [ProducesResponseType(typeof(AestheticConsentTemplateVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateConsentTemplate(int id, [FromBody] AestheticConsentTemplateVM templateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != templateVM.Id)
                return BadRequest("Consent template id mismatch");

            if (_aestheticService.GetConsentTemplateById(id) == null)
                return NotFound(id);

            try
            {
                var template = _mapper.Map<Core.Models.Aesthetic.AestheticConsentTemplate>(templateVM);
                var updated = _aestheticService.UpdateConsentTemplate(template);
                return Ok(_mapper.Map<AestheticConsentTemplateVM>(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consent template {TemplateId}", id);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("consent-templates/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteConsentTemplate(int id)
        {
            try
            {
                if (_aestheticService.GetConsentTemplateById(id) == null)
                    return NotFound(id);

                _aestheticService.DeleteConsentTemplate(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Delete blocked for consent template {TemplateId}", id);
                AddModelError(ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consent template {TemplateId}", id);
                AddModelError("Unable to delete consent template");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("consents")]
        [ProducesResponseType(typeof(IEnumerable<AestheticSignedConsentVM>), StatusCodes.Status200OK)]
        public IActionResult GetSignedConsents([FromQuery] string? consultId, [FromQuery] string? pNo, [FromQuery] string? procedureType, [FromQuery] bool includeVoided = false)
        {
            var consents = _aestheticService.GetSignedConsents(consultId, pNo, procedureType, includeVoided);
            return Ok(_mapper.Map<IEnumerable<AestheticSignedConsentVM>>(consents).Select(ApplyConsentImageUrl));
        }

        [HttpGet("consents/{consentId:int}")]
        [ProducesResponseType(typeof(AestheticSignedConsentVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSignedConsent(int consentId)
        {
            var consent = _aestheticService.GetSignedConsents(includeVoided: true).FirstOrDefault(x => x.Id == consentId);
            if (consent == null)
                return NotFound(consentId);

            return Ok(ApplyConsentImageUrl(_mapper.Map<AestheticSignedConsentVM>(consent)));
        }

        [HttpPost("consents/sign")]
        [ProducesResponseType(typeof(AestheticSignedConsentVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SignConsent([FromBody] SignAestheticConsentVM consentVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var signatureBytes = ToBytes(consentVM.SignatureImageBase64);
                var signatureImagePath = SaveConsentSignature(signatureBytes, consentVM.ConsultId, consentVM.PNo, consentVM.ProcedureType);
                var consent = _aestheticService.SignConsent(
                    consentVM.PatientId,
                    consentVM.ConsultId,
                    consentVM.PNo,
                    consentVM.ProcedureType,
                    consentVM.ConsentTemplateId,
                    consentVM.SignatureName,
                    consentVM.WitnessedBy,
                    string.IsNullOrWhiteSpace(consentVM.SignedBy) ? GetCurrentUserId() : consentVM.SignedBy,
                    consentVM.Notes,
                    signatureBytes,
                    signatureImagePath);

                return StatusCode(StatusCodes.Status201Created, ApplyConsentImageUrl(_mapper.Map<AestheticSignedConsentVM>(consent)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error signing consent for ConsultId {ConsultId}", consentVM.ConsultId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("consents/{consentId:int}/void")]
        [ProducesResponseType(typeof(AestheticSignedConsentVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult VoidConsent(int consentId, [FromBody] VoidAestheticConsentVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var consent = _aestheticService.VoidConsent(consentId, model.VoidReason, GetCurrentUserId() ?? "SYSTEM");
                return Ok(ApplyConsentImageUrl(_mapper.Map<AestheticSignedConsentVM>(consent)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error voiding consent {ConsentId}", consentId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("consents/{consentId}/viewed")]
        [ProducesResponseType(typeof(AestheticSignedConsentVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult MarkConsentViewed(int consentId)
        {
            try
            {
                var consent = _aestheticService.MarkConsentViewed(consentId, GetCurrentUserId() ?? "SYSTEM");
                return Ok(ApplyConsentImageUrl(_mapper.Map<AestheticSignedConsentVM>(consent)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking consent {ConsentId} as viewed", consentId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("follow-ups")]
        [ProducesResponseType(typeof(IEnumerable<AestheticFollowUpVM>), StatusCodes.Status200OK)]
        public IActionResult GetFollowUps([FromQuery] int? patientId, [FromQuery] int? consultationId, [FromQuery] bool? isCompleted)
        {
            var followUps = _aestheticService.GetFollowUps(patientId, consultationId, isCompleted);
            return Ok(_mapper.Map<IEnumerable<AestheticFollowUpVM>>(followUps));
        }

        [HttpGet("follow-ups/{followUpId:int}")]
        [ProducesResponseType(typeof(AestheticFollowUpVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFollowUp(int followUpId)
        {
            var followUp = _aestheticService.GetFollowUpById(followUpId);
            if (followUp == null)
                return NotFound(followUpId);

            return Ok(_mapper.Map<AestheticFollowUpVM>(followUp));
        }

        [HttpPost("follow-ups/schedule")]
        [ProducesResponseType(typeof(AestheticFollowUpVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ScheduleFollowUp([FromBody] ScheduleAestheticFollowUpVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var scheduled = _aestheticService.ScheduleFollowUp(model.ConsultationId, model.DaysAhead, false, model.Notes);
                return StatusCode(StatusCodes.Status201Created, _mapper.Map<AestheticFollowUpVM>(scheduled));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling follow-up for consultation {ConsultationId}", model.ConsultationId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("follow-ups/{followUpId:int}/complete")]
        [ProducesResponseType(typeof(AestheticFollowUpVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CompleteFollowUp(int followUpId, [FromBody] CompleteAestheticFollowUpVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var completed = _aestheticService.CompleteFollowUp(
                    followUpId,
                    model.Outcome,
                    model.PatientSatisfactionScore,
                    model.RepeatPhotosTaken,
                    model.NextTreatmentRecommendation,
                    model.Notes);

                return Ok(_mapper.Map<AestheticFollowUpVM>(completed));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing follow-up {FollowUpId}", followUpId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("analytics/revenue-per-procedure")]
        [ProducesResponseType(typeof(IEnumerable<ProcedureRevenueMetricVM>), StatusCodes.Status200OK)]
        public IActionResult GetRevenuePerProcedure([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var metrics = _aestheticService.GetRevenuePerProcedure(from, to);
            return Ok(_mapper.Map<IEnumerable<ProcedureRevenueMetricVM>>(metrics));
        }

        [HttpGet("analytics/most-used-products")]
        [ProducesResponseType(typeof(IEnumerable<ProductUsageMetricVM>), StatusCodes.Status200OK)]
        public IActionResult GetMostUsedProducts([FromQuery] int top = 10, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var metrics = _aestheticService.GetMostUsedProducts(top, from, to);
            return Ok(_mapper.Map<IEnumerable<ProductUsageMetricVM>>(metrics));
        }

        [HttpGet("analytics/complication-rates")]
        [ProducesResponseType(typeof(ComplicationRateMetricVM), StatusCodes.Status200OK)]
        public IActionResult GetComplicationRates([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var metric = _aestheticService.GetComplicationRate(from, to);
            return Ok(_mapper.Map<ComplicationRateMetricVM>(metric));
        }

        [HttpGet("analytics/patient-retention")]
        [ProducesResponseType(typeof(PatientRetentionMetricVM), StatusCodes.Status200OK)]
        public IActionResult GetPatientRetention([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var metric = _aestheticService.GetPatientRetention(from, to);
            return Ok(_mapper.Map<PatientRetentionMetricVM>(metric));
        }

        [HttpGet("analytics/before-after-outcomes")]
        [ProducesResponseType(typeof(BeforeAfterOutcomeMetricVM), StatusCodes.Status200OK)]
        public IActionResult GetBeforeAfterOutcomes([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var metric = _aestheticService.GetBeforeAfterOutcomeTracking(from, to);
            return Ok(_mapper.Map<BeforeAfterOutcomeMetricVM>(metric));
        }

        private AestheticSignedConsentVM ApplyConsentImageUrl(AestheticSignedConsentVM vm)
        {
            vm.SignatureImagePath = BuildPublicUrl(vm.SignatureImagePath);
            return vm;
        }

        private string? SaveConsentSignature(byte[]? bytes, string consultId, string pNo, string procedureType)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var uploadsRoot = EnsureUploadFolder(ConsentUploadFolder);
            var safeConsultId = MakeSafeFileSegment(consultId);
            var safePNo = MakeSafeFileSegment(pNo);
            var safeProcedureType = MakeSafeFileSegment(procedureType);
            var fileName = $"{safeConsultId}_{safePNo}_{safeProcedureType}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.png";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            System.IO.File.WriteAllBytes(fullPath, bytes);
            return $"/{ConsentUploadFolder.Replace("\\", "/")}/{fileName}";
        }

        private static string MakeSafeFileSegment(string value)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var cleaned = new string(value.Trim().Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray());
            return string.IsNullOrWhiteSpace(cleaned) ? "consent" : cleaned;
        }

        private string EnsureUploadFolder(string? folder = null)
        {
            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var targetFolder = folder ?? UploadFolder;
            var directoryPath = Path.Combine(webRoot, targetFolder.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }

        private static byte[]? ToBytes(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return null;

            var idx = base64.IndexOf(',');
            var raw = idx >= 0 ? base64[(idx + 1)..] : base64;
            return Convert.FromBase64String(raw);
        }

        private AestheticPhotoVM ToPhotoViewModel(Core.Models.Aesthetic.AestheticPhoto photo)
        {
            var vm = _mapper.Map<AestheticPhotoVM>(photo);
            vm.Url = BuildPublicUrl(vm.Url);
            vm.ThumbnailUrl = BuildPublicUrl(vm.ThumbnailUrl ?? vm.Url);
            return vm;
        }

        private string BuildPublicUrl(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            if (Uri.TryCreate(relativePath, UriKind.Absolute, out _))
                return relativePath;

            return $"{Request.Scheme}://{Request.Host}{relativePath}";
        }

        private string SaveUploadedFile(IFormFile file)
        {
            var uploadsRoot = EnsureUploadFolder();
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return $"/{UploadFolder.Replace("\\", "/")}/{fileName}";
        }

        private static bool IsSupportedImage(IFormFile file)
        {
            if (file.Length <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
            return allowed.Contains(extension);
        }

        private void DeletePhysicalFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
                return;

            var pathValue = filePath;
            if (Uri.TryCreate(filePath, UriKind.Absolute, out var uri))
            {
                pathValue = uri.LocalPath;
            }

            var relativePath = pathValue.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(webRoot, relativePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private string EnsureUploadFolder()
        {
            return EnsureUploadFolder(UploadFolder);
        }

        [HttpPost("follow-ups/{followUpId:int}/patient-satisfaction/send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendPatientSatisfactionEmail(int followUpId, [FromBody] SendPatientSatisfactionRequestVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var context = _aestheticService.GetFollowUpSubmissionContext(followUpId);
                if (string.IsNullOrWhiteSpace(context.consultId) || string.IsNullOrWhiteSpace(context.pNo))
                {
                    AddModelError("ConsultId and PNo are required to send patient satisfaction link.");
                    return BadRequest(ModelState);
                }

                var expiresOnUtc = DateTime.UtcNow.AddDays(7);
                var token = _aestheticService.CreatePatientSatisfactionToken(context.followUpId, context.consultId, context.pNo, expiresOnUtc);
                var surveyUrl = BuildPatientSatisfactionUrl(token);
                var recipientName = string.IsNullOrWhiteSpace(model.RecipientName) ? context.patientName ?? "Patient" : model.RecipientName;
                var emailBody = BuildPatientSatisfactionEmailBody(recipientName, surveyUrl, expiresOnUtc);

                var result = await _emailSender.SendEmailAsync(
                    recipientName,
                    model.RecipientEmail!,
                    "Patient Satisfaction Follow-up (1-10)",
                    emailBody,
                    true);

                if (!result.success)
                {
                    AddModelError(result.errorMsg ?? "Failed to send patient satisfaction email.");
                    return BadRequest(ModelState);
                }

                return Ok(new
                {
                    followUpId = context.followUpId,
                    consultationId = context.consultationId,
                    consultId = context.consultId,
                    pNo = context.pNo,
                    expiresOnUtc,
                    sentTo = model.RecipientEmail
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending patient satisfaction email for follow-up {FollowUpId}", followUpId);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("patient-satisfaction")]
        [ProducesResponseType(typeof(PublicPatientSatisfactionSurveyVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPatientSatisfactionSurvey([FromQuery] string token)
        {
            try
            {
                var tokenContext = _aestheticService.ValidatePatientSatisfactionToken(token);
                if (!tokenContext.HasValue)
                {
                    AddModelError("Invalid or expired patient satisfaction token.");
                    return BadRequest(ModelState);
                }

                var context = _aestheticService.GetFollowUpSubmissionContext(tokenContext.Value.followUpId);
                if (!string.Equals(context.consultId, tokenContext.Value.consultId, StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(context.pNo, tokenContext.Value.pNo, StringComparison.OrdinalIgnoreCase))
                {
                    AddModelError("Invalid patient satisfaction token context.");
                    return BadRequest(ModelState);
                }

                return Ok(new PublicPatientSatisfactionSurveyVM
                {
                    FollowUpId = context.followUpId,
                    ConsultationId = context.consultationId,
                    ConsultId = context.consultId,
                    PNo = context.pNo,
                    PatientName = context.patientName,
                    ScheduledDate = context.scheduledDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading patient satisfaction survey");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("patient-satisfaction/submit")]
        [ProducesResponseType(typeof(AestheticFollowUpVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SubmitPatientSatisfaction([FromQuery] string token, [FromBody] SubmitPatientSatisfactionVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tokenContext = _aestheticService.ValidatePatientSatisfactionToken(token);
                if (!tokenContext.HasValue)
                {
                    AddModelError("Invalid or expired patient satisfaction token.");
                    return BadRequest(ModelState);
                }

                var followUp = _aestheticService.SubmitPatientSatisfaction(
                    tokenContext.Value.followUpId,
                    tokenContext.Value.consultId,
                    tokenContext.Value.pNo,
                    model.PatientSatisfactionScore!.Value,
                    model.Outcome);

                return Ok(_mapper.Map<AestheticFollowUpVM>(followUp));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting patient satisfaction");
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        private string BuildPatientSatisfactionUrl(string token)
        {
            var baseUrl = _appSettings.ClientBaseUrl;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = $"{Request.Scheme}://{Request.Host}";
            }

            var trimmedBase = baseUrl.TrimEnd('/');
            return $"{trimmedBase}{PatientSatisfactionPath}?token={Uri.EscapeDataString(token)}";
        }

        private static string BuildPatientSatisfactionEmailBody(string recipientName, string surveyUrl, DateTime expiresOnUtc)
        {
            return $"""
                <p>Dear {System.Net.WebUtility.HtmlEncode(recipientName)},</p>
                <p>Please share your treatment satisfaction score on a scale of <strong>1-10</strong>.</p>
                <p>Click the button below to submit your score:</p>
                <p><a href=\"{surveyUrl}\" style=\"display:inline-block;padding:10px 16px;background:#1976d2;color:#fff;text-decoration:none;border-radius:4px;\">Submit Satisfaction</a></p>
                <p>This link expires on <strong>{expiresOnUtc:yyyy-MM-dd HH:mm} UTC</strong>.</p>
                <p>Thank you.</p>
                """;
        }
    }
}
