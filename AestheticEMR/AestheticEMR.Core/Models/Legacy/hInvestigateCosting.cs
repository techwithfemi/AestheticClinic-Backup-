using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInvestigateCosting")]
public partial class hInvestigateCosting
{
    public long SNo { get; set; }

    public long LabItemSNo { get; set; }

    public long StockItemSNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Subtotal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
