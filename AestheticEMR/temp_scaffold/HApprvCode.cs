using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HApprvCode
{
    public long Sno { get; set; }

    public DateTime? ApprvDate { get; set; }

    public string? ConsultId { get; set; }

    public string? ApprvCode { get; set; }

    public string? Pno { get; set; }

    public string? BillType { get; set; }

    public string? Remarks { get; set; }
}
