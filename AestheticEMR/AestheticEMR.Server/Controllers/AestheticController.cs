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

        public AestheticController(ILogger<AestheticController> logger, IMapper mapper, IAestheticService aestheticService)
            : base(logger, mapper)
        {
            _aestheticService = aestheticService;
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

            var consultation = _mapper.Map<Core.Models.Aesthetic.AestheticConsultation>(consultationVM);
            var created = _aestheticService.AddConsultation(consultation);
            return CreatedAtAction(nameof(GetConsultation), new { consultationId = created.Id }, _mapper.Map<AestheticConsultationVM>(created));
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
            return Ok(_mapper.Map<IEnumerable<AestheticPhotoVM>>(photos));
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
            return CreatedAtAction(nameof(GetConsultationPhotos), new { consultationId = consultationId }, _mapper.Map<AestheticPhotoVM>(created));
        }
    }
}
