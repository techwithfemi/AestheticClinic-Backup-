using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class WatchDogLog
{
    public int Id { get; set; }

    public string? EventId { get; set; }

    public string? Message { get; set; }

    public string Timestamp { get; set; } = null!;

    public string? CallingFrom { get; set; }

    public string? CallingMethod { get; set; }

    public int? LineNumber { get; set; }

    public string? LogLevel { get; set; }
}
