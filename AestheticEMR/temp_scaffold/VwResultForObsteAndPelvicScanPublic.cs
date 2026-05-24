using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwResultForObsteAndPelvicScanPublic
{
    public long Id { get; set; }

    public string HospName { get; set; } = null!;

    public string ResultMaster { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? EmpName { get; set; }

    public string Description { get; set; } = null!;

    public string Result { get; set; } = null!;

    public string? Desc2 { get; set; }

    public string? Class { get; set; }

    public DateTime Invdate { get; set; }

    public string Labno { get; set; } = null!;

    public string Fullname { get; set; } = null!;
}
