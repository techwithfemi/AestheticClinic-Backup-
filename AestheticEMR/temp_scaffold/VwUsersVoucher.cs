using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwUsersVoucher
{
    public string? Fullname { get; set; }

    public string UserName { get; set; } = null!;

    public string? AccountStatus { get; set; }

    public string? LoginRole { get; set; }

    public string? Privilege { get; set; }

    public string? SetId { get; set; }

    public string EmpId { get; set; } = null!;
}
