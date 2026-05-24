using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCapAndEnpenseGrouped
{
    public string Company { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string Mth { get; set; } = null!;

    public string? Yr { get; set; }

    public double? PhisIncome { get; set; }

    public double? NhisIncome { get; set; }

    public double? NhisExpense { get; set; }

    public double? NhisFfsIncome { get; set; }

    public double? PhisExpense { get; set; }

    public double? PhisFfsIncome { get; set; }
}
