using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Doctor
{
    public long Sno { get; set; }

    public string GroupCode { get; set; } = null!;

    public string DocCode { get; set; } = null!;

    public string DocName { get; set; } = null!;

    public string? Branch { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public string AcctId { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? AcctNo { get; set; }
}
