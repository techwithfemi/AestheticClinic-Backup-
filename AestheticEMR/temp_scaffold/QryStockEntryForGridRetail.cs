using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockEntryForGridRetail
{
    public int EntryId { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public int Qty { get; set; }

    public string? Comments { get; set; }

    public string? ReceivedBy { get; set; }

    public string EmpId { get; set; } = null!;
}
