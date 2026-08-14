using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForLAbCombo
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CoyCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    public bool? suppres { get; set; }

    public bool? attendedTobyLab { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Ref { get; set; }

    public bool? attendedTo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;
}
