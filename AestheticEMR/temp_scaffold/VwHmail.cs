using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHmail
{
    public long Sno { get; set; }

    public string? SentFrom { get; set; }

    public string? Message { get; set; }

    public bool? IsNew { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DateSent { get; set; }

    public string Title { get; set; } = null!;

    public string EmpIdfrom { get; set; } = null!;

    public string EmpIdto { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? SentTo { get; set; }
}
