using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwRctForAcct
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? subTotal { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;
}
