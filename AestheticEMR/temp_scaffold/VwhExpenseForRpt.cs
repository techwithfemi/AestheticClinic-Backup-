using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseForRpt
{
    public DateTime Date { get; set; }

    public string? ExpenseBy { get; set; }

    public string VouchNo { get; set; } = null!;

    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? QtyFirstApprv { get; set; }

    public decimal? PriceFirstApprv { get; set; }

    public decimal? AmountFirstApprv { get; set; }

    public string? FirstApprvalby { get; set; }

    public decimal? QtyFinalApprv { get; set; }

    public decimal? PriceFinalApprv { get; set; }

    public decimal? AmountFinalApprv { get; set; }

    public string? FinalApprvalby { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? PaidBy { get; set; }

    public DateTime? DatePaid { get; set; }

    public DateTime? TimePaid { get; set; }

    public string ExpCat { get; set; } = null!;

    public string? AcctId { get; set; }

    public string? PersNo { get; set; }

    public string Dept { get; set; } = null!;

    public bool? FirstApprv { get; set; }
}
