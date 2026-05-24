using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppProductBatch
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public int QuantityReceived { get; set; }

    public int QuantityRemaining { get; set; }

    public bool IsRecalled { get; set; }

    public DateTime? RecalledOn { get; set; }

    public string? RecallReason { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AppProcedureProductUsage> AppProcedureProductUsages { get; set; } = new List<AppProcedureProductUsage>();

    public virtual AppProduct Product { get; set; } = null!;
}
