using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HMail
{
    public long Sno { get; set; }

    public DateTime? DateSent { get; set; }

    public string Title { get; set; } = null!;

    public string EmpIdfrom { get; set; } = null!;

    public string EmpIdto { get; set; } = null!;

    public string? Message { get; set; }

    public bool? IsNew { get; set; }
}
