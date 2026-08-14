using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmpDeduction
{
    public int SNO { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime DedDate { get; set; }

    [StringLength(50)]
    public string DedCatID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Amount { get; set; }
}
