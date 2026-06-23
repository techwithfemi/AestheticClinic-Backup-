// ---------------------------------------
// SMS Debug Controller - For Testing SMS Functionality
// Remove or restrict this controller in production!
// ---------------------------------------

using AestheticEMR.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers
{
    /// <summary>
    /// Debug controller for testing SMS functionality
    /// WARNING: This controller should be removed or secured in production!
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class SmsDebugController : BaseApiController
    {
        private readonly ISmsSender _smsSender;

        public SmsDebugController(
            ISmsSender smsSender,
            ILogger<BaseApiController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _smsSender = smsSender;
        }

        /// <summary>
        /// Sends a test SMS message
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
                var (success, messageId, errorMsg) = await _smsSender.SendSmsMessageAsync(phoneNumber, message);

                if (success)
                {
                    return Ok(new
                    {
                        success = true,
                        messageId,
                        message = $"SMS sent successfully to {phoneNumber}",
                        sentAt = DateTime.UtcNow
                    });
                }

                AddModelError("sms", errorMsg ?? "Failed to send SMS");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                AddModelError("error", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
