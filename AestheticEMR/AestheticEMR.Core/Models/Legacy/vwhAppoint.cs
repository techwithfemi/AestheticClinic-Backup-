using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhAppoint
{
    [Column(TypeName = "datetime")]
    public DateTime? entryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? entryTime { get; set; }

    [StringLength(355)]
    public string Patient { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinicType { get; set; }

    [StringLength(101)]
    public string? GivenBy { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    public int? Age { get; set; }

    [StringLength(150)]
    public string? Company { get; set; }

    public bool? attendedTo { get; set; }
}
