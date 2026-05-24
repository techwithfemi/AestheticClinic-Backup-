using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingDetailsTemp
{
    public DateTime DtDate { get; set; }

    public string? ConId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Drug { get; set; } = null!;

    public string? Category { get; set; }

    public decimal? StdQty { get; set; }

    public string? StdPresc { get; set; }

    public decimal? Subtotal { get; set; }

    public bool? AttendedTo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Capitated { get; set; }

    public decimal? Qty { get; set; }

    public string? Prescription { get; set; }
}
