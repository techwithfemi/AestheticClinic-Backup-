// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// WhatsApp: Using Twilio for WhatsApp integration
// Documentation: https://www.twilio.com/docs/whatsapp
// ---------------------------------------

using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace AestheticEMR.Server.Services.WhatsApp
{
    public class WhatsAppSender(IOptions<AppSettings> configOptions, ILogger<WhatsAppSender> logger) : IWhatsAppSender
    {
        private readonly WhatsAppConfig _whatsAppConfig = configOptions.Value.WhatsAppConfig 
            ?? throw new InvalidOperationException("WhatsAppConfig is not configured in appsettings.json");

        private static readonly HttpClient _httpClient = new();
        private const string TwilioApiUrl = "https://api.twilio.com/2010-04-01";

        /// <summary>
        /// Validates that a phone number is in the correct format
        /// </summary>
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Phone number must start with + and contain only digits
            return phoneNumber.StartsWith("+") && phoneNumber.Length >= 10 && 
                   phoneNumber[1..].All(char.IsDigit);
        }

        /// <summary>
        /// Sends a WhatsApp message to a recipient
        /// </summary>
        public async Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppMessageAsync(
            string recipientPhoneNumber,
            string messageBody)
        {
            if (!_whatsAppConfig.Enabled)
            {
                logger.LogWarning("WhatsApp messaging is disabled");
                return (false, null, "WhatsApp messaging is disabled in configuration");
            }

            if (string.IsNullOrWhiteSpace(recipientPhoneNumber))
            {
                logger.LogError("Recipient phone number is empty");
                return (false, null, "Recipient phone number cannot be empty");
            }

            if (!IsValidPhoneNumber(recipientPhoneNumber))
            {
                logger.LogError("Invalid phone number format: {PhoneNumber}. Must start with + and contain only digits.", recipientPhoneNumber);
                return (false, null, $"Invalid phone number format. Expected format: +1234567890");
            }

            if (string.IsNullOrWhiteSpace(messageBody))
            {
                logger.LogError("Message body is empty");
                return (false, null, "Message body cannot be empty");
            }

            try
            {
                logger.LogInformation("Attempting to send WhatsApp message to {RecipientPhone} with message length {MessageLength}",
                    recipientPhoneNumber, messageBody.Length);

                var messageId = await SendWhatsAppMessageViaTwilioAsync(
                    FormatPhoneNumber(recipientPhoneNumber),
                    messageBody,
                    null);

                if (messageId != null)
                {
                    logger.LogInformation("WhatsApp message sent successfully to {RecipientPhone} with message ID: {MessageId}",
                        recipientPhoneNumber, messageId);
                    return (true, messageId, null);
                }

                return (false, null, "Failed to send WhatsApp message - no message ID returned");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending WhatsApp message to {RecipientPhone}: {ErrorMessage}",
                    recipientPhoneNumber, ex.Message);
                return (false, null, $"Error sending WhatsApp message: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a WhatsApp message with a template
        /// </summary>
        public async Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppTemplateMessageAsync(
            string recipientPhoneNumber,
            string templateName,
            params string[] templateVariables)
        {
            if (!_whatsAppConfig.Enabled)
            {
                logger.LogWarning("WhatsApp messaging is disabled");
                return (false, null, "WhatsApp messaging is disabled in configuration");
            }

            if (string.IsNullOrWhiteSpace(recipientPhoneNumber) || !IsValidPhoneNumber(recipientPhoneNumber))
            {
                logger.LogError("Invalid recipient phone number: {PhoneNumber}", recipientPhoneNumber);
                return (false, null, "Invalid phone number format");
            }

            try
            {
                logger.LogInformation("Attempting to send WhatsApp template message '{TemplateName}' to {RecipientPhone}",
                    templateName, recipientPhoneNumber);

                // Get the template body
                var templateBody = WhatsAppTemplates.GetTemplate(templateName, templateVariables);

                if (templateBody == null)
                {
                    logger.LogError("WhatsApp template '{TemplateName}' not found", templateName);
                    return (false, null, $"Template '{templateName}' not found");
                }

                var messageId = await SendWhatsAppMessageViaTwilioAsync(
                    FormatPhoneNumber(recipientPhoneNumber),
                    templateBody,
                    null);

                if (messageId != null)
                {
                    logger.LogInformation("WhatsApp template message sent successfully to {RecipientPhone} with message ID: {MessageId}",
                        recipientPhoneNumber, messageId);
                    return (true, messageId, null);
                }

                return (false, null, "Failed to send WhatsApp template message");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending WhatsApp template message '{TemplateName}' to {RecipientPhone}: {ErrorMessage}",
                    templateName, recipientPhoneNumber, ex.Message);
                return (false, null, $"Error sending template message: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a WhatsApp message with media
        /// </summary>
        public async Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppMessageWithMediaAsync(
            string recipientPhoneNumber,
            string messageBody,
            string mediaUrl,
            string mediaType = "image")
        {
            if (!_whatsAppConfig.Enabled)
            {
                logger.LogWarning("WhatsApp messaging is disabled");
                return (false, null, "WhatsApp messaging is disabled in configuration");
            }

            if (string.IsNullOrWhiteSpace(recipientPhoneNumber) || !IsValidPhoneNumber(recipientPhoneNumber))
            {
                logger.LogError("Invalid recipient phone number: {PhoneNumber}", recipientPhoneNumber);
                return (false, null, "Invalid phone number format");
            }

            if (string.IsNullOrWhiteSpace(mediaUrl))
            {
                logger.LogError("Media URL is empty");
                return (false, null, "Media URL cannot be empty");
            }

            try
            {
                logger.LogInformation("Attempting to send WhatsApp message with media to {RecipientPhone} (type: {MediaType})",
                    recipientPhoneNumber, mediaType);

                var messageId = await SendWhatsAppMessageViaTwilioAsync(
                    FormatPhoneNumber(recipientPhoneNumber),
                    messageBody,
                    new { url = mediaUrl, type = mediaType });

                if (messageId != null)
                {
                    logger.LogInformation("WhatsApp message with media sent successfully to {RecipientPhone} with message ID: {MessageId}",
                        recipientPhoneNumber, messageId);
                    return (true, messageId, null);
                }

                return (false, null, "Failed to send WhatsApp message with media");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending WhatsApp message with media to {RecipientPhone}: {ErrorMessage}",
                    recipientPhoneNumber, ex.Message);
                return (false, null, $"Error sending message with media: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends the actual WhatsApp message via Twilio API
        /// </summary>
        private async Task<string?> SendWhatsAppMessageViaTwilioAsync(
            string toPhoneNumber,
            string messageBody,
            object? mediaPayload)
        {
            try
            {
                var accountSid = _whatsAppConfig.AccountSid;
                var authToken = _whatsAppConfig.AuthToken;
                var fromPhone = FormatPhoneNumber(_whatsAppConfig.FromPhoneNumber);

                // Prepare the request URL
                var url = $"{TwilioApiUrl}/Accounts/{accountSid}/Messages.json";

                // Prepare request body
                var requestBody = new Dictionary<string, string>
                {
                    { "From", fromPhone },
                    { "To", toPhoneNumber },
                    { "Body", messageBody }
                };

                // Add media if provided
                if (mediaPayload is { })
                {
                    dynamic media = mediaPayload;
                    requestBody["MediaUrl"] = media.url;
                }

                // Create the HTTP request with Basic Auth
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(requestBody)
                };

                // Add Twilio Basic Authentication
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountSid}:{authToken}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                // Send the request
                using var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    logger.LogDebug("Twilio API response: {Response}", responseContent);

                    // Parse the response to extract message SID
                    if (responseContent.Contains("\"sid\""))
                    {
                        // Simple extraction of SID from JSON response
                        var sidStart = responseContent.IndexOf("\"sid\"");
                        if (sidStart > -1)
                        {
                            var sidValueStart = responseContent.IndexOf("\"", sidStart + 6);
                            var sidValueEnd = responseContent.IndexOf("\"", sidValueStart + 1);
                            if (sidValueStart > -1 && sidValueEnd > sidValueStart)
                            {
                                return responseContent.Substring(sidValueStart + 1, sidValueEnd - sidValueStart - 1);
                            }
                        }
                    }

                    return "message_sent";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    logger.LogError("Twilio API error ({StatusCode}): {ErrorResponse}",
                        response.StatusCode, errorContent);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception calling Twilio API: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Formats a phone number to Twilio WhatsApp format
        /// </summary>
        private string FormatPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith("whatsapp:"))
                return phoneNumber;

            if (!phoneNumber.StartsWith("+"))
                return $"whatsapp:+{phoneNumber.TrimStart('+')}";

            return $"whatsapp:{phoneNumber}";
        }
    }
}
