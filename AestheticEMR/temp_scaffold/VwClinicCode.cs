using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicCode
{
    public string ConsultId { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Code { get; set; }

    public string? RctCode { get; set; }

    public string ClinicId { get; set; } = null!;
}
