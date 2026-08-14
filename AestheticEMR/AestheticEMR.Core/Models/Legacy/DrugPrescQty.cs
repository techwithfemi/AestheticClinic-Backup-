using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugPrescQty")]
public partial class DrugPrescQty
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;
}
