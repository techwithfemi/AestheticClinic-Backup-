using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hconsultingdetailsTempArchive")]
public partial class hconsultingdetailsTempArchive
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

    [StringLength(255)]
    [Unicode(false)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(350)]
    public string? usage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    public double? Price { get; set; }

    public double? Subtotal { get; set; }

    public double? Cost { get; set; }
}
