using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("billingDetailsArchive")]
public partial class billingDetailsArchive
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public int? SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? billType { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

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
