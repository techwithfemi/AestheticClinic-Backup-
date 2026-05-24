using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDressing
{
    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public DateTime DrDate { get; set; }

    public string DrName { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string Dressedby { get; set; } = null!;

    public bool AttendedTo { get; set; }

    public int Id { get; set; }
}
