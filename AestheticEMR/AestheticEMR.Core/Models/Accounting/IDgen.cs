using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class IDgen
{
    public string DestName { get; set; } = null!;

    public decimal ID { get; set; }
}
