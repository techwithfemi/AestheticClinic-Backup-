using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugQtyTest")]
public partial class DrugQtyTest
{
    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Qty { get; set; }
}
