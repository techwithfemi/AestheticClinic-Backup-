using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Bank
{
    public long SNO { get; set; }

    [StringLength(3)]
    public string BankCode { get; set; } = null!;

    [StringLength(7)]
    public string BranchCode { get; set; } = null!;

    [StringLength(255)]
    public string? BankName { get; set; }

    [StringLength(255)]
    public string? Branch { get; set; }

    [StringLength(250)]
    public string? Location { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(10)]
    public string AcctID { get; set; } = null!;
}
