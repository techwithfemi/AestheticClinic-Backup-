using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicCodePublic
{
    public string ConsultId { get; set; } = null!;

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string ClinicType { get; set; } = null!;
}
