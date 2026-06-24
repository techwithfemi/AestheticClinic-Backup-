using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class LedgerCategory
{
    public short Serial { get; set; }

    public string LedgerCode { get; set; } = null!;

    public string? LedgerCodeVal { get; set; }

    public string Ledger { get; set; } = null!;

    public string Remarks { get; set; } = null!;
}
