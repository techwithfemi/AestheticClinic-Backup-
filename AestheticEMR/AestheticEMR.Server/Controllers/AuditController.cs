using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Core.Services.Audit.Interfaces;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.Services;
using AestheticEMR.Server.ViewModels.Audit;
using AutoMapper;
using DataAccess.DbAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AuditController(
    ILogger<AuditController> logger,
    IMapper mapper,
    IAuditService auditService,
    IAdminAuditReportLookupService adminAuditReportLookupService,
    ISqlDataAccess sqlDataAccess)
    : BaseApiController(logger, mapper)
{
    [HttpGet("report/users")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(IEnumerable<AdminAuditReportUserLookupVM>), 200)]
    public async Task<IActionResult> GetAuditReportUsers(CancellationToken ct)
    {
        try
        {
            var users = await adminAuditReportLookupService.GetUsersAsync(ct);
            return Ok(_mapper.Map<IEnumerable<AdminAuditReportUserLookupVM>>(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin audit report users");
            AddModelError("Unable to retrieve admin audit report users");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("report/modules")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(IEnumerable<AdminAuditReportModuleLookupVM>), 200)]
    public async Task<IActionResult> GetAuditReportModules(CancellationToken ct)
    {
        try
        {
            var modules = await adminAuditReportLookupService.GetModulesAsync(ct);
            return Ok(_mapper.Map<IEnumerable<AdminAuditReportModuleLookupVM>>(modules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin audit report modules");
            AddModelError("Unable to retrieve admin audit report modules");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("report/rows")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(IEnumerable<AdminAuditReportRowVM>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAuditReportRows(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate,
        [FromQuery] string filterType = "ALL",
        [FromQuery] string? filterValue = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken ct = default)
    {
        if (fromDate == default || toDate == default)
        {
            return BadRequest(new { fromDate, toDate, filterType, filterValue, searchTerm });
        }

        var normalizedFilterType = string.IsNullOrWhiteSpace(filterType) ? "ALL" : filterType.Trim().ToUpperInvariant();
        if (normalizedFilterType is not ("ALL" or "MODULE" or "USER"))
        {
            return BadRequest(new { fromDate, toDate, filterType, filterValue, searchTerm });
        }

        try
        {
            var rows = await adminAuditReportLookupService.GetReportRowsAsync(fromDate, toDate, normalizedFilterType, filterValue?.Trim(), searchTerm?.Trim(), ct);
            return Ok(_mapper.Map<IEnumerable<AdminAuditReportRowVM>>(rows));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin audit report rows");
            AddModelError("Unable to retrieve admin audit report rows");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("users-report")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(IEnumerable<AdminUsersReportRowVM>), 200)]
    public async Task<IActionResult> GetAdminUsersReport(CancellationToken ct)
    {
        try
        {
            const string sql = @"
SELECT
    LTRIM(RTRIM(ISNULL(JobTitle, ''))) AS JobTitle,
    LTRIM(RTRIM(ISNULL(FullName, ''))) AS FullName,
    NULLIF(LTRIM(RTRIM(ISNULL([Configuration], ''))), '') AS [Configuration],
    CAST(ISNULL(IsEnabled, 0) AS bit) AS IsEnabled,
    LTRIM(RTRIM(ISNULL(UserName, ''))) AS UserName,
    NULLIF(LTRIM(RTRIM(ISNULL(Email, ''))), '') AS Email,
    NULLIF(LTRIM(RTRIM(ISNULL(PhoneNumber, ''))), '') AS PhoneNumber,
    CAST(ISNULL(EmailConfirmed, 0) AS bit) AS EmailConfirmed,
    CAST(ISNULL(PhoneNumberConfirmed, 0) AS bit) AS PhoneNumberConfirmed,
    CAST(ISNULL(TwoFactorEnabled, 0) AS bit) AS TwoFactorEnabled,
    CreatedDate,
    UpdatedDate
FROM AspNetUsers
ORDER BY LTRIM(RTRIM(ISNULL(FullName, UserName))), LTRIM(RTRIM(ISNULL(UserName, '')));";

            var rows = await sqlDataAccess.LoadDataText<AdminUsersReportRowVM, object>(sql, new { }, "DefaultConnection");
            return Ok(rows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin users report");
            AddModelError("Unable to retrieve admin users report");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("incidents/open")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetOpenIncidents()
    {
        try
        {
            var incidents = await auditService.GetOpenIncidentsAsync();
            return Ok(incidents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving open incidents");
            AddModelError("Unable to retrieve open incidents");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("incidents")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetIncidents([FromQuery] string severity, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        try
        {
            var incidents = await auditService.GetIncidentsAsync(severity, fromDate, toDate);
            return Ok(incidents);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving incidents");
            AddModelError("Unable to retrieve incidents");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("consultation/{consultationId}")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetConsultationAuditTrail(int consultationId)
    {
        try
        {
            var trail = await auditService.GetAuditTrailByTranCodeAsync(consultationId.ToString());
            return Ok(trail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving consultation audit trail {ConsultationId}", consultationId);
            AddModelError("Unable to retrieve consultation audit trail");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(AuthPolicies.ViewAuditLogsPolicy)]
    [ProducesResponseType(typeof(List<AuditLog>), 200)]
    public async Task<IActionResult> GetPatientAuditTrail(int patientId)
    {
        try
        {
            var trail = await auditService.GetAuditTrailByTranCodeAsync(patientId.ToString());
            return Ok(trail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient audit trail {PatientId}", patientId);
            AddModelError("Unable to retrieve patient audit trail");
            return BadRequest(ModelState);
        }
    }

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

            await auditService.LogComplicationAsync(
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

            await auditService.LogSafetyIncidentAsync(
                request.TranCode,
                request.Title,
                request.Details,
                request.Severity ?? "Critical",
                request.Tags ?? string.Empty,
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

            await auditService.LogAllergyEventAsync(
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
            await auditService.MarkAsReviewedAsync(auditLogId, reviewedBy, request.ResolutionNotes);
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
