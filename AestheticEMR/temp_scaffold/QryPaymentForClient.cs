using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryPaymentForClient
{
    public long Sno { get; set; }

    public DateTime ReceiptDate { get; set; }

    public DateTime? Time { get; set; }

    public string Company { get; set; } = null!;

    public string ReceiptNo { get; set; } = null!;

    public string InvNo { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string AmountPaidInWord { get; set; } = null!;

    public string? PaymentFor { get; set; }

    public string? Description { get; set; }

    public string PayType { get; set; } = null!;

    public string? ChequeNo { get; set; }

    public string? BankName { get; set; }

    public string? ReceivedBy { get; set; }

    public string? EmpId { get; set; }

    public string? BankCode { get; set; }

    public string? CoyCode { get; set; }

    public string? AcctId { get; set; }

    public bool? IsPost { get; set; }

    public bool? IsRev { get; set; }

    public bool? Suppres { get; set; }

    public string Remarks { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
