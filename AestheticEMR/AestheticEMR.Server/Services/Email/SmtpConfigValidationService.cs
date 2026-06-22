// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Email
{
    /// <summary>
    /// Service that validates SMTP configuration is properly loaded on application startup
    /// </summary>
    public class SmtpConfigValidationService : IHostedService
    {
        private readonly ILogger<SmtpConfigValidationService> _logger;
        private readonly IOptions<AppSettings> _appSettings;

        public SmtpConfigValidationService(ILogger<SmtpConfigValidationService> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var smtpConfig = _appSettings.Value.SmtpConfig;

            if (smtpConfig == null)
            {
                _logger.LogError("⚠️ SmtpConfig is NULL! Email sending will not work. Check your appsettings.json file.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(smtpConfig.Host))
            {
                _logger.LogError("⚠️ SmtpConfig.Host is empty! Email sending will not work.");
                return Task.CompletedTask;
            }

            if (smtpConfig.Port <= 0)
            {
                _logger.LogError("⚠️ SmtpConfig.Port is invalid (must be > 0)! Email sending will not work.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(smtpConfig.EmailAddress))
            {
                _logger.LogError("⚠️ SmtpConfig.EmailAddress is empty! Email sending will not work.");
                return Task.CompletedTask;
            }

            _logger.LogInformation("✅ SMTP Configuration validated successfully:");
            _logger.LogInformation("   Host: {Host}", smtpConfig.Host);
            _logger.LogInformation("   Port: {Port}", smtpConfig.Port);
            _logger.LogInformation("   UseSSL: {UseSSL}", smtpConfig.UseSSL);
            _logger.LogInformation("   Email Address: {EmailAddress}", smtpConfig.EmailAddress);
            _logger.LogInformation("   Username: {Username}", smtpConfig.Username ?? "Not configured");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
