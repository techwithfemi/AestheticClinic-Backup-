using System;

namespace AestheticEMR.Core.Models.Aesthetic
{
    /// <summary>
    /// Comprehensive audit log for tracking all changes, incidents, and safety events in aesthetic procedures.
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }

        /// <summary>
        /// Reference to the aesthetic consultation (procedure record).
        /// </summary>
        public int? ConsultationId { get; set; }

        /// <summary>
        /// Reference to the patient.
        /// </summary>
        public int? PatientId { get; set; }

        /// <summary>
        /// Type of audit event: Create, Update, Delete, Complication, Allergy, Safety, Incident, etc.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Procedure type: Botox, Laser, Dermal Filler, Consultation, etc.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string? ProcedureType { get; set; }

        /// <summary>
        /// High-level summary/title of the event.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(200)]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the event, complication, or change.
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Severity level: Info, Warning, Error, Critical, Safety Incident.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string Severity { get; set; } = "Info";

        /// <summary>
        /// Entity type modified (e.g., AestheticConsultation, AestheticPhoto, etc.).
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string? EntityType { get; set; }

        /// <summary>
        /// Entity ID that was modified.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Field name that was changed (for field-level tracking).
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string? FieldName { get; set; }

        /// <summary>
        /// Old value before change.
        /// </summary>
        public string? OldValue { get; set; }

        /// <summary>
        /// New value after change.
        /// </summary>
        public string? NewValue { get; set; }

        /// <summary>
        /// User who triggered the event.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(40)]
        public string? PerformedBy { get; set; }

        /// <summary>
        /// Timestamp when event occurred (UTC).
        /// </summary>
        public DateTime EventDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP address or session identifier for security tracking.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(45)]
        public string? SourceIp { get; set; }

        /// <summary>
        /// Tags for categorization: #allergy, #complication, #vascular, #ptosis, #infection, #incident, etc.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// Status/resolution: Open, Reviewed, Resolved, Escalated.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string Status { get; set; } = "Open";

        /// <summary>
        /// Who reviewed/addressed this incident.
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(40)]
        public string? ReviewedBy { get; set; }

        /// <summary>
        /// Timestamp of review.
        /// </summary>
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Resolution notes or corrective actions taken.
        /// </summary>
        public string? ResolutionNotes { get; set; }
    }
}
