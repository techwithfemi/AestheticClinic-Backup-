using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAdmission220311
{
    public long Sno { get; set; }

    public DateTime AdmDate { get; set; }

    public string PNo { get; set; } = null!;

    public string WardId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? ATime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public bool? IsDischarged { get; set; }
}
