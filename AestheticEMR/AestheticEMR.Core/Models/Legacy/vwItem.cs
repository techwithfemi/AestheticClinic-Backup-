using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwItem
{
    public long SNO { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string CatCode { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? AcctID { get; set; }

    [StringLength(7)]
    public string ItemCode { get; set; } = null!;

    [StringLength(255)]
    public string ItemName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(255)]
    public string BName { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;
}
