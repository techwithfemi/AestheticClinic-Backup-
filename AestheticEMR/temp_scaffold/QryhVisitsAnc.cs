using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhVisitsAnc
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? NextApptDate { get; set; }

    public int RecId { get; set; }

    public string? ClientCat { get; set; }

    public string? EmpId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string? CoyName { get; set; }

    public bool? AttendedTo { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? PolicyType { get; set; }

    public string? CoyType { get; set; }

    public string PCatId { get; set; } = null!;

    public string? Referal { get; set; }
}
