using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUserRoleStockDept
{
    public string UserName { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? AccountStatus { get; set; }

    public string? DeptName { get; set; }

    public long SnoId { get; set; }
}
