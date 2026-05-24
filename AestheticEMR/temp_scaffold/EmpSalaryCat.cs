using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpSalaryCat
{
    public byte SalGrade { get; set; }

    public byte SalStep { get; set; }

    public decimal? Salary { get; set; }
}
