using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessBatch
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AttdDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(32)]
    public string? BatchVal { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }
}
