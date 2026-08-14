using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessDetailsGrouped
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Subtotal { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BillDate { get; set; }

    [StringLength(50)]
    public string RetainID { get; set; } = null!;
}
