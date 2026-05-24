using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpAllowance
{
    public int Sno { get; set; }

    public string EmpId { get; set; } = null!;

    public DateTime AllwDate { get; set; }

    public string AllwCatId { get; set; } = null!;

    public decimal? Amount { get; set; }
}
