using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNewRegForToday
{
    public DateTime? RegDate { get; set; }

    public string Pno { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Sex { get; set; }

    public string? CardType { get; set; }

    public string? PCatId { get; set; }

    public string? CoyName { get; set; }

    public string? OldPno { get; set; }

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }

    public byte[]? PatPix { get; set; }

    public string? UserName { get; set; }
}
