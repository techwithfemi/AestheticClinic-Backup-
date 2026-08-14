using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DepreciationMaster")]
public partial class DepreciationMaster
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AQuireDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DisposalDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastDepr { get; set; }

    public int DurationInMths { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal GrossValue { get; set; }

    [Column(TypeName = "decimal(30, 13)")]
    public decimal? DeprAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AccumDeprAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SalvageValue { get; set; }

    public int? DeprCount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AccountID { get; set; }
}
