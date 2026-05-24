using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPorderApprv
{
    public string Poid { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public DateTime? OrderDate { get; set; }

    public long Id { get; set; }
}
