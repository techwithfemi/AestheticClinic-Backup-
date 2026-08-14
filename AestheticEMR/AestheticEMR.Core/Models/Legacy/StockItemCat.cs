using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockItemCat")]
public partial class StockItemCat
{
    [StringLength(50)]
    public string drgCatCode { get; set; } = null!;

    [StringLength(250)]
    public string drgCatName { get; set; } = null!;

    [StringLength(150)]
    public string? catRemarks { get; set; }
}
