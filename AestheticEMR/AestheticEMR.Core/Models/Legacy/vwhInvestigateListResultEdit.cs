using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultEdit
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(2000)]
    public string? LabItem { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(3)]
    public string Capitated { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public int ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(101)]
    public string? treatedBy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    public bool? attendedTo { get; set; }

    public bool? attendedTobyLab { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(400)]
    public string? InvRemarks { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }
}
