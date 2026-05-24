using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhAdmissionHist
{
    public DateTime AdmDate { get; set; }

    public DateTime? ATime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;
}
