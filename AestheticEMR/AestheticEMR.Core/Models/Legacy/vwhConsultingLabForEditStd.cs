using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingLabForEditStd
{
    public int ID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(2000)]
    public string? sympItem { get; set; }

    [StringLength(50)]
    public string? sympItemCat { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? attendedTo { get; set; }

    public bool? isLab { get; set; }

    public double? Price { get; set; }

    public double? Qty { get; set; }

    public double? SubTotal { get; set; }
}
