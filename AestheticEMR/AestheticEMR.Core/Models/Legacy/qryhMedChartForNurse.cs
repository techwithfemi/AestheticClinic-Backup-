using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhMedChartForNurse
{
    [Column(TypeName = "datetime")]
    public DateTime mDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime mTime { get; set; }

    public long IDNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string drgname { get; set; } = null!;

    public long numTaken { get; set; }

    [StringLength(101)]
    public string? Nurse { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    public bool? Suppres { get; set; }

    [StringLength(2500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    public long SNo { get; set; }
}
