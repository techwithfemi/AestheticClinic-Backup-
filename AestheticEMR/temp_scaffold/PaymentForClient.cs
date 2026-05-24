using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentForClient
{
    public long Sno { get; set; }

    public DateTime ReceiptDate { get; set; }

    public DateTime? RTime { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string InvNo { get; set; } = null!;

    public string? PaymentFor { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string AmountInWord { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string PayType { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoyCode { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ValueDate { get; set; }

    public string? BankCode { get; set; }

    public DateTime? ChequeDate { get; set; }

    public bool? IsPost { get; set; }

    public bool? IsRev { get; set; }

    public DateTime? EntryDate { get; set; }

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
