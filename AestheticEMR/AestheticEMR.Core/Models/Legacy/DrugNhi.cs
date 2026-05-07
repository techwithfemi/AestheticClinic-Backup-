using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class DrugNhi
{
    public string DrgName { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? PharmCat { get; set; }

    public string? DrgCatName { get; set; }

    public double Price { get; set; }

    public string? Remarks { get; set; }

    public long Sno { get; set; }

    public string? CoyName { get; set; }

    public string? Capitated { get; set; }

    public string? TariffStatus { get; set; }

    public string? Drgcode { get; set; }

    public string? RevType { get; set; }

    public string? DrgMaster { get; set; }
}
