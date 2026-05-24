using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConsultingMedRpt
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string MedReport { get; set; } = null!;

    public DateTime? EntryDate { get; set; }
}
