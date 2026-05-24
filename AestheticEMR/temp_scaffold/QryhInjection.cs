using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInjection
{
    public int Id { get; set; }

    public DateTime? DtdAte { get; set; }

    public string Pno { get; set; } = null!;

    public string InjName { get; set; } = null!;

    public string InjBy { get; set; } = null!;

    public DateTime? InjTime { get; set; }

    public bool? AttendedTo { get; set; }

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public long? ConId { get; set; }

    public long? IDno { get; set; }

    public string? DoneBy { get; set; }

    public int? NumTaken { get; set; }
}
