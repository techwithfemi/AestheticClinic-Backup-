using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HMedChartMonitor
{
    public long Id { get; set; }

    public DateTime MDate { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public short NumOfTimes { get; set; }

    public short NumTaken { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string Dosage { get; set; } = null!;

    public DateTime MTime { get; set; }

    public bool? AttendedTo { get; set; }

    public long? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }
}
