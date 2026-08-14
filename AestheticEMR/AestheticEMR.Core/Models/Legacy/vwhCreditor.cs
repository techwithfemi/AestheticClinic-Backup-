using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhCreditor
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    [StringLength(50)]
    public string? Company { get; set; }

    public double Amount { get; set; }

    [StringLength(200)]
    public string? Remarks { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? ClientNo { get; set; }

    [StringLength(53)]
    public string? AcctID { get; set; }

    public double? Balance { get; set; }
}
