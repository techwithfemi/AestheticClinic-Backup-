using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hConsultingServiceDetail
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ServDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ServTime { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ServName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Subtotal { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Description { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? conID { get; set; }

    public bool? isdone { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EmpID { get; set; }
}
