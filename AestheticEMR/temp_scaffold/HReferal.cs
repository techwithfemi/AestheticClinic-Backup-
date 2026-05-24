using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HReferal
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ReferTo { get; set; }

    public string? PNo { get; set; }

    public string? RefReason { get; set; }

    public DateTime? RefDate { get; set; }

    public DateTime? RefTime { get; set; }

    public bool? AttendedTo { get; set; }

    public string? RefAddress { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? Comments { get; set; }

    public bool? AttendedToByRec { get; set; }

    public string? EmpId { get; set; }

    public string? Remarks { get; set; }
}
