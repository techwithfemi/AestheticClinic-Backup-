using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Aesthetic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Services.Aesthetics
{
    /// <summary>
    /// Service for managing audit logs and tracking changes in aesthetic procedures.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Log a general audit event.
        /// </summary>
        Task LogEventAsync(AuditLog auditLog);

        /// <summary>
        /// Log a complication incident.
        /// </summary>
        Task LogComplicationAsync(string tranCode, string complicationTitle, string details, string severity = "Warning", string? performedBy = null, string? sourceIp = null);

        /// <summary>
        /// Log an allergy-related event.
        /// </summary>
        Task LogAllergyEventAsync(string tranCode, string allergy, string details, string severity = "Error", string? performedBy = null, string? sourceIp = null);

        /// <summary>
        /// Log a safety incident.
        /// </summary>
        Task LogSafetyIncidentAsync(string tranCode, string title, string details,
            string severity = "Critical", string tags = "", string? performedBy = null, string? sourceIp = null);

        /// <summary>
        /// Log a field change for audit trail tracking.
        /// </summary>
        Task LogFieldChangeAsync(string tranCode, string entityType, int entityId,
            string fieldName, string? oldValue, string? newValue, string? performedBy = null, string? sourceIp = null);

        /// <summary>
        /// Get all audit entries for a transaction code.
        /// </summary>
        Task<List<AuditLog>> GetAuditTrailByTranCodeAsync(string tranCode);

        /// <summary>
        /// Get all audit entries for a consultation.
        /// </summary>
        Task<List<AuditLog>> GetConsultationAuditTrailAsync(int consultationId);

        /// <summary>
        /// Get all audit entries for a patient.
        /// </summary>
        Task<List<AuditLog>> GetPatientAuditTrailAsync(int patientId);

        /// <summary>
        /// Get all open incidents (status = Open).
        /// </summary>
        Task<List<AuditLog>> GetOpenIncidentsAsync();

        /// <summary>
        /// Get incidents filtered by severity and date range.
        /// </summary>
        Task<List<AuditLog>> GetIncidentsAsync(string severity, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Mark an incident as reviewed and add resolution notes.
        /// </summary>
        Task MarkAsReviewedAsync(long auditLogId, string reviewedBy, string resolutionNotes);

        /// <summary>
        /// Delete old audit logs (retention policy).
        /// </summary>
        Task PurgeOldEntriesAsync(int retentionDays = 365);
    }

    public class AuditService(ApplicationDbContext dbContext) : IAuditService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task LogEventAsync(AuditLog auditLog)
        {
            if (auditLog == null) return;

            if (string.IsNullOrWhiteSpace(auditLog.TranCode))
            {
                auditLog.TranCode = "GENERAL";
            }

            auditLog.EventDateTime = DateTime.UtcNow;
            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogComplicationAsync(string tranCode, string complicationTitle, string details, string severity = "Warning", string? performedBy = null, string? sourceIp = null)
        {
            var auditLog = new AuditLog
            {
                TranCode = string.IsNullOrWhiteSpace(tranCode) ? "GENERAL" : tranCode,
                EventType = "Complication",
                Summary = complicationTitle,
                Details = details,
                Severity = severity,
                Tags = "#complication",
                UserId = performedBy,
                PerformedBy = performedBy,
                SourceIp = sourceIp,
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogAllergyEventAsync(string tranCode, string allergy, string details, string severity = "Error", string? performedBy = null, string? sourceIp = null)
        {
            var auditLog = new AuditLog
            {
                TranCode = string.IsNullOrWhiteSpace(tranCode) ? "GENERAL" : tranCode,
                EventType = "Allergy",
                Summary = $"Allergy Detected: {allergy}",
                Details = details,
                Severity = severity,
                Tags = "#allergy #safety",
                UserId = performedBy,
                PerformedBy = performedBy,
                SourceIp = sourceIp,
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogSafetyIncidentAsync(string tranCode, string title, string details,
            string severity = "Critical", string tags = "", string? performedBy = null, string? sourceIp = null)
        {
            var auditLog = new AuditLog
            {
                TranCode = string.IsNullOrWhiteSpace(tranCode) ? "GENERAL" : tranCode,
                EventType = "Safety Incident",
                Summary = title,
                Details = details,
                Severity = severity,
                Tags = string.IsNullOrEmpty(tags) ? "#incident #safety" : tags,
                UserId = performedBy,
                PerformedBy = performedBy,
                SourceIp = sourceIp,
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogFieldChangeAsync(string tranCode, string entityType, int entityId,
            string fieldName, string? oldValue, string? newValue, string? performedBy = null, string? sourceIp = null)
        {
            var oldDisplay = string.IsNullOrWhiteSpace(oldValue) ? "(empty)" : oldValue;
            var newDisplay = string.IsNullOrWhiteSpace(newValue) ? "(empty)" : newValue;

            var auditLog = new AuditLog
            {
                TranCode = string.IsNullOrWhiteSpace(tranCode) ? "GENERAL" : tranCode,
                EventType = "Update",
                EntityType = entityType,
                EntityId = entityId,
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Summary = $"{entityType}#{entityId}: {fieldName} changed",
                Details = $"Changed {fieldName} from '{oldDisplay}' to '{newDisplay}'",
                Severity = "Info",
                UserId = performedBy,
                PerformedBy = performedBy,
                SourceIp = sourceIp,
                EventDateTime = DateTime.UtcNow,
                Status = "Logged"
            };

            await LogEventAsync(auditLog);
        }

        public async Task<List<AuditLog>> GetAuditTrailByTranCodeAsync(string tranCode)
        {
            return await _dbContext.AuditLogs
                .Where(a => a.TranCode == tranCode)
                .OrderByDescending(a => a.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetConsultationAuditTrailAsync(int consultationId)
            => await GetAuditTrailByTranCodeAsync(consultationId.ToString());

        public async Task<List<AuditLog>> GetPatientAuditTrailAsync(int patientId)
            => await GetAuditTrailByTranCodeAsync(patientId.ToString());

        public async Task<List<AuditLog>> GetOpenIncidentsAsync()
        {
            return await _dbContext.AuditLogs
                .Where(a => a.Status == "Open" && (a.Severity == "Error" || a.Severity == "Critical" || a.EventType == "Safety Incident"))
                .OrderByDescending(a => a.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetIncidentsAsync(string severity, DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.AuditLogs
                .Where(a => a.Severity == severity && a.EventDateTime >= fromDate && a.EventDateTime <= toDate)
                .OrderByDescending(a => a.EventDateTime)
                .ToListAsync();
        }

        public async Task MarkAsReviewedAsync(long auditLogId, string reviewedBy, string resolutionNotes)
        {
            var auditLog = await _dbContext.AuditLogs.FindAsync(auditLogId);
            if (auditLog != null)
            {
                auditLog.Status = "Reviewed";
                auditLog.ReviewedBy = reviewedBy;
                auditLog.ReviewedDate = DateTime.UtcNow;
                auditLog.ResolutionNotes = resolutionNotes;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task PurgeOldEntriesAsync(int retentionDays = 365)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
            var oldEntries = await _dbContext.AuditLogs
                .Where(a => a.EventDateTime < cutoffDate && a.Status != "Open")
                .ToListAsync();

            if (oldEntries.Any())
            {
                _dbContext.AuditLogs.RemoveRange(oldEntries);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
