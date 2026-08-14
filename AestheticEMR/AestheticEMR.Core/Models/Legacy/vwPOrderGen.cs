using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderGen
{
    [StringLength(50)]
    public string POID { get; set; } = null!;

    public int? Mth { get; set; }

    public int? Yr { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? IsApprv { get; set; }
}
