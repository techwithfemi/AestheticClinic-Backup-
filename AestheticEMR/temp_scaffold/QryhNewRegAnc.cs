using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNewRegAnc
{
    public DateTime RegDate { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string? CardType { get; set; }

    public string PCatId { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string? CoyName { get; set; }

    public string? CoyType { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? PolicyType { get; set; }
}
