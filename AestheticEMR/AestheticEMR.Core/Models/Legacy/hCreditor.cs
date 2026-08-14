using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hCreditor
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    [StringLength(50)]
    public string? ClientNo { get; set; }

    [StringLength(50)]
    public string? Pno { get; set; }

    public double? Balance { get; set; }

    public double Amount { get; set; }

    public bool? isPaid { get; set; }

    [StringLength(200)]
    public string? Remarks { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }
}
