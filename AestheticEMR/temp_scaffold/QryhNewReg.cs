using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNewReg
{
    public DateTime? RegDate { get; set; }

    public string Pno { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Sex { get; set; }

    public string? CardType { get; set; }

    public string? PCatId { get; set; }

    public string? OldpNo { get; set; }

    public string? CoyName { get; set; }

    public string? CoyType { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? PolicyType { get; set; }

    public string? PPhoneNo { get; set; }

    public string RetainName { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public byte[]? PatPix { get; set; }

    public string HasPix { get; set; } = null!;

    public string? Username { get; set; }

    public string? Expr1 { get; set; }

    public string? ClientCatId2 { get; set; }

    public string? RetainCode { get; set; }

    public string? OfficeAddress { get; set; }
}
