using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Icd
{
    public string Code { get; set; } = null!;

    public string DescShort { get; set; } = null!;

    public string DescLong { get; set; } = null!;
}
