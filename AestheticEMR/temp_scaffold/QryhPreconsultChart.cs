using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPreconsultChart
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public double? Temp { get; set; }

    public double? Pressure1 { get; set; }

    public double? Pressure2 { get; set; }

    public double? Pulse { get; set; }

    public double? RespRatio { get; set; }

    public string Fullname { get; set; } = null!;

    public double? Weight { get; set; }

    public double? Height { get; set; }

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public string PNo { get; set; } = null!;
}
