using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwMedChartAndDressingUnionGrouped
{
    public string DrgName { get; set; } = null!;

    public int? NumTaken { get; set; }

    public string Of1 { get; set; } = null!;

    public int? NumOfTimes { get; set; }

    public string Taken { get; set; } = null!;

    public string ConsultId { get; set; } = null!;
}
