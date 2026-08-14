using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvResultForRptPublic3
{
    [StringLength(100)]
    public string pNo { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(101)]
    public string DocName { get; set; } = null!;

    [StringLength(16)]
    [Unicode(false)]
    public string HospName { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(4000)]
    public string ResultMaster { get; set; } = null!;

    [StringLength(4000)]
    public string REMARKS { get; set; } = null!;

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? DESCRIPTION { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? RESULT { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? CLASS { get; set; }

    [StringLength(350)]
    [Unicode(false)]
    public string? RANGE { get; set; }

    public long ID { get; set; }

    public int? SerialNo { get; set; }
}
