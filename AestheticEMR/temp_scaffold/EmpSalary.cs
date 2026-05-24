using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpSalary
{
    public DateTime SalDate { get; set; }

    public string EmpId { get; set; } = null!;

    public string? AllwType { get; set; }

    public string? DedType { get; set; }

    public byte? SalGrade { get; set; }

    public byte? SalStep { get; set; }
}
