using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HRetainership
{
    public DateTime? RetainDate { get; set; } = DateTime.Today;
    public string RetainId { get; set; } = null!;
    public string RetainCode { get; set; } = null!;
    public string RetainName { get; set; } = null!;
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
    public int? BillEndDate { get; set; }=31;
    public decimal? RegAmount { get; set; }=0;
    public decimal? ConAmount { get; set; }=0;
    public decimal? CardRenewAmount { get; set; }=0;
    public DateTime? EntryDate { get; set; }
    public DateTime? EntryTime { get; set; }
    public string? ClientName { get; set; }
    public string? AppName { get; set; }
}
