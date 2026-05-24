using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpAllowanceCat
{
    public string AllwId { get; set; } = null!;

    public string AllwName { get; set; } = null!;

    public byte SalGrade { get; set; }

    public byte? SalStep { get; set; }

    public double? AllwRate { get; set; }
}
