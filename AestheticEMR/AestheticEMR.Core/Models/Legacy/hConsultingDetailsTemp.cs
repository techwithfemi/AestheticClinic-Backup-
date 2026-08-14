using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hConsultingDetailsTemp")]
public partial class hConsultingDetailsTemp
{
    public long ID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtTime { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? drgCatName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(1000)]
    public string? usage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? ConID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    public bool? isdone { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Subtotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Reversal { get; set; }
}
