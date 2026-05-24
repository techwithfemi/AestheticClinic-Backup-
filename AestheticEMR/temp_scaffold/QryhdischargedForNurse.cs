using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhdischargedForNurse
{
    public string Fullname { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string WardId { get; set; } = null!;

    public string SummDischbyNurse { get; set; } = null!;

    public string? ApprvBy { get; set; }

    public string? Remarks { get; set; }

    public string? ApprovedBy { get; set; }

    public string? CoyName { get; set; }

    public string ConsultId { get; set; } = null!;

    public long Id { get; set; }
}
