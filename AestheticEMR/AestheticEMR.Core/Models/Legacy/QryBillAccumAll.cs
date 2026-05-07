using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class QryBillAccumAll
{
    public long Sno { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Service { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string PatNo { get; set; } = null!;

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string CatRemarks { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string? PCatId { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Capitated { get; set; }

    public string? ClientCatId { get; set; }

    public string? Referal { get; set; }

    public string? Ref { get; set; }

    public string? Dosage { get; set; }

    public bool? IsBilled { get; set; }

    public string? ClientCat { get; set; }

    public string BillTo { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? RevType { get; set; }

    public string? Category { get; set; }

    public string? TreatedBy { get; set; }

    public string? Dept { get; set; }

    public string? BillBy { get; set; }

    public string? Hmoref { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Reversed { get; set; }
}
