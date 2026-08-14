using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAcctID
{
    [StringLength(50)]
    public string AcctType { get; set; } = null!;

    public int? IdMax { get; set; }
}
