using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingServiceForEdit
{
    [StringLength(550)]
    public string Service { get; set; } = null!;

    [StringLength(100)]
    public string CATEGORY { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [StringLength(9)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [StringLength(50)]
    public string TYPE { get; set; } = null!;

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;
}
