using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingDrugsForEditStd
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double Qty { get; set; }

    public string? Usage { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? IsDrug { get; set; }

    public double? Price { get; set; }

    public double? SubTotal { get; set; }
}
