using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicType
{
    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string? Type { get; set; }

    public double? RegFee { get; set; }

    public double? ConFee { get; set; }

    public string? Code { get; set; }

    public string? ReqVitals { get; set; }

    public string? Designation { get; set; }

    public string? EmpId { get; set; }

    public string? Consultant { get; set; }

    public long Sno { get; set; }

    public string? RctCode { get; set; }

    public string? PhoneNo { get; set; }

    public string? ClinicPeriod { get; set; }

    public string? Remarks { get; set; }

    public string? Apologies { get; set; }
}
