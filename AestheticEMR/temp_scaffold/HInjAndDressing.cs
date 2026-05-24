using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInjAndDressing
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public DateTime InjDate { get; set; }

    public int? NumOfTimes { get; set; }

    public int? NumTaken { get; set; }

    public DateTime? InjTime { get; set; }

    public string InjName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Dosage { get; set; }

    public long? ConId { get; set; }

    public bool? Suppres { get; set; }
}
