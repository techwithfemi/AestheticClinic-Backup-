using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("CrosstabTest")]
public partial class CrosstabTest
{
    public int? SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string RevType { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;

    public double subTotal { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }
}
