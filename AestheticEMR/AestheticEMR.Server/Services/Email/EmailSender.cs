// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AestheticEMR.Server.Services.Email
{
    public class EmailSender(IOptions<AppSettings> configOptions, ILogger<EmailSender> logger) : IEmailSender
    {
        private readonly SmtpConfig _smtpConfig = configOptions.Value.SmtpConfig 
            ?? throw new InvalidOperationException("SmtpConfig is not configured in appsettings.json");

        public async Task<(bool success, string? errorMsg)> SendEmailAsync(
            string recipientName,
            string recipientEmail,
            string subject,
            string body,
            bool isHtml = true)
        {
            var from = new MailboxAddress(_smtpConfig.Name, _smtpConfig.EmailAddress);
            var to = new MailboxAddress(recipientName, recipientEmail);

            return await SendEmailAsync(from, [to], subject, body, isHtml);
        }

        public async Task<(bool success, string? errorMsg)> SendEmailAsync(
            string senderName,
            string senderEmail,
            string recipientName,
            string recipientEmail,
            string subject,
            string body,
            bool isHtml = true)
        {
            var from = new MailboxAddress(senderName, senderEmail);
            var to = new MailboxAddress(recipientName, recipientEmail);

            return await SendEmailAsync(from, [to], subject, body, isHtml);
        }

        // For background tasks such as sending emails, its good practice to use job runners such
        // as hangfire https://www.hangfire.io or a service such as SendGrid https://sendgrid.com/
        public async Task<(bool success, string? errorMsg)> SendEmailAsync(
            MailboxAddress sender,
            MailboxAddress[] recipients,
            string subject,
            string body,
            bool isHtml = true)
        {
            var message = new MimeMessage();

            message.From.Add(sender);
            message.To.AddRange(recipients);
            message.Subject = subject;
            message.Body = isHtml ?
                new BodyBuilder { HtmlBody = body }.ToMessageBody() :
                new TextPart("plain") { Text = body };

            try
            {
                logger.LogInformation("Attempting to send email to {RecipientEmails} with subject '{Subject}' from {SenderEmail}",
                    string.Join(", ", recipients.Select(r => r.Address)), subject, sender.Address);

                using (var client = new SmtpClient())
                {
                    // Disable certificate validation if needed
                    if (!_smtpConfig.UseSSL)
                    {
                        client.ServerCertificateValidationCallback =
                            (sender2, certificate, chain, sslPolicyErrors) => true;
                    }

                    // Determine the security option based on port and UseSSL setting
                    SecureSocketOptions secureSocketOptions;
                    if (_smtpConfig.UseSSL)
                    {
                        secureSocketOptions = SecureSocketOptions.SslOnConnect; // Port 465
                        logger.LogInformation("Connecting with SSL/TLS on connect (port {Port})", _smtpConfig.Port);
                    }
                    else if (_smtpConfig.Port == 587)
                    {
                        secureSocketOptions = SecureSocketOptions.StartTls; // Port 587 - StartTLS
                        logger.LogInformation("Connecting with StartTLS upgrade (port {Port})", _smtpConfig.Port);
                    }
                    else
                    {
                        secureSocketOptions = SecureSocketOptions.None; // Port 25 or 2525 - No encryption
                        logger.LogInformation("Connecting without encryption (port {Port})", _smtpConfig.Port);
                    }

                    await client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, secureSocketOptions).ConfigureAwait(false);
                    logger.LogInformation("Connected to SMTP server {SmtpHost}:{SmtpPort} with security: {SecurityOption}", 
                        _smtpConfig.Host, _smtpConfig.Port, secureSocketOptions);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    if (!string.IsNullOrWhiteSpace(_smtpConfig.Username))
                    {
                        logger.LogInformation("Authenticating with username: {Username}", _smtpConfig.Username);
                        await client.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password).ConfigureAwait(false);
                        logger.LogInformation("Successfully authenticated with SMTP server as {Username}", _smtpConfig.Username);
                    }

                    await client.SendAsync(message).ConfigureAwait(false);
                    logger.LogInformation("Email sent successfully to {RecipientEmails}", string.Join(", ", recipients.Select(r => r.Address)));

                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred whilst sending email to {RecipientEmails} with subject '{Subject}'",
                    string.Join(", ", recipients.Select(r => r.Address)), subject);
                return (false, ex.Message);
            }
        }
    }
}
