using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingDetail
{
    public string? ConsultId { get; set; }

    public DateTime DtDate { get; set; }

    public string? PNo { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double Qty { get; set; }

    public string Usage { get; set; } = null!;

    public bool? AttendedTo { get; set; }
}
