using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentTypesForAcct
{
    public long Sno { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string? AccountNo { get; set; }

    public decimal AmountPaid { get; set; }

    public string PayType { get; set; } = null!;

    public bool? IsPost { get; set; }

    public string Fullname { get; set; } = null!;

    public string PaymentFor { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public bool? Suppres { get; set; }

    public string? Coyname { get; set; }

    public string? AcctId { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Reversal { get; set; }
}
