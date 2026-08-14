using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwWidalX
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? DocName { get; set; }

    [StringLength(100)]
    public string? HospName { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(4000)]
    public string ResultMaster { get; set; } = null!;

    [StringLength(1000)]
    public string REMARKS { get; set; } = null!;

    [StringLength(101)]
    public string EmpName { get; set; } = null!;

    [StringLength(50)]
    public string? DESCRIPTION { get; set; }

    [StringLength(50)]
    public string? RESULT1 { get; set; }

    [StringLength(50)]
    public string? DESC2 { get; set; }

    [StringLength(50)]
    public string? SAMPLE { get; set; }

    [StringLength(50)]
    public string? CLASS { get; set; }

    [StringLength(50)]
    public string? RANGE { get; set; }

    [StringLength(50)]
    public string? RESULT12 { get; set; }
}
