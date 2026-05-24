using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConsultingExam
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ConId { get; set; } = null!;

    public string? ExamType { get; set; }

    public string LblDesc { get; set; } = null!;

    public string LblValue { get; set; } = null!;
}
