using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultPublicXRAY
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string ConID { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(2000)]
    public string? LabItem { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(3)]
    public string Capitated { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public int ID { get; set; }

    [StringLength(50)]
    public string? clientcat { get; set; }

    public bool? attendedTo { get; set; }

    public bool? attendedTobyLab { get; set; }

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string Maturity { get; set; } = null!;
}
