using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("EventDateTime", Name = "IX_AppAuditLogs_EventDateTime")]
[Index("Severity", Name = "IX_AppAuditLogs_Severity")]
[Index("Status", Name = "IX_AppAuditLogs_Status")]
[Index("TranCode", Name = "IX_AppAuditLogs_TranCode")]
public partial class AppAuditLog
{
    [Key]
    public long Id { get; set; }

    [StringLength(50)]
    public string EventType { get; set; } = null!;

    [StringLength(50)]
    public string? UserId { get; set; }

    [StringLength(200)]
    public string Summary { get; set; } = null!;

    public string? Details { get; set; }

    [StringLength(20)]
    public string Severity { get; set; } = null!;

    [StringLength(100)]
    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    [StringLength(100)]
    public string? FieldName { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    [StringLength(50)]
    public string? PerformedBy { get; set; }

    public DateTime EventDateTime { get; set; }

    [StringLength(150)]
    public string? SourceIp { get; set; }

    [StringLength(1000)]
    public string? Tags { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string? ReviewedBy { get; set; }

    public DateTime? ReviewedDate { get; set; }

    public string? ResolutionNotes { get; set; }

    [StringLength(50)]
    public string TranCode { get; set; } = null!;
}
