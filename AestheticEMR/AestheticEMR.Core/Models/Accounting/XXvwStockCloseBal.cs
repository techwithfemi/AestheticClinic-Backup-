using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class XXvwStockCloseBal
{
    public decimal UnitsInStock { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Amount { get; set; }

    public string Period { get; set; } = null!;
}
