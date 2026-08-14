using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHDelivery
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DelvDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Method { get; set; } = null!;

    [StringLength(101)]
    public string Doctor { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string NameOfNurse { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DelvOutcome { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string DocID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? TOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AS { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Wt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BL { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? HC { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Injections { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Immunizations { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DOB { get; set; }
}
