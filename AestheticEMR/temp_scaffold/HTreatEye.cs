using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HTreatEye
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime Tdate { get; set; }

    public DateTime Ttime { get; set; }

    public string? VisualAcuity { get; set; }

    public string? Aided { get; set; }

    public string? PrevSpecRx { get; set; }

    public string? SubjectiveRefraction { get; set; }

    public string? ExtExamOd { get; set; }

    public string? ExtExamOs { get; set; }

    public string? IntExamOd { get; set; }

    public string? IntExamOs { get; set; }

    public string? Remarks { get; set; }

    public string? ConId { get; set; }

    public string? Retino { get; set; }

    public string? Refraction { get; set; }

    public string? Ophthal { get; set; }

    public string? FsprescRe { get; set; }

    public string? FsprescLe { get; set; }

    public string? Tonometry { get; set; }

    public string? RemarksEye { get; set; }
}
