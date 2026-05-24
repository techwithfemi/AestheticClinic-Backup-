using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhEdd
{
    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateTime? Edd { get; set; }

    public int? NoOfDays { get; set; }
}
