using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hMedChartForNurse")]
public partial class hMedChartForNurse
{
    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    public long numTaken { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string drgname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime mDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime mTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string givenby { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    public long IDNo { get; set; }

    public long? conID { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(2500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    public long SNo { get; set; }
}
