using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConReferal
{
    public string Psurname { get; set; } = null!;

    public string Pfirstname { get; set; } = null!;

    public string Consultid { get; set; } = null!;

    public bool? Attendedto { get; set; }
}
