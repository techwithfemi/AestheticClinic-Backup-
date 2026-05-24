using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ProposalInfo
{
    public long Sno { get; set; }

    public string CoyName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string Designation { get; set; } = null!;
}
