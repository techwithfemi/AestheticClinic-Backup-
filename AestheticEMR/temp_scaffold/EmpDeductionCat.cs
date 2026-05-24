using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpDeductionCat
{
    public string DedId { get; set; } = null!;

    public string DedName { get; set; } = null!;

    public byte SalGrade { get; set; }

    public byte? SalStep { get; set; }

    public double? DedRate { get; set; }
}
