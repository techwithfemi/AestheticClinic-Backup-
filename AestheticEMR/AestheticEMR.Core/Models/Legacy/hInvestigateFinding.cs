using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class hInvestigateFinding
{
    [Key]
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(2000)]
    public string? sympItem { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    [StringLength(400)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string? clientcat { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(50)]
    public string? sympItemCat { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    public bool? attendedTobyLab { get; set; }

    public bool? Suppres { get; set; }

    public double? Price { get; set; }

    public double? SubTotal { get; set; }

    public double? Qty { get; set; }

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

    public bool? isPost { get; set; }

    public bool? suppresForAcct { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TranID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    public bool? Reversed { get; set; }

    public long? ReversedPair { get; set; }

    public long? LabItemSNo { get; set; }
}
