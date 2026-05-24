using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwWidalX
{
    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public int? Age { get; set; }

    public string Sex { get; set; } = null!;

    public string? DocName { get; set; }

    public string? HospName { get; set; }

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string ResultMaster { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Result1 { get; set; }

    public string? Desc2 { get; set; }

    public string? Sample { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public string? Result12 { get; set; }
}
