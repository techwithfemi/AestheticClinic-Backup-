using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qrybillAccumHome
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Service { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(50)]
    public string PatNo { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string catRemarks { get; set; } = null!;

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    public double? debt { get; set; }

    [StringLength(500)]
    public string? Usage { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BillTo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    public bool? Reversed { get; set; }
}
