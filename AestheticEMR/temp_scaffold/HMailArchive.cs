using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HMailArchive
{
    public long? Sno { get; set; }

    public DateTime? DateSent { get; set; }

    public string? Title { get; set; }

    public string? EmpIdfrom { get; set; }

    public string? EmpIdto { get; set; }

    public string? Message { get; set; }

    public bool? IsNew { get; set; }
}
