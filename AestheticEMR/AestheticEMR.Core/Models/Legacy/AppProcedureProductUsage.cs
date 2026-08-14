using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("ConsultationId", Name = "IX_AppProcedureProductUsages_ConsultationId")]
[Index("ProductBatchId", Name = "IX_AppProcedureProductUsages_ProductBatchId")]
[Index("ProductId", Name = "IX_AppProcedureProductUsages_ProductId")]
public partial class AppProcedureProductUsage
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ProductBatchId { get; set; }

    public int ConsultationId { get; set; }

    [StringLength(100)]
    public string ProcedureType { get; set; } = null!;

    public int QuantityUsed { get; set; }

    public DateTime UsedOn { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("ConsultationId")]
    [InverseProperty("AppProcedureProductUsages")]
    public virtual AestheticConsultation Consultation { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("AppProcedureProductUsages")]
    public virtual AppProduct Product { get; set; } = null!;

    [ForeignKey("ProductBatchId")]
    [InverseProperty("AppProcedureProductUsages")]
    public virtual AppProductBatch ProductBatch { get; set; } = null!;
}
