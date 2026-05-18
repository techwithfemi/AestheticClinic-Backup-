using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AestheticEMR.Server.Controllers
{
    /// <summary>
    /// API controller for audit trail and incident management in aesthetic procedures.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class AuditController : BaseApiController
    {
        private readonly IAuditService _auditService;

        public AuditController(
            ILogger<AuditController> logger,
            IMapper mapper,
            IAuditService auditService)
            : base(logger, mapper)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Get all open incidents (unresolved safety events).
        /// </summary>
        [HttpGet("incidents/open")]
        [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
        [ProducesResponseType(typeof(List<AuditLog>), 200)]
        public async Task<IActionResult> GetOpenIncidents()
        {
            try
            {
                var incidents = await _auditService.GetOpenIncidentsAsync();
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving open incidents");
                AddModelError("Unable to retrieve open incidents");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get all incidents filtered by severity and date range.
        /// </summary>
        [HttpGet("incidents")]
        [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
        [ProducesResponseType(typeof(List<AuditLog>), 200)]
        public async Task<IActionResult> GetIncidents(
            [FromQuery] string severity,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            try
            {
                var incidents = await _auditService.GetIncidentsAsync(severity, fromDate, toDate);
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving incidents");
                AddModelError("Unable to retrieve incidents");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get audit trail for a specific consultation.
        /// </summary>
        [HttpGet("consultation/{consultationId}")]
        [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
        [ProducesResponseType(typeof(List<AuditLog>), 200)]
        public async Task<IActionResult> GetConsultationAuditTrail(int consultationId)
        {
            try
            {
                var trail = await _auditService.GetAuditTrailByTranCodeAsync(consultationId.ToString());
                return Ok(trail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation audit trail {ConsultationId}", consultationId);
                AddModelError("Unable to retrieve consultation audit trail");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get audit trail for a specific patient.
        /// </summary>
        [HttpGet("patient/{patientId}")]
        [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
        [ProducesResponseType(typeof(List<AuditLog>), 200)]
        public async Task<IActionResult> GetPatientAuditTrail(int patientId)
        {
            try
            {
                var trail = await _auditService.GetAuditTrailByTranCodeAsync(patientId.ToString());
                return Ok(trail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient audit trail {PatientId}", patientId);
                AddModelError("Unable to retrieve patient audit trail");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Log a complication incident.
        /// </summary>
        [HttpPost("complication")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LogComplication([FromBody] ComplicationLogRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request body is required");

                var performedBy = Utilities.GetUserId(User) ?? User?.Identity?.Name;
                var sourceIp = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _auditService.LogComplicationAsync(
                    request.TranCode,
                    request.ComplicationTitle,
                    request.Details,
                    request.Severity ?? "Warning",
                    performedBy,
                    sourceIp);

                return Ok(new { message = "Complication logged successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging complication");
                AddModelError("Unable to log complication");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Log a safety incident.
        /// </summary>
        [HttpPost("safety-incident")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LogSafetyIncident([FromBody] SafetyIncidentRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request body is required");

                var performedBy = Utilities.GetUserId(User) ?? User?.Identity?.Name;
                var sourceIp = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _auditService.LogSafetyIncidentAsync(
                    request.TranCode,
                    request.Title,
                    request.Details,
                    request.Severity ?? "Critical",
                    request.Tags ?? "",
                    performedBy,
                    sourceIp);

                return Ok(new { message = "Safety incident logged successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging safety incident");
                AddModelError("Unable to log safety incident");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Log an allergy event.
        /// </summary>
        [HttpPost("allergy")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LogAllergy([FromBody] AllergyLogRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request body is required");

                var performedBy = Utilities.GetUserId(User) ?? User?.Identity?.Name;
                var sourceIp = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _auditService.LogAllergyEventAsync(
                    request.TranCode,
                    request.Allergy,
                    request.Details,
                    request.Severity ?? "Error",
                    performedBy,
                    sourceIp);

                return Ok(new { message = "Allergy event logged successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging allergy event");
                AddModelError("Unable to log allergy event");
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Mark an incident as reviewed and add resolution notes.
        /// </summary>
        [HttpPut("{auditLogId}/review")]
        [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkAsReviewed(long auditLogId, [FromBody] ReviewRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request body is required");

                var reviewedBy = Utilities.GetUserId(User) ?? User?.Identity?.Name ?? "system";

                await _auditService.MarkAsReviewedAsync(
                    auditLogId,
                    reviewedBy,
                    request.ResolutionNotes);

                return Ok(new { message = "Incident marked as reviewed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking audit log {AuditLogId} as reviewed", auditLogId);
                AddModelError("Unable to review incident");
                return BadRequest(ModelState);
            }
        }
    }

    // Request DTOs
    public class ComplicationLogRequest
    {
        public string TranCode { get; set; } = string.Empty;
        public string ComplicationTitle { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string? Severity { get; set; }
    }

    public class SafetyIncidentRequest
    {
        public string TranCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string? Severity { get; set; }
        public string? Tags { get; set; }
    }

    public class AllergyLogRequest
    {
        public string TranCode { get; set; } = string.Empty;
        public string Allergy { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string? Severity { get; set; }
    }

    public class ReviewRequest
    {
        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
