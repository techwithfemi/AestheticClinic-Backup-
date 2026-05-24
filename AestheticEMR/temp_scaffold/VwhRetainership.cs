using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRetainership
{
    public string RetainCode { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string? Category { get; set; }

    public string? ClientType { get; set; }

    public string? Address { get; set; }

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public double? ProfFee { get; set; }

    public double? Debt { get; set; }

    public string? UseTariff { get; set; }

    public string? Active { get; set; }

    public int? BillEndDate { get; set; }

    public string? AccountNo { get; set; }

    public decimal? ConAmount { get; set; }

    public decimal? RegAmount { get; set; }

    public decimal? CardRenewAmount { get; set; }
}
