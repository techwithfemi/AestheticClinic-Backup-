using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAudiTrail
{
    public string Fullname { get; set; } = null!;

    public string TranCode { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserAction { get; set; } = null!;

    public DateTime Date { get; set; }

    public DateTime ActionTime { get; set; }

    public string? Remarks { get; set; }

    public string? Src { get; set; }

    public string? Module { get; set; }

    public long Id { get; set; }
}
