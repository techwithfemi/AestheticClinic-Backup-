using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRecordsArchiveOld
{
    public int RecId { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string? EmpId { get; set; }

    public string ClinicType { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public DateTime? Htime { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Referal { get; set; }
}
