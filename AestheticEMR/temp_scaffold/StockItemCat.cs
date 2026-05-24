using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockItemCat
{
    public string DrgCatCode { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? CatRemarks { get; set; }
}
