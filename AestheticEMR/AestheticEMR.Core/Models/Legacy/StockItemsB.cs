using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockItemsB")]
public partial class StockItemsB
{
    [StringLength(250)]
    public string ItemID { get; set; } = null!;

    [StringLength(250)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemCatID { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string QuantityPerUnit { get; set; } = null!;

    public int UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public int ReorderLevel { get; set; }

    public int Discontinued { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string brand { get; set; } = null!;
}
