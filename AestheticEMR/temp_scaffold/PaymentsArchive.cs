using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentsArchive
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClinicId { get; set; }

    public string PaymentFor { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string AmountInWord { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string PayType { get; set; } = null!;

    public DateTime? RTime { get; set; }

    public string? Remarks { get; set; }

    public bool? IsPost { get; set; }
}
