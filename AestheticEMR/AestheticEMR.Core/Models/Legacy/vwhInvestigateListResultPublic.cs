using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultPublic
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

    public double? subTotal { get; set; }

    [StringLength(3)]
    public string Capitated { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    public int ID { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string referal { get; set; } = null!;

    [StringLength(50)]
    public string? clientcat { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string treatedBy { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string remarks { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? CoyCode { get; set; }

    public bool? attendedTo { get; set; }

    public bool? attendedTobyLab { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cTime { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }

    [StringLength(400)]
    public string? invRemarks { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Company { get; set; } = null!;
}
