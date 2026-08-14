using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBranchCode
{
    [StringLength(3)]
    public string BankCode { get; set; } = null!;

    [StringLength(7)]
    public string BranchCode { get; set; } = null!;
}
