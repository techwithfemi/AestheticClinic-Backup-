using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingVisitsForHmo
{
    public DateOnly Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public string CoyName { get; set; } = null!;

    public string? Referal { get; set; }

    public string RetainName { get; set; } = null!;

    public string? Ref { get; set; }

    public string RetainId { get; set; } = null!;

    public double? Debt { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ClientCatId { get; set; }

    public bool? IsBilled { get; set; }

    public string Remarks { get; set; } = null!;
}
