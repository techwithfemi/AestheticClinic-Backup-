using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmpAllowance
{
    public int SNO { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime AllwDate { get; set; }

    [StringLength(50)]
    public string AllwCatID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Amount { get; set; }
}
