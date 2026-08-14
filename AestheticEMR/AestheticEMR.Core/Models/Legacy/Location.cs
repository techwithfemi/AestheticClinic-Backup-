using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class Location
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string LocName { get; set; } = null!;

    public int? currprd { get; set; }

    public int? curryr { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? compcode { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? company { get; set; }

    [StringLength(120)]
    [Unicode(false)]
    public string? mthnames { get; set; }

    public bool? genupdate { get; set; }

    public bool? paycalc { get; set; }

    [Column(TypeName = "numeric(5, 2)")]
    public decimal? emergency1 { get; set; }

    [Column(TypeName = "numeric(5, 2)")]
    public decimal? emergency2 { get; set; }

    [Column(TypeName = "numeric(5, 2)")]
    public decimal? lodging { get; set; }

    [Column(TypeName = "numeric(5, 2)")]
    public decimal? leaveall { get; set; }

    public bool? payleave { get; set; }

    [Column(TypeName = "numeric(3, 2)")]
    public decimal? Tax { get; set; }

    [Column(TypeName = "numeric(3, 2)")]
    public decimal? fmbl { get; set; }

    public bool? wopay { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? drive { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? mbackupdrive { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? mapdrivename { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? LocLogo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OrgID { get; set; }

    [Column(TypeName = "image")]
    public byte[]? imgLogo { get; set; }
}
