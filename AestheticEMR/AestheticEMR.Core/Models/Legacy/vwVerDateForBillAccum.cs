using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwVerDateForBillAccum
{
    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string? conID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }
}
