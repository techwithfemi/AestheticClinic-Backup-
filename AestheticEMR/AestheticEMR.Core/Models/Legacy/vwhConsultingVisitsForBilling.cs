using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingVisitsForBilling
{
    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Client { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? debt { get; set; }

    public bool? isBilled { get; set; }

    [StringLength(50)]
    public string Clinic { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
