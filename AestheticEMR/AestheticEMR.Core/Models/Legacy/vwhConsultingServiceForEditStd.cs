using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingServiceForEditStd
{
    [StringLength(550)]
    public string Service { get; set; } = null!;

    [StringLength(50)]
    public string CATEGORY { get; set; } = null!;

    public double? Price { get; set; }

    public double Qty { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(350)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    public bool? isServ { get; set; }
}
