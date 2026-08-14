using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillAccumSumm
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
