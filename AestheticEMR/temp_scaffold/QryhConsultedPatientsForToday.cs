using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultedPatientsForToday
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string TreatedBy { get; set; } = null!;

    public string ConsultId { get; set; } = null!;
}
