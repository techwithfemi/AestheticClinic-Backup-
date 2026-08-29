using AestheticEMR.Server.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/ui-settings")]
    [ApiController]
    public class UiSettingsController : BaseApiController
    {
        private readonly AppSettings _appSettings;

        public UiSettingsController(
            ILogger<UiSettingsController> logger,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
            : base(logger, mapper)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet("dialog-header-theme")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DialogHeaderThemeResponse), StatusCodes.Status200OK)]
        public ActionResult<DialogHeaderThemeResponse> GetDialogHeaderTheme()
        {
            var theme = _appSettings.DialogHeaderThemeConfig ?? new DialogHeaderThemeConfig();

            return Ok(new DialogHeaderThemeResponse
            {
                GradientStart = theme.GradientStart,
                GradientMid = theme.GradientMid,
                GradientEnd = theme.GradientEnd,
                AccentStart = theme.AccentStart,
                AccentMid = theme.AccentMid,
                AccentEnd = theme.AccentEnd,
                TitleColor = theme.TitleColor,
                CloseBackground = theme.CloseBackground,
                CloseBorder = theme.CloseBorder,
                CloseHoverBackground = theme.CloseHoverBackground,
                CloseHoverBorder = theme.CloseHoverBorder
            });
        }
    }

    public sealed class DialogHeaderThemeResponse
    {
        public string GradientStart { get; set; } = "#0b1f5e";
        public string GradientMid { get; set; } = "#12357f";
        public string GradientEnd { get; set; } = "#1d4ed8";
        public string AccentStart { get; set; } = "#14b8a6";
        public string AccentMid { get; set; } = "#f59e0b";
        public string AccentEnd { get; set; } = "#2dd4bf";
        public string TitleColor { get; set; } = "#f8fafc";
        public string CloseBackground { get; set; } = "rgba(30, 78, 216, 0.35)";
        public string CloseBorder { get; set; } = "rgba(191, 219, 254, 0.5)";
        public string CloseHoverBackground { get; set; } = "rgba(30, 78, 216, 0.55)";
        public string CloseHoverBorder { get; set; } = "rgba(219, 234, 254, 0.75)";
    }
}
