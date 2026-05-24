using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryHeye
{
    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public DateTime Dob { get; set; }

    public int? Age { get; set; }

    public string Sex { get; set; } = null!;

    public int Sno { get; set; }

    public DateTime DDate { get; set; }

    public string PNo { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? DTime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public bool? IsDischarged { get; set; }
}
