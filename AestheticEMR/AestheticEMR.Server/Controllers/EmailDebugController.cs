// Test/Debug Controller for Email Configuration
// This controller is for development/testing purposes only

using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Controllers
{
    /// <summary>
    /// Debug controller to test email configuration and sending
    /// Remove this controller before deploying to production
    /// </summary>
    [Route("api/debug")]
    [ApiController]
    public class EmailDebugController : BaseApiController
    {
        private readonly IEmailSender _emailSender;
        private readonly IOptions<AppSettings> _appSettings;

        public EmailDebugController(ILogger<EmailDebugController> logger, IMapper mapper,
            IEmailSender emailSender, IOptions<AppSettings> appSettings) : base(logger, mapper)
        {
            _emailSender = emailSender;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Check SMTP configuration (no authentication required for testing)
        /// </summary>
        [HttpGet("check-smtp-config")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult CheckSmtpConfig()
        {
            try
            {
                var config = _appSettings.Value.SmtpConfig;

                if (config == null)
                    return Ok(new { status = "ERROR", message = "SmtpConfig is null", config = (object?)null });

                var result = new
                {
                    status = "OK",
                    message = "SMTP Configuration loaded",
                    config = new
                    {
                        host = config.Host,
                        port = config.Port,
                        useSSL = config.UseSSL,
                        emailAddress = config.EmailAddress,
                        name = config.Name,
                        username = config.Username ?? "(not configured)",
                        hasPassword = !string.IsNullOrWhiteSpace(config.Password)
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking SMTP config");
                return StatusCode(500, new { status = "ERROR", message = ex.Message });
            }
        }

        /// <summary>
        /// Send a test email to verify SMTP is working
        /// </summary>
        [HttpPost("send-test-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendTestEmail([FromQuery] string testEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(testEmail))
                    return BadRequest(new { status = "ERROR", message = "testEmail query parameter is required" });

                _logger.LogInformation("Sending test email to: {TestEmail}", testEmail);

                var testBody = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body>
    <h2>Test Email from AestheticClinic EMR</h2>
    <p>This is a test email to verify SMTP configuration is working correctly.</p>
    <p>If you received this email, your email system is configured properly.</p>
    <p>Timestamp: " + DateTime.Now.ToString("O") + @"</p>
</body>
</html>";

                var result = await _emailSender.SendEmailAsync(
                    "AestheticClinic EMR",
                    testEmail,
                    "Test Email - AestheticClinic EMR",
                    testBody,
                    isHtml: true);

                if (!result.success)
                    return StatusCode(500, new { status = "ERROR", message = result.errorMsg });

                return Ok(new { status = "SUCCESS", message = "Test email sent successfully", email = testEmail });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                return StatusCode(500, new { status = "ERROR", message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
