using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugsPriceHistory
{
    public long Sno { get; set; }

    public decimal SellingPrice { get; set; }

    public string DrgName { get; set; } = null!;

    public string LocId { get; set; } = null!;

    public DateTime? EntryDate { get; set; }
}
