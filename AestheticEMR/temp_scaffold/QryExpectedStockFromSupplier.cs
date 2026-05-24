using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryExpectedStockFromSupplier
{
    public string LpoNo { get; set; } = null!;

    public string? SupplierNo { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;
}
