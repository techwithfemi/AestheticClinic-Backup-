using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hDispensing")]
public partial class hDispensing
{
    [StringLength(50)]
    public string? ConsultID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(250)]
    public string usage { get; set; } = null!;

    [StringLength(50)]
    public string dispensedby { get; set; } = null!;
}
