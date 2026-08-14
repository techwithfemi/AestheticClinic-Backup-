using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillInfoPeriodic
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? AmountGen { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;
}
