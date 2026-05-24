using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocClinicAndPatAssigned
{
    public string Patient { get; set; } = null!;

    public string? Doctor { get; set; }

    public string? PhoneNo { get; set; }

    public string EmpId { get; set; } = null!;

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public byte? PatVal { get; set; }

    public DateTime Date { get; set; }

    public bool? AttendedToByDoc { get; set; }
}
