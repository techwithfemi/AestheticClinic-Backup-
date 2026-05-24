using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EbonyHConsulting
{
    public long ConId { get; set; }

    public DateTime CDate { get; set; }

    public DateTime? Ctime { get; set; }

    public string PNo { get; set; } = null!;

    public string? Treatment { get; set; }

    public string? Complaints { get; set; }

    public string? Diagnosis { get; set; }

    public string? Hpc { get; set; }

    public string? Pmh { get; set; }

    public string? DrugHx { get; set; }

    public DateTime? EntryTime { get; set; }
}
