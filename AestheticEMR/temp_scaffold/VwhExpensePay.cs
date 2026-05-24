using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpensePay
{
    public long Sno { get; set; }

    public string? VouchNo { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Time { get; set; }

    public decimal? AmountToPay { get; set; }

    public decimal? Amount { get; set; }

    public string? RefNo { get; set; }

    public string? Description { get; set; }

    public string PaidBy { get; set; } = null!;

    public string? Recipient { get; set; }

    public bool? IsPost { get; set; }

    public string Posted { get; set; } = null!;

    public string? ApprvBy { get; set; }

    public long ItemCode { get; set; }

    public decimal AmountApprved { get; set; }

    public long? ExpIdSno { get; set; }
}
