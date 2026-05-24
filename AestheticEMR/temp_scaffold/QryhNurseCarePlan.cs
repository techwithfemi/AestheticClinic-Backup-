using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNurseCarePlan
{
    public int Id { get; set; }

    public DateTime? DtTime { get; set; }

    public string? Pno { get; set; }

    public string? ConsultId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? NurDiag { get; set; }

    public string? Objective { get; set; }

    public string? NurOrders { get; set; }

    public string? NurEval { get; set; }

    public string? EmpId { get; set; }

    public string EmpName { get; set; } = null!;
}
