using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInjAndDressing")]
public partial class hInjAndDressing
{
    public long ID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime InjDate { get; set; }

    public int? numOfTimes { get; set; }

    public int? numTaken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? injTime { get; set; }

    [StringLength(2500)]
    public string InjName { get; set; } = null!;

    [StringLength(500)]
    public string drgCatName { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(2500)]
    public string? Dosage { get; set; }

    public long? conID { get; set; }

    public bool? suppres { get; set; }
}
