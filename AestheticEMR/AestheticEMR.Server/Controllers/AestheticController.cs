// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Server.ViewModels.Aesthetic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AestheticController : BaseApiController
    {
        private readonly IAestheticService _aestheticService;
        private readonly IWebHostEnvironment _environment;
        private const string UploadFolder = "uploads/aesthetic";
        private const string ConsentUploadFolder = "uploads/aesthetic/consents";

        public AestheticController(
            ILogger<AestheticController> logger,
            IMapper mapper,
            IAestheticService aestheticService,
            IWebHostEnvironment environment)
            : base(logger, mapper)
        {
            _aestheticService = aestheticService;
            _environment = environment;
        }

        [HttpGet("patients")]
        [ProducesResponseType(typeof(IEnumerable<AestheticPatientVM>), StatusCodes.Status200OK)]
        public IActionResult GetPatients()
        {
            var patients = _aestheticService.GetPatients();
            return Ok(_mapper.Map<IEnumerable<AestheticPatientVM>>(patients));
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
                var updated = _aestheticService.UpdateConsultation(consultation, consultationVM.ConsultId, consultationVM.PNo, consultationVM.Services);
                return Ok(_mapper.Map<AestheticConsultationVM>(updated));
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

                _aestheticService.DeleteConsultation(consultationId);
                return NoContent();
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

            var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
            var updated = _aestheticService.UpdatePhoto(photo);
            return Ok(ToPhotoViewModel(updated));
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

            var photo = _mapper.Map<Core.Models.Aesthetic.AestheticPhoto>(photoVM);
            var updated = _aestheticService.UpdatePhoto(photo);
            return Ok(ToPhotoViewModel(updated));
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
    }
}
