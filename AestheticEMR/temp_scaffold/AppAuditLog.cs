using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppAuditLog
{
    public long Id { get; set; }

    public string EventType { get; set; } = null!;

    public string? UserId { get; set; }

    public string Summary { get; set; } = null!;

    public string? Details { get; set; }

    public string Severity { get; set; } = null!;

    public string? EntityType { get; set; }

    public int? EntityId { get; set; }

    public string? FieldName { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? PerformedBy { get; set; }

    public DateTime EventDateTime { get; set; }

    public string? SourceIp { get; set; }

    public string? Tags { get; set; }

    public string Status { get; set; } = null!;

    public string? ReviewedBy { get; set; }

    public DateTime? ReviewedDate { get; set; }

    public string? ResolutionNotes { get; set; }

    public string TranCode { get; set; } = null!;
}
