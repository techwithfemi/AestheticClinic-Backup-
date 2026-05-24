using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LabLabelScreen
{
    public long IndexNo { get; set; }

    public string TagName { get; set; } = null!;

    public string LblDesc { get; set; } = null!;

    public string Range { get; set; } = null!;

    public string Units { get; set; } = null!;
}
