using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Shop;

public class ProductBatchVM
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int QuantityReceived { get; set; }
    public int QuantityRemaining { get; set; }
    public bool IsRecalled { get; set; }
    public DateTime? RecalledOn { get; set; }
    public string? RecallReason { get; set; }
}

public class ProductBatchEditVM
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string BatchNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    [Range(1, int.MaxValue)]
    public int QuantityReceived { get; set; }
}

public class RecallBatchVM
{
    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = null!;
}

public class ProcedureProductUsageVM
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int ProductBatchId { get; set; }
    public string? BatchNumber { get; set; }
    public int ConsultationId { get; set; }
    public string? ProcedureType { get; set; }
    public int QuantityUsed { get; set; }
    public DateTime UsedOn { get; set; }
    public string? Notes { get; set; }
}

public class ProcedureProductUsageEditVM
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int ProductBatchId { get; set; }

    [Range(1, int.MaxValue)]
    public int ConsultationId { get; set; }

    [StringLength(100)]
    public string? ProcedureType { get; set; }

    [Range(1, int.MaxValue)]
    public int QuantityUsed { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}