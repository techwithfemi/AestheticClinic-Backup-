using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInjection
{
    public int Id { get; set; }

    public DateTime? DtdAte { get; set; }

    public string Pno { get; set; } = null!;

    public string InjName { get; set; } = null!;

    public string InjBy { get; set; } = null!;

    public DateTime? InjTime { get; set; }

    public string ConsultId { get; set; } = null!;

    public long? ConId { get; set; }

    public long? IDno { get; set; }

    public int? NumTaken { get; set; }
}
