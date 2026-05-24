using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AestheticPhoto
{
    public int Id { get; set; }

    public int ConsultationId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string? Type { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ConsultId { get; set; }

    public string? Pno { get; set; }

    public virtual AestheticConsultation Consultation { get; set; } = null!;
}
