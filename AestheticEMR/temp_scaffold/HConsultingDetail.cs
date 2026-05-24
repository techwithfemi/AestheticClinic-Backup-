using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConsultingDetail
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public DateTime? DtTime { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public decimal? Qty { get; set; }

    public string? PNo { get; set; }

    public string? Usage { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public string? Capitated { get; set; }

    public bool? Isdone { get; set; }

    public decimal? Price { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Cost { get; set; }

    public string? EmpId { get; set; }

    public string? Pending { get; set; }

    public string? Drgcode { get; set; }

    public string? Reversal { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }

    public bool? Suppres { get; set; }

    public string? TranId { get; set; }

    public long? ReversedPair { get; set; }

    public string? Remarks { get; set; }

    public bool? Reversed { get; set; }

    public string? NameInTariff { get; set; }
}
