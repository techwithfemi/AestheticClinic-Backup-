using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("billingdetails100919")]
public partial class billingdetails100919
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(250)]
    public string? Dosage { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? BillHead { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DRGCode { get; set; }

    public bool isPost { get; set; }

    public bool? isRct { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? treatedBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Dept { get; set; }

    public bool? isOLD { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
