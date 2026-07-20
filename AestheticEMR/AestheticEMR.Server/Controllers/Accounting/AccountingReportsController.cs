using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.Services.Reporting;
using AestheticEMR.Server.ViewModels.Accounting;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers.Accounting;

[Route("api/accounting/reports")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewAccountingPolicy)]
public class AccountingReportsController(
    ILegacyCrystalReportProxyService reportProxyService,
    IAccountingReportLookupService accountingReportLookupService,
    IEmrAppDefaultsService emrAppDefaultsService,
    ILogger<AccountingReportsController> logger,
    IMapper mapper) : BaseApiController(logger, mapper)
{
    [HttpGet("defaults")]
    [ProducesResponseType(typeof(AccountingReportDefaultsVM), 200)]
    public async Task<IActionResult> GetDefaults(CancellationToken ct)
    {
        var defaults = await emrAppDefaultsService.GetAsync(ct);
        return Ok(new AccountingReportDefaultsVM
        {
            CoyID = defaults.Get("CoyID")
        });
    }

    [HttpGet("general-ledger/years")]
    [ProducesResponseType(typeof(IEnumerable<AccountingReportYearVM>), 200)]
    public async Task<IActionResult> GetGeneralLedgerYears(CancellationToken ct)
    {
        var years = await accountingReportLookupService.GetGeneralLedgerYearsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<AccountingReportYearVM>>(years));
    }

    [HttpGet("general-ledger/periods")]
    [ProducesResponseType(typeof(IEnumerable<AccountingReportPeriodVM>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetGeneralLedgerPeriods([FromQuery] string coyID, [FromQuery] string year, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(year))
        {
            return BadRequest(new { coyID, year });
        }

        var periods = await accountingReportLookupService.GetGeneralLedgerPeriodsAsync(coyID.Trim(), year.Trim(), ct);
        return Ok(_mapper.Map<IEnumerable<AccountingReportPeriodVM>>(periods));
    }

    [HttpGet("general-ledger/ledgers")]
    [ProducesResponseType(typeof(IEnumerable<AccountingLedgerLookupVM>), 200)]
    public async Task<IActionResult> GetGeneralLedgerLedgers(CancellationToken ct)
    {
        var ledgers = await accountingReportLookupService.GetGeneralLedgerLedgersAsync(ct);
        return Ok(_mapper.Map<IEnumerable<AccountingLedgerLookupVM>>(ledgers));
    }

    [HttpGet("general-ledger/accounts")]
    [ProducesResponseType(typeof(IEnumerable<AccountingAccountLookupVM>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetGeneralLedgerAccounts([FromQuery] string coyID, [FromQuery] string period, [FromQuery] string ledgerCode, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(ledgerCode))
        {
            return BadRequest(new { coyID, period, ledgerCode });
        }

        var accounts = await accountingReportLookupService.GetGeneralLedgerAccountsAsync(coyID.Trim(), period.Trim(), ledgerCode.Trim(), ct);
        return Ok(_mapper.Map<IEnumerable<AccountingAccountLookupVM>>(accounts));
    }

    [HttpGet("profit-and-loss/headers")]
    [ProducesResponseType(typeof(IEnumerable<ProfitAndLossHeaderVM>), 200)]
    public async Task<IActionResult> GetProfitAndLossHeaders(CancellationToken ct)
    {
        var headers = await accountingReportLookupService.GetProfitAndLossHeadersAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ProfitAndLossHeaderVM>>(headers));
    }

    [HttpGet("balance-sheet/headers")]
    [ProducesResponseType(typeof(IEnumerable<BalanceSheetHeaderVM>), 200)]
    public async Task<IActionResult> GetBalanceSheetHeaders(CancellationToken ct)
    {
        var headers = await accountingReportLookupService.GetBalanceSheetHeadersAsync(ct);
        return Ok(_mapper.Map<IEnumerable<BalanceSheetHeaderVM>>(headers));
    }

    [HttpGet("general-ledger")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetGeneralLedger([FromQuery] string coyID, [FromQuery] string period, [FromQuery] string ledgerCode, [FromQuery] string accountNo, [FromQuery] string? ledgerDisplayText, [FromQuery] string? accountDisplayText, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(ledgerCode) || string.IsNullOrWhiteSpace(accountNo))
        {
            return BadRequest(new { coyID, period, ledgerCode, accountNo, ledgerDisplayText, accountDisplayText });
        }

        try
        {
            var companyName = await accountingReportLookupService.GetCompanyNameAsync(coyID.Trim(), ct);
            var report = await reportProxyService.GetGeneralLedgerReportAsync(coyID.Trim(), period.Trim(), ledgerCode.Trim(), accountNo.Trim(), ledgerDisplayText?.Trim(), accountDisplayText?.Trim(), companyName, ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load general ledger report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpGet("profit-and-loss")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProfitAndLoss([FromQuery] string coyID, [FromQuery] string period, [FromQuery] string year, [FromQuery] string rptBy, [FromQuery] bool isClose, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(rptBy))
        {
            return BadRequest(new { coyID, period, year, rptBy, isClose });
        }

        try
        {
            var companyName = await accountingReportLookupService.GetCompanyNameAsync(coyID.Trim(), ct);
            var report = await reportProxyService.GetProfitAndLossReportAsync(coyID.Trim(), period.Trim(), year.Trim(), rptBy.Trim(), isClose, companyName, ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load profit and loss report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpGet("profit-and-loss/details")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProfitAndLossDetails([FromQuery] string coyID, [FromQuery] string period, [FromQuery] string year, [FromQuery] string rptBy, [FromQuery] string groupID, [FromQuery] bool isClose, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(rptBy) || string.IsNullOrWhiteSpace(groupID))
        {
            return BadRequest(new { coyID, period, year, rptBy, groupID, isClose });
        }

        try
        {
            var companyName = await accountingReportLookupService.GetCompanyNameAsync(coyID.Trim(), ct);
            var report = await reportProxyService.GetProfitAndLossDetailsReportAsync(coyID.Trim(), period.Trim(), year.Trim(), rptBy.Trim(), groupID.Trim(), isClose, companyName, ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load profit and loss details report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

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

    [HttpGet("balance-sheet")]
    [Produces("application/pdf")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetBalanceSheet([FromQuery] string coyID, [FromQuery] string period, [FromQuery] string year, [FromQuery] string rptBy, [FromQuery] bool isClose, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(coyID) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(year) || string.IsNullOrWhiteSpace(rptBy))
        {
            return BadRequest(new { coyID, period, year, rptBy, isClose });
        }

        try
        {
            var companyName = await accountingReportLookupService.GetCompanyNameAsync(coyID.Trim(), ct);
            var report = await reportProxyService.GetBalanceSheetReportAsync(coyID.Trim(), period.Trim(), year.Trim(), rptBy.Trim(), isClose, companyName, ct);
            return File(report.Content, report.ContentType, report.FileName);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Unable to load balance sheet report from legacy Crystal report service");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }
}
