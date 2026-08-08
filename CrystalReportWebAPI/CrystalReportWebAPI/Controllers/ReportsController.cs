using CrystalReportWebAPI.Utilities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CrystalReportWebAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    [AllowAnonymous]
    public class ReportsController : ApiController
    {
        [Route("Accounting/BalanceSheet")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> BalanceSheet(string coyID, string period, string year, string rptBy, bool isClose = false, string companyName = null)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptBalSheet.rpt";
            var exportFilename = $"rptBalSheet-{period}.pdf";

            var conStr = ResolveConnectionStringFromRequest(Request);

            if (!isClose)
            {
                await DapperReportData.ExecuteNonQueryAsync(conStr, "CloseAccountingPeriod", new
                {
                    Period = period.Trim(),
                    coyID = coyID.Trim(),
                    UserName = string.Empty,
                    isClose = 0,
                    isBS = 1
                }, 600);
            }

            var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getBalanceSheetHeaders", new
            {
                CoyID = coyID.Trim(),
                period = period.Trim(),
                Year = year.Trim(),
                PrdType = rptBy.Trim()
            }, 600);

            var periodMeta = await DapperReportData.ExecuteDataSetAsync(conStr, @"
select top 1
    cast(PrdClose as datetime) as PrdClose
from AccountMonthOpen
where Period = @Period and CoyID = @CoyID", new
            {
                Period = period.Trim(),
                CoyID = coyID.Trim()
            }, 240);

            if (periodMeta.Tables.Count == 0
                || periodMeta.Tables[0].Rows.Count == 0
                || !periodMeta.Tables[0].Columns.Contains("PrdClose")
                || periodMeta.Tables[0].Rows[0]["PrdClose"] == DBNull.Value)
            {
                throw new Exception($"Period close date not found for Period '{period}' and CoyID '{coyID}'. Ensure the period exists in AccountMonthOpen.");
            }

            var reportDate = Convert.ToDateTime(periodMeta.Tables[0].Rows[0]["PrdClose"]);

            var header = $"As at {reportDate.ToShortDateString()}";
            return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header, companyName);
        }

        [Route("Accounting/GeneralLedger")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> GeneralLedger(string coyID, string period, string ledgerCode, string accountNo, string companyName = null, string ledgerDisplayText = null, string accountDisplayText = null)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(ledgerCode)) throw new ArgumentNullException(nameof(ledgerCode));
            if (string.IsNullOrWhiteSpace(accountNo)) throw new ArgumentNullException(nameof(accountNo));

            string reportPath = "~/Reports/Accounting";
            string reportFileName = "rptGL.rpt";
            string exportFilename = $"rptGL-{period}.pdf";

            try
            {
                var conStr = ResolveConnectionStringFromRequest(Request);
                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getGL", new
                {
                    CoyID = coyID.Trim(),
                    Period = period.Trim(),
                    LedgerCode = ledgerCode.Trim(),
                    AccountNo = accountNo.Trim()
                }, 240);

                var meta = await DapperReportData.ExecuteDataSetAsync(conStr, @"
select top 1
    cast(PrdClose as datetime) as PrdClose,
    cast(isClose as bit) as IsClose
from vwClosedAndOpenPeriods
where Period = @Period and CoyID = @CoyID", new
                {
                    Period = period.Trim(),
                    CoyID = coyID.Trim()
                }, 240);

                var reportDate = DateTime.Today;
                var isClose = false;
                if (meta.Tables.Count > 0 && meta.Tables[0].Rows.Count > 0)
                {
                    var row = meta.Tables[0].Rows[0];
                    if (row["PrdClose"] != DBNull.Value)
                    {
                        reportDate = Convert.ToDateTime(row["PrdClose"]);
                    }

                    if (row["IsClose"] != DBNull.Value)
                    {
                        isClose = Convert.ToBoolean(row["IsClose"]);
                    }
                }

                var displayText = BuildGeneralLedgerDisplayText(ledgerCode, accountNo, ledgerDisplayText, accountDisplayText);
                var header = BuildGeneralLedgerHeader(displayText, period, reportDate, isClose);
                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header, companyName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Route("Accounting/ProfitAndLoss")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> ProfitAndLoss(string coyID, string period, string year, string rptBy, bool isClose = false, string companyName = null)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptProfitAndLoss.rpt";
            const string exportFilename = "rptProfitAndLoss.pdf";

            var conStr = ResolveConnectionStringFromRequest(Request);

            if (!isClose)
            {
                await DapperReportData.ExecuteNonQueryAsync(conStr, "CloseAccountingPeriod", new
                {
                    Period = period.Trim(),
                    coyID = coyID.Trim(),
                    UserName = string.Empty,
                    isClose = 0,
                    isBS = 0
                }, 600);
            }

            var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getProfitAndLossHeaders", new
            {
                CoyID = coyID.Trim(),
                period = period.Trim(),
                Year = year.Trim(),
                PrdType = rptBy.Trim()
            }, 600);

            var periodCloseDate = DateTime.MinValue;
            if (string.Equals(rptBy.Trim(), "Period", StringComparison.OrdinalIgnoreCase))
            {
                var periodMeta = await DapperReportData.ExecuteDataSetAsync(conStr, @"
select top 1
    cast(PrdClose as datetime) as PrdClose
from AccountMonthOpen
where Period = @Period and CoyID = @CoyID", new
                {
                    Period = period.Trim(),
                    CoyID = coyID.Trim()
                }, 240);

                if (periodMeta.Tables.Count > 0
                    && periodMeta.Tables[0].Rows.Count > 0
                    && periodMeta.Tables[0].Columns.Contains("PrdClose")
                    && periodMeta.Tables[0].Rows[0]["PrdClose"] != DBNull.Value)
                {
                    periodCloseDate = Convert.ToDateTime(periodMeta.Tables[0].Rows[0]["PrdClose"]);
                }
            }

            var header = ReportHeaders.BuildProfitAndLossHeader(rptBy, year, period, string.Empty, string.Empty, isClose, periodCloseDate);
            return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header, companyName);
        }

        [Route("Accounting/ProfitAndLossDetails")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> ProfitAndLossDetails(string coyID, string period, string year, string rptBy, string groupID, bool isClose = false, string companyName = null)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));
            if (string.IsNullOrWhiteSpace(groupID)) throw new ArgumentNullException(nameof(groupID));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptGL.rpt";
            const string exportFilename = "rptProfitAndLossDetails.pdf";

            var conStr = ResolveConnectionStringFromRequest(Request);

            if (!isClose)
            {
                await DapperReportData.ExecuteNonQueryAsync(conStr, "CloseAccountingPeriod", new
                {
                    Period = period.Trim(),
                    coyID = coyID.Trim(),
                    UserName = string.Empty,
                    isClose = 0,
                    isBS = 0
                }, 600);
            }

            var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getGL_for_PL_Details", new
            {
                CoyID = coyID.Trim(),
                period = period.Trim(),
                Year = year.Trim(),
                PrdType = rptBy.Trim(),
                GroupID = groupID.Trim()
            }, 600);

            var periodCloseDate = DateTime.MinValue;
            if (string.Equals(rptBy.Trim(), "Period", StringComparison.OrdinalIgnoreCase))
            {
                var periodMeta = await DapperReportData.ExecuteDataSetAsync(conStr, @"
select top 1
    cast(PrdClose as datetime) as PrdClose
from AccountMonthOpen
where Period = @Period and CoyID = @CoyID", new
                {
                    Period = period.Trim(),
                    CoyID = coyID.Trim()
                }, 240);

                if (periodMeta.Tables.Count > 0
                    && periodMeta.Tables[0].Rows.Count > 0
                    && periodMeta.Tables[0].Columns.Contains("PrdClose")
                    && periodMeta.Tables[0].Rows[0]["PrdClose"] != DBNull.Value)
                {
                    periodCloseDate = Convert.ToDateTime(periodMeta.Tables[0].Rows[0]["PrdClose"]);
                }
            }

            var header = ReportHeaders.BuildProfitAndLossHeader(rptBy, year, period, groupID, string.Empty, isClose, periodCloseDate);
            return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header, companyName);
        }

        [Route("ClosedJob")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public async Task<HttpResponseMessage> ClosedJobAnalysis(string coyID, DateTime startDate, DateTime endDate, bool isLessDetls = false)
        {
            if (coyID is null)
            {
                throw new ArgumentNullException(nameof(coyID));
            }

            startDate = startDate.Date;
            endDate = endDate.Date;

            string reportPath;
            string reportFileName;
            string exportFilename;

            if (isLessDetls)
            {
                reportPath = "~/Reports/Billing";
                reportFileName = "rptClosedJobAnalysis2LessDetails.rpt";
                exportFilename = "rptClosedJobAnalysis2LessDetails.pdf";
            }
            else
            {
                reportPath = "~/Reports/Billing";
                reportFileName = "rptClosedJobAnalysis2.rpt";
                exportFilename = "rptClosedJobAnalysis2.pdf";
            }

            try
            {
                var conStr = ResolveConnectionStringFromRequest(Request);
                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "MonthlyClosedJobByCoy", new
                {
                    CoyID = coyID.Trim(),
                    StartDate = startDate,
                    EndDate = endDate
                }, 240);

                var strHeader = "Closed Job Analysis between " + startDate.ToShortDateString() + " and " + endDate.ToShortDateString();
                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, strHeader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Route("Invoice")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public async Task<HttpResponseMessage> InvoiceByBillNo(string invNo)
        {
            if (invNo is null)
            {
                throw new ArgumentNullException(nameof(invNo));
            }

            string reportPath = "~/Reports/Billing";
            string reportFileName = "rptBillsNormal2.rpt";
            string exportFilename = "rptBillsNormal2.pdf";

            try
            {
                var conStr = ResolveConnectionStringFromRequest(Request);
                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "GetInvoiceByBillNo", new
                {
                    BillNo = invNo.Trim()
                }, 240);

                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Route("Financial/VarianceAnalysisReport")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage FinancialVarianceAnalysisReport()
        {
            string reportPath = "~/Reports/Financial";
            string reportFileName = "YTDVarianceCrossTab.rpt";
            string exportFilename = "YTDVarianceCrossTab.pdf";

            DataSet ds = new DataSet();
            HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);

            return result;
        }

        [Route("Demonstration/ComparativeIncomeStatement")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage DemonstrationComparativeIncomeStatement()
        {
            string reportPath = "~/Reports/Demonstration";
            string reportFileName = "ComparativeIncomeStatement.rpt";
            string exportFilename = "ComparativeIncomeStatement.pdf";

            DataSet ds = new DataSet();
            HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);
            return result;
        }

        [Route("VersatileandPrecise/Invoice")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage VersatileandPreciseInvoice()
        {
            string reportPath = "~/Reports/VersatileandPrecise";
            string reportFileName = "Invoice.rpt";
            string exportFilename = "Invoice.pdf";

            DataSet ds = new DataSet();
            HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);
            return result;
        }

        [Route("VersatileandPrecise/FortifyFinancialAllinOneRetirementSavings")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage VersatileandPreciseFortifyFinancialAllinOneRetirementSavings()
        {
            string reportPath = "~/Reports/VersatileandPrecise";
            string reportFileName = "FortifyFinancialAllinOneRetirementSavings.rpt";
            string exportFilename = "FortifyFinancialAllinOneRetirementSavings.pdf";

            DataSet ds = new DataSet();
            HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);

            return result;
        }

        [Route("StaffRoster/Roster")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> Roster(string coyID, string month, string year, string deptID, bool isClose = false, string companyName = null)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(month)) throw new ArgumentNullException(nameof(month));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(deptID)) throw new ArgumentNullException(nameof(deptID));

            const string reportPath = "~/Reports/StaffRoster";
            const string reportFileName = "rptRosterForRptCrosstab.rpt";
            const string exportFilename = "rptRoster.pdf";

            try
            {
                // Staff Roster uses SmartHR database - connection passed via X-Db-Connection header
                var conStr = ResolveConnectionStringFromRequest(Request);

                // Convert month name to numeric (01-12) for vwRosterForRptCrosstab2 query
                var monthNum = ConvertMonthNameToNumeric(month.Trim());
                if (string.IsNullOrEmpty(monthNum))
                {
                    throw new Exception($"Invalid month name: '{month}'. Expected month name (e.g., 'January', 'July').");
                }

                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, @"
select distinct * from vwRosterForRptCrosstab2 where deptID=@DeptID and mth=@Mth and Yr=@Yr order by StaffName", new
                {
                    DeptID = deptID.Trim(),
                    Mth = monthNum,
                    Yr = year.Trim()
                }, 120);

                // Build header text: "Roster Details of {DeptName} Dept for {Month} {Year}"
                var deptName = string.Empty;
                var dm = await DapperReportData.ExecuteDataSetAsync(conStr, "select top 1 deptName from empDepartments where deptID=@DeptID", new { DeptID = deptID.Trim() }, 60);
                if (dm.Tables.Count > 0 && dm.Tables[0].Rows.Count > 0 && dm.Tables[0].Columns.Contains("deptName") && dm.Tables[0].Rows[0]["deptName"] != DBNull.Value)
                {
                    deptName = dm.Tables[0].Rows[0]["deptName"].ToString();
                }

                var headerText = $"Roster Details of {deptName} Dept for {month} {year}";
                var textObjects = new Dictionary<string, string>
                {
                    ["Text9"] = headerText
                };

                // Use companyName provided by caller; do not query local Company table (SmartHR DB doesn't contain it)
                var companyNameToUse = string.IsNullOrWhiteSpace(companyName) ? string.Empty : companyName;

                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, headerText, companyNameToUse, textObjects);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [Route("Admin/AuditTrail")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> AdminAuditTrail(DateTime fromDate, DateTime toDate, string filterType = "ALL", string filterValue = null, string filterDisplayText = null, string tranCode = null)
        {
            const string reportPath = "~/Reports";
            const string reportFileName = "rptAudiTrail.rpt";
            var exportFilename = $"rptAudiTrail-{fromDate:yyyyMMdd}-{toDate:yyyyMMdd}.pdf";

            var normalizedFilterType = string.IsNullOrWhiteSpace(filterType) ? "ALL" : filterType.Trim().ToUpperInvariant();
            if (normalizedFilterType != "ALL" && normalizedFilterType != "MODULE" && normalizedFilterType != "USER")
            {
                throw new ArgumentException("Invalid filter type.", nameof(filterType));
            }

            var conStr = ResolveConnectionStringFromRequest(Request);

            string sql;
            object parameters;

            if (!string.IsNullOrWhiteSpace(tranCode))
            {
                sql = @"
select distinct *
from vwAudiTrail
where TranCode = @TranCode
order by ID, module asc";
                parameters = new
                {
                    TranCode = tranCode.Trim()
                };
            }
            else
            {
                switch (normalizedFilterType)
                {
                    case "MODULE":
                        sql = @"
select distinct *
from vwAudiTrail
where module = @FilterValue
  and [date] between @FromDate and @ToDate
order by [date] desc, module asc";
                        parameters = new
                        {
                            FilterValue = (filterValue ?? string.Empty).Trim(),
                            FromDate = fromDate.Date,
                            ToDate = toDate.Date
                        };
                        break;

                    case "USER":
                        sql = @"
select distinct *
from vwAudiTrail
where username = @FilterValue
  and [date] between @FromDate and @ToDate
order by [date] desc, fullname asc";
                        parameters = new
                        {
                            FilterValue = (filterValue ?? string.Empty).Trim(),
                            FromDate = fromDate.Date,
                            ToDate = toDate.Date
                        };
                        break;

                    default:
                        sql = @"
select distinct *
from vwAudiTrail
where [date] between @FromDate and @ToDate
order by [date] desc";
                        parameters = new
                        {
                            FromDate = fromDate.Date,
                            ToDate = toDate.Date
                        };
                        break;
                }
            }

            try
            {
                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, sql, parameters, 240);
                var header = BuildAuditTrailHeader(normalizedFilterType, filterDisplayText, fromDate.Date, toDate.Date, tranCode);
                var textObjects = new Dictionary<string, string>
                {
                    ["Text10"] = header
                };

                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header, null, textObjects);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private static string ResolveConnectionStringFromRequest(HttpRequestMessage request)
        {
            const string headerName = "X-Db-Connection";

            if (!request.Headers.TryGetValues(headerName, out var values))
            {
                throw new InvalidOperationException($"Missing required header '{headerName}'.");
            }

            var connectionString = values.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Header '{headerName}' is empty.");
            }

            return connectionString;
        }

        private static string BuildAuditTrailHeader(string filterType, string filterDisplayText, DateTime fromDate, DateTime toDate, string tranCode)
        {
            if (!string.IsNullOrWhiteSpace(tranCode))
            {
                return $"AudiTrail for Transaction: {tranCode.Trim()}";
            }

            switch (filterType)
            {
                case "MODULE":
                    return $"AudiTrail for: {filterDisplayText?.Trim()} Module Between {fromDate:d} And {toDate:d}";
                case "USER":
                    return $"AudiTrail for User: {filterDisplayText?.Trim()} Between {fromDate:d} And {toDate:d}";
                default:
                    return $"AudiTrail for:  Between {fromDate:d} And {toDate:d}";
            }
        }

        private static string BuildGeneralLedgerHeader(string displayName, string period, DateTime reportDate, bool isClose)
        {
            if (isClose)
            {
                return $"{displayName} As of  {reportDate.ToShortDateString()} For the Period ended ({period.Trim()})";
            }

            if (reportDate.Date >= DateTime.Today)
            {
                return $"{displayName}  As of  {DateTime.Today}";
            }

            return $"{displayName} As of  {reportDate.ToShortDateString()}";
        }

        private static string BuildGeneralLedgerDisplayText(string ledgerCode, string accountNo, string ledgerDisplayText, string accountDisplayText)
        {
            var acct = accountNo == null ? string.Empty : accountNo.Trim();
            if (!string.IsNullOrWhiteSpace(acct) && !string.Equals(acct, "(ALL)", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(accountDisplayText))
                {
                    return accountDisplayText.Trim();
                }

                return acct;
            }

            if (!string.IsNullOrWhiteSpace(ledgerDisplayText))
            {
                return ledgerDisplayText.Trim();
            }

            return ledgerCode == null ? string.Empty : ledgerCode.Trim();
        }

        private static string ConvertMonthNameToNumeric(string monthName)
        {
            switch (monthName.ToLowerInvariant())
            {
                case "january":
                    return "01";
                case "february":
                    return "02";
                case "march":
                    return "03";
                case "april":
                    return "04";
                case "may":
                    return "05";
                case "june":
                    return "06";
                case "july":
                    return "07";
                case "august":
                    return "08";
                case "september":
                    return "09";
                case "october":
                    return "10";
                case "november":
                    return "11";
                case "december":
                    return "12";
                default:
                    return null;
            }
        }
    }
}