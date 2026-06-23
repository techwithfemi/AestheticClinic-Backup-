// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace AestheticEMR.Core.Services
{
    public interface ISmsSender
    {
        /// <summary>
        /// Sends an SMS message to a recipient
        /// </summary>
        /// <param name="recipientPhoneNumber">Recipient phone number in international format (e.g., +1234567890)</param>
        /// <param name="messageBody">Message content</param>
        /// <returns>Tuple with success status, message ID and optional error message</returns>
        Task<(bool success, string? messageId, string? errorMsg)> SendSmsMessageAsync(
            string recipientPhoneNumber,
            string messageBody);
    }
}
