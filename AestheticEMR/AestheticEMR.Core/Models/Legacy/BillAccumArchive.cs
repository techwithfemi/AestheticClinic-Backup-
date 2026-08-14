using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BillAccumArchive")]
public partial class BillAccumArchive
{
    public int SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    public bool? isBilled { get; set; }

    [StringLength(500)]
    public string? Usage { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    public double? SubTotalSys { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillBy { get; set; }
}
