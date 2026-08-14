using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class tranxAcct
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TranID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string AccountID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TranNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime TranDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CostCenterID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string TranCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string Period { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Prd2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyID { get; set; }
}
