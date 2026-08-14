using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillProcessReverseList
{
    [StringLength(50)]
    public string? CoyCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    public bool? isOLd { get; set; }

    [StringLength(50)]
    public string? BatchNo2 { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;
}
