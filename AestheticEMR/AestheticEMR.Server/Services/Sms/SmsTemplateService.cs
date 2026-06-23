using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Sms
{
    public class SmsTemplateService(IOptions<AppSettings> appSettings) : ISmsTemplateService
    {
        private readonly SmsMessageTemplateConfig _templates = appSettings.Value.SmsMessageTemplateConfig ?? new SmsMessageTemplateConfig();

        public string BuildAppointmentMessage(string patientName, DateTime? appointmentDate, DateTime? appointmentTime, string? clinicType, string action)
        {
            var model = new Dictionary<string, string>
            {
                ["PatientName"] = patientName,
                ["Action"] = action,
                ["Date"] = appointmentDate?.ToString("dd MMM yyyy") ?? "N/A",
                ["Time"] = appointmentTime?.ToString("hh:mm tt") ?? "N/A",
                ["Clinic"] = string.IsNullOrWhiteSpace(clinicType) ? "clinic" : clinicType.Trim(),
                ["ConsultId"] = "N/A",
                ["DobDayMonth"] = ""
            };

            return RenderTemplate(_templates.AppointmentTemplate, model,
                "Hello {PatientName}, your appointment has been {Action}. Date: {Date}, Time: {Time}, Clinic: {Clinic}. Please contact us if you need any changes.");
        }

        public string BuildAttendanceMessage(string patientName, DateTime attendanceDate, string? clinicType, string? consultId, string action)
        {
            var model = new Dictionary<string, string>
            {
                ["PatientName"] = patientName,
                ["Action"] = action,
                ["Date"] = attendanceDate.ToString("dd MMM yyyy"),
                ["Time"] = "",
                ["Clinic"] = string.IsNullOrWhiteSpace(clinicType) ? "clinic" : clinicType.Trim(),
                ["ConsultId"] = string.IsNullOrWhiteSpace(consultId) ? "N/A" : consultId.Trim(),
                ["DobDayMonth"] = ""
            };

            return RenderTemplate(_templates.AttendanceTemplate, model,
                "Hello {PatientName}, your attendance has been {Action} on {Date} at {Clinic}. Ref: {ConsultId}.");
        }

        public string BuildBirthdayMessage(string patientName, DateTime dateOfBirth)
        {
            var model = new Dictionary<string, string>
            {
                ["PatientName"] = patientName,
                ["Action"] = "",
                ["Date"] = DateTime.Today.ToString("dd MMM yyyy"),
                ["Time"] = "",
                ["Clinic"] = "",
                ["ConsultId"] = "",
                ["DobDayMonth"] = dateOfBirth.ToString("dd MMM")
            };

            return RenderTemplate(_templates.BirthdayTemplate, model,
                "Happy Birthday {PatientName}! We wish you good health, joy and wellness always.");
        }

        private static string RenderTemplate(string? template, IReadOnlyDictionary<string, string> model, string fallback)
        {
            var output = string.IsNullOrWhiteSpace(template) ? fallback : template;

            foreach (var (key, value) in model)
            {
                output = output.Replace($"{{{key}}}", value, StringComparison.OrdinalIgnoreCase);
            }

            return output;
        }
    }
}
