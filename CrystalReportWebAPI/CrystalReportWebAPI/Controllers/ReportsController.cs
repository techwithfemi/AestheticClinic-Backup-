using CrystalReportWebAPI.Utilities;
using Dapper;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrystalReportWebAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        [AllowAnonymous]
        [Route("Accounting/BalanceSheet")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> BalanceSheet(string coyID, string period, string year, string rptBy, bool isClose = false)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptBalSheet.rpt";
            var exportFilename = $"rptBalSheet-{period}.pdf";

            try
            {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                if (!isClose)
                {
                    await DapperReportData.ExecuteDataSetAsync(conStr, "CloseAccountingPeriod", new
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

                var header = $"As at {period.Trim()}";
                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [Route("Accounting/GeneralLedger")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> GeneralLedger(string coyID, string period, string ledgerCode, string accountNo)
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
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getGL", new
                {
                    CoyID = coyID.Trim(),
                    Period = period.Trim(),
                    LedgerCode = ledgerCode.Trim(),
                    AccountNo = accountNo.Trim()
                }, 240);

                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [Route("Accounting/ProfitAndLoss")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> ProfitAndLoss(string coyID, string period, string year, string rptBy, bool isClose = false)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptProfitAndLoss.rpt";
            const string exportFilename = "rptProfitAndLoss.pdf";

            try
            {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                if (!isClose)
                {
                    await DapperReportData.ExecuteDataSetAsync(conStr, "CloseAccountingPeriod", new
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

                var header = ReportHeaders.BuildProfitAndLossHeader(rptBy, year, period, string.Empty, string.Empty, isClose, DateTime.Today);
                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [Route("Accounting/ProfitAndLossDetails")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public async Task<HttpResponseMessage> ProfitAndLossDetails(string coyID, string period, string year, string rptBy, string groupID, bool isClose = false)
        {
            if (string.IsNullOrWhiteSpace(coyID)) throw new ArgumentNullException(nameof(coyID));
            if (string.IsNullOrWhiteSpace(period)) throw new ArgumentNullException(nameof(period));
            if (string.IsNullOrWhiteSpace(year)) throw new ArgumentNullException(nameof(year));
            if (string.IsNullOrWhiteSpace(rptBy)) throw new ArgumentNullException(nameof(rptBy));
            if (string.IsNullOrWhiteSpace(groupID)) throw new ArgumentNullException(nameof(groupID));

            const string reportPath = "~/Reports/Accounting";
            const string reportFileName = "rptGL.rpt";
            const string exportFilename = "rptProfitAndLossDetails.pdf";

            try
            {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                if (!isClose)
                {
                    await DapperReportData.ExecuteDataSetAsync(conStr, "CloseAccountingPeriod", new
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

                var header = ReportHeaders.BuildProfitAndLossHeader(rptBy, year, period, string.Empty, string.Empty, isClose, DateTime.Today);
                return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
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
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();
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

        [AllowAnonymous]
        [Route("Invoice")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public async Task<HttpResponseMessage> InvoiceByBillNo(string invNo)
        //public async Task<ActionResult<Billing_Extension>> PostBilling(Billing_Extension billing)
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
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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
    }
}