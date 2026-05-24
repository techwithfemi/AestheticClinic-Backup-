using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDrug
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? QtyPerUnit { get; set; }

    public double Cost { get; set; }

    public double? Price { get; set; }

    public string? CatRemarks { get; set; }

    public double? Nhiscost { get; set; }

    public string? DeptBillCenter { get; set; }
}
