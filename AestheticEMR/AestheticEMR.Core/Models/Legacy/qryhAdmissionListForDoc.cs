using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAdmissionListForDoc
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string? Coyname { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? AdmitBy { get; set; }

    [StringLength(101)]
    public string? AdmitedBy { get; set; }

    public bool? isDischargedByDoc { get; set; }

    [StringLength(200)]
    public string? Reason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? aTime { get; set; }

    [StringLength(400)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? WardID { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }
}
