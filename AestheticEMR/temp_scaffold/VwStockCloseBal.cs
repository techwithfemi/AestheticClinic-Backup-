using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockCloseBal
{
    public decimal UnitsInStock { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Amount { get; set; }

    public string Period { get; set; } = null!;
}
