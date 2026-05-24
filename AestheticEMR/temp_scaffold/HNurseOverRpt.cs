using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HNurseOverRpt
{
    public long Sno { get; set; }

    public DateTime? DtDate { get; set; }

    public DateTime? DtTime { get; set; }

    public string? Shift { get; set; }

    public string? Details { get; set; }

    public string? RptHead { get; set; }

    public string? SubHead { get; set; }

    public string? EmpId { get; set; }

    public string? Completed { get; set; }

    public bool? IsOld { get; set; }
}
