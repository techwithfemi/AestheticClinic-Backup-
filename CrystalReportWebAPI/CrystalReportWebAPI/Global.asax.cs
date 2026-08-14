using CrystalReportWebAPI.Utilities;
using Serilog;
using Serilog.Events;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CrystalReportWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureSerilog();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log.Information("CrystalReportWebAPI started");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                Log.Error(exception, "Unhandled application error");
            }
        }

        protected void Application_End()
        {
            Log.Information("CrystalReportWebAPI shutting down");
            Log.CloseAndFlush();
        }

        private static void ConfigureSerilog()
        {
            var connectionString = ConnectionStringResolver.ResolveDefaultConnection();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Trace.TraceWarning("Connection string 'DefaultConnection' was not found in environment variables or user secrets. SQL logging is disabled.");
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .CreateLogger();
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    tableName: "Logs",
                    autoCreateSqlTable: false,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();
        }
    }
}