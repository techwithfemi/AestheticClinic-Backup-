using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ChartOfAccounts_BeginBalance_Monitor
{
    public string GroupName { get; set; } = null!;

    public bool AttendedTo { get; set; }
}
