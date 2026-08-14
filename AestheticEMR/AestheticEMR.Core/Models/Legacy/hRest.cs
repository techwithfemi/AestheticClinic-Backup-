using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hRest
{
    [Column(TypeName = "smalldatetime")]
    public DateTime rDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime rTime { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string CertifiedBy { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime? moveDate { get; set; }

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(50)]
    public string? Age { get; set; }

    [StringLength(50)]
    public string? Reason { get; set; }

    [StringLength(150)]
    public string? Remarks { get; set; }
}
