using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockOpenBalDate")]
public partial class StockOpenBalDate
{
    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }
}
