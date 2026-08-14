using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateResultForLab
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public string? investigate { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(101)]
    public string? Empname { get; set; }

    public string? invResult { get; set; }

    [StringLength(4000)]
    public string? REMARKS { get; set; }

    [StringLength(4000)]
    public string? RESULT { get; set; }

    public double? Amount { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? billtype { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;
}
