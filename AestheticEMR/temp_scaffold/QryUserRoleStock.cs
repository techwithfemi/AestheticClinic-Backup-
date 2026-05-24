using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUserRoleStock
{
    public string UserName { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? AccountStatus { get; set; }

    public string LocId { get; set; } = null!;

    public string LocName { get; set; } = null!;
}
