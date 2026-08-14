using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hAnteNatalReg")]
public partial class hAnteNatalReg
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string? ANCRegNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? pNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtFAttd { get; set; }

    [StringLength(50)]
    public string? ShoeSize { get; set; }

    [StringLength(500)]
    public string? AssessKahn { get; set; }

    [StringLength(500)]
    public string? AssessGenotype { get; set; }

    [StringLength(50)]
    public string? PelvicScan { get; set; }

    [StringLength(50)]
    public string? Rhesus { get; set; }

    [StringLength(500)]
    public string? SpecificTT { get; set; }

    [StringLength(500)]
    public string? PMHHeart { get; set; }

    [StringLength(500)]
    public string? PMHChest { get; set; }

    [StringLength(500)]
    public string? PMHKidney { get; set; }

    [StringLength(500)]
    public string? PMHBlood { get; set; }

    [StringLength(500)]
    public string? PMHOthers { get; set; }

    [StringLength(500)]
    public string? MHOnset { get; set; }

    [StringLength(500)]
    public string? MHDuration { get; set; }

    [StringLength(500)]
    public string? MHAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Cycle { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LMP { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtEdd { get; set; }

    [StringLength(500)]
    public string? HusbName { get; set; }

    [StringLength(500)]
    public string? HusbOccu { get; set; }

    [StringLength(500)]
    public string? HusbOffice { get; set; }

    [StringLength(50)]
    public string? HusbPhone { get; set; }

    [StringLength(500)]
    public string? FHMultipreg { get; set; }

    [StringLength(500)]
    public string? FHHyper { get; set; }

    [StringLength(500)]
    public string? FHDiabetes { get; set; }

    [StringLength(500)]
    public string? FHTuber { get; set; }

    [StringLength(500)]
    public string? FHHeart { get; set; }

    [StringLength(500)]
    public string? FHOthers { get; set; }

    [StringLength(500)]
    public string? PEGenCond { get; set; }

    [StringLength(500)]
    public string? PEResp { get; set; }

    [StringLength(500)]
    public string? PECardio { get; set; }

    [StringLength(500)]
    public string? PEAbdomen { get; set; }

    [StringLength(500)]
    public string? PEVagina { get; set; }

    [StringLength(500)]
    public string? PEOthers { get; set; }

    [StringLength(500)]
    public string? PEComments { get; set; }

    [StringLength(500)]
    public string? PEOedema { get; set; }

    [StringLength(500)]
    public string? PEAnaemia { get; set; }

    [StringLength(500)]
    public string? PESpleen { get; set; }

    [StringLength(500)]
    public string? PELiver { get; set; }

    [StringLength(500)]
    public string? PEExaminer { get; set; }

    [StringLength(500)]
    public string? PESpecial { get; set; }

    [StringLength(50)]
    public string? PEHt { get; set; }

    [StringLength(50)]
    public string? PEBP { get; set; }

    [StringLength(50)]
    public string? PEWt { get; set; }

    [StringLength(500)]
    public string? PEUrine { get; set; }

    [StringLength(500)]
    public string? PEBreast { get; set; }

    [StringLength(500)]
    public string? PEScan { get; set; }

    [StringLength(500)]
    public string? PEHB { get; set; }

    [StringLength(500)]
    public string? PEGenotype { get; set; }

    [StringLength(500)]
    public string? PEkahn { get; set; }

    [StringLength(500)]
    public string? PEABO { get; set; }

    [StringLength(500)]
    public string? PEChest { get; set; }

    [StringLength(500)]
    public string? PEWR { get; set; }

    [StringLength(500)]
    public string? HPPBleed { get; set; }

    [StringLength(500)]
    public string? HPPDisch { get; set; }

    [StringLength(500)]
    public string? HPPurinary { get; set; }

    [StringLength(500)]
    public string? HPPSwell { get; set; }

    [StringLength(500)]
    public string? HPPOthers { get; set; }

    public string? DelvSumm { get; set; }

    [StringLength(500)]
    public string? PrevPregParity { get; set; }

    [StringLength(500)]
    public string? PrevPregAbortion { get; set; }

    [StringLength(500)]
    public string? PrevPregnumAlive { get; set; }

    [StringLength(3)]
    public string? isDelvd { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? IsDelv { get; set; }
}
