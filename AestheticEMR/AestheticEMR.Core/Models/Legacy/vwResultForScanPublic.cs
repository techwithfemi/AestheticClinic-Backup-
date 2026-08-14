using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwResultForScanPublic
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(50)]
    public string? AgeVal { get; set; }

    [StringLength(4000)]
    public string ResultMaster { get; set; } = null!;

    [StringLength(4000)]
    public string REMARKS { get; set; } = null!;

    [StringLength(116)]
    public string? EmpName { get; set; }

    public string? DESCRIPTION { get; set; }

    public string? RESULT { get; set; }

    [StringLength(350)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    public string? CLASS { get; set; }

    [StringLength(350)]
    public string? RANGE { get; set; }

    public long? conID { get; set; }
}
