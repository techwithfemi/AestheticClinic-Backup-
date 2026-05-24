using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConsultingItem
{
    public long Sno { get; set; }

    public DateTime Cdate { get; set; }

    public DateTime Ctime { get; set; }

    public long ConId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public bool? IsApprv { get; set; }

    public bool? AttendedTo { get; set; }
}
