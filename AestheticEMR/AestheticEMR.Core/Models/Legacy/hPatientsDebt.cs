using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsDebt")]
public partial class hPatientsDebt
{
    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }
}
