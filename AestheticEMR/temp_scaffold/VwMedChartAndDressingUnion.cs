using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwMedChartAndDressingUnion
{
    public string DrgName { get; set; } = null!;

    public int? NumOfTimes { get; set; }

    public int? NumTaken { get; set; }

    public string ConsultId { get; set; } = null!;
}
