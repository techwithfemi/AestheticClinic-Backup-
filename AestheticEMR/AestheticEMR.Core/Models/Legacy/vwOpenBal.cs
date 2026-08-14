using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwOpenBal
{
    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    public double OpenBal { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}
