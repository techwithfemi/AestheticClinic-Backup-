using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugNhislistNotInDrug
{
    public string DrgName { get; set; } = null!;

    public string? PharmCat { get; set; }

    public string? DrgCatName { get; set; }

    public double Price { get; set; }

    public string? Company { get; set; }

    public string? Remarks { get; set; }

    public long Sno { get; set; }

    public string? CoyName { get; set; }
}
