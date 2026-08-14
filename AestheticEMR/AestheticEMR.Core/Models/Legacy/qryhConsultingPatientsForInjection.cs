using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatientsForInjection
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InjDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string pSurName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    public bool? attendedTo { get; set; }

    public bool? suppres { get; set; }

    [StringLength(2500)]
    public string InjName { get; set; } = null!;

    [StringLength(500)]
    public string drgCatName { get; set; } = null!;

    public int? numOfTimes { get; set; }

    public int? numTaken { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(2500)]
    public string? Dosage { get; set; }

    public long? conID { get; set; }

    public int? RowAge { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? injTime { get; set; }
}
