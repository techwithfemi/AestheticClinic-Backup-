using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhMedChartMonitor
{
    public long? ConId { get; set; }

    public DateTime MDate { get; set; }

    public short NumOfTimes { get; set; }

    public short NumTaken { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string Dosage { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime MTime { get; set; }

    public long Id { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }
}
