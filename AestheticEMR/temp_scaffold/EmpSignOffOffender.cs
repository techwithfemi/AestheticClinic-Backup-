using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpSignOffOffender
{
    public long RecId { get; set; }

    public string StaffNo { get; set; } = null!;

    public DateTime SignInDate { get; set; }

    public DateTime SignInTime { get; set; }
}
