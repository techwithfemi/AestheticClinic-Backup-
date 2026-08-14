using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCashBankList
{
    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(50)]
    public string AcctNAme { get; set; } = null!;

    [StringLength(50)]
    public string AcctType { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }
}
