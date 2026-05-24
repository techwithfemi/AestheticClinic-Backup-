using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRetainershipArchive
{
    public DateTime RetainDate { get; set; }

    public string RetainCode { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public string Address { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public double? ProfFee { get; set; }

    public double? Debt { get; set; }

    public int? BillEndDate { get; set; }
}
