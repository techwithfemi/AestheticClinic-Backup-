using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingVisits2
{
    [StringLength(406)]
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

    [StringLength(50)]
    public string Clinic { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? clientCatID { get; set; }

    [StringLength(100)]
    public string pNo { get; set; } = null!;

    [StringLength(100)]
    public string? EnrolleeNo { get; set; }
}
