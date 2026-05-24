using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpDeduction
{
    public int Sno { get; set; }

    public string EmpId { get; set; } = null!;

    public DateTime DedDate { get; set; }

    public string DedCatId { get; set; } = null!;

    public decimal? Amount { get; set; }
}
