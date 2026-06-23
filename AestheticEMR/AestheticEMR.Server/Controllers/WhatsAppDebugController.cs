// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// WhatsApp Debug Controller - For Testing WhatsApp Functionality
// Remove or restrict this controller in production!
// ---------------------------------------

using AestheticEMR.Core.Services;
using AestheticEMR.Server.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Services.WhatsApp
{
    /// <summary>
    /// Debug controller for testing WhatsApp functionality
    /// WARNING: This controller should be removed or secured in production!
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class WhatsAppDebugController : BaseApiController
    {
        private readonly IWhatsAppSender _whatsAppSender;

        public WhatsAppDebugController(
            IWhatsAppSender whatsAppSender,
            ILogger<BaseApiController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _whatsAppSender = whatsAppSender;
        }

        /// <summary>
        /// Sends a test WhatsApp message
        /// </summary>
        /// <param name="phoneNumber">Recipient phone number in E.164 format (e.g., +1234567890)</param>
        /// <param name="message">Message body</param>
        /// <returns>Result of sending message</returns>
        [HttpPost("send-message")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMessage(
            [FromQuery] string phoneNumber,
            [FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                AddModelError("phoneNumber", "Phone number is required");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                AddModelError("message", "Message is required");
                return BadRequest(ModelState);
            }

            try
            {
                var (success, messageId, errorMsg) = await _whatsAppSender.SendWhatsAppMessageAsync(
                    phoneNumber,
                    message);

                if (success)
                {
                    return Ok(new
                    {
                        success = true,
                        messageId = messageId,
                        message = $"WhatsApp message sent successfully to {phoneNumber}",
                        sentAt = DateTime.UtcNow
                    });
                }

                AddModelError("whatsapp", errorMsg ?? "Failed to send WhatsApp message");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending WhatsApp message");
                AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Sends a WhatsApp template message
        /// </summary>
        /// <param name="phoneNumber">Recipient phone number</param>
        /// <param name="templateName">Name of the template to use</param>
        /// <param name="variables">Template variables (comma-separated)</param>
        /// <returns>Result of sending message</returns>
        [HttpPost("send-template")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendTemplateMessage(
            [FromQuery] string phoneNumber,
            [FromQuery] string templateName,
            [FromQuery] string? variables = null)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                AddModelError("phoneNumber", "Phone number is required");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(templateName))
            {
                AddModelError("templateName", "Template name is required");
                return BadRequest(ModelState);
            }

            try
            {
                var templateVars = string.IsNullOrWhiteSpace(variables)
                    ? Array.Empty<string>()
                    : variables.Split(',').Select(v => v.Trim()).ToArray();

                var (success, messageId, errorMsg) = await _whatsAppSender.SendWhatsAppTemplateMessageAsync(
                    phoneNumber,
                    templateName,
                    templateVars);

                if (success)
                {
                    return Ok(new
                    {
                        success = true,
                        messageId = messageId,
                        templateName = templateName,
                        variablesCount = templateVars.Length,
                        message = $"WhatsApp template message sent successfully to {phoneNumber}",
                        sentAt = DateTime.UtcNow
                    });
                }

                AddModelError("whatsapp", errorMsg ?? "Failed to send WhatsApp template message");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending WhatsApp template message");
                AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Gets list of available templates
        /// </summary>
        /// <returns>List of template names</returns>
        [HttpGet("templates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAvailableTemplates()
        {
            var templates = WhatsAppTemplates.GetAvailableTemplates().ToList();
            return Ok(new
            {
                count = templates.Count,
                templates = templates
            });
        }

        /// <summary>
        /// Validates a phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number to validate</param>
        /// <returns>Validation result</returns>
        [HttpGet("validate-phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ValidatePhoneNumber([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return BadRequest(new { error = "Phone number is required" });
            }

            var isValid = WhatsAppUtilities.IsValidWhatsAppPhoneNumber(phoneNumber);
            var normalized = WhatsAppUtilities.NormalizePhoneNumber(phoneNumber);
            var countryCode = WhatsAppUtilities.ExtractCountryCode(phoneNumber);

            return Ok(new
            {
                phoneNumber = phoneNumber,
                isValid = isValid,
                normalizedNumber = normalized,
                countryCode = countryCode
            });
        }

        /// <summary>
        /// Generates a WhatsApp link for opening WhatsApp with pre-filled message
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="message">Pre-filled message</param>
        /// <returns>WhatsApp link</returns>
        [HttpGet("whatsapp-link")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateWhatsAppLink(
            [FromQuery] string phoneNumber,
            [FromQuery] string? message = null)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return BadRequest(new { error = "Phone number is required" });
            }

            try
            {
                var link = WhatsAppUtilities.CreateWhatsAppLink(phoneNumber, message);
                return Ok(new
                {
                    phoneNumber = phoneNumber,
                    link = link,
                    message = message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Sends a WhatsApp message with media
        /// </summary>
        /// <param name="phoneNumber">Recipient phone number</param>
        /// <param name="messageBody">Message text</param>
        /// <param name="mediaUrl">URL of the media</param>
        /// <param name="mediaType">Type of media (image, document, audio, video)</param>
        /// <returns>Result of sending message</returns>
        [HttpPost("send-with-media")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMessageWithMedia(
            [FromQuery] string phoneNumber,
            [FromQuery] string messageBody,
            [FromQuery] string mediaUrl,
            [FromQuery] string mediaType = "image")
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                AddModelError("phoneNumber", "Phone number is required");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(mediaUrl))
            {
                AddModelError("mediaUrl", "Media URL is required");
                return BadRequest(ModelState);
            }

            try
            {
                var (success, messageId, errorMsg) = await _whatsAppSender.SendWhatsAppMessageWithMediaAsync(
                    phoneNumber,
                    messageBody,
                    mediaUrl,
                    mediaType);

                if (success)
                {
                    return Ok(new
                    {
                        success = true,
                        messageId = messageId,
                        message = $"WhatsApp message with media sent successfully to {phoneNumber}",
                        mediaType = mediaType,
                        sentAt = DateTime.UtcNow
                    });
                }

                AddModelError("whatsapp", errorMsg ?? "Failed to send WhatsApp message with media");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending WhatsApp message with media");
                AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
