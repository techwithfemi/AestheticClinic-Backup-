using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInPatientDoctorsChart
{
    public string Pno { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime CDate { get; set; }

    public DateTime? CTime { get; set; }

    public string Prescription { get; set; } = null!;

    public int Sno { get; set; }
}
