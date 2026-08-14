using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhFluidChart
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Time { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    public bool? isDischarged { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? oral { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? intrav { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Intothers { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? intVol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? intSod { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? intPot { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Urine { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VomitusAspirate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? outOthers { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? outVol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? outSod { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? outPot { get; set; }

    public long SNo { get; set; }

    [StringLength(101)]
    public string? empname { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ChartTime { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? IntakeFluid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? IntakeFluidType { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? OutPutFluid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OutputFluidType { get; set; }
}
