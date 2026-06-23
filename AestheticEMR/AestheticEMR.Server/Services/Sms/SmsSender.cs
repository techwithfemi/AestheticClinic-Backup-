// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// SMS: Using Twilio for SMS integration
// Documentation: https://www.twilio.com/docs/sms
// ---------------------------------------

using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace AestheticEMR.Server.Services.Sms
{
    public class SmsSender(IOptions<AppSettings> configOptions, ILogger<SmsSender> logger) : ISmsSender
    {
        private readonly SmsConfig _smsConfig = configOptions.Value.SmsConfig
            ?? throw new InvalidOperationException("SmsConfig is not configured in appsettings.json");

        private static readonly HttpClient _httpClient = new();
        private const string TwilioApiUrl = "https://api.twilio.com/2010-04-01";

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.StartsWith("+") && phoneNumber.Length >= 10 &&
                   phoneNumber[1..].All(char.IsDigit);
        }

        public async Task<(bool success, string? messageId, string? errorMsg)> SendSmsMessageAsync(
            string recipientPhoneNumber,
            string messageBody)
        {
            if (!_smsConfig.Enabled)
            {
                logger.LogWarning("SMS messaging is disabled");
                return (false, null, "SMS messaging is disabled in configuration");
            }

            if (string.IsNullOrWhiteSpace(recipientPhoneNumber))
            {
                logger.LogError("Recipient phone number is empty");
                return (false, null, "Recipient phone number cannot be empty");
            }

            if (!IsValidPhoneNumber(recipientPhoneNumber))
            {
                logger.LogError("Invalid phone number format: {PhoneNumber}. Must start with + and contain only digits.", recipientPhoneNumber);
                return (false, null, "Invalid phone number format. Expected format: +1234567890");
            }

            if (string.IsNullOrWhiteSpace(messageBody))
            {
                logger.LogError("Message body is empty");
                return (false, null, "Message body cannot be empty");
            }

            try
            {
                logger.LogInformation("Attempting to send SMS to {RecipientPhone} with message length {MessageLength}",
                    recipientPhoneNumber, messageBody.Length);

                var messageId = await SendSmsViaTwilioAsync(recipientPhoneNumber, messageBody);

                if (messageId != null)
                {
                    logger.LogInformation("SMS sent successfully to {RecipientPhone} with message ID: {MessageId}",
                        recipientPhoneNumber, messageId);
                    return (true, messageId, null);
                }

                return (false, null, "Failed to send SMS - no message ID returned");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending SMS to {RecipientPhone}: {ErrorMessage}", recipientPhoneNumber, ex.Message);
                return (false, null, $"Error sending SMS: {ex.Message}");
            }
        }

        private async Task<string?> SendSmsViaTwilioAsync(string toPhoneNumber, string messageBody)
        {
            try
            {
                var accountSid = _smsConfig.AccountSid;
                var authToken = _smsConfig.AuthToken;
                var fromPhone = _smsConfig.FromPhoneNumber;

                var url = $"{TwilioApiUrl}/Accounts/{accountSid}/Messages.json";

                var requestBody = new Dictionary<string, string>
                {
                    { "From", fromPhone },
                    { "To", toPhoneNumber },
                    { "Body", messageBody }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(requestBody)
                };

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountSid}:{authToken}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                using var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    logger.LogDebug("Twilio SMS API response: {Response}", responseContent);

                    if (responseContent.Contains("\"sid\""))
                    {
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

                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("Twilio SMS API error ({StatusCode}): {ErrorResponse}",
                    response.StatusCode, errorContent);
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception calling Twilio SMS API: {ErrorMessage}", ex.Message);
                return null;
            }
        }
    }
}
