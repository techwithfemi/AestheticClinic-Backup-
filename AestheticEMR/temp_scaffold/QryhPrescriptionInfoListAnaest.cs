using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPrescriptionInfoListAnaest
{
    public long Id { get; set; }

    public string Fullname { get; set; } = null!;

    public DateTime CDate { get; set; }

    public string? Ctime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public long? ConId { get; set; }
}
