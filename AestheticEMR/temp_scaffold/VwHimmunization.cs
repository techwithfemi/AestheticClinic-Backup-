using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHimmunization
{
    public long Id { get; set; }

    public string Fullname { get; set; } = null!;

    public DateTime ImDate { get; set; }

    public DateTime ImTime { get; set; }

    public string AgeValue { get; set; } = null!;

    public string Immunization { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public string? Remarks { get; set; }

    public string PNo { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? EmpName { get; set; }

    public string? StaffName { get; set; }
}
