using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("ProductId", "BatchNumber", Name = "IX_AppProductBatches_ProductId_BatchNumber", IsUnique = true)]
public partial class AppProductBatch
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [StringLength(100)]
    public string BatchNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public int QuantityReceived { get; set; }

    public int QuantityRemaining { get; set; }

    public bool IsRecalled { get; set; }

    public DateTime? RecalledOn { get; set; }

    [StringLength(500)]
    public string? RecallReason { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("AppProductBatches")]
    public virtual AppProduct Product { get; set; } = null!;
}
