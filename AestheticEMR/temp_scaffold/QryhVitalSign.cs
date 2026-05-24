using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhVitalSign
{
    public string Pno { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public string? Temp { get; set; }

    public string? Pressure { get; set; }

    public string? ExaminedBy { get; set; }

    public string? Pulse { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? RespRatio { get; set; }

    public bool? AttendedTo { get; set; }

    public string Fullname { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? AdmDate { get; set; }

    public string? Spo2 { get; set; }

    public string? Rbs { get; set; }
}
