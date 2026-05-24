using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRecordsMonitorDetail
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public string FullName { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ConsultId { get; set; } = null!;
}
