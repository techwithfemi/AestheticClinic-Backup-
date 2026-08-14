using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForInj
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    public bool? attendedToByPharm { get; set; }

    [StringLength(3000)]
    public string? prescription { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(101)]
    public string? treatedby { get; set; }

    public int? Age { get; set; }

    [StringLength(254)]
    public string? company { get; set; }

    [StringLength(50)]
    public string? CoyName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(3)]
    public string? Ref { get; set; }

    [StringLength(150)]
    public string? retainName { get; set; }
}
