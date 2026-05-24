using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDispDetailsForNurse1
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Qty { get; set; }

    public string? PNo { get; set; }

    public string? Usage { get; set; }

    public bool? AttendedTo { get; set; }

    public string Fullname { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public DateTime MDate { get; set; }

    public DateTime? MTime { get; set; }
}
