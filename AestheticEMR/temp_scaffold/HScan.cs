using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScan
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string Result { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string Empid { get; set; } = null!;

    public bool? Attendedto { get; set; }

    public long? ConId { get; set; }
}
