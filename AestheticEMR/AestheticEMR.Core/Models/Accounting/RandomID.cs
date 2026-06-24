using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class RandomID
{
    public int RowNumber { get; set; }

    public int NextID { get; set; }
}
