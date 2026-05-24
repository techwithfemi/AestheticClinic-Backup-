using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInvestigateFinding
{
    public long? Id { get; set; }

    public DateTime? Invdate { get; set; }

    public string? ConsultId { get; set; }

    public string? Pno { get; set; }

    public string? SympItem { get; set; }

    public string? Result { get; set; }

    public string? Remarks { get; set; }

    public string ClientCat { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public bool? Attendedto { get; set; }

    public string? Capitated { get; set; }
}
