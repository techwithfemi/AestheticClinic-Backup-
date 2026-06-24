using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Auditrail
{
    public long ID { get; set; }

    public string TranCode { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserAction { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    public DateTime ActionTime { get; set; }

    public string? Remarks { get; set; }

    public string? Src { get; set; }
}
