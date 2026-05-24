using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HImmunization
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime ImDate { get; set; }

    public DateTime ImTime { get; set; }

    public string AgeValue { get; set; } = null!;

    public string Immunization { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? NextApptDate { get; set; }

    public DateTime? NextApptTime { get; set; }

    public string? ImmType { get; set; }
}
