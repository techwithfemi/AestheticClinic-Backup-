using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("drugsPriceHistory")]
public partial class drugsPriceHistory
{
    public long SNO { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SellingPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string drgNAme { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }
}
