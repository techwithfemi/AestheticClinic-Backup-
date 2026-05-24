using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingCat
{
    public string ClientcatId { get; set; } = null!;

    public string? MapTo { get; set; }

    public decimal? PCent { get; set; }

    public decimal? Pvalue { get; set; }

    public string? ClientType { get; set; }

    public bool? SysVal { get; set; }

    public bool? IsCustom { get; set; }

    public string? Capitated { get; set; }

    public bool? IsReserved { get; set; }
}
