using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhCapitation
{
    [StringLength(50)]
    public string MthName { get; set; } = null!;

    [StringLength(2)]
    public string Mth { get; set; } = null!;

    [StringLength(4)]
    public string? Yr { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    public double Amount { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    public long? SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(50)]
    public string CoyName { get; set; } = null!;
}
