using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhApprovCode
{
    public string Fullname { get; set; } = null!;

    public string? Coyname { get; set; }

    public DateTime? ApprvDate { get; set; }

    public string? ConsultId { get; set; }

    public string? ApprvCode { get; set; }

    public string RetainName { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? BillType { get; set; }

    public string? Remarks { get; set; }

    public string PNo { get; set; } = null!;

    public long Sno { get; set; }
}
