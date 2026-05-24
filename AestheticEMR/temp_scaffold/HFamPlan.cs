using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HFamPlan
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public DateTime? PlanDate { get; set; }

    public DateTime? PlanTime { get; set; }

    public string Education { get; set; } = null!;

    public string? NoPregToDate { get; set; }

    public string? NoOfChildBornAlive { get; set; }

    public string? NoOfChildStillAlive { get; set; }

    public string? NoOfMiscar { get; set; }

    public string? MthYrLastPregEnded { get; set; }

    public string? ResOfLastPreg { get; set; }

    public string? MoreChild { get; set; }

    public DateTime? DtLastMenstru { get; set; }

    public string? Smoker { get; set; }

    public string? MedHist { get; set; }

    public DateTime? DtPreg1 { get; set; }

    public DateTime? DtPreg2 { get; set; }

    public DateTime? DtPreg3 { get; set; }
}
