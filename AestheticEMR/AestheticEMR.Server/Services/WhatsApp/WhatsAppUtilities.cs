// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// WhatsApp Utilities
// ---------------------------------------

using System.Text.RegularExpressions;

namespace AestheticEMR.Server.Services.WhatsApp
{
    /// <summary>
    /// Utility class for WhatsApp messaging operations
    /// </summary>
    public static class WhatsAppUtilities
    {
        /// <summary>
        /// Validates a phone number for WhatsApp messaging
        /// </summary>
        /// <param name="phoneNumber">Phone number to validate</param>
        /// <returns>True if valid for WhatsApp, false otherwise</returns>
        public static bool IsValidWhatsAppPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove all non-digit characters except leading +
            var cleaned = Regex.Replace(phoneNumber, "[^0-9+]", "");

            // Must start with + and have at least 10 digits
            if (!cleaned.StartsWith("+"))
                return false;

            var digitsOnly = cleaned[1..];
            return digitsOnly.Length >= 10 && digitsOnly.All(char.IsDigit);
        }

        /// <summary>
        /// Normalizes a phone number to E.164 format (+[country code][number])
        /// </summary>
        /// <param name="phoneNumber">Phone number to normalize</param>
        /// <returns>Normalized phone number or null if invalid</returns>
        public static string? NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return null;

            // Remove all non-digit characters except leading +
            var cleaned = Regex.Replace(phoneNumber, "[^0-9+]", "");

            // If doesn't start with +, assume US number if it's 10 digits
            if (!cleaned.StartsWith("+"))
            {
                var digitsOnly = Regex.Replace(cleaned, "[^0-9]", "");
                if (digitsOnly.Length == 10)
                    return $"+1{digitsOnly}";
                else if (digitsOnly.Length > 10)
                    return $"+{digitsOnly}";
                else
                    return null;
            }

            return cleaned;
        }

        /// <summary>
        /// Truncates message to WhatsApp limit (usually 4096 characters)
        /// </summary>
        /// <param name="message">Message to truncate</param>
        /// <param name="maxLength">Maximum length (default: 4096)</param>
        /// <returns>Truncated message</returns>
        public static string TruncateMessage(string message, int maxLength = 4096)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            if (message.Length <= maxLength)
                return message;

            return message[..maxLength] + "...";
        }

        /// <summary>
        /// Escapes special characters in message body
        /// </summary>
        /// <param name="message">Message to escape</param>
        /// <returns>Escaped message</returns>
        public static string EscapeMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            return message
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t");
        }

        /// <summary>
        /// Formats a message with emojis and structure for better readability
        /// </summary>
        /// <param name="title">Title/heading</param>
        /// <param name="content">Main content</param>
        /// <param name="footer">Optional footer/call to action</param>
        /// <returns>Formatted message</returns>
        public static string FormatStructuredMessage(string title, string content, string? footer = null)
        {
            var message = $"*{title}*\n\n{content}";

            if (!string.IsNullOrWhiteSpace(footer))
                message += $"\n\n{footer}";

            return message;
        }

        /// <summary>
        /// Checks if a phone number belongs to a specific country
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="countryCode">Country code (e.g., "1" for US, "44" for UK)</param>
        /// <returns>True if number belongs to country, false otherwise</returns>
        public static bool IsPhoneNumberFromCountry(string phoneNumber, string countryCode)
        {
            var normalized = NormalizePhoneNumber(phoneNumber);
            if (normalized == null)
                return false;

            return normalized.StartsWith($"+{countryCode}");
        }

        /// <summary>
        /// Extracts country code from a phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number in E.164 format</param>
        /// <returns>Country code or null if not found</returns>
        public static string? ExtractCountryCode(string phoneNumber)
        {
            var normalized = NormalizePhoneNumber(phoneNumber);
            if (normalized == null || normalized.Length < 2)
                return null;

            // Country codes are 1-3 digits after the +
            // Most common: 1-3 digits, we'll try to match against known patterns
            var withoutPlus = normalized[1..];

            // Check 3-digit codes first (less common but exist)
            if (withoutPlus.Length >= 3 && char.IsDigit(withoutPlus[0]) && 
                char.IsDigit(withoutPlus[1]) && char.IsDigit(withoutPlus[2]))
            {
                return withoutPlus[..3];
            }

            // Check 2-digit codes
            if (withoutPlus.Length >= 2 && char.IsDigit(withoutPlus[0]) && char.IsDigit(withoutPlus[1]))
            {
                return withoutPlus[..2];
            }

            // Check 1-digit code (mostly just +1)
            return char.IsDigit(withoutPlus[0]) ? withoutPlus[0].ToString() : null;
        }

        /// <summary>
        /// Creates a clickable WhatsApp link for opening WhatsApp with a pre-filled message
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="message">Optional pre-filled message</param>
        /// <returns>WhatsApp link</returns>
        public static string CreateWhatsAppLink(string phoneNumber, string? message = null)
        {
            var normalized = NormalizePhoneNumber(phoneNumber);
            if (normalized == null)
                throw new ArgumentException("Invalid phone number format", nameof(phoneNumber));

            var withoutPlus = normalized[1..];
            var baseLink = $"https://wa.me/{withoutPlus}";

            if (string.IsNullOrWhiteSpace(message))
                return baseLink;

            var encodedMessage = Uri.EscapeDataString(message);
            return $"{baseLink}?text={encodedMessage}";
        }

        /// <summary>
        /// Validates WhatsApp template variables
        /// </summary>
        /// <param name="templateName">Template name</param>
        /// <param name="variables">Variables provided</param>
        /// <returns>Validation result with error message if invalid</returns>
        public static (bool isValid, string? errorMessage) ValidateTemplateVariables(
            string templateName, params string[] variables)
        {
            if (!WhatsAppTemplates.TemplateExists(templateName))
            {
                return (false, $"Template '{templateName}' does not exist");
            }

            // Count expected placeholders in template
            // Note: This is a simplified check - actual implementation should parse template
            try
            {
                var template = WhatsAppTemplates.GetTemplate(templateName, variables);
                return (true, null);
            }
            catch (ArgumentException ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Logs WhatsApp message send attempt for debugging
        /// </summary>
        public static void LogMessageAttempt(
            ILogger logger,
            string recipientPhone,
            string messageBody,
            string? templateName = null,
            bool success = false,
            string? errorMessage = null)
        {
            if (success)
            {
                var msgType = templateName != null ? "template" : "text";
                logger.LogInformation(
                    "Successfully sent WhatsApp {MessageType} message to {Phone}. " +
                    "Message length: {Length} chars. {TemplateName}",
                    msgType,
                    recipientPhone,
                    messageBody.Length,
                    templateName != null ? $"Template: {templateName}" : "");
            }
            else
            {
                logger.LogError(
                    "Failed to send WhatsApp message to {Phone}. " +
                    "Error: {Error}. Message: {Message}",
                    recipientPhone,
                    errorMessage ?? "Unknown error",
                    messageBody[..Math.Min(100, messageBody.Length)]);
            }
        }
    }
}
