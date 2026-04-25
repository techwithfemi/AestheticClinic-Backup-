using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Billing
{
    public long ID { get; set; }

    public DateTime bDate { get; set; }

    public DateTime? consultDate { get; set; }

    public string billNO { get; set; } = null!;

    public string pNo { get; set; } = null!;

    public string? clientID { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? profFee { get; set; }

    public decimal? AmtBF { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public string? diagnosis { get; set; }

    public bool? isPaid { get; set; }

    public string? billType { get; set; }

    public string? InvNo { get; set; }

    public bool? isProcess { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? timeVal { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBF { get; set; }

    public string? ApprvCode { get; set; }

    public bool? isSigned { get; set; }

    public decimal? AmountSigned { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? isPost { get; set; }
}
