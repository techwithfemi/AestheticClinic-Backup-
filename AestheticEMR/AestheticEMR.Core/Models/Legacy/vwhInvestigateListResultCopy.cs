using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultCopy
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

    public int subTotal { get; set; }

    [StringLength(3)]
    public string Capitated { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public int ID { get; set; }

    [StringLength(3)]
    public string? referal { get; set; }

    [StringLength(50)]
    public string? clientcat { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(101)]
    public string? treatedBy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(150)]
    public string? Company { get; set; }

    public bool? attendedTo { get; set; }

    public bool? attendedTobyLab { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }
}
