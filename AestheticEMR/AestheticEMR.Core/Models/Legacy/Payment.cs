using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Payment
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string billNO { get; set; } = null!;

    public string pNO { get; set; } = null!;

    public string? clinicID { get; set; }

    public string paymentFor { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string AmountInWord { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string payType { get; set; } = null!;

    public DateTime? rTime { get; set; }

    public string? Remarks { get; set; }

    public string? RetainCode { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? BankCode { get; set; }

    public DateTime? ChequeDate { get; set; }

    public bool? isPost { get; set; }

    public long SNo { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? suppres { get; set; }
}
