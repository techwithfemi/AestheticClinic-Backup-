using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class LabLabel
{
    public long IndexNo { get; set; }

    public string TagName { get; set; } = null!;

    public string LblDesc { get; set; } = null!;

    public string Range { get; set; } = null!;

    public string Units { get; set; } = null!;

    public long? SubClassId { get; set; }

    public string? TagNo { get; set; }

    public long Sno { get; set; }

    public string? TagValue { get; set; }

    public string? Sample { get; set; }

    public string? Reagent { get; set; }

    public string? Remarks { get; set; }

    public int HeaderIndexNo { get; set; }
}
