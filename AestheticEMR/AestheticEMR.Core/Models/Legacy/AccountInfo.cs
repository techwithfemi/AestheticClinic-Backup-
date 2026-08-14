using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("AccountInfo")]
public partial class AccountInfo
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(50)]
    public string AcctNAme { get; set; } = null!;

    [StringLength(50)]
    public string? ParentID { get; set; }

    [StringLength(50)]
    public string AcctType { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string AcctStatus { get; set; } = null!;

    public double? Balance { get; set; }

    public bool? InitBal { get; set; }
}
