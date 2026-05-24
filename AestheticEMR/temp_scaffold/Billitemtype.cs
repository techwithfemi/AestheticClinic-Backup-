using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Billitemtype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public byte? Sort { get; set; }

    public bool? Detailed { get; set; }

    public bool? Subdivide { get; set; }
}
