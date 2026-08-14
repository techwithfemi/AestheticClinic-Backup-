using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhDebtorsAndCash
{
    [StringLength(150)]
    public string Acctname { get; set; } = null!;

    [StringLength(53)]
    public string? AcctID { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string remarks { get; set; } = null!;
}
