using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Logs_Migration
{
    public long LogID { get; set; }

    public DateTime? MigrationDate { get; set; }

    public string? SourceDB { get; set; }

    public string? DestDB { get; set; }

    public long? RowsMigrated { get; set; }

    public string? ProcessedBy { get; set; }

    public string? Status { get; set; }
}
