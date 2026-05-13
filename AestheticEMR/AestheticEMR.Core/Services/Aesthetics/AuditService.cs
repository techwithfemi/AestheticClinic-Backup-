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
        Task LogComplicationAsync(int? consultationId, int? patientId, string procedureType, 
            string complicationTitle, string details, string severity = "Warning");

        /// <summary>
        /// Log an allergy-related event.
        /// </summary>
        Task LogAllergyEventAsync(int? patientId, string allergy, string details, string severity = "Error");

        /// <summary>
        /// Log a safety incident.
        /// </summary>
        Task LogSafetyIncidentAsync(int? consultationId, int? patientId, string title, string details, 
            string severity = "Critical", string tags = "");

        /// <summary>
        /// Log a field change for audit trail tracking.
        /// </summary>
        Task LogFieldChangeAsync(int? consultationId, int? patientId, string entityType, int entityId,
            string fieldName, string? oldValue, string? newValue, string? procedureType = null);

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
        Task MarkAsReviewedAsync(int auditLogId, string reviewedBy, string resolutionNotes);

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

            auditLog.EventDateTime = DateTime.UtcNow;
            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogComplicationAsync(int? consultationId, int? patientId, string procedureType,
            string complicationTitle, string details, string severity = "Warning")
        {
            var auditLog = new AuditLog
            {
                ConsultationId = consultationId,
                PatientId = patientId,
                EventType = "Complication",
                ProcedureType = procedureType,
                Summary = complicationTitle,
                Details = details,
                Severity = severity,
                Tags = "#complication",
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogAllergyEventAsync(int? patientId, string allergy, string details, string severity = "Error")
        {
            var auditLog = new AuditLog
            {
                PatientId = patientId,
                EventType = "Allergy",
                Summary = $"Allergy Detected: {allergy}",
                Details = details,
                Severity = severity,
                Tags = "#allergy #safety",
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogSafetyIncidentAsync(int? consultationId, int? patientId, string title, string details,
            string severity = "Critical", string tags = "")
        {
            var auditLog = new AuditLog
            {
                ConsultationId = consultationId,
                PatientId = patientId,
                EventType = "Safety Incident",
                Summary = title,
                Details = details,
                Severity = severity,
                Tags = string.IsNullOrEmpty(tags) ? "#incident #safety" : tags,
                EventDateTime = DateTime.UtcNow,
                Status = "Open"
            };

            await LogEventAsync(auditLog);
        }

        public async Task LogFieldChangeAsync(int? consultationId, int? patientId, string entityType, int entityId,
            string fieldName, string? oldValue, string? newValue, string? procedureType = null)
        {
            var auditLog = new AuditLog
            {
                ConsultationId = consultationId,
                PatientId = patientId,
                EventType = "Update",
                EntityType = entityType,
                EntityId = entityId,
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                ProcedureType = procedureType,
                Summary = $"{entityType}.{fieldName} updated",
                Severity = "Info",
                EventDateTime = DateTime.UtcNow,
                Status = "Logged"
            };

            await LogEventAsync(auditLog);
        }

        public async Task<List<AuditLog>> GetConsultationAuditTrailAsync(int consultationId)
        {
            return await _dbContext.AuditLogs
                .Where(a => a.ConsultationId == consultationId)
                .OrderByDescending(a => a.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetPatientAuditTrailAsync(int patientId)
        {
            return await _dbContext.AuditLogs
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.EventDateTime)
                .ToListAsync();
        }

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

        public async Task MarkAsReviewedAsync(int auditLogId, string reviewedBy, string resolutionNotes)
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
