using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAdmissionHistory
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string WardID { get; set; } = null!;

    [StringLength(200)]
    public string Reason { get; set; } = null!;

    [StringLength(400)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? aTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public bool? isDischarged { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DOB { get; set; }

    public int? Expr1 { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? policyType { get; set; }
}
