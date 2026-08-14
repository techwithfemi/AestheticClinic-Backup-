using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAcctOpenBal
{
    [StringLength(53)]
    public string? AcctID { get; set; }

    [StringLength(225)]
    public string AcctName { get; set; } = null!;

    [StringLength(50)]
    public string AcctType { get; set; } = null!;
}
