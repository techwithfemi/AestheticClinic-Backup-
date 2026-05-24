using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPreConsultChart
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime PreDate { get; set; }

    public DateTime? PreTime { get; set; }

    public double? Temp { get; set; }

    public double? Pressure1 { get; set; }

    public double? Pressure2 { get; set; }

    public double? Pulse { get; set; }

    public double? Weight { get; set; }

    public double? Height { get; set; }

    public double? RespRatio { get; set; }
}
