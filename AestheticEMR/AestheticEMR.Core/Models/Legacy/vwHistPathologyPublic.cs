using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHistPathologyPublic
{
    [StringLength(50)]
    public string LabNo { get; set; } = null!;

    [StringLength(50)]
    public string? PathNo { get; set; }

    [StringLength(50)]
    public string? Clinician { get; set; }

    [StringLength(50)]
    public string? EtnicGroup { get; set; }

    [StringLength(50)]
    public string? Ward { get; set; }

    [StringLength(550)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(1000)]
    public string? Test { get; set; }

    [StringLength(250)]
    public string? Maternal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDate { get; set; }

    public string Report { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string DocName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string HospName { get; set; } = null!;

    [StringLength(50)]
    public string Expr1 { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(4000)]
    public string ResultMaster { get; set; } = null!;

    [StringLength(4000)]
    public string REMARKS { get; set; } = null!;

    [StringLength(116)]
    public string? EmpName { get; set; }

    [StringLength(350)]
    public string? DESCRIPTION { get; set; }

    [StringLength(350)]
    public string? RESULT { get; set; }

    [StringLength(350)]
    public string? DESC2 { get; set; }

    [StringLength(350)]
    public string? SAMPLE { get; set; }

    [StringLength(350)]
    public string? CLASS { get; set; }

    [StringLength(350)]
    public string? RANGE { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }
}
