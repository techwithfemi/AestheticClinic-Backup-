using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockItemsRetailB")]
public partial class StockItemsRetailB
{
    [StringLength(250)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;
}
