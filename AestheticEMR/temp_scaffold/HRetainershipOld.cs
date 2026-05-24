using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRetainershipOld
{
    public DateTime? RetainDate { get; set; }

    public string? RetainId { get; set; }

    public string? RetainCode { get; set; }

    public string? RetainName { get; set; }

    public string? ClientCatId { get; set; }

    public string? ClientType { get; set; }

    public string? Address { get; set; }

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public double? ProfFee { get; set; }

    public double? Debt { get; set; }

    public string? AcctId { get; set; }

    public string? DebtType { get; set; }

    public string? Active { get; set; }

    public string? UseTariff { get; set; }

    public double? Pcent { get; set; }

    public int? BillEndDate { get; set; }
}
