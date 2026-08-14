using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockItemsRetail
{
    [StringLength(250)]
    public string ItemID { get; set; } = null!;

    [StringLength(250)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemCatID { get; set; } = null!;
}
