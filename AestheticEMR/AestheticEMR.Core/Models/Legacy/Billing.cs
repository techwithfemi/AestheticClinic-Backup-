using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Billing
{
    public long ID { get; set; }

    public DateOnly bDate { get; set; } = new DateOnly();

    //public DateTime? consultDate { get; set; }=

    public string billNO { get; set; }// same as consultID in attendance, primary key in billing

    public string pNo { get; set; } = null!;

    public string? clientID { get; set; } // same as coyID

    public decimal? DebtBF { get; set; } = 0;
    public decimal? AmountBilled { get; set; }
    public decimal? Discount { get; set; } = 0;

    public decimal? AmountPaid { get; set; }

    public double? Tax { get; set; } = 0;

    //public decimal? profFee { get; set; } = 0;

    //public decimal? AmtBF { get; set; } = 0;

    public string? AmountBilledInWord { get; set; }// for printing

    public string? BillingMonth { get; set; }// current month

    public int? BillingYear { get; set; }// current year

    //public string? diagnosis { get; set; }

    public bool? isPaid { get; set; } = false;

    public string? billType { get; set; } = string.Empty;

    //public string? InvNo { get; set; }

    public bool? isProcess { get; set; } = false;

    public DateTime? AdmDate { get; set; } = null;

    public DateTime? DischDate { get; set; } = null;

    public DateTime? timeVal { get; set; } = null;

    public string? ApprvCode { get; set; } = string.Empty;

    public bool? isSigned { get; set; } = false;

    //public decimal? AmountSigned { get; set; }=decimal.Zero;

    //public DateTime? EntryDate { get; set; }

    //public DateTime? EntryTime { get; set; }

    //public string? ClientName { get; set; }

    //public string? AppName { get; set; }

    public bool? isPost { get; set; } = false;
}