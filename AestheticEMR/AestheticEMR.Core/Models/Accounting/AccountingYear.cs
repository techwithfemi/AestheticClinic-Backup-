using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AccountingYear
{
    public string CoyID { get; set; } = null!;

    public string FinYear { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string PrdType { get; set; } = null!;

    public int PrdStart { get; set; }

    public bool Suppres { get; set; }

    /// <summary>
    /// nece to det startmonth of fin year diff from calendar year of Jan 1. to be set in db manually by sa
    /// </summary>
    public int diffVal { get; set; }

    public bool expired { get; set; }

    public bool isClose { get; set; }
}
