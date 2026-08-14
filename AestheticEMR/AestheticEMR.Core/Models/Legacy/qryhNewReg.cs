using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhNewReg
{
    [Column(TypeName = "datetime")]
    public DateTime? RegDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CardType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyNAme { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? empNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? branch { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? status { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? clientCatID { get; set; }

    [Column(TypeName = "image")]
    public byte[]? PatPix { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string HasPix { get; set; } = null!;

    [StringLength(101)]
    public string? username { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Expr1 { get; set; }

    [StringLength(50)]
    public string? clientCatID2 { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? officeAddress { get; set; }
}
