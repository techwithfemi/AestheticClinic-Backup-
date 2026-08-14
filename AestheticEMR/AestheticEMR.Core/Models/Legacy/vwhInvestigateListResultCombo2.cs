using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateListResultCombo2
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ResultDate { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;
}
