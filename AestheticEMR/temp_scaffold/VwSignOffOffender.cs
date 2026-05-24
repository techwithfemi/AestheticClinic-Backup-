using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwSignOffOffender
{
    public long RecId { get; set; }

    public string StaffNo { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime SignInDate { get; set; }

    public DateTime SignInTime { get; set; }

    public string Shift { get; set; } = null!;
}
