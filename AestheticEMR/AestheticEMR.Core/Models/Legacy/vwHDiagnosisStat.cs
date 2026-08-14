using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHDiagnosisStat
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? symptoms { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? complaints { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string Diagnosis { get; set; } = null!;

    [Unicode(false)]
    public string? diagnosis2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    public int? Age { get; set; }

    public int? AgeInMths { get; set; }

    public int? AgeInDays { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;
}
