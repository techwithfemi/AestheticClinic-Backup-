using CrystalReportWebAPI.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using SmartPay.NAPS.SQLSeverDAL;

namespace CrystalReportWebAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {


        [AllowAnonymous]
        [Route("ClosedJob")]
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public async Task<HttpResponseMessage> ClosedJobAnalysis(string coyID,DateTime startDate, DateTime endDate,bool isLessDetls=false  )
        //public async Task<ActionResult<Billing_Extension>> PostBilling(Billing_Extension billing)
        {
            if (coyID  is null)
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

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlTransaction sqlTran = null;
            try
            {
                //Dim strConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConStr").ToString()
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(conStr))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sqlTran = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = sqlTran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "MonthlyClosedJobByCoy";
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@CoyID",coyID .Trim()),
                            new SqlParameter("@StartDate",startDate ),
                            new SqlParameter("@EndDate",endDate )
                        };
                        cmd.Parameters.AddRange(parameters);

                        da.SelectCommand = cmd;
                        da.SelectCommand.CommandTimeout = 240;
                        //da.Fill(ds);
                        await Task.Run(() => da.Fill(ds));
                        cmd.Parameters.Clear();

                        sqlTran.Commit();
                        conn.Close();
                    }

                }

                var cnt = ds.Tables[0].Rows.Count;
                var strHeader= "Closed Job Analysis between " + startDate.ToShortDateString()  + " and " + endDate.ToShortDateString();
                HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, strHeader);
                return result;

            }

            catch (Exception ex)
            {
                // Handle the exception if the transaction fails to commit.
                if (sqlTran != null)
                {
                    //WatchLogger.Log(ex.Message);
                    throw new Exception(ex.Message);
                    sqlTran.Rollback();
                }

                throw new Exception(ex.Message);

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

            DataSet ds=new DataSet();
            SqlDataAdapter da=new SqlDataAdapter();
            SqlTransaction sqlTran = null;
            try
            {
                //Dim strConn As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConStr").ToString()
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ToString() ;

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(conStr))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sqlTran = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = sqlTran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetInvoiceByBillNo";
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@BillNo",invNo.Trim())
                        };
                        cmd.Parameters.AddRange(parameters);
                        
                        da.SelectCommand = cmd;
                        da.SelectCommand.CommandTimeout = 240;
                        //da.Fill(ds);
                        await Task.Run(() => da.Fill(ds));
                        cmd.Parameters.Clear();

                        sqlTran.Commit();
                        conn.Close();
                    }

                }
                var cnt = ds.Tables[0].Rows.Count;
                HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename,ds);
                return result;

            }

            catch (Exception ex)
            {
                // Handle the exception if the transaction fails to commit.
                if (sqlTran != null)
                {
                    //WatchLogger.Log(ex.Message);
                    throw new Exception(ex.Message);
                    sqlTran.Rollback();
                }

                throw new Exception(ex.Message);

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