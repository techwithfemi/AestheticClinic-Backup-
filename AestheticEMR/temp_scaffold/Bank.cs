using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Bank
{
    public long Sno { get; set; }

    public string BankCode { get; set; } = null!;

    public string BranchCode { get; set; } = null!;

    public string? BankName { get; set; }

    public string? Branch { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public string AcctId { get; set; } = null!;
}
