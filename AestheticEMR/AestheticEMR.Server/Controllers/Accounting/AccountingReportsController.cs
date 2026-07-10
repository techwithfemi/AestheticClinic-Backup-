using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.Services.Reporting;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers.Accounting;

[Route("api/accounting/reports")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewAccountingPolicy)]
public class AccountingReportsController(
    ILegacyCrystalReportProxyService reportProxyService,
    ILogger<AccountingReportsController> logger,
    IMapper mapper) : BaseApiController(logger, mapper)
{
    [HttpGet("financial/variance-analysis")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetFinancialVarianceAnalysis(CancellationToken ct)
    {
        try
        {
            var report = await reportProxyService.GetFinancialVarianceAnalysisReportAsync(ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load financial variance analysis report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpGet("financial/comparative-income-statement")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetComparativeIncomeStatement(CancellationToken ct)
    {
        try
        {
            var report = await reportProxyService.GetComparativeIncomeStatementReportAsync(ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load comparative income statement report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }
}
