using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpOffence
{
    public int Sno { get; set; }

    public DateTime? OffDate { get; set; }

    public string? OffDetails { get; set; }

    public string? EmpId { get; set; }

    public string? OffCat { get; set; }

    public string? AssoQuery { get; set; }

    public string? IssuedBy { get; set; }
}
