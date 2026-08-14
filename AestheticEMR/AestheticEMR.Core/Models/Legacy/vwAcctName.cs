using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAcctName
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(50)]
    public string AcctNAme { get; set; } = null!;

    [StringLength(50)]
    public string AcctType { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string AcctStatus { get; set; } = null!;
}
