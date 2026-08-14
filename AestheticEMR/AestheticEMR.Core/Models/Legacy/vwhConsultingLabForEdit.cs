using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingLabForEdit
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

    public double Qty { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public double Price { get; set; }

    public double subTotal { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }
}
