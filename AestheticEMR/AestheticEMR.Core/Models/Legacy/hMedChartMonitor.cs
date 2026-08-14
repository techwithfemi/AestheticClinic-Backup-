using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hMedChartMonitor")]
public partial class hMedChartMonitor
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime mDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClientCat { get; set; } = null!;

    public short numOfTimes { get; set; }

    public short numTaken { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(500)]
    public string drgCatNAme { get; set; } = null!;

    [StringLength(1500)]
    [Unicode(false)]
    public string dosage { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime mTime { get; set; }

    public bool? attendedTo { get; set; }

    public long? conID { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(2500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
