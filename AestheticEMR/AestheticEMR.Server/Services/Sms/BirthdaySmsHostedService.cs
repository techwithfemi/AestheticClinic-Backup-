using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Services;
using AestheticEMR.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Sms
{
    public class BirthdaySmsHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<AppSettings> appSettings,
        ILogger<BirthdaySmsHostedService> logger) : BackgroundService
    {
        private readonly BirthdayNotificationConfig _birthdayConfig = appSettings.Value.BirthdayNotificationConfig ?? new BirthdayNotificationConfig();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_birthdayConfig.EnableSms)
            {
                logger.LogInformation("Birthday SMS hosted service is disabled by configuration.");
                return;
            }

            logger.LogInformation("Birthday SMS hosted service started. Messages will be sent at 00:00 daily.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextMidnight = now.Date.AddDays(1);
                var delay = nextMidnight - now;

                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                try
                {
                    await SendBirthdayMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while sending birthday SMS messages");
                }
            }
        }

        private async Task SendBirthdayMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var smsSender = scope.ServiceProvider.GetRequiredService<ISmsSender>();
            var smsTemplateService = scope.ServiceProvider.GetRequiredService<ISmsTemplateService>();

            var today = DateTime.Today;

            var patients = await db.HPatients
                .Where(p => p.Dob.HasValue
                            && p.Dob.Value.Month == today.Month
                            && p.Dob.Value.Day == today.Day
                            && !string.IsNullOrWhiteSpace(p.Pno)
                            && !string.IsNullOrWhiteSpace(p.PPhoneNo))
                .ToListAsync(cancellationToken);

            foreach (var patient in patients)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var phone = NormalizePhoneNumber(patient.PPhoneNo);
                if (!IsValidPhoneNumber(phone) || !patient.Dob.HasValue)
                {
                    logger.LogInformation("Skipping birthday SMS for patient {PatientNo} due to invalid phone number", patient.Pno);
                    continue;
                }

                var patientName = BuildPatientDisplayName(patient.Title, patient.PFirstname, patient.PSurName);
                var message = smsTemplateService.BuildBirthdayMessage(patientName, patient.Dob.Value);

                var (success, messageId, errorMsg) = await smsSender.SendSmsMessageAsync(phone!, message);

                if (!success)
                {
                    logger.LogWarning("Birthday SMS failed for patient {PatientNo} ({Phone}): {Error}", patient.Pno, phone, errorMsg ?? "Unknown error");
                    continue;
                }

                logger.LogInformation("Birthday SMS sent for patient {PatientNo} ({Phone}), messageId: {MessageId}", patient.Pno, phone, messageId ?? "n/a");
            }
        }

        private static string BuildPatientDisplayName(string? title, string? firstName, string? surname)
        {
            var fullName = string.Join(" ", new[]
            {
                NormalizeText(title),
                NormalizeText(firstName),
                NormalizeText(surname)
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

            return string.IsNullOrWhiteSpace(fullName) ? "Patient" : fullName;
        }

        private static string? NormalizeText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string? NormalizePhoneNumber(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var normalized = value.Trim().Replace(" ", string.Empty).Replace("-", string.Empty);

            if (normalized.StartsWith("00", StringComparison.Ordinal))
            {
                normalized = $"+{normalized[2..]}";
            }

            if (!normalized.StartsWith("+", StringComparison.Ordinal))
            {
                normalized = $"+{normalized.TrimStart('+')}";
            }

            return normalized;
        }

        private static bool IsValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            return phoneNumber.StartsWith("+", StringComparison.Ordinal)
                   && phoneNumber.Length >= 10
                   && phoneNumber[1..].All(char.IsDigit);
        }
    }
}
