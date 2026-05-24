using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocClinicAndPatAssignedAll20250620
{
    public string Patient { get; set; } = null!;

    public string Doctor { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? EmpId { get; set; }

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public byte? PatVal { get; set; }

    public DateTime Date { get; set; }

    public bool? AttendedToByDoc { get; set; }

    public string RoomNo { get; set; } = null!;

    public int IsOld { get; set; }
}
