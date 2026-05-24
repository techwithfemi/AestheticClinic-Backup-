using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPatListForService
{
    public string ConsultId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string Treatedby { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string? CoyName { get; set; }

    public string PNo { get; set; } = null!;
}
