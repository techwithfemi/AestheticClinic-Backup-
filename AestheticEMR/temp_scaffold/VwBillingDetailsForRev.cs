using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsForRev
{
    public long? Sno { get; set; }

    public DateTime Date { get; set; }

    public DateTime? BillDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? PCatId { get; set; }

    public string? RevType { get; set; }

    public string? BillNo { get; set; }

    public string? Service { get; set; }

    public decimal SubTotal { get; set; }

    public string? ClientCatId { get; set; }

    public string? RetainId { get; set; }

    public string? RetainCode { get; set; }

    public string? BillBy { get; set; }

    public string? TreatedBy { get; set; }

    public string? BillType { get; set; }

    public string Consultant { get; set; } = null!;

    public string? ClientType { get; set; }

    public string PNo { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string PatCode { get; set; } = null!;

    public string? PatCode2 { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? CoyCode { get; set; }

    public string Processed { get; set; } = null!;

    public string Locked { get; set; } = null!;

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? InvNo { get; set; }

    public bool? IsProcess { get; set; }
}
