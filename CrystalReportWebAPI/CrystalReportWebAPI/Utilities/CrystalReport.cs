using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient;
using System;

namespace CrystalReportWebAPI.Utilities
{
    public static class CrystalReport
    {
        public static HttpResponseMessage RenderReport(string reportPath, string reportFileName, string exportFilename,DataSet dsX, string header=null )
        {
            //DataTable dt = new DataTable();
            //dt = dsX.Tables[0];

            var rd = new ReportDocument();
            var fielName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName);


            //Check file exists
            if (!System.IO.File.Exists(fielName))
                throw (new Exception("Unable to locate report file:+\n" + fielName));

            rd.Load(fielName);
            
            TableLogOnInfo logOnInfo = new TableLogOnInfo();
            logOnInfo = rd.Database.Tables[0].LogOnInfo;
            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo = logOnInfo.ConnectionInfo;

            connectionInfo.DatabaseName = "db_a66d0c_clearing";
            connectionInfo.ServerName = "SQL5090.site4now.net";
            connectionInfo.Password = "khide@123";
            connectionInfo.UserID = "db_a66d0c_clearing_admin";
            rd.Database.Tables[0].ApplyLogOnInfo(logOnInfo);
            rd.SetDataSource(dsX.Tables[0]);

            if (header!= null )
            {
                TextObject myTextObjectOnReport;
                myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)rd.ReportDefinition.ReportObjects["txtHead"];
                myTextObjectOnReport.Text = header ;

                myTextObjectOnReport = (CrystalDecisions.CrystalReports.Engine.TextObject)rd.ReportDefinition.ReportObjects["txtCoy"];
                myTextObjectOnReport.Text = "Sapid Agencies Ltd";
            }


            MemoryStream ms = new MemoryStream();
            using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
            {
                stream.CopyTo(ms);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportFilename
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return result;
        }
    }
}