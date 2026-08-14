using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhComplaintsForCOVID
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

    [StringLength(3000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }
}
