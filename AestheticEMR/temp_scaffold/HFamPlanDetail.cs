using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HFamPlanDetail
{
    public long Sno { get; set; }

    public DateTime? DtDate { get; set; }

    public DateTime? DtTime { get; set; }

    public string? Pno { get; set; }

    public string? ConsultId { get; set; }

    public string? MtdChange { get; set; }

    public string? MtdSupplied { get; set; }

    public string? Qty { get; set; }

    public string? Bp { get; set; }

    public string? Wt { get; set; }

    public string? Observe { get; set; }

    public DateTime? NextAppt { get; set; }

    public string? EmpId { get; set; }
}
