using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvResultForRptPublic3
{
    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public string DocName { get; set; } = null!;

    public string HospName { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string ResultMaster { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string? EmpName { get; set; }

    public string? Description { get; set; }

    public string? Result { get; set; }

    public string? Desc2 { get; set; }

    public string? Sample { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public long Id { get; set; }

    public int? SerialNo { get; set; }
}
