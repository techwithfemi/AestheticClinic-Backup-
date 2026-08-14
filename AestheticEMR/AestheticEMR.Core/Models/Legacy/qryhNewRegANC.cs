using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhNewRegANC
{
    [Column(TypeName = "datetime")]
    public DateTime RegDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string fullname { get; set; } = null!;

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? CardType { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? branch { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }
}
