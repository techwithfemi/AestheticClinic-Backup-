using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingDetailsTemp
{
    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string? ConID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Drug { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? StdQty { get; set; }

    [StringLength(1000)]
    public string? StdPresc { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Subtotal { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(3)]
    public string? Capitated { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [StringLength(1000)]
    public string? Prescription { get; set; }
}
