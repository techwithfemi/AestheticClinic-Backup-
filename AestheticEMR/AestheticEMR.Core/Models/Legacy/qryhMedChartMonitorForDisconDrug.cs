using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhMedChartMonitorForDisconDrug
{
    public long? conID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime mDate { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime mTime { get; set; }

    public long ID { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string DrugFullyGiven { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Discontinued { get; set; } = null!;

    [StringLength(2500)]
    [Unicode(false)]
    public string? Reason { get; set; }
}
