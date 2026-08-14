using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseHeader
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    [StringLength(255)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    public string? PersNo { get; set; }

    public bool? isDone { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(225)]
    [Unicode(false)]
    public string CatType { get; set; } = null!;
}
