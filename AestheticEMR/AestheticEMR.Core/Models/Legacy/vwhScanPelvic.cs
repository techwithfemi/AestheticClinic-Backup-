using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhScanPelvic
{
    public long SNo { get; set; }

    public long ConID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(50)]
    public string? LabNo { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }

    [StringLength(100)]
    public string? AgeVal { get; set; }

    [StringLength(50)]
    public string? HospName { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? UterusCond { get; set; }

    [StringLength(50)]
    public string? UterusSize { get; set; }

    [StringLength(50)]
    public string? L { get; set; }

    [StringLength(50)]
    public string? T { get; set; }

    [StringLength(50)]
    public string? AP { get; set; }

    [StringLength(50)]
    public string? IntraUC { get; set; }

    [StringLength(50)]
    public string? ExtraUC { get; set; }

    [StringLength(50)]
    public string? GestAge { get; set; }

    [StringLength(50)]
    public string? FoetalCP { get; set; }

    [StringLength(50)]
    public string? Viability { get; set; }

    [StringLength(50)]
    public string? EDD { get; set; }

    [StringLength(50)]
    public string? BlightedSSM { get; set; }

    [StringLength(50)]
    public string? RetainedPdts { get; set; }

    [StringLength(50)]
    public string? InternalCOS { get; set; }

    [StringLength(50)]
    public string? Fibroid { get; set; }

    [StringLength(50)]
    public string? IntramuralSSM { get; set; }

    [StringLength(50)]
    public string? FibtoidLoc { get; set; }

    [StringLength(50)]
    public string? AnyCyst { get; set; }

    [StringLength(50)]
    public string? CystSize { get; set; }

    [StringLength(50)]
    public string? SizeRL { get; set; }

    [StringLength(50)]
    public string? AnyMatFollicle { get; set; }

    [StringLength(50)]
    public string? SideRLOvary { get; set; }

    [StringLength(50)]
    public string? AnyFreeColl { get; set; }

    [StringLength(50)]
    public string? CollWhere { get; set; }

    [StringLength(50)]
    public string? AnyHydroS { get; set; }

    [StringLength(50)]
    public string? SideRL { get; set; }

    [StringLength(500)]
    public string? Conclusion { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(101)]
    public string? DocName { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ImageID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? CRL { get; set; }
}
