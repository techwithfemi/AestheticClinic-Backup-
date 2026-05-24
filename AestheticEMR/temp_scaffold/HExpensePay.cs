using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpensePay
{
    public long Sno { get; set; }

    public string? VouchNo { get; set; }

    public DateTime? PayDate { get; set; }

    public DateTime? PayTime { get; set; }

    public decimal? Amount { get; set; }

    public string? PaidBy { get; set; }

    public string? Description { get; set; }

    public string? Recipient { get; set; }

    public string? PayType { get; set; }

    public string? Remarks { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? BankCode { get; set; }

    public DateTime? ChequeDate { get; set; }

    public bool? IsPost { get; set; }

    public string? AcctNoDebit { get; set; }
}
