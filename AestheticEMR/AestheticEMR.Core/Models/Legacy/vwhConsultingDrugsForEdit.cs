using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingDrugsForEdit
{
    public long ID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(1000)]
    public string drgName { get; set; } = null!;

    [StringLength(1500)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(1000)]
    public string? usage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? ConID { get; set; }

    public double Price { get; set; }

    public double subTotal { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;
}
