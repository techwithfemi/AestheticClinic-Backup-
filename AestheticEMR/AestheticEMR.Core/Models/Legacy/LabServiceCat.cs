using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class LabServiceCat
{
    public string Category { get; set; } = null!;

    public string? LabType { get; set; }

    public string? RptHead { get; set; }
}
