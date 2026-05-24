using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPoandApprvDate
{
    public string Poid { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public DateTime? OrderDateApprv { get; set; }
}
