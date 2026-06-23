// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace AestheticEMR.Core.Services
{
    public interface IWhatsAppSender
    {
        /// <summary>
        /// Sends a WhatsApp message to a recipient
        /// </summary>
        /// <param name="recipientPhoneNumber">Recipient phone number in international format (e.g., +1234567890)</param>
        /// <param name="messageBody">Message content</param>
        /// <returns>Tuple with success status and optional error message</returns>
        Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppMessageAsync(
            string recipientPhoneNumber,
            string messageBody);

        /// <summary>
        /// Sends a WhatsApp message with a template
        /// </summary>
        /// <param name="recipientPhoneNumber">Recipient phone number in international format</param>
        /// <param name="templateName">Name of the template</param>
        /// <param name="templateVariables">Variables to substitute in the template</param>
        /// <returns>Tuple with success status, message ID and optional error message</returns>
        Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppTemplateMessageAsync(
            string recipientPhoneNumber,
            string templateName,
            params string[] templateVariables);

        /// <summary>
        /// Sends a WhatsApp message with media (image, document, etc.)
        /// </summary>
        /// <param name="recipientPhoneNumber">Recipient phone number in international format</param>
        /// <param name="messageBody">Message text</param>
        /// <param name="mediaUrl">URL of the media to send</param>
        /// <param name="mediaType">Type of media (image, document, audio, video)</param>
        /// <returns>Tuple with success status, message ID and optional error message</returns>
        Task<(bool success, string? messageId, string? errorMsg)> SendWhatsAppMessageWithMediaAsync(
            string recipientPhoneNumber,
            string messageBody,
            string mediaUrl,
            string mediaType = "image");
    }

    public enum WhatsAppMediaType
    {
        Image,
        Document,
        Audio,
        Video
    }
}
