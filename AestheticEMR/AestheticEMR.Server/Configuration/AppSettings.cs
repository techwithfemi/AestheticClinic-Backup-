// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace AestheticEMR.Server.Configuration
{
    public class AppSettings
    {
        public SmtpConfig? SmtpConfig { get; set; }
        public WhatsAppConfig? WhatsAppConfig { get; set; }
        public SmsConfig? SmsConfig { get; set; }
        public SmsMessageTemplateConfig? SmsMessageTemplateConfig { get; set; }
        public BirthdayNotificationConfig? BirthdayNotificationConfig { get; set; }
        public AppointmentNotificationConfig? AppointmentNotificationConfig { get; set; }
        public AttendanceNotificationConfig? AttendanceNotificationConfig { get; set; }
        public LogRetentionConfig? LogRetentionConfig { get; set; }
        public AuditLoggingConfig? AuditLoggingConfig { get; set; }
        public LegacyReportServiceConfig? LegacyReportService { get; set; }
        public DialogHeaderThemeConfig DialogHeaderThemeConfig { get; set; } = new();
        public string? ClientBaseUrl { get; set; }
    }

    public class DialogHeaderThemeConfig
    {
        public string GradientStart { get; set; } = "#0b1f5e";
        public string GradientMid { get; set; } = "#12357f";
        public string GradientEnd { get; set; } = "#1d4ed8";
        public string AccentStart { get; set; } = "#14b8a6";
        public string AccentMid { get; set; } = "#f59e0b";
        public string AccentEnd { get; set; } = "#2dd4bf";
        public string TitleColor { get; set; } = "#f8fafc";
        public string CloseBackground { get; set; } = "rgba(11, 31, 94, 0.45)";
        public string CloseBorder { get; set; } = "rgba(255, 255, 255, 0.22)";
        public string CloseHoverBackground { get; set; } = "rgba(29, 78, 216, 0.55)";
        public string CloseHoverBorder { get; set; } = "rgba(255, 255, 255, 0.38)";
    }

    public class AuditLoggingConfig
    {
        public bool EnableSecondaryAppAuditLogs { get; set; } = false;
    }

    public class LegacyReportServiceConfig
    {
        public string? BaseUrl { get; set; }
        public string AccountingRoutePrefix { get; set; } = "api/Reports";
        public int TimeoutSeconds { get; set; } = 120;
        public string? ApiKey { get; set; }
    }

    public class SmtpConfig
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public required string EmailAddress { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class WhatsAppConfig
    {
        /// <summary>
        /// Twilio Account SID from https://www.twilio.com/console
        /// </summary>
        public required string AccountSid { get; set; }

        /// <summary>
        /// Twilio Auth Token from https://www.twilio.com/console
        /// </summary>
        public required string AuthToken { get; set; }

        /// <summary>
        /// Twilio WhatsApp-enabled phone number (format: +1234567890 or whatsapp:+1234567890)
        /// </summary>
        public required string FromPhoneNumber { get; set; }

        /// <summary>
        /// Enable WhatsApp messaging feature
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Maximum retries for failed messages
        /// </summary>
        public int MaxRetries { get; set; } = 3;
    }

    public class SmsConfig
    {
        /// <summary>
        /// Twilio Account SID from https://www.twilio.com/console
        /// </summary>
        public required string AccountSid { get; set; }

        /// <summary>
        /// Twilio Auth Token from https://www.twilio.com/console
        /// </summary>
        public required string AuthToken { get; set; }

        /// <summary>
        /// Twilio SMS-enabled phone number (format: +1234567890)
        /// </summary>
        public required string FromPhoneNumber { get; set; }

        /// <summary>
        /// Enable SMS messaging feature
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Maximum retries for failed messages
        /// </summary>
        public int MaxRetries { get; set; } = 3;
    }

    public class AppointmentNotificationConfig
    {
        /// <summary>
        /// Enable automatic appointment SMS on create/update operations
        /// </summary>
        public bool EnableSms { get; set; } = true;
    }

    public class AttendanceNotificationConfig
    {
        /// <summary>
        /// Enable automatic attendance SMS on create/update operations
        /// </summary>
        public bool EnableSms { get; set; } = true;
    }

    public class BirthdayNotificationConfig
    {
        /// <summary>
        /// Enable automatic birthday SMS notifications
        /// </summary>
        public bool EnableSms { get; set; } = true;
    }

    public class SmsMessageTemplateConfig
    {
        /// <summary>
        /// Template for appointment SMS notifications.
        /// Supported placeholders: {PatientName}, {Action}, {Date}, {Time}, {Clinic}
        /// </summary>
        public string? AppointmentTemplate { get; set; }

        /// <summary>
        /// Template for attendance SMS notifications.
        /// Supported placeholders: {PatientName}, {Action}, {Date}, {Clinic}, {ConsultId}
        /// </summary>
        public string? AttendanceTemplate { get; set; }

        /// <summary>
        /// Template for birthday SMS notifications.
        /// Supported placeholders: {PatientName}, {DobDayMonth}
        /// </summary>
        public string? BirthdayTemplate { get; set; }
    }

    public class LogRetentionConfig
    {
        public bool Enabled { get; set; } = true;
        public int RetentionDays { get; set; } = 7;
        public int CleanupIntervalHours { get; set; } = 24;
    }
}
