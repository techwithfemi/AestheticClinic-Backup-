using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocWaitingList
{
    public DateTime Date { get; set; }

    public string? EmpId { get; set; }

    public string? DocAssigned { get; set; }

    public string? Doctor { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Patient { get; set; } = null!;

    public byte? PatVal { get; set; }

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string RoomNo { get; set; } = null!;

    public int IsOff { get; set; }
}
