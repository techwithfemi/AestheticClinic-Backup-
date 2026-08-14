using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhPatientsImageURL
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ImageURL { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
