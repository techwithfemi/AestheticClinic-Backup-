using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhNewRegForToday
{
    [Column(TypeName = "datetime")]
    public DateTime? RegDate { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(301)]
    public string? fullname { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? CardType { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [Column(TypeName = "image")]
    public byte[]? PatPix { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }
}
