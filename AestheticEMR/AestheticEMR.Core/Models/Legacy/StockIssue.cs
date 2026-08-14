using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("StockIssue")]
public partial class StockIssue
{
    [Key]
    public long IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? stockQtyIn { get; set; }

    [StringLength(50)]
    public string IssuedBy { get; set; } = null!;

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string? invType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(50)]
    public string? reverseID { get; set; }

    [StringLength(3)]
    public string? reversal { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevBal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DRGCode { get; set; }

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
}
