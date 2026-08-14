using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hFluidChartCopy")]
public partial class hFluidChartCopy
{
    public long SNo { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string? intVol { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string? outVol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? outSod { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? outPot { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string empID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ntime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime nDate { get; set; }

    public long? conID { get; set; }

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

    [StringLength(100)]
    [Unicode(false)]
    public string? ChartTime { get; set; }
}
