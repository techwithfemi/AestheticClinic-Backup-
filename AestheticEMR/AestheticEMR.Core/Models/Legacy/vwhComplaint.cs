using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhComplaint
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(1100)]
    [Unicode(false)]
    public string? HomeAddress { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? symptoms { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? complaints { get; set; }

    [Unicode(false)]
    public string? diagnosis { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DOB { get; set; }

    public int Age { get; set; }

    public int AgeInMths { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Treatment { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string? DateAndTime { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string LabResult { get; set; } = null!;
}
