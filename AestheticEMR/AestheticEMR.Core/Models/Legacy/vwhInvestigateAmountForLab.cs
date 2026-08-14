using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhInvestigateAmountForLab
{
    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? billtype { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(2000)]
    public string? drgName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountAccum { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? sympItemCat { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string ConID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    public long? LabItemSNo { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType2 { get; set; }
}
