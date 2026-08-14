using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsVal")]
public partial class hPatientsVal
{
    [StringLength(50)]
    public string pno { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal pnoVal { get; set; }
}
