using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppProcedureProductUsage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ProductBatchId { get; set; }

    public int ConsultationId { get; set; }

    public string ProcedureType { get; set; } = null!;

    public int QuantityUsed { get; set; }

    public DateTime UsedOn { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AestheticConsultation Consultation { get; set; } = null!;

    public virtual AppProduct Product { get; set; } = null!;

    public virtual AppProductBatch ProductBatch { get; set; } = null!;
}
