using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class BankBalance
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BTime { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    public double Balance { get; set; }
}
