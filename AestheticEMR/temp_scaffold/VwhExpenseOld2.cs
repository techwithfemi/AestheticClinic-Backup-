using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseOld2
{
    public long Sno { get; set; }

    public DateTime ExpDate { get; set; }

    public string VouchNo { get; set; } = null!;

    public string Paidby { get; set; } = null!;

    public string? Apprvdby { get; set; }

    public string ExpName { get; set; } = null!;

    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public decimal Amount { get; set; }

    public string? AmountInWord { get; set; }

    public string? Receivedby { get; set; }

    public string PayType { get; set; } = null!;

    public DateTime? ExpTime { get; set; }

    public string? Remarks { get; set; }

    public string? RetainCode { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? BankCode { get; set; }

    public DateTime? ChequeDate { get; set; }

    public string? EmpName { get; set; }
}
